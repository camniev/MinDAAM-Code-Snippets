<%@ Page Title="Withholding Tax List" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Tax-Library.aspx.vb" Inherits="DbVoucher_Mark1.Tax_Library" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/custom controls/ctrlInfoMessage.ascx" TagName="infoWindow" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="headerlib"></div>

    <h2 class="page-title"><%: Title %>: <span>List of all witholding tax to be used for disbursement.</span></h2>

    <div class="float-right" style="margin: 5px 0px;">
        <asp:Button runat="server" ID="btnAdd" Text="Add New Withdolding Tax" />
    </div>

    <div class="float-left">
        <span style="letter-spacing: 1px; font-weight: bold; vertical-align: middle;">SEARCH FOR:</span>&nbsp;<asp:TextBox runat="server" ID="txtSearch" CssClass="searchfield"></asp:TextBox>
        <asp:Button runat="server" ID="btnSearch" Text="Search" />
    </div>

    <asp:GridView ID="grdtaxlist" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="grid" AllowPaging="true" AllowSorting="true" PageSize="10">
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
                        ConfirmText="Are you sure you want to show selected Template?" ></ajax:ConfirmButtonExtender> 
                    | 
                    <asp:LinkButton runat="server" ID="lnkDelete" Text="Hide" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xDelete" CssClass="listcontrols"></asp:LinkButton> 
                    <ajax:ConfirmButtonExtender runat="server" ID="cbelnkDelete" TargetControlID="lnkDelete"
                        ConfirmText="Are you sure you want to hide selected Template?" ></ajax:ConfirmButtonExtender>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="TaxATC" HeaderText="ATC" ItemStyle-Width="10%" />
            <asp:BoundField DataField="TaxDescription" HeaderText="Tax Short Description" ItemStyle-Width="15%" />
            <asp:BoundField DataField="TaxLongDescription" HeaderText="Tax Long Description" ItemStyle-Width="20%" />
            <asp:BoundField DataField="TaxPercentage" HeaderText="Tax Percentage" ItemStyle-Width="10%" DataFormatString="{0:#,##0}" />
            <asp:BoundField DataField="TaxShortDesc" HeaderText="Tax Formula" ItemStyle-Width="15%" />
            <asp:BoundField DataField="TaxTypeText" HeaderText="Tax Type" ItemStyle-Width="15%" />
            <asp:TemplateField ItemStyle-Width="15%" HeaderText="Status">
                    <ItemTemplate>
                        <%# If(Eval("IsVoid").Equals(False), String.Format("<p style='color: green; font-weight: bold;'>ACTIVE</p>"), String.Format("<p <p style='color: red; font-weight: bold;'>HIDDEN</p>"))%>
                    </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <div style="display: none;">
        <asp:Button runat="server" ID="btnDummy" />
    </div>
    <ajax:ModalPopupExtender runat="server" ID="mpeTax" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" TargetControlID="btnDummy" PopupControlID="pnladdTax" PopupDragHandleControlID="pnlAddTaxHeader">
    </ajax:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlAddTax" CssClass="modalPopup" Style="width: 750px !important;">
        <asp:Panel runat="server" ID="pnlAddTaxHeader" CssClass="mwPopWindowTitle" Style="width: 750px !important;">
            <asp:Label runat="server" ID="lblAddTaxDeader">&nbsp;&nbsp;Add Withholding Tax</asp:Label>
        </asp:Panel>
        <br />
        <div style="width: 725px; margin: 0 auto;">
            <asp:Label runat="server" ID="lblAddError" ForeColor="Red"></asp:Label>
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="Label25" CssClass="lblpayeelib" runat="server">ATC:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtTaxATC" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="Label32" CssClass="lblpayeelib" runat="server">Tax Short Description:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txttaxdesc" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="Label26" CssClass="lblpayeelib" runat="server">Tax Long Description:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtTaxLongDescription" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label42" CssClass="lblpayeelib" runat="server">Tax Percentage:</asp:Label><br />
                        <asp:Label runat="server" ID="lbl1" Font-Size="Smaller">(do not include '%' symbol)</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txttaxperc" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label1" CssClass="lblpayeelib" runat="server">Tax Formula:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtTaxShortDesc" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label2" CssClass="lblpayeelib" runat="server">Tax Type:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <table style="width: 100%; border-color: transparent; border-width: 0px;" class="tblradio">
                            <tr>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rdBtnTaxType" RepeatColumns="1"
                                        CssClass="rdbtn">
                                        <asp:ListItem Text="Vat Registered" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Non-Vat Registered" Value="2"></asp:ListItem>
                                        <%--<asp:ListItem Text="Real Estate Rentals" Value="3"></asp:ListItem>--%>
                                        <asp:ListItem Text="Professional Fees" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
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
    <uc1:infoWindow runat="server" ID="ucInfoWindow" />
</asp:Content>
