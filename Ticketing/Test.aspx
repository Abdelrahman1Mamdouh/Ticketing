<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Ticketing.Test" MasterPageFile="~/Site.Master" %>
<asp:Content ID="content" runat="server" ContentPlaceHolderID="MainContent">

    <asp:SqlDataSource runat="server" ID="dsTest" ConnectionString="Server=localhost;Database=dgs;Uid=root;Pwd=" ProviderName="MySql.Data.MySqlClient"
        SelectCommand="SELECT ID,Ruolo, Societa, Livello, Dipartimento FROM utenti">
       
    
       <%-- <SelectParameters>
            <asp:ControlParameter ControlID="userid" Name="id" PropertyName="Value" DefaultValue="-1" DbType="Int32" />
        </SelectParameters>--%>
    </asp:SqlDataSource>

     <asp:SqlDataSource runat="server" ID="R" ConnectionString="Server=localhost;Database=dgs;Uid=root;Pwd=" ProviderName="MySql.Data.MySqlClient"
         SelectCommand="SELECT ID, Ruolo FROM ruolo">
     </asp:SqlDataSource>

    <asp:GridView runat="server" 
        ID="prova" 
        DataSourceID="dsTest"
        DataKeyNames="ID"
        AllowPaging="true"
        AllowSorting="true"
        PageSize="5"
        AutoGenerateColumns="false">

        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
            <asp:BoundField HeaderText="Ruolo" DataField="Ruolo" />
            <asp:TemplateField>
                <ItemTemplate>
                    <ul>
                        <li><%#Eval("Societa")%></li>
                        <li><%#Eval("Livello")%></li>
                        <li><%#Eval("Dipartimento")%></li>
                        
                        
                    </ul>
                </ItemTemplate>
             
            </asp:TemplateField>
        </Columns>
       
    </asp:GridView>
    <asp:DropDownList id="ddTest"
     runat="server"
     DataSourceID="R"
     DataValueField="ID" 
     DataTextField="Ruolo">
    </asp:DropDownList>

    <div>
       
    </div>
</asp:Content>
