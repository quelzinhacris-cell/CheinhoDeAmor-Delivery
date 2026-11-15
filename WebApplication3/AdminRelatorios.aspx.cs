using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using Npgsql;

namespace WebApplication3
{
    public partial class AdminRelatorios : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Restringe o acesso estritamente ao Administrador
            if (Session["autenticado"] == null || !Session["autenticado"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CarregarDadosRelatorio("", "", "", "");
            }
        }

        private void CarregarDadosRelatorio(string nomeCliente, string cpf, string idPedido, string itemProduto)
        {
            string strConexao = ConfigurationManager.ConnectionStrings["SupabaseConnection"].ConnectionString;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(strConexao))
                {
                    conn.Open();

                    // Query unindo Pedidos, Clientes, Itens do Pedido e Produtos
                    string sql = @"
                        SELECT p.id_pedido, p.data_pedido, p.status, p.valor_total AS valor_total_pedido,
                               c.nome AS nome_cliente, c.cpf,
                               pr.nome AS nome_produto, i.quantidade, i.preco_unitario,
                               (i.quantidade * i.preco_unitario) AS subtotal_item
                        FROM pedidos p
                        INNER JOIN clientes c ON p.cpf = c.cpf
                        INNER JOIN itens_pedido i ON p.id_pedido = i.pedido_id
                        INNER JOIN produtos pr ON i.produto_id = pr.id_produto
                        WHERE 1=1";

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        if (!string.IsNullOrWhiteSpace(nomeCliente))
                        {
                            sql += " AND LOWER(c.nome) LIKE LOWER(@nome)";
                            cmd.Parameters.AddWithValue("@nome", "%" + nomeCliente.Trim() + "%");
                        }

                        if (!string.IsNullOrWhiteSpace(cpf))
                        {
                            sql += " AND REPLACE(REPLACE(REPLACE(c.cpf, '.', ''), '-', ''), ' ', '') LIKE @cpf";
                            cmd.Parameters.AddWithValue("@cpf", "%" + cpf.Trim().Replace(".", "").Replace("-", "") + "%");
                        }

                        if (!string.IsNullOrWhiteSpace(idPedido) && int.TryParse(idPedido, out int idPed))
                        {
                            sql += " AND p.id_pedido = @idPedido";
                            cmd.Parameters.AddWithValue("@idPedido", idPed);
                        }

                        if (!string.IsNullOrWhiteSpace(itemProduto))
                        {
                            sql += " AND (LOWER(pr.nome) LIKE LOWER(@produto) OR LOWER(pr.descricao) LIKE LOWER(@produto))";
                            cmd.Parameters.AddWithValue("@produto", "%" + itemProduto.Trim() + "%");
                        }

                        sql += " ORDER BY p.data_pedido DESC";
                        cmd.CommandText = sql;

                        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            gridRelatorio.DataSource = dt;
                            gridRelatorio.DataBind();

                            if (dt.Rows.Count > 0)
                            {
                                gridRelatorio.Visible = true;
                                panelResumo.Visible = true;

                                // Calcula o somatório dos subtotais dos itens exibidos na consulta atual
                                decimal gastoTotal = 0;
                                foreach (DataRow row in dt.Rows)
                                {
                                    gastoTotal += Convert.ToDecimal(row["subtotal_item"]);
                                }
                                lblGastoTotal.Text = gastoTotal.ToString("C");
                                Msg.Text = "";
                            }
                            else
                            {
                                gridRelatorio.Visible = false;
                                panelResumo.Visible = false;
                                Msg.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
                                Msg.Text = "Nenhum registro encontrado com os filtros informados.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.ForeColor = System.Drawing.Color.Red;
                Msg.Text = "Erro ao gerar relatório: " + ex.Message;
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarDadosRelatorio(
                txtNomeCliente.Text,
                txtCpf.Text,
                txtIdPedido.Text,
                txtItemProduto.Text
            );
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtNomeCliente.Text = "";
            txtCpf.Text = "";
            txtIdPedido.Text = "";
            txtItemProduto.Text = "";
            panelResumo.Visible = false;
            CarregarDadosRelatorio("", "", "", "");
        }
    }
}