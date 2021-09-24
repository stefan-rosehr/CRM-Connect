Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("Projekt")>
Partial Public Class Projekt
    Public Property Id As Integer

    Public Property Projektnummer As Integer

    Public Property Auftragsvolumen As Decimal?

    Public Property AuftragsvolumenGJ As Decimal?

    <Required>
    <StringLength(300)>
    Public Property Bezeichnung As String

    Public Property Geschaeftsjahr As Integer?

    <StringLength(50)>
    Public Property Kostenstelle As String

    Public Property Kundeid As Integer

    <StringLength(7)>
    Public Property Letzteabrechnungsperiode As String

    Public Property OffeneLeistungen As Decimal?

    Public Property PotRestumsatz As Decimal?

    <StringLength(100)>
    Public Property Projektleiter As String

    Public Property Projektstatus As Integer

    Public Property UmsatzGesamt As Decimal?

    Public Property UmsatzGJ As Decimal?

    Public Property UmsatzPeriode As Decimal?

    <StringLength(20)>
    Public Property Besitzer As String
End Class
