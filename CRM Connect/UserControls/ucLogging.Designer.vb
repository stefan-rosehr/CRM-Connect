<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucLogging
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
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucLogging))
    Me.C1InputPanel1 = New C1.Win.C1InputPanel.C1InputPanel()
    Me.bsLOGGING = New System.Windows.Forms.BindingSource(Me.components)
    Me.DsPrinsTables = New CRM_Connect.dsPrinsTables()
    Me.hdrTBL_LOGGING = New C1.Win.C1InputPanel.InputGroupHeader()
    Me.sepLine = New C1.Win.C1InputPanel.InputSeparator()
    Me.lblID = New C1.Win.C1InputPanel.InputLabel()
    Me.numID = New C1.Win.C1InputPanel.InputNumericBox()
    Me.lblSOURCE = New C1.Win.C1InputPanel.InputLabel()
    Me.cbSOURCE = New C1.Win.C1InputPanel.InputComboBox()
    Me.lblSUBTYPE = New C1.Win.C1InputPanel.InputLabel()
    Me.numSUBTYPE = New C1.Win.C1InputPanel.InputNumericBox()
    Me.InputLabel1 = New C1.Win.C1InputPanel.InputLabel()
    Me.chkACK_NEEDED = New C1.Win.C1InputPanel.InputCheckBox()
    Me.InputLabel2 = New C1.Win.C1InputPanel.InputLabel()
    Me.chkACK_OK = New C1.Win.C1InputPanel.InputCheckBox()
    Me.lblMSG_TEXT = New C1.Win.C1InputPanel.InputLabel()
    Me.txtMSG_TEXT = New C1.Win.C1InputPanel.InputTextBox()
    Me.lblINSERTED = New C1.Win.C1InputPanel.InputLabel()
    Me.dtpINSERTED = New C1.Win.C1InputPanel.InputDatePicker()
    Me.tpINSERTED = New C1.Win.C1InputPanel.InputTimePicker()
    Me.lblINSERTED_BY = New C1.Win.C1InputPanel.InputLabel()
    Me.txtINSERTED_BY = New C1.Win.C1InputPanel.InputTextBox()
    Me.lblLAST_CHANGE = New C1.Win.C1InputPanel.InputLabel()
    Me.dtpLAST_CHANGE = New C1.Win.C1InputPanel.InputDatePicker()
    Me.tpLAST_CHANGE = New C1.Win.C1InputPanel.InputTimePicker()
    Me.lblCHANGED_BY = New C1.Win.C1InputPanel.InputLabel()
    Me.txtCHANGED_BY = New C1.Win.C1InputPanel.InputTextBox()
    Me.lblACK_DONE = New C1.Win.C1InputPanel.InputLabel()
    Me.dtpACK_DONE = New C1.Win.C1InputPanel.InputDatePicker()
    Me.tbACK_DONE = New C1.Win.C1InputPanel.InputTimePicker()
    Me.lblRCT_TIME = New C1.Win.C1InputPanel.InputLabel()
    Me.dtpRCT_TIME = New C1.Win.C1InputPanel.InputDatePicker()
    Me.tbRCT_TIME = New C1.Win.C1InputPanel.InputTimePicker()
    Me.lblSEVERITY = New C1.Win.C1InputPanel.InputLabel()
    Me.cbSEVERITY = New C1.Win.C1InputPanel.InputComboBox()
    Me.InputSeparator1 = New C1.Win.C1InputPanel.InputSeparator()
    Me.lblREMARK = New C1.Win.C1InputPanel.InputLabel()
    Me.txtREMARK = New C1.Win.C1InputPanel.InputTextBox()
    Me.taLOGGING = New CRM_Connect.dsPrinsTablesTableAdapters.TBL_LOGGINGTableAdapter()
    Me.fg = New C1.Win.C1FlexGrid.C1FlexGrid()
    CType(Me.C1InputPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.bsLOGGING, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DsPrinsTables, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fg, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'C1InputPanel1
    '
    Me.C1InputPanel1.DataSource = Me.bsLOGGING
    Me.C1InputPanel1.Dock = System.Windows.Forms.DockStyle.Top
    Me.C1InputPanel1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
    Me.C1InputPanel1.Items.Add(Me.hdrTBL_LOGGING)
    Me.C1InputPanel1.Items.Add(Me.sepLine)
    Me.C1InputPanel1.Items.Add(Me.lblID)
    Me.C1InputPanel1.Items.Add(Me.numID)
    Me.C1InputPanel1.Items.Add(Me.lblSOURCE)
    Me.C1InputPanel1.Items.Add(Me.cbSOURCE)
    Me.C1InputPanel1.Items.Add(Me.lblSUBTYPE)
    Me.C1InputPanel1.Items.Add(Me.numSUBTYPE)
    Me.C1InputPanel1.Items.Add(Me.InputLabel1)
    Me.C1InputPanel1.Items.Add(Me.chkACK_NEEDED)
    Me.C1InputPanel1.Items.Add(Me.InputLabel2)
    Me.C1InputPanel1.Items.Add(Me.chkACK_OK)
    Me.C1InputPanel1.Items.Add(Me.lblMSG_TEXT)
    Me.C1InputPanel1.Items.Add(Me.txtMSG_TEXT)
    Me.C1InputPanel1.Items.Add(Me.lblINSERTED)
    Me.C1InputPanel1.Items.Add(Me.dtpINSERTED)
    Me.C1InputPanel1.Items.Add(Me.tpINSERTED)
    Me.C1InputPanel1.Items.Add(Me.lblINSERTED_BY)
    Me.C1InputPanel1.Items.Add(Me.txtINSERTED_BY)
    Me.C1InputPanel1.Items.Add(Me.lblLAST_CHANGE)
    Me.C1InputPanel1.Items.Add(Me.dtpLAST_CHANGE)
    Me.C1InputPanel1.Items.Add(Me.tpLAST_CHANGE)
    Me.C1InputPanel1.Items.Add(Me.lblCHANGED_BY)
    Me.C1InputPanel1.Items.Add(Me.txtCHANGED_BY)
    Me.C1InputPanel1.Items.Add(Me.lblACK_DONE)
    Me.C1InputPanel1.Items.Add(Me.dtpACK_DONE)
    Me.C1InputPanel1.Items.Add(Me.tbACK_DONE)
    Me.C1InputPanel1.Items.Add(Me.lblRCT_TIME)
    Me.C1InputPanel1.Items.Add(Me.dtpRCT_TIME)
    Me.C1InputPanel1.Items.Add(Me.tbRCT_TIME)
    Me.C1InputPanel1.Items.Add(Me.lblSEVERITY)
    Me.C1InputPanel1.Items.Add(Me.cbSEVERITY)
    Me.C1InputPanel1.Items.Add(Me.InputSeparator1)
    Me.C1InputPanel1.Items.Add(Me.lblREMARK)
    Me.C1InputPanel1.Items.Add(Me.txtREMARK)
    Me.C1InputPanel1.Location = New System.Drawing.Point(0, 0)
    Me.C1InputPanel1.Name = "C1InputPanel1"
    Me.C1InputPanel1.Size = New System.Drawing.Size(851, 325)
    Me.C1InputPanel1.TabIndex = 0
    '
    'bsLOGGING
    '
    Me.bsLOGGING.DataMember = "TBL_LOGGING"
    Me.bsLOGGING.DataSource = Me.DsPrinsTables
    '
    'DsPrinsTables
    '
    Me.DsPrinsTables.DataSetName = "dsPrinsTables"
    Me.DsPrinsTables.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
    '
    'hdrTBL_LOGGING
    '
    Me.hdrTBL_LOGGING.Name = "hdrTBL_LOGGING"
    '
    'sepLine
    '
    Me.sepLine.Break = C1.Win.C1InputPanel.BreakType.Group
    Me.sepLine.ElementHeight = 0
    Me.sepLine.Height = 11
    Me.sepLine.Name = "sepLine"
    Me.sepLine.Width = 692
    '
    'lblID
    '
    Me.lblID.Name = "lblID"
    Me.lblID.Text = "ID"
    Me.lblID.Width = 86
    '
    'numID
    '
    Me.numID.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "ID", True))
    Me.numID.Format = "0"
    Me.numID.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
    Me.numID.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
    Me.numID.Name = "numID"
    Me.numID.ReadOnly = True
    Me.numID.Width = 300
    '
    'lblSOURCE
    '
    Me.lblSOURCE.Name = "lblSOURCE"
    Me.lblSOURCE.Text = "Herkunft"
    Me.lblSOURCE.Width = 86
    '
    'cbSOURCE
    '
    Me.cbSOURCE.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.bsLOGGING, "SOURCE", True))
    Me.cbSOURCE.Name = "cbSOURCE"
    Me.cbSOURCE.ReadOnly = True
    Me.cbSOURCE.Width = 300
    '
    'lblSUBTYPE
    '
    Me.lblSUBTYPE.Name = "lblSUBTYPE"
    Me.lblSUBTYPE.Text = "Unter-Typ"
    Me.lblSUBTYPE.Width = 86
    '
    'numSUBTYPE
    '
    Me.numSUBTYPE.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "SUBTYPE", True))
    Me.numSUBTYPE.Format = "0"
    Me.numSUBTYPE.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
    Me.numSUBTYPE.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
    Me.numSUBTYPE.Name = "numSUBTYPE"
    Me.numSUBTYPE.ReadOnly = True
    Me.numSUBTYPE.Width = 300
    '
    'InputLabel1
    '
    Me.InputLabel1.Name = "InputLabel1"
    Me.InputLabel1.Text = "Bestätigung erforderlich"
    Me.InputLabel1.Width = 86
    '
    'chkACK_NEEDED
    '
    Me.chkACK_NEEDED.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "ACK_NEEDED", True))
    Me.chkACK_NEEDED.Name = "chkACK_NEEDED"
    '
    'InputLabel2
    '
    Me.InputLabel2.Name = "InputLabel2"
    Me.InputLabel2.Text = "Bestätigt"
    Me.InputLabel2.Width = 86
    '
    'chkACK_OK
    '
    Me.chkACK_OK.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "ACK_OK", True))
    Me.chkACK_OK.Name = "chkACK_OK"
    '
    'lblMSG_TEXT
    '
    Me.lblMSG_TEXT.Name = "lblMSG_TEXT"
    Me.lblMSG_TEXT.Text = "Meldung"
    Me.lblMSG_TEXT.Width = 86
    '
    'txtMSG_TEXT
    '
    Me.txtMSG_TEXT.Break = C1.Win.C1InputPanel.BreakType.Column
    Me.txtMSG_TEXT.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "MSG_TEXT", True))
    Me.txtMSG_TEXT.Height = 42
    Me.txtMSG_TEXT.Multiline = True
    Me.txtMSG_TEXT.Name = "txtMSG_TEXT"
    Me.txtMSG_TEXT.ReadOnly = True
    Me.txtMSG_TEXT.VerticalAlign = C1.Win.C1InputPanel.InputContentAlignment.Spread
    Me.txtMSG_TEXT.Width = 300
    '
    'lblINSERTED
    '
    Me.lblINSERTED.Name = "lblINSERTED"
    Me.lblINSERTED.Text = "Eingefügt"
    Me.lblINSERTED.Width = 86
    '
    'dtpINSERTED
    '
    Me.dtpINSERTED.Break = C1.Win.C1InputPanel.BreakType.None
    Me.dtpINSERTED.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "INSERTED", True))
    Me.dtpINSERTED.Name = "dtpINSERTED"
    Me.dtpINSERTED.ReadOnly = True
    Me.dtpINSERTED.Width = 115
    '
    'tpINSERTED
    '
    Me.tpINSERTED.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "INSERTED", True))
    Me.tpINSERTED.Format = "T"
    Me.tpINSERTED.Name = "tpINSERTED"
    Me.tpINSERTED.ReadOnly = True
    Me.tpINSERTED.Width = 81
    '
    'lblINSERTED_BY
    '
    Me.lblINSERTED_BY.Name = "lblINSERTED_BY"
    Me.lblINSERTED_BY.Text = "durch"
    Me.lblINSERTED_BY.Width = 86
    '
    'txtINSERTED_BY
    '
    Me.txtINSERTED_BY.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "INSERTED_BY", True))
    Me.txtINSERTED_BY.Name = "txtINSERTED_BY"
    Me.txtINSERTED_BY.ReadOnly = True
    Me.txtINSERTED_BY.Width = 200
    '
    'lblLAST_CHANGE
    '
    Me.lblLAST_CHANGE.Name = "lblLAST_CHANGE"
    Me.lblLAST_CHANGE.Text = "letzte Änderung"
    Me.lblLAST_CHANGE.Width = 86
    '
    'dtpLAST_CHANGE
    '
    Me.dtpLAST_CHANGE.Break = C1.Win.C1InputPanel.BreakType.None
    Me.dtpLAST_CHANGE.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "LAST_CHANGE", True))
    Me.dtpLAST_CHANGE.Name = "dtpLAST_CHANGE"
    Me.dtpLAST_CHANGE.ReadOnly = True
    Me.dtpLAST_CHANGE.Width = 115
    '
    'tpLAST_CHANGE
    '
    Me.tpLAST_CHANGE.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "LAST_CHANGE", True))
    Me.tpLAST_CHANGE.Format = "T"
    Me.tpLAST_CHANGE.Name = "tpLAST_CHANGE"
    Me.tpLAST_CHANGE.ReadOnly = True
    Me.tpLAST_CHANGE.Width = 81
    '
    'lblCHANGED_BY
    '
    Me.lblCHANGED_BY.Name = "lblCHANGED_BY"
    Me.lblCHANGED_BY.Text = "durch"
    Me.lblCHANGED_BY.Width = 86
    '
    'txtCHANGED_BY
    '
    Me.txtCHANGED_BY.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "CHANGED_BY", True))
    Me.txtCHANGED_BY.Name = "txtCHANGED_BY"
    Me.txtCHANGED_BY.ReadOnly = True
    Me.txtCHANGED_BY.Width = 200
    '
    'lblACK_DONE
    '
    Me.lblACK_DONE.Name = "lblACK_DONE"
    Me.lblACK_DONE.Text = "Zeit bestätigt"
    Me.lblACK_DONE.Width = 86
    '
    'dtpACK_DONE
    '
    Me.dtpACK_DONE.Break = C1.Win.C1InputPanel.BreakType.None
    Me.dtpACK_DONE.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "ACK_DONE", True))
    Me.dtpACK_DONE.Name = "dtpACK_DONE"
    Me.dtpACK_DONE.ReadOnly = True
    Me.dtpACK_DONE.Width = 115
    '
    'tbACK_DONE
    '
    Me.tbACK_DONE.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "ACK_DONE", True))
    Me.tbACK_DONE.Format = "T"
    Me.tbACK_DONE.Name = "tbACK_DONE"
    Me.tbACK_DONE.ReadOnly = True
    Me.tbACK_DONE.Width = 81
    '
    'lblRCT_TIME
    '
    Me.lblRCT_TIME.Name = "lblRCT_TIME"
    Me.lblRCT_TIME.Text = "Wiedervorlage am"
    Me.lblRCT_TIME.Width = 86
    '
    'dtpRCT_TIME
    '
    Me.dtpRCT_TIME.Break = C1.Win.C1InputPanel.BreakType.None
    Me.dtpRCT_TIME.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "RCT_TIME", True))
    Me.dtpRCT_TIME.Name = "dtpRCT_TIME"
    Me.dtpRCT_TIME.Width = 115
    '
    'tbRCT_TIME
    '
    Me.tbRCT_TIME.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "RCT_TIME", True))
    Me.tbRCT_TIME.Format = "T"
    Me.tbRCT_TIME.Name = "tbRCT_TIME"
    Me.tbRCT_TIME.Width = 81
    '
    'lblSEVERITY
    '
    Me.lblSEVERITY.Name = "lblSEVERITY"
    Me.lblSEVERITY.Text = "Schweregrad"
    Me.lblSEVERITY.Width = 86
    '
    'cbSEVERITY
    '
    Me.cbSEVERITY.Break = C1.Win.C1InputPanel.BreakType.Group
    Me.cbSEVERITY.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.bsLOGGING, "SEVERITY", True))
    Me.cbSEVERITY.Name = "cbSEVERITY"
    Me.cbSEVERITY.ReadOnly = True
    Me.cbSEVERITY.Width = 198
    '
    'InputSeparator1
    '
    Me.InputSeparator1.Break = C1.Win.C1InputPanel.BreakType.Group
    Me.InputSeparator1.ElementHeight = 0
    Me.InputSeparator1.Height = 11
    Me.InputSeparator1.Name = "InputSeparator1"
    Me.InputSeparator1.Width = 692
    '
    'lblREMARK
    '
    Me.lblREMARK.Name = "lblREMARK"
    Me.lblREMARK.Text = "Bemerkung"
    Me.lblREMARK.Width = 86
    '
    'txtREMARK
    '
    Me.txtREMARK.DataBindings.Add(New System.Windows.Forms.Binding("BoundValue", Me.bsLOGGING, "REMARK", True))
    Me.txtREMARK.Height = 85
    Me.txtREMARK.Multiline = True
    Me.txtREMARK.Name = "txtREMARK"
    Me.txtREMARK.VerticalAlign = C1.Win.C1InputPanel.InputContentAlignment.Spread
    Me.txtREMARK.Width = 602
    '
    'taLOGGING
    '
    Me.taLOGGING.ClearBeforeFill = True
    '
    'fg
    '
    Me.fg.ColumnInfo = resources.GetString("fg.ColumnInfo")
    Me.fg.DataSource = Me.bsLOGGING
    Me.fg.Dock = System.Windows.Forms.DockStyle.Fill
    Me.fg.Location = New System.Drawing.Point(0, 325)
    Me.fg.Name = "fg"
    Me.fg.Rows.Count = 1
    Me.fg.Rows.DefaultSize = 19
    Me.fg.Size = New System.Drawing.Size(851, 155)
    Me.fg.TabIndex = 1
    '
    'ucLogging
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.fg)
    Me.Controls.Add(Me.C1InputPanel1)
    Me.Name = "ucLogging"
    Me.Size = New System.Drawing.Size(851, 480)
    CType(Me.C1InputPanel1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.bsLOGGING, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DsPrinsTables, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fg, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents C1InputPanel1 As C1.Win.C1InputPanel.C1InputPanel
  Friend WithEvents bsLOGGING As System.Windows.Forms.BindingSource
  Friend WithEvents DsPrinsTables As CRM_Connect.dsPrinsTables
  Friend WithEvents taLOGGING As CRM_Connect.dsPrinsTablesTableAdapters.TBL_LOGGINGTableAdapter
  Friend WithEvents fg As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents hdrTBL_LOGGING As C1.Win.C1InputPanel.InputGroupHeader
  Friend WithEvents lblID As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents numID As C1.Win.C1InputPanel.InputNumericBox
  Friend WithEvents lblSOURCE As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents lblSUBTYPE As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents numSUBTYPE As C1.Win.C1InputPanel.InputNumericBox
  Friend WithEvents lblMSG_TEXT As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents txtMSG_TEXT As C1.Win.C1InputPanel.InputTextBox
  Friend WithEvents lblINSERTED As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents dtpINSERTED As C1.Win.C1InputPanel.InputDatePicker
  Friend WithEvents lblINSERTED_BY As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents txtINSERTED_BY As C1.Win.C1InputPanel.InputTextBox
  Friend WithEvents lblLAST_CHANGE As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents dtpLAST_CHANGE As C1.Win.C1InputPanel.InputDatePicker
  Friend WithEvents lblCHANGED_BY As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents txtCHANGED_BY As C1.Win.C1InputPanel.InputTextBox
  Friend WithEvents chkACK_NEEDED As C1.Win.C1InputPanel.InputCheckBox
  Friend WithEvents chkACK_OK As C1.Win.C1InputPanel.InputCheckBox
  Friend WithEvents lblACK_DONE As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents dtpACK_DONE As C1.Win.C1InputPanel.InputDatePicker
  Friend WithEvents lblRCT_TIME As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents dtpRCT_TIME As C1.Win.C1InputPanel.InputDatePicker
  Friend WithEvents lblREMARK As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents txtREMARK As C1.Win.C1InputPanel.InputTextBox
  Friend WithEvents lblSEVERITY As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents cbSOURCE As C1.Win.C1InputPanel.InputComboBox
  Friend WithEvents InputLabel1 As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents InputLabel2 As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents tpINSERTED As C1.Win.C1InputPanel.InputTimePicker
  Friend WithEvents tpLAST_CHANGE As C1.Win.C1InputPanel.InputTimePicker
  Friend WithEvents tbACK_DONE As C1.Win.C1InputPanel.InputTimePicker
  Friend WithEvents tbRCT_TIME As C1.Win.C1InputPanel.InputTimePicker
  Friend WithEvents sepLine As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents cbSEVERITY As C1.Win.C1InputPanel.InputComboBox
  Friend WithEvents InputSeparator1 As C1.Win.C1InputPanel.InputSeparator

End Class
