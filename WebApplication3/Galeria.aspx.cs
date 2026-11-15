using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;

namespace WebApplication3
{
    public partial class Galeria : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarProdutos();
            }
        }

        private void CarregarProdutos()
        {
            string strConexao = ConfigurationManager.ConnectionStrings["SupabaseConnection"].ConnectionString;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(strConexao))
                {
                    conn.Open();

                    // Busca os produtos incluindo a coluna categoria_id
                    string sql = "SELECT id_produto, nome, descricao, preco AS preco_venda, imagem, COALESCE(categoria_id, 0) AS categoria_id FROM produtos ORDER BY nome ASC";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (!dt.Columns.Contains("imagem"))
                            {
                                dt.Columns.Add("imagem", typeof(string));
                            }

                            // 1. Doces Artesanais (categoria_id = 1)
                            DataView dvDoces = new DataView(dt);
                            dvDoces.RowFilter = "categoria_id = 1";
                            rptDoces.DataSource = dvDoces;
                            rptDoces.DataBind();

                            // 2. Bolo de Pote (categoria_id = 2)
                            DataView dvBoloPote = new DataView(dt);
                            dvBoloPote.RowFilter = "categoria_id = 2";
                            rptBoloPote.DataSource = dvBoloPote;
                            rptBoloPote.DataBind();

                            // 3. Bebidas (categoria_id = 3)
                            DataView dvBebidas = new DataView(dt);
                            dvBebidas.RowFilter = "categoria_id = 3";
                            rptBebidas.DataSource = dvBebidas;
                            rptBebidas.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.Text = "Erro ao carregar o catálogo: " + ex.Message;
            }
        }

        protected void btnEncomendar_Click(object sender, EventArgs e)
        {
            if (Session["autenticado"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            Button btn = (Button)sender;
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;

            TextBox txtQtd = (TextBox)item.FindControl("txtQtd");
            HiddenField hfIdProduto = (HiddenField)item.FindControl("hfIdProduto");
            HiddenField hfPreco = (HiddenField)item.FindControl("hfPreco");

            int idProduto = Convert.ToInt32(hfIdProduto.Value);
            int quantidade = 1;
            if (txtQtd != null)
            {
                int.TryParse(txtQtd.Text, out quantidade);
                if (quantidade < 1) quantidade = 1;
            }

            decimal precoUnitario = Convert.ToDecimal(hfPreco.Value);
            decimal subtotal = precoUnitario * quantidade;

            string nomeProduto = "Produto #" + idProduto;
            string strConexao = ConfigurationManager.ConnectionStrings["SupabaseConnection"].ConnectionString;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(strConexao))
                {
                    conn.Open();
                    string sqlNome = "SELECT nome FROM produtos WHERE id_produto = @id";
                    using (NpgsqlCommand cmdNome = new NpgsqlCommand(sqlNome, conn))
                    {
                        cmdNome.Parameters.AddWithValue("@id", idProduto);
                        object res = cmdNome.ExecuteScalar();
                        if (res != null) nomeProduto = res.ToString();
                    }
                }
            }
            catch { }

            DataTable dtCarrinho = Session["CarrinhoDataTable"] as DataTable;
            if (dtCarrinho == null)
            {
                dtCarrinho = new DataTable();
                dtCarrinho.Columns.Add("id_produto", typeof(int));
                dtCarrinho.Columns.Add("nome", typeof(string));
                dtCarrinho.Columns.Add("quantidade", typeof(int));
                dtCarrinho.Columns.Add("preco_unitario", typeof(decimal));
                dtCarrinho.Columns.Add("subtotal", typeof(decimal));
            }

            bool produtoExiste = false;
            foreach (DataRow row in dtCarrinho.Rows)
            {
                if (Convert.ToInt32(row["id_produto"]) == idProduto)
                {
                    int qtdAtual = Convert.ToInt32(row["quantidade"]);
                    int novaQtd = qtdAtual + quantidade;
                    row["quantidade"] = novaQtd;
                    row["subtotal"] = novaQtd * precoUnitario;
                    produtoExiste = true;
                    break;
                }
            }

            if (!produtoExiste)
            {
                dtCarrinho.Rows.Add(idProduto, nomeProduto, quantidade, precoUnitario, subtotal);
            }

            Session["CarrinhoDataTable"] = dtCarrinho;
            Response.Redirect("~/Carrinho.aspx");
        }
    }
}