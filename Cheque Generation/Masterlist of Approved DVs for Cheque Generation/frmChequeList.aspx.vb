Imports Microsoft.Reporting.WebForms

Public Class frmChequeList
    Inherits System.Web.UI.Page

#Region "Properties"

    Property ChCreated As List(Of dvdll.Master.ChequeGeneration.ChequeGenerationRecord)
        Get
            Return ViewState("ChCreated")
        End Get
        Set(value As List(Of dvdll.Master.ChequeGeneration.ChequeGenerationRecord))
            ViewState("ChCreated") = value
        End Set
    End Property

    Property CHPayeeSearchList As List(Of dvdll.Master.ChequeGeneration.ChequeGenerationRecord)
        Get
            Return ViewState("CHPayeeSearchList")
        End Get
        Set(value As List(Of dvdll.Master.ChequeGeneration.ChequeGenerationRecord))
            ViewState("CHPayeeSearchList") = value
        End Set
    End Property

    Property ChequeGenerated As dvdll.Master.ChequeGeneration.ChequeGenerationRecord
        Get
            Return ViewState("ChequeGenerated")
        End Get
        Set(value As dvdll.Master.ChequeGeneration.ChequeGenerationRecord)
            ViewState("ChequeGenerated") = value
        End Set
    End Property

    Property SelectedChequeID As Guid
        Get
            Return ViewState("SelectedChequeID")
        End Get
        Set(value As Guid)
            ViewState("SelectedChequeID") = value
        End Set
    End Property

    Property GridPrevSortExpression As String
        Get
            Return ViewState("GridPrevSortExpression")
        End Get
        Set(value As String)
            ViewState("GridPrevSortExpression") = value
        End Set
    End Property

    Property GridSortDirection As String
        Get
            Return ViewState("GridSortDirection")
        End Get
        Set(value As String)
            ViewState("GridSortDirection") = value
        End Set
    End Property

    Property IsSearch As Boolean
        Get
            Return ViewState("IsSearch")
        End Get
        Set(value As Boolean)
            ViewState("IsSearch") = value
        End Set
    End Property

    Property PrioCHNo As String
        Get
            Return ViewState("PrioCHNo")
        End Get
        Set(value As String)
            ViewState("PrioCHNo") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
        LoadGridCreatedCh()
    End Sub

    Sub LoadGridCreatedCh()
        'If IsSearch Then
        '    CHPayeeSearchList = dvdll.Master.ChequeGeneration.getCHPayeeByFilter(txtsearch.Text)
        '    grdApprovedChList.DataSource = CHPayeeSearchList
        'Else
        '    ChCreated = dvdll.Master.ChequeGeneration.getChByStatus(dvdll.Master.ChequeGeneration.ChequeStatus.Created)
        '    grdApprovedChList.DataSource = ChCreated
        'End If
        'grdApprovedChList.DataBind()

        If IsSearch Then
            ChCreated = dvdll.Master.ChequeGeneration.getCHPayeeByFilter(txtsearch.Text)
        Else
            ChCreated = dvdll.Master.ChequeGeneration.getChByStatus(dvdll.Master.ChequeGeneration.ChequeStatus.Created)
        End If

        grdApprovedChList.DataSource = ChCreated
        grdApprovedChList.DataBind()
    End Sub

    Private Sub grdApprovedChList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdApprovedChList.PageIndexChanging
        grdApprovedChList.PageIndex = e.NewPageIndex
        LoadGridCreatedCh()
    End Sub

    Private Sub grdApprovedChList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdApprovedChList.RowCommand
        If e.CommandName = "xClear" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ChequeGeneration.getChRecordByID(id)
            dvdll.Master.ChequeGeneration.UpdateChequeStatus(id, dvdll.Master.ChequeGeneration.ChequeStatus.Cleared)

        ElseIf e.CommandName = "xCancel" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ChequeGeneration.getChRecordByID(id)
            If Not rec Is Nothing Then
                ChDetails(id)
                pnlMainDV.Visible = False
                pnlCancel.Visible = True
            End If

        ElseIf e.CommandName = "xPrint" Then
            'Dim id = Guid.Parse(e.CommandArgument)
            'Dim rec = dvdll.Master.ChequeGeneration.getChIDtoPrint(id)
            'testprint(rec)
            'pnlPrintCheque.Visible = True
            'pnlMainDV.Visible = False


        ElseIf e.CommandName = "xBackToDV" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ChequeGeneration.getChRecordByID(id)
            ChDetails(id)
            pnlMainDV.Visible = False
            pnlCancel.Visible = True
            btnCancelCheque.Visible = False
            btnBackToDV.Visible = True

        ElseIf e.CommandName = "xCancelDV" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ChequeGeneration.getChRecordByID(id)
            ChDetails(id)
            pnlMainDV.Visible = False
            pnlCancel.Visible = True
            btnCancelCheque.Visible = False
            btnBackToDV.Visible = False
            btnCancelDV.Visible = True

        ElseIf e.CommandName = "xEditCheque" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ChequeGeneration.getChRecordByID(id)
            CHEdit(id)
            pnlChequeGen.Visible = True
            pnlMainDV.Visible = False
            btnCancelCheque.Visible = False
            btnBackToDV.Visible = False

        End If

    End Sub

    Public Sub ChDetails(ChID As Guid)
        ChequeGenerated = dvdll.Master.ChequeGeneration.getChRecordByID(ChID)
        With ChequeGenerated
            txtPayToOrder.Text = ChequeGenerated.PayToOrder
            txtAmount.Text = ChequeGenerated.Amount
            txtAmountInWords.Text = ChequeGenerated.AmountInWords
            lblChequeNo.Text = ChequeGenerated.ChequeNo
        End With
        SelectedChequeID = ChequeGenerated.ID
    End Sub

    Public Sub CHEdit(ChID As Guid)
        ChequeGenerated = dvdll.Master.ChequeGeneration.getChRecordByID(ChID)

        With ChequeGenerated
            txtChequeDateEdit.Text = ChequeGenerated.ChequeDate
            txtChequeNoEdit.Text = ChequeGenerated.ChequeNo
            txtResultEdit.Text = ChequeGenerated.AmountInWords
            txtPayOrderToEdit.Text = ChequeGenerated.PayToOrder
            txtNumberEdit.Text = ChequeGenerated.Amount
            Dim sigA = Guid.Parse(ChequeGenerated.ChSignatoryA.Value.ToString)
            Dim SigB = Guid.Parse(ChequeGenerated.ChsignatoryB.Value.ToString)
            signatorySelectionA.SetSignatoryText(dvdll.Master.Signatories.getSigByID(sigA).Name)
            signatorySelectionB.SetSignatoryText(dvdll.Master.Signatories.getSigByID(SigB).Name)
            End With
        SelectedChequeID = ChequeGenerated.ID
    End Sub

    Private Sub calChequeDate_SelectionChanged(sender As Object, e As EventArgs) Handles calChequeDate.SelectionChanged
        Dim chequedate As Date

        chequedate = calChequeDate.SelectedDate
        txtChequeDateEdit.Text = chequedate.ToString("MMMM dd, yyyy")
        pnlChequeGen.Visible = True
    End Sub

    Private Sub calCancelDate_SelectionChanged(sender As Object, e As EventArgs) Handles calCancelDate.SelectionChanged
        Dim canceldate As Date

        canceldate = calCancelDate.SelectedDate
        txtCancellationDate.Text = canceldate.ToString("MMMM, dd yyyy")
        pnlCancel.Visible = True
    End Sub

    Private Sub btnBackToList_Click(sender As Object, e As EventArgs) Handles btnBackToList.Click
        pnlCancel.Visible = False
        pnlMainDV.Visible = True
    End Sub

    Private Sub btnCancelCheque_Click(sender As Object, e As EventArgs) Handles btnCancelCheque.Click
        Try
            dvdll.Master.ChequeGeneration.CancelCheque(SelectedChequeID, txtCancellationDate.Text, txtRemarks.Text)

        Catch ex As Exception
        End Try

        pnlCancel.Visible = False
        pnlMainDV.Visible = True
        LoadGridCreatedCh()
    End Sub

    Private Sub btnBackToDV_Click(sender As Object, e As EventArgs) Handles btnBackToDV.Click
        Try
            dvdll.Master.ChequeGeneration.ChequeBacktoDV(SelectedChequeID, txtCancellationDate.Text, txtRemarks.Text)

        Catch ex As Exception
        End Try

        pnlCancel.Visible = False
        pnlMainDV.Visible = True
        LoadGridCreatedCh()
    End Sub

    Private Sub btnCancelDV_Click(sender As Object, e As EventArgs) Handles btnCancelDV.Click

        Try
            dvdll.Master.ChequeGeneration.CancelDV(SelectedChequeID, txtCancellationDate.Text, txtRemarks.Text)

        Catch ex As Exception
        End Try

        pnlCancel.Visible = False
        pnlMainDV.Visible = True
        LoadGridCreatedCh()

    End Sub

    Sub testprint(rec As dvdll.ch_Cheque)
        Dim dummylst = New List(Of dvdll.ch_Cheque)
        dummylst.Add(rec)

        rptChViewer.Visible = True
        rptChViewer.Enabled = True
        rptChViewer.Reset()
        rptChViewer.LocalReport.ReportPath = "Auth/Transactions/rdlc/rptCheque.rdlc"
        rptChViewer.LocalReport.DataSources.Clear()
        rptChViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dummylst))
        rptChViewer.LocalReport.Refresh()

        'rptDVViewer.Visible = True
        'rptDVViewer.Enabled = True
        'rptDVViewer.Reset()
        'rptDVViewer.LocalReport.ReportPath = "Auth/Transactions/rdlc/rptDisbursementVoucher.rdlc"
        'rptDVViewer.LocalReport.DataSources.Clear()
        'rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dummylst))
        'rptDVViewer.LocalReport.Refresh()
    End Sub




#Region "Grid Sort"

    Private Sub grdApprovedChList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdApprovedChList.RowDataBound


        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each item In ChCreated
                If item.CHisPriority = True Then
                    PrioCHNo = item.ChequeNo
                    If Convert.ToString(e.Row.Cells(1).Text) = PrioCHNo Then
                        e.Row.BackColor = Drawing.Color.Aquamarine
                    End If
                End If
            Next
        End If

    End Sub



    Private Sub grdApprovedChList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdApprovedChList.Sorting
        Dim currentSortDirection = getSortDirection(e.SortExpression)

        Select Case e.SortExpression
            Case "ChequeNo"
                If currentSortDirection = "ASC" Then
                    ChCreated = (From p In ChCreated Order By p.ChequeNo Ascending Select p).ToList
                Else
                    ChCreated = (From p In ChCreated Order By p.ChequeNo Descending Select p).ToList
                End If
            Case "PayToOrder"
                If currentSortDirection = "ASC" Then
                    ChCreated = (From p In ChCreated Order By p.PayToOrder Ascending Select p).ToList
                Else
                    ChCreated = (From p In ChCreated Order By p.PayToOrder Descending Select p).ToList
                End If
            Case "ChequeDate"
                If currentSortDirection = "ASC" Then
                    ChCreated = (From p In ChCreated Order By p.ChequeDate Ascending Select p).ToList
                Else
                    ChCreated = (From p In ChCreated Order By p.ChequeDate Descending Select p).ToList
                End If
            Case "Amount"
                If currentSortDirection = "ASC" Then
                    ChCreated = (From p In ChCreated Order By p.Amount Ascending Select p).ToList
                Else
                    ChCreated = (From p In ChCreated Order By p.Amount Descending Select p).ToList
                End If
        End Select

        grdApprovedChList.DataSource = ChCreated
        grdApprovedChList.DataBind()
    End Sub

    Function getSortDirection(currentSortExpression As String) As String
        If GridPrevSortExpression <> Nothing Then
            If currentSortExpression = GridPrevSortExpression Then
                GridSortDirection = IIf(GridSortDirection = "ASC", "DESC", "ASC")
            Else
                GridSortDirection = "ASC"
            End If
        End If
        GridPrevSortExpression = currentSortExpression
        Return GridSortDirection
    End Function

#End Region

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        txtsearch.Focus()
        If Not String.IsNullOrEmpty(txtsearch.Text) And Not String.IsNullOrWhiteSpace(txtsearch.Text) Then
            IsSearch = True
            CHPayeeSearchList = dvdll.Master.ChequeGeneration.getCHPayeeByFilter(txtsearch.Text)
        Else
            IsSearch = False
            CHPayeeSearchList = New List(Of dvdll.Master.ChequeGeneration.ChequeGenerationRecord)
        End If
        LoadGridCreatedCh()
    End Sub

    Private Sub btnUpdateCheque_Click(sender As Object, e As EventArgs) Handles btnUpdateCheque.Click
        Dim signatoryA As Guid
        Dim signatoryB As Guid

        signatoryA = signatorySelectionA.SelectedID
        signatoryB = signatorySelectionB.SelectedID
        dvdll.Master.ChequeGeneration.UpdateCheque(SelectedChequeID, txtChequeNoEdit.Text, txtChequeDateEdit.Text, signatoryA, signatoryB)


    End Sub
End Class