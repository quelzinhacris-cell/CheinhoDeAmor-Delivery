using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;

namespace WebApplication3
{
    public partial class Carrinho : Page
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
                CarregarCarrinho();
            }
        }

        private void CarregarCarrinho()
        {
            DataTable dtCarrinho = Session["CarrinhoDataTable"] as DataTable;

            if (dtCarrinho == null || dtCarrinho.Rows.Count == 0)
            {
                gridCarrinho.Visible = false;
                painelValores.Visible = false;
                Msg.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
                Msg.Text = "O seu carrinho está vazio. Visite a Galeria para encomendar produtos!";
            }
            else
            {
                gridCarrinho.Visible = true;
                painelValores.Visible = true;
                gridCarrinho.DataSource = dtCarrinho;
                gridCarrinho.DataBind();

                CalcularTotais(dtCarrinho);
            }
        }

        private decimal ObterFreteCliente(string usuarioLogado, out string enderecoFormatado)
        {
            decimal valorFrete = 10.00m; // Valor padrão de segurança caso não encontre
            enderecoFormatado = "Endereço não cadastrado";
            string strConexao = ConfigurationManager.ConnectionStrings["SupabaseConnection"].ConnectionString;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(strConexao))
                {
                    conn.Open();
                    // Cruza a tabela clientes, endereco_entrega e faixas_entregas automaticamente
                    string sql = @"
                        SELECT e.logradouro, e.numero, e.bairro, f.valor_entrega 
                        FROM endereco_entrega e
                        INNER JOIN clientes c ON e.cpf = c.cpf
                        LEFT JOIN faixas_entregas f ON e.id_end = f.end_id
                        WHERE LOWER(TRIM(c.email)) = LOWER(TRIM(@login)) OR TRIM(c.cpf) = TRIM(@login)
                        LIMIT 1";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", usuarioLogado);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["valor_entrega"] != DBNull.Value)
                                {
                                    valorFrete = Convert.ToDecimal(reader["valor_entrega"]);
                                }
                                enderecoFormatado = reader["logradouro"] + ", " + reader["numero"] + " - " + reader["bairro"];
                            }
                        }
                    }
                }
            }
            catch
            {
                // Mantém o padrão em caso de exceção de leitura
            }

            return valorFrete;
        }

        private void CalcularTotais(DataTable dt)
        {
            decimal subtotalGeral = 0;
            foreach (DataRow row in dt.Rows)
            {
                subtotalGeral += Convert.ToDecimal(row["subtotal"]);
            }

            string usuarioLogado = Session["autenticado"].ToString();
            string enderecoTemp = "";
            decimal frete = ObterFreteCliente(usuarioLogado, out enderecoTemp);

            decimal totalGeral = subtotalGeral + frete;

            lblSubtotal.Text = subtotalGeral.ToString("C");
            lblFrete.Text = frete.ToString("C"); // Exibe o frete dinâmico puxado do banco
            lblTotal.Text = totalGeral.ToString("C");
        }

        protected void gridCarrinho_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dtCarrinho = Session["CarrinhoDataTable"] as DataTable;
            if (dtCarrinho != null)
            {
                dtCarrinho.Rows.RemoveAt(e.RowIndex);
                Session["CarrinhoDataTable"] = dtCarrinho;
                CarregarCarrinho();
            }
        }

        protected void btnContinuarComprando_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Galeria.aspx");
        }

        protected void btnFinalizarPedido_Click(object sender, EventArgs e)
        {
            DataTable dtCarrinho = Session["CarrinhoDataTable"] as DataTable;
            if (dtCarrinho == null || dtCarrinho.Rows.Count == 0)
            {
                Msg.ForeColor = System.Drawing.Color.Red;
                Msg.Text = "Carrinho vazio. Adicione itens antes de finalizar.";
                return;
            }

            string usuarioLogado = Session["autenticado"].ToString();
            string strConexao = ConfigurationManager.ConnectionStrings["SupabaseConnection"].ConnectionString;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(strConexao))
                {
                    conn.Open();

                    // 1. Descobre o CPF do cliente logado
                    string cpfCliente = "";
                    string sqlCpf = "SELECT cpf FROM clientes WHERE LOWER(TRIM(email)) = LOWER(TRIM(@login)) OR TRIM(cpf) = TRIM(@login)";

                    using (NpgsqlCommand cmdCpf = new NpgsqlCommand(sqlCpf, conn))
                    {
                        cmdCpf.Parameters.AddWithValue("@login", usuarioLogado);
                        object result = cmdCpf.ExecuteScalar();
                        if (result != null)
                        {
                            cpfCliente = result.ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(cpfCliente))
                    {
                        Msg.ForeColor = System.Drawing.Color.Red;
                        Msg.Text = "Erro: Cliente não encontrado para associar ao pedido.";
                        return;
                    }

                    // Calcula subtotal, frete automático e valor total
                    decimal subtotalGeral = 0;
                    foreach (DataRow row in dtCarrinho.Rows)
                    {
                        subtotalGeral += Convert.ToDecimal(row["subtotal"]);
                    }

                    string enderecoIgnorado = "";
                    decimal valorFrete = ObterFreteCliente(usuarioLogado, out enderecoIgnorado);
                    decimal valorTotal = subtotalGeral + valorFrete;

                    // 2. Insere na tabela 'pedidos' incluindo o valor do frete dinâmico e recupera o ID gerado
                    string sqlPedido = "INSERT INTO pedidos (cpf, data_pedido, subtotal, valor_entrega, valor_total, status) VALUES (@cpf, NOW(), @subtotal, @valor_entrega, @valorTotal, 'Pendente') RETURNING id_pedido;";
                    int idPedidoGerado = 0;

                    using (NpgsqlCommand cmdPedido = new NpgsqlCommand(sqlPedido, conn))
                    {
                        cmdPedido.Parameters.AddWithValue("@cpf", cpfCliente);
                        cmdPedido.Parameters.AddWithValue("@subtotal", subtotalGeral);
                        cmdPedido.Parameters.AddWithValue("@valor_entrega", valorFrete);
                        cmdPedido.Parameters.AddWithValue("@valorTotal", valorTotal);

                        idPedidoGerado = Convert.ToInt32(cmdPedido.ExecuteScalar());
                    }

                    // 3. Insere cada item do carrinho na tabela 'itens_pedido'
                    foreach (DataRow row in dtCarrinho.Rows)
                    {
                        int idProduto = Convert.ToInt32(row["id_produto"]);
                        int quantidade = Convert.ToInt32(row["quantidade"]);
                        decimal precoUnitario = Convert.ToDecimal(row["preco_unitario"]);

                        string sqlItem = "INSERT INTO itens_pedido (pedido_id, produto_id, quantidade, preco_unitario) VALUES (@pedido_id, @produto_id, @quantidade, @preco_unitario)";

                        using (NpgsqlCommand cmdItem = new NpgsqlCommand(sqlItem, conn))
                        {
                            cmdItem.Parameters.AddWithValue("@pedido_id", idPedidoGerado);
                            cmdItem.Parameters.AddWithValue("@produto_id", idProduto);
                            cmdItem.Parameters.AddWithValue("@quantidade", quantidade);
                            cmdItem.Parameters.AddWithValue("@preco_unitario", precoUnitario);

                            cmdItem.ExecuteNonQuery();
                        }
                    }

                    // Limpa o carrinho e redireciona para a lista de pedidos
                    Session.Remove("CarrinhoDataTable");
                    Response.Redirect("~/ExibirPedidos.aspx");
                }
            }
            catch (Exception ex)
            {
                Msg.ForeColor = System.Drawing.Color.Red;
                Msg.Text = "Erro ao finalizar pedido: " + ex.Message;
            }
        }
    }
}