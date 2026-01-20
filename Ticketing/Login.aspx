<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ticketing.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="login-page">
    <div class="box-login login-card">
        <div class="box-icona">
            <img src="Asset/user.png"  class="icon-resize"/>
        </div>
        <asp:Label
            Text="Effettua il Login:"
            runat="server" />

        <asp:TextBox ID="TUser"
            runat="server"
            placeholder="Username"
            class="user">

        </asp:TextBox>
        <asp:TextBox ID="TPass"
            runat="server"
            type="password"
            placeholder="Password"
            class="password">

        </asp:TextBox>


        <div class="box-pulsanti-login">
            <asp:Button ID="BLogin"
                Text="Login"
                OnClick="BtnLogin_Click"
                runat="server" 
                class="btn-viola"/>

            <asp:Button ID="BAnnulla"
                Text="Annulla"
                OnClick="BtnAnnulla_Click"
                runat="server" 
                class="btn-viola"/>
            <asp:Button ID="BRP"
                Text="Recupera Password"
                runat="server"
                class="btn-recupera" 
                />
        </div>


    </div>
</div>

</asp:Content>
