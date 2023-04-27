<%@ Page Title="Disbursement Voucher" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmDisbursementVoucherMain.aspx.vb" Inherits="DbVoucher_Mark1.DisbursementVoucherMain" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Auth/Transactions/controls/ctrlCreateDV.ascx" TagName="ctrlCreateDVForm" TagPrefix="ucCreateDVForm" %>
<%@ Register Src="~/Auth/Transactions/controls/ctrlEditDV.ascx" TagName="ctrlEditDVForm" TagPrefix="ucEditDVForm" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="headertrans">
    </div>
    <asp:Panel runat="server" ID="pnlMainDV">
        <h2 class="page-title">Disbursement Voucher: <span>List of Created Disbursement Vouchers</span></h2>
        <div class="float-left">
            <span style="letter-spacing: 1px; font-weight: bold; vertical-align: middle;">SEARCH FOR:</span>&nbsp;<asp:TextBox runat="server" ID="txtsearch"></asp:TextBox>
            &nbsp;
            <asp:Button runat="server" ID="btnSearch" Text="Search" />
        </div>
        <asp:GridView runat="server" ID="grdDVList" CssClass="grid" AutoGenerateColumns="false" Width="100%" AllowPaging="true"
            PageSize="15" AllowSorting="true">
            <EmptyDataTemplate>
                <b>No Record/s found!</b>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-Width="25%">
                    <ItemTemplate>
                        <%-- <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkSourceDoc" Text="View Source Document" CommandName="xViewSource" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton> |--%>
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkApprove" Text="Approve DV" CommandName="xApproveDV" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        <%-- <ajax:ConfirmButtonExtender runat="server" ID="cbeLinkApproveDV" TargetControlID="lnkApprove" 
                            ConfirmText="Are you sure you want to Approve DV?" >
                        </ajax:ConfirmButtonExtender>--%>|
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrint" Text="Print" CommandName="xPrint" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkParticular" Text="Edit DV" CommandName="xEditDV" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        <ajax:ConfirmButtonExtender runat="server" ID="cbeLinkEditDV" TargetControlID="lnkParticular" 
                            ConfirmText="Are you sure you want to Edit DV?" >
                        </ajax:ConfirmButtonExtender>
                        |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkCancel" Text="Back to ObR" CommandName="xCancel" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        <ajax:ConfirmButtonExtender runat="server" ID="cbeLinkCancelDV" TargetControlID="lnkCancel" 
                            ConfirmText="Are you sure you want to Cancel DV?" >
                        </ajax:ConfirmButtonExtender>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Control Number" DataField="DisbursementVoucherNo" ItemStyle-Width="10%" SortExpression="DisbursementVoucherNo"/>
                <asp:BoundField HeaderText="Payee" DataField="PayeeName" ItemStyle-Width="36%" SortExpression="PayeeName"/>
                <asp:BoundField HeaderText="Date Created" DataField="DateCreated" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%" SortExpression="DateCreated"/>
                <asp:BoundField HeaderText="Amount" DataField="ParticularsAmountDue" ItemStyle-Width="9%" DataFormatString="{0:#,##0.00}" SortExpression="ParticularsAmountDue" ItemStyle-HorizontalAlign="Right"/>
                <%-- <asp:BoundField HeaderText="Total Amount" DataField="TotalAmount" ItemStyle-Width="10%" />--%>
            </Columns>
        </asp:GridView>
        <div style="display: none;">
            <asp:Button runat="server" ID="Button2" />
            <asp:Button runat="server" ID="btnlnkApproval" />
        </div>
        <ajax:ModalPopupExtender runat="server" ID="mpeApprovalDate" BackgroundCssClass="modalBackground"
            CancelCOntrolID="btnCancel" TargetControlID="btnlnkApproval" PopupControlID="pnlApprovalDate" PopupDragHandleControlID="pnlApprovalDateHeader">
        </ajax:ModalPopupExtender>
        <asp:Panel runat="server" ID="pnlApprovalDate" Style="width: 210px !important;background:#ddd;">
            <asp:Panel runat="server" ID="pnlApprovalDateHeader" CssClass="mwPopWindowTitle" Style="width: 210px !important;">
                <asp:Label runat="server" ID="Label1">&nbsp;&nbsp;Continue with Approval?</asp:Label>
            </asp:Panel>
            <div style="width:210px;padding:20px;margin:0px auto;">
                <asp:Label ID="Label2" runat="server" Text="SPECIFY APPROVAL DATE" Font-Size="XX-Small" Font-Bold="true"></asp:Label>
                <br />
                <ajax:CalendarExtender runat="server" ID="ceDateFrom" PopupButtonID="imgDateFrom"
                                TargetControlID="txtApprovalDate">
                </ajax:CalendarExtender>
                <asp:TextBox runat="server" ID="txtApprovalDate"></asp:TextBox>
                <img src="../../../Images/calendar.jpg" alt="" id="imgDateFrom" style="margin-top:-20px;"/>
                <br />
                <asp:Button runat="server" ID="btnApproveDV" Text="Yes" />
                <asp:Button runat="server" ID="btnCancel" Text="No"/>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrintDV">
        <asp:Button runat="server" ID="btnBack" Text="Back to Disbursement Voucher List" />
        <rsweb:ReportViewer ID="rptDVViewer" runat="server" Width="100%" Height="100%" AsyncRendering="False" SizeToReportContent="True" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Auth\Transactions\rdlc\rptDisbursementVoucher.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:Button ID="btnPrint" runat="server" OnClientClick="javascript:window.print();" Text="Print" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlViewDetails">
        <ucCreateDVForm:ctrlCreateDVForm runat="server" ID="CtrlCreateDVForm1" Visible="false"/>
        <ucEditDVForm:ctrlEditDVForm runat="server" ID="ctrlEditDisbursementVoucher" />
    </asp:Panel>

</asp:Content>
