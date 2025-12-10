<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Ticketing.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    Runat="Server">
<div>
         <div>
     <asp:Table ID="Tickets"
         runat="server"
         CellPadding="10"
         GridLines="Both"
         HorizontalAlign="Center">
         <asp:TableRow>
             <asp:TableCell>
                Id
             </asp:TableCell>
             <asp:TableCell>
                 Cliente
            </asp:TableCell>
              <asp:TableCell>
                  Operatore
              </asp:TableCell>
              <asp:TableCell>
                   Priorita
             </asp:TableCell>
         </asp:TableRow>
     </asp:Table>

</div>
 <div>
      <asp:Button id="Crea"
        Text="Crea"
        
        runat="server"/>

       <asp:Button id="Storico"
         Text="Storico"
         
         runat="server"/>
 </div>
</div>
    </asp:Content>