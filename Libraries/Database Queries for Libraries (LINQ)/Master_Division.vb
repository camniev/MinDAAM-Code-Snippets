Partial Public Class Master
    Partial Public Class Division
        Public Shared Function getDivision() As List(Of DivisionRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of DivisionRecord)
            lstrest = (From p In db.lib_ResponsibilityCenter Where Not If(p.IsVoid, False) And p.IsActive = "1" Order By p.Division Ascending Select New DivisionRecord With {.ID = p.ID, .Division = p.Division, .DivisionDesc = p.DivisionDesc, .IsActive = p.IsActive}).ToList
            If lstrest.Count < 1 Then Return New List(Of DivisionRecord)
            Return lstrest
        End Function

        'Public Shared Function getFundSources() As List(Of FundSources)
        '    Dim db As New dvdll.dvDbEntities
        '    Dim lstrest As New Lazy(Of FundSources)
        '    lstrest = (From p In db.lib_FundSources Where Not If(p.IsVoid, False) Select New FundSources With {.ID = p.ID, .FundName = p.FundName, .Amount = p.Amount}).ToList
        '    If lstrest.Count < 1 Then Return New List(Of FundSources)
        '    Return lstrest
        'End Function

        Public Shared Function getDivisionShowAll() As List(Of DivisionRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of DivisionRecord)
            lstrest = (From p In db.lib_ResponsibilityCenter Where Not If(p.IsVoid, False) Order By p.IsActive Descending, p.Division Ascending Select New DivisionRecord With {.ID = p.ID, .Division = p.Division, .DivisionDesc = p.DivisionDesc, .IsActive = p.IsActive}).ToList
            If lstrest.Count < 1 Then Return New List(Of DivisionRecord)
            Return lstrest
        End Function

        Public Shared Function getDivisionByFilter(filter As String) As List(Of DivisionRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of DivisionRecord)
            filter = filter.ToLower.Trim
            lstRet = (From p In db.lib_ResponsibilityCenter Where p.Division.ToLower.Trim.Contains(filter) _
                      And If(p.IsVoid, False) = False Select New DivisionRecord With {.ID = p.ID, .Division = p.Division,
                                                                                     .DivisionDesc = p.DivisionDesc,
                                                                                     .IsActive = p.IsActive}).ToList
            Return lstRet
        End Function

        Public Shared Function getDivisionByID(id As Guid) As DivisionRecord
            Dim db As New dvdll.dvDbEntities
            Dim itm As New DivisionRecord

            itm = (From p In db.lib_ResponsibilityCenter Where p.ID = id Select New DivisionRecord With {.ID = p.ID, .Division = p.Division, .DivisionDesc = p.DivisionDesc, .IsActive = p.IsActive}).FirstOrDefault
            Return itm
        End Function

        Public Shared Function getDivisionPopup() As List(Of DivisionRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of DivisionRecord)
            lstrest = (From p In db.lib_ResponsibilityCenter Where Not If(p.IsVoid, False) And p.IsActive = "1" Order By p.Division Ascending Select New DivisionRecord With {.ID = p.ID, .Division = p.Division, .DivisionDesc = p.DivisionDesc, .IsActive = p.IsActive}).ToList
            If lstrest.Count < 1 Then Return New List(Of DivisionRecord)
            Return lstrest
        End Function

        Public Shared Function getDivisionByFilterPopup(filter As String) As List(Of DivisionRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of DivisionRecord)
            filter = filter.ToLower.Trim
            lstRet = (From p In db.lib_ResponsibilityCenter Where p.Division.ToLower.Trim.Contains(filter) _
                      And If(p.IsVoid, False) = False And p.IsActive = "1" Select New DivisionRecord With {.ID = p.ID, .Division = p.Division,
                                                                                     .DivisionDesc = p.DivisionDesc,
                                                                                     .IsActive = p.IsActive}).ToList
            Return lstRet
        End Function

        Public Shared Function getDivisionByIDPopup(id As Guid) As DivisionRecord
            Dim db As New dvdll.dvDbEntities
            Dim itm As New DivisionRecord

            itm = (From p In db.lib_ResponsibilityCenter Where p.ID = id And p.IsActive = "1" Select New DivisionRecord With {.ID = p.ID, .Division = p.Division, .DivisionDesc = p.DivisionDesc, .IsActive = p.IsActive}).FirstOrDefault
            Return itm
        End Function

        Public Shared Function getFundSourceByDivision(id As Guid) As List(Of FundSources)
            Dim db As New dvdll.dvDbEntities
            Dim itm As New List(Of FundSources)

            itm = (From p In db.lib_FundSources Where p.ResponsibilityCenterID = id Select New FundSources With {.ID = p.ID, .FundName = p.FundName,
                                                                                                                 .Amount = p.Amount, .DivisionID = p.ResponsibilityCenterID}).ToList
            Return itm
        End Function

        Public Shared Function getFundSourcebyID(id As Guid) As FundSources
            Dim db As New dvdll.dvDbEntities
            Dim itm As New FundSources

            itm = (From p In db.lib_FundSources Where p.ID = id Select New FundSources With {.ID = p.ID, .FundName = p.FundName,
                                                                                            .Amount = p.Amount, .DivisionID = p.ResponsibilityCenterID}).FirstOrDefault
            Return itm
        End Function

        Public Shared Function getFundSourceByNameInDivision(name As String) As List(Of FundSources)
            Dim db As New dvdll.dvDbEntities
            Dim itm As New List(Of FundSources)

            itm = (From p In db.lib_FundSources Where p.FundName = name Select New FundSources With {.ID = p.ID, .FundName = p.FundName,
                                                                                                         .Amount = p.Amount}).ToList
            Return itm

        End Function


        Public Shared Sub addNewDivision(division As String, divisiondesc As String)
            Dim db As New dvdll.dvDbEntities

            Dim newItem As New dvdll.lib_ResponsibilityCenter
            With newItem
                .ID = Guid.NewGuid
                .Division = division
                .DivisionDesc = divisiondesc
                .IsActive = "1"
            End With

            db.lib_ResponsibilityCenter.Add(newItem)
            db.SaveChanges()
        End Sub

        Public Shared Sub addNewFundSource(fundsource As String, fundamount As String, divisionID As Guid)
            Dim db As New dvdll.dvDbEntities
            Dim newFundSource As New dvdll.lib_FundSources
            With newFundSource
                .ID = Guid.NewGuid
                .FundName = fundsource
                .Amount = fundamount
                .ResponsibilityCenterID = divisionID
            End With
            db.lib_FundSources.Add(newFundSource)
            db.SaveChanges()
        End Sub

        Public Shared Sub updateDivisionID(id As Guid, division As String, divisiondesc As String)
            Dim db As New dvdll.dvDbEntities
            Dim itm = (From p In db.lib_ResponsibilityCenter Where p.ID = id Select p).FirstOrDefault
            If Not itm Is Nothing Then
                With itm
                    .Division = division
                    .DivisionDesc = divisiondesc
                End With
                db.SaveChanges()
            End If
        End Sub

        Public Shared Sub deleteDivisionByID(id As Guid)
            Dim db As New dvdll.dvDbEntities

            Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = id And Not If(p.IsVoid, False) Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsVoid = True
                db.SaveChanges()
            End If
        End Sub

        Public Shared Sub activateDivisionByID(id As Guid)
            Dim db As New dvdll.dvDbEntities

            Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsActive = "1"
                db.SaveChanges()
            End If
        End Sub

        Public Shared Sub deactivateDivisionByID(id As Guid)
            Dim db As New dvdll.dvDbEntities

            Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsActive = "0"
                db.SaveChanges()
            End If
        End Sub

        Public Shared Sub deleteFundSourceByID(id As Guid)
            Dim db As New dvdll.dvDbEntities
            Dim rec = (From p In db.lib_FundSources Where p.ID = id And Not If(p.IsVoid, False) Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsVoid = True
                db.SaveChanges()
            End If
        End Sub


        <Serializable()> _
        Public Class DivisionRecord
            Public Property ID As Guid
            Public Property Division As String
            Public Property DivisionDesc As String
            Public Property isVoid As Nullable(Of Boolean)
            Public Property IsActive As Integer
        End Class

        <Serializable()> _
        Public Class FundSources
            Public Property ID As Guid
            Public Property FundName As String
            Public Property Amount As String
            Public Property DivisionID As Guid

        End Class

    End Class
End Class
