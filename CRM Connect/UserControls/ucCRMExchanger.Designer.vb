<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucCRMExchanger
  Inherits System.Windows.Forms.UserControl

  'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
  <System.Diagnostics.DebuggerNonUserCode()>
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
  '<System.Diagnostics.DebuggerStepThrough()>
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucCRMExchanger))
    Me.inpTop = New C1.Win.C1InputPanel.C1InputPanel()
    Me.InputGroupHeader1 = New C1.Win.C1InputPanel.InputGroupHeader()
    Me.InputSeparator1 = New C1.Win.C1InputPanel.InputSeparator()
    Me.ibStartTransfer = New C1.Win.C1InputPanel.InputButton()
    Me.ibAutoStart = New C1.Win.C1InputPanel.InputButton()
    Me.ibADRead = New C1.Win.C1InputPanel.InputButton()
    Me.ibAzureBusQ = New C1.Win.C1InputPanel.InputButton()
    Me.ibPrjFolder = New C1.Win.C1InputPanel.InputButton()
    Me.InputLabel2 = New C1.Win.C1InputPanel.InputLabel()
    Me.xbSaveCSV = New C1.Win.C1InputPanel.InputCheckBox()
    Me.InputSeparator2 = New C1.Win.C1InputPanel.InputSeparator()
    Me.InputGroupHeader2 = New C1.Win.C1InputPanel.InputGroupHeader()
    Me.iProgress1 = New C1.Win.C1InputPanel.InputProgressBar()
    Me.InputButton1 = New C1.Win.C1InputPanel.InputButton()
    Me.InputSeparator3 = New C1.Win.C1InputPanel.InputSeparator()
    Me.tmAutoExec = New System.Windows.Forms.Timer(Me.components)
    Me.bwInvoiceReader = New System.ComponentModel.BackgroundWorker()
    Me.dtMain = New C1.Win.C1Command.C1DockingTab()
    Me.cdpTasks = New C1.Win.C1Command.C1DockingTabPage()
    Me.flex = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.bsTransfer = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsTransfer = New CRM_Connect.dsTransfer()
        Me.cdpMessages = New C1.Win.C1Command.C1DockingTabPage()
        Me.fgMessages = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.bsTblLog = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsLog = New CRM_Connect.dsLog()
        Me.cdpMailsettings = New C1.Win.C1Command.C1DockingTabPage()
        Me.cipVorlage = New C1.Win.C1InputPanel.C1InputPanel()
        Me.InputGroupHeader3 = New C1.Win.C1InputPanel.InputGroupHeader()
        Me.InputLabel1 = New C1.Win.C1InputPanel.InputLabel()
        Me.cbEntwurf = New C1.Win.C1InputPanel.InputComboBox()
        Me.UcOutlookMail = New hpcOutlook.ucOutlookMail()
        Me.bw_PrjFolder = New System.ComponentModel.BackgroundWorker()
        Me.bw_ESTGroups = New System.ComponentModel.BackgroundWorker()
        Me.C1ThemeController1 = New C1.Win.C1Themes.C1ThemeController()
        Me.taKhkBuchungByOp = New CRM_Connect.dsHPCReweSKR04TableAdapters.KHKBuchungByOpTableAdapter()
        Me.DsHPCReweSKR04 = New CRM_Connect.dsHPCReweSKR04()
        Me.DsPrins3Central = New CRM_Connect.dsPrins3Central()
        Me.taVW_USER_ACCOUNTS = New CRM_Connect.dsPrins3CentralTableAdapters.VW_USER_ACCOUNTSTableAdapter()
        Me.dsCRMStageing1 = New CRM_Connect.dsCRMStageing()
        Me.taVW_TBL_CRM_Firma_IMP = New CRM_Connect.dsCRMStageingTableAdapters.VW_TBL_CRM_Firma_IMPTableAdapter()
        Me.taDebitor = New CRM_Connect.dsCRMStageingTableAdapters.DebitorTableAdapter()
        Me.taProjekt = New CRM_Connect.dsCRMStageingTableAdapters.ProjektTableAdapter()
        Me.taPeriodenabrechnung = New CRM_Connect.dsCRMStageingTableAdapters.PeriodenabrechnungTableAdapter()
        Me.taHPC_Mitarbeiter = New CRM_Connect.dsCRMStageingTableAdapters.HPC_MitarbeiterTableAdapter()
        Me.taAnwesenheit = New CRM_Connect.dsCRMStageingTableAdapters.AnwesenheitTableAdapter()
        Me.taAbwesenheit = New CRM_Connect.dsCRMStageingTableAdapters.AbwesenheitTableAdapter()
        Me.taKontakte = New CRM_Connect.dsCRMStageingTableAdapters.KontakteTableAdapter()
        Me.taVW_TBL_CRM_Firma_Debitor_LNK_IMP = New CRM_Connect.dsCRMStageingTableAdapters.VW_TBL_CRM_Firma_Debitor_LNK_IMPTableAdapter()
        Me.taMitarbeiter = New CRM_Connect.dsCRMStageingTableAdapters.MitarbeiterTableAdapter()
        Me.DsPrins1 = New CRM_Connect.dsPrins()
        Me.taTBL_CRM_FIRMA_IMP = New CRM_Connect.dsPrinsTableAdapters.TBL_CRM_FIRMA_IMPTableAdapter()
        Me.taVW_CRM_DEBITOR_EXP = New CRM_Connect.dsPrinsTableAdapters.VW_CRM_DEBITOR_EXPTableAdapter()
        Me.taVW_CRM_PROJEKT_EXP = New CRM_Connect.dsPrinsTableAdapters.VW_CRM_PROJEKT_EXPTableAdapter()
        Me.taVW_CRM_PERIODENABR_EXP = New CRM_Connect.dsPrinsTableAdapters.VW_CRM_PERIODENABR_EXPTableAdapter()
        Me.taVW_ANWESENHEIT = New CRM_Connect.dsPrinsTableAdapters.VW_ANWESENHEITTableAdapter()
        Me.taWochenbericht = New CRM_Connect.dsPrinsTableAdapters.WOCHENBERICHTTableAdapter()
        Me.taPersonal = New CRM_Connect.dsPrinsTableAdapters.PERSONALTableAdapter()
        Me.taABWESENHEITSART = New CRM_Connect.dsPrinsTableAdapters.TBL_ABWESENHEITSARTTableAdapter()
        Me.taTBL_CRM_FIRMA_DEBITOR_LNK_IMP = New CRM_Connect.dsPrinsTableAdapters.TBL_CRM_FIRMA_DEBITOR_LNK_IMPTableAdapter()
        Me.taTBL_KHK_OPBUCHUNGEN = New CRM_Connect.dsPrinsTableAdapters.TBL_KHK_OPBUCHUNGENTableAdapter()
        Me.DsSQLDataLake = New CRM_Connect.dsSQLDataLake()
        Me.DsELOArchiv1 = New CRM_Connect.dsELOArchiv1()
        Me.taHpc_projects_costcenter = New CRM_Connect.dsELOArchiv1TableAdapters.hpc_projects_costcenterTableAdapter()
        Me.bw_SetFOBuchung = New System.ComponentModel.BackgroundWorker()
        CType(Me.inpTop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.dtMain.SuspendLayout()
        Me.cdpTasks.SuspendLayout()
        CType(Me.flex, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsTransfer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsTransfer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cdpMessages.SuspendLayout()
        CType(Me.fgMessages, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsTblLog, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cdpMailsettings.SuspendLayout()
        CType(Me.cipVorlage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C1ThemeController1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsHPCReweSKR04, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsPrins3Central, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsCRMStageing1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsPrins1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsSQLDataLake, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsELOArchiv1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'inpTop
        '
        Me.inpTop.AutoSizeElement = C1.Framework.AutoSizeElement.Both
        Me.inpTop.DesignScaleFactor = 1.454708!
        Me.inpTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpTop.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.inpTop.Items.Add(Me.InputGroupHeader1)
        Me.inpTop.Items.Add(Me.InputSeparator1)
        Me.inpTop.Items.Add(Me.ibStartTransfer)
        Me.inpTop.Items.Add(Me.ibAutoStart)
        Me.inpTop.Items.Add(Me.ibADRead)
        Me.inpTop.Items.Add(Me.ibAzureBusQ)
        Me.inpTop.Items.Add(Me.ibPrjFolder)
        Me.inpTop.Items.Add(Me.InputLabel2)
        Me.inpTop.Items.Add(Me.xbSaveCSV)
        Me.inpTop.Items.Add(Me.InputSeparator2)
        Me.inpTop.Items.Add(Me.InputGroupHeader2)
        Me.inpTop.Items.Add(Me.iProgress1)
        Me.inpTop.Location = New System.Drawing.Point(0, 0)
        Me.inpTop.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.inpTop.Name = "inpTop"
        Me.inpTop.Size = New System.Drawing.Size(1788, 211)
        Me.inpTop.TabIndex = 0
        Me.C1ThemeController1.SetTheme(Me.inpTop, "(default)")
        Me.inpTop.VisualStyle = C1.Win.C1InputPanel.VisualStyle.Custom
        '
        'InputGroupHeader1
        '
        Me.InputGroupHeader1.Name = "InputGroupHeader1"
        '
        'InputSeparator1
        '
        Me.InputSeparator1.Break = C1.Win.C1InputPanel.BreakType.Group
        Me.InputSeparator1.ElementHeight = 0
        Me.InputSeparator1.Name = "InputSeparator1"
        '
        'ibStartTransfer
        '
        Me.ibStartTransfer.Image = CType(resources.GetObject("ibStartTransfer.Image"), System.Drawing.Image)
        Me.ibStartTransfer.ImageSize = New System.Drawing.Size(0, 0)
        Me.ibStartTransfer.Name = "ibStartTransfer"
        Me.ibStartTransfer.Text = "Transfer starten"
        Me.ibStartTransfer.Width = 250
        '
        'ibAutoStart
        '
        Me.ibAutoStart.Break = C1.Win.C1InputPanel.BreakType.Column
        Me.ibAutoStart.CheckOnClick = True
        Me.ibAutoStart.Image = CType(resources.GetObject("ibAutoStart.Image"), System.Drawing.Image)
        Me.ibAutoStart.ImageSize = New System.Drawing.Size(0, 0)
        Me.ibAutoStart.Name = "ibAutoStart"
        Me.ibAutoStart.Text = "Automatisch starten"
        Me.ibAutoStart.Width = 250
        '
        'ibADRead
        '
        Me.ibADRead.Image = CType(resources.GetObject("ibADRead.Image"), System.Drawing.Image)
        Me.ibADRead.ImageSize = New System.Drawing.Size(0, 0)
        Me.ibADRead.Name = "ibADRead"
        Me.ibADRead.Text = "AD lesen"
        Me.ibADRead.Width = 250
        '
        'ibAzureBusQ
        '
        Me.ibAzureBusQ.Name = "ibAzureBusQ"
        Me.ibAzureBusQ.Text = "Azure Bus Überwachung"
        Me.ibAzureBusQ.Width = 250
        '
        'ibPrjFolder
        '
        Me.ibPrjFolder.Break = C1.Win.C1InputPanel.BreakType.Column
        Me.ibPrjFolder.Name = "ibPrjFolder"
        Me.ibPrjFolder.Text = "Projektordner Anlage starten"
        Me.ibPrjFolder.Width = 250
        '
        'InputLabel2
        '
        Me.InputLabel2.Name = "InputLabel2"
        Me.InputLabel2.Text = "Pascom Export in CSV speichern"
        Me.InputLabel2.ToolTipText = "Speichert den Export in einer CSV Datei im Verzeichnis %LocalUserAppDataPath% - c" &
    ":\Users\%USERNAME%\AppData\Local\CRM_Connect\CRM Connect\"
        Me.InputLabel2.Width = 200
        '
        'xbSaveCSV
        '
        Me.xbSaveCSV.Name = "xbSaveCSV"
        '
        'InputSeparator2
        '
        Me.InputSeparator2.Name = "InputSeparator2"
        '
        'InputGroupHeader2
        '
        Me.InputGroupHeader2.Name = "InputGroupHeader2"
        '
        'iProgress1
        '
        Me.iProgress1.Name = "iProgress1"
        Me.iProgress1.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
        Me.iProgress1.Width = 956
        '
        'InputButton1
        '
        Me.InputButton1.Image = CType(resources.GetObject("InputButton1.Image"), System.Drawing.Image)
        Me.InputButton1.ImageSize = New System.Drawing.Size(0, 0)
        Me.InputButton1.Name = "InputButton1"
        Me.InputButton1.Text = "Transfer starten"
        Me.InputButton1.Width = 256
        '
        'InputSeparator3
        '
        Me.InputSeparator3.Name = "InputSeparator3"
        '
        'tmAutoExec
        '
        Me.tmAutoExec.Interval = 900000
        '
        'bwInvoiceReader
        '
        '
        'dtMain
        '
        Me.dtMain.BackColor = System.Drawing.Color.FromArgb(CType(CType(238, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.dtMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dtMain.Controls.Add(Me.cdpTasks)
        Me.dtMain.Controls.Add(Me.cdpMessages)
        Me.dtMain.Controls.Add(Me.cdpMailsettings)
        Me.dtMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtMain.HotTrack = True
        Me.dtMain.Location = New System.Drawing.Point(0, 211)
        Me.dtMain.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtMain.Name = "dtMain"
        Me.dtMain.SelectedIndex = 2
        Me.dtMain.Size = New System.Drawing.Size(1788, 944)
        Me.dtMain.TabIndex = 2
        Me.dtMain.TabSizeMode = C1.Win.C1Command.TabSizeModeEnum.Fit
        Me.dtMain.TabsShowFocusCues = False
        Me.dtMain.TabsSpacing = 2
        Me.dtMain.TabStyle = C1.Win.C1Command.TabStyleEnum.Office2007
        Me.C1ThemeController1.SetTheme(Me.dtMain, "(default)")
        '
        'cdpTasks
        '
        Me.cdpTasks.Controls.Add(Me.flex)
        Me.cdpTasks.Location = New System.Drawing.Point(1, 32)
        Me.cdpTasks.Name = "cdpTasks"
        Me.cdpTasks.Size = New System.Drawing.Size(1786, 911)
        Me.cdpTasks.TabIndex = 0
        Me.cdpTasks.Text = "Tasks"
        '
        'flex
        '
        Me.flex.AllowAddNew = True
        Me.flex.AllowDelete = True
        Me.flex.BackColor = System.Drawing.Color.White
        Me.flex.ColumnInfo = resources.GetString("flex.ColumnInfo")
        Me.flex.DataSource = Me.bsTransfer
        Me.flex.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flex.ExtendLastCol = True
        Me.flex.Font = New System.Drawing.Font("Segoe UI", 11.25!)
        Me.flex.ForeColor = System.Drawing.Color.Black
        Me.flex.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.flex.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.flex.Location = New System.Drawing.Point(0, 0)
        Me.flex.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.flex.Name = "flex"
        Me.flex.Rows.Count = 1
        Me.flex.Rows.DefaultSize = 19
        Me.flex.ShowErrors = True
        Me.flex.ShowThemedHeaders = C1.Win.C1FlexGrid.ShowThemedHeadersEnum.None
        Me.flex.Size = New System.Drawing.Size(1786, 911)
        Me.flex.StyleInfo = resources.GetString("flex.StyleInfo")
        Me.flex.TabIndex = 2
        Me.C1ThemeController1.SetTheme(Me.flex, "(default)")
        Me.flex.Tree.LineColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer))
        '
        'bsTransfer
        '
        Me.bsTransfer.DataMember = "dtTransfer"
        Me.bsTransfer.DataSource = Me.DsTransfer
        '
        'DsTransfer
        '
        Me.DsTransfer.DataSetName = "dsTransfer"
        Me.DsTransfer.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'cdpMessages
        '
        Me.cdpMessages.Controls.Add(Me.fgMessages)
        Me.cdpMessages.Location = New System.Drawing.Point(1, 32)
        Me.cdpMessages.Name = "cdpMessages"
        Me.cdpMessages.Size = New System.Drawing.Size(1786, 911)
        Me.cdpMessages.TabIndex = 2
        Me.cdpMessages.Text = "Log"
        '
        'fgMessages
        '
        Me.fgMessages.AllowEditing = False
        Me.fgMessages.AutoGenerateColumns = False
        Me.fgMessages.BackColor = System.Drawing.Color.White
        Me.fgMessages.ColumnInfo = resources.GetString("fgMessages.ColumnInfo")
        Me.fgMessages.DataSource = Me.bsTblLog
        Me.fgMessages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.fgMessages.ExtendLastCol = True
        Me.fgMessages.Font = New System.Drawing.Font("Segoe UI", 11.25!)
        Me.fgMessages.ForeColor = System.Drawing.Color.Black
        Me.fgMessages.Location = New System.Drawing.Point(0, 0)
        Me.fgMessages.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.fgMessages.Name = "fgMessages"
        Me.fgMessages.Rows.Count = 1
        Me.fgMessages.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.fgMessages.ShowThemedHeaders = C1.Win.C1FlexGrid.ShowThemedHeadersEnum.None
        Me.fgMessages.Size = New System.Drawing.Size(1786, 911)
        Me.fgMessages.StyleInfo = resources.GetString("fgMessages.StyleInfo")
        Me.fgMessages.TabIndex = 0
        Me.C1ThemeController1.SetTheme(Me.fgMessages, "(default)")
        Me.fgMessages.Tree.LineColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer))
        '
        'bsTblLog
        '
        Me.bsTblLog.DataMember = "TBL_LOG"
        Me.bsTblLog.DataSource = Me.DsLog
        Me.bsTblLog.Sort = "ID DESC"
        '
        'DsLog
        '
        Me.DsLog.DataSetName = "dsLog"
        Me.DsLog.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'cdpMailsettings
        '
        Me.cdpMailsettings.Controls.Add(Me.cipVorlage)
        Me.cdpMailsettings.Controls.Add(Me.UcOutlookMail)
        Me.cdpMailsettings.Location = New System.Drawing.Point(1, 32)
        Me.cdpMailsettings.Name = "cdpMailsettings"
        Me.cdpMailsettings.Size = New System.Drawing.Size(1786, 911)
        Me.cdpMailsettings.TabIndex = 1
        Me.cdpMailsettings.Text = "Mail Einstellungen"
        '
        'cipVorlage
        '
        Me.cipVorlage.AutoSizeElement = C1.Framework.AutoSizeElement.Both
        Me.cipVorlage.Dock = System.Windows.Forms.DockStyle.Top
        Me.cipVorlage.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cipVorlage.Items.Add(Me.InputGroupHeader3)
        Me.cipVorlage.Items.Add(Me.InputLabel1)
        Me.cipVorlage.Items.Add(Me.cbEntwurf)
        Me.cipVorlage.Location = New System.Drawing.Point(0, 161)
        Me.cipVorlage.Name = "cipVorlage"
        Me.cipVorlage.Size = New System.Drawing.Size(1786, 150)
        Me.cipVorlage.TabIndex = 1
        Me.C1ThemeController1.SetTheme(Me.cipVorlage, "(default)")
        Me.cipVorlage.VisualStyle = C1.Win.C1InputPanel.VisualStyle.Custom
        '
        'InputGroupHeader3
        '
        Me.InputGroupHeader3.Name = "InputGroupHeader3"
        Me.InputGroupHeader3.Text = "Vorlage"
        '
        'InputLabel1
        '
        Me.InputLabel1.Name = "InputLabel1"
        Me.InputLabel1.Text = "AD User vorhanden"
        Me.InputLabel1.Width = 150
        '
        'cbEntwurf
        '
        Me.cbEntwurf.DropDownStyle = C1.Win.C1InputPanel.InputComboBoxStyle.DropDownList
        Me.cbEntwurf.Name = "cbEntwurf"
        Me.cbEntwurf.Width = 430
        '
        'UcOutlookMail
        '
        Me.UcOutlookMail.BackColor = System.Drawing.Color.FromArgb(CType(CType(238, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.UcOutlookMail.CurrentFolderName = ""
        Me.UcOutlookMail.CurrentMailboxName = ""
        Me.UcOutlookMail.CurrentSessionName = ""
        Me.UcOutlookMail.Dock = System.Windows.Forms.DockStyle.Top
        Me.UcOutlookMail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer))
        Me.UcOutlookMail.Location = New System.Drawing.Point(0, 0)
        Me.UcOutlookMail.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.UcOutlookMail.Name = "UcOutlookMail"
        Me.UcOutlookMail.Size = New System.Drawing.Size(1786, 161)
        Me.UcOutlookMail.TabIndex = 2
        Me.C1ThemeController1.SetTheme(Me.UcOutlookMail, "(default)")
        '
        'bw_PrjFolder
        '
        Me.bw_PrjFolder.WorkerSupportsCancellation = True
        '
        'bw_ESTGroups
        '
        Me.bw_ESTGroups.WorkerSupportsCancellation = True
        '
        'taKhkBuchungByOp
        '
        Me.taKhkBuchungByOp.ClearBeforeFill = True
        '
        'DsHPCReweSKR04
        '
        Me.DsHPCReweSKR04.DataSetName = "dsHPCReweSKR04"
        Me.DsHPCReweSKR04.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'DsPrins3Central
        '
        Me.DsPrins3Central.DataSetName = "dsPrins3Central"
        Me.DsPrins3Central.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'taVW_USER_ACCOUNTS
        '
        Me.taVW_USER_ACCOUNTS.ClearBeforeFill = True
        '
        'dsCRMStageing1
        '
        Me.dsCRMStageing1.DataSetName = "dsCRMStageing"
        Me.dsCRMStageing1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'taVW_TBL_CRM_Firma_IMP
        '
        Me.taVW_TBL_CRM_Firma_IMP.ClearBeforeFill = True
        '
        'taDebitor
        '
        Me.taDebitor.ClearBeforeFill = True
        '
        'taProjekt
        '
        Me.taProjekt.ClearBeforeFill = True
        '
        'taPeriodenabrechnung
        '
        Me.taPeriodenabrechnung.ClearBeforeFill = True
        '
        'taHPC_Mitarbeiter
        '
        Me.taHPC_Mitarbeiter.ClearBeforeFill = True
        '
        'taAnwesenheit
        '
        Me.taAnwesenheit.ClearBeforeFill = True
        '
        'taAbwesenheit
        '
        Me.taAbwesenheit.ClearBeforeFill = True
        '
        'taKontakte
        '
        Me.taKontakte.ClearBeforeFill = True
        '
        'taVW_TBL_CRM_Firma_Debitor_LNK_IMP
        '
        Me.taVW_TBL_CRM_Firma_Debitor_LNK_IMP.ClearBeforeFill = True
        '
        'taMitarbeiter
        '
        Me.taMitarbeiter.ClearBeforeFill = True
        '
        'DsPrins1
        '
        Me.DsPrins1.DataSetName = "dsPrins"
        Me.DsPrins1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'taTBL_CRM_FIRMA_IMP
        '
        Me.taTBL_CRM_FIRMA_IMP.ClearBeforeFill = True
        '
        'taVW_CRM_DEBITOR_EXP
        '
        Me.taVW_CRM_DEBITOR_EXP.ClearBeforeFill = True
        '
        'taVW_CRM_PROJEKT_EXP
        '
        Me.taVW_CRM_PROJEKT_EXP.ClearBeforeFill = True
        '
        'taVW_CRM_PERIODENABR_EXP
        '
        Me.taVW_CRM_PERIODENABR_EXP.ClearBeforeFill = True
        '
        'taVW_ANWESENHEIT
        '
        Me.taVW_ANWESENHEIT.ClearBeforeFill = True
        '
        'taWochenbericht
        '
        Me.taWochenbericht.ClearBeforeFill = True
        '
        'taPersonal
        '
        Me.taPersonal.ClearBeforeFill = True
        '
        'taABWESENHEITSART
        '
        Me.taABWESENHEITSART.ClearBeforeFill = True
        '
        'taTBL_CRM_FIRMA_DEBITOR_LNK_IMP
        '
        Me.taTBL_CRM_FIRMA_DEBITOR_LNK_IMP.ClearBeforeFill = True
        '
        'taTBL_KHK_OPBUCHUNGEN
        '
        Me.taTBL_KHK_OPBUCHUNGEN.ClearBeforeFill = True
        '
        'DsSQLDataLake
        '
        Me.DsSQLDataLake.DataSetName = "dsSQLDataLake"
        Me.DsSQLDataLake.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'DsELOArchiv1
        '
        Me.DsELOArchiv1.DataSetName = "dsELOArchiv1"
        Me.DsELOArchiv1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'taHpc_projects_costcenter
        '
        Me.taHpc_projects_costcenter.ClearBeforeFill = True
        '
        'bw_SetFOBuchung
        '
        Me.bw_SetFOBuchung.WorkerSupportsCancellation = True
        '
        'ucCRMExchanger
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dtMain)
        Me.Controls.Add(Me.inpTop)
        Me.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "ucCRMExchanger"
        Me.Size = New System.Drawing.Size(1788, 1155)
        Me.C1ThemeController1.SetTheme(Me, "(default)")
        CType(Me.inpTop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.dtMain.ResumeLayout(False)
        Me.cdpTasks.ResumeLayout(False)
        CType(Me.flex, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsTransfer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsTransfer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cdpMessages.ResumeLayout(False)
        CType(Me.fgMessages, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsTblLog, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cdpMailsettings.ResumeLayout(False)
        CType(Me.cipVorlage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C1ThemeController1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsHPCReweSKR04, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsPrins3Central, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsCRMStageing1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsPrins1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsSQLDataLake, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsELOArchiv1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents inpTop As C1.Win.C1InputPanel.C1InputPanel
  Friend WithEvents InputGroupHeader1 As C1.Win.C1InputPanel.InputGroupHeader
  Friend WithEvents bsTransfer As System.Windows.Forms.BindingSource
  Friend WithEvents InputSeparator1 As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents ibStartTransfer As C1.Win.C1InputPanel.InputButton
  Friend WithEvents DsTransfer As CRM_Connect.dsTransfer
  Friend WithEvents dsCRMStageing1 As CRM_Connect.dsCRMStageing
  Friend WithEvents DsPrins1 As CRM_Connect.dsPrins
  Friend WithEvents taVW_TBL_CRM_Firma_IMP As CRM_Connect.dsCRMStageingTableAdapters.VW_TBL_CRM_Firma_IMPTableAdapter
  Friend WithEvents taTBL_CRM_FIRMA_IMP As CRM_Connect.dsPrinsTableAdapters.TBL_CRM_FIRMA_IMPTableAdapter
  Friend WithEvents taDebitor As CRM_Connect.dsCRMStageingTableAdapters.DebitorTableAdapter
  Friend WithEvents taVW_CRM_DEBITOR_EXP As CRM_Connect.dsPrinsTableAdapters.VW_CRM_DEBITOR_EXPTableAdapter
  Friend WithEvents taVW_CRM_PROJEKT_EXP As CRM_Connect.dsPrinsTableAdapters.VW_CRM_PROJEKT_EXPTableAdapter
  Friend WithEvents taVW_CRM_PERIODENABR_EXP As CRM_Connect.dsPrinsTableAdapters.VW_CRM_PERIODENABR_EXPTableAdapter
  Friend WithEvents taProjekt As CRM_Connect.dsCRMStageingTableAdapters.ProjektTableAdapter
  Friend WithEvents ibAutoStart As C1.Win.C1InputPanel.InputButton
  Friend WithEvents InputGroupHeader2 As C1.Win.C1InputPanel.InputGroupHeader
  Friend WithEvents iProgress1 As C1.Win.C1InputPanel.InputProgressBar
  Friend WithEvents InputButton1 As C1.Win.C1InputPanel.InputButton
  Friend WithEvents InputSeparator3 As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents taPeriodenabrechnung As CRM_Connect.dsCRMStageingTableAdapters.PeriodenabrechnungTableAdapter
  Friend WithEvents tmAutoExec As System.Windows.Forms.Timer
  Friend WithEvents ibADRead As C1.Win.C1InputPanel.InputButton
  Friend WithEvents DsPrins3Central As dsPrins3Central
  Friend WithEvents taHPC_Mitarbeiter As dsCRMStageingTableAdapters.HPC_MitarbeiterTableAdapter
  Friend WithEvents taVW_USER_ACCOUNTS As dsPrins3CentralTableAdapters.VW_USER_ACCOUNTSTableAdapter
  Friend WithEvents taVW_ANWESENHEIT As dsPrinsTableAdapters.VW_ANWESENHEITTableAdapter
  Friend WithEvents taAnwesenheit As dsCRMStageingTableAdapters.AnwesenheitTableAdapter
  Friend WithEvents taAbwesenheit As dsCRMStageingTableAdapters.AbwesenheitTableAdapter
  Friend WithEvents taWochenbericht As dsPrinsTableAdapters.WOCHENBERICHTTableAdapter
  Friend WithEvents taPersonal As dsPrinsTableAdapters.PERSONALTableAdapter
  Friend WithEvents taABWESENHEITSART As dsPrinsTableAdapters.TBL_ABWESENHEITSARTTableAdapter
  Friend WithEvents taKontakte As dsCRMStageingTableAdapters.KontakteTableAdapter
  Friend WithEvents InputLabel2 As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents xbSaveCSV As C1.Win.C1InputPanel.InputCheckBox
  Friend WithEvents InputSeparator2 As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents taTBL_CRM_FIRMA_DEBITOR_LNK_IMP As dsPrinsTableAdapters.TBL_CRM_FIRMA_DEBITOR_LNK_IMPTableAdapter
  Friend WithEvents taVW_TBL_CRM_Firma_Debitor_LNK_IMP As dsCRMStageingTableAdapters.VW_TBL_CRM_Firma_Debitor_LNK_IMPTableAdapter
  Friend WithEvents taKhkBuchungByOp As dsHPCReweSKR04TableAdapters.KHKBuchungByOpTableAdapter
  Friend WithEvents DsHPCReweSKR04 As dsHPCReweSKR04
  Friend WithEvents taTBL_KHK_OPBUCHUNGEN As dsPrinsTableAdapters.TBL_KHK_OPBUCHUNGENTableAdapter
  Friend WithEvents taMitarbeiter As dsCRMStageingTableAdapters.MitarbeiterTableAdapter
  Friend WithEvents ibAzureBusQ As C1.Win.C1InputPanel.InputButton
  Friend WithEvents bwInvoiceReader As System.ComponentModel.BackgroundWorker
  Friend WithEvents dtMain As C1.Win.C1Command.C1DockingTab
  Friend WithEvents cdpTasks As C1.Win.C1Command.C1DockingTabPage
  Friend WithEvents flex As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents cdpMailsettings As C1.Win.C1Command.C1DockingTabPage
  Friend WithEvents bw_PrjFolder As System.ComponentModel.BackgroundWorker
  Friend WithEvents ibPrjFolder As C1.Win.C1InputPanel.InputButton
  Friend WithEvents bw_ESTGroups As System.ComponentModel.BackgroundWorker
  Friend WithEvents cdpMessages As C1.Win.C1Command.C1DockingTabPage
  Friend WithEvents fgMessages As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents C1ThemeController1 As C1.Win.C1Themes.C1ThemeController
  Friend WithEvents bsTblLog As BindingSource
  Friend WithEvents DsLog As dsLog
  Friend WithEvents cipVorlage As C1.Win.C1InputPanel.C1InputPanel
  Friend WithEvents InputGroupHeader3 As C1.Win.C1InputPanel.InputGroupHeader
  Friend WithEvents InputLabel1 As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents cbEntwurf As C1.Win.C1InputPanel.InputComboBox
  Friend WithEvents DsSQLDataLake As dsSQLDataLake
    Friend WithEvents UcOutlookMail As hpcOutlook.ucOutlookMail
    Friend WithEvents DsELOArchiv1 As dsELOArchiv1
    Friend WithEvents taHpc_projects_costcenter As dsELOArchiv1TableAdapters.hpc_projects_costcenterTableAdapter
    Friend WithEvents bw_SetFOBuchung As System.ComponentModel.BackgroundWorker
End Class
