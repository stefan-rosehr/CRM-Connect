<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVonBisDatum
  Inherits System.Windows.Forms.Form

  'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Wird vom Windows Form-Designer benötigt.
  Private components As System.ComponentModel.IContainer

  'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
  'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
  'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.UcVonBisDatum1 = New CRM_Connect.ucVonBisDatum()
        Me.SuspendLayout()
        '
        'UcVonBisDatum1
        '
        Me.UcVonBisDatum1.BisDatum = New Date(CType(0, Long))
        Me.UcVonBisDatum1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcVonBisDatum1.Location = New System.Drawing.Point(0, 0)
        Me.UcVonBisDatum1.Name = "UcVonBisDatum1"
        Me.UcVonBisDatum1.PrjNummer = -1
        Me.UcVonBisDatum1.Size = New System.Drawing.Size(272, 153)
        Me.UcVonBisDatum1.TabIndex = 0
        Me.UcVonBisDatum1.VonDatum = New Date(CType(0, Long))
        '
        'frmVonBisDatum
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(272, 153)
        Me.Controls.Add(Me.UcVonBisDatum1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmVonBisDatum"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents UcVonBisDatum1 As ucVonBisDatum
End Class
