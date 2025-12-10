<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="GestioneTicket.aspx.cs" Inherits="Ticketing.GestioneTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    Runat="Server">
    <div style="display: flex; flex-direction: column;">
        <asp:Label ID="Label1"
    Text="Id"
    runat="server"/>
  <asp:TextBox ID="TextBox1"
      runat="server"
      placeholder="Id">
  </asp:TextBox>
<asp:Label ID="Label2"
    Text="Cliente"
    runat="server"/>
<asp:TextBox ID="TextBox2"
    runat="server"
    placeholder="Cliente">
</asp:TextBox>
<asp:Label ID="Label3"
    Text="Tecnico"
    runat="server"/>
<asp:TextBox ID="TextBox3"
    runat="server"
    placeholder="Tecnico">
</asp:TextBox>
<asp:Label ID="Label4"
    Text="Livello"
    runat="server"/>
<asp:TextBox ID="TextBox4"
    runat="server"
    placeholder="Livello">
</asp:TextBox>
<asp:Label ID="Label5"
    Text="Stato"
    runat="server"/>
<asp:DropDownList ID="DropDownList1"
    runat="server">
        <asp:ListItem>Item1</asp:ListItem>
        <asp:ListItem>Item2</asp:ListItem>
        <asp:ListItem>Item3</asp:ListItem>
</asp:DropDownList>
<asp:Label ID="Label6"
    Text="Prodotto"
    runat="server"/>
<asp:DropDownList ID="DropDownList2"
    runat="server">
        <asp:ListItem>Prodotto1</asp:ListItem>
        <asp:ListItem>Prodotto2</asp:ListItem>
        <asp:ListItem>Prodotto3</asp:ListItem>
</asp:DropDownList>
<asp:Label ID="Label7"
    Text="Categoria"
    runat="server"/>
<asp:DropDownList ID="DropDownList3"
    runat="server">
        <asp:ListItem>Categoria1</asp:ListItem>
        <asp:ListItem>Categoria2</asp:ListItem>
        <asp:ListItem>Categoria3</asp:ListItem>
</asp:DropDownList>
<asp:Label ID="Label8"
    Text="Oggetto"
    runat="server"/>
<asp:TextBox ID="TextBox5"
    runat="server"
    placeholder="Oggetto">
</asp:TextBox>
<asp:Label ID="Label9"
    Text="Messaggio"
    runat="server"/>
<asp:TextBox ID="TextBox6"
    runat="server"
    placeholder="Messaggio">
</asp:TextBox>
<asp:Button ID="Button1" 
    Text="Crea"
    runat="server"/>
<asp:Button ID="Button2" 
    Text="Modifica"
    runat="server"/>
<asp:Button ID="Button3" 
    Text="Elimina"
    runat="server"/>
 <asp:Label ID="Label10"
    Text="Priorita"
    runat="server"/>
<asp:DropDownList ID="DropDownList4"
    runat="server">
        <asp:ListItem>Priorita1</asp:ListItem>
        <asp:ListItem>Priorita2</asp:ListItem>
        <asp:ListItem>Priorita3</asp:ListItem>
</asp:DropDownList>
 <asp:Label ID="Label11"
    Text="Comunicazione"
    runat="server"/>
<asp:TextBox ID="TextBox7"
    runat="server"
    placeholder="Comunicazione">
</asp:TextBox>
<asp:Button ID="Button4" 
    Text="Storico"
    runat="server"/>
<asp:Button ID="Button5" 
    Text="Invia Risposta"
    runat="server"/>
</div>
</asp:Content>