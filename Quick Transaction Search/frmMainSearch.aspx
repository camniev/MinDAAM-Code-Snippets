<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmMainSearch.aspx.vb" Inherits="DbVoucher_Mark1.frmMainSearch" %>

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
        <h2 class="page-title">Transaction Search: <span>Check for Transaction Current Status</span></h2>


        <div class="float-left;">
            <table style="width:100%;float:left;" class="tblGenReport">
              <tr>
                    <td style="font-weight:bold; width:125px;">Documents Submitted the Month of:&nbsp;</td>
                    <td style="font-weight:bold; width:145px;"><asp:DropDownList runat="server" ID="ddlMonths" Font-Bold="true" CssClass="MonthlyReport">
                         <asp:ListItem Enabled="True" Selected="True" Text="January" Value="1" />
                        <asp:ListItem Enabled="True" Selected="False" Text="February" Value="2" />
                        <asp:ListItem Enabled="True" Selected="False" Text="March" Value="3" />
                        <asp:ListItem Enabled="True" Selected="False" Text="April" Value="4" />
                        <asp:ListItem Enabled="True" Selected="False" Text="May" Value="5" />
                        <asp:ListItem Enabled="True" Selected="False" Text="June" Value="6" />
                        <asp:ListItem Enabled="True" Selected="False" Text="July" Value="7" />
                        <asp:ListItem Enabled="True" Selected="False" Text="August" Value="8" />
                        <asp:ListItem Enabled="True" Selected="False" Text="September" Value="9" />
                        <asp:ListItem Enabled="True" Selected="False" Text="October" Value="10" />
                        <asp:ListItem Enabled="True" Selected="False" Text="November" Value="11" />
                        <asp:ListItem Enabled="True" Selected="False" Text="December" Value="12" />
                        </asp:DropDownList></td>
                     <td style="font-weight:bold; width:45px;">Year:&nbsp;</td>
                    <td style="font-weight:bold; width:105px;"><asp:DropDownList runat="server" ID="ddlYear" Font-Bold="true" CssClass="MonthlyReport">
                         <asp:ListItem Enabled="True" Selected="True" Text="2014" Value="2014" />
                        <asp:ListItem Enabled="True" Selected="False" Text="2015" Value="2015" />
                        <asp:ListItem Enabled="True" Selected="False" Text="2016" Value="2016" />
                        <asp:ListItem Enabled="True" Selected="False" Text="2017" Value="2017"/>
                        <asp:ListItem Enabled="True" Selected="False" Text="2018" Value="2018" />
                        <asp:ListItem Enabled="True" Selected="False" Text="2019" Value="2019" />
                        <asp:ListItem Enabled="True" Selected="False" Text="2020" Value="2020" />
                        <asp:ListItem Enabled="True" Selected="False" Text="2021" Value="2021" />
                        <asp:ListItem Enabled="True" Selected="False" Text="2022" Value="2022" />
                        <asp:ListItem Enabled="True" Selected="False" Text="2023" Value="2023" />
                        <asp:ListItem Enabled="True" Selected="False" Text="2024" Value="2024" />
                        <asp:ListItem Enabled="True" Selected="False" Text="2025" Value="2025" />
                        </asp:DropDownList>

                    </td>
                    <td style="width:125px;font-weight:bold;">Enter Payee Name: </td>
                    <td style="width:350px;"><asp:TextBox runat="server" ID="txtSearchName" Width="300"></asp:TextBox></td>
                        
                     <td><asp:Button runat="server" ID="btnGenerate" Text="Search for Transaction" Height="40" Width="350"/> <asp:Button runat="server" ID="btnPrint" Text="Print List" Height="40" Width="350" Visible="false"/></td>
              </tr>

              <tr>
                    <td><strong>LEGEND:</strong></td>
                    <td colspan="7"><strong>CREATED</strong> - Obligation Request Created | <strong>VERIFIED</strong> - Obligation Request Verified | <strong>CANCELLED</strong> - Obligation Request Cancelled<br />
                                    <strong>DV CREATED</strong> - Disbursement Voucher Created | <strong>DV APPROVED</strong> - Disbursement Voucher Approved | <strong>DV CANCELLED</strong> - Disbursement Voucher Cancelled<br />
                                    <strong>CH GENERATED</strong> - Cheque Created and Ready for Release | <strong>CH RELEASED</strong> - Cheque Released to Payee
                    </td>
                    <%--<td></td>
                    <td></td>--%>
               </tr> 
              <tr>
              <td colspan="3"></td>  
              </tr>
            </table> 


            <%--<table style="width:55%; float:left;">
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
             </table>--%>
        </div>

        <hr style="float:left; width:100%;"/>

        <div style="width:100%;">
        <asp:GridView runat="server" ID="grdReport" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="DarkGray" >
              <EmptyDataTemplate>
                <b>No Record/s found!</b>
            </EmptyDataTemplate>
            <Columns>
               <%-- <asp:TemplateField ItemStyle-Width="5%">
                    <ItemTemplate>              
                        <asp:LinkButton CssClass="listcontrols" runat="server" ID="lnkPrintObR" Text="View ObR" CommandName="xPrintObR" CommandArgument='<%# Eval("ID")%>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                 <asp:BoundField HeaderText="Date Started" DataField="DateCreated" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="80px" SortExpression="DateCreated" HeaderStyle-Font-Size="XX-Small" ItemStyle-VerticalAlign="Middle"/>
                <asp:BoundField HeaderText="Control Number" DataField="TransactionNumber" ItemStyle-Width="90px" SortExpression="" ItemStyle-VerticalAlign="Middle" HeaderStyle-Font-Size="XX-Small" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Medium" HeaderStyle-HorizontalAlign="Center"/>
                <asp:BoundField HeaderText="Payee" DataField="PayeeName" ItemStyle-Width="400px" SortExpression="PayeeName" ItemStyle-VerticalAlign="Middle"/>
                <asp:BoundField HeaderText="Particular" DataField="ParticularTemplate" ItemStyle-Width="500px" SortExpression="DateCancelled" HeaderStyle-Font-Size="XX-Small" ItemStyle-VerticalAlign="Top" />
                <asp:BoundField HeaderText="Total Amount" DataField="CHAmount" ItemStyle-Width="8%" DataFormatString="{0:N}" SortExpression="CHAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HtmlEncode="false"/>
                 <asp:BoundField HeaderText="Transaction Status" DataField="xStatus" ItemStyle-Width="150px" SortExpression=""  ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" HeaderStyle-Font-Size="XX-Small" ItemStyle-Font-Size="Medium" ItemStyle-VerticalAlign="Middle"/>
            </Columns>
        </asp:GridView>

            

        </div>

      
    </asp:Panel>


</asp:Content>
