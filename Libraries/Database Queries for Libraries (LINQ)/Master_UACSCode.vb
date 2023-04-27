Partial Public Class Master
    Public Class UACSCode

        Public Shared Function getAllUACSSourceCodes() As List(Of UACSCodeRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of UACSCodeRecord)

            lstRet = (From p In db.lib_UACSCode Order By p.UACSCode Descending
                     Select New UACSCodeRecord With {.ID = p.ID, .UACSCode = p.UACSCode, .UACSDescription = p.UACSDescription, .IsActive = p.IsActive}).ToList
            Return lstRet
        End Function

        Public Shared Function getUACSSourceCodesForChoices() As List(Of UACSCodeRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of UACSCodeRecord)

            lstRet = (From p In db.lib_UACSCode Where p.IsActive = 1 Order By p.UACSCode Descending
                     Select New UACSCodeRecord With {.ID = p.ID, .UACSCode = p.UACSCode, .UACSDescription = p.UACSDescription, .IsActive = p.IsActive}).ToList
            Return lstRet
        End Function

        Public Shared Function getUACSCodeByFilter(filter As String) As List(Of UACSCodeRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of UACSCodeRecord)

            filter = filter.ToLower.Trim
            lstrest = (From p In db.lib_UACSCode Where p.IsActive = 1 And
                       p.UACSDescription.ToLower.Trim.Contains(filter) Order By p.UACSCode Descending
                       Select New UACSCodeRecord With {.ID = p.ID, .UACSCode = p.UACSCode, .UACSDescription = p.UACSDescription}).ToList

            If lstrest.Count < 1 Then Return New List(Of UACSCodeRecord)
            Return lstrest
        End Function

        Public Shared Function getUACSCodeByID(id As Guid) As lib_UACSCode
            Dim db As New dvDbEntities
            Dim rec = (From p In db.lib_UACSCode Where p.ID = id Select p Order By p.UACSCode).FirstOrDefault
            If Not rec Is Nothing Then
                Return rec
            End If
            Return Nothing
        End Function

        Public Shared Sub AddUACSCode(code As String, desc As String)
            Dim db As New dvDbEntities
            Dim newRec As New lib_UACSCode
            With newRec
                .ID = Guid.NewGuid
                .UACSCode = code
                .UACSDescription = desc
                .IsActive = 1
            End With
            db.lib_UACSCode.Add(newRec)
            db.SaveChanges()
        End Sub

        Public Shared Sub UpdateUACSCode(id As Guid, code As String, desc As String)
            Dim db As New dvDbEntities
            Dim rec = (From p In db.lib_UACSCode Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                With rec
                    .UACSCode = code
                    .UACSDescription = desc
                End With
                db.SaveChanges()
            End If
        End Sub

        Public Shared Sub ActivateUACSByID(id As Guid)
            Dim db As New dvdll.dvDbEntities

            Dim rec = (From p In db.lib_UACSCode Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsActive = 1
                db.SaveChanges()
            End If
        End Sub

        Public Shared Sub DeactivateUACSByID(id As Guid)
            Dim db As New dvdll.dvDbEntities

            Dim rec = (From p In db.lib_UACSCode Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsActive = 0
                db.SaveChanges()
            End If
        End Sub

        'Public ReadOnly Property ResponsibilityCenter As String
        '    Get
        '        Dim db As New dvDbEntities
        '        Dim rec = (From p In db.lib_ResponsibilityCenter Where p.ID = ResponsibilityCenterID Select p).FirstOrDefault
        '        If Not rec Is Nothing Then
        '            Return rec.Division
        '        End If
        '        Return ""
        '    End Get
        'End Property

        <Serializable()>
        Public Class UACSCodeRecord
            Public Property ID As Guid
            Public Property UACSCode As String
            Public Property UACSDescription As String
            Public Property IsActive As Nullable(Of Integer)
        End Class
    End Class
End Class
