using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication3
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
    {
        // se a variável de sessão "autenticado" existir faça isso.
        if (Session["autenticado"] != null)
        {
            Login.Visible = false;
            Logout.Visible = true;
            Produtos.Visible = true;
            Galeria.Visible = true;
            Carrinho.Visible = true;

            }
        else
        {
            Login.Visible = true;
            Logout.Visible = false;
            Produtos.Visible = false;
            Galeria.Visible = true;
            Carrinho.Visible = true;
            }
    }

    protected void Logout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx");

    }
}
}
