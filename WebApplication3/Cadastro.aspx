<%@ Page Title="Cadastro" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="WebApplication3.Cadastro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Conteudo" runat="server">
    <div class="row margin-top-60">
        <div class="col-3">
            <div class="box padding-20">
                <h1>Criar Conta</h1>
                <br />
                <br />
                <asp:Label ID="Msg" runat="server"></asp:Label>
                <br />
                
                <label>CPF</label>
                <asp:TextBox ID="txtCPF" runat="server"></asp:TextBox>
                <br />

                <label>NOME COMPLETO</label>
                <asp:TextBox ID="txtNome" runat="server"></asp:TextBox>
                <br />

                <label>E-MAIL</label>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                <br />

                <label>TELEFONE</label>
                <asp:TextBox ID="txtTelefone" runat="server"></asp:TextBox>
                <br />

                <label>DATA DE NASCIMENTO</label>
                <asp:TextBox ID="txtDataAniversario" runat="server" TextMode="Date"></asp:TextBox>
                <br />

                <label>SENHA</label>
                <asp:TextBox ID="txtSenha" TextMode="Password" runat="server"></asp:TextBox>
                <br />
                <br />

                <asp:Button ID="btnCadastrar" OnClick="btnCadastrar_Click" runat="server" Text="Cadastrar e Entrar" />
                <br />
                <br />
                <p>Já possui conta? <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Login.aspx">Faça login aqui</asp:HyperLink></p>
            </div>
        </div>
    </div>
</asp:Content>