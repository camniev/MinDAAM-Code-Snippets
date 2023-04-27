Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading

Imports System.Data.SqlClient
Imports System.Data


Public Class frmRptCreatedObr
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDateFrom.Text = Now.ToShortDateString
            txtDateTo.Text = Now.ToShortDateString
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        grdReport.DataSource = dvdll.Master.ObligationRequest.GetObRByDateCreated(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, txtSearchPayee.Text, txtSearchParticulars.Text, txtObRNumber.Text)
        grdReport.DataBind()

        pnlPrintObR.Visible = False
        pnlViewObR.Visible = False
        grdReport.Visible = True
    End Sub

    Sub LoadPrintObR()
        Dim dsObR = dvdll.Master.ObligationRequest.GetObRByDateCreated(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, txtSearchPayee.Text, txtSearchParticulars.Text, txtObRNumber.Text)
      
        rptViewCreatedObR.LocalReport.Refresh()
        rptViewCreatedObR.Visible = True
        rptViewCreatedObR.Enabled = True
        rptViewCreatedObR.Reset()
        rptViewCreatedObR.LocalReport.ReportPath = "Auth/Report/ObligationRequest/rptCreatedObr.rdlc"
        rptViewCreatedObR.LocalReport.DataSources.Clear()
        rptViewCreatedObR.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", New List(Of dvdll.Master.ObligationRequest.ObligationRequestRecord)))
        rptViewCreatedObR.LocalReport.DataSources.Add(New ReportDataSource("DataSet2", dsObR))
        rptViewCreatedObR.LocalReport.Refresh()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If txtDateFrom.Text = "" Or txtDateTo.Text = "" Then

        Else
            pnlPrintObR.Visible = False
            grdReport.Visible = False
            pnlViewObR.Visible = True
            LoadPrintObR()
        End If
    End Sub

    Private Sub grdReport_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdReport.RowCommand
        If e.CommandName = "xPrintObR" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.ObligationRequest.getObligationRequestByID(id)
            If Not rec Is Nothing Then
                LoadObRToPrint(rec)
            End If
            grdReport.Visible = False
            pnlViewObR.Visible = False
            pnlPrintObR.Visible = True
        End If
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

    Private Sub btnExportToExcell_Click(sender As Object, e As EventArgs) Handles btnExportToExcell.Click
        ' Response.Clear()
        '' Set the type and filename  
        'Response.AddHeader("content-disposition", "attachment;filename=FileName.xls")
        'Response.Charset = ""
        'Response.ContentType = "application/vnd.xls"

        '' Add the HTML from the GridView to a StringWriter so we can write it out later  
        'Dim sw As System.IO.StringWriter = New System.IO.StringWriter
        'Dim hw As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(sw)
        'grdReport.RenderControl(hw)

        '' Write out the data  
        'Response.Write(sw.ToString)
        'Response.End()

        '/*************/

        'Response.Clear()
        'Response.Buffer = True
        'Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.csv")
        'Response.Charset = ""
        'Response.ContentType = "application/text"

        'grdReport.AllowPaging = False
        'grdReport.DataBind()

        'Dim sb As New StringBuilder()
        'For k As Integer = 0 To grdReport.Columns.Count - 1
        '    'add separator
        '    sb.Append(grdReport.Columns(k).HeaderText + ","c)
        'Next
        ''append new line
        'sb.Append(vbCr & vbLf)
        'For i As Integer = 0 To grdReport.Rows.Count - 1
        '    For k As Integer = 0 To grdReport.Columns.Count - 1
        '        'add separator
        '        sb.Append(grdReport.Rows(i).Cells(k).Text + ","c)
        '    Next
        '    'append new line
        '    sb.Append(vbCr & vbLf)
        'Next
        'Response.Output.Write(sb.ToString())
        'Response.Flush()
        'Response.End()

        'pnlPrintObR.Visible = False
        'pnlViewObR.Visible = False
        'pnlMain.Visible = False

        '/********/

        'grdReport.AllowPaging = False
        'grdReport.AllowSorting = False

        'grdReport.DataSource = dvdll.Master.ObligationRequest.GetObRByDateCreated(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, txtSearchPayee.Text)
        'grdReport.DataBind()

        'Response.Clear()

        'Response.AddHeader("content-disposition", "attachment;filename=exported.xls")
        'Response.Charset = String.Empty

        ''Response.ContentType = "application/vnd.xls"
        'Response.ContentType = "application/vnd.ms-excel"

        'Dim sw As System.IO.StringWriter = New System.IO.StringWriter()
        'Dim hw As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(sw)
        'grdReport.RenderControl(hw)
        'Response.Write(sw.ToString())
        'Response.End()

        '/***********/

        Try
            Response.ClearContent()
            Response.AddHeader("content-disposition", "attachment;filename=MyCsvFile.csv")
            Response.ContentType = "application/text"
            Dim strBr As New StringBuilder()
            For i As Integer = 0 To grdReport.Columns.Count - 1
                strBr.Append(grdReport.Columns(i).HeaderText + ","c)
            Next
            strBr.Append(vbLf)
            For j As Integer = 0 To grdReport.Rows.Count - 1
                For k As Integer = 0 To grdReport.Columns.Count - 1
                    strBr.Append(grdReport.Rows(j).Cells(k).Text + ","c)
                Next
                strBr.Append(vbLf)
            Next
            Response.Write(strBr.ToString())
            Response.Flush()
            Response.[End]()
        Catch ex As Exception
        End Try

    End Sub
   Public Overrides Sub VerifyRenderingInServerForm(control As Control)

    End Sub

    'CHKPARTICULARS HIDDEN 07152022
    'Protected Sub chkParticulars_CheckedChanged(sender As Object, e As EventArgs) Handles chkParticulars.CheckedChanged
    '    If chkParticulars.Checked = True Then
    '        txtSearchPayee.Enabled = False
    '        txtObRNumber.Enabled = False
    '    Else
    '        txtSearchPayee.Enabled = True
    '        txtObRNumber.Enabled = True
    '    End If
    'End Sub

End Class