Public Class Tax_Library
    Inherits System.Web.UI.Page

    Property SelectedTaxID As Guid
        Get
            Return ViewState("SelectedTaxID")
        End Get
        Set(value As Guid)
            ViewState("SelectedTaxID") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Expires = 0
        Response.Cache.SetNoStore()
        Response.AppendHeader("Pragma", "no-cache")
        'If Not SysTool.AuthPage(wsbl.webMenuID.Web_SalesInvoiceAdd, Session, Response) Then
        '    Exit Sub
        'End If

        If Not IsPostBack Then
            txtSearch.Text = ""
            LoadGrid()
            grdtaxlist.Sort("TaxType", SortDirection.Ascending)
            txtSearch.Focus()
        End If
    End Sub

    Sub LoadGrid()
            Dim lst = dvdll.Master.Tax.getTax
        grdtaxlist.DataSource = lst
        grdtaxlist.DataBind()
    End Sub
    Sub ClearFields()
        txttaxdesc.Text = ""
        txtTaxATC.Text = ""
        txtTaxLongDescription.Text = ""
        txttaxperc.Text = ""
        txtTaxShortDesc.Text = ""
        rdBtnTaxType.SelectedValue = -1
        SelectedTaxID = Nothing
    End Sub
    Function IsValidSave()
        If String.IsNullOrEmpty(txttaxdesc.Text) Or String.IsNullOrWhiteSpace(txttaxdesc.Text) Then
            ucInfoWindow.SetHeader("INFO")
            ucInfoWindow.SetMessage("Please supply Tax Description")
            ucInfoWindow.ShowInfo()
            txttaxdesc.Focus()
            Return False
        End If
        If String.IsNullOrEmpty(txtTaxShortDesc.Text) Or String.IsNullOrWhiteSpace(txtTaxShortDesc.Text) Then
            ucInfoWindow.SetHeader("INFO")
            ucInfoWindow.SetMessage("Please supply Tax Short Description")
            ucInfoWindow.ShowInfo()
            txtTaxShortDesc.Focus()
            Return False
        End If
        If String.IsNullOrEmpty(txttaxperc.Text) Or String.IsNullOrWhiteSpace(txttaxperc.Text) Then
            ucInfoWindow.SetHeader("INFO")
            ucInfoWindow.SetMessage("Please supply Tax Percentage")
            ucInfoWindow.ShowInfo()
            txtTaxShortDesc.Focus()
            Return False
        Else
            Try
                Dim test = CDec(txttaxperc.Text)
            Catch ex As Exception
                ucInfoWindow.SetHeader("INFO")
                ucInfoWindow.SetMessage("Please supply a valid Tax Percentage")
                ucInfoWindow.ShowInfo()
                txtTaxShortDesc.Focus()
            End Try
        End If
        If rdBtnTaxType.SelectedValue < 1 Then
            ucInfoWindow.SetHeader("INFO")
            ucInfoWindow.SetMessage("Please select Tax Type")
            ucInfoWindow.ShowInfo()
            rdBtnTaxType.Focus()
            Return False
        End If
        Return True
    End Function

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        lblAddError.Text = ""
        Try
            If IsValidSave() Then
                If SelectedTaxID = Nothing Then
                    dvdll.Master.Tax.addNewTax(txttaxdesc.Text, txttaxperc.Text, txtTaxShortDesc.Text, rdBtnTaxType.SelectedValue, txtTaxATC.Text, txtTaxLongDescription.Text)
                    ClearFields()
                    mpeTax.Hide()
                    LoadGrid()
                    grdtaxlist.Sort("TaxType", SortDirection.Ascending)
                    grdtaxlist.PageIndex = 0
                Else
                    dvdll.Master.Tax.updateTaxByID(SelectedTaxID, txttaxdesc.Text, txttaxperc.Text, txtTaxShortDesc.Text, rdBtnTaxType.SelectedValue, txtTaxATC.Text, txtTaxLongDescription.Text)
                    ClearFields()
                    mpeTax.Hide()
                    LoadGrid()
                    grdtaxlist.Sort("TaxType", SortDirection.Ascending)
                End If
            Else
                mpeTax.Show()
            End If
        Catch ex As Exception
            mpeTax.Show()
            lblAddError.Text = ex.Message
        End Try
    End Sub

    Private Sub grdtaxlist_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdtaxlist.PageIndexChanging
        grdtaxlist.PageIndex = e.NewPageIndex
        LoadGrid()
        grdtaxlist.Sort("TaxType", SortDirection.Ascending)
    End Sub
    Private Sub grdtaxlist_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdtaxlist.RowCommand
        If e.CommandName = "xEdit" Then
            Dim rec = dvdll.Master.Tax.getTaxbyID(Guid.Parse(e.CommandArgument))
            If Not rec Is Nothing Then
                mpeTax.Show()
                With rec
                    txtTaxATC.Text = .TaxATC
                    txtTaxLongDescription.Text = .TaxLongDescription
                    txttaxdesc.Text = .TaxDescription
                    txttaxperc.Text = CInt(.TaxPercentage).ToString("#,##0")
                    txtTaxShortDesc.Text = .TaxShortDesc
                    rdBtnTaxType.SelectedValue = .TaxType
                End With
                SelectedTaxID = rec.ID
            End If
        ElseIf e.CommandName = "xShow" Then
            dvdll.Master.Tax.activateTaxByID(Guid.Parse(e.CommandArgument))
            ClearFields()
            mpeTax.Hide()
            LoadGrid()
            grdtaxlist.Sort("TaxType", SortDirection.Ascending)
        ElseIf e.CommandName = "xDelete" Then
            dvdll.Master.Tax.deleteTaxByID(Guid.Parse(e.CommandArgument))
            ClearFields()
            mpeTax.Hide()
            LoadGrid()
            grdtaxlist.Sort("TaxType", SortDirection.Ascending)
        End If
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ClearFields()
        mpeTax.Show()
        'txttaxdesc.Focus()
        txtTaxATC.Focus()
    End Sub
    Private Sub grdtaxlist_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdtaxlist.Sorting
        Dim lst = dvdll.Master.Tax.getTax
        Dim currentSortDirection = getSortDirection(e.SortExpression)

        Select Case e.SortExpression
            Case "TaxType"
                If currentSortDirection = "ASC" Then
                    lst = (From p In lst Order By p.TaxType Ascending Select p).ToList
                Else
                    lst = (From p In lst Order By p.TaxType Ascending Select p).ToList
                End If
        End Select


        grdtaxlist.DataSource = lst
        grdtaxlist.DataBind()
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
End Class