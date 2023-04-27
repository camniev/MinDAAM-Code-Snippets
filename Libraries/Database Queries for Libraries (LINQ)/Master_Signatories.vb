Partial Public Class Master
    Partial Public Class Signatories

        Public Shared Function getSignatory() As List(Of SignatoryRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of SignatoryRecord)
            lstrest = (From p In db.lib_Signatories Select New SignatoryRecord With {.ID = p.ID, .Name = p.Name, .Position = p.Position, .IsVoid = p.IsVoid}).ToList
            If lstrest.Count < 1 Then Return New List(Of SignatoryRecord)
            Return lstrest
        End Function

        Public Shared Function getSignatoryActive() As List(Of SignatoryRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of SignatoryRecord)
            lstrest = (From p In db.lib_Signatories Where p.IsVoid = False Select New SignatoryRecord With {.ID = p.ID, .Name = p.Name, .Position = p.Position, .IsVoid = p.IsVoid}).ToList
            If lstrest.Count < 1 Then Return New List(Of SignatoryRecord)
            Return lstrest
        End Function

        Public Shared Function getSignatoryByFilter(search As String) As List(Of SignatoryRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of SignatoryRecord)
            lstrest = (From p In db.lib_Signatories Where p.IsVoid = False And p.Name.Contains(search) Select New SignatoryRecord With {.ID = p.ID, .Name = p.Name, .Position = p.Position, .IsVoid = p.IsVoid}).ToList
            If lstrest.Count < 1 Then Return New List(Of SignatoryRecord)
            Return lstrest
        End Function

        Public Shared Function getSignatoryCtrl() As List(Of SignatoryRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of SignatoryRecord)
            lstrest = (From p In db.lib_Signatories Where Not If(p.IsVoid, False) Select New SignatoryRecord With {.ID = p.ID, .Name = p.Name, .Position = p.Position}).ToList
            If lstrest.Count < 1 Then Return New List(Of SignatoryRecord)
            Return lstrest
        End Function

        Public Shared Function getSigByObrIDA(id As Guid) As SignatoryRecord
            Dim db As New dvdll.dvDbEntities
            Dim itm As New SignatoryRecord

            obritm = (From p In db.or_ObligationRequest Where p.ID = id Select p.SignatoryIdA).FirstOrDefault

            itm = (From p In db.lib_Signatories Where p.ID = obritm Select New SignatoryRecord With {.ID = p.ID, _
                                                                                    .Name = p.Name, .Position = p.Position}).FirstOrDefault
            Return itm
        End Function

        Public Shared Function getSigByObrIDB(id As Guid) As SignatoryRecord
            Dim db As New dvdll.dvDbEntities
            Dim itm As New SignatoryRecord

            obritm = (From p In db.or_ObligationRequest Where p.ID = id Select p.SignatoryIdB).FirstOrDefault

            itm = (From p In db.lib_Signatories Where p.ID = obritm Select New SignatoryRecord With {.ID = p.ID, _
                                                                                    .Name = p.Name, .Position = p.Position}).FirstOrDefault
            Return itm
        End Function

        Public Shared Function getSigByID(id As Guid) As SignatoryRecord
            Dim db As New dvdll.dvDbEntities
            Dim sigitm As New SignatoryRecord

            sigitm = (From p In db.lib_Signatories Where p.ID = id Select New SignatoryRecord With {.ID = p.ID, .Name = p.Name, .Position = p.Position}).FirstOrDefault
            Return sigitm
        End Function

        Public Shared Sub addSignatory(id As Guid, name As String, position As String)
            Dim db As New dvdll.dvDbEntities

            Dim newsignatory As New dvdll.lib_Signatories
            With newsignatory
                .ID = Guid.NewGuid
                .Name = name
                .Position = position
                .IsVoid = False
            End With
            db.lib_Signatories.Add(newsignatory)
            db.SaveChanges()
        End Sub

        Public Shared Sub updateSignatory(id As Guid, updatename As String, updateposition As String)
            Dim db As New dvdll.dvDbEntities
            Dim updatesignatory = (From p In db.lib_Signatories Where p.ID = id Select p).FirstOrDefault
            If Not updatesignatory Is Nothing Then
                With updatesignatory
                    .Name = updatename
                    .Position = updateposition
                End With
                db.SaveChanges()
            End If
        End Sub

        Public Shared Sub activateSigByID(id As Guid)
            Dim db As New dvdll.dvDbEntities

            Dim rec = (From p In db.lib_Signatories Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsVoid = False
                db.SaveChanges()
            End If
        End Sub

        Public Shared Sub deleteSigByID(id As Guid)
            Dim db As New dvdll.dvDbEntities

            Dim rec = (From p In db.lib_Signatories Where p.ID = id And Not If(p.IsVoid, False) Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsVoid = True
                db.SaveChanges()
            End If

        End Sub



        <Serializable()> _
        Public Class SignatoryRecord
            Public Property ID As Guid
            Public Property Name As String
            Public Property Position As String
            Public Property IsVoid As Boolean
        End Class

    End Class
End Class
