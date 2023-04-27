<%@ Page Title="Obligation Request List" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmObligationRequestMain.aspx.vb" Inherits="DbVoucher_Mark1.frmObligationRequestMain" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Auth/Transactions/controls/ctrlCreateObligationRequest.ascx" TagName="ctrlObRForm" TagPrefix="ucObRForm" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="featuredContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="headertrans"></div>

    <asp:Panel runat="server" ID="pnlMain">

        <h2 class="page-title"><%: Title %>: <span>List of all Created Obligation Request</span></h2>

        <div class="float-right" style="margin: 5px 0px; clear: both;">
            <asp:Button runat="server" ID="btnCreateObR" Text="Create Obligation Request" />
        </div>

        <div class="float-left">
            <span style="letter-spacing: 1px; font-weight: bold; vertical-align: middle;">SEARCH FOR:</span>&nbsp;<asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>&nbsp;
            <asp:Button runat="server" ID="btnSearch" Text="Search" />
        </div>
        <%--<asp:label runat="server" Text="2" style="display:none;" ID="status2"></asp:label><asp:label runat="server" Text="3" style="display:none;" ID="status3"></asp:label><asp:label runat="server" Text="4" style="display:none;" ID="status4"></asp:label>--%>

        <asp:GridView runat="server" ID="grdObR" CssClass="grid" AutoGenerateColumns="false" Width="100%" AllowPaging="true"
            PageSize="15" AllowSorting="true">
            <EmptyDataTemplate>
                <b>No Record/s found!</b>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-Width="26%">
                    <ItemTemplate>
                        <%-- <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkSourceDoc" Text="View Source Document" CommandName="xViewSource" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton> |--%>
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkVerify" Text="Verify" CommandName="xVerify" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        <ajax:ConfirmButtonExtender runat="server" ID="cbeVerify" TargetControlID="lnkVerify" ConfirmText="Continue on verifying Obligation Request ?">
                        </ajax:ConfirmButtonExtender>
                        |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrint" Text="Print" CommandName="xPrint" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkParticular" Text="View Details" CommandName="xViewParticular" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkCancel" Text="Cancel" CommandName="xCancel" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                      <%--  <ajax:ConfirmButtonExtender runat="server" ID="cbeCancel" TargetControlID="lnkCancel" ConfirmText="Continue on cancelling Obligation Request?">
                        </ajax:ConfirmButtonExtender>--%>
                        |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkEdit" Text="Edit" CommandName="xEdit" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                       <%-- |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkDelete" Text="Delete" CommandName="xDelete" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        <ajax:ConfirmButtonExtender runat="server" ID="cbeDelete" TargetControlID="lnkDelete" ConfirmText="Continue on deleting Obligation Request record?">
                        </ajax:ConfirmButtonExtender>--%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="ObligationRequestNo" HeaderText="Control Number" ItemStyle-CssClass="hiddencolumn" HeaderStyle-CssClass="hiddencolumn" ItemStyle-Width="9%" SortExpression="ObligationRequestNo"/>
                <asp:TemplateField ItemStyle-Width="9%" HeaderText="Control Number">
                    <ItemTemplate>
                        <%# If(Eval("ObligationRequestNo") = "NO RECORD", "", Switch(Eval("AllotmentObjectClass") = "100", "01", Eval("AllotmentObjectClass") = "200", "02", Eval("AllotmentObjectClass") = "300", "06")) + If(Eval("ObligationRequestNo") = "NO RECORD", "","-") + Eval("UACSCode") + If(Eval("UACSCode") Is Nothing, "", "-") + Eval("ObligationRequestNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Payee" DataField="PayeeName" ItemStyle-Width="40%" SortExpression="PayeeName"/>
                <asp:BoundField HeaderText="Date Created" DataField="DateCreated" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="8%" SortExpression="DateCreated" ItemStyle-HorizontalAlign="Center"/>
                <%--<asp:BoundField HeaderText="Due Date" DataField="DueDate" ItemStyle-Width="9%" />--%>
                <asp:BoundField HeaderText="Total Amount" DataField="TotalAmount" ItemStyle-Width="9%" DataFormatString="{0:#,##0.00}" SortExpression="TotalAmount" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField HeaderText="Prio" DataField="isPrio" ItemStyle-Width="10px" DataFormatString="{0:#,##0.00}" SortExpression="TotalAmount" ItemStyle-HorizontalAlign="Right" Visible="false"/>

            </Columns>
        </asp:GridView>

        <div style="display: none;">
            <asp:Button runat="server" ID="btnDummy" />
             <asp:Button runat="server" ID="btnDummy2" />
        </div>

        <ajax:ModalPopupExtender runat="server" ID="mpeInfoPop" BackgroundCssClass="modalBackground"
            CancelControlID="btnOkInfoPop" TargetControlID="btnDummy" PopupControlID="pnlInfoPop" PopupDragHandleControlID="pnlInfoPopHeader">
        </ajax:ModalPopupExtender>
        <asp:Panel runat="server" ID="pnlInfoPop" CssClass="modalPopup" Style="width: 350px !important;">
            <asp:Panel runat="server" ID="pnlInfoPopHeader" CssClass="mwPopWindowTitle" Style="width: 350px !important;">
                <asp:Label runat="server" ID="lblInfoPopHeader">INFO</asp:Label>
            </asp:Panel>
            <br />
            <div style="width: 300px; margin: 0 auto;">
                <asp:Label runat="server" ID="lblInfoPopMsg"></asp:Label>
            </div>
            <br />
            <div style="width: 70px; margin: 0 auto;">
                &nbsp;<asp:Button runat="server" ID="btnOkInfoPop" Text="OK" />
            </div>
        </asp:Panel>


          <ajax:ModalPopupExtender runat="server" ID="mpeCancelDate" BackgroundCssClass="modalBackground"
            CancelCOntrolID="btnDontContinue" TargetControlID="btnDummy2" PopupControlID="pnlCancelObR" PopupDragHandleControlID="pnlCancelObrHeader">
         </ajax:ModalPopupExtender>
        <asp:Panel runat="server" ID="pnlCancelObR" Style="width: 200px !important;background:#ddd;">
           <asp:Panel runat="server" ID="pnlCancelObrHeader" CssClass="mwPopWindowTitle" Style="width: 200px !important;">
                 <asp:Label runat="server" ID="Label1">&nbsp;&nbsp;Continue with Cancel?</asp:Label>
           </asp:Panel>
            
            <div style="width:200px;padding:20px;margin:0px auto;">
                <asp:Label runat="server" Text="Specify Cancel Date."></asp:Label>
                  <ajax:CalendarExtender runat="server" ID="ceDateFrom" PopupButtonID="imgDateFrom"
                                TargetControlID="txtCancelDate">
                            </ajax:CalendarExtender>
                            <asp:TextBox runat="server" ID="txtCancelDate"></asp:TextBox>
                            <img src="../../../Images/calendar.jpg" alt="" id="imgDateFrom" style="margin-top:-20px;"/>
                <br />
                <asp:Button runat="server" ID="btnCancelObligationRequest" Text="Yes" />
                <asp:Button runat="server" ID="btnDontContinue" Text="No"/>
            </div>
        </asp:Panel>





    </asp:Panel>

    <asp:Panel runat="server" ID="pnlPrintObR">
        <asp:Button runat="server" ID="btnBack" Text="Back to OR Request List" />
        <rsweb:ReportViewer ID="rptViewer" runat="server" Width="100%" Height="100%" Enabled="true" ShowPrintButton="true" Visible="true" AsyncRendering="False" SizeToReportContent="True">
            <LocalReport ReportPath="~/Auth/Transactions/rdlc/rptObligationRequest.rdlc" ></LocalReport>
           <%-- <LocalReport ReportPath="~/DVAReports/rptObligationRequest.rdlc"></LocalReport>--%>
        </rsweb:ReportViewer>
        <asp:Button ID="btnPrint" runat="server" OnClientClick="javascript:window.print();" Text="Print" />
    </asp:Panel>



    <asp:Panel runat="server" ID="pnlObRForm">
        <h2 class="page-title"><%: Title%>: <span>Please Fill in necesssary details.</span></h2>
        <ucObRForm:ctrlObRForm runat="server" ID="ucObReqForm" />
    </asp:Panel>
</asp:Content>
