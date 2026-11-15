using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Npgsql;

namespace WebApplication3
{
    public partial class Cadastro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string GerarHashMD5(string texto)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(texto);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            // Valida se os campos obrigatórios foram preenchidos
            if (string.IsNullOrEmpty(txtCPF.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtSenha.Text) || string.IsNullOrEmpty(txtDataAniversario.Text))
            {
                Msg.Text = "Por favor, preencha CPF, E-mail, Data de Nascimento e Senha.";
                return;
            }

            string strConexao = ConfigurationManager.ConnectionStrings["SupabaseConnection"].ConnectionString;

            try
            {
                string senhaHash = GerarHashMD5(txtSenha.Text.Trim());

                using (NpgsqlConnection conn = new NpgsqlConnection(strConexao))
                {
                    conn.Open();

                    string sql = @"INSERT INTO clientes (cpf, nome, email, telefone, data_aniversario, senha) 
                      VALUES (@cpf, @nome, @email, @telefone, @data_aniversario, @senha)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cpf", txtCPF.Text.Trim());
                        cmd.Parameters.AddWithValue("@nome", txtNome.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text.Trim());
                        cmd.Parameters.AddWithValue("@data_aniversario", DateTime.Parse(txtDataAniversario.Text));
                        cmd.Parameters.AddWithValue("@senha", senhaHash);

                        cmd.ExecuteNonQuery();
                    }
                }
                // ... restante do código de login ... 

                Session["autenticado"] = txtEmail.Text.Trim();

                FormsAuthentication.Initialize();
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    txtEmail.Text.Trim(),
                    DateTime.Now,
                    DateTime.Now.AddMinutes(20),
                    false,
                    FormsAuthentication.FormsCookiePath
                );

                Response.Cookies.Add(new HttpCookie(
                    FormsAuthentication.FormsCookieName,
                    FormsAuthentication.Encrypt(ticket)
                ));

                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                Msg.Text = "Erro ao cadastrar: " + ex.Message;
            }
        }
    }
}