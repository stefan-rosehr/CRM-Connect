Imports C1.Win.C1FlexGrid.Extensions

Public Class ucKunden2CRM
  Implements ifTableAdapter

  Implements ifRecordEvent

  Private Enum VisibleFlexEnum
    Kunden
    Projekte
    Adressen
    Excel
  End Enum

  Private _ButtonsEnabled As Boolean = True
  Private Property ButtonsEnabled As Boolean
    Set(value As Boolean)
      btnKunden2CRM.Enabled = value
      btnAdressen2CRM.Enabled = False
      btnPrinsProjekteCRM.Enabled = value
      btnCRM2Prins.Enabled = value
      btnSourceView.Enabled = value
      btnTagetView.Enabled = value
      btnExcelLoad.Enabled = value
      btnExcelCheck.Enabled = value
      btnExecAll.Enabled = value
      btnCRMProjekte2Prins.Enabled = value
      _ButtonsEnabled = value
      inpTop.Update()
    End Set
    Get
      Return _ButtonsEnabled
    End Get
  End Property


  Private _VisibleFlex As VisibleFlexEnum = VisibleFlexEnum.Kunden
  Private Property VisibleFlex As VisibleFlexEnum
    Set(value As VisibleFlexEnum)

      fgExcel.Visible = False
      fgKunden.Visible = False
      fgProjekte.Visible = False
      fgAdressen.Visible = False

      Select Case value
        Case VisibleFlexEnum.Excel
          fgExcel.Visible = True
          ghTabelle.Text = "Excel-Überprüfung"
        Case VisibleFlexEnum.Kunden
          fgKunden.Visible = True
          ghTabelle.Text = DsPrinsViews.VW_KUNDEN_TO_CRM.TableName
          If Not fgFirma.Visible Then btnTagetView_Click(Me.btnTagetView, System.EventArgs.Empty)
        Case VisibleFlexEnum.Projekte
          fgProjekte.Visible = True
          ghTabelle.Text = DsPrinsViews.VW_PROJEKT_CRM.TableName
          If Not fgProjekt.Visible Then btnTagetView_Click(Me.btnTagetView, System.EventArgs.Empty)
        Case VisibleFlexEnum.Adressen
          fgAdressen.Visible = True
          ghTabelle.Text = DsPrinsViews.VW_ADRESSEN_FIRMA_CRM.TableName
          If Not fgFirma.Visible Then btnTagetView_Click(Me.btnTagetView, System.EventArgs.Empty)
      End Select

      _VisibleFlex = value
      Me.Reload()
    End Set
    Get
      Return _VisibleFlex
    End Get
  End Property


  Private _KundenRecordEvents As clRecordEvents
  Private ReadOnly Property KundenRecordEvents As clRecordEvents
    Get
      If IsNothing(_KundenRecordEvents) Then
        _KundenRecordEvents = New clRecordEvents(Me.bsVW_KUNDEN_TO_CRM, Me)

      End If
      Return _KundenRecordEvents
    End Get
  End Property

  Private _FirmaRecordEvents As clRecordEvents
  Private ReadOnly Property FirmaRecordEvents As clRecordEvents
    Get
      If IsNothing(_FirmaRecordEvents) Then
        _FirmaRecordEvents = New clRecordEvents(Me.bsFirma, Me)
      End If
      Return _FirmaRecordEvents
    End Get
  End Property

  Public Sub Reload() Implements ifTableAdapter.Reload

    Select Case VisibleFlex
      Case VisibleFlexEnum.Kunden
        Me.taVW_KUNDEN_TO_CRM.Fill(DsPrinsViews.VW_KUNDEN_TO_CRM)
      Case VisibleFlexEnum.Projekte
        Me.taVW_PROJEKT_CRM.Fill(DsPrinsViews.VW_PROJEKT_CRM)
      Case VisibleFlexEnum.Adressen
        Me.taVW_ADRESSEN_FIRMA_CRM.Fill(DsPrinsViews.VW_ADRESSEN_FIRMA_CRM)
      Case VisibleFlexEnum.Excel

    End Select
    ' --------------------------------------------------------------------------------
    If fgFirma.Visible Then
      Me.taFirma.Fill(DsSQL_CRM_Prins.Firma)
    Else
      Me.taProjekt.Fill(DsSQL_CRM_Prins.Projekt)
    End If


  End Sub

  Public Sub Save() Implements ifTableAdapter.Save
    Me.taFirma.Update(DsSQL_CRM_Prins.Firma)
    Me.taVW_KUNDEN_TO_CRM.Update(DsPrinsViews.VW_KUNDEN_TO_CRM)
  End Sub

  Public ReadOnly Property ActiveRecordEvents As clRecordEvents Implements ifRecordEvent.ActiveRecordEvents
    Get
      Return FirmaRecordEvents
    End Get
  End Property

  Private Sub ucFirma_Load(sender As Object, e As EventArgs) Handles Me.Load
    KundenRecordEvents.Reload()
    FirmaRecordEvents.Reload()

    pnlPrinsDaten.Height = (Me.Height - inpTop.Height) / 3

    fgKunden.Dock = DockStyle.Fill
    fgProjekte.Dock = DockStyle.Fill
    fgAdressen.Dock = DockStyle.Fill
    fgExcel.Dock = DockStyle.Fill
    fgFirma.Dock = DockStyle.Fill
    fgProjekt.Dock = DockStyle.Fill

    VisibleFlex = VisibleFlexEnum.Kunden

    If My.Settings.btnAutoExecPressed Then
      btnAutoExec.Pressed = True
    End If

    If System.Diagnostics.Debugger.IsAttached Then
      tmAutoExec.Interval = 1000 * 10
    End If

  End Sub

  Private Sub btnPrinsProjekteCRM_Click(sender As Object, e As EventArgs) Handles btnPrinsProjekteCRM.Click
    Dim sr As dsPrinsViews.VW_PROJEKT_CRMRow
    Dim BtnEnabled As Boolean = ButtonsEnabled

    VisibleFlex = VisibleFlexEnum.Projekte : Application.DoEvents()
    My.Application.Logging.Add("PROJEKT Export an CRM gestartet", clMessageLog.MsgSourceEnum.CRMImportExport, clMessageLog.MsgSubTypeEnum.StartMessage)

    Try
      ButtonsEnabled = False
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Visible
      pb1.Value = 0
      pb1.Maximum = DsPrinsViews.VW_PROJEKT_CRM.Rows.Count
      My.Application.Logging.Add(pb1.Maximum.ToString + " Änderungen zu übertragen", clMessageLog.MsgSourceEnum.CRMImportExport)

      'For Each sr In DsPrinsViews.VW_KUNDEN_TO_CRM.Rows

      For i As Integer = DsPrinsViews.VW_PROJEKT_CRM.Rows.Count - 1 To 0 Step -1
        sr = DsPrinsViews.VW_PROJEKT_CRM.Rows(i)

        My.Application.Logging.Add("[" + sr.PROJEKTNUMMER.ToString + "] [" + sr.BEZEICHNUNG + "] wird übertragen", clMessageLog.MsgSourceEnum.CRMImportExport)

        InsertOrUpdateProjekt(sr)
        sr.Delete() : Me.taVW_PROJEKT_CRM.Update(DsPrinsViews.VW_PROJEKT_CRM)
        pb1.PerformStep() : inpTop.Update()
      Next

    Finally
      Me.Reload()
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
      ButtonsEnabled = BtnEnabled
    End Try

    My.Application.Logging.Add("PROJEKT Export an CRM beendet: " + pb1.Maximum.ToString + " übertragen", clMessageLog.MsgSourceEnum.CRMImportExport, clMessageLog.MsgSubTypeEnum.FinalSummary)

  End Sub


  Private Sub btnKunden2CRM_Click(sender As Object, e As EventArgs) Handles btnKunden2CRM.Click
    Dim sr As dsPrinsViews.VW_KUNDEN_TO_CRMRow
    Dim BtnEnabled As Boolean = ButtonsEnabled

    VisibleFlex = VisibleFlexEnum.Kunden
    My.Application.Logging.Add("KUNDEN Export an CRM gestartet", clMessageLog.MsgSourceEnum.CRMImportExport, clMessageLog.MsgSubTypeEnum.StartMessage)

    Try
      ButtonsEnabled = False
      Me.btnKunden2CRM.Enabled = False
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Visible
      pb1.Value = 0
      pb1.Maximum = DsPrinsViews.VW_KUNDEN_TO_CRM.Rows.Count
      My.Application.Logging.Add(pb1.Maximum.ToString + " Änderungen zu übertragen", clMessageLog.MsgSourceEnum.CRMImportExport)

      'For Each sr In DsPrinsViews.VW_KUNDEN_TO_CRM.Rows

      For i As Integer = DsPrinsViews.VW_KUNDEN_TO_CRM.Rows.Count - 1 To 0 Step -1
        sr = DsPrinsViews.VW_KUNDEN_TO_CRM.Rows(i)

        My.Application.Logging.Add("[" + sr.FIRMENNUMMER.ToString + "] [" + sr.FIRMENNAME1 + "] wird übertragen", clMessageLog.MsgSourceEnum.CRMImportExport)

        InsertOrUpdateFirma(sr)
        sr.Delete()
        pb1.PerformStep() : inpTop.Update()
      Next

    Finally
      Me.Reload()
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
      ButtonsEnabled = BtnEnabled
    End Try

    My.Application.Logging.Add("KUNDEN Export an CRM beendet: " + pb1.Maximum.ToString + " übertragen", clMessageLog.MsgSourceEnum.CRMImportExport, clMessageLog.MsgSubTypeEnum.FinalSummary)

  End Sub

  Private Sub InsertOrUpdateFirma(Source As dsPrinsViews.VW_KUNDEN_TO_CRMRow)
    Dim tr As dsSQL_CRM_Prins.FirmaRow
    Dim idx As Integer = bsFirma.Find("Firmennummer", Source.FIRMENNUMMER)

    If idx > -1 Then tr = bsFirma(idx).Row Else tr = DsSQL_CRM_Prins.Firma.NewFirmaRow
    tr.BeginEdit()
    For Each Col As DataColumn In DsSQL_CRM_Prins.Firma.Columns
      If Source.Table.Columns.Contains(Col.ColumnName) Then
        tr(Col) = Source(Col.ColumnName)
      End If
    Next

    If tr.RowState = DataRowState.Detached Then
      DsSQL_CRM_Prins.Firma.AddFirmaRow(tr)
    Else
      tr.EndEdit()
    End If

  End Sub


  Private ReadOnly Property ProjektNummerFormated(PrjNummer As String) As String
    Get
      Dim tmp As String = PrjNummer.Trim
      Dim result As String = String.Empty

      If tmp.Length = 7 Then
        result = tmp.Substring(0, 1) + "." + tmp.Substring(1, 2) + "." + tmp.Substring(3)
      ElseIf tmp.Length = 6 Then
        result = tmp.Substring(1, 2) + "." + tmp.Substring(3)
      Else
        result = tmp
      End If

      Return result
    End Get
  End Property


  Private Sub InsertOrUpdateProjekt(Source As dsPrinsViews.VW_PROJEKT_CRMRow)
    Dim tr As dsSQL_CRM_Prins.ProjektRow
    Dim idx As Integer = bsProjekt.Find("Projektnummer", ProjektNummerFormated(Source.PROJEKTNUMMER))

    If idx > -1 Then tr = bsProjekt(idx).Row Else tr = DsSQL_CRM_Prins.Projekt.NewProjektRow
    tr.BeginEdit()
    For Each Col As DataColumn In DsSQL_CRM_Prins.Projekt.Columns
      If Col.ReadOnly Then Continue For
      ' ---------------------------------------------------------------------------------
      If Source.Table.Columns.Contains(Col.ColumnName) Then
        If Col.ColumnName.ToUpper = "PROJEKTNUMMER" Then
          tr(Col) = ProjektNummerFormated(Source(Col.ColumnName).ToString)
          tr("Projektnummer_int") = Source(Col.ColumnName)
        Else
          tr(Col) = Source(Col.ColumnName)
        End If
      End If
    Next

    tr.Zielsystem = 1
    If tr.RowState = DataRowState.Detached Then
      DsSQL_CRM_Prins.Projekt.AddProjektRow(tr)
    Else
      tr.EndEdit()
    End If
    Me.taProjekt.Update(DsSQL_CRM_Prins.Projekt)

  End Sub



  Private Sub bsFirma_ListChanged(sender As Object, e As System.ComponentModel.ListChangedEventArgs) Handles bsFirma.ListChanged
    Me.tbFirma.Text = bsFirma.Count.ToString
  End Sub

  Private Sub bsVW_KUNDEN_TO_CRM_ListChanged(sender As Object, e As System.ComponentModel.ListChangedEventArgs) Handles bsVW_KUNDEN_TO_CRM.ListChanged
    Me.tbPrinsKND.Text = bsVW_KUNDEN_TO_CRM.Count.ToString
  End Sub

  Private Sub btnCRM2Prins_Click(sender As Object, e As EventArgs) Handles btnCRM2Prins.Click
    Dim vw As New DataView(Me.DsSQL_CRM_Prins.Firma)
    Dim sr As dsSQL_CRM_Prins.FirmaRow
    Dim BtnEnabled As Boolean = ButtonsEnabled

    vw.RowFilter = "Zielsystem=2"

    Try
      ButtonsEnabled = False

      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Visible
      pb1.Value = 0
      pb1.Maximum = vw.Count

      For i As Integer = vw.Count - 1 To 0 Step -1
        Dim AdrRow As dsPrinsTables.ADRESSENRow
        Dim AddNew As Boolean = False

        sr = vw(i).Row

        Me.taAdressen.FillByFirmenID(DsPrinsTables.ADRESSEN, sr.FirmenID)
        ' Ist da -> Update
        If DsPrinsTables.ADRESSEN.Rows.Count = 1 Then
          AdrRow = DsPrinsTables.ADRESSEN.Rows(0)
        Else 'Nicht da -> Neu
          AdrRow = DsPrinsTables.ADRESSEN.NewADRESSENRow
          AdrRow.ID = My.Forms.frmMain.GetGeneratorValue("GEN_ADRESSEN_ID")
          AddNew = True
        End If

        AdrRow.DONT_UPDATE_STAGEING = 1 ' damit kein Änderungssatz in die TBL_CRM_STAGEING schrieben wird.
        AdrRow.TYP = 1
        AdrRow.IST_ENTSCHEIDER = 0
        AdrRow.NAME1 = sr.Firmenname1
        If Not sr.IsFirmenname2Null Then AdrRow.NAME2 = sr.Firmenname2
        If Not sr.IsFirmenname3Null Then AdrRow.NAME3 = sr.Firmenname3
        If Not sr.IsAnschriftNull Then AdrRow.ANSCHRIFT = sr.Anschrift
        If Not sr.IsPLZNull Then AdrRow.PLZ = sr.PLZ
        If Not sr.IsOrtNull Then AdrRow.ORT = sr.Ort
        If Not sr.IsFirmenIDNull Then AdrRow.FIRMENID = sr.FirmenID
        If Not sr.IsLandNull Then AdrRow.LAND = ISOLandByName(sr.Land)
        If Not sr.IsTelefonNull Then AdrRow.TELEFON = sr.Telefon
        If Not sr.IsFaxNull Then AdrRow.FAX = sr.Fax
        If Not sr.IsEMailNull Then AdrRow.EMAIL = sr.EMail
        If Not sr.IsHomepageNull Then AdrRow.INTERNET_URL = sr.Homepage
        If Not sr.IsSpracheNull Then AdrRow.SPRACHE = sr.Sprache
        If Not sr.IsBesitzerNull Then AdrRow.BESITZER = sr.Besitzer
        If Not sr.IsStateCodeNull Then AdrRow.AKTIV = Not sr.StateCode Else AdrRow.AKTIV = True
        If Not sr.IsKostenstelleNull AndAlso My.Application.DBInteger(sr.Kostenstelle) > 0 Then AdrRow.KOSTENSTELLE = My.Application.DBInteger(sr.Kostenstelle)
        If Not sr.IsKuerzelKontaktNull Then AdrRow.KUERZELKONTAKT = sr.KuerzelKontakt
        '
        If AddNew Then DsPrinsTables.ADRESSEN.AddADRESSENRow(AdrRow)
        Me.taAdressen.Update(DsPrinsTables.ADRESSEN) 'speichern

        ' Zuordnung anlegen
        If AddNew Then
          taZUORDNUNG_ADRESSE.FillById(DsPrinsTables.TBL_ZUORDNUNG_ADRESSE, -1) 'Leermenge
          Dim zr As dsPrinsTables.TBL_ZUORDNUNG_ADRESSERow = DsPrinsTables.TBL_ZUORDNUNG_ADRESSE.NewTBL_ZUORDNUNG_ADRESSERow

          zr.ID = My.Forms.frmMain.GetGeneratorValue("GEN_TBL_ZUORDNUNG_ADRESSE_ID")
          zr.ADRESSEN_ID = AdrRow.ID
          zr.URSPRUNG = 5
          zr.USERGROUP = 1
          DsPrinsTables.TBL_ZUORDNUNG_ADRESSE.Rows.Add(zr)
          taZUORDNUNG_ADRESSE.Update(DsPrinsTables.TBL_ZUORDNUNG_ADRESSE)
        End If
        ' --------------------------------------------------------------------------------------------
        sr.Delete()

        pb1.PerformStep() : inpTop.Update()
      Next

      Me.taFirma.Update(Me.DsSQL_CRM_Prins.Firma)
    Finally
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
      Me.Reload()
      ButtonsEnabled = BtnEnabled
    End Try


  End Sub


  Private Function ISOLandByName(LandName As String) As String
    Dim Result As String = "DE"
    Dim dt As dsPrinsTables.LAENDERDataTable
    dt = Me.taLAENDER.GetDataByName(LandName)
    If dt.Rows.Count >= 1 Then Result = dt.Rows(0)("SCHLUESSEL")
    Return Result
  End Function


  Private Sub btnAdressen2CRM_Click(sender As Object, e As EventArgs) Handles btnAdressen2CRM.Click
    Dim sr As dsPrinsViews.VW_ADRESSEN_FIRMA_CRMRow
    Dim BtnEnabled As Boolean = ButtonsEnabled

    VisibleFlex = VisibleFlexEnum.Adressen
    taVW_ADRESSEN_FIRMA_CRM.Fill(DsPrinsViews.VW_ADRESSEN_FIRMA_CRM)

    Try
      ButtonsEnabled = False
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Visible
      pb1.Value = 0
      pb1.Maximum = DsPrinsViews.VW_ADRESSEN_FIRMA_CRM.Rows.Count

      'For Each sr In DsPrinsViews.VW_KUNDEN_TO_CRM.Rows

      For i As Integer = DsPrinsViews.VW_ADRESSEN_FIRMA_CRM.Rows.Count - 1 To 0 Step -1
        sr = DsPrinsViews.VW_ADRESSEN_FIRMA_CRM.Rows(i)
        InsertOrUpdateFirma(sr)
        sr.Delete() : taVW_ADRESSEN_FIRMA_CRM.Update(DsPrinsViews.VW_ADRESSEN_FIRMA_CRM)
        pb1.PerformStep() : inpTop.Update()
      Next

    Finally
      Me.Reload()
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
      ButtonsEnabled = BtnEnabled
    End Try
  End Sub


  Private Sub InsertOrUpdateFirma(Source As dsPrinsViews.VW_ADRESSEN_FIRMA_CRMRow)
    Dim tr As dsSQL_CRM_Prins.FirmaRow
    Dim idx As Integer = -1

    If Not Source.IsFIRMENIDNull Then
      idx = bsFirma.Find("FirmenID", Source.FIRMENID)
    End If

    If idx > -1 Then tr = bsFirma(idx).Row Else tr = DsSQL_CRM_Prins.Firma.NewFirmaRow
    tr.BeginEdit()
    For Each Col As DataColumn In DsSQL_CRM_Prins.Firma.Columns
      If Source.Table.Columns.Contains(Col.ColumnName) AndAlso Not Col.ReadOnly Then
        If Col.DataType Is GetType(String) Then
          tr(Col) = Me.StringValue(Source(Col.ColumnName), Col.MaxLength)
        Else
          tr(Col) = Source(Col.ColumnName)
        End If
      End If
    Next

    If tr.RowState = DataRowState.Detached Then
      DsSQL_CRM_Prins.Firma.AddFirmaRow(tr)
    Else
      tr.EndEdit()
    End If

  End Sub

  Private Sub btnExcelLoad_Click(sender As Object, e As EventArgs) Handles btnExcelLoad.Click
    Dim win As OpenFileDialog = New OpenFileDialog
    Me.fgExcel.Visible = True : fgKunden.Visible = False : fgKunden.Visible = False : pnlFirma.Visible = False

    win.InitialDirectory = My.Settings.LastExcelLoadPath
    win.Filter = "Microsoft Excel |*.xls?"
    win.FilterIndex = 0
    win.Multiselect = False
    win.AddExtension = True

    If win.ShowDialog = Windows.Forms.DialogResult.OK AndAlso win.FileName.Length > 0 Then
      My.Settings.LastExcelLoadPath = System.IO.Path.GetDirectoryName(win.FileName)
      fgExcel.LoadExcel(win.FileName)

      If fgExcel.Rows.Count > fgExcel.Rows.Fixed Then
        For c As Integer = fgExcel.Cols.Fixed To fgExcel.Cols.Count - 1
          fgExcel.Cols(c).Caption = fgExcel(fgExcel.Rows.Fixed, c)
        Next

        fgExcel.RemoveItem(fgExcel.Rows.Fixed)
      End If

      For i As Integer = 0 To fgExcel.Cols.Count - 1
        fgExcel.Cols(i).Name = StringValue(fgExcel(0, i))
      Next

    End If


  End Sub

  Private Function StringValue([Value] As Object, Optional MaxLength As Integer = -1) As String
    Dim Result As String = String.Empty

    If Not IsDBNull(Value) AndAlso Not IsNothing(Value) Then

      Try
        Result = Value.ToString
      Catch ex As Exception
      End Try

    End If

    Result = Result.Trim

    If MaxLength > 0 Then
      Result = Result.Substring(0, IIf(Result.Length > MaxLength, MaxLength, Result.Length))
    End If

    Return Result
  End Function

  Private Sub btnExcelCheck_Click(sender As Object, e As EventArgs) Handles btnExcelCheck.Click
    Dim FullFileName As String

    ' ------------------------------------------------------------------------------------------------------------------------
    Dim win As SaveFileDialog = New SaveFileDialog

    win.InitialDirectory = My.Settings.LastExcelLoadPath
    win.DefaultExt = "xlsx"
    win.AddExtension = True

    If win.ShowDialog = Windows.Forms.DialogResult.OK AndAlso win.FileName.Length > 0 Then
      ' Pre-Save ...
      FullFileName = win.FileName
      Me.fgExcel.SaveExcel(FullFileName, C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells)
    Else
      Exit Sub
    End If


    Try
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Visible
      pb1.Value = 0
      pb1.Maximum = fgExcel.Rows.Count
      '------------------------------------------------------------------------------------

      For r As Integer = fgExcel.Rows.Fixed To fgExcel.Rows.Count - 1
        Dim KndName As String = StringValue(fgExcel(r, "Firmenname")).ToString.Trim
        Dim KndName2 As String = StringValue(fgExcel(r, "Firmenname2")).ToString.Trim
        Dim Land As String = fgExcel(r, "Adresse 1: Land/Region")
        Dim tmp As String

        '------------------------------------------------------------------------------------
        ' Rückschreiben wg. Leerzeichen hinten
        If Not String.IsNullOrEmpty(KndName) Then
          fgExcel(r, "Firmenname") = KndName
          KndName = KndName.Trim
        ElseIf Not String.IsNullOrEmpty(fgExcel(r, "Firmennummer")) Then

          If DsPrinsTables.KUNDEN.Rows.Count = 0 OrElse fgExcel(r, "Firmennummer") <> DsPrinsTables.KUNDEN.Rows(0)("NUMMER") Then
            Me.taKunden.FillByNummer(DsPrinsTables.KUNDEN, fgExcel(r, "Firmennummer"))
          End If

          If DsPrinsTables.KUNDEN.Rows.Count > 0 Then
            fgExcel(r, "Firmenname") = DsPrinsTables.KUNDEN.Rows(0)("NAME1")
          End If

        End If

        '------------------------------------------------------------------------------------
        If String.IsNullOrEmpty(Land) Then
          Land = "Deutschland"
          fgExcel(r, "Adresse 1: Land/Region") = Land
        End If

        '------------------------------------------------------------------------------------
        If fgExcel.Cols.Contains("Geschlecht") Then
          tmp = StringValue(fgExcel(r, "Geschlecht")).Trim
          If tmp <> "männlich" AndAlso tmp <> "weiblich" Then
            If tmp.StartsWith("H") Then tmp = "männlich"
            If tmp.StartsWith("F") Then tmp = "weiblich"
            If String.IsNullOrEmpty(tmp) Then tmp = "männlich"
            fgExcel(r, "Geschlecht") = tmp
          End If
        End If
        '------------------------------------------------------------------------------------
        CheckTelFaxNumer(r, Land, "Telefon (geschäftlich)", "Adresse 1: Telefon")
        CheckTelFaxNumer(r, Land, "Adresse 1: Telefon", "Telefon (geschäftlich)")
        CheckTelFaxNumer(r, Land, "Fax", "Fax (Kontakt)")
        CheckTelFaxNumer(r, Land, "Fax (Kontakt)", "Fax")
        '------------------------------------------------------------------------------------
        If fgExcel.Cols.Contains("Mail") Then
          tmp = StringValue(fgExcel(r, "Mail")).Trim.ToLower
          tmp = tmp.Replace("mailto:", "")
          fgExcel(r, "Mail") = tmp
        End If
        '------------------------------------------------------------------------------------
        Dim PLZ As String
        tmp = StringValue(fgExcel(r, "Adresse 1: Postleitzahl")).Trim
        PLZ = tmp

        If Land.StartsWith("Deutsch") AndAlso tmp.StartsWith("D-") Then
          tmp = tmp.Substring(2)
          PLZ = tmp
        End If

        If Not tmp.StartsWith("{\rtf") Then
          PLZ = StringValue(tmp, DsSQL_CRM_Prins.Firma.PLZColumn.MaxLength)
          fgExcel(r, "Adresse 1: Postleitzahl") = PLZ
        Else
          'tbMsg.Text += "RTF Content: " + KndName + ","
        End If
        '------------------------------------------------------------------------------------
        If fgExcel.Cols.Contains("Primärer HPC Kontakt") Then
          tmp = StringValue(fgExcel(r, "Primärer HPC Kontakt")).Trim.ToUpper
          If tmp.IndexOf("/") > -1 Then tmp = tmp.Split("/")(0)
          taPERSONAL.FillByKuerzel(DsPrins1.PERSONAL, tmp)

          If DsPrins1.PERSONAL.Rows.Count = 1 Then
            fgExcel(r, "Primärer HPC Kontakt") = DsPrins1.PERSONAL.Rows(0)("NAME")
          End If
        End If

          '"Primärer HPC Kontakt"
          '------------------------------------------------------------------------------------
          ' Kundenname aber keine Firmennummer, Suchen im KUNDEN-Stamm
          If Not String.IsNullOrEmpty(KndName) AndAlso String.IsNullOrEmpty(fgExcel(r, "Firmennummer")) Then

          KndName = KndName.Trim

          Me.taKunden.FillByName1(DsPrinsTables.KUNDEN, KndName + "%")

          ' nächster suchversuch
          If DsPrinsTables.KUNDEN.Rows.Count = 0 Then
            Dim Old As String = KndName

            KndName = KndName.Replace("Architekturbüro", "")
            KndName = KndName.Replace("Gemeinde", "")
            KndName = KndName.Replace("Ingenieurbüro", "")
            KndName = KndName.Trim

            If Old <> KndName Then
              Me.taKunden.FillByName1(DsPrinsTables.KUNDEN, KndName + "%")
            End If
          End If

          ' keine Eindeutigkeit -> Name 1 und 2 suchen
          If DsPrinsTables.KUNDEN.Rows.Count > 1 AndAlso KndName2.Length > 0 Then
            Me.taKunden.FillByName1u2(DsPrinsTables.KUNDEN, KndName + "%", KndName2 + "%")
          End If

          ' keine Eindeutigkeit -> Name 1 und 2 suchen
          If DsPrinsTables.KUNDEN.Rows.Count > 1 AndAlso KndName2.Length > 0 Then
            Me.taKunden.FillByName1(DsPrinsTables.KUNDEN, KndName + " " + KndName2 + "%")
          End If


          ' nicht da oder keine Eindeutigkeit -> Name2 suchen
          If KndName2.Length > 3 AndAlso _
            (DsPrinsTables.KUNDEN.Rows.Count = 0 OrElse DsPrinsTables.KUNDEN.Rows.Count > 1) Then

            Me.taKunden.FillByName1(DsPrinsTables.KUNDEN, KndName2 + "%")
          End If

          ' wenn genau einer da ... 
          If DsPrinsTables.KUNDEN.Rows.Count = 1 Then

            ' Kunde AKTIV und PLZ muss stimmen
            If DsPrinsTables.KUNDEN.Rows(0)("AKTIV") = "1" AndAlso _
              Me.StringValue(DsPrinsTables.KUNDEN.Rows(0)("PLZ")) = PLZ Then

              fgExcel(r, "Firmennummer") = DsPrinsTables.KUNDEN.Rows(0)("NUMMER")

              tmp = StringValue(DsPrinsTables.KUNDEN.Rows(0)("NAME1")) + "," + _
                    StringValue(DsPrinsTables.KUNDEN.Rows(0)("NAME2")) + "," + _
                    StringValue(DsPrinsTables.KUNDEN.Rows(0)("ANSCHRIFT")) + "," + _
                    StringValue(DsPrinsTables.KUNDEN.Rows(0)("PLZ")) + "," + _
                    StringValue(DsPrinsTables.KUNDEN.Rows(0)("ORT"))

              fgExcel(r, "Kundendaten") = tmp
            End If

            ' fgExcel(r, "FName1") = DsPrinsTables.KUNDEN.Rows(0)("NAME1")
            'tbMsg.Text += fgExcel(r, "Firmennummer").ToString + ", "

            'DsPrinsTables.KUNDEN.Rows(0)("LETZTEAENDERUNGDATUM") = DateTime.Now
            'Me.taKunden.Update(DsPrinsTables.KUNDEN)
            'Else
            '  Dim tr As dsSQL_CRM_Prins.FirmaRow
            '  Dim idx As Integer = bsFirma.Find("Firmenname1", KndName)

            '  If idx = -1 Then
            '    tr = DsSQL_CRM_Prins.Firma.NewFirmaRow
            '    tr.Firmenname1 = KndName
            '    tr.Telefon = StringValue(fgExcel(r, "Telefon (geschäftlich)"), tr.Table.Columns("Telefon").MaxLength)
            '    tr.Fax = StringValue(fgExcel(r, "Fax"), tr.Table.Columns("Fax").MaxLength)
            '    tr.Anschrift = StringValue(fgExcel(r, "Adresse 1: Straße 1"), tr.Table.Columns("Anschrift").MaxLength)
            '    tr.Ort = StringValue(fgExcel(r, "Adresse 1: Ort"), tr.Table.Columns("Ort").MaxLength)
            '    tr.PLZ = StringValue(fgExcel(r, "Adresse 1: Postleitzahl"), tr.Table.Columns("PLZ").MaxLength)
            '    tr.Land = Land

            '    tmp = StringValue(fgExcel(r, "Besitzer")).Trim
            '    Do
            '      If tmp.StartsWith("Heck") Then tmp = "GH" : Exit Do
            '      If tmp.StartsWith("Gleim") Then tmp = "WG" : Exit Do
            '      If tmp.StartsWith("Schneider") Then tmp = "HJS" : Exit Do
            '      If tmp.StartsWith("Blothe") Then tmp = "CB" : Exit Do
            '      Exit Do
            '    Loop

            '    tr.Besitzer = tmp
            '    DsSQL_CRM_Prins.Firma.AddFirmaRow(tr)
            '    taFirma.Update(DsSQL_CRM_Prins.Firma)
            '    DsSQL_CRM_Prins.Firma.AcceptChanges()
            '  End If
          End If
        End If

        'Application.DoEvents()
        pb1.PerformStep() : inpTop.Update()


        'If (r >= 163) Then
        'Me.fgExcel.SaveExcel(System.IO.Path.GetDirectoryName(FullFileName) + "\" + System.IO.Path.GetFileNameWithoutExtension(FullFileName) + "-" + r.ToString + ".xlsx", C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells)
        'End If


      Next

    Finally
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
    End Try


    ' ------------------------------------------------------------------------------------------------------------------------
    Me.fgExcel.SaveExcel(FullFileName, C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells)

  End Sub

  Private Sub CheckTelFaxNumer(r As Integer, Land As String, SourceName As String, TargetName As String)
    Dim tmp As String

    If fgExcel.Cols.Contains(SourceName) AndAlso _
      fgExcel.Cols.Contains(TargetName) Then

      tmp = StringValue(fgExcel(r, SourceName)).Trim

      If tmp.IndexOf("(0)") > -1 Then tmp = tmp.Replace("(0)", "")
      If tmp.StartsWith("00") Then tmp = "+" + tmp.Substring(2)

      If Land.StartsWith("Deutsch") AndAlso tmp.StartsWith("0") Then
        tmp = "+49 " + tmp.Substring(1)
        fgExcel(r, SourceName) = tmp
      End If

      If tmp.Length > 0 AndAlso StringValue(fgExcel(r, TargetName)).Trim.Length = 0 Then
        fgExcel(r, TargetName) = tmp
      End If
    End If
  End Sub



  Private Sub fgExcel_Click(sender As Object, e As EventArgs)

  End Sub

  Private Sub btnSourceView_Click(sender As Object, e As EventArgs) Handles btnSourceView.Click

    Select Case VisibleFlex
      Case VisibleFlexEnum.Kunden
        VisibleFlex = VisibleFlexEnum.Projekte
      Case VisibleFlexEnum.Projekte
        VisibleFlex = VisibleFlexEnum.Adressen
      Case VisibleFlexEnum.Adressen
        VisibleFlex = VisibleFlexEnum.Excel
      Case VisibleFlexEnum.Excel
        VisibleFlex = VisibleFlexEnum.Kunden
    End Select


    Reload()

  End Sub

  Private Sub btnTagetView_Click(sender As Object, e As EventArgs) Handles btnTagetView.Click

    fgFirma.Visible = Not fgFirma.Visible
    fgProjekt.Visible = Not fgFirma.Visible

    If fgFirma.Visible Then
      ghTarget.Text = "CRM Firma"
    Else
      ghTarget.Text = "CRM Projekt"
    End If

    Me.Reload()

  End Sub

  Private Sub tmAutoExec_Tick(sender As Object, e As EventArgs) Handles tmAutoExec.Tick
    My.Application.RestartApplicationOnException = True
    tmAutoExec.Stop()

    AutoExecute()

    My.Application.RestartApplicationOnException = False
    tmAutoExec.Start()

  End Sub

  Private _DisplayMessages As ArrayList = Nothing

  Private ReadOnly Property DisplayMessages As ArrayList
    Get
      If IsNothing(_DisplayMessages) Then _DisplayMessages = New ArrayList
      Return _DisplayMessages
    End Get
  End Property

  Private Sub DisplayMessage([value] As String)
    Dim sb As New System.Text.StringBuilder

    DisplayMessages.Insert(0, Now.ToShortDateString + " " + Now.ToLongTimeString + " : " + value)

    If DisplayMessages.Count > 10 Then
      DisplayMessages.RemoveAt(DisplayMessages.Count - 1)
    End If

    For i As Integer = 0 To DisplayMessages.Count - 1
      sb.AppendLine(DisplayMessages(i))
    Next
    tbMsg.Text = sb.ToString
    inpTop.Update()
  End Sub


  Public Sub AutoExecute()
    Dim BtnEnabled As Boolean = ButtonsEnabled

    DisplayMessage("Starte Übertragung: " + Now.ToShortDateString + " " + Now.ToLongTimeString)

    ButtonsEnabled = False
    Application.DoEvents()
    System.Threading.Thread.Sleep(1000)
    ' -------------------------------------------------------------------------------------------
    DisplayMessage("Kunden -> CRM")
    btnKunden2CRM_Click(btnKunden2CRM, System.EventArgs.Empty)
    Application.DoEvents()
    ' -------------------------------------------------------------------------------------------
    ' KEINE ADRESSEN mehr an CRM zurückgeben. Führendes System = CRM
    ' (erst, wenn aus einer ADRESSE (Firma) ein KUNDE wird, wechselt das führende System
    '
    'DisplayMessage("Adressen -> CRM")
    'btnAdressen2CRM_Click(btnAdressen2CRM, System.EventArgs.Empty)
    'Application.DoEvents()
    ' -------------------------------------------------------------------------------------------
    DisplayMessage("Projekte -> CRM")
    btnPrinsProjekteCRM_Click(btnPrinsProjekteCRM, System.EventArgs.Empty)
    Application.DoEvents()
    ' -------------------------------------------------------------------------------------------
    DisplayMessage("CRM Firmen -> Prins.NET")
    btnCRM2Prins_Click(btnCRM2Prins, System.EventArgs.Empty)
    Application.DoEvents()
    ' -------------------------------------------------------------------------------------------
    DisplayMessage("CRM Projekte -> Prins.NET")
    btnCRMProjekte2Prins_Click(btnCRM2Prins, System.EventArgs.Empty)
    Application.DoEvents()
    ' -------------------------------------------------------------------------------------------
    DisplayMessage("Projekte -> CRM")
    btnPrinsProjekteCRM_Click(btnPrinsProjekteCRM, System.EventArgs.Empty)
    Application.DoEvents()
    ' -------------------------------------------------------------------------------------------
    ButtonsEnabled = BtnEnabled

    DisplayMessage("Erfolgreich abgeschlossen: " + Now.ToShortDateString + " " + Now.ToLongTimeString)
  End Sub


  Private Sub btnExecAll_Click(sender As Object, e As EventArgs) Handles btnExecAll.Click
    AutoExecute()
  End Sub

  Private Sub btnAutoExec_PressedChanged(sender As Object, e As EventArgs) Handles btnAutoExec.PressedChanged
    My.Settings.btnAutoExecPressed = btnAutoExec.Pressed
    tmAutoExec.Enabled = btnAutoExec.Pressed
    ' -------------------------------------------------------------------------------------------
    ButtonsEnabled = Not btnAutoExec.Pressed
  End Sub

  Private Sub btnCRMProjekte2Prins_Click(sender As Object, e As EventArgs) Handles btnCRMProjekte2Prins.Click
    Dim Source As New DataView(DsSQL_CRM_Prins.Projekt)
    Me.taProjekt.Fill(DsSQL_CRM_Prins.Projekt)

    Source.RowFilter = "Zielsystem=2" ' Prins

    For i As Integer = Source.Count - 1 To 0 Step -1
      Dim p As dsSQL_CRM_Prins.ProjektRow = Source(i).Row

      If p.IsFirmenIDNull Then
        taPRC_AN_PRJ_ANLAGE.Fill(DsPrinsTables.PRC_AN_PRJ_ANLAGE, Nothing, p.FirmenID, p.Bezeichnung, p.Besitzer, p.Projekt_ID)
      Else
        taPRC_AN_PRJ_ANLAGE.Fill(DsPrinsTables.PRC_AN_PRJ_ANLAGE, p.FirmenID, p.FirmenID, p.Bezeichnung, p.Besitzer, p.Projekt_ID)
      End If

      ' bei Rückgabewert=-1 -> Fehler!
      If DsPrinsTables.PRC_AN_PRJ_ANLAGE.Rows.Count = 1 AndAlso DsPrinsTables.PRC_AN_PRJ_ANLAGE.Rows(0)("PRJ_NUMMER") > 0 Then
        p.Delete() : Me.taProjekt.Update(DsSQL_CRM_Prins.Projekt)
      End If

    Next

  End Sub

  Private Sub btnExcel2Firma_Click(sender As Object, e As EventArgs) Handles btnExcel2Firma.Click


    Try
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Visible
      pb1.Value = 0
      pb1.Maximum = fgExcel.Rows.Count
      '------------------------------------------------------------------------------------

      For r As Integer = fgExcel.Rows.Fixed To fgExcel.Rows.Count - 1
        Dim KndNr As String = StringValue(fgExcel(r, "Firmennummer")).ToString.Trim
        Dim tr As dsSQL_CRM_Prins.FirmaRow
        Dim idx As Integer

        If Not String.IsNullOrEmpty(KndNr) Then
          idx = bsFirma.Find("Firmennummer", KndNr)
        Else
          idx = bsFirma.Find("Firmenname1", StringValue(fgExcel(r, "Firmenname")))
        End If

        If idx > -1 Then tr = bsFirma(idx).Row Else tr = DsSQL_CRM_Prins.Firma.NewFirmaRow
        tr.BeginEdit()


        tr.Besitzer = "GW" 'StringValue(fgExcel(r, "Besitzer"))
        tr.Zielsystem = 1
        tr.Firmennummer = KndNr
        tr.Firmenname1 = StringValue(fgExcel(r, "Firmenname"))
        tr.Firmenname2 = StringValue(fgExcel(r, "Firmenname2"))
        tr.Firmenname3 = StringValue(fgExcel(r, "Firmenname3"))
        tr.Anschrift = StringValue(fgExcel(r, "Adresse 1: Straße 1"))
        tr.PLZ = StringValue(fgExcel(r, "Adresse 1: Postleitzahl"))
        tr.Ort = StringValue(fgExcel(r, "Adresse 1: Ort"))
        tr.Land = StringValue(fgExcel(r, "Adresse 1: Land/Region"))
        tr.Telefon = StringValue(fgExcel(r, "Adresse 1: Telefon"))
        tr.Fax = StringValue(fgExcel(r, "Fax"))
        tr.Homepage = StringValue(fgExcel(r, "Website"))

        If tr.RowState = DataRowState.Detached Then
          DsSQL_CRM_Prins.Firma.AddFirmaRow(tr)
        Else
          tr.EndEdit()
        End If

        'Application.DoEvents()
        pb1.PerformStep() : inpTop.Update()
      Next

    Finally
      pb1.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
    End Try


  End Sub
End Class
