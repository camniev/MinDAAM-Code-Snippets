Public Class ctrlCreateObligationRequest
    Inherits System.Web.UI.UserControl

#Region "Properties"


    Property DynamicControlIDs As List(Of String)
        Get
            Return ViewState("DynamicControlIDs")
        End Get
        Set(value As List(Of String))
            ViewState("DynamicControlIDs") = value
        End Set
    End Property
    Property ObligationRequest As dvdll.Master.ObligationRequest.ObligationRequestRecord
        Get
            Return ViewState("ObligationRequest")
        End Get
        Set(value As dvdll.Master.ObligationRequest.ObligationRequestRecord)
            ViewState("ObligationRequest") = value
        End Set
    End Property
    Property LatestIDCount As Integer
        Get
            Return ViewState("LatestIDCount")
        End Get
        Set(value As Integer)
            ViewState("LatestIDCount") = value
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


    Protected Overrides Sub LoadViewState(savedState As Object)
        MyBase.LoadViewState(savedState)
        For Each itmID In DynamicControlIDs
            Dim xButton As New Button
            Dim newDivision As ctrtDivision = Page.LoadControl("~/custom controls/ctrtDivision.ascx")
            Dim newParticular As ctrlParticular = Page.LoadControl("~/custom controls/ctrlParticular.ascx")
            Dim newParticulartemplate As ctrlParticularTemplate = Page.LoadControl("~/custom controls/ctrlParticularTemplate.ascx")
            'Dim xTextBoxSource As New TextBox
            Dim newPPA As ctrlPPA = Page.LoadControl("~/custom controls/ctrlPPA.ascx")
            Dim xTextBoxAcctCode As New TextBox
            Dim xTextBoxAmount As New TextBox

            xTextBoxAcctCode.CssClass = "acctcodecss"
            xTextBoxAcctCode.ReadOnly = True
            xTextBoxAmount.CssClass = "amountcss"

            Dim newRow As New TableRow
            Dim cell0 As New TableCell
            Dim cell1 As New TableCell
            Dim cell2 As New TableCell
            Dim cell3 As New TableCell
            Dim cell4 As New TableCell
            Dim cell5 As New TableCell
            Dim cell6 As New TableCell

            xButton.ID = "btn_" & itmID.ToString
            xButton.Text = "x"
            AddHandler xButton.Click, AddressOf xButton_Click
            newDivision.ID = "ucDivision_" & itmID.ToString
            AddHandler newDivision.SelectedDivision, AddressOf ucDiv_SelectedDivision
            AddHandler newDivision.SelectedDivision_FundSource, AddressOf ucDiv_SelectedDivision_FundSource
            newParticular.ID = "ucParticular_" & itmID.ToString
            AddHandler newParticular.ParticularSelected, AddressOf ucPart_ParticularSelected
            AddHandler newParticular.TravellerDetails, AddressOf ucPart_TravellerDetails

            newParticulartemplate.ID = "ucParticularTemplate_" & itmID.ToString
            AddHandler newParticulartemplate.ParticularTemplateSelected, AddressOf ucParticualrTemplate_ParticularTemplateSelected

            'xTextBoxSource.ID = "txtSource_" & itmID.ToString
            'xTextBoxSource.AutoPostBack = True
            'AddHandler xTextBoxSource.TextChanged, AddressOf xSourceDoc_TextChanged
            newPPA.ID = "ucPPA_" & itmID.ToString
            AddHandler newPPA.SelectedPPA, AddressOf ucPPA_SelectedPPA
            xTextBoxAcctCode.ID = "txtAcct_" & itmID.ToString
            xTextBoxAmount.ID = "txtAmt_" & itmID.ToString
            xTextBoxAmount.AutoPostBack = True
            AddHandler xTextBoxAmount.TextChanged, AddressOf xAmount_TextChanged

            cell0.Controls.Add(xButton)
            cell1.Controls.Add(newDivision)
            cell2.Controls.Add(newParticular)
            'cell3.Controls.Add(xTextBoxSource)
            cell3.Controls.Add(newPPA)
            cell4.Controls.Add(xTextBoxAcctCode)
            cell5.Controls.Add(xTextBoxAmount)

            newRow.Cells.Add(cell0)
            newRow.Cells.Add(cell1)
            newRow.Cells.Add(cell2)
            newRow.Cells.Add(cell3)
            newRow.Cells.Add(cell4)
            newRow.Cells.Add(cell5)
            newRow.Cells.Add(cell6)
            'tblParticular.Rows.Add(newRow)
            tblParticular.Rows.AddAt(tblParticular.Rows.Count - 1, newRow)
        Next
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ClearObligationRequest(True)
            txtDateCreated.Text = Now.ToShortDateString
            LoadUACSSourceCodes()
        End If

        dUser = System.Web.Security.Membership.GetUser.ToString
    End Sub

    Sub LoadUACSSourceCodes()
        Dim FundSourceSelectionList As List(Of dvdll.Master.UACSCode.UACSCodeRecord)
        FundSourceSelectionList = dvdll.Master.UACSCode.getUACSSourceCodesForChoices

        Dim FSSTbl As New DataTable
        FSSTbl.Columns.Add("ID", GetType(Guid))
        FSSTbl.Columns.Add("UACSCodeDesc", GetType(String))
        FSSTbl.Columns.Add("IsActive", GetType(Integer))

        For Each FSSList In FundSourceSelectionList
            FSSTbl.Rows.Add(FSSList.ID, FSSList.UACSCode + " - " + FSSList.UACSDescription, FSSList.IsActive)
        Next

        ddlFundSourceSelection.Items.Add(New ListItem With {.Text = "[-- SELECT UACS FUNDING SOURCE --]", .Value = "-1"})
        ddlFundSourceSelection.DataSource = FSSTbl
        ddlFundSourceSelection.DataBind()
    End Sub

#Region "Control Events"
    Private Sub btnAddTrans_Click(sender As Object, e As EventArgs) Handles btnAddTrans.Click
        Dim newEntry As New dvdll.Master.ObligationRequest.ORParticularEntry
        newEntry.ID = Guid.NewGuid
        newEntry.DataStatus = dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Add
        Dim xButton As New Button
        Dim newDivision As ctrtDivision = Page.LoadControl("~/custom controls/ctrtDivision.ascx")
        Dim newParticular As ctrlParticular = Page.LoadControl("~/custom controls/ctrlParticular.ascx")
        Dim newParticulartemplate As ctrlParticularTemplate = Page.LoadControl("~/custom controls/ctrlParticularTemplate.ascx")
        Dim newPPA As ctrlPPA = Page.LoadControl("~/custom controls/ctrlPPA.ascx")
        Dim xTextBoxAcctCode As New TextBox
        Dim xTextBoxAmount As New TextBox

        xTextBoxAcctCode.CssClass = "acctcodecss"
        xTextBoxAcctCode.ReadOnly = True
        xTextBoxAmount.CssClass = "amountcss"

        Dim newRow As New TableRow
        Dim cell0 As New TableCell
        Dim cell1 As New TableCell
        Dim cell2 As New TableCell
        Dim cell3 As New TableCell
        Dim cell4 As New TableCell
        Dim cell5 As New TableCell
        Dim cell6 As New TableCell

        LatestIDCount += 1

        xButton.ID = "btn_" & LatestIDCount.ToString
        xButton.Text = "x"
        AddHandler xButton.Click, AddressOf xButton_Click
        newDivision.ID = "ucDivision_" & LatestIDCount.ToString
        AddHandler newDivision.SelectedDivision, AddressOf ucDiv_SelectedDivision
        AddHandler newDivision.SelectedDivision_FundSource, AddressOf ucDiv_SelectedDivision_FundSource
        newParticular.ID = "ucParticular_" & LatestIDCount.ToString
        AddHandler newParticular.ParticularSelected, AddressOf ucPart_ParticularSelected
        AddHandler newParticular.TravellerDetails, AddressOf ucPart_TravellerDetails

        newParticulartemplate.ID = "ucParticularTemplate_" & LatestIDCount.ToString
        AddHandler newParticulartemplate.ParticularTemplateSelected, AddressOf ucParticualrTemplate_ParticularTemplateSelected

        'xTextBoxSource.ID = "txtSource_" & LatestIDCount.ToString
        'xTextBoxSource.AutoPostBack = True
        'AddHandler xTextBoxSource.TextChanged, AddressOf xSourceDoc_TextChanged
        newPPA.ID = "ucPPA_" & LatestIDCount.ToString
        AddHandler newPPA.SelectedPPA, AddressOf ucPPA_SelectedPPA
        xTextBoxAcctCode.ID = "txtAcct_" & LatestIDCount.ToString
        xTextBoxAmount.ID = "txtAmt_" & LatestIDCount.ToString
        xTextBoxAmount.AutoPostBack = True
        AddHandler xTextBoxAmount.TextChanged, AddressOf xAmount_TextChanged

        cell0.Controls.Add(xButton)
        cell1.Controls.Add(newDivision)
        cell2.Controls.Add(newParticular)
        'cell3.Controls.Add(xTextBoxSource)
        cell3.Controls.Add(newPPA)
        cell4.Controls.Add(xTextBoxAcctCode)
        cell5.Controls.Add(xTextBoxAmount)

        newRow.Cells.Add(cell0)
        newRow.Cells.Add(cell1)
        newRow.Cells.Add(cell2)
        newRow.Cells.Add(cell3)
        newRow.Cells.Add(cell4)
        newRow.Cells.Add(cell5)
        'newRow.Cells.Add(cell6)
        tblParticular.Rows.AddAt(tblParticular.Rows.Count - 1, newRow)


        DynamicControlIDs.Add(LatestIDCount.ToString)
        newEntry.IDIndex = LatestIDCount
        ObligationRequest.ORParticularEntries.Add(newEntry)
    End Sub
    Private Sub xButton_Click(sender As Object, e As EventArgs)
        Dim btn = DirectCast(sender, Button)
        Dim idStr() = btn.ID.ToString.Split("_")
        Dim idcnt = CInt(idStr(1))
        DynamicControlIDs.Remove(idcnt.ToString)
        Dim rowIdx As Integer = 0
        Dim exitFor As Boolean = False
        For idx As Integer = 0 To tblParticular.Rows.Count
            rowIdx = idx
            Dim row = tblParticular.Rows(idx)
            Dim ctrlcol = row.Cells(0).Controls
            For Each itm As Control In ctrlcol
                If itm.ID = btn.ID Then
                    exitFor = True
                    Exit For
                End If
            Next
            If exitFor Then Exit For
        Next
        tblParticular.Rows.RemoveAt(rowIdx)
        Dim remRec = ObligationRequest.ORParticularEntries.FirstOrDefault(Function(p) p.IDIndex = idcnt)
        If Not remRec Is Nothing Then
            remRec.DataStatus = dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Delete
        End If


        tblParticular.Rows.Clear()

        UpdatingInitializeTransactionLineForUpdate()


                'ComputeTotal

                Dim totAmt As Decimal = 0
                For Each entry In ObligationRequest.ORParticularEntries
                    If Not entry.Amount Is Nothing And entry.DataStatus <> dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Delete Then
                        totAmt += entry.Amount
                    End If
                Next
                ObligationRequest.TotalAmount = totAmt
        txtTotalAmount.Text = totAmt.ToString("#,#00.00")

        tblParticular.Rows.AddAt(tblParticular.Rows.Count, rowTotal)

        ComputeTotalAmount()

    End Sub
    'Private Sub xSourceDoc_TextChanged(sender As Object, e As EventArgs)
    '    Dim srcdoc = DirectCast(sender, TextBox)
    '    Dim str() = srcdoc.ID.Split("_")
    '    Dim idcnt = CInt(str(1))

    '    Dim rec = ObligationRequest.ORParticularEntries.FirstOrDefault(Function(p) p.IDIndex = idcnt)
    '    If Not rec Is Nothing Then
    '        rec.SourceDocument = srcdoc.Text
    '    End If
    'End Sub
    Private Sub xAmount_TextChanged(sender As Object, e As EventArgs)
        Dim amt = DirectCast(sender, TextBox)

        'Try
        '    Dim test = CDec(amt.Text)
        'Catch ex As Exception
        '    amt.Focus()
        '    ucInfoPop.SetMessage("Invalid Amount value {" & amt.Text & "}. Only numeric values are allowed.")
        '    ucInfoPop.ShowInfo()
        'End Try

        Dim str() = amt.ID.Split("_")
        Dim idcnt = CInt(str(1))

        Dim rec = ObligationRequest.ORParticularEntries.FirstOrDefault(Function(p) p.IDIndex = idcnt)
        If Not rec Is Nothing Then
            rec.Amount = CDec(amt.Text)
            rec.DataStatus = dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Update
            ComputeTotalAmount()
        End If
    End Sub
    Private Sub ucPart_ParticularSelected(id As Guid, acctcode As String, acctdesc As String, ctrlid As String)
        Dim str() = ctrlid.Split("_")
        Dim idcnt = CInt(str(1))

        Dim rowSel = tblParticular.Rows(getRowIdx(idcnt))
        Dim txtbox As TextBox = rowSel.Cells(5).FindControl("txtAcct_" & idcnt.ToString)
        txtbox.Text = acctcode

        Dim rec = ObligationRequest.ORParticularEntries.FirstOrDefault(Function(p) p.IDIndex = idcnt)
        If Not rec Is Nothing Then
            rec.DataStatus = dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Update
            rec.AcctID = Guid.Parse(id.ToString.Replace("{", "").Replace("}", ""))
        End If
    End Sub

    Private Sub ucPart_TravellerDetails(id As Guid, traveldestination As String, fromdate As Date, todate As Date, datesummary As String, ctrlid As String, purpose As String)
        'lbltravelername.Text = id
        Dim str() = ctrlid.Split("_")
        Dim idcnt = CInt(str(1))

        Dim rec = ObligationRequest.ORParticularEntries.FirstOrDefault(Function(p) p.IDIndex = idcnt)
        If Not rec Is Nothing Then
            rec.DataStatus = dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Update
            rec.TravelDateFrom = fromdate
            rec.TravelDateTo = todate
            rec.TravelDestination = traveldestination
            rec.TravellerPayeeID = id
            rec.TravelPurpose = purpose
        End If
    End Sub

    Private Sub ucDiv_SelectedDivision(name As String, desc As String, id As Guid, ctrlid As String)
        Dim str() = ctrlid.Split("_")
        Dim idcnt = CInt(str(1))

        Dim rec = ObligationRequest.ORParticularEntries.FirstOrDefault(Function(p) p.IDIndex = idcnt)
        If Not rec Is Nothing Then
            rec.DataStatus = dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Update
            rec.ResposibilityCenterID = Guid.Parse(id.ToString.Replace("{", "").Replace("}", ""))
        End If
    End Sub
    Private Sub ucDiv_SelectedDivision_FundSource(id As Guid, ctrlid As String)
        Dim str() = ctrlid.Split("_")
        Dim idcnt = CInt(str(1))

        Dim rec = ObligationRequest.ORParticularEntries.FirstOrDefault(Function(p) p.IDIndex = idcnt)
        If Not rec Is Nothing Then
            rec.DataStatus = dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Update
            rec.FundSource = Guid.Parse(id.ToString.Replace("{", "").Replace("}", ""))
        End If
    End Sub
    Private Sub ucPPA_SelectedPPA(name As String, code As String, id As Guid, ctrlid As String)
        Dim str() = ctrlid.Split("_")
        Dim idcnt = CInt(str(1))

        Dim rec = ObligationRequest.ORParticularEntries.FirstOrDefault(Function(p) p.IDIndex = idcnt)
        If Not rec Is Nothing Then
            rec.DataStatus = dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Update
            rec.PPAID = Guid.Parse(id.ToString.Replace("{", "").Replace("}", ""))
        End If
    End Sub
    Private Sub ucPayeeSelection_SelectPayee(id As Guid, office As String, address As String) Handles ucPayeeSelection.SelectPayee
        txtPayeeAddress.Text = address
        txtPayeeOffice.Text = office
        ObligationRequest.PayeeID = Guid.Parse(id.ToString.Replace("{", "").Replace("}", ""))
    End Sub
    Private Sub ucParticualrTemplate_ParticularTemplateSelected(id As Guid, parttemplate As String, desc As String) Handles particularSelection.ParticularTemplateSelected
        ObligationRequest.ParticularID = id
        txtObrTemplate.Text = parttemplate
        'lblTemplate.Text = parttemplate
        lblDesc.Text = desc
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not ValidSave() Then
            ucInfoPop.ShowInfo()
        Else
            ComputeTotalAmount()
            ObligationRequest.SourceDocument = "-"
            ObligationRequest.ParticularTemplate = txtObrTemplate.Text
            ObligationRequest.AllotmentObjectClass = lblObjectClass.Text
            ObligationRequest.DateCreated = txtDateCreated.Text
            ObligationRequest.dUser = dUser
            'OLD LINE - FUND CLUSTER CODE SAVED TO FundClusterCode column of or_ObligationRequest table
            ObligationRequest.fundClusterCode = ddlFundSourceSelection.SelectedItem.Value()

            If chkPriorityTransactions.Checked = True Then
                ObligationRequest.isPrio = True
            Else
                ObligationRequest.isPrio = False
            End If

            ObligationRequest.SignatoryIdA = ucsignatorySelectionA.SelectedID
            ObligationRequest.SignatoryIdB = ucsignatorySelectionB.SelectedID
            Try
                dvdll.Master.ObligationRequest.SaveObligationRequest(ObligationRequest)
            Catch ex As Exception
                'dvdll.Master.ObligationRequest.SaveObligationRequestRegular(ObligationRequest)
                'Exit Sub
            End Try

            dvdll.Master.Particulars.updateParticularByID(ObligationRequest.ParticularID, txtObrTemplate.Text, lblDesc.Text)
            ClearObligationRequest(False)
            ucInfoPop.SetMessage("Successfully Saved Obligation Request")
            ucInfoPop.ShowInfo()
            RaiseEvent ObRFormSave()
        End If
    End Sub
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        'If ObligationRequest.Status = dvdll.Master.ObligationRequest.ObRStatus.OnQueueEdit Then
        '    dvdll.Master.ObligationRequest.UpdateOBStatus(ObligationRequest.ID, dvdll.Master.ObligationRequest.ObRStatus.OnQueue)
        'End If
        RaiseEvent ObRFormBack()
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If Not ValidSave() Then
            ucInfoPop.ShowInfo()
        Else
            ComputeTotalAmount()
            ObligationRequest.SourceDocument = "-"
            ObligationRequest.ParticularTemplate = txtObrTemplate.Text
            ObligationRequest.SignatoryIdA = ucsignatorySelectionA.SelectedID
            ObligationRequest.SignatoryIdB = ucsignatorySelectionB.SelectedID
            ObligationRequest.DateCreated = txtDateCreated.Text
            ObligationRequest.AllotmentObjectClass = lblObjectClass.Text
            ObligationRequest.fundClusterCode = ddlFundSourceSelection.SelectedItem.Value()
            dvdll.Master.ObligationRequest.UpdateObligationRequest(ObligationRequest)
            ClearObligationRequest(False)
            ucInfoPop.SetMessage("Successfully Updated Obligation Request")
            ucInfoPop.ShowInfo()
            RaiseEvent ObRFormSave()
        End If
    End Sub
#End Region

#Region "Method"
    Sub ComputeTotalAmount()
        Dim totAmt As Decimal = 0
        For Each entry In ObligationRequest.ORParticularEntries
            If Not entry.Amount Is Nothing Then
                totAmt += entry.Amount
            End If
        Next
        ObligationRequest.TotalAmount = totAmt
        txtTotalAmount.Text = totAmt.ToString("#,#00.00")
    End Sub
    Function ValidSave() As Boolean
        ucInfoPop.SetHeader("INFO")
        ucInfoPop.SetMessage("")
        'If txtsourcedocu.Text = "" Then
        '    ucInfoPop.SetMessage("Please enter souce documents information")
        '    Return False
        'End If
        If ObligationRequest.PayeeID = Nothing Then
            ucInfoPop.SetMessage("Please select a Payee.")
            Return False
        End If
        If lblObjectClass.Text = "" Then
            ucInfoPop.SetMessage("Please Enter Allotment Object Class")
            Return False
        End If
        If ucsignatorySelectionA.SelectedID = Nothing Then
            ucInfoPop.SetMessage("Please Select Signatory")
            Return False
        End If
        If ucsignatorySelectionB.SelectedID = Nothing Then
            ucInfoPop.SetMessage("Please Select Signatory")
            Return False
        End If
        If ObligationRequest.ORParticularEntries.Count < 1 Then
            ucInfoPop.SetMessage("Please add at least one transaction to proceed")
            Return False
        End If
        If ddlFundSourceSelection.SelectedItem.Value = "-1" Then
            ucInfoPop.SetMessage("Please select UACS Funding Source")
            Return False
        End If
        If txtTotalAmount.Text = "0.0" Then
            ucInfoPop.SetMessage("Please enter Total Amount.")
            Return False
        End If
        For Each entry In ObligationRequest.ORParticularEntries.Where(Function(p) p.DataStatus <> dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Delete).ToList()
            'If entry.ResposibilityCenterID = Nothing Then
            '    ucInfoPop.SetMessage("Please select a Responsibility Center to proceed at row " & getRowIdx(entry.IDIndex).ToString
            '    Return False
            'End If
            If entry.AcctID = Nothing Then
                ucInfoPop.SetMessage("Please select an Account to proceed at row " & getRowIdx(entry.IDIndex).ToString)
                Return False
            End If
            If entry.PPAID = Nothing Then
                ucInfoPop.SetMessage("Please select a PPA to proceed at row " & getRowIdx(entry.IDIndex).ToString)
                Return False
            End If
            'If If(entry.Amount, 0) <= 0 Then
            '    ucInfoPop.SetMessage("Invalid Amount value at row " & getRowIdx(entry.IDIndex).ToString & ". Only numeric values are valid.")
            '    Return False
            'End If
        Next
        Return True
    End Function
    Function getRowIdx(idCnt As Integer) As Integer
        Dim idxRet As Integer = 0
        Dim exitfor As Boolean = False
        For count As Integer = 1 To tblParticular.Rows.Count - 1
            idxRet = count
            Dim row = tblParticular.Rows(count)
            Dim ctrlcol = row.Cells(0).Controls
            For Each itm As Control In ctrlcol
                Dim str() = itm.ID.Split("_")
                Dim itmID = str(1)
                If itmID = idCnt Then
                    exitfor = True
                    Exit For
                End If
            Next
            If exitfor Then Exit For
        Next
        Return idxRet
    End Function
    Sub ClearObligationRequest(isUpdate As Boolean)
        LatestIDCount = 0
        DynamicControlIDs = New List(Of String)
        ObligationRequest = New dvdll.Master.ObligationRequest.ObligationRequestRecord
        txtPayeeAddress.Text = ""
        txtPayeeOffice.Text = ""
        txtTotalAmount.Text = "0.0"
        'txtobrParticular.Text = ""
        If isUpdate Then
            btnSave.Visible = False
            btnUpdate.Visible = True
        Else
            btnSave.Visible = True
            btnUpdate.Visible = False
        End If
    End Sub
    Private Sub InitializeTransactionLineForUpdate()
        For Each obrPart In ObligationRequest.ORParticularEntries
            Dim rec = dvdll.Master.ObligationRequest.getORParticularByID(obrPart.ID)
            If Not rec Is Nothing Then
                Dim xButton As New Button
                Dim newDivision As ctrtDivision = Page.LoadControl("~/custom controls/ctrtDivision.ascx")
                Dim newParticular As ctrlParticular = Page.LoadControl("~/custom controls/ctrlParticular.ascx")
                Dim newParticulartemplate As ctrlParticularTemplate = Page.LoadControl("~/custom controls/ctrlParticularTemplate.ascx")
                'Dim xTextBoxSource As New TextBox
                Dim newPPA As ctrlPPA = Page.LoadControl("~/custom controls/ctrlPPA.ascx")
                Dim newTravel As ctrlTravel = Page.LoadControl("~/custom controls/ctrlTravel.ascx")

                Dim xTextBoxAcctCode As New TextBox
                Dim xTextBoxAmount As New TextBox

                xTextBoxAcctCode.CssClass = "acctcodecss"
                xTextBoxAmount.CssClass = "amountcss"

                Dim newRow As New TableRow
                Dim cell0 As New TableCell
                Dim cell1 As New TableCell
                Dim cell2 As New TableCell
                Dim cell3 As New TableCell
                Dim cell4 As New TableCell
                Dim cell5 As New TableCell
                Dim cell6 As New TableCell

                LatestIDCount += 1

                xButton.ID = "btn_" & LatestIDCount.ToString
                xButton.Text = "x"
                AddHandler xButton.Click, AddressOf xButton_Click
                newDivision.ID = "ucDivision_" & LatestIDCount.ToString
                AddHandler newDivision.SelectedDivision, AddressOf ucDiv_SelectedDivision
                newParticular.ID = "ucParticular_" & LatestIDCount.ToString
                AddHandler newParticular.ParticularSelected, AddressOf ucPart_ParticularSelected

                AddHandler newParticular.TravellerDetails, AddressOf ucPart_TravellerDetails

                newParticulartemplate.ID = "ucParticularTemplate_" & LatestIDCount.ToString
                AddHandler newParticulartemplate.ParticularTemplateSelected, AddressOf ucParticualrTemplate_ParticularTemplateSelected

                'xTextBoxSource.ID = "txtSource_" & LatestIDCount.ToString
                'xTextBoxSource.AutoPostBack = True
                'AddHandler xTextBoxSource.TextChanged, AddressOf xSourceDoc_TextChanged
                newPPA.ID = "ucPPA_" & LatestIDCount.ToString
                AddHandler newPPA.SelectedPPA, AddressOf ucPPA_SelectedPPA
                xTextBoxAcctCode.ID = "txtAcct_" & LatestIDCount.ToString
                xTextBoxAmount.ID = "txtAmt_" & LatestIDCount.ToString
                xTextBoxAmount.AutoPostBack = True
                AddHandler xTextBoxAmount.TextChanged, AddressOf xAmount_TextChanged

                xTextBoxAcctCode.Text = rec.ParticularAcctCode

                xTextBoxAmount.Text = rec.Amount.Value.ToString("#,##0.00")

                'xTextBoxAmount.Text = rec.Amount.Value.ToString("#,#00.00")
                newPPA.SetPPAText(rec.PPACode)
                newDivision.SetDivisionText(rec.ResponsibilityCenterDivision)
                newParticular.SetParticularNameText(rec.ParticularAcctDesc)

                cell0.Controls.Add(xButton)
                cell1.Controls.Add(newDivision)
                cell2.Controls.Add(newParticular)
                'cell3.Controls.Add(xTextBoxSource)
                cell3.Controls.Add(newPPA)
                cell4.Controls.Add(xTextBoxAcctCode)
                cell5.Controls.Add(xTextBoxAmount)

                newRow.Cells.Add(cell0)
                newRow.Cells.Add(cell1)
                newRow.Cells.Add(cell2)
                newRow.Cells.Add(cell3)
                newRow.Cells.Add(cell4)
                newRow.Cells.Add(cell5)
                'newRow.Cells.Add(cell6)
                tblParticular.Rows.AddAt(tblParticular.Rows.Count - 1, newRow)

                DynamicControlIDs.Add(LatestIDCount.ToString)
                'newEntry.IDIndex = LatestIDCount
                Dim recUpd = ObligationRequest.ORParticularEntries.Where(Function(p) p.ID = obrPart.ID).FirstOrDefault
            If Not recUpd Is Nothing Then
                recUpd.IDIndex = LatestIDCount
            End If
            End If
        Next
    End Sub

    Private Sub UpdatingInitializeTransactionLineForUpdate()
        For Each obrPart In ObligationRequest.ORParticularEntries
            If obrPart.DataStatus <> dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Delete Then
                Dim rec = dvdll.Master.ObligationRequest.getORParticularByID(obrPart.ID)
                If Not rec Is Nothing Then
                    Dim xButton As New Button
                    Dim newDivision As ctrtDivision = Page.LoadControl("~/custom controls/ctrtDivision.ascx")
                    Dim newParticular As ctrlParticular = Page.LoadControl("~/custom controls/ctrlParticular.ascx")
                    Dim newParticulartemplate As ctrlParticularTemplate = Page.LoadControl("~/custom controls/ctrlParticularTemplate.ascx")
                    'Dim xTextBoxSource As New TextBox
                    Dim newPPA As ctrlPPA = Page.LoadControl("~/custom controls/ctrlPPA.ascx")
                    Dim newTravel As ctrlTravel = Page.LoadControl("~/custom controls/ctrlTravel.ascx")

                    Dim xTextBoxAcctCode As New TextBox
                    Dim xTextBoxAmount As New TextBox

                    xTextBoxAcctCode.CssClass = "acctcodecss"
                    xTextBoxAmount.CssClass = "amountcss"

                    Dim newRow As New TableRow
                    Dim cell0 As New TableCell
                    Dim cell1 As New TableCell
                    Dim cell2 As New TableCell
                    Dim cell3 As New TableCell
                    Dim cell4 As New TableCell
                    Dim cell5 As New TableCell
                    Dim cell6 As New TableCell

                    LatestIDCount += 1

                    xButton.ID = "btn_" & LatestIDCount.ToString
                    xButton.Text = "x"
                    AddHandler xButton.Click, AddressOf xButton_Click
                    newDivision.ID = "ucDivision_" & LatestIDCount.ToString
                    AddHandler newDivision.SelectedDivision, AddressOf ucDiv_SelectedDivision
                    newParticular.ID = "ucParticular_" & LatestIDCount.ToString
                    AddHandler newParticular.ParticularSelected, AddressOf ucPart_ParticularSelected

                    AddHandler newParticular.TravellerDetails, AddressOf ucPart_TravellerDetails

                    newParticulartemplate.ID = "ucParticularTemplate_" & LatestIDCount.ToString
                    AddHandler newParticulartemplate.ParticularTemplateSelected, AddressOf ucParticualrTemplate_ParticularTemplateSelected

                    'xTextBoxSource.ID = "txtSource_" & LatestIDCount.ToString
                    'xTextBoxSource.AutoPostBack = True
                    'AddHandler xTextBoxSource.TextChanged, AddressOf xSourceDoc_TextChanged
                    newPPA.ID = "ucPPA_" & LatestIDCount.ToString
                    AddHandler newPPA.SelectedPPA, AddressOf ucPPA_SelectedPPA
                    xTextBoxAcctCode.ID = "txtAcct_" & LatestIDCount.ToString
                    xTextBoxAmount.ID = "txtAmt_" & LatestIDCount.ToString
                    xTextBoxAmount.AutoPostBack = True
                    AddHandler xTextBoxAmount.TextChanged, AddressOf xAmount_TextChanged

                    xTextBoxAcctCode.Text = rec.ParticularAcctCode
                    xTextBoxAmount.Text = rec.Amount.Value.ToString("#,#00.00")
                    newPPA.SetPPAText(rec.PPACode)
                    newDivision.SetDivisionText(rec.ResponsibilityCenterDivision)
                    newParticular.SetParticularNameText(rec.ParticularAcctDesc)

                    cell0.Controls.Add(xButton)
                    cell1.Controls.Add(newDivision)
                    cell2.Controls.Add(newParticular)
                    'cell3.Controls.Add(xTextBoxSource)
                    cell3.Controls.Add(newPPA)
                    cell4.Controls.Add(xTextBoxAcctCode)
                    cell5.Controls.Add(xTextBoxAmount)

                    newRow.Cells.Add(cell0)
                    newRow.Cells.Add(cell1)
                    newRow.Cells.Add(cell2)
                    newRow.Cells.Add(cell3)
                    newRow.Cells.Add(cell4)
                    newRow.Cells.Add(cell5)
                    'newRow.Cells.Add(cell6)
                    tblParticular.Rows.AddAt(tblParticular.Rows.Count - 1, newRow)

                    DynamicControlIDs.Add(LatestIDCount.ToString)
                    'newEntry.IDIndex = LatestIDCount
                    Dim recUpd = ObligationRequest.ORParticularEntries.Where(Function(p) p.ID = obrPart.ID).FirstOrDefault
                    If Not recUpd Is Nothing Then
                        recUpd.IDIndex = LatestIDCount
                    End If
                End If
            End If
        Next

    End Sub
    Sub ComputeEDITTotalAmount()
        Dim totAmt As Decimal = 0
        For Each entry In ObligationRequest.ORParticularEntries
            If Not entry.Amount Is Nothing And entry.DataStatus <> dvdll.Master.ObligationRequest.ParticularEntryDataStatus.Delete Then
                totAmt += entry.Amount
            End If
        Next
        ObligationRequest.TotalAmount = totAmt
        txtTotalAmount.Text = totAmt.ToString("#,#00.00")
    End Sub

#End Region

#Region "Events"
    Event ObRFormSave()
    Event ObRFormBack()

    Public Sub ClearObRForm(isUpdate As Boolean)
        ClearObligationRequest(isUpdate)
    End Sub

    Public Sub InitializeFormForUpdate(obRID As Guid)
        'Dim new_particulars As String
        ObligationRequest = dvdll.Master.ObligationRequest.getObligationRequestRecordByID(obRID)
        ObligationRequest.ORParticularEntries.AddRange(dvdll.Master.ObligationRequest.getORParticularEntriesByORID(ObligationRequest.ID))
        InitializeTransactionLineForUpdate()
        With ObligationRequest
            '.ParticularTemplate = If(String.IsNullOrEmpty(.ORParticularEntries.First.Description), "")
            'If .ORParticularEntries.First.Description.Count.Equals(0) Then
            'new_particulars = "No Particulars (pls. edit)"
            'Else
            '    new_particulars = .ORParticularEntries.First.Description
            'End If
            .ParticularTemplate = If(.ORParticularEntries.First.Description Is Nothing, "No Particulars (pls. edit)", .ORParticularEntries.First.Description)
            '.ParticularTemplate = new_particulars
            Dim payee = dvdll.Master.Payee.getPayeeByID(.PayeeID)
            ucPayeeSelection.SetPayeeText(payee.PayeeName)
            txtObrTemplate.Text = ObligationRequest.ParticularTemplate
            'particularSelection.SetParticularText(.Template)
            txtPayeeOffice.Text = payee.Office
            txtPayeeAddress.Text = payee.Address
            txtTotalAmount.Text = .TotalAmount.ToString("#,#00.00")
            'txtsourcedocu.Text = .SourceDocument

            Dim signatoryA = dvdll.Master.Signatories.getSigByObrIDA(obRID)
            ucsignatorySelectionA.SelectedID = signatoryA.ID
            ucsignatorySelectionA.setSignatoryID(signatoryA.ID)
            ucsignatorySelectionA.SetSignatoryText(signatoryA.Name)


            Dim signatoryB = dvdll.Master.Signatories.getSigByObrIDB(obRID)
            ucsignatorySelectionB.SelectedID = signatoryB.ID
            ucsignatorySelectionB.setSignatoryID(signatoryB.ID)
            ucsignatorySelectionB.SetSignatoryText(signatoryB.Name)


            'dvdll.Master.ObligationRequest.UpdateOBStatus(ObligationRequest.ID, dvdll.Master.ObligationRequest.ObRStatus.OnQueueEdit)
        End With
        ObligationRequest.dUser = dUser
    End Sub
#End Region

    Private Sub rdbObjectClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdbObjectClass.SelectedIndexChanged

        If rdbObjectClass.SelectedIndex > -1 Then
            Dim selectedObjectClass = rdbObjectClass.SelectedItem.Value
            lblObjectClass.Text = selectedObjectClass
        Else
            lblObjectClass.Text = "None"
        End If

    End Sub

#Region "No OBR"
    Private Sub chkNoOBRNum_CheckedChanged(sender As Object, e As EventArgs) Handles chkNoOBRNum.CheckedChanged
        If chkNoOBRNum.Checked = True Then
            btnSaveObrType2.Visible = True
            btnSave.Enabled = False
        Else
            btnSaveObrType2.Visible = False
            btnSave.Enabled = True
        End If
    End Sub

    Private Sub btnSaveObrType2_Click(sender As Object, e As EventArgs) Handles btnSaveObrType2.Click
        If Not ValidSave() Then
            ucInfoPop.ShowInfo()
        Else
            ComputeTotalAmount()
            ObligationRequest.SourceDocument = "-"
            ObligationRequest.ParticularTemplate = txtObrTemplate.Text
            ObligationRequest.AllotmentObjectClass = lblObjectClass.Text

            ObligationRequest.SignatoryIdA = ucsignatorySelectionA.SelectedID
            ObligationRequest.SignatoryIdB = ucsignatorySelectionB.SelectedID
            ObligationRequest.fundClusterCode = ddlFundSourceSelection.SelectedItem.Value()
            Try
                dvdll.Master.ObligationRequest.NoObrSaveObligationRequest(ObligationRequest)
            Catch ex As Exception
                'dvdll.Master.ObligationRequest.SaveObligationRequestRegular(ObligationRequest)
                Exit Sub
            End Try

            'dvdll.Master.Particulars.updateParticularByID(ObligationRequest.ParticularID, txtObrTemplate.Text, lblDesc.Text)
            ClearObligationRequest(False)
            ucInfoPop.SetMessage("Successfully Saved Transaction")
            ucInfoPop.ShowInfo()
            RaiseEvent ObRFormSave()
        End If

    End Sub

#End Region


End Class