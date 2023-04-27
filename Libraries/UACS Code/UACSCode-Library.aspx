<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UACSCode-Library.aspx.vb" Inherits="DbVoucher_Mark1.UACSCode_Library" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="headerlib"></div>
    <h2 class="page-title">UACS Funding Source Codes: <span>List of all UACS Funding Source Codes and Description</span></h2>

    <div class="float-right" style="margin: 5px 0px;">
        <asp:Button runat="server" ID="btnAdd" Text="Add New Funding Source Code" />
    </div>

    <div class="float-left">
        <span style="letter-spacing: 1px; font-weight: bold; vertical-align: middle;">SEARCH FOR:</span>&nbsp;<asp:TextBox runat="server" ID="txtSearch" CssClass="searchfield"></asp:TextBox>
        <asp:Button runat="server" ID="btnSearch" Text="Search" />
    </div>

    <%--grdSrcCode for Account Entries--%>
    <%--<asp:GridView ID="grdSrcCodes" runat="server" CssClass="grid" AutoGenerateColumns="false" Width="100%" PageSize="15" AllowPaging="true" AllowSorting="true">
        <EmptyDataTemplate>
            <b>No records!</b>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkEdit" Text="Edit" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xEdit" CssClass="listcontrols"></asp:LinkButton>
                    | 
                    <asp:LinkButton runat="server" ID="lnkDelete" Text="Delete" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xDelete" CssClass="listcontrols"></asp:LinkButton>
                    <ajax:ConfirmButtonExtender runat="server" ID="cbelnkDelete" TargetControlID="lnkDelete"
                        ConfirmText="Are you sure you want to delete selected Account?" ></ajax:ConfirmButtonExtender>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="AccountCode" HeaderText="Account Code" ItemStyle-Width="25%" />
            <asp:BoundField DataField="AccountDescription" HeaderText="Account Description" ItemStyle-Width="75%" SortExpression="AccountDescription" />
            <%--<asp:BoundField DataField="ObRType" HeaderText="Type" ItemStyle-Width="40%" />--%>
        <%--</Columns>
    </asp:GridView>--%>

    <asp:GridView ID="grdSrcCodes" runat="server" CssClass="grid" AutoGenerateColumns="false" Width="100%" PageSize="15" AllowPaging="true" AllowSorting="true">
        <EmptyDataTemplate>
            <b>No records!</b>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkEdit" Text="Edit" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xEdit" CssClass="listcontrols"></asp:LinkButton>
                    |
                    <asp:LinkButton runat="server" ID="lnkActivate" Text="Show" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xActivate" CssClass="listcontrols"></asp:LinkButton>
                    <ajax:ConfirmButtonExtender runat="server" ID="cbelnkActivate" TargetControlID="lnkActivate"
                        ConfirmText="Are you sure you want to show the selected UACS Code?"></ajax:ConfirmButtonExtender>
                    |
                    <asp:LinkButton runat="server" ID="lnkDeactivate" Text="Hide" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xDeactivate" CssClass="listcontrols"></asp:LinkButton>
                    <ajax:ConfirmButtonExtender runat="server" ID="cbelnkDeactivate" TargetControlID="lnkDeactivate"
                        ConfirmText="Are you sure you want to hide the selected UACS Code?" ></ajax:ConfirmButtonExtender>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="UACSCode" HeaderText="UACS Source Code" ItemStyle-Width="20%" />
            <asp:BoundField DataField="UACSDescription" HeaderText="Description" ItemStyle-Width="50%" SortExpression="Description" />
            <asp:TemplateField ItemStyle-Width="20%" HeaderText="Status">
                    <ItemTemplate>
                        <%# If(Eval("IsActive") = 1, String.Format("<p style='color: green; font-weight: bold;'>ACTIVE</p>"), String.Format("<p <p style='color: red; font-weight: bold;'>HIDDEN</p>"))%>
                    </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:BoundField DataField="ObRType" HeaderText="Type" ItemStyle-Width="40%" />--%>
        </Columns>
    </asp:GridView>

    <div style="display: none;">
        <asp:Button runat="server" ID="btnDummy" />
    </div>
    <ajax:ModalPopupExtender runat="server" ID="mpeUACS" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" TargetControlID="btnDummy" PopupControlID="pnlAddPayee" PopupDragHandleControlID="pnlAddPayeeHeader">
    </ajax:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlAddPayee" CssClass="modalPopup" Style="width: 750px !important;">
        <asp:Panel runat="server" ID="pnlAddPayeeHeader" CssClass="mwPopWindowTitle" Style="width: 750px !important;">
            <asp:Label runat="server" ID="lblAddPayeeHeader">&nbsp;&nbsp;Add UACS Funding Source Code</asp:Label>
        </asp:Panel>
        <br />
        <div style="width: 700px; margin: 0 auto;">
            <asp:Label runat="server" ID="lblAddError" ForeColor="Red"></asp:Label>
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="30%">
                        <asp:Label ID="Label3" CssClass="lblpayeelib" runat="server">UACS Funding Source Code:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtUACSCode" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label4" CssClass="lblpayeelib" runat="server">Description:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtUACSCodeDesc" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <br />
        <div style="width: 220px; margin: 0 auto;">
            <asp:Button runat="server" ID="btnSave" Text="Save" />
            <ajax:ConfirmButtonExtender runat="server" ID="cbe2" TargetControlID="btnSave"
                ConfirmText="Are you sure to want save?">
            </ajax:ConfirmButtonExtender>
            &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" />
        </div>
    </asp:Panel>
</asp:Content>
