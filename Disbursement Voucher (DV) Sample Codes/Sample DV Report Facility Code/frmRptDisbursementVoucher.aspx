<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmRptDisbursementVoucher.aspx.vb" Inherits="DbVoucher_Mark1.frmRptDisbursementVoucher" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/custom controls/ctrlInfoMessage.ascx" TagName="ctrlInfoWindow" TagPrefix="uc1" %>




<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="featuredContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="headerreports"></div>
     <asp:Panel runat="server" ID="pnlMain">
        <h2 class="page-title">Disbursement Voucher: <span>Generate List of Disbursement Voucher Records</span></h2>


          <div class="float-left;">
            <table style="width:45%; float:left;" class="tblGenReport">
              <tr>
                    <td style="font-weight:bold;">Date Range:&nbsp;</td>
                    <td><p>Records From: </p></td>
                    <td><p>Records To:</p> </td>
                    <td style="font-weight:bold;"><p style="line-height:13px;">Status:</p></td>
              </tr>

              <tr>
                    <td></td>
                    <td style="height:20px;">
                           <ajax:CalendarExtender runat="server" ID="ceDateFrom" PopupButtonID="imgDateFrom"
                                TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                            </ajax:CalendarExtender>
                            <asp:TextBox runat="server" ID="txtDateFrom"></asp:TextBox>
                            <img src="../../../Images/calendar.jpg" alt="" id="imgDateFrom" style="margin-top:-20px;"/>
                    </td>
                    <td style="height:20px;"> 
                          <ajax:CalendarExtender runat="server" ID="ceDateTo" PopupButtonID="imgDateTo"
                            TargetControlID="txtDateTo" Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                        <asp:TextBox runat="server" ID="txtDateTo"></asp:TextBox>
                        <img src="../../../Images/calendar.jpg" alt="" id="imgDateTo" style="margin-top:-20px;"/>
                    </td>
                    <td rowspan="4" style="padding:3px;">
                        <asp:RadioButtonList runat="server" ID="rdbtnStatus" CssClass="rdstatus" style="margin:0px;">
                        <asp:ListItem Value="1" Text="Created" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Approved"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Cancelled"></asp:ListItem>
                        <asp:ListItem Value="5" Text="On Cheque"></asp:ListItem>
                        </asp:RadioButtonList>

                    </td>
               </tr> 
              <tr>
              <td colspan="3" ><asp:Button runat="server" ID="btnGenerate" Text="View List" Height="80" Width="200" /> <asp:Button runat="server" ID="btnPrint" Text="Print List" Height="80" Width="200"/></td>  
              </tr>
            </table> 

                <table style="width:55%; float:left;">
                    <tr>
                        <td style="width:15%;"><span class="lblitemtitle">SEARCH FIELDS: </span></td>
                    </tr>
                  <tr>
                     <td><span class="lblitemtitle">DV Number: </span></td>
                     <td><asp:TextBox runat="server" ID="txtDvNumber" Width="90%"></asp:TextBox></td>
                    </tr> 
                 <tr>
                     <td style="width:15%;">
                         <span class="lblitemtitle">Payee Field: </span> 
                     </td>
                     <td>
                         <asp:TextBox runat="server" ID="txtSearchPayee" Width="90%"></asp:TextBox>
                     </td>
                 </tr>
                    <tr>
                     <td style="width:15%;">
                         <span class="lblitemtitle" style="color:#ccc;">AMOUNT Field: </span> 
                     </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSearchAmount" Width="90%" Enabled="false"></asp:TextBox>
                        </td>
                 </tr>
                    <tr>
                     <td></td>
                     <td>&nbsp;</td>
                    </tr>
                     <tr>
                     <td>&nbsp;</td>
                     <td>&nbsp;</td>
                    </tr>
                    <tr>
                     <td>&nbsp;</td>
                     <td>&nbsp;</td>
                    </tr>
             </table>

        </div>
        <hr style="float:left; width:100%;"/>
        <div style="width:100%;">
        <%--<asp:GridView runat="server" ID="grdReport" AutoGenerateColumns="false " AlternatingRowStyle-BackColor="DarkGray">
              <EmptyDataTemplate>
                <b>No Record/s found!</b>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-Width="5%">
                    <ItemTemplate>              
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrintDV" Text="View DV" CommandName="xPrintDv" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
              <asp:BoundField HeaderText="DV Number" DataField="DisbursementVoucherNo" ItemStyle-Width="7%" SortExpression="ObligationRequestNo"/>
                <asp:TemplateField ItemStyle-Width="5%">
                    <ItemTemplate>              
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrintObR" Text="View ObR" CommandName="xPrintObR" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="OBR Number" DataField="ObRNo" ItemStyle-Width="7%" SortExpression="ObligationRequestNo" HeaderStyle-Font-Size="XX-Small" ItemStyle-Font-Size="XX-Small"/>
                <asp:BoundField HeaderText="Payee" DataField="PayeeName" ItemStyle-Width="25%" SortExpression="PayeeName"/>
                <asp:BoundField HeaderText="Particulars" DataField="DVParticularTemplate" ItemStyle-Width="43%" SortExpression="Description" />
                <asp:BoundField HeaderText="Date Created" DataField="DateCreated" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="9%" SortExpression="DateCreated" />
                 <asp:BoundField HeaderText="Date Approved" DataField="DateApprovedForPayment" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="9%" SortExpression="DateCreated" />              
                <asp:BoundField HeaderText="Total Amount" DataField="ParticularsAmountDue" ItemStyle-Width="6%" DataFormatString="{0:#,##0.00}" SortExpression="TotalAmount" ItemStyle-HorizontalAlign="Right"/>
                 <asp:BoundField HeaderText="Status Code" DataField="xDvStatus" ItemStyle-Width="5%" SortExpression=""  ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" HeaderStyle-Font-Size="XX-Small"/>

           </Columns>
        </asp:GridView>--%>

            
            <asp:GridView runat="server" ID="grdReport" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="DarkGray">
                    <EmptyDataTemplate>
                    <b>No Record/s found!</b>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField ItemStyle-Width="5%">
                        <ItemTemplate>              
                            <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrintDV" Text="View DV" CommandName="xPrintDv" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="DV Number" DataField="DisbursementVoucherNo" ItemStyle-Width="8%" SortExpression="ObligationRequestNo" HeaderStyle-Font-Size="XX-Small"/>
                    <asp:TemplateField ItemStyle-Width="5%">
                        <ItemTemplate>              
                            <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrintObR" Text="View ObR" CommandName="xPrintObR" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="OBR Number" DataField="ObRNo" ItemStyle-Width="5%" SortExpression="ObligationRequestNo" HeaderStyle-Font-Size="XX-Small"/>
                    <asp:BoundField HeaderText="Payee" DataField="PayeeName" ItemStyle-Width="10%" SortExpression="xResponsibilityCenterDivision" />
                    <asp:BoundField HeaderText="Particulars" DataField="DVParticularTemplate" ItemStyle-Width="37%" SortExpression="Description" />
                    <asp:BoundField HeaderText="Date Created" DataField="DateCreated" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="8%" SortExpression="DateCreated" />
                    <%--<asp:BoundField HeaderText="Date Approved" DataField="DateApprovedForPayment" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="8%" SortExpression="DateCreated" />--%>
                    <asp:TemplateField HeaderText="Date Approved" ItemStyle-Width="5%">
                        <ItemTemplate>
                            <%# If(Eval("DateApprovedForPayment") Is Nothing, "Not approved yet", Eval("DateApprovedForPayment", "{0:dd/M/yyyy}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Total Amount" DataField="ParticularsAmountDue" ItemStyle-Width="7%" DataFormatString="{0:#,##0.00}" SortExpression="TotalAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/>
                    <asp:BoundField HeaderText="Status Code" DataField="xDvStatus" ItemStyle-Width="7%" SortExpression=""  ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" HeaderStyle-Font-Size="XX-Small"/>
                </Columns>
            </asp:GridView>
        </div>


        <asp:Panel runat="server" ID="pnlViewDV">
     
        <rsweb:ReportViewer runat="server" ID="rptViewCreatedObR" ShowPrintButton="true" Width="100%" Height="600px">
        </rsweb:ReportViewer>
        </asp:Panel>

    <asp:Panel runat="server" ID="pnlPrintDV">
     
        <rsweb:ReportViewer ID="rptDVViewer" runat="server" Width="100%" Height="100%" AsyncRendering="False" SizeToReportContent="True" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Auth\Transactions\rdlc\rptDisbursementVoucher.rdlc"></LocalReport>
        </rsweb:ReportViewer>
        <asp:Button ID="Button1" runat="server" OnClientClick="javascript:window.print();" Text="Print" Visible="false"/>
    </asp:Panel>


          <asp:Panel runat="server" ID="pnlPrintObR">
       
        <rsweb:ReportViewer ID="rptViewer" runat="server" Width="100%" Height="100%" Enabled="true" ShowPrintButton="true" Visible="true" AsyncRendering="False" SizeToReportContent="True">
            <LocalReport ReportPath="Auth\Transactions\rdlc\rptObligationRequest.rdlc" ></LocalReport>
           <%-- <LocalReport ReportPath="~/DVAReports/rptObligationRequest.rdlc"></LocalReport>--%>
        </rsweb:ReportViewer>
        <asp:Button ID="Button3" runat="server" OnClientClick="javascript:window.print();" Text="Print" />

    </asp:Panel>

     </asp:Panel>













</asp:Content>
