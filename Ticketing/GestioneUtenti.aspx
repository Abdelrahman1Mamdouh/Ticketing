<%@ Page Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="GestioneUtenti.aspx.cs"
    Inherits="Ticketing.GestioneUtenti" Title="Gestione Utenti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    runat="Server">

    <div class="grid_system">

        <div class="col-utenti-50">

            <asp:Label ID="LNome" Text="Nome" runat="server" />
            <asp:TextBox ID="TNome" runat="server" placeholder="Nome"></asp:TextBox>

            <asp:Label ID="LCognome" Text="Cognome" runat="server" />
            <asp:TextBox ID="TCognome" runat="server" placeholder="Cognome"></asp:TextBox>

            <asp:Panel ID="PnlRuolo" runat="server">
                <asp:Label ID="LRuolo" Text="Ruolo" runat="server" /><br />
                <asp:DropDownList ID="DRuolo" runat="server" AutoPostBack="false" class="select"></asp:DropDownList>
            </asp:Panel>

            <asp:Label ID="LSocieta" Text="Societa" runat="server" />
            <asp:DropDownList ID="DSocieta" runat="server" AutoPostBack="false" class="select"></asp:DropDownList>

            <asp:Panel ID="PnlLivello" runat="server">
                <asp:Label ID="LLivello" Text="Livello" runat="server" /><br />
                <asp:DropDownList ID="DLivello" runat="server" AutoPostBack="false" class="select"></asp:DropDownList>
            </asp:Panel>

            <asp:Panel ID="PnlDipartimento" runat="server">
                <asp:Label ID="LDipartimento" Text="Dipartimento" runat="server" /><br />
                <asp:DropDownList ID="DDipartimento" runat="server" AutoPostBack="false" class="select"></asp:DropDownList>
            </asp:Panel>
        </div>

        <div class="col-utenti-50">
            <asp:Label ID="LPassword"
                Text="Password"
                runat="server" />
            <asp:TextBox ID="TPassword"
                runat="server"
                placeholder="Password">
            </asp:TextBox>
            <asp:Label ID="LEmail"
                Text="Email"
                runat="server" />
            <asp:TextBox ID="TEmail"
                runat="server"
                placeholder="Email">
            </asp:TextBox>
            <asp:Label ID="LTelefono"
                Text="Telefono"
                runat="server" />
            <asp:TextBox ID="TTelefono"
                runat="server"
                placeholder="Telefono">
            </asp:TextBox>

        </div>

        <div class="box-pulsanti-utenti mt-1 mb-1">
            <asp:Button ID="BModifica"
                Text="Modifica"
                runat="server"
                OnClick="clickModifica" 
                class="btn-modifica"/>
            <asp:Button ID="BCrea"
                Text="Crea"
                runat="server"
                OnClick="clickCrea" 
                class="btn-viola"/>
            <asp:Button ID="BElimina"
                Text="Elimina"
                runat="server"
                OnClick="clickElimina"
                class="btn-annulla"/>
        </div>


        <div class="grid-view">
            <div class="box-gridview" style="max-height: 500px; overflow-y: auto;">
                <asp:GridView ID="rubricaUtenti"
                    runat="server"
                    CellPadding="3"
                    GridLines="None"
                    HorizontalAlign="Center"
                    DataKeyNames="ID"
                    
                    CssClass="gridview"
                    >

                    <Columns>
                        <asp:TemplateField HeaderText="Opzioni">
                            <ItemTemplate>
                                <div style="display: flex; justify-content: center; width: auto;">
                                    <asp:LinkButton
                                        ID="BtnApri"
                                        runat="server"
                                        CssClass="iconb"
                                        ToolTip="Apri"
                                        OnClick="clickModifica"
                                        Text="&#xf06e;">
                                        <i class="fa fa-pencil-alt"></i>
                                    </asp:LinkButton>

                                    <asp:LinkButton
                                        ID="BtnElimina"
                                        runat="server"
                                        CssClass="iconb elimina"
                                        OnClick="clickElimina"
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
        </div>

    </div>


</asp:Content>
