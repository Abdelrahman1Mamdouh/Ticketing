<%@ Page Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestioneTicket.aspx.cs" Inherits="Ticketing.GestioneTicket" %>

<%@ Register Src="~/Controls/NotifichePopup.ascx" TagPrefix="uc" TagName="NotifichePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    runat="Server">
    
        <div class="grid_system">
            <div class="col-33">

                <asp:Label ID="LId"
                    Text="Id"
                    runat="server"
                    Visible="false" />

                <asp:TextBox ID="Tid"
                    runat="server"
                    placeholder="Id"
                    Visible="false">
                </asp:TextBox>

                <asp:Label ID="LCliente"
                    Text="Cliente"
                    runat="server"
                    Visible="false" />

                <asp:TextBox ID="TCliente"
                    runat="server"
                    placeholder="Cliente"
                    Visible="false">
                </asp:TextBox>

                <asp:Label ID="LTecnico"
                    Text="Tecnico"
                    runat="server"
                    Visible="false" />

                <asp:TextBox ID="TTecnico"
                    runat="server"
                    placeholder="Tecnico"
                    Visible="false">
                </asp:TextBox>

                <asp:DropDownList
                    ID="DTecnici"
                    Visible="false"
                    runat="server"
                    AutoPostBack="false"
                    DataTextField="Nome"
                    DataValueField="ID">
                </asp:DropDownList>

                <asp:Button ID="BbAssegna"
                    Text="Assegna il Ticket"
                    Visible="false"
                    OnClick="ClickAssegna"
                    runat="server"
                    class="btn-viola"/>

                <asp:Label ID="LLivello"
                    Text="Livello"
                    runat="server"
                    Visible="false" />
                <asp:TextBox ID="TLivello"
                    runat="server"
                    Visible="false"
                    placeholder="Livello">
                </asp:TextBox>
                <asp:DropDownList ID="DLivello"
                    Visible="false"
                    runat="server"
                    DataTextField="Livello"
                    DataValueField="ID">
                </asp:DropDownList>
                <asp:Label ID="LStato"
                    Text="Stato"
                    runat="server"
                    Visible="false" />
                <asp:Label ID="LLStato"
                    Text=""
                    runat="server"
                    Visible="false" />
                <asp:DropDownList ID="DStato"
                    Visible="false"
                    runat="server"
                    DataTextField="Stato"
                    DataValueField="ID">
                </asp:DropDownList>


                <asp:Button ID="BCambiaStato"
                    Text="Cambia Stato"
                    Visible="false"
                    OnClick="CambiaStato"
                    runat="server" />

                <asp:Label ID="LProdotto"
                    Visible="false"
                    Text="Prodotto"
                    runat="server" />
                <asp:DropDownList ID="DProdotto"
                    Visible="false"
                    runat="server"
                    AutoPostBack="false"
                    DataTextField="Prodotto"
                    DataValueField="ID">
                </asp:DropDownList>
                <asp:Label ID="LCategoria"
                    Visible="false"
                    Text="Categoria"
                    runat="server" />
                <asp:DropDownList ID="DCategoria"
                    Visible="false"
                    runat="server"
                    AutoPostBack="false"
                    DataTextField="Categoria"
                    DataValueField="ID">
                </asp:DropDownList>
               


                <div>
                    <h1 id="IDlabelTicket"
                        visible="false"
                        runat="server"></h1>
                </div>



                <asp:Label ID="LPriorita"
                    Visible="false"
                    Text="Priorita"
                    runat="server" />
                <asp:DropDownList ID="DPriorita"
                    Visible="false"
                    runat="server"
                    DataTextField="Priorita"
                    DataValueField="ID">
                </asp:DropDownList>

                <asp:Button ID="BbSalva"
                    Text="Salva"
                    Visible="false"
                    OnClick="ClickSalva"
                    runat="server" 
                    class="btn-viola"/>

            </div>


            <div class="col-33">
                <asp:Label ID="LOggetto"
                    Text="Oggetto"
                    Visible="false"
                    runat="server" />
                <asp:TextBox ID="TOggetto"
                    runat="server"
                    Visible="false"
                    placeholder="Oggetto">
                </asp:TextBox>
                <asp:Label ID="LMessaggio"
                    Text="Messaggio"
                    Visible="false"
                    runat="server" />
                <asp:TextBox ID="TMessaggio"
                    runat="server"
                    Visible="false"
                    placeholder="Messaggio"
                    ClientIDMode="Static"
                    TextMode="MultiLine">
                </asp:TextBox>

            </div>



            <div class="col-33">
                <div class="box-gridview" style="max-height: 500px; overflow-y: auto;">
                    <asp:GridView ID="Storico"
                        runat="server"
                        DataKeyNames="ID"
                        GridLines="None"
                        HorizontalAlign="Center"
                        
                        
                        CssClass="gridview"
                        AutoGenerateColumns="True">
                    </asp:GridView>
                </div>
                

            </div>

            <div class="box-tasto-crea">
                <asp:Button ID="BCrea"
                    Text="Crea"
                    Visible="false"
                    OnClick="clickCrea"
                    runat="server"
                    class="btn-viola" />

                <asp:Button ID="BChiudi"
                    Text="Chiudi"
                    Visible="false"
                    OnClick="Annulla"
                    runat="server"
                    class="btn-modifica" />
            </div>


            <div class="box_comunicazione mt-2">
                <div class="box-comunicazione-100">
                    <asp:Label ID="LComunicazione"
                        Text="Comunicazione"
                        Visible="false"
                        runat="server" />
                    <asp:TextBox ID="TComunicazione"
                        runat="server"
                        Visible="false"
                        placeholder="Comunicazione"
                        ClientIDMode="Static"
                        TextMode="MultiLine">
                    </asp:TextBox>
                </div>

                <div class="box-100 mt-2">
                    <%--                <asp:Button ID="BStorico"
         Text="Storico"
         Visible="false"
         runat="server"
         OnClick="Storico" />--%>
                    <asp:Button ID="BRisposta"
                        Text="Invia Risposta"
                        Visible="false"
                        OnClick="MandaComunicazione"
                        runat="server" 
                        class="btn-viola"/>
                    <asp:Button ID="BAnnulla"
                        Text="Annulla"
                        Visible="false"
                        OnClick="Annulla"
                        runat="server" 
                        class="btn-annulla"/>

                </div>
            </div>
           
        </div>
    
    <uc:NotifichePopup ID="NotifichePopup" runat="server" />
</asp:Content>
