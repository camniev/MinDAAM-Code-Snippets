Public Class Signatories_Library
    Inherits System.Web.UI.Page

    Property SignatoryList As List(Of dvdll.Master.Signatories.SignatoryRecord)
        Get
            Return ViewState("SignatoryList")
        End Get
        Set(value As List(Of dvdll.Master.Signatories.SignatoryRecord))
            ViewState("SignatoryList") = value
        End Set
    End Property


    Property SelectedSigID As Guid
        Get
            Return ViewState("SelectedSigID")
        End Get
        Set(value As Guid)
            ViewState("SelectedSigID") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Expires = 0
        Response.Cache.SetNoStore()
        Response.AppendHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            LoadGrid()
        End If

    End Sub

    Sub LoadGrid()
        SignatoryList = dvdll.Master.Signatories.getSignatory
        grdsignatories.DataSource = SignatoryList
        grdsignatories.DataBind()
    End Sub

    Sub ClearFields()
        txtSigName.Text = ""
        txtSigPosition.Text = ""
    End Sub


    Private Sub btnAddSignatoryItem_Click(sender As Object, e As EventArgs) Handles btnAddSignatoryItem.Click
        Try
            dvdll.Master.Signatories.addSignatory(SelectedSigID, txtSigName.Text, txtSigPosition.Text)
        Catch ex As Exception

        End Try
        ClearFields()
        LoadGrid()
        mpeAddSig.Hide()
    End Sub

    Private Sub grdsignatories_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdsignatories.PageIndexChanging
        grdsignatories.PageIndex = e.NewPageIndex
        LoadGrid()
    End Sub


    Private Sub grdsignatories_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdsignatories.RowCommand
        If e.CommandName = "xEdit" Then
            Dim rec = dvdll.Master.Signatories.getSigByID(Guid.Parse(e.CommandArgument))
            If Not rec Is Nothing Then
                mpeAddSig.Show()
                With rec
                    txtSigName.Text = .Name
                    txtSigPosition.Text = .Position
                End With
                SelectedSigID = rec.ID
            End If

            btnAddSignatoryItem.Visible = False
            btnUpdateSignatoryItem.Visible = True
        ElseIf e.CommandName = "xShow" Then
            dvdll.Master.Signatories.activateSigByID(Guid.Parse(e.CommandArgument))
            ClearFields()
            LoadGrid()
        ElseIf e.CommandName = "xDelete" Then
            dvdll.Master.Signatories.deleteSigByID(Guid.Parse(e.CommandArgument))
            ClearFields()
            LoadGrid()
        End If



    End Sub

    Private Sub btnUpdateSignatoryItem_Click(sender As Object, e As EventArgs) Handles btnUpdateSignatoryItem.Click
        dvdll.Master.Signatories.updateSignatory(SelectedSigID, txtSigName.Text, txtSigPosition.Text)
        ClearFields()
        mpeAddSig.Hide()
        LoadGrid()
    End Sub


#Region "Grid Sorting"

    Private Sub grdsignatories_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdsignatories.Sorting
        Dim currentSortDirection = getSortDirection(e.SortExpression)

        Select e.SortExpression
            Case "Name"
                If currentSortDirection = "ASC" Then
                    SignatoryList = (From p In SignatoryList Order By p.Name Ascending Select p).ToList
                Else
                    SignatoryList = (From p In SignatoryList Order By p.Name Descending Select p).ToList
                End If
        End Select
        grdsignatories.DataSource = SignatoryList
        grdsignatories.DataBind()
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