<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Ticketing.Dashboard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-100">
        <div class="grid">
            <div style="width: 100%; height: 80%; overflow-x: scroll;">
                <asp:GridView ID="Tickets" 
    runat="server"
    
    GridLines="None" 
    HorizontalAlign="Center" 
    AllowPaging="True" 
    PageSize="20"
    CssClass="gridvieww"
    AutoGenerateColumns="True"
    > 
                    

    <Columns>
        <asp:TemplateField HeaderText="Opzioni">
            <ItemTemplate>
                <div style="display:flex;justify-content:center;width:auto;">
               <button class="iconb" title="Apri">
                    <i class="fa fa-eye"></i>
                </button>

                <button class="iconb" title="Modifica">
                    <i class="fa fa-pencil-alt"></i>
                </button>

                <button class="iconb elimina" title="Elimina">
                    <i class="fa fa-trash"></i>
                </button>
                    </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
            </div>
            
            <div class="grid-button">
                <asp:Button ID="BCrea" Text="Crea" runat="server" />
               
            </div>
        </div>
    </div>
</asp:Content>
