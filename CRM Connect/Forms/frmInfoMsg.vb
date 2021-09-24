Public Class frmInfoMsg

  Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
    Me.Visible = False
  End Sub

  Public Property Message As String
    Get
      Return Me.lblMessage.Text
    End Get
    Set(value As String)
      Me.lblMessage.Text = value
    End Set
  End Property


End Class