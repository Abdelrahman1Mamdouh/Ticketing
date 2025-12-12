<%@ Page Language="C#"  MasterPageFile="~/Site.master"  AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ticketing.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        
    
    <div class="grid">
            <div class="col-login ">
            <div class="box-login">

                <asp:Label
                    Text="Effettua il Login:"
                    runat="server" />

            <asp:textbox ID="TUser"
                runat="server"
                placeholder="Username"
                class="user">

            </asp:textbox>
            <asp:TextBox ID="TPass"
                runat="server"
                type="password"
                placeholder="Password"
                class="password">

            </asp:TextBox>
                <div class="grid-button-login grid-button-login">
                <asp:Button ID="BLogin"
                    Text="Invio"
                    OnClick="BtnLogin_Click"
                    runat="server" />

                <asp:Button ID="BAnnulla"
                    Text="Annulla"
                    OnClick="BtnAnnulla_Click"
                    runat="server" />
                <asp:Button ID="BRP"
                    Text="Recupera Password"
                    runat="server" 
                    class="btn-recupera"/>
                </div>

            </div>
            </div>
        </div>
    </asp:Content>
