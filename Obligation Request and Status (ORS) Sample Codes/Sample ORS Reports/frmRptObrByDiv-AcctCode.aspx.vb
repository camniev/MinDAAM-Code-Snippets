Imports Microsoft.Reporting.WebForms

Public Class frmRptObrByDiv_AcctCode
    Inherits System.Web.UI.Page


    Property SelectedDivisionID As Guid
        Get
            Return ViewState("SelectedDivisionID")
        End Get
        Set(value As Guid)
            ViewState("SelectedDivisionID") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDateFrom.Text = Now.ToShortDateString
            txtDateTo.Text = Now.ToShortDateString
            LoadDivision()
            LoadAccountCodes()
        End If

    End Sub

    Sub LoadDivision()
        ddlDivision.Items.Add(New ListItem With {.Text = "[--Select--]", .Value = -1})
        ddlDivision.DataSource = dvdll.Master.Division.getDivision
        ddlDivision.DataBind()
    End Sub

    Sub LoadAccountCodes()
        ddlAccountCode.Items.Add(New ListItem With {.Text = "[--Select--]", .Value = -1})
        ddlAccountCode.DataSource = dvdll.Master.Account.getAccountEntryRecordOrderByName
        ddlAccountCode.DataBind()
    End Sub

    Private Sub ddlAccountCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAccountCode.SelectedIndexChanged
        SelectedAccountCodeID = Guid.Parse(ddlAccountCode.SelectedItem.Value)
    End Sub

    Private Sub ddlDivision_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDivision.SelectedIndexChanged
        SelectedDivisionID = Guid.Parse(ddlDivision.SelectedItem.Value)
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        If chkAllRespoCenter.Checked Then
            If chkAllAcounts.Checked = True Then
                grdReport.DataSource = dvdll.Master.ObligationRequest.GetParticularsByAllDivisionAllAccounts(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue)
                grdReport.DataBind()
                rptViewReport.Visible = False
                grdReport.Visible = True
            Else
                grdReport.DataSource = dvdll.Master.ObligationRequest.GetParticularsByAllDivisionByAccountCode(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, SelectedAccountCodeID)
                grdReport.DataBind()
                rptViewReport.Visible = False
                grdReport.Visible = True
            End If
        Else
            If chkAllAcounts.Checked = True Then
                grdReport.DataSource = dvdll.Master.ObligationRequest.GetParticularsByDivisionAllAccounts(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, SelectedDivisionID)
                grdReport.DataBind()
                rptViewReport.Visible = False
                grdReport.Visible = True
            Else
                grdReport.DataSource = dvdll.Master.ObligationRequest.GetParticularsByDivisionByAccountCode(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, SelectedDivisionID, SelectedAccountCodeID)
                grdReport.DataBind()
                rptViewReport.Visible = False
                grdReport.Visible = True
            End If
        End If
    End Sub
    Sub LoadToPrint()
        Dim dsObR = dvdll.Master.ObligationRequest.GetParticularsByDivisionByAccountCode(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, SelectedDivisionID, SelectedAccountCodeID)
        rptViewReport.LocalReport.Refresh()
        rptViewReport.Visible = True
        rptViewReport.Enabled = True
        rptViewReport.Reset()
        rptViewReport.LocalReport.ReportPath = "Auth/Report/ObligationRequest/rptObrParticularsByDivByAcct.rdlc"
        rptViewReport.LocalReport.DataSources.Clear()
        rptViewReport.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", New List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord)))
        rptViewReport.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dsObR))
        rptViewReport.LocalReport.Refresh()
    End Sub

    Sub LoadToPrintAllAccounts()
        Dim dsObR = dvdll.Master.ObligationRequest.GetParticularsByDivisionAllAccounts(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, SelectedDivisionID)
        rptViewReport.LocalReport.Refresh()
        rptViewReport.Visible = True
        rptViewReport.Enabled = True
        rptViewReport.Reset()
        rptViewReport.LocalReport.ReportPath = "Auth/Report/ObligationRequest/rptObrParticularsByDivByAcct.rdlc"
        rptViewReport.LocalReport.DataSources.Clear()
        rptViewReport.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", New List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord)))
        rptViewReport.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dsObR))
        rptViewReport.LocalReport.Refresh()
    End Sub

    Sub LoadToPrintAllDiv()
        Dim dsObR = dvdll.Master.ObligationRequest.GetParticularsByAllDivisionByAccountCode(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, SelectedAccountCodeID)
        rptViewReport.LocalReport.Refresh()
        rptViewReport.Visible = True
        rptViewReport.Enabled = True
        rptViewReport.Reset()
        rptViewReport.LocalReport.ReportPath = "Auth/Report/ObligationRequest/rptObrParticularsByAllDivByAcct.rdlc"
        rptViewReport.LocalReport.DataSources.Clear()
        rptViewReport.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", New List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord)))
        rptViewReport.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dsObR))
        rptViewReport.LocalReport.Refresh()
    End Sub

    Sub LoadToPrintAllDivAllAccounts()
        Dim dsObR = dvdll.Master.ObligationRequest.GetParticularsByAllDivisionAllAccounts(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue)
        rptViewReport.LocalReport.Refresh()
        rptViewReport.Visible = True
        rptViewReport.Enabled = True
        rptViewReport.Reset()
        rptViewReport.LocalReport.ReportPath = "Auth/Report/ObligationRequest/rptObrParticularsByAllDivByAcct.rdlc"
        rptViewReport.LocalReport.DataSources.Clear()
        rptViewReport.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", New List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord)))
        rptViewReport.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dsObR))
        rptViewReport.LocalReport.Refresh()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If chkAllRespoCenter.Checked Then
            If chkAllAcounts.Checked = True Then
                grdReport.Visible = False
                rptViewReport.Visible = True
                LoadToPrintAllDivAllAccounts()
            Else
                grdReport.Visible = False
                rptViewReport.Visible = True
                LoadToPrintAllDiv()
            End If
        Else
            If chkAllAcounts.Checked = True Then
                grdReport.Visible = False
                rptViewReport.Visible = True
                LoadToPrintAllAccounts()
            Else
                grdReport.Visible = False
                rptViewReport.Visible = True
                LoadToPrint()
            End If
        End If
    End Sub

    Private Sub chkAllAcounts_CheckedChanged(sender As Object, e As EventArgs) Handles chkAllAcounts.CheckedChanged

        If chkAllAcounts.Checked = True Then
            ddlAccountCode.Enabled = False
        Else
            ddlAccountCode.Enabled = True
        End If
    End Sub

    Protected Sub chkAllRespoCenter_CheckedChanged(sender As Object, e As EventArgs) Handles chkAllRespoCenter.CheckedChanged
        If chkAllRespoCenter.Checked = True Then
            ddlDivision.Enabled = False
        Else
            ddlDivision.Enabled = True
        End If
    End Sub
End Class