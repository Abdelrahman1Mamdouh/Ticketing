<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="GestioneTicket.aspx.cs" Inherits="Ticketing.GestioneTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    Runat="Server">
    <div>
    <div class="grid">
    <div class="col-50">

<asp:Label ID="LProdotto"
    Text="Prodotto"
    runat="server"/>
<asp:DropDownList ID="DProdotto"
    runat="server">
        <asp:ListItem>Prodotto1</asp:ListItem>
        <asp:ListItem>Prodotto2</asp:ListItem>
        <asp:ListItem>Prodotto3</asp:ListItem>
</asp:DropDownList>
<asp:Label ID="LCategoria"
    Text="Categoria"
    runat="server"/>

<asp:DropDownList ID="DCategoria"
    runat="server">
        <asp:ListItem>Categoria1</asp:ListItem>
        <asp:ListItem>Categoria2</asp:ListItem>
        <asp:ListItem>Categoria3</asp:ListItem>
</asp:DropDownList>

        </div>

        <div class="col-50">
<asp:Label ID="LOggetto"
    Text="Oggetto"
    runat="server"/>
<asp:TextBox ID="TOggetto"
    runat="server"
    placeholder="Oggetto">
</asp:TextBox>
<asp:Label ID="LMessaggio"
    Text="Messaggio"
    runat="server"/>
<asp:TextBox ID="TMessaggio"
    runat="server"
    placeholder="Messaggio"
     ClientIDMode="Static"
    TextMode="MultiLine">
</asp:TextBox>

</div>

<div class="grid-button">
<asp:Button ID="BCrea" 
    Text="Crea"
    OnClick="clickCrea"
    runat="server"/>

</div>

</div>
</div>
</asp:Content>