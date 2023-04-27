Imports System.Text
Partial Public Class Master
    Public Class DisbursementVoucher
        Public Shared _lockObject As New Object

        Enum DVStatus
            ForApproval = 1
            Cancelled = 2
            Approved = 3
            OnEdit = 4
            OnCheque = 5
        End Enum
        Enum TaxEntryDataStatus
            Add = 1
            Update = 2
            Delete = 3
        End Enum

        Public Shared Function getDVRecordByID(dvID As Guid) As DisbursementVoucherRecord
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.dv_DisbursementVoucher Where p.ID = dvID
                       Select New DisbursementVoucherRecord With {.ID = p.ID, .PayeeID = p.PayeeID, .GrossAmount = p.GrossAmount,
                                                                  .DisbursementVoucherNo = p.DisbursementVoucherNo,
                                                                  .ParticularsAmountDue = p.ParticularsAmountDue,
                                                                  .ADADate = p.ADADate, .ADANo = p.ADANo,
                                                                  .BankName = p.BankName, .CancelledBy = p.CancelledBy,
                                                                  .CancelledDate = p.CancelledDate, .CertifiedBy = p.CertifiedBy,
                                                                  .DateApprovedForPayment = p.DateApprovedForPayment,
                                                                  .DateCertified = p.DateCertified, .DateCreated = p.DateCreated,
                                                                  .DisbursementVoucherDate = p.DisbursementVoucherDate,
                                                                  .DVParticularTemplate = p.DVParticularTemplate,
                                                                  .IsCancelled = p.IsCancelled, .JEVoucherDate = p.JEVoucherDate,
                                                                  .JEVoucherNo = p.JEVoucherNo, .ModeOfPaymentID = p.ModeOfPaymentID,
                                                                  .ModeOfPaymentOthers = p.ModeOfPaymentOthers,
                                                                  .ObligationRequestID = p.ObligationRequestID, .Status = p.Status,
                                                                  .VoucherTypeID = p.VoucherTypeID, .VoucherTypeOthers = p.VoucherTypeOthers,
                                                                  .DVSignatoryA = p.DVSignatoryA, .DVSignatoryB = p.DVSignatoryB,
                                                                  .DVSignatoryC = p.DVSignatoryC, .DVSignatoryD = p.DVSignatoryD,
                                                                  .DVSignatoryE = p.DVSignatoryE, .DVSignatoryF = p.DVSignatoryF,
                                                                  .DVApprovedBy = p.DVApprovedBy, .DVPreparedBy = p.DVPreparedBy, .DVisPriority = p.isPriority}).FirstOrDefault
            Return rec
        End Function

        Public Shared Function getDVByStatus(status As Integer) As List(Of DisbursementVoucherRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of DisbursementVoucherRecord)

            lstRet = (From p In db.dv_DisbursementVoucher Where If(p.Status, 0) = status Order By p.DateCreated Descending
                      Select New DisbursementVoucherRecord With {.ID = p.ID, .DisbursementVoucherNo = p.DisbursementVoucherNo, .PayeeID = p.PayeeID, .DateCreated = p.DateCreated,
                                                                 .GrossAmount = p.GrossAmount, .ParticularsAmountDue = p.ParticularsAmountDue, .DateApprovedForPayment = p.DateApprovedForPayment, .DVisPriority = p.isPriority}).ToList

            Return lstRet
        End Function

        Public Shared Function getDVByID(dvID As Guid) As dvdll.dv_DisbursementVoucher
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.dv_DisbursementVoucher Where p.ID = dvID Select p).FirstOrDefault
            Return rec
        End Function
        ' added by cam
        Public Shared Function getDVSignatoryA(dvSigA As Guid) As List(Of dvdll.lib_Signatories)
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.lib_Signatories Where p.ID = dvSigA Select p).ToList
            Return rec
        End Function

        Public Shared Function getDVSignatoryB(dvSigB As Guid) As List(Of dvdll.lib_Signatories)
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.lib_Signatories Where p.ID = dvSigB Select p).ToList
            Return rec
        End Function

        Public Shared Function getDVSignatoryC(dvSigC As Guid) As List(Of dvdll.lib_Signatories)
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.lib_Signatories Where p.ID = dvSigC Select p).ToList
            Return rec
        End Function

        Public Shared Function getDVSignatoryD(dvSigD As Guid) As List(Of dvdll.lib_Signatories)
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.lib_Signatories Where p.ID = dvSigD Select p).ToList
            Return rec
        End Function

        Public Shared Function getDVSignatoryE(dvSigE As Guid) As List(Of dvdll.lib_Signatories)
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.lib_Signatories Where p.ID = dvSigE Select p).ToList
            Return rec
        End Function

        Public Shared Function getDVSignatoryF(dvSigF As Guid) As List(Of dvdll.lib_Signatories)
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.lib_Signatories Where p.ID = dvSigF Select p).ToList
            Return rec
        End Function

        Public Shared Function getDVCertifiedBy(dvCertifiedBy As Guid) As List(Of dvdll.lib_Signatories)
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.lib_Signatories Where p.ID = dvCertifiedBy Select p).ToList
            Return rec
        End Function

        Public Shared Function getDVApprovedBy(dvApprovedBy As Guid) As List(Of dvdll.lib_Signatories)
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.lib_Signatories Where p.ID = dvApprovedBy Select p).ToList
            Return rec
        End Function
        'end

        Public Shared Function getDVParticularEntryByDVID(dvIDPartEntry As Guid) As List(Of dvdll.dv_ParticularEntry)
            Dim db As New dvdll.dvDbEntities
            Dim lstret = (From p In db.dv_ParticularEntry Where p.DisbursementVoucherID = dvIDPartEntry Select p).ToList
            Return lstret
        End Function

        Public Shared Function getDVPPA(dvIDPartEntry As Guid) As List(Of dvdll.lib_PPA)
            Dim db As New dvdll.dvDbEntities
            Dim lstret = (From p In db.lib_PPA Where p.ID = dvIDPartEntry Select p).ToList
            Return lstret
        End Function
        Public Shared Function getDVRESPO(dvIDPartEntry As Guid) As List(Of dvdll.lib_ResponsibilityCenter)
            Dim db As New dvdll.dvDbEntities
            Dim lstret = (From p In db.lib_ResponsibilityCenter Where p.ID = dvIDPartEntry Select p).ToList
            Return lstret
        End Function
        Public Shared Function getORParticularsByORID(orID As Guid) As List(Of dvdll.or_ObligationRequestParticular)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet = (From p In db.or_ObligationRequestParticular Where p.ObligationRequestID = orID Select p).ToList
            Return lstRet
        End Function

        Public Shared Function getDVAccountEntryByDVID(dvIDAccntEntry As Guid) As List(Of dv_AccountEntry)
            Dim db As New dvdll.dvDbEntities
            Dim lstret = (From p In db.dv_AccountEntry Where p.DisbursementVoucherID = dvIDAccntEntry Select p).ToList
            If lstret.Count > 0 Then

                Dim sortedLst As New List(Of dv_AccountEntry)
                Dim lastRec = lstret.Find(Function(p) p.AccountCode = "10104040")
                Dim birrec = lstret.Find(Function(p) p.AccountCode = "412")
                Dim phealthrec = lstret.Find(Function(p) p.AccountCode = "415")
                Dim pagibigrec = lstret.Find(Function(p) p.AccountCode = "414")
                Dim gsisrec = lstret.Find(Function(p) p.AccountCode = "413")

                For Each accountitem In lstret
                    If accountitem.AccountCode <> "10104040" And accountitem.AccountCode <> "412" And accountitem.AccountCode <> "415" And accountitem.AccountCode <> "414" And accountitem.AccountCode <> "413" Then
                        sortedLst.Add(accountitem)
                    Else
                        sortedLst.Remove(accountitem)
                    End If
                Next

                If Not gsisrec Is Nothing Then
                    sortedLst.Add(gsisrec)
                End If

                If Not pagibigrec Is Nothing Then
                    sortedLst.Add(pagibigrec)
                End If

                If Not phealthrec Is Nothing Then
                    sortedLst.Add(phealthrec)
                End If

                If Not birrec Is Nothing Then
                    sortedLst.Add(birrec)
                End If

                If Not lastRec Is Nothing Then
                    sortedLst.Add(lastRec)
                End If

                Return sortedLst
            Else
                Return lstret
            End If
        End Function

        Public Shared Function getDVTaxEntryByDVID(dvIDTaxEntry As Guid) As List(Of dv_TaxEntry)
            Dim db As New dvdll.dvDbEntities
            Dim lstret = (From p In db.dv_TaxEntry Where p.DisbursementVoucherID = dvIDTaxEntry Select p Order By p.SubAccountAmount Descending).ToList
            Return lstret
        End Function

        Public Shared Function getDVTaxEntryByDVID2(dvIDTaxEntry As Guid) As List(Of TaxEntryWithATC)
            Dim db As New dvdll.dvDbEntities

            Dim lstret = (From p In db.dv_TaxEntry Join x In db.lib_Tax On p.TaxID Equals x.ID Where p.DisbursementVoucherID = dvIDTaxEntry Order By p.SubAccountAmount Descending
                   Select New TaxEntryWithATC With {.ID = p.ID, .DisbursementVoucherID = p.DisbursementVoucherID, .TaxID = p.TaxID, .TaxOutput = p.TaxOutput,
                                                    .IsSubAcct = p.IsSubAcct, .SubAccountName = p.SubAccountName, .SubAccountAmount = p.SubAccountAmount,
                                                    .Sub_AcctID = p.Sub_AcctID, .TaxATC = x.TaxATC, .TaxShortDesc = x.TaxShortDesc}).ToList
            Return lstret
        End Function

        Public Shared Function getDVTaxSubAccountByTaxEntryID(DvID As Guid) As List(Of CompleteTax)
            Dim db As New dvdll.dvDbEntities

            Dim lstRet As New List(Of CompleteTax)
            'Dim lstret = (From p In db.dv_TaxEntry Join r In db.dv_TaxSubAccount On p.ID Equals r.TaxEntryID
            '       Where p.DisbursementVoucherID = DvID Order By p.SubAccountName
            '       Select New CompleteTax With {.SubName = p.SubAccountName, .SubAmount = p.SubAccountAmount,
            '                                    .SubAcctTaxOutput = r.SubAcctTaxOutput, .TaxID = r.TaxID,
            '                                    .TaxFormula = r.TaxFormula}).ToList

            Dim taxentries = (From p In db.dv_TaxEntry Where p.DisbursementVoucherID = DvID Select p).ToList
            For Each itm As dv_TaxEntry In taxentries
                For Each subacct In itm.xTaxSubAccounts
                    Dim newItm As New CompleteTax
                    With newItm
                        .SubName = itm.SubAccountName
                        .SubAmount = itm.SubAccountAmount
                        .SubAcctTaxOutput = subacct.SubAcctTaxOutput
                        .TaxID = subacct.TaxID
                        .TaxFormula = subacct.xTaxFormula
                    End With
                    lstRet.Add(newItm)
                Next
            Next

            Return lstRet
        End Function

        Public Shared Function getTaxEntrySubAccounts(taxEntryID As Guid) As List(Of TaxEntrySubAcct)
            Dim db As New dvDbEntities
            Dim lst = (From p In db.dv_TaxSubAccount Where p.TaxEntryID = taxEntryID
                   Select New TaxEntrySubAcct With {.ID = p.ID, .DataStatus = TaxEntryDataStatus.Update,
                                                    .SubAcctTaxOutput = p.SubAcctTaxOutput, .TaxEntryID = p.TaxEntryID,
                                                    .TaxID = p.TaxID}).ToList()

            Return lst
        End Function

        Public Shared Function getTaxEntryRecordByDVID(dvID As Guid) As List(Of TaxEntry)
            Dim db As New dvDbEntities
            Dim lst = (From p In db.dv_TaxEntry Where p.DisbursementVoucherID = dvID
                       Select New TaxEntry With {.ID = p.ID, .DataStatus = TaxEntryDataStatus.Update,
                                                 .DisbursementVoucherID = p.DisbursementVoucherID,
                                                 .TaxID = p.TaxID, .TaxOutput = p.TaxOutput,
                                                 .IsSubAcct = p.IsSubAcct, .Sub_AcctID = p.Sub_AcctID,
                                                 .SubAccountAmount = p.SubAccountAmount,
                                                 .SubAccountName = p.SubAccountName}).ToList

            'Dim lst = (From p In db.dv_TaxEntry
            '           Join x In db.dv_TaxSubAccount On x.TaxEntryID Equals p.ID
            '           Where p.DisbursementVoucherID = dvID
            '         Select New TaxEntry With {.ID = p.ID, .DataStatus = TaxEntryDataStatus.Update,
            '                                   .DisbursementVoucherID = p.DisbursementVoucherID,
            '                                   .TaxID = p.TaxID, .TaxOutput = p.TaxOutput,
            '                                   .IsSubAcct = p.IsSubAcct, .Sub_AcctID = x.TaxID,
            '                                   .SubAccountAmount = p.SubAccountAmount,
            '                                   .SubAccountName = p.SubAccountName}).ToList

            If lst.Count > 0 Then
                For Each taxent In lst
                    taxent.TaxSubAcctEntries = getTaxEntrySubAccounts(taxent.ID)
                Next
            End If

            Return lst
        End Function

        Public Shared Function getDVParticularEntry(dvParticularID As Guid) As dvdll.dv_ParticularEntry
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.dv_ParticularEntry Where p.ID = dvParticularID Select p).FirstOrDefault
            Return rec
        End Function

        Public Shared Function getDVAccountEntry(dvAccountEntryID As Guid) As dvdll.dv_AccountEntry
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.dv_AccountEntry Where p.ID = dvAccountEntryID Select p).FirstOrDefault
            Return rec
        End Function

        Public Shared Function generateDVNo() As String
            Dim db As New dvdll.dvDbEntities
            Dim dvNo As String = ""
            'Dim status = DVStatus.ForApproval
            Dim CurrentOperatingYear = Master.System.GetSettingValue(System.SettingControlIDs.CurrentYear.ToString())

            If CurrentOperatingYear = Now.Year.ToString Then

                Dim startno = Master.System.GetSettingValue(System.SettingControlIDs.DVSerialNo.ToString())

                Dim latestDVRec = (From p In db.dv_DisbursementVoucher Order By p.DisbursementVoucherNo Descending Select p).FirstOrDefault
                If latestDVRec Is Nothing Then
                    dvNo = Now.Year.ToString.Substring(2, 2) & Now.Month.ToString.PadLeft(2, "0") & "-DV" & startno.PadLeft(6, "0")
                Else
                    Dim prevNo = CInt(latestDVRec.DisbursementVoucherNo.Substring(7, 6))
                    dvNo = Now.Year.ToString.Substring(2, 2) & Now.Month.ToString.PadLeft(2, "0") & "-DV" & (prevNo + 1).ToString.PadLeft(6, "0")
                End If

            Else
                NewYearStart = "1"
                Master.System.SaveSetting(System.SettingControlIDs.CurrentYear.ToString(), Now.Year.ToString())
                dvNo = Now.Year.ToString.Substring(2, 2) & Now.Month.ToString.PadLeft(2, "0") & "-DV" & NewYearStart.PadLeft(6, "0")
            End If

            Return dvNo
        End Function

        Public Shared Sub UpdateDVStatus(id As Guid, status As Integer, Optional editedBy As Guid = Nothing)
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities

                Dim rec = (From p In db.dv_DisbursementVoucher Where p.ID = id Select p).FirstOrDefault
                If Not rec Is Nothing Then
                    If rec.Status = DVStatus.OnEdit Then
                        Throw New Exception("Unable to Update Disbursement Voucher.")
                    Else
                        rec.Status = status
                    End If
                    db.SaveChanges()
                    Dim obrrec = (From p In db.or_ObligationRequest Where p.ID = rec.ObligationRequestID Select p).FirstOrDefault
                    If Not obrrec Is Nothing Then
                        obrrec.Status = dvdll.Master.ObligationRequest.ObRStatus.Verified
                        db.SaveChanges()
                    End If
                End If
            End SyncLock
        End Sub

        Public Shared Sub UpdateDVStatusToApprove(id As Guid, status As Integer, dateapproved As Date, Optional editedBy As Guid = Nothing)
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities

                Dim rec = (From p In db.dv_DisbursementVoucher Where p.ID = id Select p).FirstOrDefault
                If Not rec Is Nothing Then
                    If rec.Status = DVStatus.OnEdit Then
                        Throw New Exception("Unable to Update Disbursement Voucher.")
                    Else
                        rec.Status = status
                        rec.DateApprovedForPayment = dateapproved
                    End If
                    db.SaveChanges()

                    'Dim obrrec = (From p In db.or_ObligationRequest Where p.ID = rec.ObligationRequestID Select p).FirstOrDefault
                    'If Not obrrec Is Nothing Then
                    '    obrrec.Status = dvdll.Master.ObligationRequest.ObRStatus.Verified
                    '    db.SaveChanges()
                    'End If
                End If
            End SyncLock
        End Sub

        Public Shared Sub SaveDV(ByRef dvRecord As DisbursementVoucherRecord, accountEntries As List(Of AccountEntry))
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities
                Dim newDV As New dvdll.dv_DisbursementVoucher
                With newDV
                    .ID = Guid.NewGuid
                    .DateCreated = Format(dvRecord.DisbursementDateCreated, "dd/MM/yyyy")
                    .DisbursementVoucherNo = generateDVNo()
                    .PayeeID = dvRecord.PayeeID
                    .GrossAmount = dvRecord.GrossAmount
                    .ObligationRequestID = dvRecord.ObligationRequestID
                    .Status = DVStatus.ForApproval
                    .ModeOfPaymentID = dvRecord.ModeOfPaymentID
                    .ModeOfPaymentOthers = dvRecord.ModeOfPaymentOthers
                    .DVParticularTemplate = dvRecord.DVParticularTemplate
                    .Status = DVStatus.ForApproval
                    '.ParticularsAmountDue = dvRecord.ParticularsAmountDue
                    .isPriority = dvRecord.DVisPriority
                    Dim ChequeAmount = accountEntries.FirstOrDefault(Function(p) p.AccountCode = "10104040")
                    If Not ChequeAmount Is Nothing Then
                        .ParticularsAmountDue = ChequeAmount.Credit
                    End If
                    .DVSignatoryA = dvRecord.DVSignatoryA
                    .DVSignatoryB = dvRecord.DVSignatoryB
                    .DVSignatoryB = dvRecord.DVSignatoryB
                    .DVSignatoryC = dvRecord.DVSignatoryC
                    .DVSignatoryD = dvRecord.DVSignatoryD
                    .DVSignatoryE = dvRecord.DVSignatoryE
                    .DVSignatoryF = dvRecord.DVSignatoryF
                    .DVPreparedBy = dvRecord.DVPreparedBy
                    .DVApprovedBy = dvRecord.DVApprovedBy
                End With
                db.dv_DisbursementVoucher.Add(newDV)
                db.SaveChanges()
                dvRecord.DisbursementVoucherNo = newDV.DisbursementVoucherNo

                For Each entryPE In dvRecord.ParticularEntries
                    Dim newParticularEntry As New dvdll.dv_ParticularEntry
                    With newParticularEntry
                        .ID = Guid.NewGuid
                        .AcctID = entryPE.AcctID
                        .Amount = entryPE.Amount
                        .PPAID = entryPE.PPAID
                        .ResponsibilityCenterID = entryPE.ResponsibilityCenterID
                        .DisbursementVoucherID = newDV.ID
                        .SourceDocument = entryPE.SourceDocument
                    End With
                    db.dv_ParticularEntry.Add(newParticularEntry)
                Next
                db.SaveChanges()

                Dim tempAcctEntries As New List(Of AccountEntry)
                If dvRecord.IsAcctCashAdvance Then
                    tempAcctEntries.AddRange(accountEntries.Where(Function(p) p.AccountCode = "10104040" Or p.AccountCode = "1030502000" Or p.AccountCode = "412").ToList)
                Else
                    tempAcctEntries.AddRange(accountEntries)
                End If
                For Each entryAccount In tempAcctEntries
                    Dim newAccountEntry As New dvdll.dv_AccountEntry
                    With newAccountEntry
                        .ID = Guid.NewGuid
                        .Credit = entryAccount.Credit
                        .Debit = entryAccount.Debit
                        .DisbursementVoucherID = newDV.ID
                        .AccountCode = entryAccount.AccountCode
                        .AccountsAndExplainations = entryAccount.AcctsAndExplanation
                        .ResponsibilityCenter = entryAccount.ResponsibilityCenter
                        .Ref = entryAccount.Ref
                    End With
                    db.dv_AccountEntry.Add(newAccountEntry)
                Next
                db.SaveChanges()

                For Each entryTax In dvRecord.TaxEntries.Where(Function(p) p.DataStatus <> TaxEntryDataStatus.Delete).ToList
                    Dim newTaxEntry As New dvdll.dv_TaxEntry
                    With newTaxEntry
                        .ID = Guid.NewGuid
                        .DisbursementVoucherID = newDV.ID
                        .IsSubAcct = entryTax.IsSubAcct
                        If .IsSubAcct Then
                            .Sub_AcctID = entryTax.Sub_AcctID
                            .SubAccountAmount = entryTax.SubAccountAmount
                            .SubAccountName = entryTax.SubAccountName
                            For Each subAcct In entryTax.TaxSubAcctEntries
                                Dim newsubacct As New dvdll.dv_TaxSubAccount
                                With newsubacct
                                    .ID = Guid.NewGuid
                                    .TaxID = subAcct.TaxID
                                    .TaxEntryID = newTaxEntry.ID
                                    .SubAcctTaxOutput = subAcct.SubAcctTaxOutput
                                    If subAcct.SubAcctTaxOutput = 0 Then
                                        Continue For
                                    End If
                                End With
                                db.dv_TaxSubAccount.Add(newsubacct)
                            Next
                        Else
                            If entryTax.TaxOutput = 0 Then
                                Continue For
                            End If
                            .TaxID = entryTax.TaxID
                            .TaxOutput = entryTax.TaxOutput
                        End If
                    End With
                    db.dv_TaxEntry.Add(newTaxEntry)
                Next
                db.SaveChanges()


                Dim dbor As New dvdll.dvDbEntities
                Dim rec = (From p In dbor.or_ObligationRequest Where p.ID = newDV.ObligationRequestID Select p).FirstOrDefault
                If Not rec Is Nothing Then
                    With rec
                        .Status = ObligationRequest.ObRStatus.Approved
                    End With
                End If
                dbor.SaveChanges()


                'SYSTEM LOGS
                Dim dblog As New dvdll.dvDbEntities

                Dim newLog As New dvdll.sys_Logs
                With newLog
                    .ID = Guid.NewGuid
                    .UserLog = dvRecord.dUser
                    .LogDate = Now
                    .LogType = "CREATED"
                    .RefNo = newDV.DisbursementVoucherNo
                End With
                dblog.sys_Logs.Add(newLog)
                dblog.SaveChanges()
                'CLOSE SYSTEM LOGS

            End SyncLock


        End Sub

        Public Shared Sub UpdateDV(dvRecord As DisbursementVoucherRecord, accountEntries As List(Of AccountEntry))
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities
                Dim rec = (From p In db.dv_DisbursementVoucher Where p.ID = dvRecord.ID Select p).FirstOrDefault
                If Not rec Is Nothing Then
                    With rec
                        .DVParticularTemplate = dvRecord.DVParticularTemplate
                        .ParticularsAmountDue = dvRecord.ParticularsAmountDue
                        .DVSignatoryA = dvRecord.DVSignatoryA
                        .DVSignatoryB = dvRecord.DVSignatoryB
                        .DVSignatoryC = dvRecord.DVSignatoryC
                        .DVSignatoryD = dvRecord.DVSignatoryD
                        .DVSignatoryE = dvRecord.DVSignatoryE
                        .DVSignatoryF = dvRecord.DVSignatoryF
                        .DVPreparedBy = dvRecord.DVPreparedBy
                        .DVApprovedBy = dvRecord.DVApprovedBy
                    End With
                    db.SaveChanges()

                    Dim delAccountEntry = (From p In db.dv_AccountEntry Where p.DisbursementVoucherID = dvRecord.ID Select p).ToList
                    For Each delData In delAccountEntry
                        db.dv_AccountEntry.Remove(delData)
                    Next
                    db.SaveChanges()

                    For Each entryAccount In accountEntries
                        Dim newAccountEntry As New dvdll.dv_AccountEntry
                        With newAccountEntry
                            .ID = Guid.NewGuid
                            .Credit = entryAccount.Credit
                            .Debit = entryAccount.Debit
                            .DisbursementVoucherID = dvRecord.ID
                            .AccountCode = entryAccount.AccountCode
                            .AccountsAndExplainations = entryAccount.AcctsAndExplanation
                            .ResponsibilityCenter = entryAccount.ResponsibilityCenter
                            .Ref = entryAccount.Ref
                        End With
                        db.dv_AccountEntry.Add(newAccountEntry)
                    Next
                    db.SaveChanges()

                    Dim delTaxEntry = (From p In db.dv_TaxEntry Where p.DisbursementVoucherID = dvRecord.ID Select p).ToList
                    For Each delData In delTaxEntry
                        db.dv_TaxEntry.Remove(delData)
                    Next
                    db.SaveChanges()

                    For Each entryTax In dvRecord.TaxEntries.Where(Function(p) p.DataStatus <> TaxEntryDataStatus.Delete).ToList
                        Dim newTaxEntry As New dvdll.dv_TaxEntry
                        With newTaxEntry
                            .ID = Guid.NewGuid
                            .DisbursementVoucherID = dvRecord.ID
                            .IsSubAcct = entryTax.IsSubAcct
                            If .IsSubAcct Then
                                .Sub_AcctID = entryTax.Sub_AcctID
                                .SubAccountAmount = entryTax.SubAccountAmount
                                .SubAccountName = entryTax.SubAccountName
                                For Each subAcct In entryTax.TaxSubAcctEntries
                                    Dim newsubacct As New dvdll.dv_TaxSubAccount
                                    With newsubacct
                                        .ID = Guid.NewGuid
                                        .TaxID = subAcct.TaxID
                                        .TaxEntryID = newTaxEntry.ID
                                        .SubAcctTaxOutput = subAcct.SubAcctTaxOutput
                                        If subAcct.SubAcctTaxOutput = 0 Then
                                            Continue For
                                        End If
                                    End With
                                    db.dv_TaxSubAccount.Add(newsubacct)
                                Next
                            Else
                                If entryTax.TaxOutput = 0 Then
                                    Continue For
                                End If
                                .TaxID = entryTax.TaxID
                                .TaxOutput = entryTax.TaxOutput
                            End If
                        End With
                        db.dv_TaxEntry.Add(newTaxEntry)
                    Next
                    db.SaveChanges()
                End If

                '    For Each entryTax In dvRecord.TaxEntries
                '        Dim newTaxEntry As New dvdll.dv_TaxEntry
                '        With newTaxEntry
                '            .ID = Guid.NewGuid
                '            .DisbursementVoucherID = dvRecord.ID
                '            .IsSubAcct = entryTax.IsSubAcct
                '            If .IsSubAcct Then
                '                .SubAccountAmount = entryTax.SubAccountAmount
                '                .SubAccountName = entryTax.SubAccountName
                '                For Each subAcct In entryTax.TaxSubAcctEntries
                '                    Dim newsubacct As New dvdll.dv_TaxSubAccount
                '                    With newsubacct
                '                        .ID = Guid.NewGuid
                '                        .TaxID = subAcct.TaxID
                '                        .TaxEntryID = newTaxEntry.ID
                '                        .SubAcctTaxOutput = subAcct.SubAcctTaxOutput
                '                        If subAcct.SubAcctTaxOutput = 0 Then
                '                            Continue For
                '                        End If
                '                    End With
                '                    db.dv_TaxSubAccount.Add(newsubacct)
                '                Next
                '            Else
                '                If entryTax.TaxOutput = 0 Then
                '                    Continue For
                '                End If
                '                .TaxID = entryTax.TaxID
                '                .TaxOutput = entryTax.TaxOutput
                '            End If
                '        End With
                '        db.dv_TaxEntry.Add(newTaxEntry)
                '    Next
                '    db.SaveChanges()
                'End If


                'SYSTEM LOGS
                Dim dblog As New dvdll.dvDbEntities

                Dim newLog As New dvdll.sys_Logs
                With newLog
                    .ID = Guid.NewGuid
                    .UserLog = dvRecord.dUser
                    .LogDate = Now
                    .LogType = "EDITED"
                    .RefNo = dvRecord.DisbursementVoucherNo
                End With
                dblog.sys_Logs.Add(newLog)
                dblog.SaveChanges()
                'CLOSE SYSTEM LOGS



            End SyncLock
        End Sub

        Public Shared Function getDVPayeeByFilter(filter As String) As List(Of DisbursementVoucherRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of DisbursementVoucherRecord)

            filter = filter.Trim.ToLower
            lstRet = (From p In db.dv_DisbursementVoucher Where p.lib_Payee.PayeeName.Trim.ToLower.Contains(filter) = False _
                      Select New DisbursementVoucherRecord With {.ID = p.ID, .DateCreated = p.DateCreated, .DisbursementVoucherNo = p.DisbursementVoucherNo,
                                                                 .PayeeID = p.PayeeID, .GrossAmount = p.GrossAmount, .Status = p.Status, .ParticularsAmountDue = p.ParticularsAmountDue,
                                                                 .DateApprovedForPayment = p.DateApprovedForPayment}).ToList
            Return lstRet
        End Function

        Public Shared Function getDVByPayee(payeeName As String, Optional status As Integer = -1) As List(Of DisbursementVoucherRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of DisbursementVoucherRecord)
            If status <> -1 Then
                lstRet = (From p In db.dv_DisbursementVoucher Where p.Status = status
                          Order By p.DateCreated Descending
                      Select New DisbursementVoucherRecord With {.ID = p.ID, .DateCreated = p.DateCreated, .DisbursementVoucherNo = p.DisbursementVoucherNo,
                      .DisbursementVoucherDate = p.DisbursementVoucherDate, .ParticularsAmountDue = p.ParticularsAmountDue,
                      .PayeeID = p.PayeeID, .GrossAmount = p.GrossAmount, .Status = p.Status, .DateApprovedForPayment = p.DateApprovedForPayment, .DVisPriority = p.isPriority}).ToList
            Else
                lstRet = (From p In db.dv_DisbursementVoucher Order By p.DateCreated Descending
                      Select New DisbursementVoucherRecord With {.ID = p.ID, .DateCreated = p.DateCreated, .DisbursementVoucherNo = p.DisbursementVoucherNo,
                      .DisbursementVoucherDate = p.DisbursementVoucherDate, .ParticularsAmountDue = p.ParticularsAmountDue,
                      .PayeeID = p.PayeeID, .GrossAmount = p.GrossAmount, .Status = p.Status, .DateApprovedForPayment = p.DateApprovedForPayment, .DVisPriority = p.isPriority}).ToList
            End If
            If lstRet.Count > 0 Then
                lstRet = (From p In lstRet Where p.PayeeName.ToLower.Contains(payeeName.ToLower) Select p).ToList
            End If

            Return lstRet
        End Function

#Region "REPORT"
        Public Shared Function getDVforReport() As List(Of dv_DisbursementVoucher)
            Dim db As New dvdll.dvDbEntities
            Dim lst = (From p In db.dv_DisbursementVoucher Select p).ToList
            Return lst
        End Function

        Public Shared Function getDVTaxEntryReportOutput(dvID As Guid, grossAmt As Decimal, particularsAmtDue As Decimal) As List(Of DVTaxEntryReport)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVTaxEntryReport)

            Dim isHeader As Boolean = True
            Dim strbldr As New StringBuilder
            Dim strbldrHeader As New StringBuilder
            Dim taxEntries = (From p In db.dv_TaxEntry Where p.DisbursementVoucherID = dvID Select p).ToList
            For Each entry In taxEntries
                If entry.IsSubAcct Then
                    strbldrHeader.Append(entry.SubAccountName + "\t" + entry.SubAccountAmount.ToString("#,##0.00"))
                    strbldr.AppendLine("\t\t" + entry.TaxDescription)
                Else
                    If isHeader Then
                        strbldrHeader.AppendLine("Gross Amount:\t" + grossAmt.ToString("#,##0.00"))
                        strbldrHeader.AppendLine("Less: w/tax")
                        isHeader = False
                    End If
                    strbldr.AppendLine("  " + entry.TaxDescription + "\t" + entry.TaxOutput.ToString("#,##0.00"))
                End If
            Next

            If taxEntries.Count > 0 Then
                If Not taxEntries.First().IsSubAcct Then
                    strbldr.AppendLine("----------")
                    strbldr.AppendLine("\t\t" + particularsAmtDue.ToString("#,##0.00"))
                    strbldr.AppendLine("\t\t=========")
                Else
                    strbldrHeader.AppendLine("Less: w/tax")
                End If
            End If

            Return lstRet
        End Function
        Public Shared Function CreateReportOutput(entry As dv_TaxEntry, grossAmt As Decimal) As String
            Dim strbldr As New StringBuilder

            Return strbldr.ToString
        End Function

        Public Shared Function GetDVByDateCreated(fromdate As Date, todate As Date, status As Integer, payee As String, DvNo As String) As List(Of dv_DisbursementVoucher)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of dv_DisbursementVoucher)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)


            If status = "1" Then
                lstRet = (From p In db.dv_DisbursementVoucher
                Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.DisbursementVoucherNo.Contains(DvNo) Order By p.DisbursementVoucherNo Ascending
                Select p).ToList

                'Join r In db.dv_ObligationRequest On p.ObligationRequestID Equals r.ID
                'Join x In db.or_ObligationRequestParticular On r.ObligationRequestID Equals r.ID
            Else
                lstRet = (From p In db.dv_DisbursementVoucher
                    Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.Status = status And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.DisbursementVoucherNo.Contains(DvNo) Order By p.DisbursementVoucherNo Ascending
                    Select p).ToList
            End If
            Return lstRet
        End Function

        Public Shared Function GetWithHeldingTaxBYMonthOnChequeDV(month As String, year As String, payee As String, taxID As Guid) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = taxID
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetWithHeldingTaxBYMonthWithSubAccountOnChequeDV(month As String, year As String, payee As String, taxID As Guid) As List(Of DVINfoWithTax)

            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)

            lstRet = (From p In db.dv_TaxSubAccount
                      Join x In db.dv_TaxEntry On p.TaxEntryID Equals x.ID
                      Join y In db.dv_DisbursementVoucher On x.DisbursementVoucherID Equals y.ID
                      Where y.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And y.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval And y.DateApprovedForPayment.Value.Month = month And y.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And x.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = taxID
                      Order By y.DisbursementVoucherNo
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = x.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = y.lib_Payee.PayeeName, .DateCreated = y.DateCreated, .ApprovedDate = y.DateApprovedForPayment,
                                                     .GrossAmount = y.GrossAmount, .Status = y.Status, .TaxOutput = p.SubAcctTaxOutput, .TaxIdforLib = p.TaxID, .WithholdingTaxFormula = p.lib_Tax.TaxDescription,
                                                     .Percentage = p.lib_Tax.TaxPercentage, .SubAccountName = x.SubAccountName, .SubAccountAmount = x.SubAccountAmount, .PayeeTIN = y.lib_Payee.TIN}).ToList

            Return lstRet

        End Function

        Public Shared Function GetFARSData(fromdate As Date, todate As Date, payee As String, DvNo As String, chequeNo As String, ObRNo As String) As List(Of FARSData)

            Dim db As New dvDbEntities
            Dim lstRet As New List(Of FARSData)

            lstRet = (From p In db.dv_DisbursementVoucher
                      Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                      Join y In db.or_ObligationRequestParticular On x.ID Equals y.ObligationRequestID
                      Join z In db.ch_Cheque On p.ID Equals z.dvID
                      Join a In db.lib_PPA On y.PPAID Equals a.ID
                      Join b In db.lib_ModeOfPayment On p.ModeOfPaymentID Equals b.ID
                      Join c In db.lib_Payee On p.PayeeID Equals c.ID
                      Join d In db.lib_Account On y.AcctID Equals d.ID
                      Where z.Date >= fromdate And z.Date <= todate And c.PayeeName.Trim.ToLower.Contains(payee) And p.DisbursementVoucherNo.Contains(DvNo) And x.ObligationRequestNo.Contains(ObRNo) And z.ChequeNo.Contains(chequeNo)
                      Order By a.PPAName, c.PayeeName
                      Select New FARSData With {.DateCheque = z.Date, .DisbursementVoucherNo = p.DisbursementVoucherNo, .ObligationRequestNo = x.ObligationRequestNo, .ChequeNo = z.ChequeNo,
                                                .AllotmentObjectClass = x.AllotmentObjectClass, .PPAName = a.PPAName, .PPACode = a.PPACode, .ModeOfPayment = b.ModeOfPayment,
                                                .PayeeName = c.PayeeName, .AccountDescription = d.AccountDescription, .Amount = y.Amount, .ChequeAmount = z.Amount}).ToList
            
            Return lstRet

        End Function

        Public Shared Function GetWithHeldingTaxBYMonthOnChequeDVALL(month As String, year As String, payee As String) As List(Of dv_TaxEntry)
            Dim db As New dvDbEntities
            'Dim lstRet As New List(Of DVINfoWithTax)

            'lstRet = (From p In db.dv_TaxEntry
            '           Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
            '           And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
            '           And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee)
            '           Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
            '           Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
            '                                   .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
            '                                   .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage}).ToList
            'Return lstRet

            Dim lstRet As New List(Of dv_TaxEntry)

            lstRet = (From p In db.dv_TaxEntry
                       Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                       And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                       And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee)
                       Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending Select p).ToList

            Return lstRet

        End Function

        Public Shared Function GetWithHeldingTaxPayees(lst As List(Of dv_TaxEntry)) As List(Of Guid)
            Dim db As New dvDbEntities

            Dim PayeeIDs As List(Of Guid) = (From p In lst Select p.dv_DisbursementVoucher.PayeeID.Value).Distinct.ToList()
            Return PayeeIDs

        End Function
        Public Shared Function GetWithHeldingTaxTotalGross(lst As List(Of dv_TaxEntry), PayeeID As Guid, TaxID As Guid)
            Dim db As New dvDbEntities

            Dim totalGross = (From p In lst Where p.dv_DisbursementVoucher.PayeeID = PayeeID And p.TaxID = TaxID Select p.TaxOutput).Sum
            If totalGross Is Nothing Then
                Return 0
            Else
                Return totalGross
            End If
        End Function
        Public Shared Function TaxDummy() As List(Of MonthlyAllWithholdingTax)
            Return New List(Of MonthlyAllWithholdingTax)
        End Function

        Public Shared Function GetWithHeldingTaxBYMonthWithSubAccountOnChequeDVALL(month As String, year As String, payee As String) As List(Of DVINfoWithTax)

            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)

            lstRet = (From p In db.dv_TaxSubAccount
                      Join x In db.dv_TaxEntry On p.TaxEntryID Equals x.ID
                      Join y In db.dv_DisbursementVoucher On x.DisbursementVoucherID Equals y.ID
                      Where y.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And y.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval And y.DateApprovedForPayment.Value.Month = month And y.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And x.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee)
                      Order By y.DisbursementVoucherNo
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = x.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = y.lib_Payee.PayeeName, .DateCreated = y.DateCreated, .ApprovedDate = y.DateApprovedForPayment,
                                                     .GrossAmount = y.GrossAmount, .Status = y.Status, .TaxOutput = p.SubAcctTaxOutput, .TaxIdforLib = p.TaxID, .WithholdingTaxFormula = p.lib_Tax.TaxDescription,
                                                     .Percentage = p.lib_Tax.TaxPercentage, .SubAccountName = x.SubAccountName, .SubAccountAmount = x.SubAccountAmount}).ToList

            Return lstRet

        End Function

        <Serializable()> _
        Public Class DVTaxEntryReport
            Public Property Detail As String
        End Class


        Public Shared Function GetDVMatrix(fromdate As Date, todate As Date, status As Integer) As List(Of dv_AccountEntry)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of dv_AccountEntry)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            If status = "1" Then
                lstRet = (From p In db.dv_AccountEntry Where p.dv_DisbursementVoucher.DateCreated >= fromdate And p.dv_DisbursementVoucher.DateCreated <= todate _
                   And p.AccountCode <> "412" And p.AccountCode <> "413" And p.AccountCode <> "414" And p.AccountCode <> "415" And p.AccountCode <> "108" And p.AccountCode <> "10104040" _
                   And p.AccountCode <> "20201020" And p.AccountCode <> "20201030" And p.AccountCode <> "20201040" And p.AccountCode <> "2020103000" _
                   Order By p.AccountCode Ascending Select p).ToList
            Else
                lstRet = (From p In db.dv_AccountEntry Where p.dv_DisbursementVoucher.DateCreated >= fromdate And p.dv_DisbursementVoucher.DateCreated <= todate And p.dv_DisbursementVoucher.Status = status _
                       And p.AccountCode <> "412" And p.AccountCode <> "413" And p.AccountCode <> "414" And p.AccountCode <> "415" And p.AccountCode <> "108" And p.AccountCode <> "10104040" _
                   And p.AccountCode <> "20201020" And p.AccountCode <> "20201030" And p.AccountCode <> "20201040" And p.AccountCode <> "2020103000" _
                   Order By p.AccountCode Ascending Select p).ToList

            End If
            Return lstRet
        End Function

        'ALL TAX
        Public Shared Function GetType1TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Services EWT
            Dim tax1 = Guid.Parse("fb49344b-6a0f-46f7-a50b-1b4a0ce04033")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType2TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Real Estate Final Vat (Direct)
            Dim tax1 = Guid.Parse("1f1af2ec-5120-4b7a-8d42-2ab3344a417c")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType3TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Services EWT (Non - VAT)
            Dim tax1 = Guid.Parse("99700c98-1bbe-4a1f-a221-6385687322ca")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType4TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Services Final VAT
            Dim tax1 = Guid.Parse("136d3fd6-7183-4346-b5db-685d63f6ac56")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType5TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Real Estate EWT VAT (Direct)
            Dim tax1 = Guid.Parse("33699fc9-7e80-4c33-ab70-6bf2c8d26f2d")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType6TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Supplies EWT (Non - VAT)
            Dim tax1 = Guid.Parse("e1ce6233-4e22-4a7c-87a4-81b440b8f423")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType7TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Supplies EWT
            Dim tax1 = Guid.Parse("26544e00-32e7-41c7-b25a-919679f4676f")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType8TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Professional Fee
            Dim tax1 = Guid.Parse("d38c4473-5065-4a25-8975-a7f9ce1aae94")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType9TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Real Estate Rentals (Non-VAT) EWT
            Dim tax1 = Guid.Parse("ef71626d-6293-4106-a242-bd68dd70464b")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType10TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Supplies FINAL VAT
            Dim tax1 = Guid.Parse("95b1813b-0f91-4c22-aeb6-c07125956dba")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType11TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Services GMP
            Dim tax1 = Guid.Parse("22f6e85f-f3a9-42ee-a627-d096d2f1db25")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType12TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Supplies GMP
            Dim tax1 = Guid.Parse("79f41a49-8a48-4a70-9d04-d484a9b5bcb7")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function
        Public Shared Function GetType13TaxBYMonthOnChequeDV(month As String, year As String, payee As String) As List(Of DVINfoWithTax)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of DVINfoWithTax)
            'Real Estate Rentals (VAT) EWT
            Dim tax1 = Guid.Parse("8ac7860b-fcce-4ebe-b80c-fbb7bf2b779b")

            lstRet = (From p In db.dv_TaxEntry
                      Where p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.Cancelled And p.dv_DisbursementVoucher.Status <> dvdll.Master.DisbursementVoucher.DVStatus.ForApproval _
                      And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Month = month And p.dv_DisbursementVoucher.DateApprovedForPayment.Value.Year = year _
                      And p.TaxID IsNot Nothing And p.dv_DisbursementVoucher.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.lib_Tax.ID = tax1
                      Order By p.dv_DisbursementVoucher.DisbursementVoucherNo Ascending
                      Select New DVINfoWithTax With {.ID = p.ID, .DisbursementVoucherNo = p.dv_DisbursementVoucher.DisbursementVoucherNo, .Payeename = p.dv_DisbursementVoucher.lib_Payee.PayeeName,
                                              .DateCreated = p.dv_DisbursementVoucher.DateCreated, .ApprovedDate = p.dv_DisbursementVoucher.DateApprovedForPayment, .GrossAmount = p.dv_DisbursementVoucher.GrossAmount, .Status = p.dv_DisbursementVoucher.Status,
                                              .TaxOutput = p.TaxOutput, .WithholdingTaxFormula = p.lib_Tax.TaxDescription, .Percentage = p.lib_Tax.TaxPercentage, .PayeeTIN = p.dv_DisbursementVoucher.lib_Payee.TIN}).ToList
            Return lstRet
        End Function

#End Region

        <Serializable()>
        Public Class DisbursementVoucherRecord
            Public Property ID As Guid
            Public Property ObligationRequestID As Guid
            Public Property DisbursementVoucherNo As String
            Public Property DisbursementVoucherDate As Nullable(Of Date)
            Public Property DisbursementDateCreated As Nullable(Of Date)
            Public Property ModeOfPaymentID As Guid
            Public Property GrossAmount As Nullable(Of Decimal)
            Public Property ParticularsAmountDue As Nullable(Of Decimal)
            Public Property CertifiedBy As Nullable(Of Guid)
            Public Property DateCertified As Nullable(Of Date)
            Public Property ApprovedForPaymentBy As Nullable(Of Guid)
            Public Property DateApprovedForPayment As Nullable(Of Date)
            Public Property ReceivedPaymentDate As Nullable(Of Date)
            Public Property ADANo As String
            Public Property ADADate As Nullable(Of Date)
            Public Property BankName As String
            Public Property ORNo As String
            Public Property JEVoucherNo As String
            Public Property JEVoucherDate As Nullable(Of Date)
            Public Property VoucherTypeID As Nullable(Of Guid)
            Public Property VoucherTypeOthers As String
            Public Property PreparedBy As Nullable(Of Guid)
            Public Property ApprovedBy As Nullable(Of Guid)
            Public Property ModeOfPaymentOthers As String
            Public Property IsCancelled As Nullable(Of Boolean)
            Public Property CancelledDate As Nullable(Of Date)
            Public Property CancelledBy As Nullable(Of Guid)
            Public Property Status As Integer
            Public Property DateCreated As Nullable(Of Date)
            Public Property PayeeID As Nullable(Of Guid)
            Public Property DVParticularTemplate As String
            Public Property DVSignatoryA As Nullable(Of Guid)
            Public Property DVSignatoryB As Nullable(Of Guid)
            Public Property DVSignatoryC As Nullable(Of Guid)
            Public Property DVSignatoryD As Nullable(Of Guid)
            Public Property DVSignatoryE As Nullable(Of Guid)
            Public Property DVSignatoryF As Nullable(Of Guid)
            Public Property DVPreparedBy As Nullable(Of Guid)
            Public Property DVApprovedBy As Nullable(Of Guid)
            Public Property IsAcctCashAdvance As Boolean

            Public Property DVisPriority As Nullable(Of Boolean)
            Public Property dUser As String


            Private _ObRNo As String
            Private _ObRDateCreatedx As String
            Private _ObrAllotmentx As String

            Public Property ModeOfPayment As String
            Public Property ObRNo As String
                Get
                    If String.IsNullOrEmpty(_ObRNo) Or String.IsNullOrWhiteSpace(_ObRNo) Then
                        Dim db As New dvdll.dvDbEntities
                        Dim rec = (From p In db.or_ObligationRequest Where p.ID = ObligationRequestID Select p).FirstOrDefault
                        If Not rec Is Nothing Then
                            Return rec.ObligationRequestNo
                        End If
                        Return ""
                    Else
                        Return _ObRNo
                    End If
                End Get
                Set(value As String)
                    _ObRNo = value
                End Set
            End Property
            Public Property ObRDateCreated As String
                Get
                    If String.IsNullOrEmpty(_ObRDateCreatedx) Or String.IsNullOrWhiteSpace(_ObRDateCreatedx) Then
                        Dim db As New dvdll.dvDbEntities
                        Dim rec = (From p In db.or_ObligationRequest Where p.ID = ObligationRequestID Select p).FirstOrDefault
                        If Not rec Is Nothing Then
                            Return rec.DateCreated.Value.ToShortDateString
                        End If
                        Return ""
                    Else
                        Return _ObRDateCreatedx
                    End If
                End Get
                Set(value As String)
                    _ObRDateCreatedx = value
                End Set
            End Property

            Public Property ObRAllotmentObjectClass As String
                Get
                    If String.IsNullOrEmpty(_ObrAllotmentx) Or String.IsNullOrWhiteSpace(_ObrAllotmentx) Then
                        Dim db As New dvdll.dvDbEntities
                        Dim rec = (From p In db.or_ObligationRequest Where p.ID = ObligationRequestID Select p).FirstOrDefault
                        If Not rec Is Nothing Then
                            Return rec.AllotmentObjectClass
                        End If
                        Return ""
                    Else
                        Return _ObrAllotmentx
                    End If
                End Get
                Set(value As String)
                    _ObrAllotmentx = value
                End Set
            End Property

            Public Property PayeesTin As String

            Public Property TaxEntries As List(Of TaxEntry)
            Public Property ParticularEntries As List(Of ParticularEntry)
            Public Property AccounEntries As List(Of AccountEntry)
            Public Property TaxEntrySubAccts As List(Of TaxEntrySubAcct)

            Public ReadOnly Property PayeeName As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_Payee Where p.ID = PayeeID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.PayeeName
                    End If
                    Return ""
                End Get
            End Property
            Public ReadOnly Property PayeeAddress As String
                Get
                    If Not PayeeID Is Nothing Then
                        Dim db As New dvDbEntities
                        Dim rec = (From p In db.lib_Payee Where p.ID = PayeeID Select p).FirstOrDefault
                        If Not rec Is Nothing Then
                            Return rec.Address
                        End If
                    End If
                    Return ""
                End Get
            End Property
            Public ReadOnly Property PayeeOffice As String
                Get
                    If Not PayeeID Is Nothing Then
                        Dim db As New dvDbEntities
                        Dim rec = (From p In db.lib_Payee Where p.ID = PayeeID Select p).FirstOrDefault
                        If Not rec Is Nothing Then
                            Return rec.Office
                        End If
                    End If
                    Return ""
                End Get
            End Property
            Public ReadOnly Property PayeeTIN As String
                Get
                    If Not PayeeID Is Nothing Then
                        Dim db As New dvDbEntities
                        Dim rec = (From p In db.lib_Payee Where p.ID = PayeeID Select p).FirstOrDefault
                        If Not rec Is Nothing Then
                            Return rec.TIN
                        End If
                    End If
                    Return ""
                End Get
            End Property
            Public ReadOnly Property PayeeEmail As String
                Get
                    If Not PayeeID Is Nothing Then
                        Dim db As New dvDbEntities
                        Dim rec = (From p In db.lib_Payee Where p.ID = PayeeID Select p).FirstOrDefault
                        If Not rec Is Nothing Then
                            Return rec.EmailAddress
                        End If
                    End If
                    Return ""
                End Get
            End Property


            Sub New()
                TaxEntries = New List(Of TaxEntry)
                ParticularEntries = New List(Of ParticularEntry)
                TaxEntrySubAccts = New List(Of TaxEntrySubAcct)
            End Sub
        End Class
        <Serializable()> _
        Public Class TaxEntry
            Public Property ID As Guid
            Public Property IDIndex As Integer
            Public Property DisbursementVoucherID As Guid
            Public Property TaxID As Nullable(Of Guid)
            Public Property TaxOutput As Nullable(Of Decimal)
            Public Property DataStatus As Integer
            Public Property IsSubAcct As Boolean
            Public Property SubAccountName As String
            Public Property SubAccountAmount As Nullable(Of Decimal)
            Public Property Sub_AcctID As Nullable(Of Guid)
            Public Property TaxATC As String
            Public Property TaxSubAcctEntries As List(Of TaxEntrySubAcct)
            Public ReadOnly Property TaxDescription As String
                Get
                    Dim db As New dvDbEntities
                    Dim rec = (From p In db.lib_Tax Where p.ID = TaxID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.TaxShortDesc
                    End If
                    Return ""
                End Get
            End Property
            Public ReadOnly Property SubAcctEntriesTaxTotal As Decimal
                Get
                    Dim total As Decimal = 0
                    If TaxSubAcctEntries.Count > 0 Then
                        For Each entry In TaxSubAcctEntries
                            total += entry.SubAcctTaxOutput
                        Next
                    End If
                    Return total
                End Get
            End Property
            Sub New()
                TaxSubAcctEntries = New List(Of TaxEntrySubAcct)
            End Sub
        End Class
        <Serializable()> _
        Public Class TaxEntrySubAcct
            Public Property ID As Guid
            Public Property IDIndex As Integer
            Public Property TaxEntryID As Guid
            Public Property TaxID As Guid
            Public Property SubAcctTaxOutput As Decimal
            Public Property DataStatus As Integer

            Public ReadOnly Property TaxDescription As String
                Get
                    Dim db As New dvDbEntities
                    Dim rec = (From p In db.lib_Tax Where p.ID = TaxID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.TaxShortDesc
                    End If
                    Return ""
                End Get
            End Property

            Public Property TaxFormula As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_Tax Where p.ID = TaxID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.TaxShortDesc
                    End If
                    Return ""
                End Get
                Set(value As String)
                    rec = value
                End Set
            End Property
            Sub New()
            End Sub
        End Class
        <Serializable()> _
        Public Class ParticularEntry
            Public Property ID As Guid
            Public Property AcctID As Guid
            Public Property Amount As Decimal
            Public Property PPAID As Guid
            Public Property ResponsibilityCenterID As Guid
            Public Property DisbursementVoucherID As Guid
            Public Property SourceDocument As String

            Public ReadOnly Property AccountCode As String
                Get
                    Dim db As New dvDbEntities
                    Dim rec = (From p In db.lib_Account Where p.ID = AcctID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.AccountCode
                    End If
                    Return ""
                End Get
            End Property
            Public ReadOnly Property AccountDescAndExpl As String
                Get
                    Dim db As New dvDbEntities
                    Dim rec = (From p In db.lib_Account Where p.ID = AcctID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.AccountDescription
                    End If
                    Return ""
                End Get
            End Property
            Public ReadOnly Property ResponsibilityCenter As String
                Get
                    Dim db As New dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = ResponsibilityCenterID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Sub New()
            End Sub
        End Class
        <Serializable()> _
        Public Class AccountEntry
            Public Property ID As Guid
            Public Property AcctID As Guid
            Public Property ResponsibilityCenter As String
            Public Property AcctsAndExplanation As String
            Public Property AccountCode As String
            Public Property Ref As String
            Public Property Debit As Decimal
            Public Property Credit As Decimal
        End Class
        <Serializable()> _
        Public Class DebitCreditEntry
            Public Property ID As Guid
            Public Property AcctID As Guid
            Public Property Debit As Decimal
            Public Property Credit As Decimal
            Public Property ReferenceID As Guid
            Public Property Status As Integer
            Sub New()
                Debit = 0
                Credit = 0
            End Sub
        End Class
        <Serializable()> _
        Public Class FARSData
            Public Property DateCheque As Date
            Public Property DisbursementVoucherNo As String
            Public Property ObligationRequestNo As String
            Public Property ChequeNo As String
            Public Property AllotmentObjectClass As String
            Public Property PPAName As String
            Public Property PPACode As String
            Public Property ModeOfPayment As String
            Public Property PayeeName As String
            Public Property AccountDescription As String
            Public Property Amount As String
            Public Property ChequeAmount As String
        End Class


#Region "For Reports"
        <Serializable()> _
        Public Class CompleteTax
            Public Property ID As Guid
            Public Property IDIndex As Integer
            Public Property TaxEntryID As Guid
            Public Property TaxID As Guid
            Public Property SubAcctTaxOutput As Decimal
            Public Property DataStatus As Integer
            Public Property TaxFormula As String
            Public Property SubName As String
            Public Property SubAmount As Decimal
            Sub New()
            End Sub
        End Class

        <Serializable()> _
        Public Class DVINfoWithTax
            Public Property ID As Guid
            Public Property ObligationRequestID As Guid
            Public Property DisbursementVoucherNo As String
            Public Property DisbursementVoucherDate As Nullable(Of Date)

            Public Property GrossAmount As Nullable(Of Decimal)
            Public Property ParticularsAmountDue As Nullable(Of Decimal)

            Public Property CancelledDate As Nullable(Of Date)

            Public Property Status As Integer
            Public Property DateCreated As Nullable(Of Date)
            Public Property PayeeID As Nullable(Of Guid)
            Public Property Payeename As String
            Public Property PayeeTIN As String
            Public Property DVParticularTemplate As String
            Public Property DisbursementVoucherID As Guid
            Public Property TaxOutput As Decimal
            Public Property IsSubAcct As Boolean
            Public Property SubAccountName As String
            Public Property SubAccountAmount As Nullable(Of Decimal)
            Public Property Sub_AcctID As Nullable(Of Guid)
            Public Property TaxSubAcctEntries As List(Of TaxEntrySubAcct)
            Public Property WithholdingTaxFormula As String
            Public Property TaxIdforLib As Guid
            Public Property Percentage As String
            Public Property ApprovedDate As Nullable(Of Date)

            Public Property TaxEntryID As Guid
            Public Property TaxID As Guid
            Public Property SubAcctTaxOutput As Decimal
            Public Property DataStatus As Integer
        End Class
        'TaxWithATC
        Public Class TaxEntryWithATC
            Public Property ID As Guid
            Public Property DisbursementVoucherID As Nullable(Of Guid)
            Public Property TaxID As Nullable(Of Guid)
            Public Property TaxOutput As Nullable(Of Decimal)
            Public Property IsSubAcct As Nullable(Of Boolean)
            Public Property SubAccountName As String
            Public Property SubAccountAmount As Nullable(Of Decimal)
            Public Property Sub_AcctID As Nullable(Of Guid)
            Public Property TaxATC As String
            Public Property TaxShortDesc As String
        End Class

        <Serializable()> _
        Public Class DVMatrix
            Public Property ID As Guid
            Public Property DisbursementVoucherID As Guid
            Public Property DisbursementVoucherNo As String
            Public Property DisbursementDateCreated As Nullable(Of Date)
            Public Property Status As Integer

            Public Property AccountsExplanation As String
            Public Property AccountsCode

            Public Property TotalDebit As Decimal
            Public Property TotalCredit As Decimal
        End Class

        'MonthlyWithholdingTax
        Public Class MonthlyAllWithholdingTax
            Public Property PayeeList As String
            Public Property TaxDesc As String


            Public Property Col1ID As Guid
            Public ReadOnly Property Col1Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_Tax Where p.ID = Col1ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.TaxDescription
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col1Data As Decimal

        End Class

#End Region



    End Class
End Class