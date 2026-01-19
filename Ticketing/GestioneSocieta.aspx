<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="GestioneSocieta.aspx.cs" Inherits="Ticketing.GestioneSocieta" Title="Societa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    runat="Server">
    <div class="grid">
        <div class="col-50">

            <asp:Label ID="LNome"
                Text="Nome Societa"
                runat="server" />
            <asp:TextBox ID="TNome"
                runat="server"
                placeholder="Nome Societa">
            </asp:TextBox>
            <asp:Label ID="LIndirizzo"
                Text="Indirizzo"
                runat="server" />
            <asp:TextBox ID="TIndirizzo"
                runat="server"
                placeholder="Indirizzo">
            </asp:TextBox>
            <asp:Label ID="LCitta"
                Text="Citta"
                runat="server" />
            <asp:TextBox ID="TCitta"
                runat="server"
                placeholder="Citta">
            </asp:TextBox>
            <asp:Label ID="LCap"
                Text="Cap"
                runat="server" />
            <asp:TextBox ID="TCap"
                runat="server"
                placeholder="Cap">
            </asp:TextBox>
            <asp:Label ID="LTelefono"
                Text="Telefono"
                runat="server" />
            <asp:TextBox ID="TTelefono"
                runat="server"
                placeholder="Telefono">
            </asp:TextBox>
            <asp:Label ID="LeMail"
                Text="eMail"
                runat="server" />
            <asp:TextBox ID="TeMail"
                runat="server"
                placeholder="eMail">
            </asp:TextBox>
            <asp:Label ID="LPIva"
                Text="P. Iva"
                runat="server" />
            <asp:TextBox ID="TPIva"
                runat="server"
                placeholder="P. Iva">
            </asp:TextBox>
            <asp:Label ID="LNote"
                Text="Note"
                runat="server" />
            <asp:TextBox ID="TNote"
                runat="server"
                placeholder="Note"
                ClientIDMode="Static"
                TextMode="MultiLine">
            </asp:TextBox>
        </div>

            <div class ="col-50 gridvieww">
            
    <asp:GridView ID="rubricaSocieta" 
        runat="server"
        CellPadding="3"
        GridLines="None"
        HorizontalAlign="Center"
        DataKeyNames="ID"
        CssClass="gridvieww"
        AllowPaging="True"
        PageSize="20">
         <Columns>
     <asp:TemplateField HeaderText="Opzioni">
         <ItemTemplate>

             <div style="display: flex; justify-content: center; width: auto;">
                 <asp:LinkButton
                     ID="BtnApri"
                     runat="server"
                     CssClass="iconb"
                     ToolTip="Apri"
                     OnClick="clickMod"
                     >
                     <i class="fa fa-pencil-alt"></i>
                 </asp:LinkButton>

                 <asp:LinkButton
                     ID="BtnElimina"
                     runat="server"
                     CssClass="iconb elimina"
                    OnClick="clickElim"
                     ToolTip="Elimina" Text="&#xf1f8;">
                    <i class="fa-solid fa-trash"></i>
                 </asp:LinkButton>

             </div>
         </ItemTemplate>
     </asp:TemplateField>
 </Columns>
    </asp:GridView>
                
</div>

        <div class="grid-button">
            <asp:Button ID="BModifica"
                Text="Modifica"
                OnClick="clickModifica"
                runat="server" />
            <asp:Button ID="BCrea"
                Text="Crea"
                OnClick="clickCrea"
                runat="server" />
            <asp:Button ID="BElimina"
                Text="Annulla"
                OnClick="clickElimina"
                runat="server" />
        </div>

    

    </div> 
</asp:Content>
