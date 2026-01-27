<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Ticketing.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--<asp:HiddenField runat="server" ID="userid" Value="<%=user.ID %>" />--%>


    <asp:SqlDataSource runat="server" ID="SOC" ConnectionString="Server=localhost;Database=dgs;Uid=root;Pwd=" ProviderName="MySql.Data.MySqlClient"
        SelectCommand="SELECT ID, Nome as Societa FROM societa"></asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="LIV" ConnectionString="Server=localhost;Database=dgs;Uid=root;Pwd=" ProviderName="MySql.Data.MySqlClient"
        SelectCommand="SELECT ID,Livello FROM livello"></asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="STA" ConnectionString="Server=localhost;Database=dgs;Uid=root;Pwd=" ProviderName="MySql.Data.MySqlClient"
        SelectCommand="SELECT ID,Stato FROM stato"></asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="PRO" ConnectionString="Server=localhost;Database=dgs;Uid=root;Pwd=" ProviderName="MySql.Data.MySqlClient"
        SelectCommand="SELECT ID,Prodotto FROM prodotto"></asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="PRI" ConnectionString="Server=localhost;Database=dgs;Uid=root;Pwd=" ProviderName="MySql.Data.MySqlClient"
        SelectCommand="SELECT ID,Priorita FROM priorita"></asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="TEC" ConnectionString="Server=localhost;Database=dgs;Uid=root;Pwd=" ProviderName="MySql.Data.MySqlClient"
        SelectCommand="SELECT ID, concat(Nome, ' ', Cognome) as Tecnico from utente WHERE Ruolo=2"></asp:SqlDataSource>

    <div class="filtri">
        <asp:Button ID="Alltick" Text="Tutti" OnClick="AllTicket" Visible="false" runat="server" class="btn-filter" />
        <asp:Button ID="Mytick" Text="In Lavorazione..." OnClick="MyTicket" Visible="false" runat="server" class="btn-filter" />

        <asp:DropDownList ID="DTecnico"
            Visible="false"
            runat="server"
            DataTextField="Tecnico"
            DataValueField="Tecnico"
            DataSourceID="TEC"
            AppendDataBoundItems="true">
        </asp:DropDownList>

        <asp:DropDownList ID="DLivello"
            Visible="false"
            runat="server"
            DataTextField="Livello"
            DataValueField="Livello"
            DataSourceID="LIV"
            AppendDataBoundItems="true">
        </asp:DropDownList>

        <asp:DropDownList ID="DStato"
            Visible="false"
            runat="server"
            DataTextField="Stato"
            DataValueField="Stato"
            DataSourceID="STA"
            AppendDataBoundItems="true">
        </asp:DropDownList>

        <asp:DropDownList ID="DPriorita"
            Visible="false"
            runat="server"
            DataTextField="Priorita"
            DataValueField="Priorita"
            DataSourceID="PRI"
            AppendDataBoundItems="true">
        </asp:DropDownList>

        <asp:DropDownList ID="DSocieta"
            Visible="false"
            runat="server"
            DataTextField="Societa"
            DataValueField="Societa"
            DataSourceID="SOC"
            AppendDataBoundItems="true">
        </asp:DropDownList>

        <asp:DropDownList ID="DProdotto"
            Visible="false"
            runat="server"
            DataTextField="Prodotto"
            DataValueField="Prodotto"
            DataSourceID="PRO"
            AppendDataBoundItems="true">
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
            </div>
        </div>
    </div>


    <div class="box-gridview" style="max-height: 500px; overflow-y: auto;">
        <asp:GridView ID="Tickets"
            runat="server"
            DataKeyNames="ID"
            GridLines="None"
            HorizontalAlign="Center"
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

        <%--<asp:GridView runat="server" ID="prova" DataSourceID="dsTest" DataKeyNames="ID" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="true" PageSize="5">
        </asp:GridView>
        <asp:SqlDataSource runat="server" ID="dsTest" ConnectionString="Server=localhost;Database=dgs;Uid=root;Pwd=" ProviderName="MySql.Data.MySqlClient"
            SelectCommand="SELECT ID,Ruolo, Societa, Livello, Dipartimento FROM utenti">
            <SelectParameters>
                <asp:ControlParameter ControlID="userid" Name="id" PropertyName="Value" DefaultValue="-1" DbType="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
    </div>
</asp:Content>
