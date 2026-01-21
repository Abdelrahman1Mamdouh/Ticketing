<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Ticketing.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

   

        <div class="filtri">
            <asp:Button ID="Alltick" Text="Tutti" OnClick="AllTicket" Visible="false" runat="server" class="btn-filter"/>
            <asp:Button ID="Mytick" Text="In Lavorazione..." OnClick="MyTicket" Visible="false" runat="server" class="btn-filter"/>


            <asp:DropDownList ID="DTecnico"
                Visible="false"
                runat="server"
                DataTextField="Tecnico"
                DataValueField="Tecnico">
            </asp:DropDownList>


            <asp:DropDownList ID="DLivello"
                Visible="false"
                runat="server"
                DataTextField="Livello"
                DataValueField="Livello">
            </asp:DropDownList>


            <asp:DropDownList ID="DStato"
                Visible="false"
                runat="server"
                DataTextField="Stato"
                DataValueField="Stato">
            </asp:DropDownList>


            <asp:DropDownList ID="DPriorita"
                Visible="false"
                runat="server"
                DataTextField="Priorita"
                DataValueField="Priorita">
            </asp:DropDownList>


            <asp:DropDownList ID="DSocieta"
                Visible="false"
                runat="server"
                DataTextField="Societa"
                DataValueField="Societa">
            </asp:DropDownList>


            <asp:DropDownList ID="DProdotto"
                Visible="false"
                runat="server"
                DataTextField="Prodotto"
                DataValueField="Prodotto">
            </asp:DropDownList>
            <asp:Button ID="BVedi" Text="Filtra" OnClick="MixTicket" Visible="false" runat="server" class="btn-filter" />

        </div>

        <div class="mt-1 mb-1">
            <asp:Button ID="BCrea" Text="New Ticket" OnClick="CreateTicket" Visible="false" runat="server" class="btn-viola" />
    <div class="col-100">
        <div class="grid">
            <div style="width: 100%; height: 80%; overflow-x: scroll;">
                <%--<asp:GridView ID="Ticketss"
                    runat="server"
                    DataKeyNames="ID"
                    GridLines="None"
                    HorizontalAlign="Center"
                    AllowPaging="True"
                    PageSize="100"
                    CssClass="gridvieww"
                    AutoGenerateColumns="True">

                    <Columns>
                        <asp:TemplateField HeaderText="Opzioni">
                            <ItemTemplate>
                                <div style="display: flex; justify-content: center; width: auto;">
                                    <asp:LinkButton
                                        ID="BtnApri"
                                        runat="server"
                                        CssClass="iconb"
                                        ToolTip="Apri"
                                        OnClick="ClickSelectTicket"

                                        Text="&#xf06e;">    
                                        <i class="fa fa-pencil-alt"></i>
                                    </asp:LinkButton>

                                    <asp:LinkButton
                                        ID="BtnElimina"
                                        runat="server"
                                        CssClass="iconb elimina"
                                        OnClick="ClickDeleteTicket"
                                        ToolTip="Elimina" Text="&#xf1f8;">
                                       <i class="fa-solid fa-trash"></i>
                                    </asp:LinkButton>

                                    <%--<asp:Button ID="BtnElimina" runat="server" CssClass="iconb elimina" ToolTip="Elimina" Text="&#xf1f8;" />--%>
                             <%--   </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>--%>
           
        </div>
    


    <div class="box-gridview">
        <asp:GridView ID="Tickets"
            runat="server"
            DataKeyNames="ID"
            GridLines="None"
            HorizontalAlign="Center"
            AllowPaging="True"
            PageSize="100"
            CssClass="gridview"
            AutoGenerateColumns="True">

            <Columns>
                <asp:TemplateField HeaderText="Opzioni">
                    <ItemTemplate>
                        <div style="display: flex; justify-content: center; width: auto;">
                            <asp:LinkButton
                                ID="BtnApri"
                                runat="server"
                                CssClass="iconb"
                                ToolTip="Apri"
                                OnClick="ClickSelectTicket"
                                Text="&#xf06e;">
                        <i class="fa fa-pencil-alt"></i>
                            </asp:LinkButton>

                            <asp:LinkButton
                                ID="BtnElimina"
                                runat="server"
                                CssClass="iconb elimina"
                                OnClick="ClickDeleteTicket"
                                ToolTip="Elimina" Text="&#xf1f8;">
                       <i class="fa-solid fa-trash"></i>
                            </asp:LinkButton>

                            <%--<asp:Button ID="BtnElimina" runat="server" CssClass="iconb elimina" ToolTip="Elimina" Text="&#xf1f8;" />--%>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
