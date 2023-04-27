Imports dvdllMaster = dvdll.Master

Public Class UACSCode_Library
    Inherits System.Web.UI.Page
    ' FOR ACCOUNT ENTRY (OLD)
    Property AccountCodeList As List(Of dvdll.Master.Account.AccountEntryRecord)
        Get
            Return ViewState("AccountCodeList")
        End Get
        Set(value As List(Of dvdll.Master.Account.AccountEntryRecord))
            ViewState("AccountCodeList") = value
        End Set
    End Property
    ' FOR UACS CODE (OLD)
    Property UACSCodeList As List(Of dvdll.Master.UACSCode.UACSCodeRecord)
        Get
            Return ViewState("UACSCodeList")
        End Get
        Set(value As List(Of dvdll.Master.UACSCode.UACSCodeRecord))
            ViewState("UACSCodeList") = value
        End Set
    End Property
    ' FOR ACCT ENTRY (OLD)
    Property SelectedAcctID As Guid
        Get
            Return ViewState("SelectedAcctID")
        End Get
        Set(value As Guid)
            ViewState("SelectedAcctID") = value
        End Set
    End Property

    Property SelectedUACSCodeID As Guid
        Get
            Return ViewState("SelectedUACSCodeID")
        End Get
        Set(value As Guid)
            ViewState("SelectedUACSCodeID") = value
        End Set
    End Property

    'Property AcctSearchList As List(Of dvdll.lib_Account)
    '    Get
    '        Return ViewState("AcctSearchList")
    '    End Get
    '    Set(value As List(Of dvdll.lib_Account))
    '        ViewState("AcctSearchList") = value
    '    End Set
    'End Property
    Property AcctSearchList As List(Of dvdll.Master.Account.AccountEntryRecord)
        Get
            Return ViewState("AcctSearchList")
        End Get
        Set(value As List(Of dvdll.Master.Account.AccountEntryRecord))
            ViewState("AcctSearchList") = value
        End Set
    End Property
    Property isSearch As Nullable(Of Boolean)
        Get
            Return ViewState("isSearch")
        End Get
        Set(value As Nullable(Of Boolean))
            ViewState("isSearch") = value
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
        If Not IsPostBack Then
            ClearForm()
            LoadSpecialTypes()
            LoadGrid()
            txtSearch.Focus()
        End If
    End Sub

#Region "Control Events"
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ClearForm()
        mpeUACS.Show()
        'txtAcctCode.Focus()
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        lblAddError.Text = ""
        'Try
        '    If SelectedAcctID = Nothing Then
        '        dvdll.Master.Account.AddAccount(txtAcctCode.Text, txtAcctDesc.Text, ddlType.SelectedValue)
        '        ClearForm()
        '        mpeUACS.Hide()
        '        LoadGrid()
        '        grdSrcCodes.PageIndex = 0
        '    Else
        '        dvdll.Master.Account.UpdateAccount(SelectedAcctID, txtAcctCode.Text, txtAcctDesc.Text, ddlType.SelectedValue)
        '        ClearForm()
        '        mpeUACS.Hide()
        '        LoadGrid()
        '    End If
        'Catch ex As Exception
        '    mpeUACS.Show()
        '    lblAddError.Text = ex.Message
        'End Try

        Try
            If SelectedUACSCodeID = Nothing Then
                dvdll.Master.UACSCode.AddUACSCode(txtUACSCode.Text, txtUACSCodeDesc.Text)
                ClearForm()
                mpeUACS.Hide()
                LoadGrid()
                grdSrcCodes.PageIndex = 0
            Else
                dvdll.Master.UACSCode.UpdateUACSCode(SelectedUACSCodeID, txtUACSCode.Text, txtUACSCodeDesc.Text)
                ClearForm()
                mpeUACS.Hide()
                LoadGrid()
            End If
        Catch ex As Exception
            mpeUACS.Show()
            lblAddError.Text = ex.Message
        End Try
    End Sub
    Private Sub grdSrcCodes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdSrcCodes.PageIndexChanging
        grdSrcCodes.PageIndex = e.NewPageIndex
        LoadGrid()

    End Sub

    Private Sub grdSrcCodes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdSrcCodes.RowCommand
        If e.CommandName = "xEdit" Then
            Dim rec = dvdll.Master.UACSCode.getUACSCodeByID(Guid.Parse(e.CommandArgument))
            If Not rec Is Nothing Then
                mpeUACS.Show()
                With rec
                    txtUACSCode.Text = rec.UACSCode
                    txtUACSCodeDesc.Text = rec.UACSDescription
                End With
                SelectedUACSCodeID = rec.ID
                txtUACSCode.Focus()
            End If
        ElseIf e.CommandName = "xActivate" Then
            dvdll.Master.UACSCode.ActivateUACSByID(Guid.Parse(e.CommandArgument))
            ClearForm()
            mpeUACS.Hide()
            LoadGrid()
        ElseIf e.CommandName = "xDeactivate" Then
            dvdll.Master.UACSCode.DeactivateUACSByID(Guid.Parse(e.CommandArgument))
            ClearForm()
            mpeUACS.Hide()
            LoadGrid()
        End If
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'txtSearch.Focus()
        'If Not String.IsNullOrEmpty(txtSearch.Text) And Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
        '    AcctSearchList = dvdll.Master.Account.getAccountsByFilter(txtSearch.Text)
        'Else
        '    AcctSearchList = New List(Of dvdll.lib_Account)
        'End If
        'LoadGrid()
        txtSearch.Focus()

        If Not String.IsNullOrEmpty(txtSearch.Text) And Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
            isSearch = True
            AcctSearchList = dvdll.Master.Account.getAccountsByFilter(txtSearch.Text)
        Else
            isSearch = False
            AcctSearchList = New List(Of dvdll.Master.Account.AccountEntryRecord)
        End If
        LoadGrid()
    End Sub

#End Region

#Region "Methods"
    ' EDITED BY CAMERON
    ' GETS DATA FROM MASTER_UACSCODE
    Sub LoadGrid()
        'Dim AccountCodeList = dvdll.Master.Account.getAccounts
        'grdSrcCodes.DataSource = AccountCodeList
        'grdSrcCodes.DataBind()

        'Dim AccountCodeList = dvdll.Master.Account.getAccounts

        If isSearch = True Then
            ' CREATE A VERSION OF THIS FOR UACS CODE (WITH FILTER)
            'AccountCodeList = dvdll.Master.Account.getAccountsByFilter(txtSearch.Text)

            ' UACS CODE (WITH FILTER)
            UACSCodeList = dvdll.Master.UACSCode.getUACSCodeByFilter(txtSearch.Text)
        Else
            ' CREATE A VERSION OF THIS FOR UACS CODE (ALL RECORDS)
            'AccountCodeList = dvdll.Master.Account.getAccountEntryRecord

            ' UACS CODE (ALL RECORDS)
            UACSCodeList = dvdll.Master.UACSCode.getAllUACSSourceCodes
        End If
        ' DISPLAYS ACCOUNT CODE LIST TO GRIDVIEW
        'grdSrcCodes.DataSource = AccountCodeList

        ' DISPLAYS UACS SOURCE CODES LIST TO GRIDVIEW
        grdSrcCodes.DataSource = UACSCodeList
        grdSrcCodes.DataBind()
    End Sub

    Sub LoadSpecialTypes()
        Dim lst As New List(Of ListItem)
        lst.Add(New ListItem With {.Value = 0, .Text = "[-N/A-]"})
        lst.Add(New ListItem With {.Value = dvdllMaster.Account.AccountSpecialType.Travel, .Text = "Travel"})
        'ddlType.DataSource = lst
        'ddlType.DataBind()
    End Sub
    Sub ClearForm()
        'txtAcctDesc.Text = ""
        txtSearch.Text = ""
        'txtAcctCode.Text = ""
        'ddlType.SelectedValue = 0
        SelectedAcctID = Nothing
    End Sub

#End Region

#Region "Sorting"

    Private Sub grdSrcCodes_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdSrcCodes.Sorting
        Dim currentSortDirection = getSortDirection(e.SortExpression)
        Dim AccountCodeList = dvdll.Master.Account.getAccounts
        Select Case e.SortExpression
            Case "AccountDescription"
                If currentSortDirection = "ASC" Then
                    AccountCodeList = (From p In AccountCodeList Order By p.AccountDescription Ascending Select p).ToList
                Else
                    AccountCodeList = (From p In AccountCodeList Order By p.AccountDescription Descending Select p).ToList
                End If
        End Select

        grdSrcCodes.DataSource = AccountCodeList
        grdSrcCodes.DataBind()

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