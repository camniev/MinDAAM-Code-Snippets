<%@ Page Title="Division Library" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Division-Library.aspx.vb" Inherits="DbVoucher_Mark1.Division_Library" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="headerlib"> </div>

 <h2 class="page-title"><%: Title %>: <span>List of all responsibility centers in MinDA</span></h2>

   

    <div class="float-right" style="margin:5px 0px;">
    <asp:Button runat="server" ID="btnAdd" Text="Add New Division" />
    </div>

     <div class="float-left">
    <span style="letter-spacing:1px;font-weight:bold;vertical-align:middle;">SEARCH FOR:</span>&nbsp;<asp:TextBox runat="server" ID="txtSearch" CssClass="searchfield"></asp:TextBox>  <asp:Button runat="server" ID="btnSearch" Text="Search" />
    </div>

    <asp:GridView ID="grddivision" runat="server" CssClass="grid" AutoGenerateColumns="false" Width ="100%" AllowPaging="true" AllowSorting="true" PageSize="13">
        <EmptyDataTemplate>
            <b>No records!</b>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkEdit" Text="Edit" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xEdit" CssClass="listcontrols"></asp:LinkButton> | 
                    <asp:LinkButton runat="server" ID="lnkDelete" Text="Delete" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xDelete" CssClass="listcontrols"></asp:LinkButton> | 
                    <asp:LinkButton runat="server" ID="lnkActivate" Text="Display" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xActivate" CssClass="listcontrols"></asp:LinkButton> | 
                    <asp:LinkButton runat="server" ID="lnkDeactivate" Text="Hide" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xDeactivate" CssClass="listcontrols"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Division" HeaderText="Division" ItemStyle-Width="30%" SortExpression="Division"/>
            <asp:BoundField DataField="DivisionDesc" HeaderText="Division Description" ItemStyle-Width="40%" SortExpression="DivisionDesc"/>
            <%--<asp:BoundField DataField="IsActive" HeaderText="Status" ItemStyle-Width="10%" SortExpression="IsActive"/>--%>
            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Status">
                    <ItemTemplate>
                        <%# If(Eval("IsActive").ToString() = "1", String.Format("<p style='color: green; font-weight: bold;'>ACTIVE</p>"), String.Format("<p <p style='color: red; font-weight: bold;'>HIDDEN</p>"))%>
                    </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>


    <ajax:ModalPopupExtender runat="server" ID="mpeDivision" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" TargetControlID="btnAdd" PopupControlID="pnlAddDivision" PopupDragHandleControlID="pnlAddDivisionHeader">
    </ajax:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlAddDivision" CssClass="modalPopup" Style="width: 750px !important;">
        <asp:Panel runat="server" ID="pnlAddDivisionHeader" CssClass="mwPopWindowTitle" Style="width: 750px !important;">
            <asp:Label runat="server" ID="lblAddDivisionHeader">&nbsp;&nbsp;Add Division</asp:Label>
        </asp:Panel>
        <br />
        <div style="width: 700px; margin: 0 auto;">
            <asp:Label runat="server" ID="lblAddError" ForeColor="Red"></asp:Label>
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="20%">
                        <asp:Label ID="Label3" CssClass="lblpayeelib" runat="server">Division Name:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtdivisioname" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                       <asp:TableCell Width="20%">
                        <asp:Label ID="Label1" CssClass="lblpayeelib" runat="server">Division Description:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtdivisiondesc" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

        <br />
        <div style="width: 420px; margin: 0 auto;">
             <asp:Button runat="server" ID="btnSave" Text="Save" />
            <ajax:ConfirmButtonExtender runat="server" ID="cbe2" TargetControlID="btnSave"
                ConfirmText="Are you sure to want save?" >
            </ajax:ConfirmButtonExtender>
            &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" />
        </div>



        <%--ADD FUND SOURCES--%>

    <asp:Panel runat="server" ID="pnlFundSourcePanel" Visible="false">
        <div style="float:right;">
        <asp:Button runat="server" ID="btnAddFundSources" Text="Add Fund Sources" Font-Size="XX-Small" Visible="false"/>
        </div>

        <asp:GridView ID="grdFundSources" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="grid" Visible="false">
        <EmptyDataTemplate>
            <b>No records!</b>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkEditFund" Text="Edit" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xEditFund" CssClass="listcontrols"></asp:LinkButton> | 
                    <asp:LinkButton runat="server" ID="lnkDeleteFund" Text="Delete" CommandArgument='<%# Eval("ID")%>'
                        CommandName="xDeleteFund" CssClass="listcontrols"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="FundName" HeaderText="Fund Name" ItemStyle-Width="50%"/>
            <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="30%" DataFormatString="{0:#,##0.00}"  HtmlEncode="false" />
        </Columns>
    </asp:GridView>

    <ajax:ModalPopupExtender runat="server" ID="mpeFundSources" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancelFund" TargetControlID="btnAddFundSources" PopupControlID="pnlAddFundSources" PopupDragHandleControlID="pnlAddFundSourcesHeader" Enabled="false">
    </ajax:ModalPopupExtender>
                    <asp:Panel runat="server" ID="pnlAddFundSources" Width="600" BackColor="#cccccc" Visible="false">
                         <asp:Panel runat="server" ID="pnlAddFundSourcesHeader" CssClass="mwPopWindowTitle" Style="width: 600px !important;" Visible="false">
                            <asp:Label runat="server" ID="Label6">&nbsp;&nbsp;Add Fund Sources</asp:Label>
                        </asp:Panel>
                        <br />
                        <div style="width: 500px; margin: 0 auto;">

                          <asp:Label runat="server" ID="Label2" ForeColor="Red"></asp:Label>
                            <asp:Table ID="Table2" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="30%">
                                        <asp:Label ID="Label4" CssClass="lblpayeelib" runat="server">Fund Source Name:</asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txtFundSourceName" runat="server" Width="90%"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                       <asp:TableCell Width="30%">
                                        <asp:Label ID="Label5" CssClass="lblpayeelib" runat="server">Amount:</asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txtFundAmount" runat="server" Width="90%"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell><asp:Button ID="btnCancelFund" runat="server" Text="Cancel"/></asp:TableCell>
                                    <asp:TableCell><asp:Button ID="btnAddFund" runat="server" Text="Add Fund Source"/> <asp:Button runat="server" ID="btnUpdateFund" Text="Update Fund Details" Visible="false"/></asp:TableCell>

                                </asp:TableRow>
                            </asp:Table>
                      </div>

                    </asp:Panel>
    </asp:Panel>
    </asp:Panel>
    


</asp:Content>
