using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Npgsql;

namespace WebApplication3
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Função que converte o texto para Hash MD5
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

        protected void Entrar_Click(object sender, EventArgs e)
        {
            string loginDigitado = NomeAcesso.Text.Trim();
            string senhaDigitada = Senha.Text.Trim();

            // 1. VERIFICAÇÃO ESPECIAL PARA O ADMIN
            // Defina aqui a senha que deseja usar para o Admin (ex: "123456" ou a sua senha antiga)
            if (loginDigitado.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Substitua "123456" pela senha que deseja para o Admin
                if (senhaDigitada == "123456")
                {
                    RealizarLoginSucesso("Admin");
                    return;
                }
                else
                {
                    Msg.Text = "Senha do Administrador incorreta!";
                    return;
                }
            }

            // 2. FLUXO NORMAL PARA OS CLIENTES (Consulta no Supabase)
            string strConexao = ConfigurationManager.ConnectionStrings["SupabaseConnection"].ConnectionString;

            try
            {
                string senhaCriptografada = GerarHashMD5(senhaDigitada);

                using (NpgsqlConnection conn = new NpgsqlConnection(strConexao))
                {
                    conn.Open();

                    string sql = "SELECT COUNT(*) FROM clientes WHERE (email = @login OR cpf = @login) AND senha = @senha";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", loginDigitado);
                        cmd.Parameters.AddWithValue("@senha", senhaCriptografada);

                        int usuarioEncontrado = Convert.ToInt32(cmd.ExecuteScalar());

                        if (usuarioEncontrado > 0)
                        {
                            RealizarLoginSucesso(loginDigitado);
                        }
                        else
                        {
                            Msg.Text = "E-mail/CPF ou senha incorretos!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.Text = "Erro ao conectar com o banco: " + ex.Message;
            }
        }

        // Método auxiliar para criar a sessão e o ticket de autenticação
        private void RealizarLoginSucesso(string usuario)
        {
            Session["autenticado"] = usuario;

            FormsAuthentication.Initialize();
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                usuario,
                DateTime.Now,
                DateTime.Now.AddMinutes(20),
                false,
                FormsAuthentication.FormsCookiePath
            );

            Response.Cookies.Add(new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(ticket)
            ));

            Response.Redirect(FormsAuthentication.GetRedirectUrl(usuario, false));
        }
    }
}