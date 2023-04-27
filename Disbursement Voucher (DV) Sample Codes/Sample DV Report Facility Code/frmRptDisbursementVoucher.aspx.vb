Imports Microsoft.Reporting.WebForms

Public Class frmRptDisbursementVoucher
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDateFrom.Text = Now.ToShortDateString
            txtDateTo.Text = Now.ToShortDateString
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        grdReport.DataSource = dvdll.Master.DisbursementVoucher.GetDVByDateCreated(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, txtSearchPayee.Text, txtDvNumber.Text)
        grdReport.DataBind()
        pnlViewDV.Visible = False
        pnlPrintDV.Visible = False
        pnlPrintObR.Visible = False
        grdReport.Visible = True
    End Sub

    Sub LoadPrintDV()
        Dim dsDv = dvdll.Master.DisbursementVoucher.GetDVByDateCreated(CDate(txtDateFrom.Text), CDate(txtDateTo.Text), rdbtnStatus.SelectedValue, txtSearchPayee.Text, txtDvNumber.Text)
        rptViewCreatedObR.LocalReport.Refresh()
        rptViewCreatedObR.Visible = True
        rptViewCreatedObR.Enabled = True
        rptViewCreatedObR.Reset()
        rptViewCreatedObR.LocalReport.ReportPath = "Auth/Report/DisbursementVoucher/rptDVRecords.rdlc"
        rptViewCreatedObR.LocalReport.DataSources.Clear()
        rptViewCreatedObR.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dsDv))
        rptViewCreatedObR.LocalReport.Refresh()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        grdReport.Visible = False
        pnlPrintDV.Visible = False
        pnlPrintObR.Visible = False
        pnlViewDV.Visible = True
        LoadPrintDV()
    End Sub

    Private Sub grdReport_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdReport.RowCommand
        If e.CommandName = "xPrintDv" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.DisbursementVoucher.getDVByID(id)
            If Not rec Is Nothing Then
                LoadDVtoPrint(rec)
            End If
            grdReport.Visible = False
            pnlPrintObR.Visible = False
            pnlPrintDV.Visible = True
        End If

        If e.CommandName = "xPrintObR" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec2 = dvdll.Master.DisbursementVoucher.getDVByID(id)
            Dim rec = dvdll.Master.ObligationRequest.getObligationRequestByID(rec2.ObligationRequestID)
            If Not rec Is Nothing Then
                LoadObRToPrint(rec)
            End If
            grdReport.Visible = False
            pnlPrintDV.Visible = False
            pnlPrintObR.Visible = True
        End If

    End Sub



    Sub LoadDVtoPrint(dvRec As dvdll.dv_DisbursementVoucher)
        Dim dsDv = New List(Of dvdll.dv_DisbursementVoucher)
        dsDv.Add(dvRec)

        Dim dvParticlarEntry = dvdll.Master.DisbursementVoucher.getDVParticularEntryByDVID(dvRec.ID)
        Dim dsObRParticulars = dvdll.Master.DisbursementVoucher.getORParticularsByORID(dvRec.ObligationRequestID)
        Dim dvAccountEntry = dvdll.Master.DisbursementVoucher.getDVAccountEntryByDVID(dvRec.ID)
        Dim dvTaxEntry = dvdll.Master.DisbursementVoucher.getDVTaxEntryByDVID(dvRec.ID)
        Dim dvTaxEntry2 = dvdll.Master.DisbursementVoucher.getDVTaxEntryByDVID2(dvRec.ID)
        Dim dvPPA = dvdll.Master.DisbursementVoucher.getDVPPA(dvRec.ID)
        Dim dvRESPO = dvdll.Master.DisbursementVoucher.getDVRESPO(dsDv(0).dv_ParticularEntry(0).ResponsibilityCenterID)
        Dim dvSigA = dvdll.Master.DisbursementVoucher.getDVSignatoryA(dsDv(0).DVSignatoryA)
        Dim dvSigB = dvdll.Master.DisbursementVoucher.getDVSignatoryB(dsDv(0).DVSignatoryB)
        Dim dvSigC = dvdll.Master.DisbursementVoucher.getDVSignatoryC(dsDv(0).DVSignatoryC)
        Dim dvSigD = dvdll.Master.DisbursementVoucher.getDVSignatoryD(dsDv(0).DVSignatoryD)
        Dim dvSigE = dvdll.Master.DisbursementVoucher.getDVSignatoryE(dsDv(0).DVSignatoryE)
        Dim dvSigF = dvdll.Master.DisbursementVoucher.getDVSignatoryF(dsDv(0).DVSignatoryF)
        Dim dvCertifiedBy = dvdll.Master.DisbursementVoucher.getDVCertifiedBy(dsDv(0).DVPreparedBy)
        Dim dvApprovedBy = dvdll.Master.DisbursementVoucher.getDVApprovedBy(dsDv(0).DVApprovedBy)

        Dim isSub = dvTaxEntry.Find(Function(p) p.IsSubAcct = True)

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
End Class