<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlCreateObligationRequest.ascx.vb" Inherits="DbVoucher_Mark1.ctrlCreateObligationRequest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/custom controls/ctrlPayee.ascx" TagName="payeeSelection" TagPrefix="uc1" %>
<%@ Register Src="~/custom controls/ctrlParticular.ascx" TagName="particularSelection" TagPrefix="uc2" %>
<%@ Register Src="~/custom controls/ctrtDivision.ascx" TagName="divisionSelection" TagPrefix="uc3" %>
<%@ Register Src="~/custom controls/ctrlPPA.ascx" TagName="ppaSelection" TagPrefix="uc4" %>
<%@ Register Src="~/custom controls/ctrlParticularTemplate.ascx" TagName="particularSelection" TagPrefix="uc5" %>

<%@ Register Src="~/custom controls/ctrlSignatory.ascx" TagName="signatorySelection" TagPrefix="uc6" %>
<%@ Register Src="~/custom controls/ctrlInfoMessage.ascx" TagName="InfoPopMessage" TagPrefix="uc7" %>

     

<asp:Panel runat="server" ID="pnlMain">

    <asp:Label runat="server" ID="lblORError" ForeColor="Red"></asp:Label>
    <asp:Panel runat="server" ID="pnlparticulardesc" GroupingText="Obligation Request Particulars" Width="49%" CssClass="float-right">
        <div class="float-right" style="width: 100%;">
            <table style="width: 100%; margin-top: -2px;" class="obrform">
                <tr>
                    <td style="width: 30%;">
                        <uc5:particularSelection runat="server" ID="particularSelection" />
                    </td>


                    <td style="text-align: right; vertical-align: middle; padding: 5px 3px 0px 0px;">
                        <asp:Label ID="Label4" runat="server" CssClass="obrform" Style="text-align: right; margin-top: 3px;">Allotment Object Class: </asp:Label>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:RadioButtonList ID="rdbObjectClass" runat="server" RepeatDirection="Horizontal" CellPadding="3" CellSpacing="3" TextAlign="Right" CssClass="radiolist" AutoPostBack="false">
                            <asp:ListItem Text="PS" Value="100"></asp:ListItem>
                            <asp:ListItem Text="MOOE" Value="200"></asp:ListItem>
                            <asp:ListItem Text="CO" Value="300"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:Label runat="server" ID="lblObjectClass" Text="" Visible="false"></asp:Label>
                    </td>


                </tr>
                <tr>
                    <td style="width: 95%;" colspan="3">
                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtObrTemplate" Rows="4" Width="625"></asp:TextBox></td>
                </tr>
            </table>
            <%--  <asp:Label runat="server" ID="lblTemplate" Visible="false"></asp:Label>--%>
            <asp:Label runat="server" ID="lblDesc" Visible="false"></asp:Label>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPayee" GroupingText="Payee Information" Width="50%">
        <div class="float-left">
            <table style="width: 100%;" class="obrform">
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="Label1" runat="server">Payee: </asp:Label></td>
                    <td>
                        <uc1:payeeSelection runat="server" ID="ucPayeeSelection" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server">Office:</asp:Label></td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPayeeOffice" ReadOnly="true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server">Address:</asp:Label></td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPayeeAddress" ReadOnly="true"></asp:TextBox></td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <%--<asp:Label ID="Label5" runat="server" CssClass="obrform"><span>Source Documents:</span> </asp:Label>&nbsp; <asp:TextBox runat="server" ID="txtsourcedocu" Width="98.5%"></asp:TextBox>--%>
 

    <table style="width: 100%; margin: -5px 0px 0px;" class="obrform">
        <tr>
            <td style="width: 7%; padding-top:5px;">
                 Fund Cluster:</td>
              <td style="width: 15%">
                  <%--OLD, STATIC DATA--%>
                  <%--<asp:DropDownList ID="FundSourceSelection" runat="server">
                      <asp:ListItem Value="-1">SELECT UACS FUNDING SOURCE</asp:ListItem>
                      <asp:ListItem Value="01101101">01101101 - Current Agency Specific Budget</asp:ListItem>
                      <asp:ListItem Value="01104102">01104102 - Retirement and Life Insurance Premium</asp:ListItem>
                      <asp:ListItem Value="01101406">01101406 - Misc Personnel Benefit Fund</asp:ListItem>
                  </asp:DropDownList>--%>

                  <asp:DropDownList runat="server" ID="ddlFundSourceSelection" DataTextField="UACSCodeDesc" DataValueField="ID" AutoPostBack="false" AppendDataBoundItems="true"></asp:DropDownList>
            </td>
        </tr>
        <tr>
             <td style="width: 7%; padding-top:5px;">
                <asp:Label ID="Label7" runat="server" Text="Date Created:"></asp:Label></td>
              <td style="width: 15%">
                 <ajax:CalendarExtender runat="server" ID="ceDateFrom" PopupButtonID="imgDateCreated" TargetControlID="txtDateCreated">
                 </ajax:CalendarExtender>
                 <asp:TextBox runat="server" ID="txtDateCreated" Width="150"></asp:TextBox>
                 <img src="../../../Images/calendar.jpg" alt="" id="imgDateCreated" style="margin-top:-20px;"/>
            </td>

            <td style="width: 5%; padding-top:5px;">
                <asp:Label runat="server" Text="Block A - "></asp:Label></td>
            <td style="width: 29%">
                <uc6:signatorySelection runat="server" ID="ucsignatorySelectionA" />
            </td>
            <td style="width: 5%; padding-top:5px;">
                <asp:Label ID="Label6" runat="server" Text="Block B - "></asp:Label></td>
            <td style="width: 29%">
                <uc6:signatorySelection runat="server" ID="ucsignatorySelectionB" />
            </td>
          
        </tr>
    </table>

<br />

    <div class="float-right" style="margin: 3px 0px 0px 0px;">
        <td style="width: 20%; text-align: right;">
            <asp:Button runat="server" ID="btnAddTrans" Text="Add Transaction" /></td>
    </div>

<br />
    <div class="float-left;">
        <h2 class="page-title">Transaction Entries: <span>Create account entries for this request:</span></h2>
    </div>
    <asp:Table ID="tblParticular" runat="server" class="gridobr" Width="100%">
        <asp:TableRow>
            <asp:TableHeaderCell></asp:TableHeaderCell>
            <asp:TableHeaderCell Width="18%">Responsibility Center</asp:TableHeaderCell>
            <asp:TableHeaderCell Width="30%">Transaction Entries</asp:TableHeaderCell>
            <%-- <asp:TableHeaderCell Width="220">Source Document</asp:TableHeaderCell>--%>
            <asp:TableHeaderCell Width="18%">PPA</asp:TableHeaderCell>
            <asp:TableHeaderCell Width="16%">Account Code</asp:TableHeaderCell>
            <asp:TableHeaderCell Width="18%">Amount</asp:TableHeaderCell>
        </asp:TableRow>
        <asp:TableRow ID="rowTotal">
            <asp:TableCell ColumnSpan="5" CssClass="totalamt" Font-Bold="true" HorizontalAlign="Right" VerticalAlign="Middle" Font-Size="Large">Total Amount</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox runat="server" ID="txtTotalAmount" Enabled="true" Width="250" ></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <div style="float:right;padding:5px; border:1px solid #ccc; width:350px;margin:0px 0px 0px 0px;vertical-align:middle; text-align:left;">

       <asp:CheckBox runat="server" ID="chkPriorityTransactions" text="" /><span style="display:inline-block;font-size:10px; letter-spacing:1px;margin:-15px 0px 0px 3px;">PRIORITY TRANSACTION </span>&nbsp;&nbsp;|&nbsp;&nbsp; 
       <asp:CheckBox runat="server" ID="chkNoOBRNum" Text="" AutoPostBack="true"/><span style="display:inline-block;font-size:10px; letter-spacing:1px;margin:-15px 0px 0px 3px;">NO OBR NUMBER TO SET </span>
        <br />
    <asp:Button runat="server" ID="btnSaveObrType2" Text="Save Transaction" Visible="false"/>
    </div>
</asp:Panel>



<uc7:InfoPopMessage runat="server" ID="ucInfoPop" />

<asp:Button runat="server" ID="btnBack" Text="Back" />
<ajax:ConfirmButtonExtender runat="server" ID="cbeBack" TargetControlID="btnBack"
    ConfirmText="All changes made will not be saved. Continue?">
</ajax:ConfirmButtonExtender>
&nbsp;<asp:Button runat="server" ID="btnSave" Text="Send Obligation Request to process queue" />
<ajax:ConfirmButtonExtender runat="server" ID="cbeSave" TargetControlID="btnSave"
    ConfirmText="Are you sure you want to contine sending Obligation Request?">
</ajax:ConfirmButtonExtender>
&nbsp;<asp:Button runat="server" ID="btnUpdate" Text="Send Updated Obligation Request to process queue" />
<ajax:ConfirmButtonExtender runat="server" ID="cbeUpdate" TargetControlID="btnUpdate"
    ConfirmText="Are you sure of the changes made?">
</ajax:ConfirmButtonExtender>


