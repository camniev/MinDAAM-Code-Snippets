<%@ Page Title="Account Entries" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Account-Library.aspx.vb" Inherits="DbVoucher_Mark1.Account_Library" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="headerlib"></div>
    <h2 class="page-title"><%: Title %>: <span>List of all Account codes and description</span></h2>

    <div class="float-right" style="margin: 5px 0px;">
        <asp:Button runat="server" ID="btnAdd" Text="Add New Account" />
    </div>

    <div class="float-left">
        <span style="letter-spacing: 1px; font-weight: bold; vertical-align: middle;">SEARCH FOR:</span>&nbsp;<asp:TextBox runat="server" ID="txtSearch" CssClass="searchfield"></asp:TextBox>
        <asp:Button runat="server" ID="btnSearch" Text="Search" />
    </div>

    <asp:GridView ID="grdAccts" runat="server" CssClass="grid" AutoGenerateColumns="false" Width="100%" PageSize="15" AllowPaging="true" AllowSorting="true">
        <EmptyDataTemplate>
            <b>No records!</b>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkEdit" Text="Edit" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xEdit" CssClass="listcontrols"></asp:LinkButton>
                    | 
                    <asp:LinkButton runat="server" ID="lnkShow" Text="Show" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xShow" CssClass="listcontrols"></asp:LinkButton>
                    <ajax:ConfirmButtonExtender runat="server" ID="cbelnkShow" TargetControlID="lnkShow"
                        ConfirmText="Are you sure you want to show selected Account?" ></ajax:ConfirmButtonExtender>
                    | 
                    <asp:LinkButton runat="server" ID="lnkDelete" Text="Hide" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xDelete" CssClass="listcontrols"></asp:LinkButton>
                    <ajax:ConfirmButtonExtender runat="server" ID="cbelnkDelete" TargetControlID="lnkDelete"
                        ConfirmText="Are you sure you want to hide selected Account?" ></ajax:ConfirmButtonExtender>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="AccountCode" HeaderText="Account Code" ItemStyle-Width="15%" />
            <asp:BoundField DataField="AccountDescription" HeaderText="Account Description" ItemStyle-Width="55%" SortExpression="AccountDescription" />
            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Status">
                    <ItemTemplate>
                        <%# If(Eval("IsVoid").Equals(False), String.Format("<p style='color: green; font-weight: bold;'>ACTIVE</p>"), String.Format("<p <p style='color: red; font-weight: bold;'>HIDDEN</p>"))%>
                    </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:BoundField DataField="ObRType" HeaderText="Type" ItemStyle-Width="40%" />--%>
        </Columns>
    </asp:GridView>

    <div style="display: none;">
        <asp:Button runat="server" ID="btnDummy" />
    </div>
    <ajax:ModalPopupExtender runat="server" ID="mpeAcct" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" TargetControlID="btnDummy" PopupControlID="pnlAddPayee" PopupDragHandleControlID="pnlAddPayeeHeader">
    </ajax:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlAddPayee" CssClass="modalPopup" Style="width: 750px !important;">
        <asp:Panel runat="server" ID="pnlAddPayeeHeader" CssClass="mwPopWindowTitle" Style="width: 750px !important;">
            <asp:Label runat="server" ID="lblAddPayeeHeader">&nbsp;&nbsp;Add Account</asp:Label>
        </asp:Panel>
        <br />
        <div style="width: 700px; margin: 0 auto;">
            <asp:Label runat="server" ID="lblAddError" ForeColor="Red"></asp:Label>
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="30%">
                        <asp:Label ID="Label3" CssClass="lblpayeelib" runat="server">Account Code:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtAcctCode" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label4" CssClass="lblpayeelib" runat="server">Account Description:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtAcctDesc" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label1" CssClass="lblpayeelib" runat="server">Type</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList runat="server" ID="ddlType" DataTextField="Text" DataValueField="Value" Width="100%"></asp:DropDownList>
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
