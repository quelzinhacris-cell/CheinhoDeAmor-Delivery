<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication3.Default" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--ESte conteudo será enviado para o HEAD do site.master-->
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="Conteudo" runat="server">
    <!--ESte conteudo será enviado para o BODY do site.master-->
    <!-- Place somewhere in the <body> of your page -->
    <div class="flexslider">
        <ul class="slides">
            <li>
                <img src="Images/CapaSite.png" />
            </li>

        </ul>
    </div>
    <script>
        $(window).load(function () {
            $('.flexslider').flexslider({
                animation: "slide",
                ontimeupdate: 10
            });
        });

    </script>

    <div class="row margin-top-120">
        <div class="col-12">
            <div class="border padding-14 box-shadow">
                <br />
                <div class="padding-14">
                    <h2>Sobre Nós</h2>
                    <br />
                    Theodore Zeldin diz: "Gastronomia é a arte de usar comida para criar felicidade"
                   
                    <br />
                    <br />
                    E nós acreditamos que o nosso amor juntamente com ingredientes selecionados e de alta qualidade cria-se felicidade em quem experimenta!
                    <br />
                    <br />
                    Quer experimentar?
                </div>
            </div>
        </div>


        <div class="margin-top-60">
            <div class="row">
                <div class="col-4">
                    <div class="border">
                        <img width="100%" src="Images/Brownie.jpg" />
                        <br />
                        <div class="padding-14">
                            <h2>Doces Artesanais</h2>
                            <br />
                            Nossos doces artesanais são preparados com ingredientes selecionados e muito amor, preservando o sabor caseiro que remete às melhores lembranças. Cada receita é feita à mão, em pequenos lotes, garantindo frescor, qualidade e um toque único em cada detalhe. Doce na medida certa para tornar seus momentos ainda mais especiais.
                        <br />
                            <br />
                            <a href="Galeria.aspx">Acesse aqui nosso cardápio</a>

                        </div>
                    </div>
                </div>
                <div class="col-4">
                    <div class="border">
                        <img width="100%" src="Images/BoloPote.JPG" />
                        <br />
                        <div class="padding-14">
                            <h2>Bolo de Pote</h2>
                            <br />
                            Nossos bolos de pote são camadas de amor, sabor e frescor, preparados artesanalmente com ingredientes selecionados. Cada pote combina massa fofinha, recheios cremosos e o toque caseiro que transforma qualquer momento em uma doce pausa. Práticos, irresistíveis e feitos para adoçar o seu dia.
                       <br />
                            <br />
                            <br />
                            <a href="Galeria.aspx">Acesse aqui nosso cardápio</a>

                        </div>
                    </div>
                </div>
                <div class="col-4">
                    <div class="border">
                        <img width="100%" src="Images/chocoQuente.jpg" />
                        <br />
                        <div class="padding-14">
                            <h2>Bebidas</h2>
                            <br />
                            Nossas bebidas são feitas com ingredientes selecionados e preparados na medida certa para oferecer cremosidade, aroma e aquele aconchego irresistível. Perfeitas para qualquer momento, elas combinam sabor intenso e textura aveludada, trazendo a sensação de um abraço quentinho ou refrescante em cada gole.
                        <br />
                            <br />
                            <br />
                            <a href="Galeria.aspx">Acesse aqui nosso cardápio</a>
                        </div>
                    </div>
                </div>

             </div>  
            </div>
        </div>
</asp:Content>
