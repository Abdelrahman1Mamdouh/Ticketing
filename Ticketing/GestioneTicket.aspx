<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestioneTicket.aspx.cs" Inherits="Ticketing.GestioneTicket"%>
<%@ Register Src="~/Controls/NotifichePopup.ascx" TagPrefix="uc" TagName="NotifichePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    Runat="Server">
    <div>
    <div class="grid">
    <div class="col-50">

  <asp:Label ID="LId"
    Text="Id"
    runat="server"
      Visible="false"/>

  <asp:TextBox ID="Tid"
      runat="server"
      placeholder="Id"
      Visible="false">
  </asp:TextBox>

<asp:Label ID="LCliente"
    Text="Cliente"
    runat="server"
    Visible="false"/>

<asp:TextBox ID="TCliente"
    runat="server"
    placeholder="Cliente"
    Visible="false">
</asp:TextBox>

<asp:Label ID="LTecnico"
    Text="Tecnico"
    runat="server"
    Visible="false"/>

<asp:TextBox ID="TTecnico"
    runat="server"
    placeholder="Tecnico"
    Visible="false">
</asp:TextBox>
<asp:Label ID="LLivello"
    Text="Livello"
    runat="server"
    Visible="false"/>
<asp:TextBox ID="TLivello"
    runat="server"
    Visible="false"
    placeholder="Livello">
</asp:TextBox>
<asp:Label ID="LStato"
    Text="Stato"
    runat="server"
    Visible="false"/>
<asp:DropDownList ID="DStato"
    Visible="false"
    runat="server">
        <asp:ListItem>Item1</asp:ListItem>
        <asp:ListItem>Item2</asp:ListItem>
        <asp:ListItem>Item3</asp:ListItem>
</asp:DropDownList>






<asp:Label ID="LProdotto"
    Visible="false"
    Text="Prodotto"
    runat="server"/> 
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
    runat="server"/>
<asp:DropDownList ID="DCategoria"
    Visible="false"
    runat="server"
    AutoPostBack="false"
DataTextField="Categoria"
DataValueField="ID">       
</asp:DropDownList>



<div>
    <h1 ID="IDlabelTicket"
        Visible="false"
        runat="server"
        ></h1>
</div>



<asp:Label ID="LPriorita"
    Visible="false"
    Text="Priorita"
    runat="server"/>
 <asp:DropDownList ID="DPriorita"
     Visible="false"
    runat="server">
        <asp:ListItem>Priorita1</asp:ListItem>
        <asp:ListItem>Priorita2</asp:ListItem>
        <asp:ListItem>Priorita3</asp:ListItem>
</asp:DropDownList>





        </div>
        <div class="col-50">
<asp:Label ID="LOggetto"
    Text="Oggetto"
    Visible="false"
    runat="server"/>
<asp:TextBox ID="TOggetto"
    runat="server"
    Visible="false"
    placeholder="Oggetto">
</asp:TextBox>
<asp:Label ID="LMessaggio"
    Text="Messaggio"
    Visible="false"
    runat="server"/>
<asp:TextBox ID="TMessaggio"
    runat="server"
    Visible="false"
    placeholder="Messaggio"
     ClientIDMode="Static"
    TextMode="MultiLine">
</asp:TextBox>

</div>

<div class="grid-button">
<asp:Button ID="BCrea" 
    Text="Crea"
    Visible="false"
    OnClick="clickCrea"
    runat="server"/>
<asp:Button ID="BModifica" 
    Text="Modifica"
    Visible="false"
    runat="server"/> 
<asp:Button ID="BElimina" 
    Text="Elimina"
    Visible="false"
    runat="server"/>
</div>

<div class="col-100">

 <asp:Label ID="LComunicazione"
    Text="Comunicazione"
     Visible="false"
    runat="server"/>
<asp:TextBox ID="TComunicazione"
    runat="server"
    Visible="false"
    placeholder="Comunicazione"
    ClientIDMode="Static"
    TextMode="MultiLine">
</asp:TextBox>
</div>

<div class="btn-comunicazione">
<asp:Button id="BStorico"
  Text="Storico"         
  runat="server"
    OnClick="Storico"/>
<asp:Button ID="BRisposta" 
    Text="Invia Risposta"
    Visible="false"
    runat="server"/>
</div>
</div>
</div>
    <uc:NotifichePopup ID="NotifichePopup" runat="server" />
</asp:Content>