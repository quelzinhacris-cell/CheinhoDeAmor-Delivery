<%@ Page Title="Carrinho de Compras" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Carrinho.aspx.cs" Inherits="WebApplication3.Carrinho" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Conteudo" runat="server">
    <div class="row margin-top-60">
        <div class="col-12">
            <h1>Seu Carrinho de Compras</h1>
            <br />

            <!-- Mensagens de erro ou sucesso -->
            <asp:Label ID="Msg" runat="server" Font-Bold="true"></asp:Label>
            <br /><br />

            <!-- Grid com os itens do carrinho -->
            <asp:GridView ID="gridCarrinho" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowDeleting="gridCarrinho_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="id_produto" HeaderText="Cód." />
                    <asp:BoundField DataField="nome" HeaderText="Produto" />
                    <asp:BoundField DataField="quantidade" HeaderText="Quantidade" />
                    <asp:BoundField DataField="preco_unitario" HeaderText="Preço Unitário" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
                    
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Remover" HeaderText="Ações" />
                </Columns>
            </asp:GridView>

            <br />

            <!-- Resumo de Valores -->
            <div style="text-align: right; font-size: 18px;" id="painelValores" runat="server">
                <p>Subtotal dos Produtos: <asp:Label ID="lblSubtotal" runat="server" Text="R$ 0,00" Font-Bold="true"></asp:Label></p>
                <p>Frete: <asp:Label ID="lblFrete" runat="server" Text="R$ 10,00" Font-Bold="true"></asp:Label></p>
                <p><strong>Valor Total: <asp:Label ID="lblTotal" runat="server" Text="R$ 0,00" Font-Bold="true" ForeColor="Green"></asp:Label></strong></p>
                
                <br />
                <!-- Forma de Pagamento -->
                <p><strong>Forma de Pagamento:</strong> Pix (Aprovação imediata)</p>
                <br />

                <asp:Button ID="btnContinuarComprando" runat="server" Text="Adicionar Mais Itens (Galeria)" CssClass="btn btn-secondary" OnClick="btnContinuarComprando_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnFinalizarPedido" runat="server" Text="Finalizar Pedido" CssClass="btn btn-success" OnClick="btnFinalizarPedido_Click" />
            </div>
        </div>
    </div>
</asp:Content>