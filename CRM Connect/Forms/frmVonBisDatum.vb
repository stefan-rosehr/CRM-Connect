Public Class frmVonBisDatum

  Public Property VonDatum As Date
    Get
      Return UcVonBisDatum1.VonDatum
    End Get
    Set(value As Date)
      UcVonBisDatum1.VonDatum = value
    End Set
  End Property

  Public Property BisDatum As Date
    Get
      Return UcVonBisDatum1.BisDatum
    End Get
    Set(value As Date)
      UcVonBisDatum1.BisDatum = value
    End Set
  End Property


  Public Property PrjNummer As Integer
    Get
      Return UcVonBisDatum1.PrjNummer
    End Get
    Set(value As Integer)
      UcVonBisDatum1.PrjNummer = value
    End Set
  End Property



End Class