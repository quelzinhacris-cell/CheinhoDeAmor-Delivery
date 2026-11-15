<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CadastroProdutos.aspx.cs" Inherits="WebApplication3.CadastroProdutos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Conteudo" runat="server">
    <div class="row margin-top-60">

        <div class="col-8">

            <div class="border box-shadow padding-14">
                <h2>Cadastro de Produtos</h2>
                <br />
                <asp:Label ID="Alerta" runat="server" Font-Size="16px" ForeColor="Red"></asp:Label>
                <br />
                <label>Nome do Produto</label>
                <asp:TextBox ID="Nome" runat="server" MaxLength="255"></asp:TextBox>
                <label>Descrição</label>
                <asp:TextBox ID="Descricao" MaxLength="255" TextMode="MultiLine" Rows="6" runat="server"></asp:TextBox>
                <label>Situacao</label>
                <asp:DropDownList Width="100" ID="Situacao" runat="server">
                    <asp:ListItem Text="Ativo" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Inativo" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <label>Quantidade</label>
                <asp:TextBox ID="Quantidade" Text="0"  Width="100" runat="server"></asp:TextBox>
                <label>Preço de Custo</label>
                <asp:TextBox ID="PrecoCusto" Text="0"  Width="100" runat="server"></asp:TextBox>
                <label>Preço de Venda</label>
                <asp:TextBox ID="PrecoVenda" Text="0" Width="100" runat="server" MaxLength="50"></asp:TextBox>
                <br />
                <br />
                <label>Imagem do Produto</label>
                <asp:FileUpload ID="FileUpload" runat="server" />
                <br />
                <br />
                <asp:Button ID="Cadastrar" OnClick="Cadastrar_Click" runat="server" Text="Cadastrar" />
                <br />
                <br />

            </div>
        </div>
    </div>
</asp:Content>
