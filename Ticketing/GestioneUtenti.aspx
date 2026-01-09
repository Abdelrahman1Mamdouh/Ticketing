<%@ Page Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="GestioneUtenti.aspx.cs" 
    Inherits="Ticketing.GestioneUtenti" Title="Gestione Utenti"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    Runat="Server">

    <div class="grid">

    <div class="col-50">

<asp:Label ID="LNome" Text="Nome" runat="server"/>
<asp:TextBox ID="TNome" runat="server" placeholder="Nome"></asp:TextBox>

<asp:Label ID="LCognome" Text="Cognome" runat="server"/>
<asp:TextBox ID="TCognome" runat="server" placeholder="Cognome"></asp:TextBox>

    <asp:Panel ID="PnlRuolo" runat="server">
        <asp:Label ID="LRuolo" Text="Ruolo" runat="server"/><br />
        <asp:DropDownList ID="DRuolo" runat="server" AutoPostBack="false"></asp:DropDownList>
    </asp:Panel>
        
        <asp:Label ID="LSocieta" Text="Societa" runat="server"/>
        <asp:DropDownList ID="DSocieta" runat="server" AutoPostBack="false"></asp:DropDownList>

    <asp:Panel ID="PnlLivello" runat="server">
        <asp:Label ID="LLivello" Text="Livello" runat="server"/><br />
        <asp:DropDownList ID="DLivello" runat="server" AutoPostBack="false"></asp:DropDownList>
    </asp:Panel>

    <asp:Panel ID="PnlDipartimento" runat="server">
        <asp:Label ID="LDipartimento" Text="Dipartimento" runat="server"/><br />
        <asp:DropDownList ID="DDipartimento" runat="server" AutoPostBack="false"></asp:DropDownList>
    </asp:Panel>
    </div>

<div class="col-50">
    <asp:Label ID="LPassword"
    Text="Password"
    runat="server"/>
<asp:TextBox ID="TPassword"
    runat="server"
    placeholder="Password">
</asp:TextBox>
    <asp:Label ID="LEmail"
    Text="Email"
    runat="server"/>
<asp:TextBox ID="TEmail"
    runat="server"
    placeholder="Email">
</asp:TextBox>
<asp:Label ID="LTelefono"
    Text="Telefono"
    runat="server"/>
<asp:TextBox ID="TTelefono"
    runat="server"
    placeholder="Telefono">
</asp:TextBox>

        </div>

<div class="grid-button">
<asp:Button ID="BModifica"
    Text="Modifica"
    runat="server"
    OnClick="clickModifica"/>
<asp:Button ID="BCrea"
    Text="Crea"
    runat="server"
    OnClick="clickCrea"/>
<asp:Button ID="BElimina"
    Text="Elimina"
    runat="server"
    OnClick="clickElimina"/>
    </div>



</div>
</asp:Content>