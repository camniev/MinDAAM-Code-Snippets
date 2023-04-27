<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmChequeGeneration.aspx.vb" Inherits="DbVoucher_Mark1.frmChequeGeneration" %>

<%@ Register Src="~/custom controls/ctrlInfoMessage.ascx" TagName="InfoWindow" TagPrefix="uc1" %>

<%@ Register Src="~/custom controls/ctrlSignatory.ascx" TagName="signatorySelection" TagPrefix="uc2" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="headertrans"></div>

    <asp:Panel runat="server" ID="pnlMainDV">
        <h2 class="page-title">Cheque Generation: <span>List of Approved Disbursement Vouchers for cheque generation.</span></h2>
          <div class="float-left">
            <span style="letter-spacing: 1px; font-weight: bold; vertical-align: middle;">SEARCH FOR:</span>&nbsp;<asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>&nbsp;
            <asp:Button runat="server" ID="btnSearch" Text="Search" />
        </div>

        <asp:GridView runat="server" ID="grdApprovedDVList" CssClass="grid" AutoGenerateColumns="false" Width="100%" AllowPaging="true"
            PageSize="15" AllowSorting="true">
            <EmptyDataTemplate>
                <b>No Record/s found!</b>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-Width="25%">
                    <ItemTemplate>
                        <%-- <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkSourceDoc" Text="View Source Document" CommandName="xViewSource" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton> |--%>
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkGenerate" Text="Generate Cheque" CommandName="xGenerate" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        |
                       <%-- <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrint" Text="Print" CommandName="xPrint" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                       --%> |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkParticular" Text="UnApprove Voucher" CommandName="xBackDV" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                         <ajax:ConfirmButtonExtender runat="server" ID="cbeLnkBacktoVoucher" TargetControlID="lnkParticular" 
                            ConfirmText="Are you sure to want to Unapprove the DV?" >
                        </ajax:ConfirmButtonExtender>
                       <%-- |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkCancel" Text="Cancel Cheque" CommandName="xCancel" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                    --%></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Control Number" DataField="DisbursementVoucherNo" ItemStyle-Width="10%" SortExpression="DisbursementVoucherNo" />
                <asp:BoundField HeaderText="Payee" DataField="PayeeName" ItemStyle-Width="36%" SortExpression="PayeeName" />
                <asp:BoundField HeaderText="Date Created" DataField="DateCreated" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%" SortExpression="DateCreated" />
                <asp:BoundField HeaderText="Date Approved" DataField="DateApprovedForPayment" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%" SortExpression="" />
                <asp:BoundField HeaderText="Amount" DataField="ParticularsAmountDue" DataFormatString="{0:#,##0.00}" ItemStyle-Width="9%" SortExpression="ParticularsAmountDue" ItemStyle-HorizontalAlign="Right"/>
               <%-- <asp:BoundField HeaderText="Total Amount" DataField="TotalAmount" ItemStyle-Width="10%" />--%>

            </Columns>
        </asp:GridView>
        <div style="display: none;">
            <asp:Button runat="server" ID="Button2" />
        </div>
    </asp:Panel>


    <asp:panel runat="server" ID="pnlChequeGen">
         <h2 class="page-title">Cheque Details: <span>Enter Details for cheque generation.</span></h2>

        <div style="float:right;width:23%;padding:10px;margin:0px;">

            <span style="text-transform: uppercase; font-size: 10px; letter-spacing: 1px;color:#ff0000;">for reference:</span> <hr />
            <span style="text-transform: uppercase; font-size: 10px; letter-spacing: 1px;">Disbursement Voucher no:</span>
            <asp:Label runat="server" ID="lblRefDVNo" Font-Bold="true" Font-Names="Courier New" Font-Size="Medium"></asp:Label> <br /><br />
            <span style="text-transform: uppercase; font-size: 10px; letter-spacing: 1px;color:#ff0000;margin-top:20px;">Choose Cheque Date:</span> <hr />
            <asp:Calendar ID="calChequeDate" runat="server" CssClass="cal" Width="300" Style="margin-bottom: 10px;"></asp:Calendar>
            
        
         <%--   <span style="text-transform: uppercase; font-size: 10px; letter-spacing: 1px;">DV Approval Date:</span><br />
            <asp:Label runat="server" ID="lblRefDVDate"></asp:Label>--%>
            
        </div>


       <div style="float:left;width:75%;">
        <table style="width:100%;">
                 <tr>
                <td style="width:15%;text-align:right;"> <asp:Label ID="Label1" runat="server" Text="Cheque No:" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtChequeNo" CssClass="chequetbox" Width="95%"></asp:TextBox></td>
                </tr>

                <tr>
                <td style="width:15%;text-align:right;"> <asp:Label runat="server" Text="Date:" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtChequeDate" CssClass="chequetbox" Width="95%" ReadOnly="true"></asp:TextBox></td>
                </tr>

                <tr>
                <td style="width:15%;text-align:right;vertical-align:top;"><asp:Label runat="server" Text="Pay to the order of:" CssClass="chequelabel"></asp:Label></td>
                <td style="vertical-align:top;">&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtPayOrderTo" CssClass="chequetbox"  Width="95%" ReadOnly="true" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                
                <tr>
                <td style="width:15%;text-align:right;"><asp:Label runat="server" Text="amount" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtNumber" CssClass="chequetbox" Width="95%" ReadOnly="true"></asp:TextBox></td>
                </tr>

                <tr>
                <td style="width:15%;text-align:right;vertical-align:top"><asp:Label runat="server" Text="amount (in words)" CssClass="chequelabel"></asp:Label></td>
                <td style="vertical-align:top;">&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtResult" CssClass="chequetbox" TextMode="MultiLine" Width="95%" ReadOnly="true" Rows="4"></asp:TextBox> <asp:CheckBox runat="server" id="chkUseUsGroupNames" Visible="false" /></td> 
                </tr> 
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Label ID="lblAllotment" runat="server" Text="xxx" Visible="false"></asp:Label></td>
                </tr>
        </table>
        </div>

        <br />
        <div class="float-left;margin-top:20px;">
        <h2 class="page-title">Signatories: <span>Cheque Signatory</span></h2>
    </div>

      <table style="width: 75%; margin: 6px 0px 0px;" class="obrform">
        <tr>
            <td style="width: 7%; text-align:right; padding:5px 0px 0px;">
                <asp:Label ID="Label2" runat="server" Text="Block A - "></asp:Label></td>
            <td style="width: 30%">
                <uc2:signatorySelection runat="server" ID="signatorySelectionA" />
            </td>
            <td style="width: 7%; text-align:right; padding:5px 0px 0px;">
                <asp:Label ID="Label6" runat="server" Text="Block B - "></asp:Label></td>
            <td style="width: 30%">
                <uc2:signatorySelection runat="server" ID="signatorySelectionB" />
            </td>
        </tr>
          </table>





        <div style="float:right;margin:0px 10px;">
             <asp:Button ID="btnBack" runat="server" Text="Back"/> <asp:Button ID="btnGenerateCheque" runat="server" Text="Generate Cheque"/>
        </div>

        

    </asp:panel>

    <asp:Panel runat="server" ID="pnlPrintCheque" Visible="false">
           <asp:Button runat="server" ID="btnBacktolist" Text="Back to Disbursement Voucher List" />
        <rsweb:ReportViewer ID="rptChViewer" runat="server" Width="100%" Height="100%" AsyncRendering="False" SizeToReportContent="True" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Auth\Transactions\rdlc\rptCheque.rdlc"></LocalReport>
        </rsweb:ReportViewer>
        <asp:Button ID="btnPrint" runat="server" OnClientClick="javascript:window.print();" Text="Print" Visible="false"/>

        <asp:Button runat="server" ID="btnClosePrint" Text="Back to List"/>
    </asp:Panel>




    <div style="display: none;">
    <asp:Button runat="server" ID="btnDummypop" />
</div>
    <ajax:ModalPopupExtender runat="server" ID="mpeInfoPop" BackgroundCssClass="modalBackground"
    CancelControlID="btnOkInfoPop" TargetControlID="btnDummypop" PopupControlID="pnlInfoPop" PopupDragHandleControlID="pnlInfoPopHeader">
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




</asp:Content>
