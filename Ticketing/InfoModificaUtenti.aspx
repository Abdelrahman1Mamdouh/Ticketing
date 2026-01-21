<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InfoModificaUtenti.aspx.cs" Inherits="Ticketing.InfoModificaUtenti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    runat="Server">

    <div class="grid">
        <div class="col-50">
            <asp:Label ID="LNome"
                Text="Nome"
                runat="server" />
            <asp:TextBox ID="TNome"
                runat="server"
                placeholder="Nome">
            </asp:TextBox>
            <asp:Label ID="LCognome"
                Text="Cognome"
                runat="server" />
            <asp:TextBox ID="TCognome"
                runat="server"
                placeholder="Cognome">
            </asp:TextBox>
            <asp:Label ID="LTelefono"
                Text="Telefono"
                runat="server" />
            <asp:TextBox ID="TTelefono"
                runat="server"
                placeholder="Telefono">
            </asp:TextBox>
            <asp:Label ID="LEmail"
                Text="Email"
                runat="server" />
            <asp:TextBox ID="TEmail"
                runat="server"
                placeholder="Email">
            </asp:TextBox>
            <asp:Label ID="LPassword"
                Text="Password"
                runat="server" />
            <asp:TextBox ID="TPassword"
                runat="server"
                placeholder="Password">
            </asp:TextBox>
        </div>
    </div>
    <div class="grid-button">
        <asp:Button ID="BSalvaModifiche"
            Text="Salva Modifica"
            runat="server" 
            OnClick="clickSalvaModifiche"
            class="btn-viola"/>
    </div>
</asp:Content>

