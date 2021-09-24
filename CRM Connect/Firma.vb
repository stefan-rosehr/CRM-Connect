Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("Firma")>
Partial Public Class Firma
    Public Property Id As Integer

    <StringLength(40)>
    Public Property FirmenID As String

    <StringLength(20)>
    Public Property Firmennummer As String

    <Required>
    <StringLength(160)>
    Public Property Firmenname1 As String

    <StringLength(160)>
    Public Property Firmenname2 As String

    <StringLength(160)>
    Public Property Firmenname3 As String

    <StringLength(250)>
    Public Property Anschrift As String

    <StringLength(20)>
    Public Property PLZ As String

    <StringLength(80)>
    Public Property Ort As String

    <StringLength(80)>
    Public Property Land As String

    <StringLength(50)>
    Public Property Telefon As String

    <StringLength(50)>
    Public Property Fax As String

    <StringLength(200)>
    Public Property Homepage As String

    <StringLength(20)>
    Public Property Sprache As String

    <StringLength(100)>
    Public Property EMail As String

    <StringLength(20)>
    Public Property Besitzer As String

    Public Property Zielsystem As Integer?
End Class
