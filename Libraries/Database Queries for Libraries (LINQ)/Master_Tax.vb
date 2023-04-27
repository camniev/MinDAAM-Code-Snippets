Partial Public Class Master

    Partial Public Class Tax

        Public Shared Function getTax() As List(Of TaxRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of TaxRecord)
            lstrest = (From p In db.lib_Tax Order By p.TaxDescription Ascending Select New TaxRecord With {.ID = p.ID,
                                                                                                     .TaxATC = p.TaxATC,
                                                                                                     .TaxDescription = p.TaxDescription,
                                                                                                     .TaxLongDescription = p.TaxLongDescription,
                                                                                                     .TaxPercentage = p.TaxPercentage,
                                                                                                     .TaxShortDesc = p.TaxShortDesc,
                                                                                                     .TaxType = p.TaxType,
                                                                                                    .IsVoid = p.IsVoid}).ToList
            If lstrest.Count < 1 Then Return New List(Of TaxRecord)
            Return lstrest

        End Function

        Public Shared Function getTaxCtrl() As List(Of TaxRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstrest As New List(Of TaxRecord)
            lstrest = (From p In db.lib_Tax Where Not If(p.IsVoid, False) Order By p.TaxDescription Ascending Select New TaxRecord With {.ID = p.ID,
                                                                                                     .TaxATC = p.TaxATC,
                                                                                                     .TaxDescription = p.TaxDescription,
                                                                                                     .TaxLongDescription = p.TaxLongDescription,
                                                                                                     .TaxPercentage = p.TaxPercentage,
                                                                                                     .TaxShortDesc = p.TaxShortDesc,
                                                                                                     .TaxType = p.TaxType,
                                                                                                    .IsVoid = p.IsVoid}).ToList
            If lstrest.Count < 1 Then Return New List(Of TaxRecord)
            Return lstrest

        End Function
        Public Shared Function getTaxByFilter(filter As String) As List(Of TaxRecord)
            Dim db As New dvdll.dvDbEntities
            Dim lstRet As New List(Of TaxRecord)

            filter = filter.Trim.ToLower
            lstRet = (From p In db.lib_Tax Where p.TaxDescription.Trim.ToLower.Contains(filter) And If(p.IsVoid, False) = False _
                      Select New TaxRecord With {.ID = p.ID, .TaxDescription = p.TaxDescription, .TaxPercentage = p.TaxPercentage,
                                                 .TaxShortDesc = p.TaxShortDesc,
                                                 .TaxType = p.TaxType}).ToList

            Return lstRet
        End Function

        Public Shared Function getTaxbyID(id As Guid) As TaxRecord
            Dim db As New dvdll.dvDbEntities
            Dim itm As New TaxRecord

            itm = (From p In db.lib_Tax Where p.ID = id
                   Select New TaxRecord With {.ID = p.ID, .TaxATC = p.TaxATC, .TaxLongDescription = p.TaxLongDescription, .TaxDescription = p.TaxDescription,
                                              .TaxPercentage = p.TaxPercentage, .TaxShortDesc = p.TaxShortDesc,
                                              .TaxType = p.TaxType}).FirstOrDefault
            Return itm
        End Function

        Public Shared Sub addNewTax(taxdesc As String, taxperc As String, taxShortDesc As String, taxType As Integer, taxATC As String, taxLongDesc As String)
            Dim db As New dvdll.dvDbEntities

            Dim newItem As New dvdll.lib_Tax
            With newItem
                .ID = Guid.NewGuid
                .TaxATC = taxATC
                .TaxDescription = taxdesc
                .TaxLongDescription = taxLongDesc
                .TaxPercentage = taxperc
                .TaxShortDesc = taxShortDesc
                .TaxType = taxType
                .IsVoid = False
            End With
            db.lib_Tax.Add(newItem)
            db.SaveChanges()
        End Sub

        Public Shared Sub updateTaxByID(id As Guid, taxdesc As String, taxperc As String, taxShortDesc As String, taxType As Integer, taxATC As String, taxLongDesc As String)
            Dim db As New dvdll.dvDbEntities

            Dim itm = (From p In db.lib_Tax Where p.ID = id Select p).FirstOrDefault
            If Not itm Is Nothing Then
                With itm
                    .TaxATC = taxATC
                    .TaxDescription = taxdesc
                    .TaxLongDescription = taxLongDesc
                    .TaxPercentage = taxperc
                    .TaxShortDesc = taxShortDesc
                    .TaxType = taxType
                End With
                db.SaveChanges()
            End If
        End Sub

        Public Shared Sub activateTaxByID(id As Guid)
            Dim db As New dvdll.dvDbEntities

            Dim rec = (From p In db.lib_Tax Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsVoid = False
                db.SaveChanges()
            End If
        End Sub

        Public Shared Sub deleteTaxByID(id As Guid)
            Dim db As New dvdll.dvDbEntities

            Dim rec = (From p In db.lib_Tax Where p.ID = id Select p).FirstOrDefault
            If Not rec Is Nothing Then
                rec.IsVoid = True
                db.SaveChanges()
            End If
        End Sub

        <Serializable()> _
        Public Class TaxRecord
            Public Property ID As Guid
            Public Property TaxATC As String
            Public Property TaxDescription As String
            Public Property TaxLongDescription As String
            Public Property TaxPercentage As String
            Public Property TaxShortDesc As String
            Public Property TaxType As Integer
            Public Property IsVoid As Boolean
            Public ReadOnly Property TaxTypeText
                Get
                    Select Case TaxType
                        Case 1
                            Return "Vat Registered"
                        Case 2
                            Return "Non-Vat Registered"
                        Case 3
                            'Return "Real Estate Rentals"
                            Return "Professional Fees"
                    End Select
                    Return ""
                End Get
            End Property

        End Class
    End Class

End Class
