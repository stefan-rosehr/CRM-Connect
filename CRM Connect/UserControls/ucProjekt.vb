Public Class ucProjekt
  Implements ifTableAdapter
  Implements ifRecordEvent


  Private _AuftragRecordEvents As clRecordEvents
  Private ReadOnly Property ProjektRecordEvents As clRecordEvents
    Get
      If IsNothing(_AuftragRecordEvents) Then
        _AuftragRecordEvents = New clRecordEvents(Me.bsProjekt, Me)
      End If
      Return _AuftragRecordEvents
    End Get
  End Property


  Public Sub Reload() Implements ifTableAdapter.Reload
    Me.ProjektTableAdapter.Fill(DsSQL_CRM_Prins.Projekt)
  End Sub

  Public Sub Save() Implements ifTableAdapter.Save
    Me.ProjektTableAdapter.Update(DsSQL_CRM_Prins.Projekt)
  End Sub

  Public ReadOnly Property ActiveRecordEvents As clRecordEvents Implements ifRecordEvent.ActiveRecordEvents
    Get
      Return ProjektRecordEvents
    End Get
  End Property

  Private Sub ucFirma_Load(sender As Object, e As EventArgs) Handles Me.Load
    ProjektRecordEvents.Reload()
  End Sub
End Class
