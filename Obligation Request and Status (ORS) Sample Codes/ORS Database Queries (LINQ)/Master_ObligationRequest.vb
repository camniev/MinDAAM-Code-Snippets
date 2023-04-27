Imports System.Globalization

Partial Public Class Master
    Partial Public Class ObligationRequest

        Public Shared _lockObject As New Object

        Enum ObRStatus
            OnQueue = 1
            Verified = 2
            Cancelled = 3
            Deleted = 4
            OnQueueEdit = 5
            'DVCreated
            Approved = 6

            DVApproved = 7
            DVCancelled = 8
            CHGenerated = 9
            CHCancelled = 10
            CHReleased = 11
        End Enum

        Enum ParticularEntryDataStatus
            Add = 1
            Update = 2
            Delete = 3
        End Enum

        Public Shared Function getObligationRequestsByStatus(status As Integer) As List(Of ObligationRequestRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of ObligationRequestRecord)

            lstRet = (From p In db.or_ObligationRequest Where If(p.Status, 0) = status Order By p.DateCreated Descending, p.ObligationRequestNo Descending
                      Select New ObligationRequestRecord With {.ID = p.ID, .DateCreated = p.DateCreated, .DueDate = p.DueDate,
                                                               .ObligationRequestNo = p.ObligationRequestNo,
                                                               .PayeeID = p.PayeeID, .SourceDocument = p.SourceDocument, .TotalAmount = p.TotalAmount, .AllotmentObjectClass = p.AllotmentObjectClass, .SignatoryIdA = p.SignatoryIdA, .SignatoryIdB = p.SignatoryIdB, .isPrio = p.isPriority, .fundClusterCode = p.FundClusterCode, .fundClusterCodeId = p.FundClusterCodeID}).ToList

            Return lstRet
        End Function

        Public Shared Function getObligationRequests() As List(Of ObligationRequestRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of ObligationRequestRecord)

            lstRet = (From p In db.or_ObligationRequest Order By p.DateCreated Descending
                      Select New ObligationRequestRecord With {.ID = p.ID, .DateCreated = p.DateCreated, .DueDate = p.DueDate,
                                                               .ObligationRequestNo = p.ObligationRequestNo,
                                                               .PayeeID = p.PayeeID, .SourceDocument = p.SourceDocument, .TotalAmount = p.TotalAmount, .Status = p.Status}).ToList
            Return lstRet
        End Function

        Public Shared Function getObRPayeeByFilter(filter As String) As List(Of ObligationRequestRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of ObligationRequestRecord)

            filter = filter.Trim.ToLower
            lstRet = (From p In db.or_ObligationRequest Where p.lib_Payee.PayeeName.Trim.ToLower.Contains(filter) = False _
                      Select New ObligationRequestRecord With {.ID = p.ID, .DateCreated = p.DateCreated, .DueDate = p.DueDate,
                                                               .ObligationRequestNo = p.ObligationRequestNo,
                                                               .PayeeID = p.PayeeID, .SourceDocument = p.SourceDocument, .TotalAmount = p.TotalAmount, .AllotmentObjectClass = p.AllotmentObjectClass, .Status = p.Status, .fundClusterCode = p.FundClusterCode}).ToList
            Return lstRet
        End Function

        Public Shared Function getObligationRequestsByPayee(payeeName As String, Optional status As Integer = -1) As List(Of ObligationRequestRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of ObligationRequestRecord)
            If status <> -1 Then
                lstRet = (From p In db.or_ObligationRequest Where p.Status = status
                          Order By p.DateCreated Descending
                      Select New ObligationRequestRecord With {.ID = p.ID, .DateCreated = p.DateCreated, .DueDate = p.DueDate,
                      .ObligationRequestNo = p.ObligationRequestNo, .isPrio = p.isPriority, .AllotmentObjectClass = p.AllotmentObjectClass,
                      .PayeeID = p.PayeeID, .SourceDocument = p.SourceDocument, .TotalAmount = p.TotalAmount, .Status = p.Status}).ToList
            Else
                lstRet = (From p In db.or_ObligationRequest Order By p.DateCreated Descending
                      Select New ObligationRequestRecord With {.ID = p.ID, .DateCreated = p.DateCreated, .DueDate = p.DueDate,
                      .ObligationRequestNo = p.ObligationRequestNo, .isPrio = p.isPriority, .AllotmentObjectClass = p.AllotmentObjectClass,
                      .PayeeID = p.PayeeID, .SourceDocument = p.SourceDocument, .TotalAmount = p.TotalAmount, .Status = p.Status}).ToList
            End If
            If lstRet.Count > 0 Then
                lstRet = (From p In lstRet Where p.PayeeName.ToLower.Contains(payeeName.ToLower) Select p).ToList
            End If

            Return lstRet
        End Function
        Public Shared Function getObligationRequestByID(orID As Guid) As dvdll.or_ObligationRequest
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.or_ObligationRequest Where p.ID = orID Select p).FirstOrDefault

            If rec.AllotmentObjectClass = "100" Then
                rec.AllotmentObjectClass = "PS"
            ElseIf rec.AllotmentObjectClass = "200" Then
                rec.AllotmentObjectClass = "MOOE"
            Else
                rec.AllotmentObjectClass = "CO"
            End If

            Return rec
        End Function
        Public Shared Function getORParticularsByORID(orID As Guid) As List(Of dvdll.or_ObligationRequestParticular)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet = (From p In db.or_ObligationRequestParticular Where p.ObligationRequestID = orID Select p).ToList
            Return lstRet
        End Function
        Public Shared Function getORParticularByID(particularID As Guid) As dvdll.or_ObligationRequestParticular
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.or_ObligationRequestParticular Where p.ID = particularID Select p).FirstOrDefault
            Return rec
        End Function

        Public Shared Function generateObligationRequestNo(ObRDateCreated As Date) As String
            Dim db As New dvdll.dvDbEntities

            Dim ItemYear = ObRDateCreated.Year.ToString

            If ItemYear = Now.Year.ToString Then
                Dim startno = Master.System.GetSettingValue(System.SettingControlIDs.OBRSerialNo.ToString())
                Dim latestORRec = (From p In db.or_ObligationRequest Where p.ObligationRequestNo <> "NO RECORD" And p.DateCreated.Value.Year = Now.Year Order By p.ObligationRequestNo Descending Select p).FirstOrDefault

                If latestORRec Is Nothing Then
                    'orNo = Now.Year.ToString() & "-" & Now.Month.ToString.PadLeft(2, "0") & "-" & startno.PadLeft(6, "0") //Now.Year.ToString() changed to below code
                    orNo = ItemYear & "-" & Now.Month.ToString.PadLeft(2, "0") & "-" & startno.PadLeft(6, "0")
                Else
                    Dim prevNo = CInt(latestORRec.ObligationRequestNo.Substring(8, 6))

                    orNo = ItemYear & "-" & Now.Month.ToString.PadLeft(2, "0") & "-" & (prevNo + 1).ToString.PadLeft(6, "0")

                End If

            Else

                Dim NewYearRec = (From p In db.or_ObligationRequest Where p.ObligationRequestNo <> "NO RECORD" And p.DateCreated.Value.Year = ObRDateCreated.Year
                           Order By p.ObligationRequestNo Descending Select p).FirstOrDefault

                If NewYearRec Is Nothing Then
                    Dim NewYear = ObRDateCreated.Year.ToString
                    Dim NewMonth = ObRDateCreated.Month.ToString
                    NewYearStart = "1"
                    orNo = ItemYear & "-" & NewMonth.ToString.PadLeft(2, "0") & "-" & NewYearStart.PadLeft(6, "0")
                Else
                    Dim NewYear = ObRDateCreated.Year.ToString
                    Dim NewMonth = ObRDateCreated.Month.ToString
                    Dim prevNo = CInt(NewYearRec.ObligationRequestNo.Substring(8, 6))
                    orNo = ItemYear & "-" & NewMonth.ToString.PadLeft(2, "0") & "-" & (prevNo + 1).ToString.PadLeft(6, "0")
                End If
            End If


            Return orNo
        End Function

        Public Shared Sub SaveObligationRequest(orRecord As ObligationRequestRecord)
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities
                Dim newOR As New dvdll.or_ObligationRequest
                Dim AOC As String
                With newOR
                    .ID = Guid.NewGuid
                    .DateCreated = orRecord.DateCreated

                    If orRecord.AllotmentObjectClass = "100" Then
                        AOC = "01"
                    ElseIf orRecord.AllotmentObjectClass = "200" Then
                        AOC = "02"
                    Else
                        AOC = "06"
                    End If

                    .ObligationRequestNo = generateObligationRequestNo(orRecord.DateCreated)
                    .PayeeID = orRecord.PayeeID
                    .TotalAmount = orRecord.TotalAmount
                    .SourceDocument = orRecord.SourceDocument
                    .Status = ObRStatus.OnQueue
                    .ParticularID = orRecord.ParticularID
                    .AllotmentObjectClass = orRecord.AllotmentObjectClass
                    .SignatoryIdA = orRecord.SignatoryIdA
                    .SignatoryIdB = orRecord.SignatoryIdB
                    .isPriority = orRecord.isPrio
                    .FundClusterCodeID = orRecord.fundClusterCodeId
                End With
                db.or_ObligationRequest.Add(newOR)
                db.SaveChanges()

                For Each entry In orRecord.ORParticularEntries
                    If entry.DataStatus = ParticularEntryDataStatus.Delete Then
                        Continue For
                    End If
                    Dim newORParticular As New dvdll.or_ObligationRequestParticular
                    With newORParticular
                        .ID = Guid.NewGuid
                        .Amount = entry.Amount
                        .Description = orRecord.ParticularTemplate
                        .ObligationRequestID = newOR.ID
                        .PPAID = entry.PPAID
                        .FundSource = entry.FundSource
                        If Not entry.ResposibilityCenterID = Guid.Empty Then
                            .ResponsibilityCenterID = entry.ResposibilityCenterID
                            .FundSource = entry.FundSource
                        End If
                        .AcctID = entry.AcctID
                        If Not entry.TravellerPayeeID = Nothing Then
                            .TravelDestination = entry.TravelDestination
                            .TravelDateFrom = entry.TravelDateFrom
                            .TravelDateTo = entry.TravelDateTo
                            .TravellerPayeeID = entry.TravellerPayeeID
                            .TravelPurpose = entry.TravelPurpose
                        End If
                    End With
                    db.or_ObligationRequestParticular.Add(newORParticular)
                Next
                db.SaveChanges()



                'SYSTEM LOGS
                Dim dblog As New dvdll.dvDbEntities

                Dim newLog As New dvdll.sys_Logs
                With newLog
                    .ID = Guid.NewGuid
                    .UserLog = orRecord.dUser
                    .LogDate = Now
                    .LogType = "CREATED"
                    .RefNo = newOR.ObligationRequestNo
                End With
                dblog.sys_Logs.Add(newLog)
                dblog.SaveChanges()
                'CLOSE SYSTEM LOGS


            End SyncLock

        End Sub

        Public Shared Sub NoObrSaveObligationRequest(orRecord As ObligationRequestRecord)
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities
                Dim newOR As New dvdll.or_ObligationRequest
                With newOR
                    .ID = Guid.NewGuid
                    .DateCreated = Now
                    .ObligationRequestNo = "NO RECORD"
                    .PayeeID = orRecord.PayeeID
                    .TotalAmount = orRecord.TotalAmount
                    .SourceDocument = orRecord.SourceDocument
                    .Status = ObRStatus.OnQueue
                    .ParticularID = orRecord.ParticularID
                    .AllotmentObjectClass = orRecord.AllotmentObjectClass
                    .SignatoryIdA = orRecord.SignatoryIdA
                    .SignatoryIdB = orRecord.SignatoryIdB
                    .FundClusterCode = orRecord.fundClusterCode

                End With
                db.or_ObligationRequest.Add(newOR)
                db.SaveChanges()

                For Each entry In orRecord.ORParticularEntries
                    Dim newORParticular As New dvdll.or_ObligationRequestParticular
                    With newORParticular
                        .ID = Guid.NewGuid
                        .Amount = entry.Amount
                        .Description = orRecord.ParticularTemplate
                        .ObligationRequestID = newOR.ID
                        .PPAID = entry.PPAID
                        If Not entry.ResposibilityCenterID = Guid.Empty Then
                            .ResponsibilityCenterID = entry.ResposibilityCenterID
                            .FundSource = entry.FundSource
                        End If
                        .AcctID = entry.AcctID
                        If Not entry.TravellerPayeeID = Nothing Then
                            .TravelDestination = entry.TravelDestination
                            .TravelDateFrom = entry.TravelDateFrom
                            .TravelDateTo = entry.TravelDateTo
                            .TravellerPayeeID = entry.TravellerPayeeID
                            .TravelPurpose = entry.TravelPurpose
                        End If
                    End With
                    db.or_ObligationRequestParticular.Add(newORParticular)
                Next
                db.SaveChanges()
            End SyncLock
        End Sub

        Public Shared Sub UpdateOBStatus(id As Guid, status As Integer, Optional editedBy As Guid = Nothing)
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities

                Dim rec = (From p In db.or_ObligationRequest Where p.ID = id Select p).FirstOrDefault
                If Not rec Is Nothing Then
                    If rec.Status = ObRStatus.OnQueueEdit Then
                        Throw New Exception("Unable to update Obligation Request. It is currently being edited by " + rec.EditorName + ".")
                    Else
                        rec.Status = status
                        If Not editedBy = Nothing Then
                            rec.EditedByEmployeeID = editedBy
                            rec.EditedOn = Now
                        End If
                        db.SaveChanges()
                    End If
                End If

            End SyncLock
        End Sub

        Public Shared Sub CancelDate(id As Guid, canceldate As Date, user As String)
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities

                Dim rec = (From p In db.or_ObligationRequest Where p.ID = id Select p).FirstOrDefault
                If Not rec Is Nothing Then
                    With rec
                        .DateCancelled = canceldate
                    End With
                    db.SaveChanges()
                End If


                'SYSTEM LOGS
                Dim dblog As New dvdll.dvDbEntities

                Dim newLog As New dvdll.sys_Logs
                With newLog
                    .ID = Guid.NewGuid
                    .UserLog = user
                    .LogDate = Now
                    .LogType = "CANCELLED"
                    .RefNo = rec.ObligationRequestNo
                End With
                dblog.sys_Logs.Add(newLog)
                dblog.SaveChanges()
                'CLOSE SYSTEM LOGS


            End SyncLock
        End Sub

        Public Shared Sub UpdateObligationRequest(orRecord As ObligationRequestRecord)
            SyncLock _lockObject
                Dim db As New dvdll.dvDbEntities
                Dim rec = (From p In db.or_ObligationRequest Where p.ID = orRecord.ID Select p).FirstOrDefault
                If Not rec Is Nothing Then
                    With rec
                        .PayeeID = orRecord.PayeeID
                        .TotalAmount = orRecord.TotalAmount
                        .SourceDocument = orRecord.SourceDocument
                        .ParticularID = orRecord.ParticularID
                        .Status = ObRStatus.OnQueue
                        .SignatoryIdA = orRecord.SignatoryIdA
                        .SignatoryIdB = orRecord.SignatoryIdB
                        '.DateCreated = orRecord.DateCreated
                        .AllotmentObjectClass = orRecord.AllotmentObjectClass
                        .FundClusterCode = orRecord.fundClusterCode
                    End With
                    db.SaveChanges()

                    Dim ParticularEntries = (From p In orRecord.ORParticularEntries Select p).ToList

                    For Each updData In ParticularEntries
                        Dim updParticular = (From p In db.or_ObligationRequestParticular Where p.ID = updData.ID Select p).FirstOrDefault
                        If Not updParticular Is Nothing Then
                            With updParticular
                                .Amount = updData.Amount
                                .AcctID = updData.AcctID
                                .PPAID = updData.PPAID
                                .ResponsibilityCenterID = updData.ResposibilityCenterID
                                .Description = orRecord.ParticularTemplate

                                If updData.TravellerPayeeID <> Nothing Then
                                    .TravelDestination = updData.TravelDestination
                                    .TravelDateFrom = updData.TravelDateFrom
                                    .TravelDateTo = updData.TravelDateTo
                                    .TravellerPayeeID = updData.TravellerPayeeID
                                    .TravelPurpose = updData.TravelPurpose
                                End If
                            End With
                        Else
                            'Dim newORParticular = (From p In db.or_ObligationRequestParticular Where p.ID <> updData.ID Select p).FirstOrDefault
                            Dim newORParticularEntries As New dvdll.or_ObligationRequestParticular
                            With newORParticularEntries
                                .ID = Guid.NewGuid
                                .Amount = updData.Amount
                                .Description = orRecord.ParticularTemplate
                                .ObligationRequestID = orRecord.ID
                                .PPAID = updData.PPAID
                                If Not updData.ResposibilityCenterID = Guid.Empty Then
                                    .ResponsibilityCenterID = updData.ResposibilityCenterID
                                    .FundSource = updData.FundSource
                                End If
                                .AcctID = updData.AcctID
                                If Not updData.TravellerPayeeID = Nothing Then
                                    .TravelDestination = updData.TravelDestination
                                    .TravelDateFrom = updData.TravelDateFrom
                                    .TravelDateTo = updData.TravelDateTo
                                    .TravellerPayeeID = updData.TravellerPayeeID
                                    .TravelPurpose = updData.TravelPurpose
                                End If
                            End With
                            db.or_ObligationRequestParticular.Add(newORParticularEntries)
                        End If
                    Next
                    db.SaveChanges()


                    Dim delParticularEntries = (From p In orRecord.ORParticularEntries
                                                Where p.DataStatus = ParticularEntryDataStatus.Delete
                                                Select p).ToList
                    For Each delData In delParticularEntries
                        Dim delParticular = (From p In db.or_ObligationRequestParticular Where p.ID = delData.ID Select p).FirstOrDefault
                        db.or_ObligationRequestParticular.Remove(delParticular)
                    Next
                    db.SaveChanges()

                End If




                'SYSTEM LOGS
                Dim dblog As New dvdll.dvDbEntities

                Dim newLog As New dvdll.sys_Logs
                With newLog
                    .ID = Guid.NewGuid
                    .UserLog = orRecord.dUser
                    .LogDate = Now
                    .LogType = "EDITED"
                    .RefNo = orRecord.ObligationRequestNo
                End With
                dblog.sys_Logs.Add(newLog)
                dblog.SaveChanges()
                'CLOSE SYSTEM LOGS




            End SyncLock


        End Sub

        Public Shared Function getObligationRequestRecordByID(orID As Guid) As ObligationRequestRecord
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.or_ObligationRequest Where p.ID = orID
                       Select New ObligationRequestRecord With {.ID = p.ID, .DateCreated = p.DateCreated,
                                                                .DueDate = p.DueDate, .ObligationRequestNo = p.ObligationRequestNo, .PayeeID = p.PayeeID,
                                                                .SourceDocument = p.SourceDocument, .TotalAmount = p.TotalAmount,
                                                                .Status = p.Status, .ParticularID = p.ParticularID, .isPrio = p.isPriority}).FirstOrDefault
            Return rec
        End Function
        Public Shared Function getORParticularEntriesByORID(orID As Guid) As List(Of ORParticularEntry)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet = (From p In db.or_ObligationRequestParticular Where p.ObligationRequestID = orID
                          Select New ORParticularEntry With {.ID = p.ID, .Amount = p.Amount, .AcctID = p.AcctID, .PPAID = p.PPAID,
                                                             .ResposibilityCenterID = If(p.ResponsibilityCenterID, Guid.Empty),
                                                             .Description = p.Description}).ToList
            Return lstRet
        End Function


#Region "REPORT"
        Public Shared Function getOBRforReport() As List(Of or_ObligationRequest)
            Dim db As New dvdll.dvDbEntities
            Dim lst = (From p In db.or_ObligationRequest Select p).ToList
            Return lst
        End Function
        Public Shared Function getOBRLisPerDate(fromdate As Date, todate As Date) As List(Of ObligationRequestRecord)
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.or_ObligationRequest Where p.DateCreated <= fromdate And p.DateCreated >= todate Select New ObligationRequestRecord With
                       {.ID = p.ID, .DateCreated = p.DateCreated, .DueDate = p.DueDate,
                                                               .ObligationRequestNo = p.ObligationRequestNo,
                                                               .PayeeID = p.PayeeID, .SourceDocument = p.SourceDocument, .TotalAmount = p.TotalAmount, .Status = p.Status}).ToList
            Return rec
        End Function

        Public Shared Function GetObRByDateCreated(fromdate As Date, todate As Date, status As Integer, payee As String, particulars As String, obrNo As String) As List(Of or_ObligationRequest)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of or_ObligationRequest)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            'And p.lib_Particulars.Template.Contains(particulars)

            If particulars <> "" Then
                'SEARCH WITH PARTICULARS 07152022
                If status = "1" Then
                    lstRet = (From p In db.or_ObligationRequest
                              Join q In db.or_ObligationRequestParticular On p.ID Equals q.ObligationRequestID
                              Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And q.Description.Trim.ToLower.Contains(particulars) And p.ObligationRequestNo <> "NO RECORD" And p.ObligationRequestNo.Contains(obrNo) Order By p.ObligationRequestNo Ascending
                              Select p).ToList
                    'lstRet = lstRet.
                    For Each item In lstRet
                        If item.Status = "3" Then
                            item.TotalAmount = "0.00"
                        End If
                    Next
                ElseIf status = "7" Then
                    lstRet = (From p In db.or_ObligationRequest
                              Join q In db.or_ObligationRequestParticular On p.ID Equals q.ObligationRequestID
                          Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.Status = ObRStatus.OnQueue And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And q.Description.Trim.ToLower.Contains(particulars) And p.ObligationRequestNo <> "NO RECORD" And p.ObligationRequestNo.Contains(obrNo) Order By p.ObligationRequestNo Ascending
                          Select p).ToList
                ElseIf status = "3" Then
                    lstRet = (From p In db.or_ObligationRequest
                              Join q In db.or_ObligationRequestParticular On p.ID Equals q.ObligationRequestID
                          Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.Status = status And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And q.Description.Trim.ToLower.Contains(particulars) And p.ObligationRequestNo <> "NO RECORD" And p.ObligationRequestNo.Contains(obrNo) Order By p.ObligationRequestNo Ascending
                          Select p).ToList
                ElseIf status = "99" Then
                    lstRet = (From p In db.or_ObligationRequest
                              Join q In db.or_ObligationRequestParticular On p.ID Equals q.ObligationRequestID
                   Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And q.Description.Trim.ToLower.Contains(particulars) _
                   And p.ObligationRequestNo <> "NO RECORD" And p.ObligationRequestNo.Contains(obrNo) _
                   And p.Status <> ObligationRequest.ObRStatus.CHGenerated And p.Status <> ObligationRequest.ObRStatus.CHReleased And p.Status <> ObligationRequest.ObRStatus.CHCancelled
                   Order By p.ObligationRequestNo Ascending
                   Select p).ToList
                    'lstRet = lstRet.
                    For Each item In lstRet
                        If item.Status = "3" Then
                            item.TotalAmount = "0.00"
                        End If
                    Next
                Else
                    lstRet = (From p In db.or_ObligationRequest
                              Join q In db.or_ObligationRequestParticular On p.ID Equals q.ObligationRequestID
                        Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.Status <> Master.ObligationRequest.ObRStatus.Cancelled And p.Status <> Master.ObligationRequest.ObRStatus.OnQueue And
                        p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And q.Description.Trim.ToLower.Contains(particulars) And p.ObligationRequestNo <> "NO RECORD" And p.ObligationRequestNo.Contains(obrNo) Order By p.ObligationRequestNo Ascending
                        Select p).ToList
                    For Each item In lstRet
                        If item.Status = "3" Then
                            item.TotalAmount = "0.00"
                        End If
                    Next
                End If
            Else
                'SEARCH WITHOUT PARTICULARS 07152022
                If status = "1" Then
                    lstRet = (From p In db.or_ObligationRequest
                    Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.ObligationRequestNo <> "NO RECORD" And p.ObligationRequestNo.Contains(obrNo) Order By p.ObligationRequestNo Ascending
                    Select p).ToList
                    For Each item In lstRet
                        If item.Status = "3" Then
                            item.TotalAmount = "0.00"
                        End If
                    Next

                ElseIf status = "7" Then
                    lstRet = (From p In db.or_ObligationRequest
                          Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.Status = ObRStatus.OnQueue And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.ObligationRequestNo <> "NO RECORD" And p.ObligationRequestNo.Contains(obrNo) Order By p.ObligationRequestNo Ascending
                          Select p).ToList


                ElseIf status = "3" Then
                    lstRet = (From p In db.or_ObligationRequest
                          Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.Status = status And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.ObligationRequestNo <> "NO RECORD" And p.ObligationRequestNo.Contains(obrNo) Order By p.ObligationRequestNo Ascending
                          Select p).ToList

                ElseIf status = "99" Then
                    lstRet = (From p In db.or_ObligationRequest
                   Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) _
                   And p.ObligationRequestNo <> "NO RECORD" And p.ObligationRequestNo.Contains(obrNo) _
                   And p.Status <> ObligationRequest.ObRStatus.CHGenerated And p.Status <> ObligationRequest.ObRStatus.CHReleased And p.Status <> ObligationRequest.ObRStatus.CHCancelled
                   Order By p.ObligationRequestNo Ascending
                   Select p).ToList
                    'lstRet = lstRet.
                    For Each item In lstRet
                        If item.Status = "3" Then
                            item.TotalAmount = "0.00"
                        End If
                    Next

                Else
                    lstRet = (From p In db.or_ObligationRequest
                        Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.Status <> Master.ObligationRequest.ObRStatus.Cancelled And p.Status <> Master.ObligationRequest.ObRStatus.OnQueue And
                        p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee) And p.ObligationRequestNo <> "NO RECORD" And p.ObligationRequestNo.Contains(obrNo) Order By p.ObligationRequestNo Ascending
                        Select p).ToList
                    For Each item In lstRet
                        If item.Status = "3" Then
                            item.TotalAmount = "0.00"
                        End If
                    Next
                End If
            End If
                For Each item In lstRet
                    If item.AllotmentObjectClass = "100" Then
                        item.AllotmentObjectClass = "PS"
                    ElseIf item.AllotmentObjectClass = "200" Then
                        item.AllotmentObjectClass = "MOOE"
                    Else
                        item.AllotmentObjectClass = "CO"
                    End If
                Next


                Return lstRet
        End Function

        Public Shared Function GetObrWithParticulars(fromdate As Date, todate As Date, status As Integer, ObrNo As String) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            If status = "1" Then
                lstRet = (From p In db.or_ObligationRequestParticular
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated >= fromdate And x.DateCreated <= todate And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD" And x.ObligationRequestNo.Contains(ObrNo)
                    Order By x.ObligationRequestNo Descending
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated, .TotalAmount = x.TotalAmount}).ToList
                For Each item In lstRet
                    If item.Status = "3" Then
                        item.TotalAmount = "0.00"
                        item.Amount = "0.00"
                    End If
                Next


            Else
                lstRet = (From p In db.or_ObligationRequestParticular
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated >= fromdate And x.DateCreated <= todate And x.Status = status And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD" And x.ObligationRequestNo.Contains(ObrNo)
                    Order By x.ObligationRequestNo Descending
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated, .TotalAmount = x.TotalAmount}).ToList

                For Each item In lstRet
                    If item.Status = "3" Then
                        item.TotalAmount = "0.00"
                        item.Amount = "0.00"
                    End If
                Next

            End If
            Return lstRet
        End Function

        Public Shared Function GetObRByDateCreatedAOC(fromdate As Date, todate As Date, status As Integer, AOC As String) As List(Of or_ObligationRequest)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of or_ObligationRequest)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            If status = "1" Then
                lstRet = (From p In db.or_ObligationRequest
                Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.AllotmentObjectClass = AOC And p.ObligationRequestNo <> "NO RECORD" And p.Status = status Order By p.ObligationRequestNo Ascending
                Select p).ToList

                For Each item In lstRet
                    If item.Status = "3" Then
                        item.TotalAmount = "0.00"
                    End If
                Next

            Else
                lstRet = (From p In db.or_ObligationRequest
                    Where p.DateCreated >= fromdate And p.DateCreated <= todate And p.Status = status And p.AllotmentObjectClass = AOC And p.ObligationRequestNo <> "NO RECORD" Order By p.ObligationRequestNo Ascending
                    Select p).ToList

                For Each item In lstRet
                    If item.Status = "3" Then
                        item.TotalAmount = "0.00"
                    End If
                Next

            End If
            Return lstRet
        End Function

        
        Public Shared Function GetParticularsByDivisionByAccountCode(fromdate As Date, todate As Date, status As Integer, DivisionID As Guid, AccountCodeID As Guid) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            If status = "1" Then
                lstRet = (From p In db.or_ObligationRequestParticular
                    Where p.AcctID = AccountCodeID And p.ResponsibilityCenterID = DivisionID
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated >= fromdate And x.DateCreated <= todate And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD"
                    Order By p.ObligationRequestID
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated}).ToList
                For Each item In lstRet
                    If item.Status = "3" Then
                        item.Amount = "0.00"
                    End If
                Next

            Else
                lstRet = (From p In db.or_ObligationRequestParticular
                    Where p.AcctID = AccountCodeID And p.ResponsibilityCenterID = DivisionID
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated >= fromdate And x.DateCreated <= todate And x.Status = status And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD"
                    Order By p.ObligationRequestID
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated}).ToList

                For Each item In lstRet
                    If item.Status = "3" Then
                        item.Amount = "0.00"
                    End If
                Next

            End If
            Return lstRet
        End Function
        Public Shared Function GetParticularsByDivisionAllAccounts(fromdate As Date, todate As Date, status As Integer, DivisionID As Guid) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            If status = "1" Then
                lstRet = (From p In db.or_ObligationRequestParticular
                    Where p.ResponsibilityCenterID = DivisionID
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated >= fromdate And x.DateCreated <= todate And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD"
                    Order By p.AcctID Ascending
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated}).ToList
                For Each item In lstRet
                    If item.Status = "3" Then
                        item.Amount = "0.00"
                    End If
                Next

            Else
                lstRet = (From p In db.or_ObligationRequestParticular
                    Where p.ResponsibilityCenterID = DivisionID
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated >= fromdate And x.DateCreated <= todate And x.Status = status And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD"
                    Order By p.AcctID Ascending
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated}).ToList
                For Each item In lstRet
                    If item.Status = "3" Then
                        item.Amount = "0.00"
                    End If
                Next

            End If
            Return lstRet
        End Function

        Public Shared Function GetParticularsByAllDivisionByAccountCode(fromdate As Date, todate As Date, status As Integer, AccountCodeID As Guid) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            If status = "1" Then
                lstRet = (From p In db.or_ObligationRequestParticular
                    Where p.AcctID = AccountCodeID
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated >= fromdate And x.DateCreated <= todate And p.AcctID = AccountCodeID And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD"
                    Order By p.ObligationRequestID
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated}).ToList
                For Each item In lstRet
                    If item.Status = "3" Then
                        item.Amount = "0.00"
                    End If
                Next

            Else
                lstRet = (From p In db.or_ObligationRequestParticular
                    Where p.AcctID = AccountCodeID
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated >= fromdate And x.DateCreated <= todate And x.Status = status And p.AcctID = AccountCodeID And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD"
                    Order By p.ObligationRequestID
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated}).ToList

                For Each item In lstRet
                    If item.Status = "3" Then
                        item.Amount = "0.00"
                    End If
                Next

            End If
            Return lstRet
        End Function
        Public Shared Function GetParticularsByAllDivisionAllAccounts(fromdate As Date, todate As Date, status As Integer) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(0).AddMinutes(0).AddSeconds(0)

            Dim newfromdate As DateTime = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            Dim reformattedfromdate As String = newfromdate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)

            Dim newtodate As DateTime = DateTime.ParseExact(todate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            Dim reformattedtodate As String = newtodate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)

            If status = "1" Then
                lstRet = (From p In db.or_ObligationRequestParticular
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated >= reformattedfromdate And x.DateCreated <= reformattedtodate And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD"
                    Order By p.AcctID Ascending
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated}).ToList
                For Each item In lstRet
                    If item.Status = "3" Then
                        item.Amount = "0.00"
                    End If
                Next

            Else
                lstRet = (From p In db.or_ObligationRequestParticular
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated >= fromdate And x.DateCreated <= todate And x.Status = status And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD"
                    Order By p.AcctID Ascending
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated}).ToList
                For Each item In lstRet
                    If item.Status = "3" Then
                        item.Amount = "0.00"
                    End If
                Next

            End If
            Return lstRet
        End Function

        Public Shared Function GetParticularsByPrograms(fromdate As Date, todate As Date, DivisionID As Guid) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            lstRet = (From p In db.or_ObligationRequestParticular
                Where p.ResponsibilityCenterID = DivisionID
                Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                Where x.DateCreated >= fromdate And x.DateCreated <= todate And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD" And p.or_ObligationRequest.Status <> ObligationRequest.ObRStatus.Cancelled
                Order By p.AcctID Ascending
                Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                         .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                         .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated}).ToList
            For Each item In lstRet
                If item.Status = "3" Then
                    item.Amount = "0.00"
                End If
            Next


            Return lstRet
        End Function

        Public Shared Function GetMonthlyObR(month As String, year As String) As List(Of or_ObligationRequest)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of or_ObligationRequest)

            lstRet = (From p In db.or_ObligationRequest
            Where p.DateCreated.Value.Month = month And p.DateCreated.Value.Year = year And p.ObligationRequestNo <> "NO RECORD" Order By p.ObligationRequestNo Ascending
            Select p).ToList

            For Each item In lstRet
                If item.Status = "3" Then
                    If item.DateCancelled.Value.Month = month Then
                        item.TotalAmount = "0.00"
                    Else
                    End If

                End If
            Next

            For Each item In lstRet
                If item.AllotmentObjectClass = "100" Then
                    item.AllotmentObjectClass = "PS"
                ElseIf item.AllotmentObjectClass = "200" Then
                    item.AllotmentObjectClass = "MOOE"
                Else
                    item.AllotmentObjectClass = "CO"
                End If
            Next


            Return lstRet
        End Function
        Public Shared Function GetMonthlyObRPastRecord(month As String, year As String) As List(Of or_ObligationRequest)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of or_ObligationRequest)

            lstRet = (From p In db.or_ObligationRequest
            Where p.DateCancelled.Value.Month = month And p.DateCancelled.Value.Year = year And
            p.ObligationRequestNo <> "NO RECORD" Order By p.ObligationRequestNo Ascending
            Select p).ToList

            'p.DateCreated.Value.Month <> month And p.DateCreated.Value.Year <> year And

            For Each item In lstRet
                If item.Status = "3" Then
                    If item.DateCreated.Value.Month = month And item.DateCreated.Value.Year = year Then
                        item.TotalAmount = "0.00"
                    Else
                        item.TotalAmount = item.TotalAmount * -1
                    End If
                End If
            Next

            For Each item In lstRet
                If item.AllotmentObjectClass = "100" Then
                    item.AllotmentObjectClass = "PS"
                ElseIf item.AllotmentObjectClass = "200" Then
                    item.AllotmentObjectClass = "MOOE"
                Else
                    item.AllotmentObjectClass = "CO"
                End If
            Next


            Return lstRet
        End Function

        Public Shared Function GetMonthlyObrParticularsByAccountCode(month As String, year As String, AcctCodeID As Guid, AOC As Integer, status As Integer) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)

            If status = "1" Then
                lstRet = (From p In db.or_ObligationRequestParticular
                    Where p.AcctID = AcctCodeID
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                Where x.DateCreated.Value.Month = month And x.DateCreated.Value.Year = year And x.AllotmentObjectClass = AOC And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD" And x.Status <> ObRStatus.Cancelled
                    Order By x.ObligationRequestNo Ascending
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo, .ObligationRequestID = p.ObligationRequestID,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated, .DateCancelled = x.DateCancelled}).ToList
                Dim subTotal = 0
                For Each item In lstRet

                    getAmount = (From p In db.or_ObligationRequestParticular Where p.ObligationRequestID = item.ObligationRequestID Select p.ObligationRequestID).FirstOrDefault

                    If item.ObligationRequestID = getAmount Then
                        subTotal = item.Amount + subTotal

                        item.SubAmount += subTotal
                    Else
                        item.SubAmount = item.Amount
                    End If
                    'item.SubAmount += subTotal
                Next
            Else
                lstRet = (From p In db.or_ObligationRequestParticular
                    Where p.AcctID = AcctCodeID
                    Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                    Where x.DateCreated.Value.Month = month And x.DateCreated.Value.Year = year And x.AllotmentObjectClass = AOC And x.Status = status And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD"
                    Order By x.ObligationRequestNo Ascending
                    Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                             .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                             .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated, .DateCancelled = x.DateCancelled}).ToList

            End If

            For Each item In lstRet
                If item.Status = "3" Then
                    If item.DateCancelled.Value.Month = month Then
                        item.TotalAmount = "0.00"
                        item.Amount = "0.00"
                    Else
                    End If

                End If
            Next



            Return lstRet
        End Function

        Public Shared Function GetActualMonthlyObR(fromdate As Date, todate As Date, PAPCodeID As Guid, AOC As Integer) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            'If status = "1" Then   Where p.PPAID = PAPCodeID
            lstRet = (From p In db.or_ObligationRequestParticular
                Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
            Where x.DateCreated >= fromdate And x.DateCreated <= todate And x.AllotmentObjectClass = AOC And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD" And p.or_ObligationRequest.Status <> "3"
                Order By x.ObligationRequestNo Ascending
                Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo, .ObligationRequestID = p.ObligationRequestID,
                                                         .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                         .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated, .DateCancelled = x.DateCancelled}).ToList

            For Each item In lstRet
                If item.Status = "3" Then
                    item.TotalAmount = "0.00"
                    item.Amount = "0.00"
                End If
            Next

            Return lstRet
        End Function


        Public Shared Function GetActualMonthReportParticulars(fromdate As Date, todate As Date, AOC As Integer) As List(Of or_ObligationRequestParticular)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of or_ObligationRequestParticular)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            lstRet = (From obrpart In db.or_ObligationRequestParticular
                        Join obr In db.or_ObligationRequest On obrpart.ObligationRequestID Equals obr.ID
                        Where obr.DateCreated >= fromdate And obr.DateCreated <= todate _
                        And obr.AllotmentObjectClass = AOC And obr.ObligationRequestNo <> "NO RECORD" _
                        And obr.Status <> ObRStatus.Cancelled And obr.Status <> ObRStatus.CHCancelled And obr.Status <> ObRStatus.DVCancelled
                        Order By obr.ObligationRequestNo Ascending
                        Select obrpart Order By obrpart.lib_Account.AccountCode).ToList()

            Return lstRet
        End Function
        Public Shared Function GetActualMonthReportAccounts(lst As List(Of or_ObligationRequestParticular)) As List(Of Guid)
            Dim db As New dvDbEntities

            Dim AcctIDs As List(Of Guid) = (From obrpart In lst
                                               Select obrpart.AcctID.Value).Distinct.ToList()

            Return AcctIDs
        End Function
        Public Shared Function GetActualMonthTotalPerRC(lst As List(Of or_ObligationRequestParticular),
                                                             acctID As Guid, rcID As Guid) As Decimal
            Dim db As New dvDbEntities

            Dim total = (From p In lst
                            Where p.AcctID = acctID And p.ResponsibilityCenterID = rcID
                            Select p.Amount).Sum
            If total Is Nothing Then
                Return 0
            Else
                Return total
            End If
        End Function
        Public Shared Function ActualMonthlyObRData_Dummy() As List(Of ActualMonthlyObRData)
            Return New List(Of ActualMonthlyObRData)
        End Function




        'Fund Allocation Balance
        Public Shared Function GetBalance(fromdate As Date, todate As Date, AOC As Integer) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            lstRet = (From p In db.or_ObligationRequestParticular
               Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
               Where x.DateCreated >= fromdate And x.DateCreated <= todate And x.AllotmentObjectClass = AOC And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD"
               Order By x.ObligationRequestNo Ascending
               Join y In db.lib_FundSources On p.ResponsibilityCenterID Equals y.ResponsibilityCenterID
               Where x.DateCreated.Value.Year = y.sys_FiscalYear.Year And x.AllotmentObjectClass = y.AllotmentObjectClass
               Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo, .ObligationRequestID = p.ObligationRequestID,
                                                        .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                        .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated, .DateCancelled = x.DateCancelled, .FundAllocationAmount = y.Amount}).ToList

            For Each item In lstRet
                If item.Status = "3" Then
                    item.TotalAmount = "0.00"
                    item.Amount = "0.00"
                End If
            Next



            Return lstRet

        End Function

        'Travel
        Public Shared Function GetTravels(fromdate As Date, todate As Date, TravelType As String, traveler As String, destination As String) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)

            fromdate = fromdate.AddHours(0).AddMinutes(0).AddSeconds(0)
            todate = todate.AddHours(23).AddMinutes(59).AddSeconds(59)

            lstRet = (From p In db.or_ObligationRequestParticular
                Join x In db.or_ObligationRequest On p.ObligationRequestID Equals x.ID
                Where p.TravelDateFrom >= fromdate And p.TravelDateFrom <= todate And p.or_ObligationRequest.ObligationRequestNo <> "NO RECORD" And p.or_ObligationRequest.Status <> ObligationRequest.ObRStatus.Cancelled _
                And p.or_ObligationRequest.Status <> ObligationRequest.ObRStatus.CHCancelled And p.or_ObligationRequest.Status <> ObligationRequest.ObRStatus.DVCancelled _
                And p.lib_Account.AccountCode.Contains(TravelType) And p.lib_Payee.PayeeName.ToLower.Trim.Contains(traveler) And p.TravelDestination.Contains(destination)
                Order By x.ObligationRequestNo Ascending
                Select New ORParticularEntryRecord With {.ID = p.ID, .Status = x.Status, .ObligationRequestNo = x.ObligationRequestNo,
                                                         .Allotment = x.AllotmentObjectClass, .PayeeID = x.PayeeID, .ResposibilityCenterID = p.ResponsibilityCenterID,
                                                         .AcctID = p.AcctID, .Description = p.Description, .Amount = p.Amount, .DateCreated = x.DateCreated, .TotalAmount = x.TotalAmount,
                                                         .TravellerPayeeID = p.TravellerPayeeID, .TravelDestination = p.TravelDestination, .TravelPurpose = p.TravelPurpose,
                                                         .TravelDateFrom = p.TravelDateFrom, .TravelDateTo = p.TravelDateTo}).ToList


            For Each item In lstRet
                If item.Status = "3" Then
                    item.TotalAmount = "0.00"
                    item.Amount = "0.00"
                End If
            Next



            Return lstRet
        End Function
#End Region

#Region "SEARCH"
        Public Shared Function SearchTransaction(month As String, year As String, payee As String) As List(Of ORParticularEntryRecord)
            Dim db As New dvDbEntities
            Dim lstRet As New List(Of ORParticularEntryRecord)
            Dim lstDV As New List(Of ORParticularEntryRecord)
            Dim lstCHQ As New List(Of ORParticularEntryRecord)

            lstRet = (From p In db.or_ObligationRequest
                   Where p.DateCreated.Value.Month = month And p.DateCreated.Value.Year = year And p.lib_Payee.PayeeName.Trim.ToLower.Contains(payee)
                   Order By p.ObligationRequestNo Ascending
                   Select New ORParticularEntryRecord With {.ID = p.ID, .Status = p.Status, .ObligationRequestNo = p.ObligationRequestNo,
                                                            .Allotment = p.AllotmentObjectClass, .PayeeID = p.PayeeID, .Description = p.or_ObligationRequestParticular.FirstOrDefault.Description,
                                                            .DateCreated = p.DateCreated, .DateCancelled = p.DateCancelled,
                                                             .ParticularTemplate = p.or_ObligationRequestParticular.FirstOrDefault.Description, .CHAmount = p.TotalAmount}).ToList

            For Each item In lstRet
                If item.Status = "6" Or item.Status = "7" Or item.Status = "8" Then
                    lstDV = (From p In db.dv_DisbursementVoucher Where p.or_ObligationRequest.ObligationRequestNo = item.ObligationRequestNo
                            Select New ORParticularEntryRecord With {.DVNumber = p.DisbursementVoucherNo}).ToList
                    item.TransactionNumber = lstDV.FirstOrDefault.DVNumber

                End If

                If item.Status = "9" Or item.Status = "10" Or item.Status = "11" Then
                    'lstDV = (From p In db.dv_DisbursementVoucher Where p.or_ObligationRequest.ObligationRequestNo = item.ObligationRequestNo
                    '        Select New ORParticularEntryRecord With {.DVNumber = p.DisbursementVoucherNo}).ToList

                    'lstCHQ = (From p In db.ch_Cheque Where p.dvID = lstDV.FirstOrDefault.ID
                    '       Select New ORParticularEntryRecord With {.CHNumber = p.ChequeNo}).ToList

                    lstCHQ = (From x In db.ch_Cheque Join y In db.dv_DisbursementVoucher
                           On x.dvID Equals y.ID Where y.or_ObligationRequest.ObligationRequestNo = item.ObligationRequestNo
                           Order By x.ChequeNo
                           Select New ORParticularEntryRecord With {.CHNumber = x.ChequeNo, .CHAmount = x.Amount}).ToList

                    item.TransactionNumber = lstCHQ.FirstOrDefault.CHNumber
                    item.CHAmount = lstCHQ.FirstOrDefault.CHAmount
                End If
            Next


            Return lstRet

        End Function

#End Region

        <Serializable()> _
        Public Class ObligationRequestRecord
            Public Property ID As Guid
            Public Property PayeeID As Guid
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
            Public Property ObligationRequestNo As String
            Public Property DateCreated As Nullable(Of Date)
            Public Property DueDate As Nullable(Of Date)
            Public Property TotalAmount As Decimal
            Public Property ORParticularEntries As List(Of ORParticularEntry)
            Public Property SourceDocument As String
            Public Property Status As String
            Public Property ParticularID As Guid
            Public Property ParticularTemplate As String
            Public Property AllotmentObjectClass As String
            Public Property SignatoryIdA As Nullable(Of Guid)
            Public Property SignatoryIdB As Nullable(Of Guid)
            Public Property OnBreakGroup As Nullable(Of Integer)
            Public Property xReportStatus As String
            Public Property fundClusterCode As String
            Public Property fundClusterCodeId As Nullable(Of Guid)
            Public ReadOnly Property UACSCode As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_UACSCode Where p.ID = fundClusterCodeId Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.UACSCode
                    End If
                    Return ""
                End Get
            End Property

            Public Property isPrio As Nullable(Of Boolean)
            Public Property dUser As String

            Public ReadOnly Property Template As String
                Get
                    Dim db As New dvDbEntities
                    Dim rec = (From p In db.lib_Particulars Where p.ID = ParticularID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Template
                    End If
                    Return ""
                End Get
            End Property
            Sub New()
                ORParticularEntries = New List(Of ORParticularEntry)
                Status = ObRStatus.OnQueue
            End Sub
        End Class

        <Serializable()> _
        Public Class ORParticularEntry
            Public Property ID As Guid
            Public Property IDIndex As Integer
            Public Property ResposibilityCenterID As Nullable(Of Guid)
            Public Property PPAID As Guid
            Public Property Amount As Nullable(Of Decimal)
            Public Property DataStatus As Integer
            Public Property AcctID As Guid
            Public Property TravellerPayeeID As Guid
            Public Property TravelDestination As String
            Public Property TravelDateFrom As Nullable(Of DateTime)
            Public Property TravelDateTo As Nullable(Of DateTime)
            Public Property TravelPurpose As String
            Public Property Description As String
            Public Property FundSource As Guid
            Public ReadOnly Property TravelDateSummary As String
                Get
                    If If(TravelDateFrom, DateTime.MinValue) <> DateTime.MinValue Or
                       If(TravelDateTo, DateTime.MinValue) <> DateTime.MinValue Then
                        Return TravelDateFrom.Value.ToShortDateString() + " - " + TravelDateTo.Value.ToShortDateString()
                    End If
                    Return ""
                End Get
            End Property
        End Class

        'FOR REPORT

        <Serializable()> _
        Public Class ORParticularEntryRecord

            Public Property subTotal As Nullable(Of Integer)


            Public Property ID As Guid
            Public Property IDIndex As Integer
            Public Property ResposibilityCenterID As Nullable(Of Guid)
            Public ReadOnly Property xRespoCenter As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = ResposibilityCenterID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property PPAID As Guid
            Public ReadOnly Property xPPA As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_PPA Where p.ID = PPAID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.PPACode
                    End If
                    Return ""
                End Get
            End Property
            Public ReadOnly Property xParticulars As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.or_ObligationRequestParticular Where p.ObligationRequestID = ObligationRequestID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Description
                    End If
                    Return ""
                End Get
            End Property
            Public Property Amount As Nullable(Of Decimal)
            Public Property DataStatus As Integer
            Public Property AcctID As Guid
            Public ReadOnly Property xAccountCode As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_Account Where p.ID = AcctID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.AccountCode
                    End If
                    Return ""
                End Get
            End Property
            Public ReadOnly Property xAccountDesc As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_Account Where p.ID = AcctID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.AccountDescription
                    End If
                    Return ""
                End Get
            End Property
            Public Property TravellerPayeeID As Guid
            Public Property TravelDestination As String
            Public Property TravelDateFrom As Nullable(Of DateTime)
            Public Property TravelDateTo As Nullable(Of DateTime)
            Public Property TravelPurpose As String
            Public Property Description As String
            Public Property FundSource As Guid
            Public ReadOnly Property TravelDateSummary As String
                Get
                    If If(TravelDateFrom, DateTime.MinValue) <> DateTime.MinValue Or
                       If(TravelDateTo, DateTime.MinValue) <> DateTime.MinValue Then
                        Return TravelDateFrom.Value.ToShortDateString() + " to " + TravelDateTo.Value.ToShortDateString()
                    End If
                    Return ""
                End Get
            End Property
            Public Property PayeeID As Guid
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
            Public ReadOnly Property TravellerNameX As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_Payee Where p.ID = TravellerPayeeID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.PayeeName
                    End If
                    Return ""
                End Get
            End Property
            Public Property ObligationRequestNo As String
            Public Property DateCreated As Nullable(Of Date)
            Public Property DateCancelled As Nullable(Of Date)
            Public Property DueDate As Nullable(Of Date)
            Public Property TotalAmount As Decimal
            Public Property ORParticularEntries As List(Of ORParticularEntry)
            Public Property SourceDocument As String
            Public Property Status As String
            Public Property ParticularID As Guid
            Public Property ParticularTemplate As String
            Public Property Allotment As String
            Public ReadOnly Property xAllotmentObjectClass As String
                Get
                    Select Case Allotment
                        Case 100
                            Return "PS"
                        Case 200
                            Return "MOOE"
                        Case 300
                            Return "CO"
                        Case Else
                            Return ""
                    End Select
                End Get
            End Property
            Public Property SignatoryIdA As Nullable(Of Guid)
            Public Property SignatoryIdB As Nullable(Of Guid)
            Public ReadOnly Property xStatus As String
                Get
                    Select Case Status
                        Case 1
                            Return "Created"
                        Case 2
                            Return "Verified"
                        Case 3
                            Return "Cancelled"
                        Case 6
                            Return "DV Created"
                        Case 7
                            Return "DV Approved"
                        Case 8
                            Return "DV Cancelled"
                        Case 9
                            Return "CH Generated"
                        Case 10
                            Return "CH Released"
                        Case Else
                            Return ""
                    End Select
                End Get
            End Property
            Public Property ObligationRequestID As Guid

            Public Property DVNumber As String
            Public Property CHNumber As String
            Public Property CHAmount As Decimal

            Public Property TransactionNumber As String

            Public Property SubAmount As Decimal

            Public Property FundAllocationAmount As Decimal

        End Class

        'GetActualMonthlyObR REPORT
        Public Class ActualMonthlyObRData
            Public Property Account As String
            Public Property UACS As String
            Public Property OldAccount As String

            Public Property Col1ID As Guid
            Public ReadOnly Property Col1Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col1ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col1Data As Decimal

            Public Property Col2ID As Guid
            Public ReadOnly Property Col2Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col2ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col2Data As Decimal

            Public Property Col3ID As Guid
            Public ReadOnly Property Col3Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col3ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col3Data As Decimal

            Public Property Col4ID As Guid
            Public ReadOnly Property Col4Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col4ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col4Data As Decimal

            Public Property Col5ID As Guid
            Public ReadOnly Property Col5Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col5ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col5Data As Decimal

            Public Property Col6ID As Guid
            Public ReadOnly Property Col6Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col6ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col6Data As Decimal

            Public Property Col7ID As Guid
            Public ReadOnly Property Col7Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col7ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col7Data As Decimal

            Public Property Col8ID As Guid
            Public ReadOnly Property Col8Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col8ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col8Data As Decimal

            Public Property Col9ID As Guid
            Public ReadOnly Property Col9Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col9ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col9Data As Decimal

            Public Property Col10ID As Guid
            Public ReadOnly Property Col10Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col10ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col10Data As Decimal

            Public Property Col11ID As Guid
            Public ReadOnly Property Col11Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col11ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col11Data As Decimal

            Public Property Col12ID As Guid
            Public ReadOnly Property Col12Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col12ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col12Data As Decimal

            Public Property Col13ID As Guid
            Public ReadOnly Property Col13Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col13ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col13Data As Decimal

            Public Property Col14ID As Guid
            Public ReadOnly Property Col14Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col14ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col14Data As Decimal

            Public Property Col15ID As Guid
            Public ReadOnly Property Col15Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col15ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col15Data As Decimal

            Public Property Col16ID As Guid
            Public ReadOnly Property Col16Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col16ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col16Data As Decimal

            Public Property Col17ID As Guid
            Public ReadOnly Property Col17Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col17ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col17Data As Decimal

            Public Property Col18ID As Guid
            Public ReadOnly Property Col18Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col18ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col18Data As Decimal

            Public Property Col19IDX As Guid
            Public ReadOnly Property Col19HeaderX As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col19IDX Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col19DataX As Decimal

            Public Property Col19ID As Guid
            Public ReadOnly Property Col19Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col19ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col19Data As Decimal

            Public Property Col20ID As Guid
            Public ReadOnly Property Col20Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col20ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col20Data As Decimal

            Public Property Col21ID As Guid
            Public ReadOnly Property Col21Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col21ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col21Data As Decimal

            Public Property Col22ID As Guid
            Public ReadOnly Property Col22Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col22ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col22Data As Decimal

            Public Property Col23ID As Guid
            Public ReadOnly Property Col23Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col23ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col23Data As Decimal

            Public Property Col24ID As Guid
            Public ReadOnly Property Col24Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col24ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col24Data As Decimal

            Public Property Col25ID As Guid
            Public ReadOnly Property Col25Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col25ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col25Data As Decimal

            Public Property Col26ID As Guid
            Public ReadOnly Property Col26Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col26ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col26Data As Decimal

            Public Property Col27ID As Guid
            Public ReadOnly Property Col27Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col27ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col27Data As Decimal

            Public Property Col28ID As Guid
            Public ReadOnly Property Col28Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col28ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col28Data As Decimal

            Public Property Col29ID As Guid
            Public ReadOnly Property Col29Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col29ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col29Data As Decimal

            Public Property Col30ID As Guid
            Public ReadOnly Property Col30Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col30ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col30Data As Decimal

            Public Property Col31ID As Guid
            Public ReadOnly Property Col31Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col31ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col31Data As Decimal

            Public Property Col32ID As Guid
            Public ReadOnly Property Col32Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col32ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col32Data As Decimal

            Public Property Col33ID As Guid
            Public ReadOnly Property Col33Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col33ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col33Data As Decimal

            Public Property Col34ID As Guid
            Public ReadOnly Property Col34Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col34ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col34Data As Decimal

            Public Property Col35ID As Guid
            Public ReadOnly Property Col35Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col35ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col35Data As Decimal

            Public Property Col36ID As Guid
            Public ReadOnly Property Col36Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col36ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col36Data As Decimal

            Public Property Col37ID As Guid
            Public ReadOnly Property Col37Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col37ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col37Data As Decimal

            Public Property Col38ID As Guid
            Public ReadOnly Property Col38Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col38ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col38Data As Decimal



            Public Property Col60ID As Guid
            Public ReadOnly Property Col60Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col60ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col60Data As Decimal




            Public Property Col39ID As Guid
            Public ReadOnly Property Col39Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col39ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col39Data As Decimal

            Public Property Col40ID As Guid
            Public ReadOnly Property Col40Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col40ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col40Data As Decimal

            Public Property Col40IDX As Guid
            Public ReadOnly Property Col40HeaderX As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col40IDX Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col40DataX As Decimal

            Public Property Col41ID As Guid
            Public ReadOnly Property Col41Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col41ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col41Data As Decimal

            Public Property Col42ID As Guid
            Public ReadOnly Property Col42Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col42ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col42Data As Decimal

            Public Property Col43ID As Guid
            Public ReadOnly Property Col43Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col43ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col43Data As Decimal

            Public Property Col44ID As Guid
            Public ReadOnly Property Col44Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col44ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col44Data As Decimal

            Public Property Col45ID As Guid
            Public ReadOnly Property Col45Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col45ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col45Data As Decimal

            Public Property Col46ID As Guid
            Public ReadOnly Property Col46Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col46ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col46Data As Decimal

            Public Property Col47ID As Guid
            Public ReadOnly Property Col47Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col47ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col47Data As Decimal

            Public Property Col48ID As Guid
            Public ReadOnly Property Col48Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col48ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col48Data As Decimal

            Public Property Col49ID As Guid
            Public ReadOnly Property Col49Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col49ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col49Data As Decimal

            Public Property Col50ID As Guid
            Public ReadOnly Property Col50Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col50ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col50Data As Decimal

            Public Property Col51ID As Guid
            Public ReadOnly Property Col51Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col51ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col51Data As Decimal

            Public Property Col52ID As Guid
            Public ReadOnly Property Col52Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col52ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col52Data As Decimal

            Public Property Col53ID As Guid
            Public ReadOnly Property Col53Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col53ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col53Data As Decimal

            Public Property Col54ID As Guid
            Public ReadOnly Property Col54Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col54ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col54Data As Decimal

            Public Property Col55ID As Guid
            Public ReadOnly Property Col55Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col55ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col55Data As Decimal

            'for IRD_ICF
            Public Property Col56ID As Guid
            Public ReadOnly Property Col56Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col56ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col56Data As Decimal

            'for AMO WM Regular
            Public Property Col57ID As Guid
            Public ReadOnly Property Col57Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col57ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col57Data As Decimal

            'for AMO WM 
            Public Property Col61ID As Guid
            Public ReadOnly Property Col61Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col61ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col61Data As Decimal

            'for OED NOW
            Public Property Col58ID As Guid
            Public ReadOnly Property Col58Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col58ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col58Data As Decimal

            'for OED Regular
            Public Property Col59ID As Guid
            Public ReadOnly Property Col59Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col59ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col59Data As Decimal

            'for PRD NOW
            Public Property Col62ID As Guid
            Public ReadOnly Property Col62Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col62ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col62Data As Decimal

            'for Purd SET
            Public Property Col63ID As Guid
            Public ReadOnly Property Col63Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col63ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col63Data As Decimal

            Public Property Col64ID As Guid
            Public ReadOnly Property Col64Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col64ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col64Data As Decimal

            Public Property Col65ID As Guid
            Public ReadOnly Property Col65Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col65ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property
            Public Property Col65Data As Decimal

            Public Property Col66ID As Guid
            Public ReadOnly Property Col66Header As String
                Get
                    Dim db As New dvdll.dvDbEntities
                    Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = Col66ID Select p).FirstOrDefault
                    If Not rec Is Nothing Then
                        Return rec.Division
                    End If
                    Return ""
                End Get
            End Property

            Public Property Col66Data As Decimal

        End Class
    End Class
End Class