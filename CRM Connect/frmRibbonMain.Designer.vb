<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRibbonMain
  Inherits C1.Win.C1Ribbon.C1RibbonForm

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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRibbonMain))
    Me.C1Ribbon1 = New C1.Win.C1Ribbon.C1Ribbon()
    Me.RibbonApplicationMenu1 = New C1.Win.C1Ribbon.RibbonApplicationMenu()
    Me.rmStamm = New C1.Win.C1Ribbon.RibbonMenu()
    Me.RibbonSeparator2 = New C1.Win.C1Ribbon.RibbonSeparator()
    Me.rbAuftraege = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbArtikel = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbIdentNummernListe = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbPruefmittel = New C1.Win.C1Ribbon.RibbonButton()
    Me.RibbonSeparator5 = New C1.Win.C1Ribbon.RibbonSeparator()
    Me.rbSplashScreen = New C1.Win.C1Ribbon.RibbonButton()
    Me.RibbonSeparator1 = New C1.Win.C1Ribbon.RibbonSeparator()
    Me.rbEnde = New C1.Win.C1Ribbon.RibbonButton()
    Me.RibbonConfigToolBar1 = New C1.Win.C1Ribbon.RibbonConfigToolBar()
    Me.RibbonQat1 = New C1.Win.C1Ribbon.RibbonQat()
    Me.rtFile = New C1.Win.C1Ribbon.RibbonTab()
    Me.rgAuftrag = New C1.Win.C1Ribbon.RibbonGroup()
    Me.rbFirma = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbAuftragIdentFolder = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbKundenPrins = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbLogging = New C1.Win.C1Ribbon.RibbonButton()
    Me.rgAnsichtNavigator = New C1.Win.C1Ribbon.RibbonGroup()
    Me.rbAnsichtListe = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbAnsichtForm = New C1.Win.C1Ribbon.RibbonButton()
    Me.rgFind = New C1.Win.C1Ribbon.RibbonGroup()
    Me.rbSearch = New C1.Win.C1Ribbon.RibbonButton()
    Me.rgRecord = New C1.Win.C1Ribbon.RibbonGroup()
    Me.RibbonSeparator4 = New C1.Win.C1Ribbon.RibbonSeparator()
    Me.rbRecordNeu = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbRecordSpeichern = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbRecordLoeschen = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbRecordAktualisieren = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbRecordFehler = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbRecordVerwerfen = New C1.Win.C1Ribbon.RibbonButton()
    Me.rgCopyPaste = New C1.Win.C1Ribbon.RibbonGroup()
    Me.rbCopy = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbPaste = New C1.Win.C1Ribbon.RibbonButton()
    Me.rgExtras = New C1.Win.C1Ribbon.RibbonGroup()
    Me.rbEditIdent = New C1.Win.C1Ribbon.RibbonToggleButton()
    Me.rbStücklisteIdent = New C1.Win.C1Ribbon.RibbonButton()
    Me.rtListen = New C1.Win.C1Ribbon.RibbonTab()
    Me.rgExcel = New C1.Win.C1Ribbon.RibbonGroup()
    Me.rbStartExcel = New C1.Win.C1Ribbon.RibbonButton()
    Me.rgReports = New C1.Win.C1Ribbon.RibbonGroup()
    Me.rbAuftragskarte = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbKennNummernkarte = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbIdentNummernExcel = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbBestueckungslisteQuer = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbBestueckungslisteHoch = New C1.Win.C1Ribbon.RibbonButton()
    Me.rbBestueckungslisteCharge = New C1.Win.C1Ribbon.RibbonButton()
    Me.RibbonTopToolBar1 = New C1.Win.C1Ribbon.RibbonTopToolBar()
    Me.RibbonBottomToolBar1 = New C1.Win.C1Ribbon.RibbonBottomToolBar()
    CType(Me.C1Ribbon1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'C1Ribbon1
    '
    Me.C1Ribbon1.ApplicationMenuHolder = Me.RibbonApplicationMenu1
    Me.C1Ribbon1.BottomToolBarHolder = Me.RibbonBottomToolBar1
    Me.C1Ribbon1.ConfigToolBarHolder = Me.RibbonConfigToolBar1
    Me.C1Ribbon1.Location = New System.Drawing.Point(0, 0)
    Me.C1Ribbon1.Name = "C1Ribbon1"
    Me.C1Ribbon1.QatHolder = Me.RibbonQat1
    Me.C1Ribbon1.Size = New System.Drawing.Size(1016, 153)
    Me.C1Ribbon1.Tabs.Add(Me.rtFile)
    Me.C1Ribbon1.Tabs.Add(Me.rtListen)
    Me.C1Ribbon1.TopToolBarHolder = Me.RibbonTopToolBar1
    '
    'RibbonApplicationMenu1
    '
    Me.RibbonApplicationMenu1.LeftPaneItems.Add(Me.rmStamm)
    Me.RibbonApplicationMenu1.LeftPaneItems.Add(Me.RibbonSeparator2)
    Me.RibbonApplicationMenu1.LeftPaneItems.Add(Me.rbAuftraege)
    Me.RibbonApplicationMenu1.LeftPaneItems.Add(Me.rbArtikel)
    Me.RibbonApplicationMenu1.LeftPaneItems.Add(Me.rbIdentNummernListe)
    Me.RibbonApplicationMenu1.LeftPaneItems.Add(Me.rbPruefmittel)
    Me.RibbonApplicationMenu1.LeftPaneItems.Add(Me.RibbonSeparator5)
    Me.RibbonApplicationMenu1.LeftPaneItems.Add(Me.rbSplashScreen)
    Me.RibbonApplicationMenu1.LeftPaneItems.Add(Me.RibbonSeparator1)
    Me.RibbonApplicationMenu1.LeftPaneItems.Add(Me.rbEnde)
    Me.RibbonApplicationMenu1.Name = "RibbonApplicationMenu1"
    '
    'rmStamm
    '
    Me.rmStamm.LargeImage = CType(resources.GetObject("rmStamm.LargeImage"), System.Drawing.Image)
    Me.rmStamm.Name = "rmStamm"
    Me.rmStamm.Text = "Stammlisten"
    '
    'RibbonSeparator2
    '
    Me.RibbonSeparator2.Name = "RibbonSeparator2"
    '
    'rbAuftraege
    '
    Me.rbAuftraege.LargeImage = CType(resources.GetObject("rbAuftraege.LargeImage"), System.Drawing.Image)
    Me.rbAuftraege.Name = "rbAuftraege"
    Me.rbAuftraege.Text = "Aufträge"
    '
    'rbArtikel
    '
    Me.rbArtikel.LargeImage = CType(resources.GetObject("rbArtikel.LargeImage"), System.Drawing.Image)
    Me.rbArtikel.Name = "rbArtikel"
    Me.rbArtikel.Text = "Artikelstamm"
    '
    'rbIdentNummernListe
    '
    Me.rbIdentNummernListe.LargeImage = CType(resources.GetObject("rbIdentNummernListe.LargeImage"), System.Drawing.Image)
    Me.rbIdentNummernListe.Name = "rbIdentNummernListe"
    Me.rbIdentNummernListe.Text = "Ident-Nummern-Übersicht"
    '
    'rbPruefmittel
    '
    Me.rbPruefmittel.LargeImage = CType(resources.GetObject("rbPruefmittel.LargeImage"), System.Drawing.Image)
    Me.rbPruefmittel.Name = "rbPruefmittel"
    Me.rbPruefmittel.Text = "Mess && Prüfmittel"
    '
    'RibbonSeparator5
    '
    Me.RibbonSeparator5.Name = "RibbonSeparator5"
    '
    'rbSplashScreen
    '
    Me.rbSplashScreen.LargeImage = CType(resources.GetObject("rbSplashScreen.LargeImage"), System.Drawing.Image)
    Me.rbSplashScreen.Name = "rbSplashScreen"
    Me.rbSplashScreen.SmallImage = CType(resources.GetObject("rbSplashScreen.SmallImage"), System.Drawing.Image)
    Me.rbSplashScreen.Text = "Info"
    '
    'RibbonSeparator1
    '
    Me.RibbonSeparator1.Name = "RibbonSeparator1"
    '
    'rbEnde
    '
    Me.rbEnde.LargeImage = CType(resources.GetObject("rbEnde.LargeImage"), System.Drawing.Image)
    Me.rbEnde.Name = "rbEnde"
    Me.rbEnde.Text = "&Beenden"
    '
    'RibbonConfigToolBar1
    '
    Me.RibbonConfigToolBar1.Name = "RibbonConfigToolBar1"
    '
    'RibbonQat1
    '
    Me.RibbonQat1.Name = "RibbonQat1"
    '
    'rtFile
    '
    Me.rtFile.Groups.Add(Me.rgAuftrag)
    Me.rtFile.Groups.Add(Me.rgAnsichtNavigator)
    Me.rtFile.Groups.Add(Me.rgFind)
    Me.rtFile.Groups.Add(Me.rgRecord)
    Me.rtFile.Groups.Add(Me.rgCopyPaste)
    Me.rtFile.Groups.Add(Me.rgExtras)
    Me.rtFile.Name = "rtFile"
    Me.rtFile.Text = "Daten"
    '
    'rgAuftrag
    '
    Me.rgAuftrag.Items.Add(Me.rbFirma)
    Me.rgAuftrag.Items.Add(Me.rbAuftragIdentFolder)
    Me.rgAuftrag.Items.Add(Me.rbKundenPrins)
    Me.rgAuftrag.Items.Add(Me.rbLogging)
    Me.rgAuftrag.Name = "rgAuftrag"
    Me.rgAuftrag.Text = "Quellen"
    '
    'rbFirma
    '
    Me.rbFirma.LargeImage = CType(resources.GetObject("rbFirma.LargeImage"), System.Drawing.Image)
    Me.rbFirma.Name = "rbFirma"
    Me.rbFirma.SmallImage = CType(resources.GetObject("rbFirma.SmallImage"), System.Drawing.Image)
    Me.rbFirma.Text = "CRM Firma"
    '
    'rbAuftragIdentFolder
    '
    Me.rbAuftragIdentFolder.LargeImage = CType(resources.GetObject("rbAuftragIdentFolder.LargeImage"), System.Drawing.Image)
    Me.rbAuftragIdentFolder.Name = "rbAuftragIdentFolder"
    Me.rbAuftragIdentFolder.SmallImage = CType(resources.GetObject("rbAuftragIdentFolder.SmallImage"), System.Drawing.Image)
    Me.rbAuftragIdentFolder.Text = "CRM Projekt"
    '
    'rbKundenPrins
    '
    Me.rbKundenPrins.LargeImage = CType(resources.GetObject("rbKundenPrins.LargeImage"), System.Drawing.Image)
    Me.rbKundenPrins.Name = "rbKundenPrins"
    Me.rbKundenPrins.SmallImage = CType(resources.GetObject("rbKundenPrins.SmallImage"), System.Drawing.Image)
    Me.rbKundenPrins.Text = "Kunden Prins"
    '
    'rbLogging
    '
    Me.rbLogging.LargeImage = CType(resources.GetObject("rbLogging.LargeImage"), System.Drawing.Image)
    Me.rbLogging.Name = "rbLogging"
    Me.rbLogging.SmallImage = CType(resources.GetObject("rbLogging.SmallImage"), System.Drawing.Image)
    Me.rbLogging.Text = "Log"
    '
    'rgAnsichtNavigator
    '
    Me.rgAnsichtNavigator.Items.Add(Me.rbAnsichtListe)
    Me.rgAnsichtNavigator.Items.Add(Me.rbAnsichtForm)
    Me.rgAnsichtNavigator.Name = "rgAnsichtNavigator"
    Me.rgAnsichtNavigator.Text = "Ansicht"
    '
    'rbAnsichtListe
    '
    Me.rbAnsichtListe.LargeImage = CType(resources.GetObject("rbAnsichtListe.LargeImage"), System.Drawing.Image)
    Me.rbAnsichtListe.Name = "rbAnsichtListe"
    Me.rbAnsichtListe.SmallImage = CType(resources.GetObject("rbAnsichtListe.SmallImage"), System.Drawing.Image)
    Me.rbAnsichtListe.Text = "Liste"
    '
    'rbAnsichtForm
    '
    Me.rbAnsichtForm.LargeImage = CType(resources.GetObject("rbAnsichtForm.LargeImage"), System.Drawing.Image)
    Me.rbAnsichtForm.Name = "rbAnsichtForm"
    Me.rbAnsichtForm.SmallImage = CType(resources.GetObject("rbAnsichtForm.SmallImage"), System.Drawing.Image)
    Me.rbAnsichtForm.Text = "Details"
    '
    'rgFind
    '
    Me.rgFind.Items.Add(Me.rbSearch)
    Me.rgFind.Name = "rgFind"
    Me.rgFind.Text = "Suchen"
    '
    'rbSearch
    '
    Me.rbSearch.LargeImage = CType(resources.GetObject("rbSearch.LargeImage"), System.Drawing.Image)
    Me.rbSearch.Name = "rbSearch"
    Me.rbSearch.SmallImage = CType(resources.GetObject("rbSearch.SmallImage"), System.Drawing.Image)
    Me.rbSearch.Text = "Detailsuche"
    '
    'rgRecord
    '
    Me.rgRecord.Items.Add(Me.RibbonSeparator4)
    Me.rgRecord.Items.Add(Me.rbRecordNeu)
    Me.rgRecord.Items.Add(Me.rbRecordSpeichern)
    Me.rgRecord.Items.Add(Me.rbRecordLoeschen)
    Me.rgRecord.Items.Add(Me.rbRecordAktualisieren)
    Me.rgRecord.Items.Add(Me.rbRecordFehler)
    Me.rgRecord.Items.Add(Me.rbRecordVerwerfen)
    Me.rgRecord.Name = "rgRecord"
    Me.rgRecord.Text = "Daten"
    '
    'RibbonSeparator4
    '
    Me.RibbonSeparator4.Name = "RibbonSeparator4"
    '
    'rbRecordNeu
    '
    Me.rbRecordNeu.LargeImage = CType(resources.GetObject("rbRecordNeu.LargeImage"), System.Drawing.Image)
    Me.rbRecordNeu.Name = "rbRecordNeu"
    Me.rbRecordNeu.ShortcutKeyDisplayString = "Neuen Datensatz anlegen F6"
    Me.rbRecordNeu.ShortcutKeys = System.Windows.Forms.Keys.F6
    Me.rbRecordNeu.SmallImage = CType(resources.GetObject("rbRecordNeu.SmallImage"), System.Drawing.Image)
    Me.rbRecordNeu.Text = "Neu"
    Me.rbRecordNeu.ToolTip = "Neuen Datensatz anlegen F6"
    '
    'rbRecordSpeichern
    '
    Me.rbRecordSpeichern.LargeImage = CType(resources.GetObject("rbRecordSpeichern.LargeImage"), System.Drawing.Image)
    Me.rbRecordSpeichern.Name = "rbRecordSpeichern"
    Me.rbRecordSpeichern.ShortcutKeyDisplayString = "Änderungen speichern F9"
    Me.rbRecordSpeichern.ShortcutKeys = System.Windows.Forms.Keys.F9
    Me.rbRecordSpeichern.SmallImage = CType(resources.GetObject("rbRecordSpeichern.SmallImage"), System.Drawing.Image)
    Me.rbRecordSpeichern.Text = "Speichern"
    Me.rbRecordSpeichern.ToolTip = "Änderungen speichern F9"
    '
    'rbRecordLoeschen
    '
    Me.rbRecordLoeschen.LargeImage = CType(resources.GetObject("rbRecordLoeschen.LargeImage"), System.Drawing.Image)
    Me.rbRecordLoeschen.Name = "rbRecordLoeschen"
    Me.rbRecordLoeschen.ShortcutKeyDisplayString = "Aktuellen Datensatz löschen F7"
    Me.rbRecordLoeschen.ShortcutKeys = System.Windows.Forms.Keys.F7
    Me.rbRecordLoeschen.SmallImage = CType(resources.GetObject("rbRecordLoeschen.SmallImage"), System.Drawing.Image)
    Me.rbRecordLoeschen.Text = "Löschen"
    Me.rbRecordLoeschen.ToolTip = "Aktuellen Datensatz löschen F7"
    '
    'rbRecordAktualisieren
    '
    Me.rbRecordAktualisieren.LargeImage = CType(resources.GetObject("rbRecordAktualisieren.LargeImage"), System.Drawing.Image)
    Me.rbRecordAktualisieren.Name = "rbRecordAktualisieren"
    Me.rbRecordAktualisieren.ShortcutKeyDisplayString = "Ansicht aktualisieren - Daten erneut vom Server abrufen F5"
    Me.rbRecordAktualisieren.ShortcutKeys = System.Windows.Forms.Keys.F5
    Me.rbRecordAktualisieren.SmallImage = CType(resources.GetObject("rbRecordAktualisieren.SmallImage"), System.Drawing.Image)
    Me.rbRecordAktualisieren.Text = "Aktualisieren"
    Me.rbRecordAktualisieren.ToolTip = "Ansicht aktualisieren - Daten erneut vom Server abrufen F5"
    '
    'rbRecordFehler
    '
    Me.rbRecordFehler.LargeImage = CType(resources.GetObject("rbRecordFehler.LargeImage"), System.Drawing.Image)
    Me.rbRecordFehler.Name = "rbRecordFehler"
    Me.rbRecordFehler.SmallImage = CType(resources.GetObject("rbRecordFehler.SmallImage"), System.Drawing.Image)
    Me.rbRecordFehler.Text = "Fehler"
    '
    'rbRecordVerwerfen
    '
    Me.rbRecordVerwerfen.LargeImage = CType(resources.GetObject("rbRecordVerwerfen.LargeImage"), System.Drawing.Image)
    Me.rbRecordVerwerfen.Name = "rbRecordVerwerfen"
    Me.rbRecordVerwerfen.ShortcutKeyDisplayString = "Änderungen verwerfen Shift+F5"
    Me.rbRecordVerwerfen.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F5), System.Windows.Forms.Keys)
    Me.rbRecordVerwerfen.SmallImage = CType(resources.GetObject("rbRecordVerwerfen.SmallImage"), System.Drawing.Image)
    Me.rbRecordVerwerfen.Text = "Verwerfen"
    Me.rbRecordVerwerfen.ToolTip = "Änderungen verwerfen Shift+F5"
    '
    'rgCopyPaste
    '
    Me.rgCopyPaste.Items.Add(Me.rbCopy)
    Me.rgCopyPaste.Items.Add(Me.rbPaste)
    Me.rgCopyPaste.Name = "rgCopyPaste"
    Me.rgCopyPaste.Text = "Bearbeiten"
    '
    'rbCopy
    '
    Me.rbCopy.LargeImage = CType(resources.GetObject("rbCopy.LargeImage"), System.Drawing.Image)
    Me.rbCopy.Name = "rbCopy"
    Me.rbCopy.ShortcutKeyDisplayString = "Artikel kopieren Alt+A"
    Me.rbCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
    Me.rbCopy.SmallImage = CType(resources.GetObject("rbCopy.SmallImage"), System.Drawing.Image)
    Me.rbCopy.Text = "kopieren"
    Me.rbCopy.ToolTip = "Artikel kopieren Alt+A"
    '
    'rbPaste
    '
    Me.rbPaste.LargeImage = CType(resources.GetObject("rbPaste.LargeImage"), System.Drawing.Image)
    Me.rbPaste.Name = "rbPaste"
    Me.rbPaste.ShortcutKeyDisplayString = "Artikel/Arbeitsgang einfügen Strg+A"
    Me.rbPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
    Me.rbPaste.SmallImage = CType(resources.GetObject("rbPaste.SmallImage"), System.Drawing.Image)
    Me.rbPaste.SupportedGroupSizing = C1.Win.C1Ribbon.SupportedGroupSizing.TextAlwaysVisible
    Me.rbPaste.Text = "Einfügen"
    Me.rbPaste.ToolTip = "Artikel/Arbeitsgang einfügen Strg+A"
    '
    'rgExtras
    '
    Me.rgExtras.Items.Add(Me.rbEditIdent)
    Me.rgExtras.Items.Add(Me.rbStücklisteIdent)
    Me.rgExtras.Name = "rgExtras"
    Me.rgExtras.Text = "Extras"
    '
    'rbEditIdent
    '
    Me.rbEditIdent.LargeImage = CType(resources.GetObject("rbEditIdent.LargeImage"), System.Drawing.Image)
    Me.rbEditIdent.Name = "rbEditIdent"
    Me.rbEditIdent.SmallImage = CType(resources.GetObject("rbEditIdent.SmallImage"), System.Drawing.Image)
    Me.rbEditIdent.Text = "Ident-Nr. ändern"
    Me.rbEditIdent.ToolTip = "Änderung Ident-Nr. zulassen"
    '
    'rbStücklisteIdent
    '
    Me.rbStücklisteIdent.LargeImage = CType(resources.GetObject("rbStücklisteIdent.LargeImage"), System.Drawing.Image)
    Me.rbStücklisteIdent.Name = "rbStücklisteIdent"
    Me.rbStücklisteIdent.SmallImage = CType(resources.GetObject("rbStücklisteIdent.SmallImage"), System.Drawing.Image)
    Me.rbStücklisteIdent.Text = "Ident-Nr. Stückliste"
    '
    'rtListen
    '
    Me.rtListen.Groups.Add(Me.rgExcel)
    Me.rtListen.Groups.Add(Me.rgReports)
    Me.rtListen.Name = "rtListen"
    Me.rtListen.Text = "Listen / Report"
    '
    'rgExcel
    '
    Me.rgExcel.Items.Add(Me.rbStartExcel)
    Me.rgExcel.Name = "rgExcel"
    Me.rgExcel.Text = "Excel Export"
    '
    'rbStartExcel
    '
    Me.rbStartExcel.LargeImage = CType(resources.GetObject("rbStartExcel.LargeImage"), System.Drawing.Image)
    Me.rbStartExcel.Name = "rbStartExcel"
    Me.rbStartExcel.SmallImage = CType(resources.GetObject("rbStartExcel.SmallImage"), System.Drawing.Image)
    Me.rbStartExcel.Text = "Starten"
    '
    'rgReports
    '
    Me.rgReports.Items.Add(Me.rbAuftragskarte)
    Me.rgReports.Items.Add(Me.rbKennNummernkarte)
    Me.rgReports.Items.Add(Me.rbIdentNummernExcel)
    Me.rgReports.Items.Add(Me.rbBestueckungslisteQuer)
    Me.rgReports.Items.Add(Me.rbBestueckungslisteHoch)
    Me.rgReports.Items.Add(Me.rbBestueckungslisteCharge)
    Me.rgReports.Name = "rgReports"
    Me.rgReports.Text = "Formulare"
    '
    'rbAuftragskarte
    '
    Me.rbAuftragskarte.LargeImage = CType(resources.GetObject("rbAuftragskarte.LargeImage"), System.Drawing.Image)
    Me.rbAuftragskarte.Name = "rbAuftragskarte"
    Me.rbAuftragskarte.SmallImage = CType(resources.GetObject("rbAuftragskarte.SmallImage"), System.Drawing.Image)
    Me.rbAuftragskarte.Text = "Auftrags-/Laufkarte"
    '
    'rbKennNummernkarte
    '
    Me.rbKennNummernkarte.LargeImage = CType(resources.GetObject("rbKennNummernkarte.LargeImage"), System.Drawing.Image)
    Me.rbKennNummernkarte.Name = "rbKennNummernkarte"
    Me.rbKennNummernkarte.SmallImage = CType(resources.GetObject("rbKennNummernkarte.SmallImage"), System.Drawing.Image)
    Me.rbKennNummernkarte.Text = "Kenn-Nummernkarte"
    '
    'rbIdentNummernExcel
    '
    Me.rbIdentNummernExcel.LargeImage = CType(resources.GetObject("rbIdentNummernExcel.LargeImage"), System.Drawing.Image)
    Me.rbIdentNummernExcel.Name = "rbIdentNummernExcel"
    Me.rbIdentNummernExcel.SmallImage = CType(resources.GetObject("rbIdentNummernExcel.SmallImage"), System.Drawing.Image)
    Me.rbIdentNummernExcel.Text = "Identnummern-Liste"
    '
    'rbBestueckungslisteQuer
    '
    Me.rbBestueckungslisteQuer.LargeImage = CType(resources.GetObject("rbBestueckungslisteQuer.LargeImage"), System.Drawing.Image)
    Me.rbBestueckungslisteQuer.Name = "rbBestueckungslisteQuer"
    Me.rbBestueckungslisteQuer.SmallImage = CType(resources.GetObject("rbBestueckungslisteQuer.SmallImage"), System.Drawing.Image)
    Me.rbBestueckungslisteQuer.Text = "Bestückungsliste Querformat"
    '
    'rbBestueckungslisteHoch
    '
    Me.rbBestueckungslisteHoch.LargeImage = CType(resources.GetObject("rbBestueckungslisteHoch.LargeImage"), System.Drawing.Image)
    Me.rbBestueckungslisteHoch.Name = "rbBestueckungslisteHoch"
    Me.rbBestueckungslisteHoch.SmallImage = CType(resources.GetObject("rbBestueckungslisteHoch.SmallImage"), System.Drawing.Image)
    Me.rbBestueckungslisteHoch.Text = "Bestückungsliste Hochformat"
    '
    'rbBestueckungslisteCharge
    '
    Me.rbBestueckungslisteCharge.LargeImage = CType(resources.GetObject("rbBestueckungslisteCharge.LargeImage"), System.Drawing.Image)
    Me.rbBestueckungslisteCharge.Name = "rbBestueckungslisteCharge"
    Me.rbBestueckungslisteCharge.SmallImage = CType(resources.GetObject("rbBestueckungslisteCharge.SmallImage"), System.Drawing.Image)
    Me.rbBestueckungslisteCharge.Text = "Bestückungsliste mit Charge"
    '
    'RibbonTopToolBar1
    '
    Me.RibbonTopToolBar1.Name = "RibbonTopToolBar1"
    '
    'RibbonBottomToolBar1
    '
    Me.RibbonBottomToolBar1.Name = "RibbonBottomToolBar1"
    '
    'frmRibbonMain
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1016, 760)
    Me.Controls.Add(Me.C1Ribbon1)
    Me.MinimumSize = New System.Drawing.Size(1024, 768)
    Me.Name = "frmRibbonMain"
    Me.Text = "frmRibbonMain"
    Me.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver
    CType(Me.C1Ribbon1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents C1Ribbon1 As C1.Win.C1Ribbon.C1Ribbon
  Private WithEvents RibbonApplicationMenu1 As C1.Win.C1Ribbon.RibbonApplicationMenu
  Private WithEvents rmStamm As C1.Win.C1Ribbon.RibbonMenu
  Private WithEvents RibbonSeparator2 As C1.Win.C1Ribbon.RibbonSeparator
  Private WithEvents rbAuftraege As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbArtikel As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbIdentNummernListe As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbPruefmittel As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents RibbonSeparator5 As C1.Win.C1Ribbon.RibbonSeparator
  Private WithEvents rbSplashScreen As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents RibbonSeparator1 As C1.Win.C1Ribbon.RibbonSeparator
  Private WithEvents rbEnde As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents RibbonConfigToolBar1 As C1.Win.C1Ribbon.RibbonConfigToolBar
  Private WithEvents RibbonQat1 As C1.Win.C1Ribbon.RibbonQat
  Private WithEvents rtFile As C1.Win.C1Ribbon.RibbonTab
  Private WithEvents rgAuftrag As C1.Win.C1Ribbon.RibbonGroup
  Private WithEvents rbFirma As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbAuftragIdentFolder As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbKundenPrins As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbLogging As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rgAnsichtNavigator As C1.Win.C1Ribbon.RibbonGroup
  Private WithEvents rbAnsichtListe As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbAnsichtForm As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rgFind As C1.Win.C1Ribbon.RibbonGroup
  Private WithEvents rbSearch As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rgRecord As C1.Win.C1Ribbon.RibbonGroup
  Private WithEvents RibbonSeparator4 As C1.Win.C1Ribbon.RibbonSeparator
  Private WithEvents rbRecordNeu As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbRecordSpeichern As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbRecordLoeschen As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbRecordAktualisieren As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbRecordFehler As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbRecordVerwerfen As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rgCopyPaste As C1.Win.C1Ribbon.RibbonGroup
  Private WithEvents rbCopy As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbPaste As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rgExtras As C1.Win.C1Ribbon.RibbonGroup
  Private WithEvents rbEditIdent As C1.Win.C1Ribbon.RibbonToggleButton
  Private WithEvents rbStücklisteIdent As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rtListen As C1.Win.C1Ribbon.RibbonTab
  Private WithEvents rgExcel As C1.Win.C1Ribbon.RibbonGroup
  Private WithEvents rbStartExcel As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rgReports As C1.Win.C1Ribbon.RibbonGroup
  Private WithEvents rbAuftragskarte As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbKennNummernkarte As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbIdentNummernExcel As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbBestueckungslisteQuer As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbBestueckungslisteHoch As C1.Win.C1Ribbon.RibbonButton
  Private WithEvents rbBestueckungslisteCharge As C1.Win.C1Ribbon.RibbonButton
  Friend WithEvents RibbonBottomToolBar1 As C1.Win.C1Ribbon.RibbonBottomToolBar
  Friend WithEvents RibbonTopToolBar1 As C1.Win.C1Ribbon.RibbonTopToolBar
End Class
