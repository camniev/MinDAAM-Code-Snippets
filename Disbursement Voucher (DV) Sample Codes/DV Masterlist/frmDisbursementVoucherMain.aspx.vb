Imports Microsoft.Reporting.WebForms

Public Class DisbursementVoucherMain
    Inherits System.Web.UI.Page

#Region "Property"

    Property DVPayeeSearchList As List(Of dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord)
        Get
            Return ViewState("DVPayeeSearchList")
        End Get
        Set(value As List(Of dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord))
            ViewState("DVPayeeSearchList") = value
        End Set
    End Property

    Property ObRList As List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord)
        Get
            Return ViewState("ObRList")
        End Get
        Set(value As List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord))
            ViewState("ObRList") = value
        End Set
    End Property

    Property ObRListdv As List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord)
        Get
            Return ViewState("ObRListdv")
        End Get
        Set(value As List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord))
            ViewState("ObRListdv") = value
        End Set
    End Property

    Property SelectedObRID As Guid
        Get
            Return ViewState("SelectedObRID")
        End Get
        Set(value As Guid)
            ViewState("SelectedObRID") = value
        End Set
    End Property

    Property SelectedDVID As Guid
        Get
            Return ViewState("SelectedDVID")
        End Get
        Set(value As Guid)
            ViewState("SelectedDVID") = value
        End Set
    End Property

    Property DVQueue As List(Of dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord)
        Get
            Return ViewState("DVQueue")
        End Get
        Set(value As List(Of dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord))
            ViewState("DVQueue") = value
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

    Property haveSub As Boolean
        Get
            Return ViewState("haveSub")
        End Get
        Set(value As Boolean)
            ViewState("haveSub") = value
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

    Property PrioDVNo As String
        Get
            Return ViewState("PrioDVNo")
        End Get
        Set(value As String)
            ViewState("PrioDVNo") = value
        End Set
    End Property
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadGridDV()
            pnlPrintDV.Visible = False
            pnlViewDetails.Visible = False
            pnlMainDV.Visible = True
            txtApprovalDate.Text = Now.ToShortDateString
            mpeApprovalDate.Hide()
        End If
    End Sub

    Private Sub grdDVList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDVList.PageIndexChanging
        grdDVList.PageIndex = e.NewPageIndex
        LoadGridDV()
    End Sub

    Sub LoadGridDV()
        'DVQueue = dvdll.Master.DisbursementVoucher.getDVByStatus(dvdll.Master.DisbursementVoucher.DVStatus.ForApproval)
        'grdDVList.DataSource = DVQueue
        'grdDVList.DataBind()

        If IsSearch Then
            DVQueue = dvdll.Master.DisbursementVoucher.getDVByPayee(txtsearch.Text, dvdll.Master.DisbursementVoucher.DVStatus.ForApproval)
        Else
            DVQueue = dvdll.Master.DisbursementVoucher.getDVByStatus(dvdll.Master.DisbursementVoucher.DVStatus.ForApproval)
        End If
        grdDVList.DataSource = DVQueue
        grdDVList.DataBind()
    End Sub

    Private Sub grdDVList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdDVList.RowCommand
        If e.CommandName = "xApproveDV" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.DisbursementVoucher.getDVByID(id)
            If Not rec Is Nothing Then
                SelectedDVID = rec.ID
            End If
            mpeApprovalDate.Show()


        ElseIf e.CommandName = "xPrint" Then
            pnlMainDV.Visible = False
            pnlPrintDV.Visible = True
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.DisbursementVoucher.getDVByID(id)
            If Not rec Is Nothing Then
                LoadDVtoPrint(rec)
            End If

        ElseIf e.CommandName = "xCancel" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.DisbursementVoucher.getDVByID(id)
            Try
                dvdll.Master.DisbursementVoucher.UpdateDVStatus(id, dvdll.Master.DisbursementVoucher.DVStatus.Cancelled)

                SelectedObRID = dvdll.Master.DisbursementVoucher.getDVByID(id).ObligationRequestID
                dvdll.Master.ObligationRequest.UpdateOBStatus(SelectedObRID, dvdll.Master.ObligationRequest.ObRStatus.OnQueue)

            Catch ex As Exception
                'ShowInfoPopUp("Obligation Request Approved", "Items on Queue for Disbursement Voucher")
            End Try
            LoadGridDV()

        ElseIf e.CommandName = "xEditDV" Then
            Dim id = Guid.Parse(e.CommandArgument)
            ctrlEditDisbursementVoucher.InitializeFormForUpdate(id)
            pnlMainDV.Visible = False
            pnlPrintDV.Visible = False
            pnlViewDetails.Visible = True
        End If
    End Sub

    Sub LoadDVtoPrint(dvRec As dvdll.dv_DisbursementVoucher)
        Dim dsDv = New List(Of dvdll.dv_DisbursementVoucher)
        dsDv.Add(dvRec)



        Dim dvParticlarEntry = dvdll.Master.DisbursementVoucher.getDVParticularEntryByDVID(dvRec.ID)
        Dim dvAccountEntry = dvdll.Master.DisbursementVoucher.getDVAccountEntryByDVID(dvRec.ID)
        Dim dsObRParticulars = dvdll.Master.DisbursementVoucher.getORParticularsByORID(dvRec.ObligationRequestID)
        Dim dvTaxEntry = dvdll.Master.DisbursementVoucher.getDVTaxEntryByDVID(dvRec.ID)
        Dim dvTaxEntry2 = dvdll.Master.DisbursementVoucher.getDVTaxEntryByDVID2(dvRec.ID)
        Dim dvPPA = dvdll.Master.DisbursementVoucher.getDVPPA(dsDv(0).dv_ParticularEntry(0).PPAID)
        Dim dvRESPO = dvdll.Master.DisbursementVoucher.getDVRESPO(dsDv(0).dv_ParticularEntry(0).ResponsibilityCenterID)
        Dim isSub = dvTaxEntry.Find(Function(p) p.IsSubAcct = True)
        Dim dvSigA = dvdll.Master.DisbursementVoucher.getDVSignatoryA(dsDv(0).DVSignatoryA)
        Dim dvSigB = dvdll.Master.DisbursementVoucher.getDVSignatoryB(dsDv(0).DVSignatoryB)
        Dim dvSigC = dvdll.Master.DisbursementVoucher.getDVSignatoryC(dsDv(0).DVSignatoryC)
        Dim dvSigD = dvdll.Master.DisbursementVoucher.getDVSignatoryD(dsDv(0).DVSignatoryD)
        Dim dvSigE = dvdll.Master.DisbursementVoucher.getDVSignatoryE(dsDv(0).DVSignatoryE)
        Dim dvSigF = dvdll.Master.DisbursementVoucher.getDVSignatoryF(dsDv(0).DVSignatoryF)
        Dim dvCertifiedBy = dvdll.Master.DisbursementVoucher.getDVCertifiedBy(dsDv(0).DVPreparedBy)
        Dim dvApprovedBy = dvdll.Master.DisbursementVoucher.getDVApprovedBy(dsDv(0).DVApprovedBy)

        If Not isSub Is Nothing Then
            Dim dvTaxSubAccounts = dvdll.Master.DisbursementVoucher.getDVTaxSubAccountByTaxEntryID(dvRec.ID)
            rptDVViewer.Visible = True
            rptDVViewer.Enabled = True
            rptDVViewer.Reset()
            rptDVViewer.LocalReport.ReportPath = "Auth/Transactions/rdlc/rptDisbursementVoucherWithSub.rdlc"
            rptDVViewer.LocalReport.DataSources.Clear()
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dsDv))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", dvParticlarEntry))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet3", dvAccountEntry))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet4", dvTaxEntry))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet5", dvTaxSubAccounts))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet6", dvPPA))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet7", dvRESPO))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet8", dsObRParticulars))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigA", dvSigA))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigB", dvSigB))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigC", dvSigC))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigD", dvSigD))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigE", dvSigE))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigF", dvSigF))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigCertifiedBy", dvCertifiedBy))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigApprovedBy", dvApprovedBy))
            rptDVViewer.LocalReport.Refresh()
        Else
            rptDVViewer.Visible = True
            rptDVViewer.Enabled = True
            rptDVViewer.Reset()
            rptDVViewer.LocalReport.ReportPath = "Auth/Transactions/rdlc/rptDisbursementVoucher.rdlc"
            rptDVViewer.LocalReport.DataSources.Clear()
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dsDv))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", dvParticlarEntry))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet3", dvAccountEntry))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet4", dvTaxEntry))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet5", dvPPA))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet6", dvRESPO))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet7", dvTaxEntry2))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet8", dsObRParticulars))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigA", dvSigA))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigB", dvSigB))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigC", dvSigC))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigD", dvSigD))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigE", dvSigE))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigF", dvSigF))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigCertifiedBy", dvCertifiedBy))
            rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DVSigApprovedBy", dvApprovedBy))
            rptDVViewer.LocalReport.Refresh()

        End If
    End Sub

    Private Sub grdDVList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDVList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each item In DVQueue
                If item.DVisPriority = True Then
                    PrioDVNo = item.DisbursementVoucherNo
                    If Convert.ToString(e.Row.Cells(1).Text) = PrioDVNo Then
                        e.Row.BackColor = Drawing.Color.Aquamarine
                    End If
                End If
            Next
        End If
    End Sub

    'Sub LoadDVtoPrint(dvRec As dvdll.dv_DisbursementVoucher)
    '    Dim dsDv = New List(Of dvdll.dv_DisbursementVoucher)
    '    'Dim getTaxEntryID = dvdll.Master.DisbursementVoucher.getTaxID(dvRec.ID)
    '    dsDv.Add(dvRec)

    '    Dim dvParticlarEntry = dvdll.Master.DisbursementVoucher.getDVParticularEntryByDVID(dvRec.ID)
    '    Dim dvAccountEntry = dvdll.Master.DisbursementVoucher.getDVAccountEntryByDVID(dvRec.ID)
    '    Dim dvTaxEntry = dvdll.Master.DisbursementVoucher.getDVTaxEntryByDVID(dvRec.ID)
    '    'Dim dvSubAccount = dvdll.Master.DisbursementVoucher.getDVSubAccountEntryByTaxEntryID(getTaxEntryID.ID)
    '    rptDVViewer.Visible = True
    '    rptDVViewer.Enabled = True
    '    rptDVViewer.Reset()
    '    rptDVViewer.LocalReport.ReportPath = "Auth/Transactions/rdlc/rptDisbursementVoucher.rdlc"
    '    rptDVViewer.LocalReport.DataSources.Clear()
    '    rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dsDv))
    '    rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", dvParticlarEntry))
    '    rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet3", dvAccountEntry))
    '    rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet4", dvTaxEntry))
    '    'rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet5", dvSubAccount))
    '    rptDVViewer.LocalReport.Refresh()
    'End Sub

    Private Sub grdDVList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdDVList.Sorting
        Dim currentSortDirection = getSortDirection(e.SortExpression)

        Select Case e.SortExpression
            Case "DisbursementVoucherNo"
                If currentSortDirection = "ASC" Then
                    DVQueue = (From p In DVQueue Order By p.DisbursementVoucherNo Ascending Select p).ToList
                Else
                    DVQueue = (From p In DVQueue Order By p.DisbursementVoucherNo Descending Select p).ToList
                End If
            Case "PayeeName"
                If currentSortDirection = "ASC" Then
                    DVQueue = (From p In DVQueue Order By p.PayeeName Ascending Select p).ToList
                Else
                    DVQueue = (From p In DVQueue Order By p.PayeeName Descending Select p).ToList
                End If
            Case "DateCreated"
                If currentSortDirection = "ASC" Then
                    DVQueue = (From p In DVQueue Order By p.DateCreated Ascending Select p).ToList
                Else
                    DVQueue = (From p In DVQueue Order By p.DateCreated Descending Select p).ToList
                End If
            Case "ParticularsAmountDue"
                If currentSortDirection = "ASC" Then
                    DVQueue = (From p In DVQueue Order By p.ParticularsAmountDue Ascending Select p).ToList
                Else
                    DVQueue = (From p In DVQueue Order By p.ParticularsAmountDue Descending Select p).ToList
                End If
        End Select

        grdDVList.DataSource = DVQueue
        grdDVList.DataBind()
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

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        txtsearch.Focus()
        If Not String.IsNullOrEmpty(txtsearch.Text) And Not String.IsNullOrWhiteSpace(txtsearch.Text) Then
            IsSearch = True
            DVPayeeSearchList = dvdll.Master.DisbursementVoucher.getDVPayeeByFilter(txtsearch.Text)
        Else
            IsSearch = False
            DVPayeeSearchList = New List(Of dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord)
        End If
        LoadGridDV()
    End Sub

    Sub ApproveDV(DVRec As Guid)
        Try
            dvdll.Master.DisbursementVoucher.UpdateDVStatusToApprove(DVRec, dvdll.Master.DisbursementVoucher.DVStatus.Approved, CDate(txtApprovalDate.Text))

            SelectedObRID = dvdll.Master.DisbursementVoucher.getDVByID(DVRec).ObligationRequestID
            dvdll.Master.ObligationRequest.UpdateOBStatus(SelectedObRID, dvdll.Master.ObligationRequest.ObRStatus.DVApproved)

        Catch ex As Exception
            'ShowInfoPopUp("Obligation Request Approved", "Items on Queue for Disbursement Voucher")
        End Try
    End Sub

    Private Sub btnApproveDV_Click(sender As Object, e As EventArgs) Handles btnApproveDV.Click
        ApproveDV(SelectedDVID)
        LoadGridDV()
    End Sub
End Class

