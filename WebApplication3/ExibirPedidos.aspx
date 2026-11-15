<%@ Page Title="Gerenciamento de Pedidos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExibirPedidos.aspx.cs" Inherits="WebApplication3.ExibirPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Conteudo" runat="server">
    <div class="row margin-top-60">
        <div class="col-12">
            <div class="box padding-20">
                <h1>Painel de Pedidos</h1>
                <br />
                
                <div style="display: flex; gap: 10px; margin-bottom: 20px;">
                    <asp:TextBox ID="txtBuscar" runat="server" Placeholder="Buscar por CPF ou Nº do Pedido..."></asp:TextBox>
                    <asp:Button ID="btnBuscar" OnClick="btnBuscar_Click" runat="server" Text="Buscar" />
                </div>

                <asp:Label ID="Msg" runat="server" ForeColor="Red"></asp:Label>
                <br />

                <asp:GridView ID="gridPedidos" runat="server" AutoGenerateColumns="False" CssClass="table" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="id_pedido" HeaderText="Nº Pedido" />
                        <asp:BoundField DataField="cpf" HeaderText="CPF do Cliente" />
                        <asp:BoundField DataField="data_pedido" HeaderText="Data/Hora" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:BoundField DataField="itens_resumo" HeaderText="Itens do Pedido" />
                        <asp:BoundField DataField="subtotal" HeaderText="Subtotal" DataFormatString="{0:C2}" />
                        <asp:BoundField DataField="status" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
