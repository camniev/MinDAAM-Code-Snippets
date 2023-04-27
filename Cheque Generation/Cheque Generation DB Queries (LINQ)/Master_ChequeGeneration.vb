
Partial Public Class Master
    Public Class ChequeGeneration
        Public Shared _lockObject As New Object
        Enum ChequeStatus
            Created = 1
            Cleared = 2
            Canceled = 3
            OnEdit = 4
        End Enum

        Public Shared Function getChRecordByID(ChID As Guid) As ChequeGenerationRecord
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.ch_Cheque Where p.ID = ChID Select New ChequeGenerationRecord With {
                     .ID = p.ID, .Amount = p.Amount, .AmountInWords = p.AmountInWords, .ChequeDate = p.Date, .ChequeNo = p.ChequeNo,
                     .DataCanceled = p.DateCanceled, .DateCreated = p.DateCreated, .PayToOrder = p.PayToOrder, .Remarks = p.Remarks, .AllotmentClass = p.AllotmentObjectClass,
                     .ChSignatoryA = p.ChSignatoryA, .ChsignatoryB = p.ChSignatoryB}).FirstOrDefault
            Return rec
        End Function

        Public Shared Function getChIDtoPrint(ChID As Guid) As dvdll.ch_Cheque
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.ch_Cheque Where p.ID = ChID Select p).FirstOrDefault
            Return rec
        End Function

        Public Shared Function getCheques() As List(Of ch_Cheque)
            Dim db As New dvDbEntities
            Dim lst = (From p In db.ch_Cheque Select p).ToList
            Return lst
        End Function

        Public Shared Function getChByStatus(status As Integer) As List(Of ChequeGenerationRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstret As New List(Of ChequeGenerationRecord)

            'lstret = (From p In db.ch_Cheque Where If(p.Status, 0) = status Order By p.DateCreated Descending
            '    Select New ChequeGenerationRecord With {.ID = p.ID, .ChequeNo = p.ChequeNo, .PayToOrder = p.PayToOrder,
            '                                            .Amount = p.Amount, .AmountInWords = p.AmountInWords, .ChequeDate = p.Date, .DateCreated = p.DateCreated,
            '                                            .AllotmentClass = p.AllotmentObjectClass}).ToList
            lstret = (From p In db.ch_Cheque Where p.Status = status Order By p.DateCreated Descending
               Select New ChequeGenerationRecord With {.ID = p.ID, .ChequeNo = p.ChequeNo, .PayToOrder = p.PayToOrder,
                                                       .Amount = p.Amount, .AmountInWords = p.AmountInWords, .ChequeDate = p.Date, .DateCreated = p.DateCreated,
                                                       .AllotmentClass = p.AllotmentObjectClass, .CHisPriority = p.isPriority}).ToList

            Return lstret


        End Function

        Public Shared Function getCHPayeeByFilter(filter As String) As List(Of ChequeGenerationRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of ChequeGenerationRecord)

            lstRet = (From p In db.ch_Cheque Where p.PayToOrder.Contains(filter) And p.Status <> ChequeStatus.Canceled Order By p.Date
                Select New ChequeGenerationRecord With {.ID = p.ID, .DateCreated = p.DateCreated, .ChequeNo = p.ChequeNo, .ChequeDate = p.Date, _
                                                                 .PayToOrder = p.PayToOrder, .Amount = p.Amount, .Status = p.Status}).ToList
            Return lstRet
        End Function

        Public Shared Function SaveCheque(chequeno As String, dateissued As String, amount As String,
                                     amountwords As String, paytoorder As String, dvId As Guid, allotment As String, sigA As Guid, sigB As Guid, isItPrio As Boolean) As ch_Cheque
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities
                Dim newCh As New dvdll.ch_Cheque
                With newCh
                    .ID = Guid.NewGuid
                    .ChequeNo = chequeno
                    .Status = ChequeStatus.Created
                    .Date = dateissued
                    .DateCreated = Now
                    .Amount = amount
                    .AmountInWords = amountwords
                    .PayToOrder = paytoorder
                    .dvID = dvId
                    .ChSignatoryA = sigA
                    .ChSignatoryB = sigB
                    .isPriority = isItPrio
                    If allotment = "100" Then
                        .AllotmentObjectClass = "50100000-00"
                    ElseIf allotment = "200" Then
                        .AllotmentObjectClass = "50200000-00"
                    Else
                        .AllotmentObjectClass = "50600000-00"
                    End If
                    '.AllotmentObjectClass = allotment
                End With
                db.ch_Cheque.Add(newCh)
                db.SaveChanges()
                Return newCh
            End SyncLock
        End Function

        Public Shared Sub UpdateCheque(chid As Guid, chequenoEdit As String, dateissuedEdit As String, signA As Guid, signB As Guid)

            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities

                Dim rec = (From p In db.ch_Cheque Where p.ID = chid Select p).FirstOrDefault
                If Not rec Is Nothing Then
                    rec.ChequeNo = chequenoEdit
                    rec.Date = dateissuedEdit
                End If
                db.SaveChanges()

            End SyncLock
        End Sub

        Public Shared Sub CancelCheque(id As Guid, canceldate As String, remarks As String)
            Dim db As New dvDbEntities

            Dim item = (From p In db.ch_Cheque Where p.ID = id Select p).FirstOrDefault
            If Not item Is Nothing Then
                With item
                    .DateCanceled = canceldate
                    .Remarks = remarks
                    .Status = dvdll.Master.ChequeGeneration.ChequeStatus.Canceled
                End With
                db.SaveChanges()
            End If

            Dim canceledDV = (From p In db.ch_Cheque Where p.ID = id Select p.dvID).FirstOrDefault
            Dim theDVrec = (From x In db.dv_DisbursementVoucher Where x.ID = canceledDV Select x).FirstOrDefault
            If Not theDVrec Is Nothing Then
                With theDVrec
                    .Status = dvdll.Master.DisbursementVoucher.DVStatus.Cancelled
                    .CancelledDate = canceldate
                End With
                db.SaveChanges()
            End If

            Dim CanceledOBR = (From p In db.dv_DisbursementVoucher Where p.ID = canceledDV Select p.ObligationRequestID).FirstOrDefault
            Dim theOBRrec = (From y In db.or_ObligationRequest Where y.ID = CanceledOBR Select y).FirstOrDefault
            If Not theOBRrec Is Nothing Then
                With theOBRrec
                    .Status = dvdll.Master.ObligationRequest.ObRStatus.Cancelled
                    .DateCancelled = canceldate
                End With
                db.SaveChanges()
            End If

           

        End Sub

        Public Shared Sub ChequeBacktoDV(id As Guid, canceldate As String, remarks As String)
            Dim db As New dvdll.dvDbEntities

            Dim item = (From p In db.ch_Cheque Where p.ID = id Select p).FirstOrDefault
            If Not item Is Nothing Then
                With item
                    .DateCanceled = canceldate
                    .Remarks = remarks
                    .Status = ChequeStatus.Canceled
                End With
                db.SaveChanges()
            End If

            Dim canceledDV = (From p In db.ch_Cheque Where p.ID = id Select p.dvID).FirstOrDefault
            Dim theDVrec = (From x In db.dv_DisbursementVoucher Where x.ID = canceledDV Select x).FirstOrDefault
            If Not theDVrec Is Nothing Then
                With theDVrec
                    .Status = dvdll.Master.DisbursementVoucher.DVStatus.ForApproval
                End With
                db.SaveChanges()
            End If

        End Sub

        Public Shared Sub CancelDV(id As Guid, canceldate As String, remarks As String)
            Dim db As New dvdll.dvDbEntities

            Dim item = (From p In db.ch_Cheque Where p.ID = id Select p).FirstOrDefault
            If Not item Is Nothing Then
                With item
                    .DateCanceled = canceldate
                    .Remarks = remarks
                    .Status = ChequeStatus.Canceled
                End With
                db.SaveChanges()
            End If

            Dim canceledDV = (From p In db.ch_Cheque Where p.ID = id Select p.dvID).FirstOrDefault
            Dim theDVrec = (From x In db.dv_DisbursementVoucher Where x.ID = canceledDV Select x).FirstOrDefault
            If Not theDVrec Is Nothing Then
                With theDVrec
                    .Status = dvdll.Master.DisbursementVoucher.DVStatus.Cancelled
                    .CancelledDate = canceldate
                End With
                db.SaveChanges()
            End If

            Dim ReturnOBR = (From p In db.dv_DisbursementVoucher Where p.ID = canceledDV Select p.ObligationRequestID).FirstOrDefault
            Dim theOBRrec = (From y In db.or_ObligationRequest Where y.ID = ReturnOBR Select y).FirstOrDefault
            If Not theOBRrec Is Nothing Then
                With theOBRrec
                    .Status = dvdll.Master.ObligationRequest.ObRStatus.Verified
                End With
                db.SaveChanges()
            End If


        End Sub


        Public Shared Sub UpdateChequeStatus(id As Guid, status As Integer, Optional editedBy As Guid = Nothing)
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities

                Dim rec = (From p In db.ch_Cheque Where p.ID = id Select p).FirstOrDefault
                If Not rec Is Nothing Then
                        rec.Status = status
                End If

                db.SaveChanges()
            End SyncLock
        End Sub

#Region "REPORT"
        Public Shared Function getChforReport() As List(Of ch_Cheque)
            Dim db As New dvdll.dvDbEntities
            Dim lst = (From p In db.ch_Cheque Select p).ToList
            Return lst
        End Function

        Public Shared Function GetChequeList(fromdate As Date, status As Integer) As List(Of ChequeIssued)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ChequeIssued)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)

            If status = "1" Then
                'lstRet = (From p In db.ch_Cheque
                'Where p.Date = fromdate Order By p.ChequeNo Ascending
                'Select p).ToList
                lstRet = (From p In db.ch_Cheque
                          Join x In db.dv_DisbursementVoucher On p.dvID Equals x.ID
                          Join y In db.or_ObligationRequest On x.ObligationRequestID Equals y.ID
                          Join z In db.or_ObligationRequestParticular On y.ID Equals z.ObligationRequestID
                          Where p.Date = fromdate Order By p.ChequeNo Ascending
                          Select New ChequeIssued With {.ID = p.ID, .ChequeNo = p.ChequeNo, .PayToOrder = p.PayToOrder,
                                                        .Amount = p.Amount, .AmountInWords = p.AmountInWords, .ChequeDate = p.Date, .DateCreated = p.DateCreated,
                                                        .AllotmentClass = p.AllotmentObjectClass, .Description = x.DVParticularTemplate, .AccountCode = z.lib_Account.AccountCode, .PAP = z.lib_PPA.PPACode, .GrossAmount = z.Amount, .Division = z.lib_ResponsibilityCenter.Division}).ToList

            Else
                lstRet = (From p In db.ch_Cheque
                          Join x In db.dv_DisbursementVoucher On p.dvID Equals x.ID
                          Join y In db.or_ObligationRequest On x.ObligationRequestID Equals y.ID
                          Join z In db.or_ObligationRequestParticular On y.ID Equals z.ObligationRequestID
                          Where p.Date = fromdate Order By p.ChequeNo Ascending
                          Select New ChequeIssued With {.ID = p.ID, .ChequeNo = p.ChequeNo, .PayToOrder = p.PayToOrder,
                                                        .Amount = p.Amount, .AmountInWords = p.AmountInWords, .ChequeDate = p.Date, .DateCreated = p.DateCreated,
                                                        .AllotmentClass = p.AllotmentObjectClass, .Description = x.DVParticularTemplate, .AccountCode = z.lib_Account.AccountCode, .PAP = z.lib_PPA.PPACode, .GrossAmount = z.Amount, .Division = z.ResponsibilityCenterDivision}).ToList

            End If
            Return lstRet
        End Function

        Public Shared Function GetChByDateCreated(fromdate As Date, todate As Date, status As Integer, payee As String, ChNo As String) As List(Of ch_Cheque)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ch_Cheque)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            If status = "1" Then
                'lstRet = (From p In db.ch_Cheque
                'Where p.Date >= fromdate And p.Date <= todate And p.PayToOrder.Trim.ToLower.Contains(payee) And p.ChequeNo.Contains(ChNo) Order By p.Date Ascending
                'Select p).ToList
                If payee = "" Then
                    lstRet = (From p In db.ch_Cheque
            Where p.Date >= fromdate And p.Date <= todate And p.ChequeNo.Contains(ChNo) Order By p.Date Ascending
            Select p).ToList
                Else
                    lstRet = (From p In db.ch_Cheque
           Where p.Date >= fromdate And p.Date <= todate And p.ChequeNo.Contains(ChNo) And p.PayToOrder.Contains(payee.Trim.ToLower) Order By p.Date Ascending
           Select p).ToList
                End If

            Else
                If payee = "" Then
                    lstRet = (From p In db.ch_Cheque
                        Where p.Date >= fromdate And p.Date <= todate And p.Status = status And p.ChequeNo.Contains(ChNo) Order By p.Date Ascending
                        Select p).ToList
                Else
                    lstRet = (From p In db.ch_Cheque
                       Where p.Date >= fromdate And p.Date <= todate And p.Status = status And p.ChequeNo.Contains(ChNo) And p.PayToOrder.Contains(payee.Trim.ToLower) Order By p.Date Ascending
                       Select p).ToList
                End If
            End If
            Return lstRet
        End Function

        Public Shared Function GetAllCHQTransactions(payee As String, fromdate As Date) As List(Of ChequeIssued)
            Dim db As New dvDbEntities
            Dim lstOBR As New List(Of ChequeIssued)
            Dim lstDV As New List(Of ChequeIssued)
            Dim lstCHQ As New List(Of ChequeIssued)
            Dim lstRet As New List(Of ChequeIssued)
            Dim lstEmpty As New List(Of ChequeIssued)


            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)

            lstRet = (From p In db.ch_Cheque Where p.PayToOrder.Contains(payee.Trim.ToLower)
                Select New ChequeIssued With {.ChequeNo = p.ChequeNo, .Amount = p.Amount, .DateCreated = p.DateCreated}).ToList

            Return lstRet
                  
        End Function
        Public Shared Function GetAllDVTransactions(payee As String, fromdate As Date) As List(Of ChequeIssued)
            Dim db As New dvDbEntities
            Dim lstOBR As New List(Of ChequeIssued)
            Dim lstDV As New List(Of ChequeIssued)
            Dim lstCHQ As New List(Of ChequeIssued)
            Dim lstRet As New List(Of ChequeIssued)
            Dim lstEmpty As New List(Of ChequeIssued)


            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)

           
                lstDV = (From x In db.or_ObligationRequest
                         Where x.lib_Payee.PayeeName.Trim.ToLower.Contains(payee)
                         Join y In db.dv_DisbursementVoucher On x.ID Equals y.ObligationRequestID
                         Where y.lib_Payee.PayeeName.Trim.ToLower.Contains(payee)
                         Select New ChequeIssued With {.Name = x.lib_Payee.PayeeName,
                                          .AllotmentClass = x.AllotmentObjectClass,
                                          .ObrNo = x.ObligationRequestNo, .ObrDateCancelled = x.DateCancelled, .ObrDateCreated = x.DateCreated, .ObrTotal = x.TotalAmount, .ObrStatus = x.Status,
                                          .DVNo = y.DisbursementVoucherNo, .DVDateCancelled = y.CancelledDate, .DVDateCreated = y.DateCreated, .DVTotal = y.ParticularsAmountDue, .DVStatus = y.Status}).ToList
            For Each item In lstDV
                If item.ObrStatus = "1" Then
                    item.ObrStatus = "Created"
                ElseIf item.ObrStatus = "2" Then
                    item.ObrStatus = "Verified"
                ElseIf item.ObrStatus = "3" Then
                    item.ObrStatus = "Cancelled"
                ElseIf item.ObrStatus = "6" Then
                    item.ObrStatus = "DV Created"
                End If

            Next


            For Each dvitem In lstDV
                If dvitem.DVStatus = "1" Then
                    dvitem.DVStatus = "Created"
                ElseIf dvitem.DVStatus = "2" Then
                    dvitem.DVStatus = "DV Cancelled"
                ElseIf dvitem.DVStatus = "3" Then
                    dvitem.DVStatus = "Approved"
                End If
            Next



            Return lstDV


            'lstCHQ = (From x In db.or_ObligationRequest
            '   Where x.lib_Payee.PayeeName.Trim.ToLower.Contains(payee)
            '   Join y In db.dv_DisbursementVoucher On x.ID Equals y.ObligationRequestID
            '   Where y.lib_Payee.PayeeName.Trim.ToLower.Contains(payee)
            '   Join z In db.ch_Cheque On y.ID Equals z.dvID
            '   Where z.PayToOrder.Trim.ToLower.Contains(payee)
            '    Select New ChequeIssued With {.Name = x.lib_Payee.PayeeName, .ID = x.ID, .ChequeNo = z.ChequeNo, .PayToOrder = z.PayToOrder,
            '            .Amount = z.Amount, .ChequeDate = z.Date, .DateCreated = z.DateCreated,
            '                    .AllotmentClass = x.AllotmentObjectClass,
            '                    .ObrNo = x.ObligationRequestNo, .ObrDateCancelled = x.DateCancelled, .ObrDateCreated = x.DateCreated, .ObrTotal = x.TotalAmount, .ObrStatus = x.Status,
            '                    .DVNo = y.DisbursementVoucherNo, .DVDateCancelled = y.CancelledDate, .DVDateCreated = y.DateCreated, .DVTotal = y.ParticularsAmountDue, .DVStatus = y.Status}).ToList
            'If lstCHQ.Count = 0 Then
            '    Return lstDV
            'Else
            '    Return lstCHQ
            'End If

            ' 

            'lstRet = (From x In db.or_ObligationRequest
            '          Join y In db.dv_DisbursementVoucher On x.ID Equals y.ObligationRequestID
            '          Join z In db.ch_Cheque On y.ID Equals z.dvID
            '          Order By x.ObligationRequestNo Ascending
            '            Select New ChequeIssued With {.ID = x.ID, .ChequeNo = z.ChequeNo, .PayToOrder = z.PayToOrder,
            '                              .Amount = z.Amount, .ChequeDate = z.Date, .DateCreated = z.DateCreated,
            '                              .AllotmentClass = x.AllotmentObjectClass,
            '                              .ObrNo = x.ObligationRequestNo, .ObrDateCancelled = x.DateCancelled, .ObrDateCreated = x.DateCreated, .ObrTotal = x.TotalAmount, .ObrStatus = x.Status,
            '                              .DVNo = y.DisbursementVoucherNo, .DVDateCancelled = y.CancelledDate, .DVDateCreated = y.DateCreated, .DVTotal = y.ParticularsAmountDue, .DVStatus = y.Status}).ToList

            'Return lstRet
        End Function
        Public Shared Function GetAllOBRTransactions(payee As String, fromdate As Date) As List(Of ChequeIssued)
            Dim db As New dvDbEntities
            Dim lstOBR As New List(Of ChequeIssued)
            Dim lstDV As New List(Of ChequeIssued)
            Dim lstCHQ As New List(Of ChequeIssued)
            Dim lstRet As New List(Of ChequeIssued)
            Dim lstEmpty As New List(Of ChequeIssued)


            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)

            lstOBR = (From x In db.or_ObligationRequest
                      Where x.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) Order By x.ObligationRequestNo Descending
                        Select New ChequeIssued With {.Name = x.lib_Payee.PayeeName,
                                          .AllotmentClass = x.AllotmentObjectClass,
                                          .ObrNo = x.ObligationRequestNo, .ObrDateCancelled = x.DateCancelled, .ObrDateCreated = x.DateCreated, .ObrTotal = x.TotalAmount, .ObrStatus = x.Status}).ToList
            For Each item In lstOBR
                If item.ObrStatus = "1" Then
                    item.ObrStatus = "Created"
                ElseIf item.ObrStatus = "2" Then
                    item.ObrStatus = "Verified"
                ElseIf item.ObrStatus = "3" Then
                    item.ObrStatus = "Cancelled"
                Else
                    item.ObrStatus = "DV Created"
                End If
            Next
          
                    Return lstOBR
               
        End Function

#End Region

        <Serializable()>
        Public Class ChequeGenerationRecord
            Public Property ID As Guid
            Public Property ChequeNo As String
            Public Property ChequeDate As Nullable(Of Date)
            Public Property DateCreated As Nullable(Of Date)
            Public Property DataCanceled As Nullable(Of Date)
            Public Property Remarks As String
            Public Property Status As Integer
            Public Property dvID As Guid
            Public Property Amount As String
            Public Property AmountInWords As String
            Public Property PayToOrder As String
            Public Property AllotmentClass As String
            Public Property ChSignatoryA As Nullable(Of Guid)
            Public Property ChsignatoryB As Nullable(Of Guid)

            Public Property CHisPriority As Nullable(Of Boolean)

        End Class
        <Serializable()>
        Public Class ChequeIssued
            Public Property ID As Guid
            Public Property ChequeNo As String
            Public Property ChequeDate As Nullable(Of Date)
            Public Property DateCreated As Nullable(Of Date)
            Public Property DataCanceled As Nullable(Of Date)
            Public Property Remarks As String
            Public Property Status As Integer
            Public Property dvID As Guid
            Public Property Amount As Integer
            Public Property AmountInWords As String
            Public Property PayToOrder As String
            Public Property AllotmentClass As String
            Public Property ChSignatoryA As Nullable(Of Guid)
            Public Property ChsignatoryB As Nullable(Of Guid)

            Public Property Description As String
            Public Property AccountCode As String
            Public Property Division As String
            Public Property PAP As String
            Public Property GrossAmount As String

            Public Property ObrNo As String
            Public Property ObrDateCreated As String
            Public Property ObrDateCancelled As String
            Public Property ObrTotal As String
            Public Property Name As String
            Public Property ObrStatus As String



            Public Property DVNo As String
            Public Property DVStatus As String
            Public Property DVDateCreated As String
            Public Property DVDateCancelled As String
            Public Property DVTotal As String

        End Class

    End Class
End Class
