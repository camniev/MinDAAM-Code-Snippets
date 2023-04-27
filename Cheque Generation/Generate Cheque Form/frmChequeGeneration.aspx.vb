Imports Microsoft.Reporting.WebForms
Imports System.Net.Mail

Public Class frmChequeGeneration
    Inherits System.Web.UI.Page

#Region "Properties"
    Property DVPayeeSearchList As List(Of dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord)
        Get
            Return ViewState("DVPayeeSearchList")
        End Get
        Set(value As List(Of dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord))
            ViewState("DVPayeeSearchList") = value
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

    Property DisbursmentVoucher As dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord
        Get
            Return ViewState("DisbursmentVoucher")
        End Get
        Set(value As dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord)
            ViewState("DisbursmentVoucher") = value
        End Set
    End Property

    Property DVApproved As List(Of dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord)
        Get
            Return ViewState("DVApproved")
        End Get
        Set(value As List(Of dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord))
            ViewState("DVApproved") = value
        End Set
    End Property

    Property ChequeGeneration As dvdll.Master.ChequeGeneration.ChequeGenerationRecord
        Get
            Return ViewState("ChequeGeneration")
        End Get
        Set(value As dvdll.Master.ChequeGeneration.ChequeGenerationRecord)
            ViewState("ChequeGeneration") = value
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

    Property CHisPrio As Boolean
        Get
            Return ViewState("CHisPrio")
        End Get
        Set(value As Boolean)
            ViewState("CHisPrio") = value
        End Set
    End Property

    Property Email As String
        Get
            Return ViewState("Email")
        End Get
        Set(value As String)
            ViewState("Email") = value
        End Set
    End Property

    Property ItemDetails As String
        Get
            Return ViewState("ItemDetails")
        End Get
        Set(value As String)
            ViewState("ItemDetails") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            pnlChequeGen.Visible = False
            LoadGridApproveDV()
        End If
    End Sub

    Sub LoadGridApproveDV()
        'DVApproved = dvdll.Master.DisbursementVoucher.getDVByStatus(dvdll.Master.DisbursementVoucher.DVStatus.Approved)
        'grdApprovedDVList.DataSource = DVApproved
        'grdApprovedDVList.DataBind()

        If IsSearch Then
            DVApproved = dvdll.Master.DisbursementVoucher.getDVByPayee(txtSearch.Text, dvdll.Master.DisbursementVoucher.DVStatus.Approved)
        Else
            DVApproved = dvdll.Master.DisbursementVoucher.getDVByStatus(dvdll.Master.DisbursementVoucher.DVStatus.Approved)
        End If
        grdApprovedDVList.DataSource = DVApproved
        grdApprovedDVList.DataBind()

    End Sub


#Region " Convert Number to Words"

    'Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGO.Click
    '    txtResult.Text = ConverttoWord.ConvertNum(txtNumber.Text)
    'End Sub


    Public Class ConverttoWord

        Public Shared Function ConvertNum(ByVal Input As Decimal) As String


            Dim formatnumber As String
            Dim numparts(10) As String ' break the number into parts
            Dim suffix(10) As String 'trillion, billion .million etc
            Dim Wordparts(10) As String  'add the number parts and suffix

            Dim output As String = Nothing

            Dim T As String = ""
            Dim B As String = ""
            Dim M As String = ""
            Dim TH As String = ""
            Dim H As String = ""
            Dim C As String = ""

            formatnumber = Format(Input, "0000000000000.00") 'format the input number to a 16 characters string by suffixing and prefixing 0s
            '
            numparts(0) = primWord(Mid(formatnumber, 1, 1)) 'Trillion

            numparts(1) = primWord(Mid(formatnumber, 2, 1)) 'hundred billion..x
            numparts(2) = primWord(Mid(formatnumber, 3, 2)) 'billion

            numparts(3) = primWord(Mid(formatnumber, 5, 1)) 'hundred million...x
            numparts(4) = primWord(Mid(formatnumber, 6, 2)) 'million

            numparts(5) = primWord(Mid(formatnumber, 8, 1)) 'hundred thousand....x
            numparts(6) = primWord(Mid(formatnumber, 9, 2)) 'thousand


            numparts(7) = primWord(Mid(formatnumber, 11, 1)) 'hundred
            numparts(8) = primWord(Mid(formatnumber, 12, 2)) 'Tens

            numparts(9) = primWord(Mid(formatnumber, 15, 2)) 'cents



            suffix(0) = " Trillion "
            suffix(1) = " Hundred "  '....x
            suffix(2) = " Billion "
            suffix(3) = " Hundred " '  ....x
            suffix(4) = " Million "
            suffix(5) = " Hundred " ' .....x
            suffix(6) = " Thousand "
            suffix(7) = " Hundred "
            suffix(8) = " "
            suffix(9) = ""

            For i = 0 To 9
                If numparts(i) <> "" Then
                    Wordparts(i) = numparts(i) & suffix(i)
                End If

                T = Wordparts(0)

                If Wordparts(1) <> "" And Wordparts(2) = "" Then
                    B = Wordparts(1) & " Billion "
                Else
                    B = Wordparts(1) & Wordparts(2)
                End If

                If Wordparts(3) <> "" And Wordparts(4) = "" Then
                    M = Wordparts(3) & " Million "
                Else
                    M = Wordparts(3) & Wordparts(4)
                End If

                If Wordparts(5) <> "" And Wordparts(6) = "" Then

                    TH = Wordparts(5) & " Thousand "
                Else
                    TH = Wordparts(5) & Wordparts(6)
                End If

                H = Wordparts(7) & Wordparts(8)
                If Wordparts(9) = "" Then
                    C = " Pesos and  Zero Centavos "
                Else
                    C = " Pesos and " & Wordparts(9) & " Centavos "
                End If
            Next
            output = T & B & M & TH & H & C
            Return output


        End Function


        Public Shared Function primWord(ByVal Num As Integer) As String

            'This two dimensional array store the primary word convertion of numbers 0 to 99
            primWord = ""
            Dim wordList(,) As Object = {{1, "One"}, {2, "Two"}, {3, "Three"}, {4, "Four"}, {5, "Five"},
        {6, "Six "}, {7, "Seven "}, {8, "Eight "}, {9, "Nine "}, {10, "Ten "}, {11, "Eleven "}, {12, "Twelve "}, {13, "Thirteen "},
        {14, "Fourteen "}, {15, "Fifteen "}, {16, "Sixteen "}, {17, "Seventeen "}, {18, "Eighteen "}, {19, "Nineteen "},
        {20, "Twenty "}, {21, "Twenty One "}, {22, "Twenty Two"}, {23, "Twenty Three"}, {24, "Twenty Four"}, {25, "Twenty Five"},
        {26, "Twenty Six"}, {27, "Twenty Seven"}, {28, "Twenty Eight"}, {29, "Twenty Nine"}, {30, "Thirty "}, {31, "Thirty One "},
        {32, "Thirty Two"}, {33, "Thirty Three"}, {34, "Thirty Four"}, {35, "Thirty Five"}, {36, "Thirty Six"}, {37, "Thirty Seven"},
        {38, "Thirty Eight"}, {39, "Thirty Nine"}, {40, "Forty "}, {41, "Forty One "}, {42, "Forty Two"}, {43, "Forty Three"},
        {44, "Forty Four"}, {45, "Forty Five"}, {46, "Forty Six"}, {47, "Forty Seven"}, {48, "Forty Eight"}, {49, "Forty Nine"},
        {50, "Fifty "}, {51, "Fifty One "}, {52, "Fifty Two"}, {53, "Fifty Three"}, {54, "Fifty Four"}, {55, "Fifty Five"},
        {56, "Fifty Six"}, {57, "Fifty Seven"}, {58, "Fifty Eight"}, {59, "Fifty Nine"}, {60, "Sixty "}, {61, "Sixty One "},
        {62, "Sixty Two"}, {63, "Sixty Three"}, {64, "Sixty Four"}, {65, "Sixty Five"}, {66, "Sixty Six"}, {67, "Sixty Seven"}, {68, "Sixty Eight"},
        {69, "Sixty Nine"}, {70, "Seventy "}, {71, "Seventy One "}, {72, "Seventy Two"}, {73, "Seventy Three"}, {74, "Seventy Four"},
        {75, "Seventy Five"}, {76, "Seventy Six"}, {77, "Seventy Seven"}, {78, "Seventy Eight"}, {79, "Seventy Nine"},
        {80, "Eighty "}, {81, "Eighty One "}, {82, "Eighty Two"}, {83, "Eighty Three"}, {84, "Eighty Four"}, {85, "Eighty Five"},
        {86, "Eighty Six"}, {87, "Eighty Seven"}, {88, "Eighty Eight"}, {89, "Eighty Nine"}, {90, "Ninety "}, {91, "Ninety One "},
        {92, "Ninety Two"}, {93, "Ninety Three"}, {94, "Ninety Four"}, {95, "Ninety Five"}, {96, "Ninety Six"}, {97, "Ninety Seven"},
        {98, "Ninety Eight"}, {99, "Ninety Nine"}}

            Dim i As Integer
            For i = 0 To UBound(wordList)
                If Num = wordList(i, 0) Then
                    primWord = wordList(i, 1)
                    Exit For
                End If
            Next
            Return primWord
        End Function


    End Class

#End Region

    Private Sub grdApprovedDVList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdApprovedDVList.PageIndexChanging
        grdApprovedDVList.PageIndex = e.NewPageIndex
        LoadGridApproveDV()
    End Sub

    Private Sub grdApprovedDVList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdApprovedDVList.RowCommand
        If e.CommandName = "xGenerate" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.DisbursementVoucher.getDVRecordByID(id)
            DisbursmentVoucher = rec
            LoadDVDetails(id)
            pnlMainDV.Visible = False
            pnlChequeGen.Visible = True

        ElseIf e.CommandName = "xBackDV" Then
            Dim id = Guid.Parse(e.CommandArgument)
            Dim rec = dvdll.Master.DisbursementVoucher.getDVByID(id)

            dvdll.Master.DisbursementVoucher.UpdateDVStatusToApprove(rec.ID, dvdll.Master.DisbursementVoucher.DVStatus.ForApproval, Now.ToShortDateString)

            SelectedObRID = dvdll.Master.DisbursementVoucher.getDVByID(rec.ID).ObligationRequestID
            dvdll.Master.ObligationRequest.UpdateOBStatus(SelectedObRID, dvdll.Master.ObligationRequest.ObRStatus.Approved)
            'Try
            '    dvdll.Master.DisbursementVoucher.UpdateDVStatus(id, dvdll.Master.DisbursementVoucher.DVStatus.ForApproval)
            'Catch ex As Exception

            '    'ShowInfoPopUp("Obligation Request Approved", "Items on Queue for Disbursement Voucher")
            '    End Try

            LoadGridApproveDV()
        End If
    End Sub

    Public Sub LoadDVDetails(dvID As Guid)
        DisbursmentVoucher = dvdll.Master.DisbursementVoucher.getDVRecordByID(dvID)
        With DisbursmentVoucher
            txtPayOrderTo.Text = DisbursmentVoucher.PayeeName
            txtNumber.Text = DisbursmentVoucher.ParticularsAmountDue
            txtResult.Text = ConverttoWord.ConvertNum(txtNumber.Text)
            lblRefDVNo.Text = DisbursmentVoucher.DisbursementVoucherNo
            lblAllotment.Text = DisbursmentVoucher.ObRAllotmentObjectClass
            If DisbursmentVoucher.DVisPriority.HasValue Then
                CHisPrio = DisbursmentVoucher.DVisPriority
            Else
            End If

            Email = DisbursmentVoucher.PayeeEmail
            ItemDetails = DisbursmentVoucher.DVParticularTemplate
        End With
    End Sub

    Private Sub calChequeDate_SelectionChanged(sender As Object, e As EventArgs) Handles calChequeDate.SelectionChanged
        Dim chequedate As Date

        chequedate = calChequeDate.SelectedDate
        txtChequeDate.Text = chequedate.ToString("MMMM dd, yyyy")
        pnlChequeGen.Visible = True
    End Sub

    Sub testprint(rec As dvdll.ch_Cheque)
        Dim dummylst = New List(Of dvdll.ch_Cheque)
        dummylst.Add(rec)

        rptChViewer.Visible = True
        rptChViewer.Enabled = True
        rptChViewer.Reset()
        rptChViewer.LocalReport.ReportPath = "Auth/Transactions/rdlc/rptCheque.rdlc"
        rptChViewer.LocalReport.DataSources.Clear()
        rptChViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dummylst))
        rptChViewer.LocalReport.Refresh()

        'rptDVViewer.Visible = True
        'rptDVViewer.Enabled = True
        'rptDVViewer.Reset()
        'rptDVViewer.LocalReport.ReportPath = "Auth/Transactions/rdlc/rptDisbursementVoucher.rdlc"
        'rptDVViewer.LocalReport.DataSources.Clear()
        'rptDVViewer.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", dummylst))
        'rptDVViewer.LocalReport.Refresh()
    End Sub

    Event ChequeSaved()

    Private Sub btnGenerateCheque_Click(sender As Object, e As EventArgs) Handles btnGenerateCheque.Click
        Dim signatoryA As Guid
        Dim signatoryB As Guid
        If Not ValidGenerate() Then
            mpeInfoPop.Show()
        Else
            signatoryA = signatorySelectionA.SelectedID
            signatoryB = signatorySelectionB.SelectedID
            Dim chequeprint = dvdll.Master.ChequeGeneration.SaveCheque(txtChequeNo.Text, txtChequeDate.Text, txtNumber.Text, txtResult.Text, txtPayOrderTo.Text, DisbursmentVoucher.ID, lblAllotment.Text, signatoryA, signatoryB, CHisPrio)
            testprint(chequeprint)
            pnlChequeGen.Visible = False
            pnlPrintCheque.Visible = True
            RaiseEvent ChequeSaved()
        End If


        Try
            dvdll.Master.DisbursementVoucher.UpdateDVStatus(DisbursmentVoucher.ID, dvdll.Master.DisbursementVoucher.DVStatus.OnCheque)

            SelectedObRID = dvdll.Master.DisbursementVoucher.getDVByID(DisbursmentVoucher.ID).ObligationRequestID
            dvdll.Master.ObligationRequest.UpdateOBStatus(SelectedObRID, dvdll.Master.ObligationRequest.ObRStatus.CHGenerated)

        Catch ex As Exception
        End Try

        'Try
        '    If Email = "" Then
        '        Exit Sub
        '    Else
        '        SendSimpleMail()
        '    End If
        'Catch ex As Exception
        '    Exit Sub
        'End Try



    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        pnlChequeGen.Visible = False
        pnlMainDV.Visible = True
    End Sub

    Private Sub btnClosePrint_Click(sender As Object, e As EventArgs) Handles btnClosePrint.Click
        pnlPrintCheque.Visible = False
        pnlMainDV.Visible = True
        LoadGridApproveDV()
    End Sub

    Function ValidGenerate() As Boolean
        lblInfoPopMsg.Text = ""
        If txtChequeNo.Text = "" Then
            lblInfoPopMsg.Text = "Please enter Cheque No."
            Return False
        End If
        If txtChequeDate.Text = "" Then
            lblInfoPopMsg.Text = "Please enter Cheque Date"
            Return False
        End If
        Return True
    End Function

    Private Sub grdApprovedDVList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdApprovedDVList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each item In DVApproved
                If item.DVisPriority = True Then
                    PrioDVNo = item.DisbursementVoucherNo
                    If Convert.ToString(e.Row.Cells(1).Text) = PrioDVNo Then
                        e.Row.BackColor = Drawing.Color.Aquamarine
                    End If
                End If
            Next
        End If
    End Sub

#Region "Grid Sorting"

    Private Sub grdApprovedDVList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdApprovedDVList.Sorting
        Dim currentSortDirection = getSortDirection(e.SortExpression)

        Select Case e.SortExpression
            Case "DisbursementVoucherNo"
                If currentSortDirection = "ASC" Then
                    DVApproved = (From p In DVApproved Order By p.DisbursementVoucherNo Ascending Select p).ToList
                Else
                    DVApproved = (From p In DVApproved Order By p.DisbursementVoucherNo Descending Select p).ToList
                End If
            Case "PayeeName"
                If currentSortDirection = "ASC" Then
                    DVApproved = (From p In DVApproved Order By p.PayeeName Ascending Select p).ToList
                Else
                    DVApproved = (From p In DVApproved Order By p.PayeeName Descending Select p).ToList
                End If
            Case "DateCreated"
                If currentSortDirection = "ASC" Then
                    DVApproved = (From p In DVApproved Order By p.DateCreated Ascending Select p).ToList
                Else
                    DVApproved = (From p In DVApproved Order By p.DateCreated Descending Select p).ToList
                End If
            Case "ParticularsAmountDue"
                If currentSortDirection = "ASC" Then
                    DVApproved = (From p In DVApproved Order By p.ParticularsAmountDue Ascending Select p).ToList
                Else
                    DVApproved = (From p In DVApproved Order By p.ParticularsAmountDue Descending Select p).ToList
                End If

        End Select

        grdApprovedDVList.DataSource = DVApproved
        grdApprovedDVList.DataBind()
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


    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        txtSearch.Focus()
        If Not String.IsNullOrEmpty(txtSearch.Text) And Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
            IsSearch = True
            DVPayeeSearchList = dvdll.Master.DisbursementVoucher.getDVPayeeByFilter(txtSearch.Text)
        Else
            IsSearch = False
            DVPayeeSearchList = New List(Of dvdll.Master.DisbursementVoucher.DisbursementVoucherRecord)
        End If
        LoadGridApproveDV()
    End Sub

    Sub SendSimpleMail()

        Dim Alert As New System.Net.Mail.MailMessage
        Alert.To.Add(Email)
        Alert.Subject = "Notification of Prepared Cheque"
        Alert.Body = "Cheque is Ready for the " + ItemDetails
        Dim client As New SmtpClient()

        Try
            client.Send(Alert)
        Catch ex As Exception
            ' ...
        End Try

    End Sub
End Class