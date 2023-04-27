<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmRptCreatedObr.aspx.vb" Inherits="DbVoucher_Mark1.frmRptCreatedObr" EnableEventValidation="false" %>

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
        <h2 class="page-title">Obligation Request: <span>Generate List of Obligation Requests Records</span></h2>


        <div class="float-left;">
            <table style="width:48%;float:left;" class="tblGenReport">
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
                        <asp:ListItem Value="1" Text="All Created OBR" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="7" Text="OBR Status: Created"></asp:ListItem>
                        <asp:ListItem Value="2" Text="OBR Status: Verified"></asp:ListItem>
                        <asp:ListItem Value="3" Text="OBR Status: Cancelled"></asp:ListItem>
                        <asp:ListItem Value="99" Text="OBR No Cheques"></asp:ListItem>
                        <%--<asp:ListItem Value="6" Text="DV Created"></asp:ListItem>--%>
                        </asp:RadioButtonList>

                    </td>
               </tr> 
              <tr>
              <td colspan="3"><asp:Button runat="server" ID="btnGenerate" Text="View List" Height="80" Width="200"/> <asp:Button runat="server" ID="btnPrint" Text="Print List" Height="80" Width="200"/></td>  
              </tr>
            </table> 


            <table style="width:52%; float:left;">
                    <tr>
                        <td style="width:15%;"><span class="lblitemtitle">SEARCH FIELDS: </span></td>
                    </tr>

                 <tr>
                     <td style="width:15%;"> <span class="lblitemtitle">ObR Number:</span></td>
                     <td> <asp:TextBox runat="server" ID="txtObRNumber" Width="90%"></asp:TextBox></td>
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
                         <span class="lblitemtitle">Particulars Field (search keywords): </span> 
                     </td>
                     <td>
                         <asp:TextBox runat="server" ID="txtSearchParticulars" Width="90%"></asp:TextBox>
                     </td>
                 </tr>
                <%-- CHKPARTICULARS HIDDEN 07152022
                    <tr>
                    <td style="width:15%;">

                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkParticulars" CssClass="checkbox" AutoPostBack="true"/><span class="lblitemtitle" style="color:#333;font-size:14px;">Search only Particulars</span>
                    </td>
                </tr>--%>
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
                     <td></td>
                    </tr>
                    <tr>
                     <td><asp:Button runat="server" ID="btnExportToExcell" Visible="false" /></td>
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
        <asp:GridView runat="server" ID="grdReport" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="DarkGray">
              <EmptyDataTemplate>
                <b>No Record/s found!</b>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-Width="5%">
                    <ItemTemplate>              
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrintObR" Text="View ObR" CommandName="xPrintObR" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Control Number" DataField="ObligationRequestNo" ItemStyle-Width="8%" SortExpression="ObligationRequestNo" HeaderStyle-Font-Size="XX-Small"/>
                <asp:BoundField HeaderText="All. Obj. Class" DataField="AllotmentObjectClass" ItemStyle-Width="5%" SortExpression="ObligationRequestNo" HeaderStyle-Font-Size="XX-Small"/>
                <asp:BoundField HeaderText="Responsibility Center" DataField="xResponsibilityCenterDivision" ItemStyle-Width="20%" SortExpression="xResponsibilityCenterDivision" />
                <asp:BoundField HeaderText="Payee" DataField="PayeeName" ItemStyle-Width="30%" SortExpression="PayeeName"/>
                <asp:BoundField HeaderText="Particulars" DataField="xDescription" ItemStyle-Width="43%" SortExpression="Description" />
                <asp:BoundField HeaderText="Date Created" DataField="DateCreated" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%" SortExpression="DateCreated" />
                <asp:BoundField HeaderText="Total Amount" DataField="TotalAmount" ItemStyle-Width="7%" DataFormatString="{0:#,##0.00}" SortExpression="TotalAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/>
                 <asp:BoundField HeaderText="Status Code" DataField="xStatus" ItemStyle-Width="7%" SortExpression=""  ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" HeaderStyle-Font-Size="XX-Small"/>
            </Columns>
        </asp:GridView>
        </div>


        <asp:Panel runat="server" ID="pnlViewObR">
        <rsweb:ReportViewer runat="server" ID="rptViewCreatedObR" ShowPrintButton="true" Width="100%">
        </rsweb:ReportViewer>
        </asp:Panel>


         <asp:Panel runat="server" ID="pnlPrintObR" Visible="false">
        <rsweb:ReportViewer ID="rptViewer" runat="server" Width="100%" Height="100%" Enabled="true" ShowPrintButton="true" Visible="true" AsyncRendering="False" SizeToReportContent="True">
            <LocalReport ReportPath="~/Auth/Transactions/rdlc/rptObligationRequest.rdlc" ></LocalReport>
           <%-- <LocalReport ReportPath="~/DVAReports/rptObligationRequest.rdlc"></LocalReport>--%>
        </rsweb:ReportViewer>
        <asp:Button ID="Button3" runat="server" OnClientClick="javascript:window.print();" Text="Print" Visible="false"/>

    </asp:Panel>
      
    </asp:Panel>
</asp:Content>
