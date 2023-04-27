<%@ Page Title="Signatories" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Signatories-Library.aspx.vb" Inherits="DbVoucher_Mark1.Signatories_Library" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="featuredContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
      <div id="headerlib"> </div>

    <asp:Panel runat="server" ID="pnlMain">

    <h2 class="page-title"><%: Title %>: <span>List of all Signatories in MinDA.</span></h2>

     <div class="float-right" style="margin:5px 0px;">
    <asp:Button runat="server" ID="btnAddSig" Text="Add New Signatory" />
    </div>
         <asp:GridView ID="grdsignatories" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="grid" PageSize="10"
            AllowPaging="true" AllowSorting="true">
            <EmptyDataTemplate>
                <b>No records!</b>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lnkEdit" Text="Edit" CommandArgument='<%# Eval("ID")%>'
                            CommandName="xEdit" CssClass="listcontrols"></asp:LinkButton> |
                        <asp:LinkButton runat="server" ID="lnkShow" Text="Show" CommandArgument='<%# Eval("ID")%>'
                            CommandName="xShow" CssClass="listcontrols"></asp:LinkButton>
                        <ajax:ConfirmButtonExtender runat="server" ID="cbelnkShow" TargetControlID="lnkShow"
                            ConfirmText="Are you sure to show this record?">
                        </ajax:ConfirmButtonExtender> | 
                        <asp:LinkButton runat="server" ID="lnkDelete" Text="Hide" CommandArgument='<%# Eval("ID")%>'
                            CommandName="xDelete" CssClass="listcontrols"></asp:LinkButton>
                        <ajax:ConfirmButtonExtender runat="server" ID="cbe1" TargetControlID="lnkDelete"
                            ConfirmText="Are you sure to hide this record?">
                        </ajax:ConfirmButtonExtender>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="40%" SortExpression="Name"/>
                <asp:BoundField DataField="Position" HeaderText="Position" ItemStyle-Width="25%" />
                <asp:TemplateField ItemStyle-Width="15%" HeaderText="Status">
                    <ItemTemplate>
                        <%# If(Eval("IsVoid").Equals(False), String.Format("<p style='color: green; font-weight: bold;'>ACTIVE</p>"), String.Format("<p <p style='color: red; font-weight: bold;'>HIDDEN</p>"))%>
                    </ItemTemplate>
            </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>



     <ajax:ModalPopupExtender runat="server" ID="mpeAddSig" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" TargetControlID="btnAddSig" PopupControlID="pnlAddSignatory" PopupDragHandleControlID="pnlAddSigHead">
    </ajax:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlAddSignatory" CssClass="modalPopup" Style="width: 750px !important;">
         <asp:Panel runat="server" ID="pnlAddSigHead" CssClass="mwPopWindowTitle" Style="width: 750px !important;">
            <asp:Label runat="server" ID="lblAddPpa">&nbsp;&nbsp;Add Signatory</asp:Label>
        </asp:Panel>
          <div style="width: 700px; margin: 0 auto;">
            <asp:Label runat="server" ID="lblAddError" ForeColor="Red"></asp:Label>
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="20%">
                        <asp:Label ID="Label3" CssClass="lblpayeelib" runat="server">Name:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSigName" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label4" CssClass="lblpayeelib" runat="server">Position:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSigPosition" runat="server" Width="90%"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

        <asp:button runat="server" ID="btnCancel" text="Cancel"/> <asp:Button runat="server" ID="btnAddSignatoryItem" text="Add Signatory" /> 
        <asp:Button runat="server" ID="btnUpdateSignatoryItem" Text="Update Signatory Record" Visible="false"/>
    </asp:Panel>












</asp:Content>
