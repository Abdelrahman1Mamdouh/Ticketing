<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestioneTicket.aspx.cs" Inherits="Ticketing.GestioneTicket" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Ticket</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="display: flex; flex-direction: column;">

            <asp:Label ID="LabelId"
                Text="Id"
                runat="server"/>
              <asp:TextBox ID="TextBoxId"
                  runat="server"
                  placeholder="Id">
              </asp:TextBox>
            <asp:Label ID="LabelCliente"
                Text="Cliente"
                runat="server"/>
            <asp:TextBox ID="TextBoxCliente"
                runat="server"
                placeholder="Cliente">
            </asp:TextBox>
            <asp:Label ID="LabelTecnico"
                Text="Tecnico"
                runat="server"/>
            <asp:TextBox ID="TextBoxTecnico"
                runat="server"
                placeholder="Tecnico">
            </asp:TextBox>
            <asp:Label ID="LabelLivello"
                Text="Livello"
                runat="server"/>
            <asp:TextBox ID="TextBoxLivello"
                runat="server"
                placeholder="Livello">
            </asp:TextBox>
            <asp:Label ID="LabelStato"
                Text="Stato"
                runat="server"/>
            <asp:DropDownList ID="ListaStato"
                runat="server">
                    <asp:ListItem>Item1</asp:ListItem>
                    <asp:ListItem>Item2</asp:ListItem>
                    <asp:ListItem>Item3</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="LabelProdotto"
                Text="Prodotto"
                runat="server"/>
            <asp:DropDownList ID="DropDownListProdotto"
                runat="server">
                    <asp:ListItem>Prodotto1</asp:ListItem>
                    <asp:ListItem>Prodotto2</asp:ListItem>
                    <asp:ListItem>Prodotto3</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="LabelCategoria"
                Text="Categoria"
                runat="server"/>
            <asp:DropDownList ID="DropDownListCategoria"
                runat="server">
                    <asp:ListItem>Categoria1</asp:ListItem>
                    <asp:ListItem>Categoria2</asp:ListItem>
                    <asp:ListItem>Categoria3</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="LabelOggetto"
                Text="Oggetto"
                runat="server"/>
            <asp:TextBox ID="TextBoxOggetto"
                runat="server"
                placeholder="Oggetto">
            </asp:TextBox>
            <asp:Label ID="LabelMessaggio"
                Text="Messaggio"
                runat="server"/>
            <asp:TextBox ID="TextBoxMessaggio"
                runat="server"
                placeholder="Messaggio">
            </asp:TextBox>
            <asp:Button ID="CreaMesaggio" 
                Text="Crea"
                runat="server"/>
            <asp:Button ID="ModificaMessaggio" 
                Text="Modifica"
                runat="server"/>
            <asp:Button ID="EliminaMessaggio" 
                Text="Elimina"
                runat="server"/>
             <asp:Label ID="LabelPriorita"
                Text="Priorita"
                runat="server"/>
            <asp:DropDownList ID="DropDownListPriorit"
                runat="server">
                    <asp:ListItem>Priorita1</asp:ListItem>
                    <asp:ListItem>Priorita2</asp:ListItem>
                    <asp:ListItem>Priorita3</asp:ListItem>
            </asp:DropDownList>
             <asp:Label ID="LabelComunicazione"
                Text="Comunicazione"
                runat="server"/>
            <asp:TextBox ID="TextBoxComunicazione"
                runat="server"
                placeholder="Comunicazione">
            </asp:TextBox>
            <asp:Button ID="ButtonStorico" 
                Text="Storico"
                runat="server"/>
            <asp:Button ID="ButtonInviaRisposta" 
                Text="Invia Risposta"
                runat="server"/>
        </div>
    </form>
</body>
</html>
