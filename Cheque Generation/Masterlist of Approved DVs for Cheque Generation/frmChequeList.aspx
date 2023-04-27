<%--<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmChequeList.aspx.vb" Inherits="DbVoucher_Mark1.frmChequeList" validateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<%@ Page Title="Cheque List" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmChequeList.aspx.vb" Inherits="DbVoucher_Mark1.frmChequeList" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/custom controls/ctrlSignatory.ascx" TagName="signatorySelection" TagPrefix="uc2" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="headertrans"></div>

      <asp:Panel runat="server" ID="pnlMainDV">
        <h2 class="page-title">Cheque List: <span>List of created Cheques.</span></h2>
          <div class="float-left">
            <span style="letter-spacing: 1px; font-weight: bold; vertical-align: middle;">SEARCH FOR:</span>&nbsp;<asp:TextBox runat="server" ID="txtsearch"></asp:TextBox>&nbsp;
            <asp:Button runat="server" ID="btnSearch" Text="Search" />
        </div>

          <asp:GridView runat="server" ID="grdApprovedChList" CssClass="grid" AutoGenerateColumns="false" Width="100%" AllowPaging="true"
            PageSize="15" AllowSorting="true">
            <EmptyDataTemplate>
                <b>No Record/s found!</b>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-Width="37%">
                    <ItemTemplate>
                        <%-- <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkSourceDoc" Text="View Source Document" CommandName="xViewSource" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton> |--%>
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkGenerate" Text="Released" CommandName="xClear" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        <ajax:ConfirmButtonExtender runat="server" ID="cbeLinkClearCheque" TargetControlID="lnkGenerate" 
                            ConfirmText="Post Cheque as Released" >
                        </ajax:ConfirmButtonExtender>
                        |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrint" Text="Print" CommandName="xPrint" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkParticular" Text="Cancel Transaction" CommandName="xCancel" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                          <ajax:ConfirmButtonExtender runat="server" ID="cbeLinkCancelCheque" TargetControlID="lnkParticular" 
                            ConfirmText="Are you sure you want to Cancel the Cheque and the Whole Transaction?" >
                        </ajax:ConfirmButtonExtender>
                         |
                         <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkCancelDV" Text="Cancel Cheque/DV" CommandName="xCancelDV" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                          <ajax:ConfirmButtonExtender runat="server" ID="ConfirmButtonExtender2" TargetControlID="lnkCancelDV" 
                            ConfirmText="Are you sure you want to Cancel the Cheque and Cancel the DV and Return Transaction to Verified OBR" >
                        </ajax:ConfirmButtonExtender>
                         |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkBacktoDV" Text="Back to DV" CommandName="xBackToDV" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                          <ajax:ConfirmButtonExtender runat="server" ID="ConfirmButtonExtender1" TargetControlID="lnkBacktoDV" 
                            ConfirmText="Are you sure you want to Cancel Cheque and Post DV as Unapproved?" >
                        </ajax:ConfirmButtonExtender>
                          |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkEditCheque" Text="Edit" CommandName="xEditCheque" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                          <ajax:ConfirmButtonExtender runat="server" ID="ConfirmButtonExtender3" TargetControlID="lnkEditCheque" 
                            ConfirmText="Are you sure you want to Edit the Cheque?" >
                        </ajax:ConfirmButtonExtender>
                       <%-- |
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkCancel" Text="Cancel Cheque" CommandName="xCancel" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                    --%></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Cheque No." DataField="ChequeNo" ItemStyle-Width="6%" SortExpression="ChequeNo" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField HeaderText="Pay to order" DataField="PayToOrder" ItemStyle-Width="32%" SortExpression="PayToOrder" />
                <asp:BoundField HeaderText="Date" DataField="ChequeDate" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="8%" SortExpression="ChequeDate" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Amount" DataField="Amount" DataFormatString="{0:#,##0.00}" ItemStyle-Width="7%" SortExpression="Amount" ItemStyle-HorizontalAlign="Right"/>
               <%-- <asp:BoundField HeaderText="Total Amount" DataField="TotalAmount" ItemStyle-Width="10%" />--%>

            </Columns>
        </asp:GridView>
        <div style="display: none;">
            <asp:Button runat="server" ID="Button2" />
        </div>
          
      </asp:Panel>


    <asp:Panel runat="server" ID="pnlCancel" Visible="false">
         <h2 class="page-title">Cheque Cancellation: <span>Enter Details for Cheque Cancellation.</span></h2>

          <div style="float:right;width:23%;padding:10px;margin:0px;">

            <span style="text-transform: uppercase; font-size: 10px; letter-spacing: 1px;color:#ff0000;">for reference:</span> <hr />
            <span style="text-transform: uppercase; font-size: 10px; letter-spacing: 1px;">Cheque no:</span>
            <asp:Label runat="server" ID="lblChequeNo" Font-Bold="true" Font-Names="Courier New" Font-Size="Medium"></asp:Label> <br /><br />
            <span style="text-transform: uppercase; font-size: 10px; letter-spacing: 1px;color:#ff0000;margin-top:20px;">Choose Cancellation Date:</span> <hr />
            <asp:Calendar ID="calCancelDate" runat="server" CssClass="cal" Width="300" Style="margin-bottom: 10px;"></asp:Calendar>
             
        </div>


        <div style="float:left;width:75%;">
        <table style="width:100%;">
                <tr>
                <td style="width:15%;text-align:right;"> <asp:Label ID="Label1" runat="server" Text="Pay to Order to:" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtPayToOrder" CssClass="chequetbox" Width="95%" ReadOnly="true"></asp:TextBox></td>
                </tr>

                <tr>
                <td style="width:15%;text-align:right;"> <asp:Label ID="Label3" runat="server" Text="Amount:" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtAmount" CssClass="chequetbox" Width="95%" ReadOnly="true"></asp:TextBox></td>
                </tr>

                <tr>
                <td style="width:15%;text-align:right;vertical-align:top;> <asp:Label ID="Label4" runat="server" Text="Amount in Words:" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtAmountInWords" CssClass="chequetbox" Width="95%" TextMode="MultiLine" ReadOnly="true" Rows="2"></asp:TextBox></td>
                </tr>   

                <tr>
                <td style="width:15%;text-align:right;"> <asp:Label ID="Label5" runat="server" Text="Cancellation Date:" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtCancellationDate" CssClass="chequetbox" Width="95%" ReadOnly="true"></asp:TextBox></td>
                </tr>

                <tr>
                <td style="width:15%;text-align:right;vertical-align:top;"> <asp:Label ID="Label2" runat="server" Text="Remarks:" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtRemarks" CssClass="chequetbox" Width="95%" TextMode="MultiLine" Rows="4"></asp:TextBox></td>
                </tr>
            </table>

            <asp:Button runat="server" ID="btnBackToList" Text="Back to List" /> <asp:Button runat="server"  ID="btnCancelCheque" Text="Cancel Cheque"/> <asp:Button runat="server"  ID="btnBackToDV" Text="Cancel Cheque and Post DV as Unapproved" Visible="false"/> <asp:Button runat="server"  ID="btnCancelDV" Text="Cancel Cheque and DV and Post Transaction as Verified OBR" Visible="false"/>

              </div>

    </asp:Panel>


    <!-- EDIT CHEQUE -->

    <asp:panel runat="server" ID="pnlChequeGen" Visible="false">
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
                <td style="width:15%;text-align:right;"> <asp:Label ID="Label6" runat="server" Text="Cheque No:" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtChequeNoEdit" CssClass="chequetbox" Width="95%"></asp:TextBox></td>
                </tr>

                <tr>
                <td style="width:15%;text-align:right;"> <asp:Label ID="Label7" runat="server" Text="Date:" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtChequeDateEdit" CssClass="chequetbox" Width="95%" ReadOnly="true"></asp:TextBox></td>
                </tr>

                <tr>
                <td style="width:15%;text-align:right;vertical-align:top;"><asp:Label ID="Label8" runat="server" Text="Pay to the order of:" CssClass="chequelabel"></asp:Label></td>
                <td style="vertical-align:top;">&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtPayOrderToEdit" CssClass="chequetbox"  Width="95%" ReadOnly="true" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                
                <tr>
                <td style="width:15%;text-align:right;"><asp:Label ID="Label9" runat="server" Text="amount" CssClass="chequelabel"></asp:Label></td>
                <td>&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtNumberEdit" CssClass="chequetbox" Width="95%" ReadOnly="true"></asp:TextBox></td>
                </tr>

                <tr>
                <td style="width:15%;text-align:right;vertical-align:top"><asp:Label ID="Label10" runat="server" Text="amount (in words)" CssClass="chequelabel"></asp:Label></td>
                <td style="vertical-align:top;">&nbsp; &nbsp;<asp:TextBox runat="server" ID="txtResultEdit" CssClass="chequetbox" TextMode="MultiLine" Width="95%" ReadOnly="true" Rows="4"></asp:TextBox> <asp:CheckBox runat="server" id="chkUseUsGroupNames" Visible="false" /></td> 
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
                <asp:Label ID="Label11" runat="server" Text="Block A - "></asp:Label></td>
            <td style="width: 30%">
                <uc2:signatorySelection runat="server" ID="signatorySelectionA" />
            </td>
            <td style="width: 7%; text-align:right; padding:5px 0px 0px;">
                <asp:Label ID="Label12" runat="server" Text="Block B - "></asp:Label></td>
            <td style="width: 30%">
                <uc2:signatorySelection runat="server" ID="signatorySelectionB" />
            </td>
        </tr>
          </table>





        <div style="float:right;margin:0px 10px;">
             <asp:Button ID="btnBack" runat="server" Text="Back"/> <asp:Button ID="btnUpdateCheque" runat="server" Text="Update Cheque Details"/>
        </div>

        

    </asp:panel>



    <!-- -->





    <asp:Panel runat="server" ID="pnlPrintCheque" Visible="false">
           <asp:Button runat="server" ID="Button1" Text="Back to Disbursement Voucher List" />
        <rsweb:ReportViewer ID="rptChViewer" runat="server" Width="100%" Height="100%" AsyncRendering="False" SizeToReportContent="True" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Auth\Transactions\rdlc\rptCheque.rdlc"></LocalReport>
        </rsweb:ReportViewer>
        <asp:Button ID="btnPrint" runat="server" OnClientClick="javascript:window.print();" Text="Print" Visible="false"/>

        <asp:Button runat="server" ID="btnClosePrint" Text="Back to List"/>
    </asp:Panel>


</asp:content>
