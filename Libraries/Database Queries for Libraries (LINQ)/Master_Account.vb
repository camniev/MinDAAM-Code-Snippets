Partial Public Class Master
    Public Class Account

        Public Enum SpecialAccountCodes
            'Account CODE Is changed from 108 to 10104040

            DueToBIR = 412
            CashNationalTreasury = 10104040
            CashAdvance = 148
            CashIPURE = 9
            'SalarayAndWages = 5010101001
        End Enum
        Public Enum AccountSpecialType
            Regular = 0
            Travel = 1
        End Enum

        Public Shared Function getAccountEntryRecord() As List(Of AccountEntryRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of AccountEntryRecord)

            lstRet = (From p In db.lib_Account Order By p.AccountDescription Ascending
                     Select New AccountEntryRecord With {.ID = p.ID, .AccountCode = p.AccountCode, .AccountDesc = p.AccountDescription, .AccountDescription = p.AccountDescription,
                                                         .ObRType = p.ObRType, .IsVoid = p.IsVoid}).ToList
            Return lstRet
        End Function


        Public Shared Function getAccountEntryRecordOrderByName() As List(Of AccountEntryRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of AccountEntryRecord)

            lstRet = (From p In db.lib_Account Where p.IsVoid = False Order By p.AccountDescription Ascending
                      Select New AccountEntryRecord With {.ID = p.ID, .AccountCode = p.AccountCode, .AccountDesc = p.AccountDescription, .AccountDescription = p.AccountDescription, .ObRType = p.ObRType, .IsVoid = p.IsVoid}).ToList
            Return lstRet
        End Function

        Public Shared Function getAccountSpecialTypes() As List(Of KeyValuePair(Of String, Integer))
            Dim lst As New List(Of KeyValuePair(Of String, Integer))

            lst.Add(New KeyValuePair(Of String, Integer)("[-N/A-]", 0))
            lst.Add(New KeyValuePair(Of String, Integer)("Travel", AccountSpecialType.Travel))

            Return lst
        End Function

        Public Shared Function getAccounts() As List(Of lib_Account)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of lib_Account)
            lstrest = (From p In db.lib_Account Where Not If(p.IsVoid, False)
                       Order By p.AccountCode Ascending Select p).ToList
            If lstrest.Count < 1 Then Return New List(Of lib_Account)
            Return lstrest
        End Function

        Public Shared Function getFundSourceMine(name As String) As List(Of lib_Account)
            Dim db As New dvdll.dvDbEntities
            Dim itm As New List(Of lib_Account)

            itm = (From p In db.lib_FundSources Where p.FundName = name Select New lib_Account With {.AccountCode = p.FundName}).ToList
            Return itm

        End Function

        Public Shared Function getAccountsByFilter(filter As String) As List(Of AccountEntryRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of AccountEntryRecord)

            filter = filter.ToLower.Trim
            lstrest = (From p In db.lib_Account Where If(p.IsVoid, False) = False And
                       p.AccountDescription.ToLower.Trim.Contains(filter) Order By p.AccountDescription Ascending
                       Select New AccountEntryRecord With {.ID = p.ID, .AccountCode = p.AccountCode, .AccountDesc = p.AccountDescription, .AccountDescription = p.AccountDescription,
                                                         .IsVoid = p.IsVoid, .ObRType = p.ObRType}).ToList

            If lstrest.Count < 1 Then Return New List(Of AccountEntryRecord)
            Return lstrest
        End Function

        Public Shared Function getAccountByID(id As Guid) As lib_Account
            Dim db As New dvDbEntities
            Dim rec = (From p In db.lib_Account Where p.ID = id Select p Order By p.AccountDescription).FirstOrDefault
            If Not rec Is Nothing Then
                Return rec
            End If
            Return Nothing
        End Function
        Public Shared Function getAccountCode108() As Master.DisbursementVoucher.AccountEntry

            'Account CODE Is changed from 108 to 10104040

            Dim db As New dvDbEntities
            Dim rec = (From p In db.lib_Account Where p.AccountCode = "10104040"
                       Select New Master.DisbursementVoucher.AccountEntry With {.ID = p.ID, .AccountCode = p.AccountCode,
                                                                                .AcctsAndExplanation = p.AccountDescription,
                                                                                .Ref = "", .ResponsibilityCenter = "",
                                                                                .Credit = 0}).FirstOrDefault
            If rec Is Nothing Then
                Dim newAcct As New lib_Account
                With newAcct
                    .ID = Guid.NewGuid
                    .AccountCode = "10104040"
                    .AccountDescription = "Cash-National Treasury, Modified"
                    .IsVoid = False
                    .ObRType = 0
                End With
                db.lib_Account.Add(newAcct)
                db.SaveChanges()
                rec = New Master.DisbursementVoucher.AccountEntry
                With rec
                    .ID = newAcct.ID
                    .AcctID = newAcct.ID
                    .AccountCode = newAcct.AccountCode
                    .AcctsAndExplanation = newAcct.AccountDescription
                    .Ref = ""
                    .ResponsibilityCenter = ""
                    .Credit = 0
                End With
            End If
            Return rec
        End Function
        Public Shared Function getAccountCode412() As Master.DisbursementVoucher.AccountEntry
            Dim db As New dvDbEntities
            Dim rec = (From p In db.lib_Account Where p.AccountCode = "412"
                       Select New Master.DisbursementVoucher.AccountEntry With {.ID = p.ID, .AccountCode = p.AccountCode,
                                                                                .AcctsAndExplanation = p.AccountDescription,
                                                                                .Ref = "", .ResponsibilityCenter = "",
                                                                                .Credit = 0}).FirstOrDefault
            If rec Is Nothing Then
                Dim newAcct As New lib_Account
                With newAcct
                    .ID = Guid.NewGuid
                    .AccountCode = "412"
                    .AccountDescription = "Due to BIR"
                    .IsVoid = False
                    .ObRType = 0
                End With
                db.lib_Account.Add(newAcct)
                db.SaveChanges()
                rec = New Master.DisbursementVoucher.AccountEntry
                With rec
                    .ID = newAcct.ID
                    .AcctID = newAcct.ID
                    .AccountCode = newAcct.AccountCode
                    .AcctsAndExplanation = newAcct.AccountDescription
                    .Ref = ""
                    .ResponsibilityCenter = ""
                    .Credit = 0
                End With
            End If
            Return rec
        End Function
        Public Shared Function getAccountbyCode(code As String) As lib_Account
            Dim db As New dvDbEntities
            Dim rec = (From p In db.lib_Account Where p.AccountCode = code Select p).FirstOrDefault
            If Not rec Is Nothing Then
                Return rec
            End If
            Return Nothing
        End Function
        Public Shared Function getAccountEntriesByDVID(dvID As Guid) As List(Of Master.DisbursementVoucher.AccountEntry)
            Dim db As New dvDbEntities
            Dim lst As New List(Of Master.DisbursementVoucher.AccountEntry)
            lst = (From p In db.dv_AccountEntry Where p.DisbursementVoucherID = dvID
                 Select New Master.DisbursementVoucher.AccountEntry With {.ID = p.ID, .AccountCode = p.AccountCode,
                                                                          .AcctsAndExplanation = p.AccountsAndExplainations,
                                                                          .Credit = p.Credit, .Debit = p.Debit,
                                                                          .Ref = p.Ref, .ResponsibilityCenter = p.ResponsibilityCenter}).ToList
            If lst.Count > 0 Then
                Dim sortedLst As New List(Of Master.DisbursementVoucher.AccountEntry)
                For Each rec In lst
                    If Not rec.AccountCode = SpecialAccountCodes.CashNationalTreasury And
                        Not rec.AccountCode = SpecialAccountCodes.DueToBIR Then
                        sortedLst.Add(rec)
                    End If
                Next
                Dim dueToBIR = lst.FirstOrDefault(Function(p) p.AccountCode = SpecialAccountCodes.DueToBIR)
                If Not dueToBIR Is Nothing Then
                    sortedLst.Add(dueToBIR)
                End If
                Dim treasury = lst.FirstOrDefault(Function(p) p.AccountCode = SpecialAccountCodes.CashNationalTreasury)
                If Not treasury Is Nothing Then
                    sortedLst.Add(treasury)
                End If

                Return sortedLst
            End If

            Return lst
        End Function
       

        Public Shared Sub AddAccount(code As String, desc As String, type As Integer)
            Dim db As New dvDbEntities
            Dim newRec As New lib_Account
            With newRec
                .ID = Guid.NewGuid
                .AccountCode = code
                .AccountDescription = desc
                .ObRType = type
                .IsVoid = False
            End With
            db.lib_Account.Add(newRec)
            db.SaveChanges()
        End Sub
        Public Shared Sub UpdateAccount(id As Guid, code As String, desc As String, type As Integer)
            Dim db As New dvDbEntities
            Dim rec = (From p In db.lib_Account Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                With rec
                    .AccountCode = code
                    .AccountDescription = desc
                    .ObRType = type
                End With
                db.SaveChanges()
            End If
        End Sub
        Public Shared Sub ActivateAccount(id As Guid)
            Dim db As New dvDbEntities
            Dim rec = (From p In db.lib_Account Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsVoid = False
                db.SaveChanges()
            End If
        End Sub
        Public Shared Sub DeleteAccount(id As Guid)
            Dim db As New dvDbEntities
            Dim rec = (From p In db.lib_Account Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsVoid = True
                db.SaveChanges()
            End If
        End Sub
        <Serializable()> _
        Public Class AccountEntryRecord
            Public Property ID As Guid
            Public Property AccountCode As String
            Public Property AccountDesc As String
            Public Property AccountDescription As String
            Public Property IsVoid As Nullable(Of Boolean)
            Public Property ObRType As Nullable(Of Integer)
            Public Property Type As Nullable(Of Integer)
        End Class

        <Serializable()> _
        Public Class FundSourcesMine

            Public Property FundName As String


        End Class

    End Class
End Class
