<%@ Page Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="GestioneUtenti.aspx.cs"
    Inherits="Ticketing.GestioneUtenti" Title="Gestione Utenti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    
    <div class="grid_system">

        <div class="col-utenti-50">
            <asp:Label ID="LNome" Text="Nome" runat="server" />
            <asp:TextBox ID="TNome" runat="server" placeholder="Nome"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RfVNome" runat="server" ControlToValidate="TNome" 
                ErrorMessage="Nome obbligatorio" ForeColor="Red" Display="Dynamic" ValidationGroup="UserForm"/>

            <asp:Label ID="LCognome" Text="Cognome" runat="server" />
            <asp:TextBox ID="TCognome" runat="server" placeholder="Cognome"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RfVCognome" runat="server" ControlToValidate="TCognome" 
                ErrorMessage="Cognome obbligatorio" ForeColor="Red" Display="Dynamic" ValidationGroup="UserForm"/>
            
            <asp:Panel ID="PnlRuolo" runat="server">
                <asp:Label ID="LRuolo" Text="Ruolo" runat="server" /><br />
                <asp:DropDownList ID="DRuolo" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="DRuolo_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RfVRuolo" runat="server" ControlToValidate="DRuolo" 
                    InitialValue="0" ErrorMessage="Seleziona un ruolo" ForeColor="Red" Display="Dynamic" ValidationGroup="UserForm"/>
            </asp:Panel>

            <asp:Label ID="LSocieta" Text="Societa" runat="server" />
            <asp:DropDownList ID="DSocieta" runat="server" AutoPostBack="false"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="RfVSocieta" runat="server" 
                ControlToValidate="DSocieta" 
                InitialValue="0" 
                ErrorMessage="Società obbligatoria" 
                ForeColor="Red" 
                Display="Dynamic" 
                ValidationGroup="UserForm"/>

            <asp:Panel ID="PnlLivello" runat="server">
                <asp:Label ID="LLivello" Text="Livello" runat="server" /><br />
                <asp:DropDownList ID="DLivello" runat="server" AutoPostBack="false"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RfVLivello" runat="server" 
                    ControlToValidate="DLivello" 
                    InitialValue="0" 
                    ErrorMessage="Livello obbligatorio" 
                    ForeColor="Red" 
                    Display="Dynamic"
                    ValidationGroup="UserForm"/>
            </asp:Panel>

            <asp:Panel ID="PnlDipartimento" runat="server">
                <asp:Label ID="LDipartimento" Text="Dipartimento" runat="server" /><br />
                <asp:DropDownList ID="DDipartimento" runat="server" AutoPostBack="false"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RfVDipartimento" runat="server" 
                    ControlToValidate="DDipartimento" 
                    InitialValue="0" 
                    ErrorMessage="Dipartimento obbligatorio" 
                    ForeColor="Red" 
                    Display="Dynamic" 
                    ValidationGroup="UserForm"/>
            </asp:Panel>
        </div>

        <div class="col-utenti-50">
            <asp:Label ID="LPassword" Text="Password" runat="server" />
            <asp:TextBox ID="TPassword" runat="server" placeholder="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RfVPassword" runat="server" ControlToValidate="TPassword" 
                ErrorMessage="Password obbligatoria" ForeColor="Red" Display="Dynamic" ValidationGroup="UserForm"/>

            <asp:Label ID="LEmail" Text="Email" runat="server" />
            <asp:TextBox ID="TEmail" runat="server" placeholder="Email"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RfVEmail" runat="server" ControlToValidate="TEmail" 
                ErrorMessage="Email obbligatoria" ForeColor="Red" Display="Dynamic" ValidationGroup="UserForm"/>
            <asp:RegularExpressionValidator ID="RevEmail" runat="server" ControlToValidate="TEmail" 
                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" 
                ErrorMessage="Email non valida" ForeColor="Red" Display="Dynamic" ValidationGroup="UserForm"/>

            <asp:Label ID="LTelefono" Text="Telefono" runat="server" />
            <asp:TextBox ID="TTelefono" runat="server" placeholder="Telefono"></asp:TextBox>
             <asp:RegularExpressionValidator ID="RevPhone" runat="server" ControlToValidate="TTelefono" 
                ValidationExpression="^\+[1-9]\d{1,14}$" 
                ErrorMessage="Formato internazionale richiesto (es. +39...)" 
                ForeColor="Red" Display="Dynamic" ValidationGroup="UserForm"/>
        </div>

        <div class="box-pulsanti-utenti mt-1 mb-1">
            <asp:Button ID="BCrea" Text="Crea" runat="server" OnClick="HandleButtonClick" ValidationGroup="UserForm" class="btn-viola"/>
            <asp:Button ID="BCancel" Text="Annulla" runat="server" OnClick="clickAnnulla" Visible="false" CausesValidation="false" class="btn-annulla"/>
        </div>

    </div> 

    <hr class="mt-2 mb-2" />

   
    <div class="grid-view">
        <div class="box-gridview">
            <asp:GridView ID="rubricaUtenti" runat="server"
                AutoGenerateColumns="False" 
                DataKeyNames="ID"
                CssClass="gridview"
                GridLines="None"
                AllowPaging="True"
                PageSize="20"
                
                >

                <Columns>
    <asp:TemplateField HeaderText="Opzioni">
        <ItemTemplate>
            <div class="icon-container">
                <asp:LinkButton ID="BtnApri" runat="server" CssClass="iconb" OnClick="clickModifica" ToolTip="Modifica">
                    <i class="fa fa-pencil-alt"></i>
                </asp:LinkButton>
                <asp:LinkButton ID="BtnElimina" runat="server" CssClass="iconb elimina" OnClick="clickElimina" 
                    OnClientClick="return confirm('Sei sicuro?');" ToolTip="Elimina">
                    <i class="fa-solid fa-trash"></i>
                </asp:LinkButton>
            </div>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:BoundField DataField="ID" HeaderText="ID" />
    <asp:BoundField DataField="utente" HeaderText="Utente" />
    <asp:BoundField DataField="Ruolo" HeaderText="Ruolo" />
    <asp:BoundField DataField="Societa" HeaderText="Soc." />
    <asp:BoundField DataField="Livello" HeaderText="Liv." />
    <asp:BoundField DataField="Dipartimento" HeaderText="Dip." />
    <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
    <asp:BoundField DataField="Email" HeaderText="Email" />
</Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>