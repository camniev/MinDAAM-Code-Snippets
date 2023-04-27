Imports dvdllMaster = dvdll.Master
Imports System.Globalization

Public Class ctrlCreateDV
    Inherits System.Web.UI.UserControl


#Region "Properties"
    Property TaxDynamicControlIDs As List(Of String)
        Get
            Return ViewState("TaxDynamicControlIDs")
        End Get
        Set(value As List(Of String))
            ViewState("TaxDynamicControlIDs") = value
        End Set
    End Property
    Property TaxLatestIDCount As Integer
        Get
            Return ViewState("TaxLatestIDCount")
        End Get
        Set(value As Integer)
            ViewState("TaxLatestIDCount") = value
        End Set
    End Property
    Property DisbursementVoucherRecord As dvdllMaster.DisbursementVoucher.DisbursementVoucherRecord
        Get
            Return ViewState("DisbursementVoucherRecord")
        End Get
        Set(value As dvdllMaster.DisbursementVoucher.DisbursementVoucherRecord)
            ViewState("DisbursementVoucherRecord") = value
        End Set
    End Property

    Property ObligationRequestRecord As dvdllMaster.ObligationRequest.ObligationRequestRecord
        Get
            Return ViewState("ObligationRequestRecord")
        End Get
        Set(value As dvdllMaster.ObligationRequest.ObligationRequestRecord)
            ViewState("ObligationRequestRecord") = value
        End Set
    End Property


    Property SubAcctControlCount As Integer
        Get
            Return ViewState("SubAcctControlCount")
        End Get
        Set(value As Integer)
            ViewState("SubAcctControlCount") = value
        End Set
    End Property
    Property SubAcctControlCountList As List(Of Integer)
        Get
            Return ViewState("SubAcctControlCountList")
        End Get
        Set(value As List(Of Integer))
            ViewState("SubAcctControlCountList") = value
        End Set
    End Property

    Property DisbursementVoucher As dvdllMaster.DisbursementVoucher.DisbursementVoucherRecord
        Get
            Return ViewState("DisbursementVoucherRecord")
        End Get
        Set(value As dvdllMaster.DisbursementVoucher.DisbursementVoucherRecord)
            ViewState("DisbursementVoucherRecord") = value
        End Set
    End Property

    Property IsUpdatingDV As Boolean
        Get
            Return ViewState("IsUpdatingDV")
        End Get
        Set(value As Boolean)
            ViewState("IsUpdatingDV") = value
        End Set
    End Property
    Property AccountEntries As List(Of dvdllMaster.DisbursementVoucher.AccountEntry)
        Get
            Return ViewState("AccountEntries")
        End Get
        Set(value As List(Of dvdllMaster.DisbursementVoucher.AccountEntry))
            ViewState("AccountEntries") = value
        End Set
    End Property

    Property TotalDebit As Decimal
        Get
            If String.IsNullOrEmpty(lbltotaldebit.Text) Or String.IsNullOrWhiteSpace(lbltotaldebit.Text) Then
                Return 0
            End If
            Return CDec(lbltotaldebit.Text)
        End Get
        Set(value As Decimal)
            lbltotaldebit.Text = value.ToString("#,##0.00")
        End Set
    End Property
    Property TotalCredit As Decimal
        Get
            If String.IsNullOrEmpty(lbltotalcredit.Text) Or String.IsNullOrWhiteSpace(lbltotalcredit.Text) Then
                Return 0
            End If
            Return CDec(lbltotalcredit.Text)
        End Get
        Set(value As Decimal)
            lbltotalcredit.Text = value.ToString("#,##0.00")
        End Set
    End Property

    Property AccountListCount As Integer
        Get
            Return ViewState("AccountListCount")
        End Get
        Set(value As Integer)
            ViewState("AccountListCount") = value
        End Set
    End Property

    Property SelectedAccountCodeID As Guid
        Get
            Return ViewState("SelectedAccountCodeID")
        End Get
        Set(value As Guid)
            ViewState("SelectedAccountCodeID") = value
        End Set
    End Property

    Property dUser As String
        Get
            Return ViewState("dUser")
        End Get
        Set(value As String)
            ViewState("dUser") = value
        End Set
    End Property


#End Region

    Event DisbursementSaved()
    Event DisbursementDisregarded()
    Public Event SubAccountTotalTax(TaxSubTotal As Decimal)

#Region "Loading ViewState"
    Protected Overrides Sub LoadViewState(savedState As Object)
        MyBase.LoadViewState(savedState)
        If Not TaxDynamicControlIDs Is Nothing Then
            For Each _TaxLatestIDCount In TaxDynamicControlIDs
                Dim xButton As New Button
                Dim newTax As ctrlTax = Page.LoadControl("~/custom controls/ctrlTax.ascx")
                Dim xLabelValueOutput As New Label
                Dim xLabelTaxFormula As New Label

                Dim newRow As New TableRow
                Dim cell0 As New TableCell
                Dim cell1 As New TableCell
                Dim cell2 As New TableCell
                Dim cell3 As New TableCell

                xButton.ID = "btn_" & _TaxLatestIDCount.ToString
                xButton.Text = "x"
                AddHandler xButton.Click, AddressOf xButton_Click
                newTax.ID = "ucTax_" & _TaxLatestIDCount.ToString
                AddHandler newTax.SelectTax, AddressOf ucTax_SelectedTax

                xLabelValueOutput.ID = "lblTaxOutput_" & _TaxLatestIDCount.ToString
                xLabelTaxFormula.ID = "lblTaxFormula_" & _TaxLatestIDCount.ToString

                cell0.Controls.Add(xButton)
                cell1.Controls.Add(newTax)
                cell3.Controls.Add(xLabelTaxFormula)
                cell2.Controls.Add(xLabelValueOutput)

                newRow.Style.Add("border-right", "2px solid black")
                newRow.Cells.Add(cell0)
                newRow.Cells.Add(cell1)
                newRow.Cells.Add(cell3)
                newRow.Cells.Add(cell2)

                tblComputation.Rows.AddAt(tblComputation.Rows.Count - 1, newRow)
            Next
        End If

        If Not SubAcctControlCountList Is Nothing Then
            For Each SubAcctCtrlID In SubAcctControlCountList
                Dim taxType As ctrlTaxType2 = Page.LoadControl("~/custom controls/ctrlTaxType2.ascx")
                taxType.ID = "ucTaxType2_" & SubAcctCtrlID.ToString
                AddHandler taxType.AddNewSubAcct, AddressOf ucTaxType2_AddNewSubAcct
                AddHandler taxType.RemoveSubAcct, AddressOf ucTaxType2_RemoveSubAcct
                AddHandler taxType.NewComputation, AddressOf ucTaxType2_NewComputation
                pnlTax2.Controls.Add(taxType)
            Next
        End If
    End Sub
#End Region

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            pnlMoPOthers.Visible = False
            pnlVTOthers.Visible = False
            'txtDVDate.Text = DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            txtDVDate.Text = Now.ToShortDateString
            LoadAccountCodes()
        End If
        'tblLiabilities.Visible = False
        dUser = System.Web.Security.Membership.GetUser.ToString
    End Sub

#Region "Control Events"
    Private Sub xButton_Click(sender As Object, e As EventArgs)
        Dim btn = DirectCast(sender, Button)
        Dim idStr() = btn.ID.ToString.Split("_")
        Dim idcnt = CInt(idStr(1))
        TaxDynamicControlIDs.Remove(idcnt.ToString)
        Dim rowIdx As Integer = 0
        Dim exitFor As Boolean = False
        For idx As Integer = 0 To tblComputation.Rows.Count
            rowIdx = idx
            Dim row = tblComputation.Rows(idx)
            Dim ctrlcol = row.Cells(0).Controls
            For Each itm As Control In ctrlcol
                If itm.ID = btn.ID Then
                    exitFor = True
                    Exit For
                End If
            Next
            If exitFor Then Exit For
        Next
        tblComputation.Rows.RemoveAt(rowIdx)
        Dim remRec = DisbursementVoucherRecord.TaxEntries.FirstOrDefault(Function(p) p.IDIndex = idcnt)
        If Not remRec Is Nothing Then
            remRec.DataStatus = dvdllMaster.DisbursementVoucher.TaxEntryDataStatus.Delete
            Dim acctRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = "412")
            If Not acctRec Is Nothing Then
                acctRec.Credit = acctRec.Credit - remRec.TaxOutput
                If acctRec.Credit = 0 Then
                    AccountEntries.Remove(acctRec)
                End If
            End If
        End If
        ComputeParticularsAmountDue()
        LoadAccountEntries()
        If tblComputation.Rows.Count < 3 Then
            AddNewTaxLine()
        End If
        If TaxDynamicControlIDs.Count < 1 Then
            AddNewTaxLine()
        End If
    End Sub

    Private Sub ucTax_SelectedTax(id As Guid, desc As String, percentage As Decimal, shortDesc As String, type As Integer,
                                  hasAddedNewTax As Boolean, ctrlID As String)
        lbllesstax.Visible = True
        pnlTax2.Visible = False
        Dim str() = ctrlID.Split("_")
        Dim idcnt = CInt(str(1))

        Dim rec = DisbursementVoucherRecord.TaxEntries.FirstOrDefault(Function(p) p.IDIndex = idcnt)
        If Not rec Is Nothing Then
            Dim oldTax As Decimal = 0
            If IsUpdatingDV Then
                rec.DataStatus = dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Update
            End If
            'oldTax = rec.TaxOutput
            rec.TaxOutput = ComputeTax(type, percentage, shortDesc)
            rec.TaxID = id
            Dim rowIdx = getRowIdx(idcnt)
            Dim row = tblComputation.Rows.Item(rowIdx)
            Dim label As Label = row.FindControl("lblTaxOutput_" + idcnt.ToString)
            Dim label2 As Label = row.FindControl("lblTaxFormula_" + idcnt.ToString)
            If Not label Is Nothing Then
                'label.Text = rec.TaxOutput.ToString("#,#00.00")
                label.Text = rec.TaxOutput.ToString()
            End If
            If Not label2 Is Nothing Then
                label2.Text = shortDesc
            End If

            If AccountEntries.Exists(Function(p) p.AccountCode = "412") Then
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "412")
                If acctRec Is Nothing Then
                    acctRec.Credit += rec.TaxOutput
                Else
                    acctRec.Credit -= oldTax
                    acctRec.Credit += rec.TaxOutput
                End If
            Else
                Dim newRec = dvdllMaster.Account.getAccountCode412
                With newRec
                    .Credit = rec.TaxOutput
                End With
                AccountEntries.Insert(AccountEntries.Count - 1, newRec)
            End If

            If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "412")
                If Not acctRec Is Nothing Then
                    'acctRec0.Credit = DisbursementVoucherRecord.GrossAmount - acctRec.Credit
                    acctRec0.Credit = CDec(lblGrossAmount.Text) - acctRec.Credit
                Else
                    'acctRec0.Credit = DisbursementVoucherRecord.GrossAmount
                    acctRec0.Credit = CDec(lblGrossAmount.Text)
                End If
            End If
        End If
        ComputeParticularsAmountDue()
        ComputeDebitCredit()
        LoadAccountEntries()
        'RecomputeDebitCredit()

        If hasAddedNewTax Then
            AddNewTaxLine()
        End If
    End Sub
    Private Sub rdBtnModeOfPayment_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdBtnModeOfPayment.SelectedIndexChanged
        Dim rdbtn = DirectCast(sender, RadioButtonList)
        If rdbtn.SelectedValue = dvdllMaster.ModeOfPayment.getOthersID.ToString Then
            pnlMoPOthers.Visible = True
            txtMoPOthers.Text = ""
        Else
            pnlMoPOthers.Visible = False
        End If
        DisbursementVoucher.ModeOfPaymentID = Guid.Parse(rdBtnModeOfPayment.SelectedItem.Value)
    End Sub

    Private Sub chkLiabilities_CheckedChanged(sender As Object, e As EventArgs) Handles chkLiabilities.CheckedChanged
        If chkLiabilities.Checked = True Then
            tblLiabilities.Visible = True
        Else
            tblLiabilities.Visible = False
        End If
    End Sub

    Private Sub chkRemittance_CheckedChanged(sender As Object, e As EventArgs) Handles chkRemittance.CheckedChanged
        If chkRemittance.Checked = True Then
            tblRemittances.Visible = True
        Else
            tblRemittances.Visible = False
        End If
    End Sub

    Private Sub chkPersonal_CheckedChanged(sender As Object, e As EventArgs) Handles chkPersonal.CheckedChanged
        If chkPersonal.Checked = True Then
            tblPersonal.Visible = True
        Else
            tblPersonal.Visible = False
        End If
    End Sub

    Private Sub chkTrustfund_CheckedChanged(sender As Object, e As EventArgs) Handles chkTrustfund.CheckedChanged
        Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = dvdllMaster.Account.SpecialAccountCodes.CashNationalTreasury)
        If Not acctRec Is Nothing Then
            If chkTrustfund.Checked = False Then
                acctRec.AcctsAndExplanation = "Cash-National Treasury, Modified"
                acctRec.AccountCode = 10104040

            Else
                acctRec.AcctsAndExplanation = "Cash-Trust Fund"
            End If
            GlobalAcctRecs.lastAcctRecCode = acctRec.AccountCode
            LoadAccountEntries()
        End If
    End Sub

    Private Sub chkIPURE_CheckedChanged(sender As Object, e As EventArgs) Handles chkIPURE.CheckedChanged
        Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = dvdllMaster.Account.SpecialAccountCodes.CashNationalTreasury)
        'If Not acctRec Is Nothing Then
        If chkIPURE.Checked = False Then
            If acctRec Is Nothing Then
                Dim acctRec1 = AccountEntries.Find(Function(p) p.AccountCode = dvdllMaster.Account.SpecialAccountCodes.CashIPURE)
                acctRec1.AcctsAndExplanation = "Cash-National Treasury, Modified"
                acctRec1.AccountCode = 10104040
                GlobalAcctRecs.lastAcctRecExp = acctRec1.AcctsAndExplanation
                GlobalAcctRecs.lastAcctRecCode = acctRec1.AccountCode
            End If
        Else
            If Not acctRec Is Nothing Then
                acctRec.AcctsAndExplanation = "Cash in Bank - Current Account, IPURE"
                acctRec.AccountCode = 9
                GlobalAcctRecs.lastAcctRecExp = acctRec.AcctsAndExplanation
                GlobalAcctRecs.lastAcctRecCode = acctRec.AccountCode
            End If
        End If
        LoadAccountEntries()
        'End If
    End Sub

    Private Sub chkCashAdvance_CheckedChanged(sender As Object, e As EventArgs) Handles chkCashAdvance.CheckedChanged
        If chkCashAdvance.Checked Then
            DisbursementVoucherRecord.IsAcctCashAdvance = True
            Dim TaxAccount = AccountEntries.Find(Function(p) p.AccountCode = "412")

            Dim cashAdv As New dvdllMaster.DisbursementVoucher.AccountEntry
            With cashAdv
                .AccountCode = "1030502000"
                .AcctsAndExplanation = "Cash Advance"
                '.ID = Guid.NewGuid
                '.Ref = ""
                '.ResponsibilityCenter = cashAdv.ResponsibilityCenter
                '.Debit = 0
                '.Credit = 0
            End With

            For Each acct In AccountEntries.Where(Function(p) p.AccountCode <> "412" Or p.AccountCode <> "10104040").ToList
                cashAdv.Debit += acct.Debit
            Next
            AccountEntries.Insert(0, cashAdv)

            chkOfficeSupplies.Enabled = False
            chkOtherSupplies.Enabled = False
        Else
            DisbursementVoucherRecord.IsAcctCashAdvance = False
            Dim cashAdv = AccountEntries.Find(Function(p) p.AccountCode = "1030502000")
            AccountEntries.Remove(cashAdv)
            chkOfficeSupplies.Enabled = True
            chkOtherSupplies.Enabled = True
        End If
        LoadAccountEntries()
    End Sub

    Private Sub chkAccountsPayable_CheckedChanged(sender As Object, e As EventArgs) Handles chkAccountsPayable.CheckedChanged
        If chkAccountsPayable.Checked = True Then
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.AcctsAndExplanation = "Accounts Payable"
                    existingRec.AccountCode = "20101010"
                    'existingRec.Debit = item.Amount
                End If
            Next
        Else
            RefreshAcctEntries()
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.Debit += item.Amount
                Else
                    Dim newRec As New dvdllMaster.DisbursementVoucher.AccountEntry
                    With newRec
                        .AccountCode = item.AccountCode
                        .AcctsAndExplanation = item.AccountDescAndExpl
                        .ID = Guid.NewGuid
                        .AcctID = dvdllMaster.Account.getAccountbyCode(item.AccountCode).ID
                        .ResponsibilityCenter = item.ResponsibilityCenter
                        .Ref = ""
                        .Debit = item.Amount
                    End With
                    AccountEntries.Insert(0, newRec)
                End If
            Next
        End If
        LoadAccountEntries()
    End Sub

    'Private Sub chkCashAdvance_CheckedChanged(sender As Object, e As EventArgs) Handles chkCashAdvance.CheckedChanged
    '    If chkCashAdvance.Checked Then
    '        DisbursementVoucherRecord.IsAcctCashAdvance = True

    '        Dim cashAdv As New dvdllMaster.DisbursementVoucher.AccountEntry
    '        With cashAdv
    '            .AccountCode = "1030502000"
    '            .AcctsAndExplanation = "Cash Advance"
    '            .ID = Guid.NewGuid
    '            .Ref = ""
    '            .ResponsibilityCenter = cashAdv.ResponsibilityCenter
    '            .Debit = 0
    '            .Credit = 0
    '        End With

    '        For Each acct In AccountEntries.Where(Function(p) p.AccountCode <> "412" Or p.AccountCode <> "10104040").ToList
    '            cashAdv.Debit += acct.Debit
    '        Next
    '        AccountEntries.Insert(0, cashAdv)

    '        If chkDuetoBIR.Checked = True Then
    '            Dim duetobir As New dvdllMaster.DisbursementVoucher.AccountEntry
    '            With duetobir
    '                .ID = Guid.NewGuid
    '                .AccountCode = "412"
    '                .AcctsAndExplanation = "Due to BIR"
    '                .Credit = txtBIR.Text
    '            End With
    '            AccountEntries.Insert(1, duetobir)
    '            ComputePayroll()
    '        Else
    '            Dim bir = AccountEntries.Find(Function(p) p.AccountCode = "412")
    '            AccountEntries.Remove(bir)
    '        End If

    '        chkOfficeSupplies.Enabled = False
    '        chkOtherSupplies.Enabled = False
    '    Else
    '        DisbursementVoucherRecord.IsAcctCashAdvance = False
    '        Dim cashAdv = AccountEntries.Find(Function(p) p.AccountCode = "1030502000")
    '        AccountEntries.Remove(cashAdv)
    '        chkOfficeSupplies.Enabled = True
    '        chkOtherSupplies.Enabled = True
    '    End If
    '    LoadAccountEntries()
    'End Sub

    Private Sub chkOfficeSupplies_CheckedChanged(sender As Object, e As EventArgs) Handles chkOfficeSupplies.CheckedChanged
        If chkOfficeSupplies.Checked = True Then
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.AcctsAndExplanation = "Office Supplies"
                    existingRec.AccountCode = "165"
                    'existingRec.Debit = item.Amount
                End If
            Next
            chkCashAdvance.Enabled = False
            chkOtherSupplies.Enabled = False
        Else
            RefreshAcctEntries()
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.Debit += item.Amount
                Else
                    Dim newRec As New dvdllMaster.DisbursementVoucher.AccountEntry
                    With newRec
                        .AccountCode = item.AccountCode
                        .AcctsAndExplanation = item.AccountDescAndExpl
                        .ID = Guid.NewGuid
                        .AcctID = dvdllMaster.Account.getAccountbyCode(item.AccountCode).ID
                        .ResponsibilityCenter = item.ResponsibilityCenter
                        .Ref = ""
                        .Debit = item.Amount
                        .AccountCode = item.AccountCode
                    End With
                    'AccountEntries.Add(newRec)
                    AccountEntries.Insert(0, newRec)
                End If
            Next
            chkCashAdvance.Enabled = True
            chkOtherSupplies.Enabled = True
        End If
        LoadAccountEntries()

    End Sub
    Private Sub chkOtherSupplies_CheckedChanged(sender As Object, e As EventArgs) Handles chkOtherSupplies.CheckedChanged
        If chkOtherSupplies.Checked = True Then
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.AcctsAndExplanation = "Other Supplies"
                    existingRec.AccountCode = "155"
                    'existingRec.Debit = item.Amount
                End If
            Next
            chkCashAdvance.Enabled = False
            chkOfficeSupplies.Enabled = False
        Else
            RefreshAcctEntries()
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.Debit += item.Amount
                Else
                    Dim newRec As New dvdllMaster.DisbursementVoucher.AccountEntry
                    With newRec
                        .AccountCode = item.AccountCode
                        .AcctsAndExplanation = item.AccountDescAndExpl
                        .ID = Guid.NewGuid
                        .AcctID = dvdllMaster.Account.getAccountbyCode(item.AccountCode).ID
                        .ResponsibilityCenter = item.ResponsibilityCenter
                        .Ref = ""
                        .Debit = item.Amount
                        .AccountCode = item.AccountCode
                    End With
                    'AccountEntries.Add(newRec)
                    AccountEntries.Insert(0, newRec)
                End If
            Next
            chkCashAdvance.Enabled = True
            chkOfficeSupplies.Enabled = True
        End If
        LoadAccountEntries()

    End Sub

    Private Sub chkApplyAccount_CheckedChanged(sender As Object, e As EventArgs) Handles chkApplyAccount.CheckedChanged
        SelectedAccountCodeID = Guid.Parse(ddlAccountCode.SelectedItem.Value)
        If chkApplyAccount.Checked = True Then
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.AcctsAndExplanation = ddlAccountCode.SelectedItem.ToString
                    existingRec.AccountCode = dvdll.Master.Account.getAccountByID(SelectedAccountCodeID).AccountCode
                    'existingRec.Debit = item.Amount

                    GlobalAcctRecs.firstAcctRecExp = existingRec.AcctsAndExplanation
                    GlobalAcctRecs.firstAcctRecCode = existingRec.AccountCode
                End If
            Next
            chkAccountsPayable.Enabled = False
            chkCashAdvance.Enabled = False
            ddlAccountCode.Enabled = False
            chkOfficeSupplies.Enabled = False
            chkOtherSupplies.Enabled = False
        Else
            RefreshAcctEntries()
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.Debit += item.Amount
                Else
                    Dim newRec As New dvdllMaster.DisbursementVoucher.AccountEntry
                    With newRec
                        .AccountCode = item.AccountCode
                        .AcctsAndExplanation = item.AccountDescAndExpl
                        .ID = Guid.NewGuid
                        .AcctID = dvdllMaster.Account.getAccountbyCode(item.AccountCode).ID
                        .ResponsibilityCenter = item.ResponsibilityCenter
                        .Ref = ""
                        .Debit = item.Amount
                    End With
                    AccountEntries.Insert(0, newRec)
                End If
            Next
            chkAccountsPayable.Enabled = True
            chkCashAdvance.Enabled = True
            ddlAccountCode.Enabled = True
            chkOfficeSupplies.Enabled = True
            chkOtherSupplies.Enabled = True
        End If
        LoadAccountEntries()


    End Sub

    Private Sub chkExpenseToInventory_CheckedChanged(sender As Object, e As EventArgs) Handles chkExpenseToInventory.CheckedChanged
        Dim acctRecOfficeExpense = AccountEntries.Find(Function(p) p.AccountCode = "5020301000")
        Dim acctRecOthersExpense = AccountEntries.Find(Function(p) p.AccountCode = "5020399000")

        If chkExpenseToInventory.Checked = True Then
            If Not acctRecOfficeExpense Is Nothing Then
                acctRecOfficeExpense.AcctsAndExplanation = "Office Supplies Inventory"
                acctRecOfficeExpense.AccountCode = "1040401000"
            End If
        Else
            RefreshAcctEntries()
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.Debit += item.Amount
                Else
                    Dim newRec As New dvdllMaster.DisbursementVoucher.AccountEntry
                    With newRec
                        .AccountCode = item.AccountCode
                        .AcctsAndExplanation = item.AccountDescAndExpl
                        .ID = Guid.NewGuid
                        .AcctID = dvdllMaster.Account.getAccountbyCode(item.AccountCode).ID
                        .ResponsibilityCenter = item.ResponsibilityCenter
                        .Ref = ""
                        .Debit = item.Amount
                    End With
                    AccountEntries.Insert(0, newRec)
                End If
            Next
            chkAccountsPayable.Enabled = True
            chkCashAdvance.Enabled = True
            ddlAccountCode.Enabled = True
            chkOfficeSupplies.Enabled = True
            chkOtherSupplies.Enabled = True

        End If

        If chkExpenseToInventory.Checked = True Then
            If Not acctRecOthersExpense Is Nothing Then
                acctRecOthersExpense.AcctsAndExplanation = "Other Supplies Inventory"
                acctRecOthersExpense.AccountCode = "1040499000"
            End If
        Else
            RefreshAcctEntries()
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.Debit += item.Amount
                Else
                    Dim newRec As New dvdllMaster.DisbursementVoucher.AccountEntry
                    With newRec
                        .AccountCode = item.AccountCode
                        .AcctsAndExplanation = item.AccountDescAndExpl
                        .ID = Guid.NewGuid
                        .AcctID = dvdllMaster.Account.getAccountbyCode(item.AccountCode).ID
                        .ResponsibilityCenter = item.ResponsibilityCenter
                        .Ref = ""
                        .Debit = item.Amount
                    End With
                    AccountEntries.Insert(0, newRec)
                End If
            Next
            chkAccountsPayable.Enabled = True
            chkCashAdvance.Enabled = True
            ddlAccountCode.Enabled = True
            chkOfficeSupplies.Enabled = True
            chkOtherSupplies.Enabled = True
        End If
        LoadAccountEntries()
    End Sub

    'PERSONAL
    Private Sub chkNewAmount_CheckedChanged(sender As Object, e As EventArgs) Handles chkNewAmount.CheckedChanged
        If chkNewAmount.Checked = True Then
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
                Dim DebitAmounts = AccountEntries.FirstOrDefault(Function(p) p.Debit)
                If Not existingRec Is Nothing Then
                    existingRec.Credit = txtNewAmount.Text
                End If
                DebitAmounts.Debit = CDec(txtNewAmount.Text)
                lblGrossAmount.Text = CDec(txtNewAmount.Text)
                lblTotalAmountDue.Text = CDec(txtNewAmount.Text)
                lbltotalcredit.Text = CDec(txtNewAmount.Text)
                lbltotaldebit.Text = CDec(txtNewAmount.Text)
            Next
            chkAccountsPayable.Enabled = False
            chkCashAdvance.Enabled = False
            chkOtherSupplies.Enabled = False
            chkOfficeSupplies.Enabled = False

        Else

            RefreshAcctEntries()
            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                Dim existingRecAmount = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
                If Not existingRec Is Nothing Then
                    existingRec.Debit += item.Amount
                Else
                    Dim newRec As New dvdllMaster.DisbursementVoucher.AccountEntry
                    With newRec
                        .AccountCode = item.AccountCode
                        .AcctsAndExplanation = item.AccountDescAndExpl
                        .ID = Guid.NewGuid
                        .AcctID = dvdllMaster.Account.getAccountbyCode(item.AccountCode).ID
                        .ResponsibilityCenter = item.ResponsibilityCenter
                        .Ref = ""
                        .Debit = item.Amount
                        .AccountCode = item.AccountCode
                    End With
                    'AccountEntries.Add(newRec)
                    AccountEntries.Insert(0, newRec)
                End If

                If Not existingRecAmount Is Nothing Then
                    existingRecAmount.Credit = DisbursementVoucher.GrossAmount
                End If
                lblGrossAmount.Text = DisbursementVoucher.GrossAmount
                lblTotalAmountDue.Text = DisbursementVoucher.GrossAmount
                lbltotalcredit.Text = DisbursementVoucher.GrossAmount
                lbltotaldebit.Text = DisbursementVoucher.GrossAmount

            Next
            ComputeDebitCredit()

            chkAccountsPayable.Enabled = True
            chkCashAdvance.Enabled = True
            chkOtherSupplies.Enabled = True
            chkOfficeSupplies.Enabled = True

        End If
        LoadAccountEntries()
        'RecomputeDebitCredit()
    End Sub

    'REMITTANCE
    Private Sub chkDuetoPagibigRemittance_CheckedChanged(sender As Object, e As EventArgs) Handles chkDuetoPagibigRemittance.CheckedChanged
        If chkDuetoPagibigRemittance.Checked = True Then
            If txtRemitPagibig.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Due to PAG-IBIG Remittance Amount")
                ucInfoWindow.ShowInfo()
                chkDuetoPagibigRemittance.Checked = False
            Else
                Dim duetopagibigremit As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetopagibigremit
                    .ID = Guid.NewGuid
                    .AccountCode = "20201030"
                    .AcctsAndExplanation = "Due to PAG-IBIG"
                    .Debit = txtRemitPagibig.Text
                End With
                AccountEntries.Insert(AccountListCount, duetopagibigremit)
                ComputeRemit()
            End If
        Else
            Dim pagibigremit = AccountEntries.Find(Function(p) p.AccountCode = "20201030")

            'SUBTRACT REMOVED REMITTANCE (DEBIT) AMOUNT FROM TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "20201030")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit -= acctRec.Debit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "20201030")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit -= acctRec.Debit
                End If
            End If

            AccountEntries.Remove(pagibigremit)
        End If
        ComputeDebitCredit()
        LoadAccountEntries()
        'RecomputeDebitCredit()
    End Sub

    Private Sub chkDuetoIntelicareRemittance_CheckedChanged(sender As Object, e As EventArgs) Handles chkDuetoIntelicareRemittance.CheckedChanged
        If chkDuetoIntelicareRemittance.Checked = True Then
            If txtRemitIntelicare.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Due to Intellicare Remittance Amount")
                ucInfoWindow.ShowInfo()
                chkDuetoIntelicareRemittance.Checked = False
            Else
                Dim duetointelicareremit As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetointelicareremit
                    .ID = Guid.NewGuid
                    .AccountCode = "29999990"
                    .AcctsAndExplanation = "Other Payables"
                    .Debit = txtRemitIntelicare.Text
                End With
                AccountEntries.Insert(AccountListCount, duetointelicareremit)
                ComputeRemit()
            End If
        Else
            'INITIALIZE ACCOUNT TO REMOVE
            Dim intelicareremit = AccountEntries.Find(Function(p) p.AccountCode = "29999990")

            'SUBTRACT REMOVED REMITTANCE (DEBIT) AMOUNT FROM TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "29999990")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit -= acctRec.Debit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "29999990")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit -= acctRec.Debit
                End If
            End If

            'REMOVE INTELICARE REMITTANCE ROW AFTER SUBTRACTING
            AccountEntries.Remove(intelicareremit)
        End If
        ComputeDebitCredit()
        LoadAccountEntries()
    End Sub

    Private Sub chkDuetoGSISRemittance_CheckedChanged(sender As Object, e As EventArgs) Handles chkDuetoGSISRemittance.CheckedChanged
        If chkDuetoGSISRemittance.Checked = True Then
            If txtRemitGSIS.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Due to GSIS Remittance Amount")
                ucInfoWindow.ShowInfo()
                chkDuetoGSISRemittance.Checked = False
            Else
                Dim duetogsisremit As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetogsisremit
                    .ID = Guid.NewGuid
                    .AccountCode = "20201020"
                    .AcctsAndExplanation = "Due to GSIS"
                    .Debit = txtRemitGSIS.Text
                End With
                AccountEntries.Insert(AccountListCount, duetogsisremit)
                ComputeRemit()
            End If
        Else
            Dim gsisremit = AccountEntries.Find(Function(p) p.AccountCode = "20201020")

            'SUBTRACT REMOVED REMITTANCE (DEBIT) AMOUNT FROM TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "20201020")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit -= acctRec.Debit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "20201020")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit -= acctRec.Debit
                End If
            End If

            AccountEntries.Remove(gsisremit)
        End If
        ComputeDebitCredit()
        LoadAccountEntries()
        'RecomputeDebitCredit()
    End Sub
    Private Sub chkDuetoBIRRemittance_CheckedChanged(sender As Object, e As EventArgs) Handles chkDuetoBIRRemittance.CheckedChanged
        If chkDuetoBIRRemittance.Checked = True Then
            If txtRemitBIR.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Due to BIR Remittance Amount")
                ucInfoWindow.ShowInfo()
                chkDuetoBIRRemittance.Checked = False
            Else
                Dim duetobirremit As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetobirremit
                    .ID = Guid.NewGuid
                    .AccountCode = "20201010"
                    .AcctsAndExplanation = "Due to BIR"
                    .Debit = txtRemitBIR.Text
                End With
                AccountEntries.Insert(AccountListCount, duetobirremit)
                ComputeRemit()
            End If
        Else
            Dim birremit = AccountEntries.Find(Function(p) p.AccountCode = "20201010")

            'SUBTRACT REMOVED REMITTANCE (DEBIT) AMOUNT FROM TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "20201010")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit -= acctRec.Debit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "20201010")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit -= acctRec.Debit
                End If
            End If

            AccountEntries.Remove(birremit)
        End If
        ComputeDebitCredit()
        LoadAccountEntries()
        'RecomputeDebitCredit()
    End Sub
    Private Sub chkDuetoPhilHealthRemittance_CheckedChanged(sender As Object, e As EventArgs) Handles chkDuetoPhilHealthRemittance.CheckedChanged
        If chkDuetoPhilHealthRemittance.Checked = True Then
            If txtRemitPhilHealth.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Due to BIR Remittance Amount")
                ucInfoWindow.ShowInfo()
                chkDuetoPhilHealthRemittance.Checked = False
            Else
                Dim duetophilhealthremit As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetophilhealthremit
                    .ID = Guid.NewGuid
                    .AccountCode = "20201040"
                    .AcctsAndExplanation = "Due to PHIL. HEALTH"
                    .Debit = txtRemitPhilHealth.Text
                End With
                AccountEntries.Insert(AccountListCount, duetophilhealthremit)
                ComputeRemit()
            End If
        Else
            Dim philhealthremit = AccountEntries.Find(Function(p) p.AccountCode = "20201040")

            'SUBTRACT REMOVED REMITTANCE (DEBIT) AMOUNT FROM TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "20201040")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit -= acctRec.Debit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "20201040")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit -= acctRec.Debit
                End If
            End If

            AccountEntries.Remove(philhealthremit)
        End If
        ComputeDebitCredit()
        LoadAccountEntries()
    End Sub

    'LIABILITIES
    Private Sub chkDueToGsis_CheckedChanged(sender As Object, e As EventArgs) Handles chkDueToGsis.CheckedChanged
        If chkDueToGsis.Checked = True Then
            If txtGsis.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Due to GSIS Amount")
                ucInfoWindow.ShowInfo()
                chkDueToGsis.Checked = False
            Else
                Dim duetogsis As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetogsis
                    .ID = Guid.NewGuid
                    .AccountCode = "413"
                    .AcctsAndExplanation = "Due to GSIS"
                    .Credit = txtGsis.Text
                End With
                AccountEntries.Insert(AccountListCount, duetogsis)
                ComputePayroll()
            End If
        Else
            Dim gsis = AccountEntries.Find(Function(p) p.AccountCode = "413")

            'ADD AMOUNT OF REMOVED CREDIT TO TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "413")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit += acctRec.Credit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "413")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit += acctRec.Credit
                End If
            End If

            AccountEntries.Remove(gsis)
        End If
        ComputeDebitCredit()
        LoadAccountEntries()
    End Sub
    Private Sub chkDueToPagibig_CheckedChanged(sender As Object, e As EventArgs) Handles chkDueToPagibig.CheckedChanged
        If chkDueToPagibig.Checked = True Then
            If txtPagibig.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Due to PAG-IBIG Amount")
                ucInfoWindow.ShowInfo()
                chkDueToPagibig.Checked = False
            Else
                Dim duetopagibig As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetopagibig
                    .ID = Guid.NewGuid
                    .AccountCode = "414"
                    .AcctsAndExplanation = "Due to PAG-IBIG"
                    .Credit = txtPagibig.Text
                End With
                AccountEntries.Insert(AccountListCount, duetopagibig)
                ComputePayroll()
            End If
        Else
            Dim pagibig = AccountEntries.Find(Function(p) p.AccountCode = "414")

            'ADD AMOUNT OF REMOVED CREDIT TO TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "2020103000")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit += acctRec.Credit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "414")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit += acctRec.Credit
                End If
            End If

            AccountEntries.Remove(pagibig)
        End If
        ComputeDebitCredit()
        LoadAccountEntries()
    End Sub
    Private Sub chkDueToPhilHealth_CheckedChanged(sender As Object, e As EventArgs) Handles chkDueToPhilHealth.CheckedChanged
        If chkDueToPhilHealth.Checked = True Then
            If txtPhilHealth.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Due to Phil. Health Amount")
                ucInfoWindow.ShowInfo()
                chkDueToPhilHealth.Checked = False
            Else
                Dim duetophilhealth As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetophilhealth
                    .ID = Guid.NewGuid
                    .AccountCode = "415"
                    .AcctsAndExplanation = "Due to PHILHEALTH"
                    .Credit = txtPhilHealth.Text
                End With
                AccountEntries.Insert(AccountListCount, duetophilhealth)
                ComputePayroll()
            End If
        Else
            Dim philhealth = AccountEntries.Find(Function(p) p.AccountCode = "415")

            'ADD AMOUNT OF REMOVED CREDIT TO TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "415")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit += acctRec.Credit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "415")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit += acctRec.Credit
                End If
            End If

            AccountEntries.Remove(philhealth)
        End If
        ComputeDebitCredit()
        LoadAccountEntries()
    End Sub
    Private Sub chkDuetoBIR_CheckedChanged(sender As Object, e As EventArgs) Handles chkDuetoBIR.CheckedChanged
        If chkDuetoBIR.Checked = True Then
            If txtBIR.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Due to BIR Amount")
                ucInfoWindow.ShowInfo()
                chkDuetoBIR.Checked = False
            Else
                Dim duetobir As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetobir
                    .ID = Guid.NewGuid
                    .AccountCode = "412"
                    .AcctsAndExplanation = "Due to BIR"
                    .Credit = txtBIR.Text
                End With
                AccountEntries.Insert(AccountListCount, duetobir)
                ComputePayroll()
            End If
        Else
            Dim bir = AccountEntries.Find(Function(p) p.AccountCode = "412")

            'ADD AMOUNT OF REMOVED CREDIT TO TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "412")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit += acctRec.Credit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "412")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit += acctRec.Credit
                End If
            End If

            AccountEntries.Remove(bir)
        End If

        ComputeDebitCredit()
        LoadAccountEntries()
    End Sub
    Private Sub chkDueToIntelicare_CheckedChanged(sender As Object, e As EventArgs) Handles chkDueToIntelicare.CheckedChanged
        If chkDueToIntelicare.Checked = True Then
            If txtIntelicare.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Due to Intellicare Amount")
                ucInfoWindow.ShowInfo()
                chkDueToIntelicare.Checked = False
            Else
                Dim duetointelicare As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetointelicare
                    .ID = Guid.NewGuid
                    .AccountCode = "29999990"
                    .AcctsAndExplanation = "Other Payables"
                    .Credit = txtIntelicare.Text
                End With
                AccountEntries.Insert(AccountListCount, duetointelicare)
                ComputePayroll()
            End If
        Else
            Dim intelicare = AccountEntries.Find(Function(p) p.AccountCode = "29999990")

            'ADD AMOUNT OF REMOVED CREDIT TO TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "29999990")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit += acctRec.Credit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "29999990")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit += acctRec.Credit
                End If
            End If

            AccountEntries.Remove(intelicare)
        End If
        ComputeDebitCredit()
        LoadAccountEntries()
    End Sub
    Private Sub chkDuetoCashAdvance_CheckedChanged(sender As Object, e As EventArgs) Handles chkDuetoCashAdvance.CheckedChanged
        If chkDuetoCashAdvance.Checked = True Then
            If txtCashAdvances.Text = "" Then
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Enter Cash Advance Amount")
                ucInfoWindow.ShowInfo()
                chkDuetoCashAdvance.Checked = False
            Else
                Dim duetoCashAdvance As New dvdllMaster.DisbursementVoucher.AccountEntry
                With duetoCashAdvance
                    .ID = Guid.NewGuid
                    .AccountCode = "1030502000"
                    .AcctsAndExplanation = "Cash Advances"
                    .Credit = txtCashAdvances.Text
                End With
                AccountEntries.Insert(AccountListCount, duetoCashAdvance)
                ComputePayroll()
            End If
        Else
            Dim cashadvance = AccountEntries.Find(Function(p) p.AccountCode = "1030502000")

            'ADD AMOUNT OF REMOVED CREDIT TO TOTAL CREDIT AMOUNT
            'ACCOUNT CODE IN STATIC (Cash-National Treasury; Modified); CHANGED TO DYNAMIC (GlobalAcctRecs.lastAcctRecCode)
            'If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            '    Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            '    Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "1030502000")
            '    If Not acctRec Is Nothing Then
            '        acctRec0.Credit += acctRec.Credit
            '    End If
            'End If

            If AccountEntries.Exists(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode) Then
                Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
                Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "1030502000")
                If Not acctRec Is Nothing Then
                    acctRec0.Credit += acctRec.Credit
                End If
            End If

            AccountEntries.Remove(cashadvance)
        End If

        ComputeDebitCredit()
        LoadAccountEntries()
    End Sub

    Private Sub chTaxType2_CheckedChanged(sender As Object, e As EventArgs) Handles chTaxType2.CheckedChanged
        ucConfirmExtender.SetMessage("Existing Tax Entries will be disregarded, continue?")
        ucConfirmExtender.SetHeader("WARNING")
        ucConfirmExtender.SetOkButtonText("Continue")
        ucConfirmExtender.SetControlID(chTaxType2.ID)
        ucConfirmExtender.ShowWindow()
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If IsValidSave() Then

            If rdBtnModeOfPayment.SelectedItem.Value = dvdllMaster.ModeOfPayment.getOthersID.ToString Then
                DisbursementVoucher.ModeOfPaymentOthers = txtMoPOthers.Text
            End If
            'Form Validation for Voucher Type HIDDEN (Selecting Voucher Type removed per FD's request) 04282022
            'If rdBtnVoucherType.SelectedItem.Value = dvdllMaster.VoucherType.getOthersID.ToString Then
            '    DisbursementVoucher.VoucherTypeOthers = txtVTOthers.Text
            'End If

            DisbursementVoucher.DVParticularTemplate = txtParticularTemplate.Text

            If chTaxType2.Checked Then
                DisbursementVoucher.ParticularsAmountDue = AccountEntries.Find(Function(p) p.AccountCode = "10104040").Credit
            Else
                DisbursementVoucher.ParticularsAmountDue = lblTotalAmountDue.Text
            End If

            Dim txtReviewDetail As String
            'txtReviewDetail = "Kindly review the ff. details before saving: "
            'txtReviewDetail += vbNewLine + "Date: " + txtDVDate.Text
            'txtReviewDetail += vbNewLine + "SAVE NOW? (Click 'Continue' to save)"

            txtReviewDetail = "<table>"
            txtReviewDetail += "<tr><td>Kindly review the ff. details before saving: </td></tr>"
            txtReviewDetail += "<tr><td>&nbsp;</td></tr>"
            txtReviewDetail += "<tr><td><p>Payee: <span style='font-weight: bold;'>" + txtPayeeName.Text + "</span></p></td></tr>"
            txtReviewDetail += "<tr><td><p>Date: <span style='font-weight: bold;'>" + txtDVDate.Text + "</span></p></td></tr>"
            txtReviewDetail += "<tr><td>&nbsp;</td></tr>"
            txtReviewDetail += "<tr><td><p style='font-weight: bold;'>SAVE NOW? (Click 'Continue' to save)</p></td></tr>"
            txtReviewDetail += "</table>"

            'MsgBox(txtReviewDetail)

            ucConfirmExtenderDetail.SetMessage(txtReviewDetail)
            'ucConfirmExtenderDetail.SetMessage("SAVE NOW?")
            ucConfirmExtenderDetail.SetHeader("REVIEW DETAILS")
            ucConfirmExtenderDetail.SetOkButtonText("Continue")
            ucConfirmExtenderDetail.SetControlID(chTaxType2.ID)
            ucConfirmExtenderDetail.ShowWindow()

        End If
    End Sub
#End Region

#Region "Methods"
    Public Sub CreateDisbursementVoucherInstance(obrID As Guid)
        DisbursementVoucherRecord = New dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord
        IsUpdatingDV = False
        TaxDynamicControlIDs = New List(Of String)
        TaxLatestIDCount = 0
        AccountEntries = New List(Of dvdll.Master.DisbursementVoucher.AccountEntry)

        AccountListCount = 0

        Dim obrRec = dvdllMaster.ObligationRequest.getObligationRequestRecordByID(obrID)
        obrRec.ORParticularEntries.AddRange(dvdllMaster.ObligationRequest.getORParticularEntriesByORID(obrID))
        If Not obrRec Is Nothing Then
            With DisbursementVoucherRecord
                .ID = Guid.NewGuid
                .DateCreated = Now
                .Status = dvdllMaster.DisbursementVoucher.DVStatus.ForApproval
                .PayeeID = obrRec.PayeeID
                .ParticularsAmountDue = 0
                .ORNo = obrRec.ObligationRequestNo
                .ObligationRequestID = obrRec.ID
                .GrossAmount = obrRec.TotalAmount
                .DisbursementVoucherDate = Now
                .IsAcctCashAdvance = False
                .DVisPriority = obrRec.isPrio
            End With

            lblPayeeName.Text = obrRec.PayeeName
            txtPayeeName.Text = obrRec.PayeeName
            lblPayeeAddress.Text = DisbursementVoucherRecord.PayeeAddress
            txtPayeeAddress.Text = DisbursementVoucherRecord.PayeeAddress
            lblPayeeOffice.Text = DisbursementVoucherRecord.PayeeOffice
            txtPayeeOffice.Text = DisbursementVoucherRecord.PayeeOffice
            'lblPayeeTIN.Text = DisbursementVoucherRecord.PayeeTIN
            txtPayeeTin.Text = DisbursementVoucherRecord.PayeeTIN
            txtObrNo.Text = DisbursementVoucherRecord.ORNo
            txtObrDate.Text = obrRec.DateCreated
            lblDVDateCreated.Text = Now.ToShortDateString
            lblParticularTemplate.Text = obrRec.Template
            'txtParticularTemplate.Text = obrRec.Template

            Dim TemplateCont = dvdllMaster.ObligationRequest.getORParticularEntriesByORID(obrID)
            For Each item In TemplateCont
                txtParticularTemplate.Text = item.Description
            Next

            txtvoucherdate.Text = Now.Date
            lbltotalcredit.Text = DisbursementVoucherRecord.GrossAmount
            lbltotaldebit.Text = DisbursementVoucherRecord.GrossAmount
            ComputeParticularsAmountDue()

            For Each obrPart In obrRec.ORParticularEntries
                Dim newRec As New dvdllMaster.DisbursementVoucher.ParticularEntry
                With newRec
                    .ID = Guid.NewGuid
                    .DisbursementVoucherID = DisbursementVoucherRecord.ID
                    .Amount = obrPart.Amount
                    .PPAID = obrPart.PPAID
                    .ResponsibilityCenterID = obrPart.ResposibilityCenterID
                    .AcctID = obrPart.AcctID
                End With
                DisbursementVoucherRecord.ParticularEntries.Add(newRec)
            Next

            For Each item In DisbursementVoucherRecord.ParticularEntries
                Dim existingRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = item.AccountCode)
                If Not existingRec Is Nothing Then
                    existingRec.Debit += item.Amount
                Else
                    Dim newRec As New dvdllMaster.DisbursementVoucher.AccountEntry
                    With newRec
                        .AccountCode = item.AccountCode
                        .AcctsAndExplanation = item.AccountDescAndExpl
                        .ID = Guid.NewGuid
                        .AcctID = dvdllMaster.Account.getAccountbyCode(item.AccountCode).ID
                        .ResponsibilityCenter = item.ResponsibilityCenter
                        .Ref = ""
                        .Debit = item.Amount
                        .AccountCode = item.AccountCode
                        AccountListCount = AccountListCount + 1
                    End With
                    'AccountEntries.Add(newRec)
                    AccountEntries.Insert(0, newRec)
                End If
            Next
            '****** ADD ONS *********
            Dim cashrec = dvdllMaster.Account.getAccountCode108
            cashrec.Credit = DisbursementVoucher.GrossAmount
            AccountEntries.Add(cashrec)
            '*********END ADD ONS*********
            LoadAccountEntries()
            LoadRadioButtons(dvdllMaster.ModeOfPayment.getModeOfPayments, dvdllMaster.VoucherType.getVoucherTypes)
            AddNewTaxLine()
        End If

        ComputeDebitCredit()
        GlobalAcctRecs.lastAcctRecCode = "10104040"
        'RecomputeDebitCredit()
    End Sub
    Sub LoadAccountEntries()
        Dim acctEntriesSource As New List(Of dvdllMaster.DisbursementVoucher.AccountEntry)
        If chkCashAdvance.Checked Then
            acctEntriesSource.AddRange(AccountEntries.Where(Function(p) p.AccountCode = "1030502000" Or p.AccountCode = "10104040" Or p.AccountCode = "412").ToList)
        ElseIf chkLiabilities.Checked Then
            acctEntriesSource.AddRange(AccountEntries)
        Else
            acctEntriesSource.AddRange(AccountEntries)
        End If

        grdAcctEntries.DataSource = acctEntriesSource
        grdAcctEntries.DataBind()


    End Sub
    Function IsValidSave()
        If rdBtnModeOfPayment.SelectedIndex < 0 Then
            ucInfoWindow.SetHeader("INFO")
            ucInfoWindow.SetMessage("Please select Mode of Payment to continue")
            ucInfoWindow.ShowInfo()
            Return False
        End If
        ' Form Validation for Voucher Type HIDDEN (Selecting Voucher Type removed per FD's request) 04282022
        'If rdBtnVoucherType.SelectedIndex < 0 Then
        '    ucInfoWindow.SetHeader("INFO")
        '    ucInfoWindow.SetMessage("Please select Voucher Type to continue")
        '    ucInfoWindow.ShowInfo()
        '    Return False
        'End If

        If lbltotalcredit.Text <> lbltotaldebit.Text Then
            ucInfoWindow.SetHeader("INFO")
            ucInfoWindow.SetMessage("Accounts not Balanced")
            ucInfoWindow.ShowInfo()
            Return False
        End If
        Return True
    End Function
    Sub LoadRadioButtons(modeList As List(Of dvdll.lib_ModeOfPayment), voucherList As List(Of dvdll.lib_VoucherType))
        Dim tempList As New List(Of ListItem)
        Dim temp As New ListItem
        For Each itm In modeList
            'If itm.ModeOfPayment = "Others" Then
            '    temp.Text = itm.ModeOfPayment
            '    temp.Value = itm.ID.ToString()
            'Else
            tempList.Add(New ListItem With {.Text = itm.ModeOfPayment, .Value = itm.ID.ToString()})
            'End If
            'MsgBox(itm.ModeOfPayment + " " + itm.ID.ToString())
        Next
        'tempList.Add(temp)
        rdBtnModeOfPayment.DataSource = tempList
        rdBtnModeOfPayment.DataBind()

        tempList = New List(Of ListItem)
        temp = New ListItem

        For Each itm In voucherList
            If itm.VouchertType = "Others" Then
                temp.Text = itm.VouchertType
                temp.Value = itm.ID.ToString
            Else
                tempList.Add(New ListItem With {.Text = itm.VouchertType, .Value = itm.ID.ToString})
            End If
            'MsgBox(itm.VouchertType + " " + itm.ID.ToString())
        Next
        tempList.Add(temp)
        rdBtnVoucherType.DataSource = tempList
        rdBtnVoucherType.DataBind()
    End Sub
    '================================TAX METHODS=======================================
    Sub AddNewTaxLine()
        Dim newEntry As New dvdllMaster.DisbursementVoucher.TaxEntry
        newEntry.ID = Guid.NewGuid
        newEntry.DataStatus = dvdllMaster.DisbursementVoucher.TaxEntryDataStatus.Add
        newEntry.IsSubAcct = False
        Dim xButton As New Button
        Dim newTax As ctrlTax = Page.LoadControl("~/custom controls/ctrlTax.ascx")
        Dim xLabelValueOutput As New Label
        Dim xLabelTaxFormula As New Label

        Dim newRow As New TableRow
        Dim cell0 As New TableCell
        Dim cell1 As New TableCell
        Dim cell2 As New TableCell
        Dim cell3 As New TableCell

        TaxLatestIDCount += 1

        xButton.ID = "btn_" & TaxLatestIDCount.ToString
        xButton.Text = "x"
        AddHandler xButton.Click, AddressOf xButton_Click
        newTax.ID = "ucTax_" & TaxLatestIDCount.ToString
        AddHandler newTax.SelectTax, AddressOf ucTax_SelectedTax

        xLabelValueOutput.ID = "lblTaxOutput_" & TaxLatestIDCount.ToString
        xLabelTaxFormula.ID = "lblTaxFormula_" & TaxLatestIDCount.ToString

        cell0.Controls.Add(xButton)
        cell1.Controls.Add(newTax)
        cell3.Controls.Add(xLabelTaxFormula)
        cell2.Controls.Add(xLabelValueOutput)

        'newRow.Style.Add("border-right", "2px solid black")
        newRow.Cells.Add(cell0)
        newRow.Cells.Add(cell1)
        newRow.Cells.Add(cell3)
        newRow.Cells.Add(cell2)
        tblComputation.Rows.AddAt(tblComputation.Rows.Count - 1, newRow)

        TaxDynamicControlIDs.Add(TaxLatestIDCount.ToString)
        newEntry.IDIndex = TaxLatestIDCount
        DisbursementVoucherRecord.TaxEntries.Add(newEntry)
    End Sub
    Function ComputeTax(taxType As Integer, percentage As Decimal, shortDesc As String) As Decimal
        Dim output As Decimal = 0
        Select Case taxType
            Case 1
                'output = (DisbursementVoucherRecord.GrossAmount / 1.12) * (percentage / 100)
                If shortDesc = "GAx5%" Then
                    output = (CDec(lblGrossAmount.Text)) * (percentage / 100)
                Else
                    output = (CDec(lblGrossAmount.Text) / 1.12) * (percentage / 100)
                End If
            Case 2
                'output = DisbursementVoucherRecord.GrossAmount * (percentage / 100)
                output = CDec(lblGrossAmount.Text) * (percentage / 100)
            Case 3
                'output = DisbursementVoucherRecord.GrossAmount * (percentage / 100)
                output = CDec(lblGrossAmount.Text) * (percentage / 100)
        End Select
        Return Math.Round(output, 2)
    End Function
    Function getRowIdx(ctrlID As String) As Integer
        Dim idxRet As Integer = 0
        Dim exitfor As Boolean = False
        For count As Integer = 1 To tblComputation.Rows.Count - 1
            idxRet = count
            Dim row = tblComputation.Rows(count)
            Dim ctrlcol = row.Cells(0).Controls
            For Each itm As Control In ctrlcol
                Dim str() = itm.ID.Split("_")
                Dim itmID = str(1)
                If itmID = ctrlID Then
                    exitfor = True
                    Exit For
                End If
            Next
            If exitfor Then Exit For
        Next
        Return idxRet
    End Function
    Sub ComputeParticularsAmountDue()
        If chkNewAmount.Checked = True Then

            Dim grossAmt = CDec(lblGrossAmount.Text)
            Dim totalToDiff As Decimal = 0
            For Each taxRec In DisbursementVoucherRecord.TaxEntries
                If taxRec.DataStatus <> dvdllMaster.DisbursementVoucher.TaxEntryDataStatus.Delete Then
                    totalToDiff += taxRec.TaxOutput
                End If
            Next
            lblGrossAmount.Text = grossAmt
            lblTotalAmountDue.Text = grossAmt - totalToDiff
            DisbursementVoucherRecord.ParticularsAmountDue = grossAmt - totalToDiff

        Else
            Dim grossAmt = DisbursementVoucherRecord.GrossAmount
            Dim totalToDiff As Decimal = 0
            For Each taxRec In DisbursementVoucherRecord.TaxEntries
                If taxRec.DataStatus <> dvdllMaster.DisbursementVoucher.TaxEntryDataStatus.Delete Then
                    If taxRec.TaxOutput Is Nothing Then

                    Else
                        totalToDiff += taxRec.TaxOutput
                    End If

                End If
            Next
            lblGrossAmount.Text = grossAmt
            lblTotalAmountDue.Text = (grossAmt - totalToDiff).Value.ToString("#,#00.00")
            DisbursementVoucherRecord.ParticularsAmountDue = grossAmt - totalToDiff
        End If


        If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "412")
            If Not acctRec Is Nothing Then
                'acctRec0.Credit = DisbursementVoucherRecord.GrossAmount - acctRec.Credit
                acctRec0.Credit = CDec(lblGrossAmount.Text) - acctRec.Credit
            Else
                'acctRec0.Credit = DisbursementVoucherRecord.GrossAmount
                acctRec0.Credit = CDec(lblGrossAmount.Text)
            End If
        End If



        ComputeDebitCredit()
        'RecomputeDebitCredit()
    End Sub

    Sub AddNewTaxSubAcct()
        Dim newEntry As New dvdllMaster.DisbursementVoucher.TaxEntry
        newEntry.ID = Guid.NewGuid
        newEntry.DataStatus = dvdllMaster.DisbursementVoucher.TaxEntryDataStatus.Add
        newEntry.IsSubAcct = True

        Dim taxType As ctrlTaxType2 = Page.LoadControl("~/custom controls/ctrlTaxType2.ascx")
        taxType.ID = "ucTaxType2_" & SubAcctControlCount.ToString
        AddHandler taxType.AddNewSubAcct, AddressOf ucTaxType2_AddNewSubAcct
        AddHandler taxType.RemoveSubAcct, AddressOf ucTaxType2_RemoveSubAcct
        AddHandler taxType.NewComputation, AddressOf ucTaxType2_NewComputation

        pnlTax2.Controls.Add(taxType)

        newEntry.IDIndex = SubAcctControlCount
        SubAcctControlCountList.Add(SubAcctControlCount)
        DisbursementVoucherRecord.TaxEntries.Add(newEntry)

        taxType.TaxLatestIDCount = 0
        taxType.TaxDynamicControlIDs = New List(Of String)
        taxType.TaxEntry = newEntry
        taxType.AddNewTaxLine()
    End Sub
    Private Sub ucTaxType2_AddNewSubAcct()
        SubAcctControlCount += 1
        AddNewTaxSubAcct()
    End Sub
    Private Sub ucTaxType2_RemoveSubAcct(ctrlID As String)
        Dim idx As Integer = ctrlID.Split("_")(1)
        Dim ctrl As ctrlTaxType2 = pnlTax2.FindControl("ucTaxType2_" & idx)
        pnlTax2.Controls.Remove(ctrl)
        SubAcctControlCountList.Remove(CInt(idx))
        Dim remRec = DisbursementVoucherRecord.TaxEntries.FirstOrDefault(Function(p) p.IDIndex = idx)
        If Not remRec Is Nothing Then
            remRec.DataStatus = dvdllMaster.DisbursementVoucher.TaxEntryDataStatus.Delete
            Dim acctRec = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = "412")
            If Not acctRec Is Nothing Then
                Dim taxoutput As Decimal = 0
                If Not remRec.TaxOutput Is Nothing Then
                    taxoutput = remRec.TaxOutput.Value
                End If
                acctRec.Credit = acctRec.Credit - taxoutput
                If acctRec.Credit = 0 Then
                    AccountEntries.Remove(acctRec)
                End If
            End If
        End If

        If SubAcctControlCountList.Count < 1 Then
            AddNewTaxSubAcct()
        End If
    End Sub
    Private Sub ucTaxType2_NewComputation(ctrlID As String)
        Dim idx As Integer = ctrlID.Split("_")(1)
        Dim ctrl As ctrlTaxType2 = pnlTax2.FindControl("ucTaxType2_" & idx)

        Dim taxEntry = DisbursementVoucherRecord.TaxEntries.Find(Function(p) p.ID = ctrl.TaxEntry.ID)
        With taxEntry
            If IsUpdatingDV Then
                .DataStatus = dvdllMaster.DisbursementVoucher.TaxEntryDataStatus.Update
            End If
            .SubAccountAmount = ctrl.TaxEntry.SubAccountAmount
            .SubAccountName = ctrl.TaxEntry.SubAccountName
            .Sub_AcctID = ctrl.TaxEntry.Sub_AcctID
            .TaxSubAcctEntries.Clear()
            .TaxSubAcctEntries.AddRange(ctrl.TaxEntry.TaxSubAcctEntries)
        End With

        If AccountEntries.Exists(Function(p) p.AccountCode = "412") Then
            Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "412")
            acctRec.Credit = ComputeTotalTaxFromSubAccounts()
        Else
            Dim totalTaxSubAcct = ComputeTotalTaxFromSubAccounts()
            If totalTaxSubAcct > 0 Then
                Dim newRec = dvdllMaster.Account.getAccountCode412
                With newRec
                    .Credit = totalTaxSubAcct
                End With
                AccountEntries.Insert(AccountEntries.Count - 1, newRec)
            End If
        End If

        If AccountEntries.Exists(Function(p) p.AcctID = taxEntry.Sub_AcctID) Then
            Dim acctRec = AccountEntries.Find(Function(p) p.AcctID = taxEntry.Sub_AcctID)
            acctRec.Debit = ComputeTotalByAcctID(acctRec.AcctID)
        Else
            Dim newrec As New dvdllMaster.DisbursementVoucher.AccountEntry
            Dim acctRec = dvdllMaster.Account.getAccountByID(taxEntry.Sub_AcctID)
            With newrec
                .ID = Guid.NewGuid
                .AccountCode = acctRec.AccountCode
                .AcctsAndExplanation = acctRec.AccountDescription
                .Credit = 0
                .Debit = taxEntry.SubAccountAmount
                .Ref = ""
                .ResponsibilityCenter = ""
                .AcctID = acctRec.ID
            End With
            Dim insertIdx As Integer = IIf(AccountEntries.Count > 2, AccountEntries.IndexOf(AccountEntries.Find(Function(p) p.AccountCode = "412")) - 1, 0)
            AccountEntries.Insert(insertIdx, newrec)
        End If

        If AccountEntries.Exists(Function(p) p.AccountCode = "10104040") Then
            Dim acctRec0 = AccountEntries.Find(Function(p) p.AccountCode = "10104040")
            Dim acctRec = AccountEntries.Find(Function(p) p.AccountCode = "412")
            Dim totalCredit = 0
            If Not acctRec Is Nothing Then
                'acctRec0.Credit = DisbursementVoucherRecord.GrossAmount - acctRec.Credit
                acctRec0.Credit = ComputeTotalSubAccounts() - acctRec.Credit
            Else
                'acctRec0.Credit = DisbursementVoucherRecord.GrossAmount
                acctRec0.Credit = ComputeTotalSubAccounts()
            End If
        End If
        ComputeDebitCredit()
        LoadAccountEntries()
        'RecomputeDebitCredit()
    End Sub
    Function ComputeTotalTaxFromSubAccounts()
        Dim total As Decimal = 0
        For Each taxentry In DisbursementVoucher.TaxEntries
            total += taxentry.SubAcctEntriesTaxTotal
        Next
        Return total
    End Function
    Function ComputeTotalByAcctID(acctID As Guid)
        Dim total As Decimal = 0
        For Each entry In DisbursementVoucher.TaxEntries.Where(Function(p) p.Sub_AcctID = acctID And p.DataStatus <> dvdllMaster.DisbursementVoucher.TaxEntryDataStatus.Delete).ToList()
            total += entry.SubAccountAmount
        Next
        Return total
    End Function
    Function ComputeTotalSubAccounts()
        Dim total As Decimal = 0
        For Each entry In DisbursementVoucher.TaxEntries.Where(Function(p) p.DataStatus <> dvdllMaster.DisbursementVoucher.TaxEntryDataStatus.Delete).ToList
            total += entry.SubAccountAmount
        Next
        Return total
    End Function

    Sub RefreshAcctEntries()
        Dim temp As New List(Of dvdllMaster.DisbursementVoucher.AccountEntry)
        temp.Add(AccountEntries.Where(Function(p) p.AccountCode = "10104040").First)
        AccountEntries = New List(Of dvdll.Master.DisbursementVoucher.AccountEntry)
        AccountEntries.Add(temp.First)
    End Sub

    Sub ComputeDebitCredit()
        Dim locDebit As Double = 0.0
        Dim locCredit As Double = 0.0
        TotalCredit = 0
        TotalDebit = 0

        For Each entry In AccountEntries
            TotalCredit += entry.Credit
            TotalDebit += entry.Debit
            'MsgBox(entry.AcctsAndExplanation + " | Debit: " + entry.Debit.ToString("#,##0.00") + " | Credit: " + entry.Credit.ToString("#,##0.00"))
        Next
    End Sub

    Sub LoadAccountCodes()
        ddlAccountCode.Items.Add(New ListItem With {.Text = "[--Select--]", .Value = -1})
        ddlAccountCode.DataSource = dvdll.Master.Account.getAccountEntryRecordOrderByName
        ddlAccountCode.DataBind()
    End Sub
    '==================================================================================
    '================================UPDATE/EDIT=======================================
    Public Sub InitializeFormForUpdate(dvID As Guid)
        LoadRadioButtons(dvdllMaster.ModeOfPayment.getModeOfPayments, dvdllMaster.VoucherType.getVoucherTypes)
        DisbursementVoucherRecord = New dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord
        IsUpdatingDV = True
        TaxDynamicControlIDs = New List(Of String)
        TaxLatestIDCount = 0
        AccountEntries = New List(Of dvdll.Master.DisbursementVoucher.AccountEntry)

        DisbursementVoucherRecord = dvdllMaster.DisbursementVoucher.getDVRecordByID(dvID)
        If Not DisbursementVoucherRecord Is Nothing Then
            AccountEntries = dvdllMaster.Account.getAccountEntriesByDVID(DisbursementVoucherRecord.ID)
            DisbursementVoucher.TaxEntries = dvdllMaster.DisbursementVoucher.getTaxEntryRecordByDVID(DisbursementVoucher.ID)

            lblPayeeName.Text = DisbursementVoucherRecord.PayeeName
            txtPayeeName.Text = DisbursementVoucherRecord.PayeeName
            lblPayeeAddress.Text = DisbursementVoucherRecord.PayeeAddress
            txtPayeeAddress.Text = DisbursementVoucherRecord.PayeeAddress
            lblPayeeOffice.Text = DisbursementVoucherRecord.PayeeOffice
            txtPayeeOffice.Text = DisbursementVoucherRecord.PayeeOffice
            'lblPayeeTIN.Text = DisbursementVoucherRecord.PayeeTIN
            txtPayeeTin.Text = DisbursementVoucherRecord.PayeeTIN
            txtObrNo.Text = DisbursementVoucherRecord.ObRNo
            txtObrDate.Text = DisbursementVoucherRecord.ObRDateCreated
            lblDVDateCreated.Text = Now.ToShortDateString
            lblParticularTemplate.Text = DisbursementVoucherRecord.DVParticularTemplate
            txtParticularTemplate.Text = DisbursementVoucherRecord.DVParticularTemplate
            txtvoucherdate.Text = Now.Date
            lbltotalcredit.Text = DisbursementVoucherRecord.GrossAmount
            lbltotaldebit.Text = DisbursementVoucherRecord.GrossAmount
            rdBtnModeOfPayment.SelectedValue = DisbursementVoucher.ModeOfPaymentID.ToString
            'rdBtnVoucherType.SelectedValue = DisbursementVoucher.VoucherTypeID.ToString
            ComputeParticularsAmountDue()

            For Each taxrec In DisbursementVoucher.TaxEntries
                Dim xButton As New Button
                Dim newTax As ctrlTax = Page.LoadControl("~/custom controls/ctrlTax.ascx")
                Dim xLabelValueOutput As New Label
                Dim xLabelTaxFormula As New Label

                Dim newRow As New TableRow
                Dim cell0 As New TableCell
                Dim cell1 As New TableCell
                Dim cell2 As New TableCell
                Dim cell3 As New TableCell

                TaxLatestIDCount += 1

                xButton.ID = "btn_" & TaxLatestIDCount.ToString
                xButton.Text = "x"
                AddHandler xButton.Click, AddressOf xButton_Click
                newTax.ID = "ucTax_" & TaxLatestIDCount.ToString
                AddHandler newTax.SelectTax, AddressOf ucTax_SelectedTax

                xLabelValueOutput.ID = "lblTaxOutput_" & TaxLatestIDCount.ToString
                xLabelTaxFormula.ID = "lblTaxFormula_" & TaxLatestIDCount.ToString

                xLabelValueOutput.Text = taxrec.TaxOutput.ToString("#,#00.00")
                xLabelTaxFormula.Text = taxrec.TaxDescription

                cell0.Controls.Add(xButton)
                cell1.Controls.Add(newTax)
                cell3.Controls.Add(xLabelTaxFormula)
                cell2.Controls.Add(xLabelValueOutput)

                newRow.Cells.Add(cell0)
                newRow.Cells.Add(cell1)
                newRow.Cells.Add(cell3)
                newRow.Cells.Add(cell2)
                tblComputation.Rows.AddAt(tblComputation.Rows.Count - 1, newRow)

                TaxDynamicControlIDs.Add(TaxLatestIDCount.ToString)
                taxrec.IDIndex = TaxLatestIDCount
            Next
            LoadAccountEntries()
            AddNewTaxLine()
        End If
    End Sub
    '==================================================================================

    Public Sub ComputePayroll()
        Dim total As Decimal = 0
        Dim acct108 As dvdll.Master.DisbursementVoucher.AccountEntry

        If Not String.IsNullOrEmpty(txtGsis.Text) And Not String.IsNullOrWhiteSpace(txtGsis.Text) Then
            total += CDec(txtGsis.Text)
        ElseIf Not String.IsNullOrEmpty(txtPagibig.Text) And Not String.IsNullOrWhiteSpace(txtPagibig.Text) Then
            total += CDec(txtPagibig.Text)
        ElseIf Not String.IsNullOrEmpty(txtPhilHealth.Text) And Not String.IsNullOrWhiteSpace(txtPhilHealth.Text) Then
            total += CDec(txtPhilHealth.Text)
        ElseIf Not String.IsNullOrEmpty(txtBIR.Text) And Not String.IsNullOrWhiteSpace(txtBIR.Text) Then
            total += CDec(txtBIR.Text)
        ElseIf Not String.IsNullOrEmpty(txtIntelicare.Text) And Not String.IsNullOrWhiteSpace(txtIntelicare.Text) Then
            total += CDec(txtIntelicare.Text)
        ElseIf Not String.IsNullOrEmpty(txtCashAdvances.Text) And Not String.IsNullOrWhiteSpace(txtCashAdvances.Text) Then
            total += CDec(txtCashAdvances.Text)
        End If

        'Dim salaryAcct = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = "5010101001" Or "1030502000" Or "5021103000" Or "5021601000")

        'Dim salaryAcct As New dvdll.Master.DisbursementVoucher.AccountEntry

        'For Each acct In AccountEntries.Where(Function(p) p.AccountCode <> "412" Or p.AccountCode <> "10104040").ToList
        '    salaryAcct.Debit += acct.Debit
        'Next

        'If Not salaryAcct Is Nothing Then
        'Dim acct108 = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = dvdllMaster.Account.SpecialAccountCodes.CashNationalTreasury)

        If GlobalAcctRecs.lastAcctRecCode = "10104040" Then
            acct108 = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = dvdllMaster.Account.SpecialAccountCodes.CashNationalTreasury)
        Else
            acct108 = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
        End If

        If Not acct108 Is Nothing Then
            'acct108.Credit = salaryAcct.Debit - total
            acct108.Credit -= total
        End If
        'End If


    End Sub
    Public Sub ComputeRemit()
        Dim acct108 As dvdll.Master.DisbursementVoucher.AccountEntry
        Dim total As Decimal = 0

        If Not String.IsNullOrEmpty(txtRemitGSIS.Text) And Not String.IsNullOrWhiteSpace(txtRemitGSIS.Text) Then
            total += CDec(txtRemitGSIS.Text)
        ElseIf Not String.IsNullOrEmpty(txtRemitPagibig.Text) And Not String.IsNullOrWhiteSpace(txtRemitPagibig.Text) Then
            total += CDec(txtRemitPagibig.Text)
        ElseIf Not String.IsNullOrEmpty(txtRemitPhilHealth.Text) And Not String.IsNullOrWhiteSpace(txtRemitPhilHealth.Text) Then
            total += CDec(txtRemitPhilHealth.Text)
        ElseIf Not String.IsNullOrEmpty(txtRemitBIR.Text) And Not String.IsNullOrWhiteSpace(txtRemitBIR.Text) Then
            total += CDec(txtRemitBIR.Text)
        ElseIf Not String.IsNullOrEmpty(txtRemitIntelicare.Text) And Not String.IsNullOrWhiteSpace(txtRemitIntelicare.Text) Then
            total += CDec(txtRemitIntelicare.Text)
        End If

        If GlobalAcctRecs.lastAcctRecCode = "10104040" Then
            acct108 = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = dvdllMaster.Account.SpecialAccountCodes.CashNationalTreasury)
        Else
            acct108 = AccountEntries.FirstOrDefault(Function(p) p.AccountCode = GlobalAcctRecs.lastAcctRecCode)
        End If
        If Not acct108 Is Nothing Then
            acct108.Credit = acct108.Credit + total
        End If
    End Sub

#End Region

    Private Sub ucConfirmExtender_Confirmed(ctrlID As String) Handles ucConfirmExtender.Confirmed
        If ctrlID = chTaxType2.ID Then
            DisbursementVoucherRecord.TaxEntries = New List(Of dvdll.Master.DisbursementVoucher.TaxEntry)
            SubAcctControlCountList = New List(Of Integer)
            SubAcctControlCount = 1
            TaxDynamicControlIDs = New List(Of String)
            TaxLatestIDCount = 0
            If chTaxType2.Checked Then
                AddNewTaxSubAcct()
                pnlTax1.Visible = False
                pnlTax2.Visible = True
            Else
                AddNewTaxLine()
                pnlTax2.Visible = False
                pnlTax1.Visible = True
            End If
            RefreshAcctEntries()
            LoadAccountEntries()
        End If
    End Sub
    Private Sub ucConfirmExtender_Cancelled(ctrlID As String) Handles ucConfirmExtender.Cancelled
        If ctrlID = chTaxType2.ID Then
            If chTaxType2.Checked Then
                chTaxType2.Checked = False
            Else
                chTaxType2.Checked = True
            End If
        End If
    End Sub

    Private Sub ucConfirmExtenderDetail_Confirmed(ctrlID As String) Handles ucConfirmExtenderDetail.Confirmed
        'SaveDetails
        'Dim DVDate As Date = DateTime.Parse(txtDVDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture)
        DisbursementVoucher.DisbursementDateCreated = txtDVDate.Text
        DisbursementVoucher.DVSignatoryA = signatorySelectionA.SelectedID
        DisbursementVoucher.DVSignatoryB = signatorySelectionB.SelectedID
        DisbursementVoucher.DVSignatoryC = signatorySelectionC.SelectedID
        DisbursementVoucher.DVSignatoryD = signatorySelectionD.SelectedID
        DisbursementVoucher.DVSignatoryE = signatorySelectionE.SelectedID
        DisbursementVoucher.DVSignatoryF = signatorySelectionF.SelectedID
        DisbursementVoucher.DVPreparedBy = signatorySelectionG.SelectedID
        DisbursementVoucher.DVApprovedBy = signatorySelectionH.SelectedID

        DisbursementVoucher.dUser = dUser

        dvdll.Master.DisbursementVoucher.SaveDV(DisbursementVoucher, AccountEntries)
        ucInfoWindow.SetHeader("INFO")
        ucInfoWindow.SetMessage("Successfully Saved Disbursement Voucher [" + DisbursementVoucher.DisbursementVoucherNo + "]")
        ucInfoWindow.ShowInfo()

        SubAcctControlCountList = New List(Of Integer)
        SubAcctControlCount = 0
        TaxDynamicControlIDs = New List(Of String)
        TaxLatestIDCount = 0
        RaiseEvent DisbursementSaved()
    End Sub

    Protected Sub btnDisregard_Click(sender As Object, e As EventArgs) Handles btnDisregard.Click
        RaiseEvent DisbursementDisregarded()
    End Sub
End Class
Public Class GlobalAcctRecs
    Public Shared firstAcctRecExp As String
    Public Shared lastAcctRecExp As String
    Public Shared firstAcctRecCode As String
    Public Shared lastAcctRecCode As String
End Class