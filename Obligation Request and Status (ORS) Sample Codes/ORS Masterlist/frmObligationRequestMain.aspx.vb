Imports Microsoft.Reporting.WebForms

Public Class frmObligationRequestMain
    Inherits System.Web.UI.Page

#Region "Properties"



    Property ObrPayeeSearchList As List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord)
        Get
            Return ViewState("ObrPayeeSearchList")
        End Get
        Set(value As List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord))
            ViewState("ObrPayeeSearchList") = value
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

    Property ObRList As List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord)
        Get
            Return ViewState("ObRList")
        End Get
        Set(value As List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord))
            ViewState("ObRList") = value
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

    Property UserSession As dvdll.Master.User
        Get
            Return Session("UserSession")
        End Get
        Set(value As dvdll.Master.User)
            Session("UserSession") = value
        End Set
    End Property


    Property PrioObrNo As String
        Get
            Return ViewState("PrioObrNo")
        End Get
        Set(value As String)
            ViewState("PrioObrNo") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            IsSearch = False
            pnlPrintObR.Visible = False
            pnlObRForm.Visible = False
            LoadGrid()
            Title = "Obligation Requests"
            txtCancelDate.Text = Now.ToShortDateString
            mpeCancelDate.Hide()
        End If
        dUser = System.Web.Security.Membership.GetUser.ToString
    End Sub

#Region "Control Events"
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        pnlMain.Visible = True
        pnlPrintObR.Visible = False        
    End Sub
    Private Sub grdObR_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdObR.PageIndexChanging
        grdObR.PageIndex = e.NewPageIndex
        LoadGrid()
        grdObR.Sort("DateCreated", SortDirection.Descending)
    End Sub
    Private Sub grdObR_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdObR.RowCommand
        If e.CommandName = "xEdit" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ObligationRequest.getObligationRequestByID(id)
            If Not rec Is Nothing Then
                'If rec.Status = dvdll.Master.ObligationRequest.ObRStatus.OnQueueEdit Then
                '    ShowInfoPopUp("INFO", "Obligation Request currently being edited by " + rec.EditorName + ".")
                '    Exit Sub
                'End If
                pnlMain.Visible = False
                pnlPrintObR.Visible = False
                pnlObRForm.Visible = True
                ucObReqForm.ClearObRForm(True)
                Title = "Obligation Request"
                ucObReqForm.InitializeFormForUpdate(id)
            End If
        ElseIf e.CommandName = "xVerify" Then
            Dim id = Guid.Parse(e.CommandArgument)

            Try
                Dim rec = dvdll.Master.ObligationRequest.getObligationRequestByID(id)
                dvdll.Master.ObligationRequest.UpdateOBStatus(id, dvdll.Master.ObligationRequest.ObRStatus.Verified)
                ShowInfoPopUp(" Obligation Request Approved", "Obligation Request No: " + dvdll.Master.ObligationRequest.getObligationRequestByID(id).ObligationRequestNo + vbNewLine + "is on queue for Disbursement Voucher Creation")
            Catch ex As Exception
                ShowInfoPopUp("INFO", ex.Message)
            End Try

            LoadGrid()
        ElseIf e.CommandName = "xCancel" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ObligationRequest.getObligationRequestByID(id)
            If Not rec Is Nothing Then
                SelectedObRID = rec.ID
            End If
            mpeCancelDate.Show()

            'Dim id = Guid.Parse(e.CommandArgument)
            'Try
            '    Dim rec = dvdll.Master.ObligationRequest.getObligationRequestByID(id)
            '    dvdll.Master.ObligationRequest.UpdateOBStatus(id, dvdll.Master.ObligationRequest.ObRStatus.Cancelled)
            '    ' ShowInfoPopUp("Obligation Request Cancelled", "Item is Sent to Cancelled Obligation Requests")
            'Catch ex As Exception
            '    ShowInfoPopUp("INFO", ex.Message)
            'End Try
            'LoadGrid()

        ElseIf e.CommandName = "xViewSource" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ObligationRequest.getObligationRequestByID(id)
            If Not rec Is Nothing Then
                ShowInfoPopUp("Source Documents", rec.SourceDocument)
            End If
        ElseIf e.CommandName = "xViewParticular" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ObligationRequest.getObligationRequestByID(id)
            If Not rec Is Nothing Then
                ShowInfoPopUp("Particular Details", rec.ParticularTemplate)
            End If
        ElseIf e.CommandName = "xPrint" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ObligationRequest.getObligationRequestByID(id)
            If Not rec Is Nothing Then
                pnlMain.Visible = False
                pnlPrintObR.Visible = True
                LoadObRToPrint(rec)
            End If
        End If
    End Sub
    Private Sub btnCreateObR_Click(sender As Object, e As EventArgs) Handles btnCreateObR.Click
        ucObReqForm.ClearObRForm(False)
        pnlMain.Visible = False
        pnlObRForm.Visible = True
        mpeCancelDate.Hide()
    End Sub
#End Region

#Region "Methods"
    Sub ShowInfoPopUp(header As String, message As String)
        lblInfoPopHeader.Text = header
        lblInfoPopMsg.Text = message
        mpeInfoPop.Show()
    End Sub
    Sub LoadGrid()
        If IsSearch Then
            ObRList = dvdll.Master.ObligationRequest.getObligationRequestsByPayee(txtSearch.Text, dvdll.Master.ObligationRequest.ObRStatus.OnQueue)
        Else
            ObRList = dvdll.Master.ObligationRequest.getObligationRequestsByStatus(dvdll.Master.ObligationRequest.ObRStatus.OnQueue)
        End If
        grdObR.DataSource = ObRList
        grdObR.DataBind()
        grdObR.Sort("DateCreated", SortDirection.Descending)

    End Sub
    Sub LoadObRToPrint(obrRec As dvdll.or_ObligationRequest)
        Dim dsObR = New List(Of dvdll.or_ObligationRequest)
        dsObR.Add(obrRec)

        Dim dsObRParticulars = dvdll.Master.ObligationRequest.getORParticularsByORID(obrRec.ID)
        rptViewer.Visible = True
        rptViewer.Enabled = True
        rptViewer.Reset()
        rptViewer.LocalReport.ReportPath = "Auth/Transactions/rdlc/rptObligationRequest.rdlc"
        rptViewer.LocalReport.DataSources.Clear()
        rptViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dsObR))
        rptViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", dsObRParticulars))
        rptViewer.LocalReport.Refresh()
    End Sub
#End Region

#Region "Obligation Request Events"
    Private Sub ucObReqForm_ObRFormBack() Handles ucObReqForm.ObRFormBack
        pnlMain.Visible = True
        pnlObRForm.Visible = False
        pnlPrintObR.Visible = False
        Title = "Obligation Requests"
    End Sub
    Private Sub ucObReqForm_ObRFormSave() Handles ucObReqForm.ObRFormSave
        pnlMain.Visible = True
        pnlObRForm.Visible = False
        pnlPrintObR.Visible = False
        Title = "Obligation Requests"
        LoadGrid()
        grdObR.Sort("DateCreated", SortDirection.Descending)
    End Sub
#End Region

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        txtSearch.Focus()
        If Not String.IsNullOrEmpty(txtSearch.Text) And Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
            IsSearch = True
            ObrPayeeSearchList = dvdll.Master.ObligationRequest.getObRPayeeByFilter(txtSearch.Text)
        Else
            IsSearch = False
            ObrPayeeSearchList = New List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord)
        End If
        LoadGrid()
        grdObR.Sort("DateCreated", SortDirection.Descending)
    End Sub

    Private Sub grdObR_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdObR.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each item In ObRList
                If item.isPrio = True Then
                    PrioObrNo = item.ObligationRequestNo
                    If Convert.ToString(e.Row.Cells(1).Text) = PrioObrNo Then
                        e.Row.BackColor = Drawing.Color.Aquamarine
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub grdObR_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdObR.Sorting

        Dim currentSortDirection = getSortDirection(e.SortExpression)

        Select Case e.SortExpression
            Case "ObligationRequestNo"
                If currentSortDirection = "ASC" Then
                    ObRList = (From p In ObRList Order By p.ObligationRequestNo Ascending Select p).ToList
                Else
                    ObRList = (From p In ObRList Order By p.ObligationRequestNo Descending Select p).ToList
                End If
            Case "PayeeName"
                If currentSortDirection = "ASC" Then
                    ObRList = (From p In ObRList Order By p.PayeeName Ascending Select p).ToList
                Else
                    ObRList = (From p In ObRList Order By p.PayeeName Descending Select p).ToList
                End If
            Case "DateCreated"
                If currentSortDirection = "ASC" Then
                    ObRList = (From p In ObRList Order By p.DateCreated Ascending Select p).ToList
                Else
                    ObRList = (From p In ObRList Order By p.DateCreated Descending Select p).ToList
                End If
                'Case "DueDate"
                '    If currentSortDirection = "ASC" Then
                '        ObRList = (From p In ObRList Order By p.DueDate Ascending Select p).ToList
                '    Else
                '        ObRList = (From p In ObRList Order By p.DueDate Descending Select p).ToList
                '    End If
            Case "TotalAmount"
                If currentSortDirection = "ASC" Then
                    ObRList = (From p In ObRList Order By p.TotalAmount Ascending Select p).ToList
                Else
                    ObRList = (From p In ObRList Order By p.TotalAmount Descending Select p).ToList
                End If
        End Select

        grdObR.DataSource = ObRList
        grdObR.DataBind()
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

    Private Sub btnCancelObligationRequest_Click(sender As Object, e As EventArgs) Handles btnCancelObligationRequest.Click

        Try
            Dim rec = dvdll.Master.ObligationRequest.getObligationRequestByID(SelectedObRID)
            dvdll.Master.ObligationRequest.UpdateOBStatus(SelectedObRID, dvdll.Master.ObligationRequest.ObRStatus.Cancelled)
            dvdll.Master.ObligationRequest.CancelDate(SelectedObRID, CDate(txtCancelDate.Text), dUser)
            ' ShowInfoPopUp("Obligation Request Cancelled", "Item is Sent to Cancelled Obligation Requests")
        Catch ex As Exception
            ShowInfoPopUp("INFO", ex.Message)
        End Try
        LoadGrid()
    End Sub

End Class
