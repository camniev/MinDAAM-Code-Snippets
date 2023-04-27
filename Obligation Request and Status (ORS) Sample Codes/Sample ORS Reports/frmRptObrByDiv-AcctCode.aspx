<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmRptObrByDiv-AcctCode.aspx.vb" Inherits="DbVoucher_Mark1.frmRptObrByDiv_AcctCode" %>
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
        <h2 class="page-title">Obligation Request Reports: <span>Generate List of Particular Details By Division and Account Codes</span></h2>
        <div class="float-left;">
        <table style="width:45%;float:left;" class="tblGenReport">
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
                        <asp:ListItem Value="2" Text="Verified"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Cancelled"></asp:ListItem>
                        <asp:ListItem Value="6" Text="DV Created"></asp:ListItem>
                        </asp:RadioButtonList>

                    </td>
               </tr> 
              <tr>
              <td colspan="3">
              <asp:Button runat="server" ID="btnGenerate" Text="View List" Height="80" Width="200"/> <asp:Button runat="server" ID="btnPrint" Text="Print List" Height="80" Width="200"/>
              </td>  
              </tr>


            <table id="searchfieldstbl" style="width:55%; float:left;">
                    <tr>
                        <td style="width:15%;"><span class="lblitemtitle">SEARCH FIELDS: </span></td>
                    </tr>
                 <tr>
                     <td style="width:15%;">
                         <span class="lblitemtitle">Responsibility Center / Division: </span> 
                     </td>
                     <td>
                         <asp:DropDownList runat="server" ID="ddlDivision" DataTextField="Division" DataValueField="ID"
    AutoPostBack="false" AppendDataBoundItems="true"></asp:DropDownList>
                     </td>
                 </tr>
                <tr>
                     <td></td>
                     <td><asp:CheckBox runat="server" ID="chkAllRespoCenter" CssClass="checkbox" AutoPostBack="true"/><span class="lblitemtitle" style="color:#333;font-size:14px;">All Responsibility Center/Division</span></td>
                    </tr>
                    <tr>
                     <td></td>
                     <td></td>
                    </tr>
                     <tr>
                     <td>&nbsp;</td>
                     <td>&nbsp;</td>
                    </tr>
                    <tr>
                     <td style="width:15%;">
                         <span class="lblitemtitle">Account Code: </span> 
                     </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlAccountCode" DataTextField="AccountDescription" DataValueField="ID"
    AutoPostBack="false" AppendDataBoundItems="true"></asp:DropDownList>
                        </td>
                 </tr>
                 <tr>
                     <td></td>
                     <td><asp:CheckBox runat="server" ID="chkAllAcounts" CssClass="checkbox" AutoPostBack="true"/><span class="lblitemtitle" style="color:#333;font-size:14px;">All Accounts</span></td>
                    </tr>
                    <tr>
                     <td></td>
                     <td></td>
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
            </table> 
        </div>

        <hr style="float:left; width:100%;"/>

        <div style="width:100%;">
        <asp:GridView runat="server" ID="grdReport" AutoGenerateColumns="false " AlternatingRowStyle-BackColor="DarkGray">
             <EmptyDataTemplate>
                <b>No Record/s found!</b>
            </EmptyDataTemplate>
            <Columns>
               <%-- <asp:TemplateField ItemStyle-Width="5%">
                    <ItemTemplate>              
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrintObR" Text="View ObR" CommandName="xPrintObR" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:BoundField HeaderText="Status" DataField="xStatus" ItemStyle-Width="55px" SortExpression="" HeaderStyle-Font-Size="XX-Small" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="XX-Small"/>
                <asp:BoundField HeaderText="OBR No." DataField="ObligationRequestNo" ItemStyle-Width="80px" SortExpression=""/>
                <asp:BoundField HeaderText="A-Obj-C" DataField="xAllotmentObjectClass" ItemStyle-Width="50px" SortExpression="" HeaderStyle-Font-Size="XX-Small" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField HeaderText="Payee Name" DataField="PayeeName" ItemStyle-Width="400px" SortExpression=""/>
                <asp:BoundField HeaderText="Division" DataField="xRespoCenter" ItemStyle-Width="80px" SortExpression="" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField HeaderText="AccountCode" DataField="xAccountCode" ItemStyle-Width="110px" SortExpression="" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField HeaderText="Account Description" DataField="xAccountDesc"  ItemStyle-Width="450px" SortExpression="" />
                <asp:BoundField HeaderText="Amount" DataField="Amount" ItemStyle-Width="50px" DataFormatString="{0:#,##0.00}" SortExpression="TotalAmount" ItemStyle-HorizontalAlign="Right"/>
            </Columns>
        </asp:GridView>


        <rsweb:ReportViewer runat="server" ID="rptViewReport" ShowPrintButton="true" Width="100%" Height="">
        </rsweb:ReportViewer>
        </div>






    </asp:Panel>

</asp:Content>
