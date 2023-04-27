<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlCreateDV.ascx.vb" Inherits="DbVoucher_Mark1.ctrlCreateDV" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/custom controls/ctrlPayeeDV.ascx" TagName="payeeSelectionDV" TagPrefix="uc1" %>
<%@ Register Src="~/custom controls/ctrlParticular.ascx" TagName="particularSelection" TagPrefix="uc2" %>
<%@ Register Src="~/custom controls/ctrtDivision.ascx" TagName="divisionSelection" TagPrefix="uc3" %>
<%@ Register Src="~/custom controls/ctrlPPA.ascx" TagName="ppaSelection" TagPrefix="uc4" %>
<%@ Register Src="~/custom controls/ctrlParticularTemplate.ascx" TagName="particularSelection" TagPrefix="uc5" %>
<%@ Register Src="~/custom controls/ctrlTax.ascx" TagName="taxSelection" TagPrefix="uc6" %>
<%@ Register Src="~/custom controls/ctrlInfoMessage.ascx" TagName="InfoWindow" TagPrefix="uc7" %>

<%@ Register Src="~/custom controls/ctrlSignatory.ascx" TagName="signatorySelection" TagPrefix="uc8" %>

<%@ Register Src="~/custom controls/ctrlTaxType2.ascx" TagName="taxSelectionType2" TagPrefix="uc9" %>
<%@ Register Src="~/custom controls/ctrlConfirmExtender.ascx" TagName="ConfirmExtender" TagPrefix="uc10" %>

<asp:Button runat="server" ID="btnDisregardTop" Text="Disregard Creation of Disbursement Voucher" />
<ajax:ConfirmButtonExtender runat="server" ID="ConfirmButtonExtender1" TargetControlID="btnDisregard"
    ConfirmText="All changes made will not be saved. Continue?">
</ajax:ConfirmButtonExtender>
&nbsp;<asp:Button runat="server" ID="btnSaveTop" Text="Save Disbursement Voucher" />
<asp:Label runat="server" ID="lblDVDateCreated" Visible="false"></asp:Label>
<asp:Panel runat="server" ID="pnlDVHeader">

    <%-- DISBURSEMENT HEADER INFO --%>
    <table style="border-color: transparent; border-width: 0px;" class="tblradio">
        <tr>
            <td style="width: 12%; vertical-align: bottom;"><span>Mode of Payment:</span>&nbsp;<asp:Label runat="server" ID="lblMoPError" ForeColor="Red" Font-Size="Small"></asp:Label></td>
            <td style="width: 35%;">
                <asp:RadioButtonList runat="server" ID="rdBtnModeOfPayment" RepeatColumns="3" CssClass="rdbtn" AutoPostBack="true"
                    DataTextField="Text" DataValueField="Value">
                </asp:RadioButtonList>
            </td>
            <td style="width: 50%;">
                <asp:Panel runat="server" ID="pnlMoPOthers" CssClass="pnlOthers">
                    <span>Others:</span>&nbsp;<asp:TextBox runat="server" ID="txtMoPOthers"></asp:TextBox>
                </asp:Panel>
            </td>
        </tr>

    </table>
    <div class="right" style="width: 39%; float: right;">
        <table style="width: 100%;">
            <tr>
                <td class="lblitemtitle" style="width: 32%;">TIN:&nbsp;</td>
                <td>
                    <asp:TextBox runat="server" ID="txtPayeeTin" CssClass="lblRecordFixed" ReadOnly="true" Width="90%"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="lblitemtitle">Obligation Request No.:&nbsp;</td>
                <td>
                    <asp:TextBox runat="server" ID="txtObrNo" CssClass="lblRecordFixed" ReadOnly="true" Width="90%"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="lblitemtitle">Verification Date:&nbsp;</td>
                <td>
                    <asp:TextBox runat="server" ID="txtObrDate" CssClass="lblRecordFixed" ReadOnly="true" Width="90%"></asp:TextBox></td>
            </tr>
        </table>
    </div>
    <div class="left" style="width: 59%;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 10%;" class="lblitemtitle">Payee:&nbsp;<asp:Label runat="server" ID="lblPayeeName" CssClass="lblRecordFixed" Visible="false"></asp:Label></td>
                <td>
                    <asp:TextBox runat="server" ID="txtPayeeName" CssClass="lblRecordFixed" ReadOnly="true"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="lblitemtitle">Office:&nbsp;<asp:Label runat="server" ID="lblPayeeOffice" Visible="false"></asp:Label></td>
                <td>
                    <asp:TextBox runat="server" ID="txtPayeeOffice" CssClass="lblRecordFixed" ReadOnly="true"></asp:TextBox></td>

            </tr>
            <tr>
                <td class="lblitemtitle">Address:&nbsp;<asp:Label runat="server" ID="lblPayeeAddress" Visible="false"></asp:Label></td>
                <td>
                    <asp:TextBox runat="server" ID="txtPayeeAddress" CssClass="lblRecordFixed" ReadOnly="true"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="lblitemtitle">DV Date:&nbsp;<asp:Label runat="server" ID="Label9" Visible="false"></asp:Label></td>
                <td>
                     <ajax:CalendarExtender runat="server" ID="ceDVDate" PopupButtonID="imgDateCreated" TargetControlID="txtDVDate" Format="dd/MM/yyyy">
                     </ajax:CalendarExtender>
                     <asp:TextBox runat="server" ID="txtDVDate" Width="150"></asp:TextBox>
                     <img src="../../../Images/calendar.jpg" alt="" id="imgDateCreated" style="margin-top:-20px;"/>
                </td>
            </tr>
        </table>
    </div>

    <%-- TAX ENTRIES --%>
    <div class="float-left;">
        <h2 class="page-title">Particulars: <span>Particular Details and With Holding Tax</span></h2>
    </div>  
    <div style="width: 100%;">
        <asp:Label runat="server" ID="lblParticularTemplate" Visible="false"></asp:Label>
        <asp:TextBox runat="server" ID="txtParticularTemplate" CssClass="dvparticulartxt" TextMode="MultiLine" Rows="4"></asp:TextBox>

        <asp:Label ID="Label1" runat="server" class="lblitemtitle" Font-Underline="true">Withholding tax:</asp:Label>
        |
        <asp:CheckBox runat="server" ID="chTaxType2" Text="" AutoPostBack="true" />
        <label style="display: inline-block; padding-bottom: 2px; padding-left: 5px;">Tax Type w/ Sub Accounts</label>

        <asp:Panel ID="pnlTax1" runat="server" Width="100%">
            <asp:Table runat="server" ID="tblComputation" Width="35%">
                <asp:TableRow>
                    <asp:TableCell Width="5%"></asp:TableCell>
                    <asp:TableCell Width="5%"></asp:TableCell>
                    <asp:TableCell Width="10%">Gross Amount</asp:TableCell>
                    <asp:TableCell Width="10%" Style="">
                        <asp:Label runat="server" ID="lblGrossAmount"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" ID="lbllesstax" Visible="false"> Less: w/ Tax</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell Style=""> 
                    </asp:TableCell>
                </asp:TableRow>
                <%--INSERT TABLE ROW HERE ON ADDNEWTAXLINE--%>
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell style="border-top:1px solid #ccc;">
                        <asp:Label runat="server" ID="lblTotalAmountDue"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlTax2" Width="100%">
        </asp:Panel>

<%--        <asp:CheckBox runat="server" ID="chCommitSubAccountTax" AutoPostBack="true" Visible="false"/>
         <label style="display: inline-block; padding-bottom: 2px; padding-left: 5px;" id="lblcommit" runat="server" visible="false">Commit Sub Account Tax</label>--%>
    </div>
    

    <%-- JOURNAL ENTRIES --%>
    <div class="float-left;">
        <h2 class="page-title">Journal Entry Voucher: <span>Accounting Entries</span></h2>
    </div>
    <br />

   
    <div style="float:left;">
    <table style="width: 100%;">
        <tr>
            <td colspan="4">
                <table style="border-color: transparent; border-width: 0px;" class="tblradio" hidden>
                    <tr>
                        <td Class="lblitemtitle">Voucher&nbsp;Type:&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rdBtnVoucherType" RepeatColumns="4" CssClass="rdbtn" AutoPostBack="true"
                                DataTextField="Text" DataValueField="Value">
                            </asp:RadioButtonList>
                            &nbsp;
                            <asp:Panel runat="server" ID="pnlVTOthers" CssClass="pnlOthers">
                                <span>Others:</span>&nbsp;<asp:TextBox runat="server" ID="txtVTOthers"></asp:TextBox>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 9%;" class="lblitemtitle">JE&nbsp;Voucher&nbsp;No.:&nbsp;</td>
            <td>
                <asp:TextBox runat="server" ID="txtJEVoucherNo" Width="300px" CssClass="lblRecordFixed" ReadOnly="true" Text="To be generated upon saving" Enabled="false"></asp:TextBox>
            </td>
            <td style="width: 4%;" class="lblitemtitle">Date&nbsp;:&nbsp;</td>
            <td>
                <asp:TextBox runat="server" ID="txtvoucherdate" ReadOnly="true"></asp:TextBox></td>
        </tr>
    </table>
    </div>

    <hr />
    <asp:Panel runat="server" ID="pnlSpecialOptions">
     <div style="float: left; border:1px solid #ccc; padding:10px; width:98%; margin:10px 0px 0px;">
         <table>
             <tr>
                 <td Class="lblitemtitle" style="border-bottom:1px solid #000;">Special Options</td>
                 <td></td>
                 <td></td>
                 <td></td>
                 <td></td>
             </tr>
             <tr>
                 <td hidden><asp:CheckBox runat="server" ID="chkTrustfund" Text="Cash Source: Trust Fund" AutoPostBack="true" CssClass="fund" /></td>
                 <td><asp:CheckBox runat="server" ID="chkCashAdvance" Text="Set Account Entries as Cash Advance" AutoPostBack="true" CssClass="fund" /></td>
                 <td hidden><asp:CheckBox runat="server" ID="chkOfficeSupplies" Text="Set Account Entries as Office Supplies" AutoPostBack="true" CssClass="fund" /></td>
                 <td hidden><asp:CheckBox runat="server" ID="chkOtherSupplies" Text="Set Account Entries as Other Supplies" AutoPostBack="true" CssClass="fund" /></td>
                 <td><asp:CheckBox runat="server" ID="chkAccountsPayable" Text="Set to Accounts Payable" AutoPostBack="true" CssClass="fund" /></td>
                 <td><asp:CheckBox runat="server" ID="chkIPURE" Text="Cash in Bank - Current Account, IPURE" AutoPostBack="true" CssClass="fund" /></td>
            </tr>

             <tr>
                 <td colspan="3"><asp:CheckBox runat="server" ID="chkApplyAccount" Text="Apply Account to All Entries:" AutoPostBack="true" CssClass="fund" /> <asp:DropDownList runat="server" ID="ddlAccountCode" DataTextField="AccountDescription" DataValueField="ID"
    AutoPostBack="false" AppendDataBoundItems="true" Font-Bold="true" Font-Size="Small"></asp:DropDownList></td>
                 <td colspan="2" hidden>
                     <asp:CheckBox runat="server" id="chkExpenseToInventory" Text="Set All Account Expenses to Inventory" AutoPostBack="true" CssClass="fund"/>
                 </td>

             </tr>

              <tr>
                <td Class="lblitemtitle" style="border-bottom:1px solid #000;"><asp:CheckBox runat="server" ID="chkRemittance" Text="Remittance" AutoPostBack="true" CssClass="fund" /></td>
                 <td></td>
                 <td></td>
                 <td></td>
                 <td></td>
             </tr>

             <asp:Table runat="server" ID="tblRemittances" style="width:100%;" Visible="false">
                 <asp:TableRow>
                     <asp:TableCell><asp:CheckBox runat="server" ID="chkDuetoBIRRemittance" Text="Due to BIR:Remittance" AutoPostBack="true" CssClass="fund" />
                         <br /> <asp:TextBox runat="server" ID="txtRemitBIR"></asp:TextBox>
                     </asp:TableCell>
                     <asp:TableCell><asp:CheckBox runat="server" ID="chkDuetoIntelicareRemittance" Text="Due to INTELICARE:Remittance" AutoPostBack="true" CssClass="fund" />
                         <br /> <asp:TextBox runat="server" ID="txtRemitIntelicare"></asp:TextBox>
                     </asp:TableCell>
                     <asp:TableCell><asp:CheckBox runat="server" ID="chkDuetoPhilHealthRemittance" Text="Due to PHIL. HEALTH:Remittance" AutoPostBack="true" CssClass="fund" />
                         <br /> <asp:TextBox runat="server" ID="txtRemitPhilHealth"></asp:TextBox>
                     </asp:TableCell>
                     <asp:TableCell><asp:CheckBox runat="server" ID="chkDuetoPagibigRemittance" Text="Due to PAG-IBIG:Remittance" AutoPostBack="true" CssClass="fund" />
                         <br/> <asp:TextBox runat="server" ID="txtRemitPagibig"></asp:TextBox> 
                     </asp:TableCell>
                     <asp:TableCell><asp:CheckBox runat="server" ID="chkDuetoGSISRemittance" Text="Due to GSIS" AutoPostBack="true" CssClass="fund" />
                         <br /> <asp:TextBox runat="server" ID="txtRemitGSIS"></asp:TextBox>
                     </asp:TableCell>
                 </asp:TableRow>
             </asp:Table>

              <tr>
                <td Class="lblitemtitle" style="border-bottom:1px solid #000;"><asp:CheckBox runat="server" ID="chkLiabilities" Text="Liabilities" AutoPostBack="true" CssClass="fund" /></td>
                 <td></td>
                 <td></td>
                 <td></td>
                 <td></td>
             </tr>

          <asp:table runat="server" id="tblLiabilities" style="width:100%;" Visible="false">
          <asp:TableRow>
               <asp:TableCell><asp:CheckBox runat="server" ID="chkDuetoBIR" Text="Due to BIR - For Payroll " AutoPostBack="true" CssClass="fund" />
                   <br /><asp:TextBox runat="server" ID="txtBIR"></asp:TextBox></asp:TableCell>
                 <asp:TableCell><asp:CheckBox runat="server" ID="chkDueToIntelicare" Text="Due to INTELLICARE/INA" AutoPostBack="true" CssClass="fund" />
                   <br /><asp:TextBox runat="server" ID="txtIntelicare"></asp:TextBox></asp:TableCell>
               <asp:TableCell><asp:CheckBox runat="server" ID="chkDueToPhilHealth" Text="Due to PHIL. HEALTH" AutoPostBack="true" CssClass="fund" />
                    <br /><asp:TextBox runat ="server" ID="txtPhilHealth"></asp:TextBox></asp:TableCell>
               <asp:TableCell><asp:CheckBox runat="server" ID="chkDueToPagibig" Text="Due to PAG-IBIG" AutoPostBack="true" CssClass="fund" />
                    <br /><asp:TextBox runat ="server" ID="txtPagibig"></asp:TextBox></asp:TableCell>
                <asp:TableCell><asp:CheckBox runat="server" ID="chkDueToGsis" Text="Due to GSIS" AutoPostBack="true" CssClass="fund" />
                    <br /><asp:TextBox runat ="server" ID="txtGsis"></asp:TextBox></asp:TableCell>  

              <asp:TableCell><asp:CheckBox runat="server" ID="chkDuetoCashAdvance" Text="Cash Advances" AutoPostBack="true" CssClass="fund" />
                    <br /><asp:TextBox runat ="server" ID="txtCashAdvances"></asp:TextBox></asp:TableCell>  
            </asp:TableRow>
         </asp:table>

             <%--Personal checkbox HIDDEN 04282022--%>
             <tr hidden>
                <td Class="lblitemtitle" style="border-bottom:1px solid #000;"><asp:CheckBox runat="server" ID="chkPersonal" Text="Personal" AutoPostBack="true" CssClass="fund" /></td>
                 <td></td>
                 <td></td>
                 <td></td>
                 <td></td>
             </tr>
             <asp:table runat="server" id="tblPersonal" style="width:100%;" Visible="false">
          <asp:TableRow>
               <asp:TableCell><asp:CheckBox runat="server" ID="chkNewAmount" Text="Change Total - Due to Personal Payments" AutoPostBack="true" CssClass="fund" />
                   <br /><asp:TextBox runat="server" ID="txtNewAmount"></asp:TextBox></asp:TableCell>
               <%--  <asp:TableCell><asp:CheckBox runat="server" ID="CheckBox2" Text="Due to INTELLICARE/INA" AutoPostBack="true" CssClass="fund" />
                   <br /><asp:TextBox runat="server" ID="TextBox2"></asp:TextBox></asp:TableCell>
               <asp:TableCell><asp:CheckBox runat="server" ID="CheckBox3" Text="Due to PHIL. HEALTH" AutoPostBack="true" CssClass="fund" />
                    <br /><asp:TextBox runat ="server" ID="TextBox3"></asp:TextBox></asp:TableCell>
               <asp:TableCell><asp:CheckBox runat="server" ID="CheckBox4" Text="Due to PAG-IBIG" AutoPostBack="true" CssClass="fund" />
                    <br /><asp:TextBox runat ="server" ID="TextBox4"></asp:TextBox></asp:TableCell>
                <asp:TableCell><asp:CheckBox runat="server" ID="CheckBox5" Text="Due to GSIS" AutoPostBack="true" CssClass="fund" />
                    <br /><asp:TextBox runat ="server" ID="TextBox5"></asp:TextBox></asp:TableCell>  --%>
            </asp:TableRow>
         </asp:table>

            <%-- Billables checkbox HIDDEN 04282022
             <tr>
                <td Class="lblitemtitle" style="border-bottom:1px solid #000;"><asp:CheckBox runat="server" ID="chkBillable" Text="Billables" AutoPostBack="true" CssClass="fund" /></td>
                 <td></td>
                 <td></td>
                 <td></td>
                 <td></td>
             </tr>--%>
             <asp:table runat="server" id="tblBillable" style="width:100%;" Visible="false">
          <asp:TableRow>
               <asp:TableCell><asp:CheckBox runat="server" ID="chkBill" Text="Bills" AutoPostBack="true" CssClass="fund" />
                   <br /><asp:TextBox runat="server" ID="TextBox1"></asp:TextBox></asp:TableCell>
               <%--  <asp:TableCell><asp:CheckBox runat="server" ID="CheckBox2" Text="Due to INTELLICARE/INA" AutoPostBack="true" CssClass="fund" />
                   <br /><asp:TextBox runat="server" ID="TextBox2"></asp:TextBox></asp:TableCell>
               <asp:TableCell><asp:CheckBox runat="server" ID="CheckBox3" Text="Due to PHIL. HEALTH" AutoPostBack="true" CssClass="fund" />
                    <br /><asp:TextBox runat ="server" ID="TextBox3"></asp:TextBox></asp:TableCell>
               <asp:TableCell><asp:CheckBox runat="server" ID="CheckBox4" Text="Due to PAG-IBIG" AutoPostBack="true" CssClass="fund" />
                    <br /><asp:TextBox runat ="server" ID="TextBox4"></asp:TextBox></asp:TableCell>
                <asp:TableCell><asp:CheckBox runat="server" ID="CheckBox5" Text="Due to GSIS" AutoPostBack="true" CssClass="fund" />
                    <br /><asp:TextBox runat ="server" ID="TextBox5"></asp:TextBox></asp:TableCell>  --%>
            </asp:TableRow>
         </asp:table>
         </table>

        





     </div>
    </asp:Panel>

    <%-- ACCOUNT ENTRIES --%>
    <asp:GridView runat="server" ID="grdAcctEntries" AutoGenerateColumns="false" AllowPaging="false" CssClass="grid" Width="100%">
        <EmptyDataTemplate>
            <b>No Account Entries found</b>
        </EmptyDataTemplate>
        <Columns>
           <%-- <asp:BoundField HeaderText="Responsibility Center" DataField="ResponsibilityCenter" ItemStyle-Width="15%" />--%>
            <asp:BoundField HeaderText="Account Title" DataField="AcctsAndExplanation" ItemStyle-Width="47.5%" />
            <asp:BoundField HeaderText="Account Code" DataField="AccountCode" ItemStyle-Width="10%" />
            <%--<asp:BoundField HeaderText="Ref" DataField="Ref" ItemStyle-Width="10%" />--%>
            <asp:BoundField HeaderText="Debit" DataField="Debit" ItemStyle-Width="16.25%" DataFormatString="{0:#,##0.00}" />
            <asp:BoundField HeaderText="Credit" DataField="Credit" ItemStyle-Width="16.25%" DataFormatString="{0:#,##0.00}" />
        </Columns>
    </asp:GridView>
    <table style="width: 100%;">
        <tr>
            <td style="width: 64%; text-align: right;" class="lblitemtitle">TOTAL</td>
            <td style="width: 18%; text-align: left;">
                <asp:Label runat="server" ID="lbltotaldebit" CssClass="total" DataFormatString="{0:#,##0.00}"></asp:Label></td>
            <td style="width: 18%; text-align: left;">
                <asp:Label runat="server" ID="lbltotalcredit" CssClass="total" DataFormatString="{0:#,##0.00}"></asp:Label></td>
        </tr>
    </table>

    <%-- SIGNATORIES --%>
    <div class="float-left;">
        <h2 class="page-title">Signatories: <span></span></h2>
    </div>

    <%--OLD SIGNATORY - COMPLETE FROM A TO F 04282022--%>
    <table style="width: 95%; margin: 6px 0px 0px; display: none;" class="obrform">
        <tr>
            <%--<td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label2" runat="server" Text="Block A - "></asp:Label></td>
            <td style="width: 30%">
                <uc8:signatorySelection runat="server" ID="signatorySelectionA" />
            </td>--%>

            <td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label6" runat="server" Text="Block B - "></asp:Label></td>
            <td style="width: 30%"><uc8:signatorySelection runat="server" ID="signatorySelectionB" />
            </td>
        </tr>

        <tr>
            <%--<td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label3" runat="server" Text="Block C - "></asp:Label></td>
            <td style="width: 30%">
                <uc8:signatorySelection runat="server" ID="signatorySelectionC" />
            </td>
            <td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label4" runat="server" Text="Block D - "></asp:Label></td>
            <td style="width: 30%">
                <uc8:signatorySelection runat="server" ID="signatorySelectionD" />
            </td>--%>
        </tr>

        <tr>
            <td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label5" runat="server" Text="Block E - "></asp:Label></td>
            <td style="width: 30%">
                <uc8:signatorySelection runat="server" ID="signatorySelectionE" />
            </td>

            <td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label7" runat="server" Text="Block F - "></asp:Label></td>
            <td style="width: 30%"><uc8:signatorySelection runat="server" ID="signatorySelectionF" />
            </td>
        </tr>

        <tr style="border-top: 1px solid rgba(0,0,0,0.3);">
            <%--<td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label8" runat="server" Text="Certified By - "></asp:Label></td>
            <td style="width: 30%">
                <uc8:signatorySelection runat="server" ID="signatorySelectionG" />
            </td>--%>

            <%--<td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
               <asp:Label ID="Label10" runat="server" Text="Approved By - "></asp:Label></td>
            <td style="width: 30%"><uc8:signatorySelection runat="server" ID="signatorySelectionH" />
            </td>--%>
        </tr>
    </table>

    <%--NEW SIGNATORY - A,C,D Present 04282022--%>
    <table style="width: 95%; margin: 6px 0px 0px;" class="obrform">
        <tr>
            <td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label11" runat="server" Text="Block A - "></asp:Label></td>
            <td style="width: 30%">
                <uc8:signatorySelection runat="server" ID="signatorySelectionA" />
            </td>

            <td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label13" runat="server" Text="Block B - "></asp:Label></td>
            <td style="width: 30%">
                <uc8:signatorySelection runat="server" ID="signatorySelectionC" />
            </td>
        </tr>

        <tr>
            
            <td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label14" runat="server" Text="Block C - "></asp:Label></td>
            <td style="width: 30%">
                <uc8:signatorySelection runat="server" ID="signatorySelectionD" />
            </td>
        </tr>

        <tr style="border-top: 1px solid rgba(0,0,0,0.3);">
            <td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
                <asp:Label ID="Label17" runat="server" Text="Certified By - "></asp:Label></td>
            <td style="width: 30%">
                <uc8:signatorySelection runat="server" ID="signatorySelectionG" />
            </td>

            <td style="width: 7%; text-align: right; padding: 5px 0px 0px;">
               <asp:Label ID="Label18" runat="server" Text="Approved By - "></asp:Label></td>
            <td style="width: 30%"><uc8:signatorySelection runat="server" ID="signatorySelectionH" />
            </td>
        </tr>
    </table>

</asp:Panel>
<br />

<asp:Button runat="server" ID="btnDisregard" Text="Disregard Creation of Disbursement Voucher" />
<ajax:ConfirmButtonExtender runat="server" ID="cbeDisregard" TargetControlID="btnDisregard"
    ConfirmText="All changes made will not be saved. Continue?">
</ajax:ConfirmButtonExtender>
&nbsp;<asp:Button runat="server" ID="btnSave" Text="Save Disbursement Voucher" />
<%--<ajax:ConfirmButtonExtender runat="server" ID="cbeSave" TargetControlID="btnSave"
    ConfirmText="Are you sure on saving Disbursement Voucher?">
</ajax:ConfirmButtonExtender>--%>

<uc7:InfoWindow runat="server" ID="ucInfoWindow" />
<uc10:ConfirmExtender runat="server" ID="ucConfirmExtender" />
<uc10:ConfirmExtender runat="server" ID="ucConfirmExtenderDetail" />
