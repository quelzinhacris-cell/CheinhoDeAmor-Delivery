<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FaleConosco.aspx.cs" Inherits="WebApplication3.FaleConosco" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Conteudo" runat="server">
    <div class="row margin-top-60"> 

        <div class="col-6">

            <div class="border box-shadow padding-14">
                <h2>Fale Conosco</h2>
                <br />
                <asp:Label ID="Alerta" runat="server" Font-Size="16px" ForeColor="#FF0000"></asp:Label>
                <br />
                <label>Seu Nome</label>
                <asp:TextBox ID="SeuNome" runat="server" MaxLength="100"></asp:TextBox>
                <label>E-mail</label>
                <asp:TextBox ID="Email" runat="server" MaxLength="255" TextMode="Email"></asp:TextBox>
                <label>Mensagem</label>
                <asp:TextBox ID="Mensagem" MaxLength="255" TextMode="MultiLine" Rows="6" runat="server"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="Enviar" OnClick="Enviar_Click" runat="server" Text="Enviar" />
                <br />
                <br />
            </div>

        </div>
        <div class="col-6">
            <div class="border box-shadow">
              
                <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3679.261171428217!2d-47.36417572574783!3d-22.75568693248637!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x94c89b6aabe08fa9%3A0xe429a1f403ad249a!2sCheinho%20de%20Amor%20-%20Bolos%2C%20Doces%20e%20Presentes%20-%20Delivery%20em%20Americana%2C%20Santa%20B%C3%A1rbara%20D%C2%B4Oeste%20e%20Nova%20Odessa!5e0!3m2!1spt-BR!2sbr!4v1764976548528!5m2!1spt-BR!2sbr" width="100%" height="100%" style="border:0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
               
            </div>


        </div>
      </div>
</asp:Content>
