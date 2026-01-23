<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotifichePopup.ascx.cs" Inherits="Ticketing.Controls.NotifichePopup" %>

<asp:Panel ID="pnlNotifichePopup" runat="server" CssClass="modal" Style="display: none;">
    <div class="modal-content">


        <button type="button" class="modal-close" onclick="document.getElementById('<%= pnlNotifichePopup.ClientID %>').style.display='none'">×</button>

        <div class="grid">
            <asp:GridView ID="gvNotifichePopup" runat="server" AutoGenerateColumns="False"
                CellPadding="3" GridLines="None" HorizontalAlign="Center"
                AllowPaging="True" PageSize="100" CssClass="gridview">
                <Columns>
                    <asp:TemplateField HeaderText="Ticket #">
                        <ItemTemplate>
                            <asp:LinkButton
                                ID="btnTicket"
                                runat="server"
                                Text='<%# Eval("TicketID") %>'
                                CommandName="SelectTicket"
                                CommandArgument='<%# Eval("TicketID") %>'
                                OnClick="ClickSelectTicket"
                                CssClass="btn-viola" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:BoundField DataField="TicketID" HeaderText="Ticket #" />--%>
                    <asp:BoundField DataField="Mittente" HeaderText="Da" />
                    <asp:BoundField DataField="Messaggio" HeaderText="Messaggio" />
                    <asp:TemplateField HeaderText="Stato">
                        <ItemTemplate>
                            <%# Eval("letturaNotifica").ToString() == "1" ? "✅ Letta" : "✉️ Nuova" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </div>
</asp:Panel>

<style>
    .modal {
        position: fixed;
        z-index: 10000;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        overflow: auto;
        background-color: rgba(0,0,0,0.5);
        display: none;
    }

    .modal-content {
        background-color: #fff;
        margin: 5% auto;
        padding: 20px;
        width: 90%;
        max-width: 800px;
        border-radius: 4px;
        box-shadow: 0 4px 8px rgba(0,0,0,0.2);
    }

    .modal-close {
        float: right;
        font-size: 24px;
        border: none;
        background: transparent;
        cursor: pointer;
        font-weight: bold;
    }
</style>
