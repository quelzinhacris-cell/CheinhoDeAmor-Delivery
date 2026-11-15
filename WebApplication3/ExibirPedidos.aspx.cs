using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;

namespace WebApplication3
{
    public partial class ExibirPedidos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["autenticado"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string usuarioLogado = Session["autenticado"].ToString();

                if (usuarioLogado.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    CarregarTodosPedidos();
                }
                else
                {
                    CarregarPedidosClienteLogado(usuarioLogado);
                }
            }
        }

        private void CarregarTodosPedidos()
        {
            string strConexao = ConfigurationManager.ConnectionStrings["SupabaseConnection"].ConnectionString;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(strConexao))
                {
                    conn.Open();
                    string sql = @"
                        SELECT p.id_pedido, p.cpf, p.data_pedido, p.valor_total AS subtotal, p.status,
                               STRING_AGG(pr.nome || ' (x' || ip.quantidade || ')', ', ') AS itens_resumo
                        FROM pedidos p
                        LEFT JOIN itens_pedido ip ON p.id_pedido = ip.pedido_id
                        LEFT JOIN produtos pr ON ip.produto_id = pr.id_produto
                        GROUP BY p.id_pedido, p.cpf, p.data_pedido, p.valor_total, p.status
                        ORDER BY p.data_pedido DESC";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            gridPedidos.DataSource = dt;
                            gridPedidos.DataBind();
                            gridPedidos.Visible = (dt.Rows.Count > 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.ForeColor = System.Drawing.Color.Red;
                Msg.Text = "Erro ao carregar pedidos: " + ex.Message;
            }
        }

        private void CarregarPedidosClienteLogado(string login)
        {
            string strConexao = ConfigurationManager.ConnectionStrings["SupabaseConnection"].ConnectionString;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(strConexao))
                {
                    conn.Open();

                    string sql = @"
                        SELECT p.id_pedido, p.cpf, p.data_pedido, p.valor_total AS subtotal, p.status,
                               STRING_AGG(pr.nome || ' (x' || ip.quantidade || ')', ', ') AS itens_resumo
                        FROM pedidos p
                        INNER JOIN clientes c ON p.cpf = c.cpf
                        LEFT JOIN itens_pedido ip ON p.id_pedido = ip.pedido_id
                        LEFT JOIN produtos pr ON ip.produto_id = pr.id_produto
                        WHERE LOWER(TRIM(c.email)) = LOWER(TRIM(@login)) 
                           OR TRIM(c.cpf) = TRIM(@login)
                        GROUP BY p.id_pedido, p.cpf, p.data_pedido, p.valor_total, p.status
                        ORDER BY p.data_pedido DESC";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", login);

                        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gridPedidos.DataSource = dt;
                                gridPedidos.DataBind();
                                gridPedidos.Visible = true;
                                Msg.Text = "";
                            }
                            else
                            {
                                gridPedidos.Visible = false;
                                Msg.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
                                Msg.Text = "Nenhum pedido encontrado para a sua conta.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.ForeColor = System.Drawing.Color.Red;
                Msg.Text = "Erro ao buscar pedidos: " + ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string busca = txtBuscar.Text.Trim();
            string usuarioLogado = Session["autenticado"].ToString();
            bool isAdmin = usuarioLogado.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            if (string.IsNullOrEmpty(busca))
            {
                if (isAdmin)
                    CarregarTodosPedidos();
                else
                    CarregarPedidosClienteLogado(usuarioLogado);
                return;
            }

            string strConexao = ConfigurationManager.ConnectionStrings["SupabaseConnection"].ConnectionString;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(strConexao))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        string sql = "";

                        if (isAdmin)
                        {
                            // Busca global para o Administrador (incluindo CPF, E-mail, Nome do Cliente, ID e Nome do Produto)
                            sql = @"
                                SELECT p.id_pedido, p.cpf, p.data_pedido, p.valor_total AS subtotal, p.status,
                                       STRING_AGG(pr.nome || ' (x' || ip.quantidade || ')', ', ') AS itens_resumo
                                FROM pedidos p
                                LEFT JOIN clientes c ON p.cpf = c.cpf
                                LEFT JOIN itens_pedido ip ON p.id_pedido = ip.pedido_id
                                LEFT JOIN produtos pr ON ip.produto_id = pr.id_produto
                                WHERE (TRIM(p.cpf) ILIKE @busca 
                                   OR LOWER(TRIM(c.email)) ILIKE LOWER(TRIM(@busca))
                                   OR c.nome ILIKE @busca
                                   OR pr.nome ILIKE @busca)";

                            if (int.TryParse(busca, out int idPedidoBusca))
                            {
                                sql += " OR p.id_pedido = @idPedido";
                                cmd.Parameters.AddWithValue("@idPedido", idPedidoBusca);
                            }
                            cmd.Parameters.AddWithValue("@busca", "%" + busca + "%");
                        }
                        else
                        {
                            // Busca restrita estritamente ao cliente logado
                            sql = @"
                                SELECT p.id_pedido, p.cpf, p.data_pedido, p.valor_total AS subtotal, p.status,
                                       STRING_AGG(pr.nome || ' (x' || ip.quantidade || ')', ', ') AS itens_resumo
                                FROM pedidos p
                                INNER JOIN clientes c ON p.cpf = c.cpf
                                LEFT JOIN itens_pedido ip ON p.id_pedido = ip.pedido_id
                                LEFT JOIN produtos pr ON ip.produto_id = pr.id_produto
                                WHERE (LOWER(TRIM(c.email)) = LOWER(TRIM(@login)) OR TRIM(c.cpf) = TRIM(@login))
                                  AND (pr.nome ILIKE @busca";

                            cmd.Parameters.AddWithValue("@login", usuarioLogado);
                            cmd.Parameters.AddWithValue("@busca", "%" + busca + "%");

                            if (int.TryParse(busca, out int idPedidoBusca))
                            {
                                sql += " OR p.id_pedido = @idPedido";
                                cmd.Parameters.AddWithValue("@idPedido", idPedidoBusca);
                            }
                            else
                            {
                                sql += " OR p.status ILIKE @buscaStatus";
                                cmd.Parameters.AddWithValue("@buscaStatus", "%" + busca + "%");
                            }
                            sql += ")";
                        }

                        sql += " GROUP BY p.id_pedido, p.cpf, p.data_pedido, p.valor_total, p.status ORDER BY p.data_pedido DESC";
                        cmd.CommandText = sql;

                        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gridPedidos.DataSource = dt;
                                gridPedidos.DataBind();
                                gridPedidos.Visible = true;
                                Msg.Text = "";
                            }
                            else
                            {
                                gridPedidos.Visible = false;
                                Msg.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
                                Msg.Text = "Nenhum pedido encontrado com este critério.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.ForeColor = System.Drawing.Color.Red;
                Msg.Text = "Erro na busca: " + ex.Message;
            }
        }
    }
}