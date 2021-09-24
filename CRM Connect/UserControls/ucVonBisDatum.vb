Public Class ucVonBisDatum


  Public Property VonDatum As Date
    Get
      Return iDtpVon.Value
    End Get
    Set(value As Date)
      iDtpVon.Value = value
    End Set
  End Property

  Public Property BisDatum As Date
    Get
      Return iDtpBis.Value
    End Get
    Set(value As Date)
      iDtpBis.Value = value
    End Set
  End Property

  Public Property PrjNummer As Integer
    Get
      Dim Result As Integer = -1

      Try
        Result = CInt(iTxtProjektNr.Text)
      Catch ex As Exception
      End Try

      Return Result
    End Get
    Set(value As Integer)
      If value >= 0 Then
        iTxtProjektNr.Text = value.ToString
      Else
        iTxtProjektNr.Text = String.Empty
      End If
    End Set
  End Property


  Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
    If Not IsNothing(ParentForm) Then
      ParentForm.DialogResult = DialogResult.OK
    End If
  End Sub

  Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    If Not IsNothing(ParentForm) Then
      ParentForm.DialogResult = DialogResult.Cancel
    End If
  End Sub
End Class
