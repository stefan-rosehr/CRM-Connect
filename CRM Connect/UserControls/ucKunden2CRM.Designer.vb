<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucKunden2CRM
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucKunden2CRM))
    Me.inpTop = New C1.Win.C1InputPanel.C1InputPanel()
    Me.bsFirma = New System.Windows.Forms.BindingSource(Me.components)
    Me.DsSQL_CRM_Prins = New CRM_Connect.dsSQL_CRM_Prins()
    Me.InputGroupHeader1 = New C1.Win.C1InputPanel.InputGroupHeader()
    Me.InputSeparator1 = New C1.Win.C1InputPanel.InputSeparator()
    Me.btnKunden2CRM = New C1.Win.C1InputPanel.InputButton()
    Me.btnAdressen2CRM = New C1.Win.C1InputPanel.InputButton()
    Me.btnPrinsProjekteCRM = New C1.Win.C1InputPanel.InputButton()
    Me.btnCRM2Prins = New C1.Win.C1InputPanel.InputButton()
    Me.btnCRMProjekte2Prins = New C1.Win.C1InputPanel.InputButton()
    Me.InputSeparator2 = New C1.Win.C1InputPanel.InputSeparator()
    Me.InputLabel1 = New C1.Win.C1InputPanel.InputLabel()
    Me.tbPrinsKND = New C1.Win.C1InputPanel.InputTextBox()
    Me.InputLabel2 = New C1.Win.C1InputPanel.InputLabel()
    Me.tbFirma = New C1.Win.C1InputPanel.InputTextBox()
    Me.InputSeparator4 = New C1.Win.C1InputPanel.InputSeparator()
    Me.btnSourceView = New C1.Win.C1InputPanel.InputButton()
    Me.btnTagetView = New C1.Win.C1InputPanel.InputButton()
    Me.InputSeparator3 = New C1.Win.C1InputPanel.InputSeparator()
    Me.btnExecAll = New C1.Win.C1InputPanel.InputButton()
    Me.btnAutoExec = New C1.Win.C1InputPanel.InputButton()
    Me.InputSeparator5 = New C1.Win.C1InputPanel.InputSeparator()
    Me.btnExcelLoad = New C1.Win.C1InputPanel.InputButton()
    Me.btnExcelCheck = New C1.Win.C1InputPanel.InputButton()
    Me.btnExcel2Firma = New C1.Win.C1InputPanel.InputButton()
        Me.InputSeparator6 = New C1.Win.C1InputPanel.InputSeparator()
        Me.ltlbMsg = New C1.Win.C1InputPanel.InputLabel()
        Me.tbMsg = New C1.Win.C1InputPanel.InputTextBox()
        Me.InputGroupHeader2 = New C1.Win.C1InputPanel.InputGroupHeader()
        Me.pb1 = New C1.Win.C1InputPanel.InputProgressBar()
        Me.ghTabelle = New C1.Win.C1InputPanel.InputGroupHeader()
        Me.pnlFirma = New System.Windows.Forms.Panel()
        Me.fgProjekt = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.bsProjekt = New System.Windows.Forms.BindingSource(Me.components)
        Me.fgFirma = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.inpMid = New C1.Win.C1InputPanel.C1InputPanel()
        Me.ghTarget = New C1.Win.C1InputPanel.InputGroupHeader()
        Me.pnlPrinsDaten = New System.Windows.Forms.Panel()
        Me.fgAdressen = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.bsVW_ADRESSEN_FIRMA_CRM = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsPrinsViews = New CRM_Connect.dsPrinsViews()
        Me.fgProjekte = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.VWPROJEKTCRMBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.fgExcel = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.fgKunden = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.bsVW_KUNDEN_TO_CRM = New System.Windows.Forms.BindingSource(Me.components)
        Me.tmAutoExec = New System.Windows.Forms.Timer(Me.components)
        Me.DsPrinsTables = New CRM_Connect.dsPrinsTables()
        Me.taAdressen = New CRM_Connect.dsPrinsTablesTableAdapters.ADRESSENTableAdapter()
        Me.taKreditor = New CRM_Connect.dsPrinsTablesTableAdapters.KREDITORTableAdapter()
        Me.taKunden = New CRM_Connect.dsPrinsTablesTableAdapters.KUNDENTableAdapter()
        Me.taZUORDNUNG_ADRESSE = New CRM_Connect.dsPrinsTablesTableAdapters.TBL_ZUORDNUNG_ADRESSETableAdapter()
        Me.taLAENDER = New CRM_Connect.dsPrinsTablesTableAdapters.LAENDERTableAdapter()
        Me.taPRC_AN_PRJ_ANLAGE = New CRM_Connect.dsPrinsTablesTableAdapters.PRC_AN_PRJ_ANLAGETableAdapter()
        Me.taPERSONAL = New CRM_Connect.dsPrinsTableAdapters.PERSONALTableAdapter()
        Me.taFirma = New CRM_Connect.dsSQL_CRM_PrinsTableAdapters.FirmaTableAdapter()
        Me.taProjekt = New CRM_Connect.dsSQL_CRM_PrinsTableAdapters.ProjektTableAdapter()
        Me.DsSQLCRMPrinsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.bsVW_PROJEKT_CRM = New System.Windows.Forms.BindingSource(Me.components)
        Me.taVW_KUNDEN_TO_CRM = New CRM_Connect.dsPrinsViewsTableAdapters.VW_KUNDEN_TO_CRMTableAdapter()
        Me.TableAdapterManager = New CRM_Connect.dsPrinsViewsTableAdapters.TableAdapterManager()
        Me.taVW_ADRESSEN_FIRMA_CRM = New CRM_Connect.dsPrinsViewsTableAdapters.VW_ADRESSEN_FIRMA_CRMTableAdapter()
        Me.taVW_PROJEKT_CRM = New CRM_Connect.dsPrinsViewsTableAdapters.VW_PROJEKT_CRMTableAdapter()
        Me.DsPrins1 = New CRM_Connect.dsPrins()
        CType(Me.inpTop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsFirma, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsSQL_CRM_Prins, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFirma.SuspendLayout()
        CType(Me.fgProjekt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsProjekt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fgFirma, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.inpMid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPrinsDaten.SuspendLayout()
        CType(Me.fgAdressen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsVW_ADRESSEN_FIRMA_CRM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsPrinsViews, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fgProjekte, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VWPROJEKTCRMBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fgExcel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fgKunden, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsVW_KUNDEN_TO_CRM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsPrinsTables, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsSQLCRMPrinsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsVW_PROJEKT_CRM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsPrins1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'inpTop
        '
        Me.inpTop.AutoSizeElement = C1.Framework.AutoSizeElement.Both
        Me.inpTop.DataSource = Me.bsFirma
        Me.inpTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpTop.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.inpTop.Items.Add(Me.InputGroupHeader1)
        Me.inpTop.Items.Add(Me.InputSeparator1)
        Me.inpTop.Items.Add(Me.btnKunden2CRM)
        Me.inpTop.Items.Add(Me.btnAdressen2CRM)
        Me.inpTop.Items.Add(Me.btnPrinsProjekteCRM)
        Me.inpTop.Items.Add(Me.btnCRM2Prins)
        Me.inpTop.Items.Add(Me.btnCRMProjekte2Prins)
        Me.inpTop.Items.Add(Me.InputSeparator2)
        Me.inpTop.Items.Add(Me.InputLabel1)
        Me.inpTop.Items.Add(Me.tbPrinsKND)
        Me.inpTop.Items.Add(Me.InputLabel2)
        Me.inpTop.Items.Add(Me.tbFirma)
        Me.inpTop.Items.Add(Me.InputSeparator4)
        Me.inpTop.Items.Add(Me.btnSourceView)
        Me.inpTop.Items.Add(Me.btnTagetView)
        Me.inpTop.Items.Add(Me.InputSeparator3)
        Me.inpTop.Items.Add(Me.btnExecAll)
        Me.inpTop.Items.Add(Me.btnAutoExec)
        Me.inpTop.Items.Add(Me.InputSeparator5)
        Me.inpTop.Items.Add(Me.btnExcelLoad)
        Me.inpTop.Items.Add(Me.btnExcelCheck)
        Me.inpTop.Items.Add(Me.btnExcel2Firma)
        Me.inpTop.Items.Add(Me.InputSeparator6)
        Me.inpTop.Items.Add(Me.ltlbMsg)
        Me.inpTop.Items.Add(Me.tbMsg)
        Me.inpTop.Items.Add(Me.InputGroupHeader2)
        Me.inpTop.Items.Add(Me.pb1)
        Me.inpTop.Items.Add(Me.ghTabelle)
        Me.inpTop.Location = New System.Drawing.Point(0, 0)
        Me.inpTop.Name = "inpTop"
        Me.inpTop.Size = New System.Drawing.Size(1167, 320)
        Me.inpTop.TabIndex = 0
        '
        'bsFirma
        '
        Me.bsFirma.DataMember = "Firma"
        Me.bsFirma.DataSource = Me.DsSQL_CRM_Prins
        '
        'DsSQL_CRM_Prins
        '
        Me.DsSQL_CRM_Prins.DataSetName = "dsSQL_CRM_Prins"
        Me.DsSQL_CRM_Prins.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'InputGroupHeader1
        '
        Me.InputGroupHeader1.Name = "InputGroupHeader1"
        Me.InputGroupHeader1.Text = "Prins-CRM Schnittstelle"
        '
        'InputSeparator1
        '
        Me.InputSeparator1.ElementHeight = 0
        Me.InputSeparator1.Name = "InputSeparator1"
        '
        'btnKunden2CRM
        '
        Me.btnKunden2CRM.Name = "btnKunden2CRM"
        Me.btnKunden2CRM.Text = "Prins KUNDEN -> CRM Übertragung starten"
        Me.btnKunden2CRM.Width = 505
        '
        'btnAdressen2CRM
        '
        Me.btnAdressen2CRM.Enabled = False
        Me.btnAdressen2CRM.Name = "btnAdressen2CRM"
        Me.btnAdressen2CRM.Text = "Prins ADRESSEN -> CRM Übertragung starten"
        Me.btnAdressen2CRM.Width = 505
        '
        'btnPrinsProjekteCRM
        '
        Me.btnPrinsProjekteCRM.Name = "btnPrinsProjekteCRM"
        Me.btnPrinsProjekteCRM.Tag = "1"
        Me.btnPrinsProjekteCRM.Text = "Prins PROJEKT -> CRM Übertragung starten"
        Me.btnPrinsProjekteCRM.Width = 505
        '
        'btnCRM2Prins
        '
        Me.btnCRM2Prins.Name = "btnCRM2Prins"
        Me.btnCRM2Prins.Text = "CRM Firma -> Prins Übertragung starten"
        Me.btnCRM2Prins.Width = 505
        '
        'btnCRMProjekte2Prins
        '
        Me.btnCRMProjekte2Prins.Image = CType(resources.GetObject("btnCRMProjekte2Prins.Image"), System.Drawing.Image)
        Me.btnCRMProjekte2Prins.ImageSize = New System.Drawing.Size(0, 0)
        Me.btnCRMProjekte2Prins.Name = "btnCRMProjekte2Prins"
        Me.btnCRMProjekte2Prins.Text = "CRM Projekte -> Prins Übertragung starten"
        Me.btnCRMProjekte2Prins.Width = 505
        '
        'InputSeparator2
        '
        Me.InputSeparator2.Height = 15
        Me.InputSeparator2.Name = "InputSeparator2"
        Me.InputSeparator2.Width = 505
        '
        'InputLabel1
        '
        Me.InputLabel1.Name = "InputLabel1"
        Me.InputLabel1.Text = "Prins Kunden. Anzahl geändert"
        Me.InputLabel1.Width = 300
        '
        'tbPrinsKND
        '
        Me.tbPrinsKND.Name = "tbPrinsKND"
        Me.tbPrinsKND.ReadOnly = True
        Me.tbPrinsKND.Width = 200
        '
        'InputLabel2
        '
        Me.InputLabel2.Name = "InputLabel2"
        Me.InputLabel2.Text = "CRM Firma - Anzahl Eintragungen"
        Me.InputLabel2.Width = 300
        '
        'tbFirma
        '
        Me.tbFirma.Break = C1.Win.C1InputPanel.BreakType.Column
        Me.tbFirma.Name = "tbFirma"
        Me.tbFirma.ReadOnly = True
        Me.tbFirma.Width = 200
        '
        'InputSeparator4
        '
        Me.InputSeparator4.ElementHeight = 0
        Me.InputSeparator4.Name = "InputSeparator4"
        '
        'btnSourceView
        '
        Me.btnSourceView.Image = CType(resources.GetObject("btnSourceView.Image"), System.Drawing.Image)
        Me.btnSourceView.ImageSize = New System.Drawing.Size(0, 0)
        Me.btnSourceView.Name = "btnSourceView"
        Me.btnSourceView.Text = "Prins-Quelle umschalten"
        Me.btnSourceView.Width = 300
        '
        'btnTagetView
        '
        Me.btnTagetView.Image = CType(resources.GetObject("btnTagetView.Image"), System.Drawing.Image)
        Me.btnTagetView.ImageSize = New System.Drawing.Size(0, 0)
        Me.btnTagetView.Name = "btnTagetView"
        Me.btnTagetView.Text = "CRM-Quelle umschalten"
        Me.btnTagetView.Width = 300
        '
        'InputSeparator3
        '
        Me.InputSeparator3.ElementHeight = 0
        Me.InputSeparator3.Name = "InputSeparator3"
        '
        'btnExecAll
        '
        Me.btnExecAll.Image = CType(resources.GetObject("btnExecAll.Image"), System.Drawing.Image)
        Me.btnExecAll.ImageSize = New System.Drawing.Size(0, 0)
        Me.btnExecAll.Name = "btnExecAll"
        Me.btnExecAll.Text = "Alle prüfen"
        Me.btnExecAll.Width = 300
        '
        'btnAutoExec
        '
        Me.btnAutoExec.CheckOnClick = True
        Me.btnAutoExec.Image = CType(resources.GetObject("btnAutoExec.Image"), System.Drawing.Image)
        Me.btnAutoExec.ImageSize = New System.Drawing.Size(0, 0)
        Me.btnAutoExec.Name = "btnAutoExec"
        Me.btnAutoExec.Text = "Automatisch prüfen"
        Me.btnAutoExec.Width = 300
        '
        'InputSeparator5
        '
        Me.InputSeparator5.ElementHeight = 0
        Me.InputSeparator5.Name = "InputSeparator5"
        '
        'btnExcelLoad
        '
        Me.btnExcelLoad.Image = CType(resources.GetObject("btnExcelLoad.Image"), System.Drawing.Image)
        Me.btnExcelLoad.ImageSize = New System.Drawing.Size(0, 0)
        Me.btnExcelLoad.Name = "btnExcelLoad"
        Me.btnExcelLoad.Text = "Excel Laden"
        Me.btnExcelLoad.Width = 300
        '
        'btnExcelCheck
        '
        Me.btnExcelCheck.Image = CType(resources.GetObject("btnExcelCheck.Image"), System.Drawing.Image)
        Me.btnExcelCheck.ImageSize = New System.Drawing.Size(0, 0)
        Me.btnExcelCheck.Name = "btnExcelCheck"
        Me.btnExcelCheck.Text = "Excel Kontakt prüfen"
        Me.btnExcelCheck.Width = 300
        '
        'btnExcel2Firma
        '
        Me.btnExcel2Firma.Break = C1.Win.C1InputPanel.BreakType.Column
        Me.btnExcel2Firma.Image = CType(resources.GetObject("btnExcel2Firma.Image"), System.Drawing.Image)
        Me.btnExcel2Firma.ImageSize = New System.Drawing.Size(0, 0)
        Me.btnExcel2Firma.Name = "btnExcel2Firma"
        Me.btnExcel2Firma.Text = "Excel in CRM Firma übernehmen"
        Me.btnExcel2Firma.Width = 300
        '
        'InputSeparator6
        '
        Me.InputSeparator6.Height = 15
        Me.InputSeparator6.Name = "InputSeparator6"
        '
        'ltlbMsg
        '
        Me.ltlbMsg.Break = C1.Win.C1InputPanel.BreakType.Row
        Me.ltlbMsg.Name = "ltlbMsg"
        Me.ltlbMsg.Text = "Kurzprotokoll"
        Me.ltlbMsg.Width = 300
        '
        'tbMsg
        '
        Me.tbMsg.Break = C1.Win.C1InputPanel.BreakType.Column
        Me.tbMsg.Height = 165
        Me.tbMsg.Multiline = True
        Me.tbMsg.Name = "tbMsg"
        Me.tbMsg.ReadOnly = True
        Me.tbMsg.VerticalAlign = C1.Win.C1InputPanel.InputContentAlignment.Spread
        Me.tbMsg.Width = 316
        '
        'InputGroupHeader2
        '
        Me.InputGroupHeader2.Name = "InputGroupHeader2"
        '
        'pb1
        '
        Me.pb1.Break = C1.Win.C1InputPanel.BreakType.Column
        Me.pb1.Name = "pb1"
        Me.pb1.Step = 1
        Me.pb1.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
        Me.pb1.Width = 820
        '
        'ghTabelle
        '
        Me.ghTabelle.Name = "ghTabelle"
        Me.ghTabelle.Text = "Prins Daten"
        '
        'pnlFirma
        '
        Me.pnlFirma.Controls.Add(Me.fgProjekt)
        Me.pnlFirma.Controls.Add(Me.fgFirma)
        Me.pnlFirma.Controls.Add(Me.inpMid)
        Me.pnlFirma.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFirma.Location = New System.Drawing.Point(0, 585)
        Me.pnlFirma.Name = "pnlFirma"
        Me.pnlFirma.Size = New System.Drawing.Size(1167, 222)
        Me.pnlFirma.TabIndex = 6
        '
        'fgProjekt
        '
        Me.fgProjekt.ColumnInfo = resources.GetString("fgProjekt.ColumnInfo")
        Me.fgProjekt.DataSource = Me.bsProjekt
        Me.fgProjekt.Location = New System.Drawing.Point(218, 34)
        Me.fgProjekt.Name = "fgProjekt"
        Me.fgProjekt.Rows.Count = 1
        Me.fgProjekt.Rows.DefaultSize = 19
        Me.fgProjekt.Size = New System.Drawing.Size(688, 150)
        Me.fgProjekt.TabIndex = 7
        Me.fgProjekt.Visible = False
        '
        'bsProjekt
        '
        Me.bsProjekt.DataMember = "Projekt"
        Me.bsProjekt.DataSource = Me.DsSQL_CRM_Prins
        '
        'fgFirma
        '
        Me.fgFirma.ColumnInfo = resources.GetString("fgFirma.ColumnInfo")
        Me.fgFirma.DataSource = Me.bsFirma
        Me.fgFirma.Location = New System.Drawing.Point(60, 85)
        Me.fgFirma.Name = "fgFirma"
        Me.fgFirma.Rows.Count = 1
        Me.fgFirma.Rows.DefaultSize = 19
        Me.fgFirma.Size = New System.Drawing.Size(1152, 177)
        Me.fgFirma.TabIndex = 6
        '
        'inpMid
        '
        Me.inpMid.AutoSizeElement = C1.Framework.AutoSizeElement.Both
        Me.inpMid.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpMid.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.inpMid.Items.Add(Me.ghTarget)
        Me.inpMid.Location = New System.Drawing.Point(0, 0)
        Me.inpMid.Name = "inpMid"
        Me.inpMid.Size = New System.Drawing.Size(1167, 28)
        Me.inpMid.TabIndex = 5
        '
        'ghTarget
        '
        Me.ghTarget.Name = "ghTarget"
        Me.ghTarget.Text = "CRM Firma"
        '
        'pnlPrinsDaten
        '
        Me.pnlPrinsDaten.Controls.Add(Me.fgAdressen)
        Me.pnlPrinsDaten.Controls.Add(Me.fgProjekte)
        Me.pnlPrinsDaten.Controls.Add(Me.fgExcel)
        Me.pnlPrinsDaten.Controls.Add(Me.fgKunden)
        Me.pnlPrinsDaten.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlPrinsDaten.Location = New System.Drawing.Point(0, 320)
        Me.pnlPrinsDaten.Name = "pnlPrinsDaten"
        Me.pnlPrinsDaten.Size = New System.Drawing.Size(1167, 265)
        Me.pnlPrinsDaten.TabIndex = 8
        '
        'fgAdressen
        '
        Me.fgAdressen.ColumnInfo = resources.GetString("fgAdressen.ColumnInfo")
        Me.fgAdressen.DataSource = Me.bsVW_ADRESSEN_FIRMA_CRM
        Me.fgAdressen.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.fgAdressen.Location = New System.Drawing.Point(0, 188)
        Me.fgAdressen.Name = "fgAdressen"
        Me.fgAdressen.Rows.Count = 1
        Me.fgAdressen.Rows.DefaultSize = 19
        Me.fgAdressen.Size = New System.Drawing.Size(1167, 77)
        Me.fgAdressen.TabIndex = 8
        '
        'bsVW_ADRESSEN_FIRMA_CRM
        '
        Me.bsVW_ADRESSEN_FIRMA_CRM.DataMember = "VW_ADRESSEN_FIRMA_CRM"
        Me.bsVW_ADRESSEN_FIRMA_CRM.DataSource = Me.DsPrinsViews
        '
        'DsPrinsViews
        '
        Me.DsPrinsViews.DataSetName = "dsPrinsViews"
        Me.DsPrinsViews.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'fgProjekte
        '
        Me.fgProjekte.ColumnInfo = resources.GetString("fgProjekte.ColumnInfo")
        Me.fgProjekte.DataSource = Me.VWPROJEKTCRMBindingSource
        Me.fgProjekte.Dock = System.Windows.Forms.DockStyle.Top
        Me.fgProjekte.Location = New System.Drawing.Point(0, 118)
        Me.fgProjekte.Name = "fgProjekte"
        Me.fgProjekte.Rows.Count = 1
        Me.fgProjekte.Rows.DefaultSize = 19
        Me.fgProjekte.Size = New System.Drawing.Size(1167, 76)
        Me.fgProjekte.TabIndex = 6
        '
        'VWPROJEKTCRMBindingSource
        '
        Me.VWPROJEKTCRMBindingSource.DataMember = "VW_PROJEKT_CRM"
        Me.VWPROJEKTCRMBindingSource.DataSource = Me.DsPrinsViews
        '
        'fgExcel
        '
        Me.fgExcel.ColumnInfo = "10,1,0,0,0,95,Columns:"
        Me.fgExcel.Dock = System.Windows.Forms.DockStyle.Top
        Me.fgExcel.Location = New System.Drawing.Point(0, 43)
        Me.fgExcel.Name = "fgExcel"
        Me.fgExcel.Rows.DefaultSize = 19
        Me.fgExcel.Size = New System.Drawing.Size(1167, 75)
        Me.fgExcel.TabIndex = 4
        Me.fgExcel.Visible = False
        '
        'fgKunden
        '
        Me.fgKunden.ColumnInfo = resources.GetString("fgKunden.ColumnInfo")
        Me.fgKunden.DataSource = Me.bsVW_KUNDEN_TO_CRM
        Me.fgKunden.Dock = System.Windows.Forms.DockStyle.Top
        Me.fgKunden.Location = New System.Drawing.Point(0, 0)
        Me.fgKunden.Name = "fgKunden"
        Me.fgKunden.Rows.Count = 1
        Me.fgKunden.Rows.DefaultSize = 19
        Me.fgKunden.Size = New System.Drawing.Size(1167, 43)
        Me.fgKunden.TabIndex = 2
        '
        'bsVW_KUNDEN_TO_CRM
        '
        Me.bsVW_KUNDEN_TO_CRM.DataMember = "VW_KUNDEN_TO_CRM"
        Me.bsVW_KUNDEN_TO_CRM.DataSource = Me.DsPrinsViews
        '
        'tmAutoExec
        '
        Me.tmAutoExec.Interval = 900000
        '
        'DsPrinsTables
        '
        Me.DsPrinsTables.DataSetName = "dsPrinsTables"
        Me.DsPrinsTables.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'taAdressen
        '
        Me.taAdressen.ClearBeforeFill = True
        '
        'taKreditor
        '
        Me.taKreditor.ClearBeforeFill = True
        '
        'taKunden
        '
        Me.taKunden.ClearBeforeFill = True
        '
        'taZUORDNUNG_ADRESSE
        '
        Me.taZUORDNUNG_ADRESSE.ClearBeforeFill = True
        '
        'taLAENDER
        '
        Me.taLAENDER.ClearBeforeFill = True
        '
        'taPRC_AN_PRJ_ANLAGE
        '
        Me.taPRC_AN_PRJ_ANLAGE.ClearBeforeFill = True
        '
        'taPERSONAL
        '
        Me.taPERSONAL.ClearBeforeFill = True
        '
        'taFirma
        '
        Me.taFirma.ClearBeforeFill = True
        '
        'taProjekt
        '
        Me.taProjekt.ClearBeforeFill = True
        '
        'DsSQLCRMPrinsBindingSource
        '
        Me.DsSQLCRMPrinsBindingSource.DataSource = Me.DsSQL_CRM_Prins
        Me.DsSQLCRMPrinsBindingSource.Position = 0
        '
        'bsVW_PROJEKT_CRM
        '
        Me.bsVW_PROJEKT_CRM.DataMember = "VW_PROJEKT_CRM"
        Me.bsVW_PROJEKT_CRM.DataSource = Me.DsPrinsViews
        '
        'taVW_KUNDEN_TO_CRM
        '
        Me.taVW_KUNDEN_TO_CRM.ClearBeforeFill = True
        '
        'TableAdapterManager
        '
        Me.TableAdapterManager.BackupDataSetBeforeUpdate = False
        Me.TableAdapterManager.Connection = Nothing
        Me.TableAdapterManager.UpdateOrder = CRM_Connect.dsPrinsViewsTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete
        Me.TableAdapterManager.VW_ADRESSEN_FIRMA_CRMTableAdapter = Nothing
        Me.TableAdapterManager.VW_KUNDEN_TO_CRMTableAdapter = Nothing
        Me.TableAdapterManager.VW_PROJEKT_CRMTableAdapter = Nothing
        '
        'taVW_ADRESSEN_FIRMA_CRM
        '
        Me.taVW_ADRESSEN_FIRMA_CRM.ClearBeforeFill = True
        '
        'taVW_PROJEKT_CRM
        '
        Me.taVW_PROJEKT_CRM.ClearBeforeFill = True
        '
        'DsPrins1
        '
        Me.DsPrins1.DataSetName = "dsPrins"
        Me.DsPrins1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ucKunden2CRM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlFirma)
        Me.Controls.Add(Me.pnlPrinsDaten)
        Me.Controls.Add(Me.inpTop)
        Me.Name = "ucKunden2CRM"
        Me.Size = New System.Drawing.Size(1167, 807)
        CType(Me.inpTop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsFirma, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsSQL_CRM_Prins, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFirma.ResumeLayout(False)
        CType(Me.fgProjekt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsProjekt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fgFirma, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.inpMid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPrinsDaten.ResumeLayout(False)
        CType(Me.fgAdressen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsVW_ADRESSEN_FIRMA_CRM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsPrinsViews, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fgProjekte, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VWPROJEKTCRMBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fgExcel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fgKunden, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsVW_KUNDEN_TO_CRM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsPrinsTables, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsSQLCRMPrinsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsVW_PROJEKT_CRM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsPrins1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents inpTop As C1.Win.C1InputPanel.C1InputPanel
  Friend WithEvents bsVW_KUNDEN_TO_CRM As System.Windows.Forms.BindingSource
  Friend WithEvents DsPrinsViews As CRM_Connect.dsPrinsViews
  Friend WithEvents taVW_KUNDEN_TO_CRM As CRM_Connect.dsPrinsViewsTableAdapters.VW_KUNDEN_TO_CRMTableAdapter
  Friend WithEvents TableAdapterManager As CRM_Connect.dsPrinsViewsTableAdapters.TableAdapterManager
  Friend WithEvents bsFirma As System.Windows.Forms.BindingSource
  Friend WithEvents DsSQL_CRM_Prins As CRM_Connect.dsSQL_CRM_Prins
  Friend WithEvents taFirma As CRM_Connect.dsSQL_CRM_PrinsTableAdapters.FirmaTableAdapter
  Friend WithEvents InputGroupHeader1 As C1.Win.C1InputPanel.InputGroupHeader
  Friend WithEvents btnKunden2CRM As C1.Win.C1InputPanel.InputButton
  Friend WithEvents InputSeparator1 As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents InputSeparator2 As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents InputLabel1 As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents tbPrinsKND As C1.Win.C1InputPanel.InputTextBox
  Friend WithEvents InputLabel2 As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents tbFirma As C1.Win.C1InputPanel.InputTextBox
  Friend WithEvents pb1 As C1.Win.C1InputPanel.InputProgressBar
  Friend WithEvents DsPrinsTables As CRM_Connect.dsPrinsTables
  Friend WithEvents taAdressen As CRM_Connect.dsPrinsTablesTableAdapters.ADRESSENTableAdapter
  Friend WithEvents taKreditor As CRM_Connect.dsPrinsTablesTableAdapters.KREDITORTableAdapter
  Friend WithEvents taKunden As CRM_Connect.dsPrinsTablesTableAdapters.KUNDENTableAdapter
  Friend WithEvents taZUORDNUNG_ADRESSE As CRM_Connect.dsPrinsTablesTableAdapters.TBL_ZUORDNUNG_ADRESSETableAdapter
  Friend WithEvents btnCRM2Prins As C1.Win.C1InputPanel.InputButton
  Friend WithEvents taLAENDER As CRM_Connect.dsPrinsTablesTableAdapters.LAENDERTableAdapter
  Friend WithEvents btnAdressen2CRM As C1.Win.C1InputPanel.InputButton
  Friend WithEvents taVW_ADRESSEN_FIRMA_CRM As CRM_Connect.dsPrinsViewsTableAdapters.VW_ADRESSEN_FIRMA_CRMTableAdapter
  Friend WithEvents InputSeparator4 As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents btnExcelLoad As C1.Win.C1InputPanel.InputButton
  Friend WithEvents btnExcelCheck As C1.Win.C1InputPanel.InputButton
  Friend WithEvents ghTabelle As C1.Win.C1InputPanel.InputGroupHeader
  Friend WithEvents taVW_PROJEKT_CRM As CRM_Connect.dsPrinsViewsTableAdapters.VW_PROJEKT_CRMTableAdapter
  Friend WithEvents bsVW_PROJEKT_CRM As System.Windows.Forms.BindingSource
  Friend WithEvents btnSourceView As C1.Win.C1InputPanel.InputButton
  Friend WithEvents pnlFirma As System.Windows.Forms.Panel
  Friend WithEvents fgFirma As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents inpMid As C1.Win.C1InputPanel.C1InputPanel
  Friend WithEvents ghTarget As C1.Win.C1InputPanel.InputGroupHeader
  Friend WithEvents bsVW_ADRESSEN_FIRMA_CRM As System.Windows.Forms.BindingSource
  Friend WithEvents InputSeparator5 As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents pnlPrinsDaten As System.Windows.Forms.Panel
  Friend WithEvents fgAdressen As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents fgProjekte As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents fgExcel As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents fgKunden As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents btnPrinsProjekteCRM As C1.Win.C1InputPanel.InputButton
  Friend WithEvents bsProjekt As System.Windows.Forms.BindingSource
  Friend WithEvents taProjekt As CRM_Connect.dsSQL_CRM_PrinsTableAdapters.ProjektTableAdapter
  Friend WithEvents fgProjekt As C1.Win.C1FlexGrid.C1FlexGrid
  Friend WithEvents btnTagetView As C1.Win.C1InputPanel.InputButton
  Friend WithEvents btnAutoExec As C1.Win.C1InputPanel.InputButton
  Friend WithEvents tmAutoExec As System.Windows.Forms.Timer
  Friend WithEvents btnExecAll As C1.Win.C1InputPanel.InputButton
  Friend WithEvents ltlbMsg As C1.Win.C1InputPanel.InputLabel
  Friend WithEvents tbMsg As C1.Win.C1InputPanel.InputTextBox
  Friend WithEvents InputGroupHeader2 As C1.Win.C1InputPanel.InputGroupHeader
  Friend WithEvents InputSeparator3 As C1.Win.C1InputPanel.InputSeparator
  Friend WithEvents btnCRMProjekte2Prins As C1.Win.C1InputPanel.InputButton
  Friend WithEvents VWPROJEKTCRMBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents DsSQLCRMPrinsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents taPRC_AN_PRJ_ANLAGE As CRM_Connect.dsPrinsTablesTableAdapters.PRC_AN_PRJ_ANLAGETableAdapter
  Friend WithEvents taPERSONAL As CRM_Connect.dsPrinsTableAdapters.PERSONALTableAdapter
  Friend WithEvents btnExcel2Firma As C1.Win.C1InputPanel.InputButton
  Friend WithEvents InputSeparator6 As C1.Win.C1InputPanel.InputSeparator
    Friend WithEvents DsPrins1 As dsPrins
End Class
