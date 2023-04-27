Public Class Division_Library
    Inherits System.Web.UI.Page

    Property DivisionList As List(Of dvdll.Master.Division.DivisionRecord)
        Get
            Return ViewState("DivisionList")
        End Get
        Set(value As List(Of dvdll.Master.Division.DivisionRecord))
            ViewState("DivisionList") = value
        End Set
    End Property

    Property SelectedDivisionID As Guid
        Get
            Return ViewState("SelectedDivisionID")
        End Get
        Set(value As Guid)
            ViewState("SelectedDivisionID") = value
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


    Sub LoadGrid()
        Dim lst = dvdll.Master.Division.getDivisionShowAll
        grddivision.DataSource = lst
        grddivision.DataBind()
    End Sub

    Sub LoadFundGrid()
        Dim lst = dvdll.Master.Division.getFundSourceByDivision(SelectedDivisionID)
        grdFundSources.DataSource = lst
        grdFundSources.DataBind()
    End Sub

    Sub ClearFields()
        txtdivisioname.Text = ""
        txtdivisiondesc.Text = ""
        SelectedDivisionID = Nothing
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Expires = 0
        Response.Cache.SetNoStore()
        Response.AppendHeader("Pragma", "no-cache")
        'If Not SysTool.AuthPage(wsbl.webMenuID.Web_SalesInvoiceAdd, Session, Response) Then
        '    Exit Sub
        'End If

        If Not IsPostBack Then
            txtSearch.Focus()
            txtSearch.Text = ""
            ClearFields()
            LoadGrid()
            'LoadFundGrid()
        End If
        pnlFundSourcePanel.Visible = False
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        lblAddError.Text = ""
        Try
            If SelectedDivisionID = Nothing Then
                dvdll.Master.Division.addNewDivision(txtdivisioname.Text, txtdivisiondesc.Text)
                ClearFields()
                mpeDivision.Hide()
                LoadGrid()
                grddivision.PageIndex = 0

            Else
                dvdll.Master.Division.updateDivisionID(SelectedDivisionID, txtdivisioname.Text, txtdivisiondesc.Text)
                ClearFields()
                mpeDivision.Hide()
                LoadGrid()
            End If
        Catch ex As Exception
            mpeDivision.Show()
            lblAddError.Text = ex.Message
        End Try
    End Sub

    Private Sub btnAddFund_Click(sender As Object, e As EventArgs) Handles btnAddFund.Click
        Try
            dvdll.Master.Division.addNewFundSource(txtFundSourceName.Text, txtFundAmount.Text, SelectedDivisionID)
        Catch ex As Exception

        End Try
        LoadFundGrid()
        pnlFundSourcePanel.Visible = False
    End Sub

    Private Sub grddivision_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grddivision.PageIndexChanging
        grddivision.PageIndex = e.NewPageIndex
        LoadGrid()
    End Sub

    Private Sub grddivision_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grddivision.RowCommand
        If e.CommandName = "xEdit" Then
            Dim rec = dvdll.Master.Division.getDivisionByID(Guid.Parse(e.CommandArgument))
            If Not rec Is Nothing Then
                mpeDivision.Show()
                With rec
                    txtdivisioname.Text = .Division
                    txtdivisiondesc.Text = .DivisionDesc
                End With
                SelectedDivisionID = rec.ID
            End If

            'Dim lst = dvdll.Master.Division.getFundSourceByDivision(SelectedDivisionID)
            'grdFundSources.DataSource = lst
            'grdFundSources.DataBind()
            LoadFundGrid()
            pnlFundSourcePanel.Visible = True


        ElseIf e.CommandName = "xDelete" Then
            dvdll.Master.Division.deleteDivisionByID(Guid.Parse(e.CommandArgument))
            ClearFields()
            mpeDivision.Hide()
            LoadGrid()
        ElseIf e.CommandName = "xActivate" Then
        dvdll.Master.Division.activateDivisionByID(Guid.Parse(e.CommandArgument))
        ClearFields()
        mpeDivision.Hide()
        LoadGrid()
        ElseIf e.CommandName = "xDeactivate" Then
        dvdll.Master.Division.deactivateDivisionByID(Guid.Parse(e.CommandArgument))
        ClearFields()
        mpeDivision.Hide()
        LoadGrid()
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ClearFields()
        pnlFundSourcePanel.Visible = False
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlFundSourcePanel.Visible = False
    End Sub

    Private Sub grdFundSources_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdFundSources.RowCommand
        If e.CommandName = "xEditFund" Then
            Dim rec = dvdll.Master.Division.getFundSourcebyID(Guid.Parse(e.CommandArgument))
            If Not rec Is Nothing Then
                mpeFundSources.Show()
                pnlAddFundSources.Visible = True
                With rec
                    txtFundSourceName.Text = .FundName
                    txtFundAmount.Text = .Amount
                End With
            End If

        ElseIf e.CommandName = "xDeleteFund" Then
            dvdll.Master.Division.deleteFundSourceByID(Guid.Parse(e.CommandArgument))
            Dim lst = dvdll.Master.Division.getFundSourceByDivision(SelectedDivisionID)
            grdFundSources.DataSource = lst

            grdFundSources.DataBind()
            pnlFundSourcePanel.Visible = True
        End If
    End Sub

#Region "Grid Sorting"

    Private Sub grddivision_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grddivision.Sorting
        Dim currentSortDirection = getSortDirection(e.SortExpression)
        Dim DivisionList = dvdll.Master.Division.getDivisionShowAll
        Select Case e.SortExpression
            Case "Division"
                If currentSortDirection = "ASC" Then
                    DivisionList = (From p In DivisionList Order By p.Division Ascending Select p).ToList
                Else
                    DivisionList = (From p In DivisionList Order By p.Division Descending Select p).ToList
                End If
            Case ""
        End Select
        grddivision.DataSource = DivisionList
        grddivision.DataBind()
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
End Class