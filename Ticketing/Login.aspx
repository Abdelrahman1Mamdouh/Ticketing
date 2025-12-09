<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ticketing.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:textbox ID="user"
                runat="server"
                placeholder="Username"></asp:textbox>
            <asp:TextBox runat="server"
                placeholder="Password"></asp:TextBox>
            <asp:Button ID="btnLogin" 
                Text="Invio"
                runat="server"/>
            <asp:Button ID="Button2" 
                Text="Annulla"
                runat="server"/>
            <asp:Label id="Label"
                text="Recupera Password"
                runat="server" />
        </div>
    </form>
</body>
</html>
