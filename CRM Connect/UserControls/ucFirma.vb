Public Class ucFirma
  Implements ifTableAdapter
  Implements ifRecordEvent


  Private _AuftragRecordEvents As clRecordEvents
  Private ReadOnly Property FirmaRecordEvents As clRecordEvents
    Get
      If IsNothing(_AuftragRecordEvents) Then
        _AuftragRecordEvents = New clRecordEvents(Me.bsFirma, Me)
      End If
      Return _AuftragRecordEvents
    End Get
  End Property


  Public Sub Reload() Implements ifTableAdapter.Reload
    Me.FirmaTableAdapter.Fill(DsSQL_CRM_Prins.Firma)
  End Sub

  Public Sub Save() Implements ifTableAdapter.Save
    Me.FirmaTableAdapter.Update(DsSQL_CRM_Prins.Firma)
  End Sub

  Public ReadOnly Property ActiveRecordEvents As clRecordEvents Implements ifRecordEvent.ActiveRecordEvents
    Get
      Return FirmaRecordEvents
    End Get
  End Property

  Private Sub ucFirma_Load(sender As Object, e As EventArgs) Handles Me.Load
    FirmaRecordEvents.Reload()
  End Sub
End Class
