<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExibirProdutos.aspx.cs" Inherits="WebApplication3.ExibirProdutos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Conteudo" runat="server">
<div class ="row margin-top-60">
    <div class ="col-10">
        <div class ="border box-shadow padding-14">
            <asp:TextBox ID="BuscarProduto" Width="120px" runat="server"></asp:TextBox>

            <asp:Button ID="Buscar" OnClick="Buscar_Click" runat="server" Text="Buscar" />
            <asp:Button ID="Cancelar" Visible="false" OnClick="Cancelar_Click" runat="server" Text="Fechar"  />
            <asp:Button ID="Cadastrar" Style="float:right"  OnClick="Cadastrar_Click" runat="server" Text="Cadastrar" />
            <br />
            <br />
            <asp:GridView AutoGenerateSelectButton="true" OnSelectedIndexChanged="Produtos_SelectedIndexChanged" ID="Produtos" Width="100%" CellPadding="8" BorderColor="#c0c0c0" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-HorizontalAlign="Center"/>
                     <asp:BoundField DataField="Nome" HeaderText="Nome Produto" HeaderStyle-HorizontalAlign="Center"/>
                     <asp:BoundField DataField="Quantidade" HeaderText="Quantidade em estoque" HeaderStyle-HorizontalAlign="Center" />
                     <asp:BoundField DataField="PrecoCusto" HeaderText="Preço de Custo" HeaderStyle-HorizontalAlign="Center"/>
                     <asp:BoundField DataField="PrecoVenda" HeaderText="Preço de Venda" HeaderStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
           
            
        </div>
    </div>
</div>
</asp:Content>
