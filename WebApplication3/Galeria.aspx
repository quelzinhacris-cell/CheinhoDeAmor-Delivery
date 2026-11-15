<%@ Page Title="Galeria de Produtos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Galeria.aspx.cs" Inherits="WebApplication3.Galeria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .categoria-titulo {
            color: #8B4513;
            border-bottom: 2px solid #8B4513;
            padding-bottom: 8px;
            margin-top: 40px;
            margin-bottom: 25px;
            font-weight: bold;
        }
        .row-produtos {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
            margin-bottom: 30px;
        }
        .card-produto {
            background: #ffffff;
            border: 1px solid #e0e0e0;
            border-radius: 10px;
            padding: 20px;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            height: 100%;
            box-shadow: 0 4px 8px rgba(0,0,0,0.05);
        }
        .card-produto img {
            width: 100%;
            height: 180px; /* Altura ideal para o container da foto */
            object-fit: contain; /* Garante que a foto inteira apareça sem cortes */
            border-radius: 6px;
            margin-bottom: 15px;
            background-color: #fafafa; /* Fundo suave caso a proporção da foto varie */
        }
        .card-produto h4 {
            font-size: 1.1rem;
            color: #333;
            margin-bottom: 10px;
            font-weight: bold;
        }
        .card-produto p {
            font-size: 0.9rem;
            color: #666;
            flex-grow: 1;
            margin-bottom: 15px;
        }
        .preco-produto {
            font-size: 1.25rem;
            font-weight: bold;
            color: #28a745;
            margin-bottom: 15px;
        }
        .acao-produto {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Conteudo" runat="server">
    <div class="container margin-top-60" style="padding: 20px;">
        <h2>Nosso Cardápio Gourmet</h2>
        <asp:Label ID="Msg" runat="server" Font-Bold="true"></asp:Label>

        <!-- SEÇÃO 1: DOCES ARTESANAIS -->
        <h3 class="categoria-titulo">Doces Artesanais</h3>
        <div class="row-produtos">
            <asp:Repeater ID="rptDoces" runat="server">
                <ItemTemplate>
                    <div style="flex: 1; min-width: 280px; max-width: 32%;">
                        <div class="card-produto">
                            <div>
                                <img src='<%# Eval("imagem") %>' alt='<%# Eval("nome") %>' onerror="this.src='Images/CapaSite.png';" />
                                <h4><%# Eval("nome") %></h4>
                                <p><%# Eval("descricao") %></p>
                            </div>
                            <div>
                                <div class="preco-produto">
                                    <%# Convert.ToDecimal(Eval("preco_venda")).ToString("C") %>
                                </div>
                                
                                <asp:HiddenField ID="hfIdProduto" Value='<%# Eval("id_produto") %>' runat="server" />
                                <asp:HiddenField ID="hfPreco" Value='<%# Eval("preco_venda") %>' runat="server" />
                                
                                <div class="acao-produto">
                                    <label style="font-weight: 600; font-size: 0.9rem;">Qtd:</label>
                                    <asp:TextBox ID="txtQtd" runat="server" Text="1" TextMode="Number" Width="55px" CssClass="form-control text-center"></asp:TextBox>
                                    <asp:Button ID="btnEncomendar" runat="server" Text="Encomendar" OnClick="btnEncomendar_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- SEÇÃO 2: BOLO DE POTE -->
        <h3 class="categoria-titulo">Bolo de Pote</h3>
        <div class="row-produtos">
            <asp:Repeater ID="rptBoloPote" runat="server">
                <ItemTemplate>
                    <div style="flex: 1; min-width: 280px; max-width: 32%;">
                        <div class="card-produto">
                            <div>
                                <img src='<%# Eval("imagem") %>' alt='<%# Eval("nome") %>' onerror="this.src='Images/CapaSite.png';" />
                                <h4><%# Eval("nome") %></h4>
                                <p><%# Eval("descricao") %></p>
                            </div>
                            <div>
                                <div class="preco-produto">
                                    <%# Convert.ToDecimal(Eval("preco_venda")).ToString("C") %>
                                </div>
                                
                                <asp:HiddenField ID="hfIdProduto" Value='<%# Eval("id_produto") %>' runat="server" />
                                <asp:HiddenField ID="hfPreco" Value='<%# Eval("preco_venda") %>' runat="server" />
                                
                                <div class="acao-produto">
                                    <label style="font-weight: 600; font-size: 0.9rem;">Qtd:</label>
                                    <asp:TextBox ID="txtQtd" runat="server" Text="1" TextMode="Number" Width="55px" CssClass="form-control text-center"></asp:TextBox>
                                    <asp:Button ID="btnEncomendar" runat="server" Text="Encomendar" OnClick="btnEncomendar_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- SEÇÃO 3: BEBIDAS -->
        <h3 class="categoria-titulo">Bebidas</h3>
        <div class="row-produtos">
            <asp:Repeater ID="rptBebidas" runat="server">
                <ItemTemplate>
                    <div style="flex: 1; min-width: 280px; max-width: 32%;">
                        <div class="card-produto">
                            <div>
                                <img src='<%# Eval("imagem") %>' alt='<%# Eval("nome") %>' onerror="this.src='Images/CapaSite.png';" />
                                <h4><%# Eval("nome") %></h4>
                                <p><%# Eval("descricao") %></p>
                            </div>
                            <div>
                                <div class="preco-produto">
                                    <%# Convert.ToDecimal(Eval("preco_venda")).ToString("C") %>
                                </div>
                                
                                <asp:HiddenField ID="hfIdProduto" Value='<%# Eval("id_produto") %>' runat="server" />
                                <asp:HiddenField ID="hfPreco" Value='<%# Eval("preco_venda") %>' runat="server" />
                                
                                <div class="acao-produto">
                                    <label style="font-weight: 600; font-size: 0.9rem;">Qtd:</label>
                                    <asp:TextBox ID="txtQtd" runat="server" Text="1" TextMode="Number" Width="55px" CssClass="form-control text-center"></asp:TextBox>
                                    <asp:Button ID="btnEncomendar" runat="server" Text="Encomendar" OnClick="btnEncomendar_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>