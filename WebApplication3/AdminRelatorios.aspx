<%@ Page Title="Relatório e Consulta Avançada - Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminRelatorios.aspx.cs" Inherits="WebApplication3.AdminRelatorios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .filtro-card {
            background-color: #f8f9fa;
            border: 1px solid #e0e0e0;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 25px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
        }
        .resumo-gasto {
            font-size: 1.1rem;
            font-weight: bold;
            color: #28a745;
            background: #e9f7ef;
            padding: 10px 15px;
            border-radius: 5px;
            margin-bottom: 15px;
            display: inline-block;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Conteudo" runat="server">
    <div class="container margin-top-60" style="padding: 20px;">
        <h2>Consulta Avançada de Pedidos e Clientes</h2>
        <p class="text-muted">Filtre por cliente, CPF, número do pedido ou item comprado para auditoria e relatórios.</p>
        <hr />

        <asp:Label ID="Msg" runat="server" Font-Bold="true"></asp:Label>

        <!-- Filtros de Pesquisa -->
        <div class="filtro-card">
            <h4>Filtros de Pesquisa</h4>
            <div class="row" style="display: flex; flex-wrap: wrap; gap: 15px; margin-top: 15px;">
                <div style="flex: 1; min-width: 220px;">
                    <label>Nome do Cliente:</label>
                    <asp:TextBox ID="txtNomeCliente" runat="server" CssClass="form-control" Placeholder="Ex: Raquel"></asp:TextBox>
                </div>
                <div style="flex: 1; min-width: 180px;">
                    <label>CPF:</label>
                    <asp:TextBox ID="txtCpf" runat="server" CssClass="form-control" Placeholder="Ex: 11122233344"></asp:TextBox>
                </div>
                <div style="flex: 1; min-width: 150px;">
                    <label>Nº do Pedido:</label>
                    <asp:TextBox ID="txtIdPedido" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Ex: 35"></asp:TextBox>
                </div>
                <div style="flex: 1; min-width: 200px;">
                    <label>Item / Produto (ex: Chocolate):</label>
                    <asp:TextBox ID="txtItemProduto" runat="server" CssClass="form-control" Placeholder="Ex: Brownie ou Chocolate"></asp:TextBox>
                </div>
            </div>
            <div style="margin-top: 20px; display: flex; gap: 10px;">
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar Resultados" CssClass="btn btn-primary" OnClick="btnFiltrar_Click" />
                <asp:Button ID="btnLimpar" runat="server" Text="Limpar Filtros" CssClass="btn btn-secondary" OnClick="btnLimpar_Click" />
            </div>
        </div>

        <!-- Indicador de Gasto Total Filtrado -->
        <asp:Panel ID="panelResumo" runat="server" Visible="false">
            <div class="resumo-gasto">
                Gasto Total nos Filtros Selecionados: <asp:Label ID="lblGastoTotal" runat="server" Text="R$ 0,00"></asp:Label>
            </div>
        </asp:Panel>

        <!-- Tabela de Resultados -->
        <div style="margin-top: 20px;">
            <asp:GridView ID="gridRelatorio" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="id_pedido" HeaderText="Nº Pedido" />
                    <asp:BoundField DataField="data_pedido" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="nome_cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="cpf" HeaderText="CPF" />
                    <asp:BoundField DataField="nome_produto" HeaderText="Item Comprado" />
                    <asp:BoundField DataField="quantidade" HeaderText="Qtd" />
                    <asp:BoundField DataField="preco_unitario" HeaderText="Preço Unit." DataFormatString="{0:C}" />
                    <asp:BoundField DataField="subtotal_item" HeaderText="Subtotal Item" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="valor_total_pedido" HeaderText="Total Pedido" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="status" HeaderText="Status" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
