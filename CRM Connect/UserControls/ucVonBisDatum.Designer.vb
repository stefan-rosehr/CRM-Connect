<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucVonBisDatum
  Inherits System.Windows.Forms.UserControl

  'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
    Me.C1InputPanel1 = New C1.Win.C1InputPanel.C1InputPanel()
    Me.InputGroupHeader1 = New C1.Win.C1InputPanel.InputGroupHeader()
        Me.InputLabel1 = New C1.Win.C1InputPanel.InputLabel()
        Me.iDtpVon = New C1.Win.C1InputPanel.InputDatePicker()
        Me.InputLabel2 = New C1.Win.C1InputPanel.InputLabel()
        Me.iDtpBis = New C1.Win.C1InputPanel.InputDatePicker()
        Me.lbPrjNummer = New C1.Win.C1InputPanel.InputLabel()
        Me.iTxtProjektNr = New C1.Win.C1InputPanel.InputTextBox()
        Me.InputSeparator1 = New C1.Win.C1InputPanel.InputSeparator()
        Me.btnOk = New C1.Win.C1InputPanel.InputButton()
        Me.btnCancel = New C1.Win.C1InputPanel.InputButton()
        CType(Me.C1InputPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'C1InputPanel1
        '
        Me.C1InputPanel1.AutoSizeElement = C1.Framework.AutoSizeElement.Both
        Me.C1InputPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.C1InputPanel1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.C1InputPanel1.Items.Add(Me.InputGroupHeader1)
        Me.C1InputPanel1.Items.Add(Me.InputLabel1)
        Me.C1InputPanel1.Items.Add(Me.iDtpVon)
        Me.C1InputPanel1.Items.Add(Me.InputLabel2)
        Me.C1InputPanel1.Items.Add(Me.iDtpBis)
        Me.C1InputPanel1.Items.Add(Me.lbPrjNummer)
        Me.C1InputPanel1.Items.Add(Me.iTxtProjektNr)
        Me.C1InputPanel1.Items.Add(Me.InputSeparator1)
        Me.C1InputPanel1.Items.Add(Me.btnOk)
        Me.C1InputPanel1.Items.Add(Me.btnCancel)
        Me.C1InputPanel1.Location = New System.Drawing.Point(0, 0)
        Me.C1InputPanel1.Name = "C1InputPanel1"
        Me.C1InputPanel1.Size = New System.Drawing.Size(239, 155)
        Me.C1InputPanel1.TabIndex = 0
        '
        'InputGroupHeader1
        '
        Me.InputGroupHeader1.Name = "InputGroupHeader1"
        '
        'InputLabel1
        '
        Me.InputLabel1.Name = "InputLabel1"
        Me.InputLabel1.Text = "Von Datum"
        Me.InputLabel1.Width = 100
        '
        'iDtpVon
        '
        Me.iDtpVon.Name = "iDtpVon"
        '
        'InputLabel2
        '
        Me.InputLabel2.Name = "InputLabel2"
        Me.InputLabel2.Text = "Bis Datum"
        Me.InputLabel2.Width = 100
        '
        'iDtpBis
        '
        Me.iDtpBis.Name = "iDtpBis"
        '
        'lbPrjNummer
        '
        Me.lbPrjNummer.Name = "lbPrjNummer"
        Me.lbPrjNummer.Text = "Projekt Nr. (Restruct)"
        Me.lbPrjNummer.Width = 100
        '
        'iTxtProjektNr
        '
        Me.iTxtProjektNr.Name = "iTxtProjektNr"
        '
        'InputSeparator1
        '
        Me.InputSeparator1.Name = "InputSeparator1"
        Me.InputSeparator1.Width = 200
        '
        'btnOk
        '
        Me.btnOk.Break = C1.Win.C1InputPanel.BreakType.None
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Text = "OK"
        Me.btnOk.Width = 100
        '
        'btnCancel
        '
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Text = "Abbrechen"
        Me.btnCancel.Width = 100
        '
        'ucVonBisDatum
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.C1InputPanel1)
        Me.Name = "ucVonBisDatum"
        Me.Size = New System.Drawing.Size(239, 155)
        CType(Me.C1InputPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents C1InputPanel1 As C1.Win.C1InputPanel.C1InputPanel
  Friend WithEvents InputGroupHeader1 As C1.Win.C1InputPanel.InputGroupHeader
  Friend WithEvents InputLabel1 As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents iDtpVon As C1.Win.C1InputPanel.InputDatePicker
  Friend WithEvents InputLabel2 As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents iDtpBis As C1.Win.C1InputPanel.InputDatePicker
  Friend WithEvents InputSeparator1 As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents btnOk As C1.Win.C1InputPanel.InputButton
  Friend WithEvents btnCancel As C1.Win.C1InputPanel.InputButton
    Friend WithEvents lbPrjNummer As C1.Win.C1InputPanel.InputLabel
    Friend WithEvents iTxtProjektNr As C1.Win.C1InputPanel.InputTextBox
End Class
