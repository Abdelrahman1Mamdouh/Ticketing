<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Ticketing.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    Runat="Server">
<div class="grid">
        <div class="col-100">
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
 <div class="grid-button">

   

      <asp:Button id="BCrea"
        Text="Crea"
        
        runat="server"/>

       <asp:Button id="BStorico"
         Text="Storico"
         
         runat="server"/>
 </div>
</div>
    </asp:Content>