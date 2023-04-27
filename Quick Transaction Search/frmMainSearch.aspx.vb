Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading

Imports System.Data.SqlClient
Imports System.Data

Public Class frmMainSearch
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ddlMonths.SelectedValue = Now.Month.ToString
            ddlYear.SelectedValue = Now.Year.ToString
        End If


    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click

        grdReport.DataSource = dvdll.Master.ObligationRequest.SearchTransaction(ddlMonths.SelectedValue, ddlYear.SelectedValue, txtSearchName.Text)
        grdReport.DataBind()
        grdReport.Visible = True

    End Sub


    'Sub LoadPrintObR()
    '    Dim dsObR = dvdll.Master.ObligationRequest.GetMonthlyObR(ddlMonths.SelectedValue, ddlYear.SelectedValue)
    '    Dim dsObRPastRec = dvdll.Master.ObligationRequest.GetMonthlyObRPastRecord(ddlMonths.SelectedValue, ddlYear.SelectedValue)

    '    rptViewCreatedObR.LocalReport.Refresh()
    '    rptViewCreatedObR.Visible = True
    '    rptViewCreatedObR.Enabled = True
    '    rptViewCreatedObR.Reset()
    '    rptViewCreatedObR.LocalReport.ReportPath = "Auth/Report/ObligationRequest/rptMonthlyObrReport.rdlc"
    '    rptViewCreatedObR.LocalReport.DataSources.Clear()
    '    rptViewCreatedObR.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dsObR))
    '    rptViewCreatedObR.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", dsObRPastRec))
    '    rptViewCreatedObR.LocalReport.Refresh()
    'End Sub

    'Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click

    '    grdReport.Visible = False
    '    pnlPastRecords.Visible = False
    '    pnlPrintObR.Visible = True
    '    pnlViewObR.Visible = True
    '    rptViewCreatedObR.Visible = True
    '    rptViewer.Visible = True
    '    LoadPrintObR()
    'End Sub

End Class