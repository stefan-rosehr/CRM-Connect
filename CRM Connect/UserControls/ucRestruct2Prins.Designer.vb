<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucRestruct2Prins
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucRestruct2Prins))
    Me.ipTop = New C1.Win.C1InputPanel.C1InputPanel()
    Me.InputGroupHeader1 = New C1.Win.C1InputPanel.InputGroupHeader()
    Me.ibUpdateProjekte = New C1.Win.C1InputPanel.InputButton()
    Me.ibWBImport = New C1.Win.C1InputPanel.InputButton()
    Me.flex = New C1.Win.C1FlexGrid.C1FlexGrid()
    Me.DsRestruct = New CRM_Connect.dsRestruct()
    Me.DsRestructBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.taVProjekt_Kunde = New CRM_Connect.dsRestructTableAdapters.vProjekt_KundeTableAdapter()
    Me.taVWochenbericht = New CRM_Connect.dsRestructTableAdapters.vWochenberichtTableAdapter()
    Me.c1Progress = New C1.Win.C1InputPanel.InputProgressBar()
    Me.InputSeparator1 = New C1.Win.C1InputPanel.InputSeparator()
    Me.DsPrins = New CRM_Connect.dsPrins()
    Me.DsPrinsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.ibCancel = New C1.Win.C1InputPanel.InputButton()
    CType(Me.ipTop, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.flex, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DsRestruct, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DsRestructBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DsPrins, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DsPrinsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'ipTop
    '
    Me.ipTop.AutoSizeElement = C1.Framework.AutoSizeElement.Both
    Me.ipTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.ipTop.Font = New System.Drawing.Font("Segoe UI", 9.0!)
    Me.ipTop.Items.Add(Me.InputGroupHeader1)
    Me.ipTop.Items.Add(Me.ibUpdateProjekte)
    Me.ipTop.Items.Add(Me.ibWBImport)
    Me.ipTop.Items.Add(Me.ibCancel)
    Me.ipTop.Items.Add(Me.InputSeparator1)
    Me.ipTop.Items.Add(Me.c1Progress)
    Me.ipTop.Location = New System.Drawing.Point(0, 0)
    Me.ipTop.Name = "ipTop"
    Me.ipTop.Size = New System.Drawing.Size(845, 129)
    Me.ipTop.TabIndex = 0
    '
    'InputGroupHeader1
    '
    Me.InputGroupHeader1.Name = "InputGroupHeader1"
    '
    'ibUpdateProjekte
    '
    Me.ibUpdateProjekte.Name = "ibUpdateProjekte"
    Me.ibUpdateProjekte.Text = "Projekte übernehmen/aktualisieren"
    Me.ibUpdateProjekte.Width = 240
    '
    'ibWBImport
    '
    Me.ibWBImport.Break = C1.Win.C1InputPanel.BreakType.Column
    Me.ibWBImport.Name = "ibWBImport"
    Me.ibWBImport.Text = "Wochenberichte Vormonat übernehmen"
    Me.ibWBImport.Width = 240
    '
    'flex
    '
    Me.flex.AllowEditing = False
    Me.flex.AutoGenerateColumns = False
    Me.flex.ColumnInfo = resources.GetString("flex.ColumnInfo")
    Me.flex.Dock = System.Windows.Forms.DockStyle.Fill
    Me.flex.ExtendLastCol = True
    Me.flex.Location = New System.Drawing.Point(0, 129)
    Me.flex.Name = "flex"
    Me.flex.Rows.Count = 2
    Me.flex.Rows.DefaultSize = 19
    Me.flex.Size = New System.Drawing.Size(845, 445)
    Me.flex.TabIndex = 1
    '
    'DsRestruct
    '
    Me.DsRestruct.DataSetName = "dsRestruct"
    Me.DsRestruct.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
    '
    'DsRestructBindingSource
    '
    Me.DsRestructBindingSource.DataSource = Me.DsRestruct
    Me.DsRestructBindingSource.Position = 0
    '
    'taVProjekt_Kunde
    '
    Me.taVProjekt_Kunde.ClearBeforeFill = True
    '
    'taVWochenbericht
    '
    Me.taVWochenbericht.ClearBeforeFill = True
    '
    'c1Progress
    '
    Me.c1Progress.Name = "c1Progress"
    Me.c1Progress.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
    Me.c1Progress.Width = 489
    '
    'InputSeparator1
    '
    Me.InputSeparator1.Height = 18
    Me.InputSeparator1.Name = "InputSeparator1"
    Me.InputSeparator1.Width = 489
    '
    'DsPrins
    '
    Me.DsPrins.DataSetName = "dsPrins"
    Me.DsPrins.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
    '
    'DsPrinsBindingSource
    '
    Me.DsPrinsBindingSource.DataSource = Me.DsPrins
    Me.DsPrinsBindingSource.Position = 0
    '
    'ibCancel
    '
    Me.ibCancel.Break = C1.Win.C1InputPanel.BreakType.Group
    Me.ibCancel.Enabled = False
    Me.ibCancel.Name = "ibCancel"
    Me.ibCancel.Text = "&Abbrechen"
    Me.ibCancel.Width = 240
    '
    'ucRestruct2Prins
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.flex)
    Me.Controls.Add(Me.ipTop)
    Me.Name = "ucRestruct2Prins"
    Me.Size = New System.Drawing.Size(845, 574)
    CType(Me.ipTop, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.flex, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DsRestruct, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DsRestructBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DsPrins, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DsPrinsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

  Friend WithEvents ipTop As C1.Win.C1InputPanel.C1InputPanel
  Friend WithEvents InputGroupHeader1 As C1.Win.C1InputPanel.InputGroupHeader
  Friend WithEvents flex As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents DsPrinsBindingSource As BindingSource
  Friend WithEvents DsPrins As dsPrins
  Friend WithEvents DsRestruct As dsRestruct
  Friend WithEvents DsRestructBindingSource As BindingSource
  Friend WithEvents taVProjekt_Kunde As dsRestructTableAdapters.vProjekt_KundeTableAdapter
  Friend WithEvents taVWochenbericht As dsRestructTableAdapters.vWochenberichtTableAdapter
  Friend WithEvents ibUpdateProjekte As C1.Win.C1InputPanel.InputButton
  Friend WithEvents ibWBImport As C1.Win.C1InputPanel.InputButton
  Friend WithEvents InputSeparator1 As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents c1Progress As C1.Win.C1InputPanel.InputProgressBar
  Friend WithEvents ibCancel As C1.Win.C1InputPanel.InputButton
End Class
