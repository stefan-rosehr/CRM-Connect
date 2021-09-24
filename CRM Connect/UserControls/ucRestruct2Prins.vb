Public Class ucRestruct2Prins


  Private _CanCancel As Boolean = False
  Private Property CanCancel As Boolean
    Get
      Return _CanCancel
    End Get
    Set(value As Boolean)
      _CanCancel = value

      ibUpdateProjekte.Enabled = Not _CanCancel
      ibWBImport.Enabled = Not _CanCancel
      ibCancel.Enabled = _CanCancel
      _CancelImport = False
    End Set

  End Property



  Public Enum RestructProjektStatusEnum
    Beauftragt = 2
    Abgeschlossen = 8
    Akquisition = 9
    unterbrochen = 11
    KeinAuftrag = 13
    AktivOhneBerechnung = 14
  End Enum

  Private Sub ibImportFromRestruct_Click(sender As Object, e As EventArgs)
    CheckUpdateProjekte()
    ImportWochenberichte()
  End Sub

  Private Sub ibUpdateProjekte_Click(sender As Object, e As EventArgs) Handles ibUpdateProjekte.Click
    CheckUpdateProjekte()
  End Sub


  Private Sub ibWBImport_Click(sender As Object, e As EventArgs) Handles ibWBImport.Click
    ImportWochenberichte()
  End Sub

  Private CurrentFlexRow As Integer

  Private _CancelImport As Boolean = False

  Private Sub ImportWochenberichte()
    Dim Von, Bis As DateTime
    Dim tblvWochenbericht As dsRestruct.vWochenberichtDataTable = DsRestruct.vWochenbericht
    Dim AddMonth As Integer = -1
    Dim frmVonBis As New frmVonBisDatum

    If Today.Day > 15 Then AddMonth = 0
    Von = New Date(Today.Year, Today.Month, 1).AddMonths(AddMonth)
    Bis = Von.AddMonths(1).AddDays(-1)

    frmVonBis.VonDatum = Von
    frmVonBis.BisDatum = Bis
    frmVonBis.PrjNummer = 0

    If frmVonBis.ShowDialog = DialogResult.Cancel Then Exit Sub
    ' ----------------------------------------------------------------
    Von = frmVonBis.VonDatum
    Bis = frmVonBis.BisDatum
    ' ----------------------------------------------------------------
    If frmVonBis.PrjNummer > 0 Then
      taVWochenbericht.FillByDatumPrj(tblvWochenbericht, Von, Bis, frmVonBis.PrjNummer.ToString + "%")
    Else
      taVWochenbericht.FillByDatum(tblvWochenbericht, Von, Bis)
    End If
    ' ----------------------------------------------------------------
    Debug.WriteLine("tblvWochenbericht : " + tblvWochenbericht.Rows.Count.ToString + " Records")

    ' Monatsdaten nur mit RESTRUCT_ID
    DsPrins.WOCHENBERICHT.TableAdapter.FillByDatumRestruct(DsPrins.WOCHENBERICHT, Von, Bis)

    flex.Rows.Count = flex.Rows.Fixed
    CurrentFlexRow = flex.Rows.Fixed
    c1Progress.Value = 0
    c1Progress.Visibility = C1.Win.C1InputPanel.Visibility.Visible
    c1Progress.Minimum = 0
    c1Progress.Maximum = tblvWochenbericht.Rows.Count
    c1Progress.Step = 1
    CanCancel = True

    Try
      For Each WbRow As dsRestruct.vWochenberichtRow In tblvWochenbericht.Rows
        ImportWBRow(WbRow)
        c1Progress.PerformStep()
        ipTop.Update()
        Application.DoEvents()

        If _CancelImport Then
          If MsgBox("Übertragung wirklich abbrechen?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
            Exit For
          End If
          _CancelImport = False
        End If
      Next
    Finally
      _CancelImport = False
      c1Progress.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
      CanCancel = False
    End Try

    Debug.WriteLine("WOCHENBERICHT : " + DsPrins.WOCHENBERICHT.Rows.Count.ToString + " Records")

  End Sub

  Private _NotFoundMA As ArrayList = Nothing
  Private ReadOnly Property NotFoundMA As ArrayList
    Get
      If IsNothing(_NotFoundMA) Then _NotFoundMA = New ArrayList
      Return _NotFoundMA
    End Get
  End Property

  Private _vWochenberichtByRestructID As DataView
  Private ReadOnly Property vWochenberichtByRestructID As DataView
    Get
      If IsNothing(_vWochenberichtByRestructID) Then
        _vWochenberichtByRestructID = New DataView(DsPrins.WOCHENBERICHT)
        _vWochenberichtByRestructID.Sort = "RESTRUCT_ID "
      End If
      Return _vWochenberichtByRestructID
    End Get
  End Property


  Private Function ImportWBRow(vWRow As dsRestruct.vWochenberichtRow) As Boolean
    Dim Ok As Boolean = False

    Do
      Dim dt As dsRestruct.vWochenberichtDataTable = DsRestruct.vWochenbericht
      ' --------------------------------------------------------------
      Dim WbRow As dsPrins.WOCHENBERICHTRow = Nothing
      Dim PersRow As dsPrins.PERSONALRow = GetPERSONALRowByVNName(vWRow.cVorname, vWRow.cName)
      Dim PrjRow As dsPrins.PROJEKTRow = Nothing
      Dim PrjPhase As Integer = -1
      Dim SubPrjNr As Integer
      Dim Idx As Integer = vWochenberichtByRestructID.Find(vWRow.ID)
      Dim InternesProjekt As Boolean = False
      ' --------------------------------------------------------------
      If vWRow.IsfDauerNull OrElse vWRow.fDauer = 0 Then Exit Do
      ' --------------------------------------------------------------
      If Idx = -1 Then
        If IsNothing(PersRow) Then
          If Not NotFoundMA.Contains(vWRow.cVorname + " " + vWRow.cName) Then
            NotFoundMA.Add(vWRow.cVorname + " " + vWRow.cName)
            Debug.WriteLine("Nicht gefunden:" + vWRow.cVorname + " " + vWRow.cName)
            MsgBox("Mitarbeiter nicht gefunden:" + vWRow.cVorname + " " + vWRow.cName, MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)
          End If
        Else

          If Not vWRow.IscNrNull Then ' Projektnummer
            Dim KPrjNr As String
            Dim sp As String() = vWRow.cNr.Split("-")
            ' ---------------------------------------------------------------
            If sp.Length > 1 Then
              KPrjNr = sp(0)
              SubPrjNr = My.Application.DBInteger(sp(1))
            Else
              KPrjNr = vWRow.cNr
            End If
            ' ---------------------------------------------------------------
            Select Case My.Application.DBInteger(KPrjNr)
              Case 18000, 19000 ' Allgem. Arbeiten #4
                DsPrins.PROJEKT.TableAdapter.FillByNummer(DsPrins.PROJEKT, 4)
                InternesProjekt = True
              Case 18002, 19002 ' Fortbildung #6
                DsPrins.PROJEKT.TableAdapter.FillByNummer(DsPrins.PROJEKT, 6)
                InternesProjekt = True
              Case Else
                DsPrins.PROJEKT.TableAdapter.FillByKndPrjNr(DsPrins.PROJEKT, KPrjNr)
            End Select
            ' ---------------------------------------------------------------
            If DsPrins.PROJEKT.Rows.Count = 0 Then
              taVProjekt_Kunde.FillByCNr(DsRestruct.vProjekt_Kunde, KPrjNr)
              If DsRestruct.vProjekt_Kunde.Rows.Count > 0 Then
                PrjRow = CheckAll(DsRestruct.vProjekt_Kunde.Rows(0))

                ' wird von CheckAll verwendet und steh nicht mehr zur Verfügung
                PersRow = GetPERSONALRowByVNName(vWRow.cVorname, vWRow.cName)
              Else
                ' Satz muss da sein, sonst EXIT
                Throw New Exception(KPrjNr + " in Restruct-DB nicht gefunden!")
              End If

            Else
              PrjRow = DsPrins.PROJEKT.Rows(0)
              CheckCreatePrjStandort(PrjRow.NUMMER, Nothing)
              Debug.WriteLine("Gefunden:" + KPrjNr)
            End If
            ' ---------------------------------------------------------------
            PrjPhase = GetPrjPhaseID(PrjRow, SubPrjNr, vWRow.cBeschreibung)
          End If
          ' ---------------------------------------------------------------
          WbRow = DsPrins.WOCHENBERICHT.NewWOCHENBERICHTRow

          If Not IsNothing(PrjRow) Then
            WbRow.PROJEKTNUMMER = PrjRow.NUMMER
            WbRow.STANDORT = 194 ' statisch 
          ElseIf Not IsNothing(vWRow.cTyp) Then
            InternesProjekt = True
            Select Case vWRow.cTyp
              Case "U"
                WbRow.PROJEKTNUMMER = 1
                WbRow.STANDORT = 84
                WbRow.BTEXT = "Urlaub"
                WbRow.MERKMAL = "E01"
                Exit Do ' kein Urlaub mehr über Restruct
              Case "K"
                WbRow.PROJEKTNUMMER = 2
                WbRow.STANDORT = 84
                WbRow.BTEXT = "Krank"
                WbRow.MERKMAL = "E02"
                Exit Do ' kein krank mehr über Restruct
              Case "B"
                WbRow.PROJEKTNUMMER = 6
                WbRow.STANDORT = 84
                WbRow.BTEXT = "Berufsschule"
                WbRow.MERKMAL = "F01"
              Case Else
                WbRow.PROJEKTNUMMER = 4
                WbRow.STANDORT = 84
                WbRow.BTEXT = "Allgemeine Arbeiten"
                WbRow.MERKMAL = "E04"
            End Select
          Else
            Exit Do
          End If
          ' ---------------------------------------------------------------
          WbRow.DATUM = vWRow.dDatum
          WbRow.PERSONALNR = PersRow.NUMMER
          WbRow.MENGE = vWRow.fDauer
          If Not vWRow.IscTextNull Then
            WbRow.BTEXT = vWRow.cText
          ElseIf WbRow.IsBTEXTNull OrElse String.IsNullOrEmpty(WbRow.BTEXT) Then
            WbRow.BTEXT = "Abstimmung"
          End If
          If PrjPhase > -1 Then WbRow.PRJ_PHASE = PrjPhase
          WbRow.VERBUCHT = "0"
          WbRow.FREIGEGEBEN = "0"
          WbRow.SELEKTIERT = "0"
          If (Not IsNothing(PrjRow) AndAlso PrjRow.INTERN = "1") OrElse InternesProjekt OrElse SubPrjNr = 0 OrElse (Not vWRow.bAbrechnen) OrElse (Not vWRow.IsbOhneBerechnungNull AndAlso vWRow.bOhneBerechnung) Then
            WbRow.ABRECHENBAR = "0"
            If WbRow.IsMERKMALNull Then

              If WbRow.PROJEKTNUMMER = 6 Then
                WbRow.MERKMAL = "E06"

              ElseIf SubPrjNr = 0 Then
                WbRow.MERKMAL = "D09"
              Else
                WbRow.MERKMAL = "E04"
              End If
            End If
          Else
            WbRow.ABRECHENBAR = "1"
          End If

          WbRow.ABGERECHNET = "0"
          WbRow.RESTRUCT_ID = vWRow.ID
          WbRow.TBL_HOMEOFFICE_GRUND_ID = vWRow.TBL_HOMEOFFICE_GRUND_ID

          If WbRow.IsMERKMALNull Then WbRow.MERKMAL = "A01"
          WbRow.LETZTEAENDERUNGDATUM = Now
          WbRow.LETZTEAENDERUNGUSER = "RESTRUCT"
          DsPrins.WOCHENBERICHT.AddWOCHENBERICHTRow(WbRow)
          DsPrins.WOCHENBERICHT.SaveChanges()
        End If

      Else ' WB-ID ist bereits vorhanden
        Dim SaveChanges As Boolean = False
        WbRow = vWochenberichtByRestructID(Idx).Row

        CheckCreatePrjStandort(WbRow.PROJEKTNUMMER, Nothing)

        If WbRow.MENGE <> vWRow.fDauer Then
          Debug.WriteLine("WbRow.MENGE <> vWRow.fDauer ")
          WbRow.MENGE = vWRow.fDauer
          SaveChanges = True
        End If

        If Not vWRow.IscTextNull Then
          If WbRow.BTEXT.IndexOf(vWRow.cText) = -1 Then
            WbRow.BTEXT += ControlChars.CrLf + vWRow.cText
            SaveChanges = True
          End If

          If SaveChanges Then DsPrins.WOCHENBERICHT.SaveChanges()
        End If
      End If
      ' ---------------------------------------------------------------
      Try
        flex.Rows.Count = CurrentFlexRow + 1
        flex(CurrentFlexRow, 1) = If(WbRow.HasErrors, WbRow.RowError, "Ok")
        flex(CurrentFlexRow, 2) = WbRow.RESTRUCT_ID
        flex(CurrentFlexRow, 3) = vWRow.cVorname + " " + vWRow.cName
        flex(CurrentFlexRow, 4) = WbRow.DATUM
        flex(CurrentFlexRow, 5) = CDec(WbRow.MENGE)
        flex(CurrentFlexRow, 6) = WbRow.PROJEKTNUMMER
        flex(CurrentFlexRow, 7) = WbRow.BTEXT
        flex.Update()
      Catch ex As Exception
        flex(CurrentFlexRow, 1) = ex.Message
      End Try
      flex.Row = CurrentFlexRow
      CurrentFlexRow += 1

      ' ---------------------------------------------------------------
      Exit Do
    Loop

    Return Ok
  End Function




  Private Sub CheckUpdateProjekte()
    Dim tblvProjektKunde As dsRestruct.vProjekt_KundeDataTable = DsRestruct.vProjekt_Kunde

    taVProjekt_Kunde.FillByStatus(tblvProjektKunde, RestructProjektStatusEnum.Beauftragt)
    For Each KPRow As dsRestruct.vProjekt_KundeRow In tblvProjektKunde.Rows
      CheckAll(KPRow)
    Next

  End Sub


  Private Function CheckAll(KPRow As dsRestruct.vProjekt_KundeRow) As dsPrins.PROJEKTRow
    Dim PrjRow As dsPrins.PROJEKTRow

    PrjRow = CheckCreateProjekt(KPRow)
    Debug.WriteLine("Projekt: " + PrjRow.NUMMER.ToString + " / " + PrjRow.SUCHKURZNAME)
    ' ---------------------------------------------------
    Dim sp As String() = KPRow.cNrSub.Split("-")
    Dim SubId As Integer = -1

    If sp.Length > 1 Then
      SubId = CInt(sp(1))
      GetPrjPhaseID(PrjRow, SubId, KPRow.cBeschreibungSub)
    Else
      Debug.WriteLine(KPRow.cNrSub + " ?")
    End If

    Return PrjRow
  End Function


  Private Function GetPrjPhaseID(PrjRow As dsPrins.PROJEKTRow, SubPrjNr As Integer, Bezeichnung As String) As Integer
    Dim Result As Integer = -1

    DsPrins.PRJ_LEISTUNGSPHASE.TableAdapter.FillByPrjNrPhase(DsPrins.PRJ_LEISTUNGSPHASE, PrjRow.NUMMER, SubPrjNr)
    If DsPrins.PRJ_LEISTUNGSPHASE.Rows.Count = 0 Then
      Dim Row As dsPrins.PRJ_LEISTUNGSPHASERow = DsPrins.PRJ_LEISTUNGSPHASE.NewPRJ_LEISTUNGSPHASERow

      Row.PHASE = SubPrjNr
      Row.PRJ_NUMMER = PrjRow.NUMMER
      Row.NAME = MaxString(Bezeichnung, DsPrins.PRJ_LEISTUNGSPHASE.NAMEColumn.MaxLength)
      Row.AKTIV = True
      DsPrins.PRJ_LEISTUNGSPHASE.AddPRJ_LEISTUNGSPHASERow(Row)
      DsPrins.PRJ_LEISTUNGSPHASE.SaveChanges()

      Result = Row.ID
    Else
      Result = DsPrins.PRJ_LEISTUNGSPHASE.Rows(0)("ID")
    End If

    Return Result
  End Function


  Private Function CheckCreateProjekt(KPRow As dsRestruct.vProjekt_KundeRow) As dsPrins.PROJEKTRow
    Dim PrjRow As dsPrins.PROJEKTRow

    DsPrins.PROJEKT.TableAdapter.FillByKndPrjNr(DsPrins.PROJEKT, KPRow.cNr)
    ' ---------------------------------------------------------------
    ' nicht gefunden
    If DsPrins.PROJEKT.Rows.Count = 0 Then
      Dim KndRow As dsPrins.KUNDENRow = CheckCreateKunde(KPRow)

      PrjRow = DsPrins.PROJEKT.NewPROJEKTRow
      PrjRow.NUMMER = DsPrins.Generator.GetGeneratorValue("PROJEKT_GEN")

      PrjRow.SUCHKURZNAME = KPRow.cNr
      PrjRow.NAME1 = MaxString(KPRow.cNr + ": " + KPRow.cFirma, DsPrins.PROJEKT.NAME1Column.MaxLength)
      PrjRow.KURZNAME = MaxString(PrjRow.NAME1, DsPrins.PROJEKT.KURZNAMEColumn.MaxLength)
      PrjRow.NAME2 = MaxString(KPRow.cBeschreibung, DsPrins.PROJEKT.NAME2Column.MaxLength)
      PrjRow.KUNDENNUMMER = KndRow.NUMMER
      PrjRow.STATUS = 2
      PrjRow.ANSCHRIFT = KPRow._cStr
      PrjRow.LANDSAP = KndRow.LANDSAP
      PrjRow.PLZ = KPRow.cPLZ
      PrjRow.ORT = KPRow.cOrt
      PrjRow.INTERN = "0"
      PrjRow.GESPERRT = "0"
      PrjRow.INVISIBLE = "0"
      PrjRow.CLOSED_LOCK = "0"
      PrjRow.VLG_VERTRIEB = "0"
      PrjRow.AUTO_AVOL = "0"
      PrjRow.ISBP = "0"
      PrjRow.KGRUPPE = "RESTRUCT"
      PrjRow.ANLAGEDATUM = Today
      PrjRow.AUFTRAGSDATUM = Today
      PrjRow.LETZTEAENDERUNGDATUM = Now
      PrjRow.LETZTEAENDERUNGUSER = "RESTRUCT"
      DsPrins.PROJEKT.AddPROJEKTRow(PrjRow)
      DsPrins.PROJEKT.SaveChanges()
      ' ----------------------------------------------------------------------------
      CheckCreatePrjStandort(PrjRow.NUMMER, KPRow)
      ' ----------------------------------------------------------------------------
    Else
      Dim tmp As String
      Dim RowChanged As Boolean = False

      PrjRow = DsPrins.PROJEKT.Rows(0)

      tmp = MaxString(KPRow.cNr + ": " + KPRow.cFirma, DsPrins.PROJEKT.NAME1Column.MaxLength)
      RowChanged = My.Application.SetRowValueIfChanged(PrjRow, "NAME1", tmp)

      tmp = MaxString(PrjRow.NAME1, DsPrins.PROJEKT.KURZNAMEColumn.MaxLength)
      RowChanged = RowChanged Or My.Application.SetRowValueIfChanged(PrjRow, "KURZNAME", tmp)

      tmp = MaxString(KPRow.cBeschreibung, DsPrins.PROJEKT.NAME2Column.MaxLength)
      RowChanged = RowChanged Or My.Application.SetRowValueIfChanged(PrjRow, "NAME2", tmp)

      If RowChanged Then DsPrins.PROJEKT.SaveChanges()
      ' ----------------------------------------------------------------------------
      CheckCreatePrjStandort(PrjRow.NUMMER, KPRow)
      ' ----------------------------------------------------------------------------
    End If
    Return PrjRow
  End Function

  Private Function CheckCreatePrjStandort(PrjNummer As Integer, KPRow As dsRestruct.vProjekt_KundeRow) As dsPrins.PROJEKT_STANDORTRow
    Dim PstRow As dsPrins.PROJEKT_STANDORTRow
    Dim PersRow As dsPrins.PERSONALRow = Nothing
    Dim Kst As Integer = If(PrjNummer < 10, 7100, 3611)

    If Not IsNothing(KPRow) Then PersRow = GetPERSONALRowByVNName(KPRow)

    DsPrins.PROJEKT_STANDORT.TableAdapter.FillByPrjKst(DsPrins.PROJEKT_STANDORT, PrjNummer, Kst)
    If DsPrins.PROJEKT_STANDORT.Rows.Count > 0 Then
      PstRow = DsPrins.PROJEKT_STANDORT.Rows(0)
    Else
      PstRow = DsPrins.PROJEKT_STANDORT.NewPROJEKT_STANDORTRow
      PstRow.PROJEKTNUMMER = PrjNummer
      PstRow.KOSTENSTELLE = Kst
      PstRow.AKTIV = "1"
      PstRow.VERTRIEB = "0"
      PstRow.PROJEKT_SPEZIFISCH = "0"
      PstRow.AUTO_DEG = "0"
      PstRow.DEG_GJAHR = "0"
      PstRow.NUMMER = 0 ' Wird vom TRIGGER eingetragen

      If PstRow.IsUNTERGEORDNETNull Then PstRow.UNTERGEORDNET = "0"
      If Not IsNothing(PersRow) AndAlso PstRow.IsNRBEARBEITERNull Then
        PstRow.NRBEARBEITER = PersRow.NUMMER
      End If

      If PstRow.RowState = DataRowState.Detached Then
        DsPrins.PROJEKT_STANDORT.AddPROJEKT_STANDORTRow(PstRow)
      End If
      DsPrins.PROJEKT_STANDORT.SaveChanges()
    End If

    Return PstRow
  End Function


  Private Function CheckCreateKunde(KPRow As dsRestruct.vProjekt_KundeRow) As dsPrins.KUNDENRow
    Dim KndRow As dsPrins.KUNDENRow = Nothing

    DsPrins.KUNDEN.TableAdapter.FillByCKNr(DsPrins.KUNDEN, KPRow.cKNr)
    If DsPrins.KUNDEN.Rows.Count = 0 Then

      If Not KPRow.IscFirmaNull AndAlso Not KPRow.Is_cStrNull AndAlso
        Not KPRow.IscPLZNull AndAlso Not KPRow.IscOrtNull Then

        DsPrins.KUNDEN.TableAdapter.FillByFirmaStrOrt(DsPrins.KUNDEN, KPRow.cFirma, KPRow._cStr, KPRow.cPLZ, KPRow.cOrt)
        If DsPrins.KUNDEN.Rows.Count > 0 Then
          KndRow = DsPrins.KUNDEN.Rows(0)
          KndRow.CKNR = KPRow.cKNr
          DsPrins.KUNDEN.SaveChanges()
        End If
      End If
    Else
      KndRow = DsPrins.KUNDEN.Rows(0)
    End If
    ' -----------------------------------------------------------------
    If IsNothing(KndRow) Then
      KndRow = DsPrins.KUNDEN.NewKUNDENRow

      KndRow.NAME1 = KPRow.cFirma
      KndRow.ANSCHRIFT = KPRow._cStr

      KndRow.LANDSAP = "DE"

      KndRow.PLZ = KPRow.cPLZ
      KndRow.ORT = KPRow.cOrt
      KndRow.AKTIV = "1"
      KndRow.SAP_BUCHKREIS = 5090
      KndRow.VERTRIEB = "0"
      KndRow.AUTO_AVOL = "0"
      KndRow.AUTO_DEG = "0"
      KndRow.DEG_GJAHR = "0"
      KndRow.SAGEEXPORT = "1"
      KndRow.CKNR = KPRow.cKNr

      DsPrins.KUNDEN.AddKUNDENRow(KndRow)
      DsPrins.KUNDEN.SaveChanges()
    End If

    Return KndRow
  End Function

  Public ReadOnly Property MaxString(ByVal Field As Object, ByVal MaxLength As Integer) As String
    Get
      Dim tmp As String = Me.DBString(Field)
      Return tmp.Substring(0, Math.Min(tmp.Length, MaxLength))
    End Get
  End Property

  Public ReadOnly Property DBString(ByVal Field As Object) As String
    Get
      Dim Value As String = String.Empty
      Try
        If Not IsNothing(Field) AndAlso Not IsDBNull(Field) Then
          Value = CType(Field, String)
        End If
      Catch
        Value = String.Empty
      End Try
      Return Value
    End Get
  End Property

  Private Function GetPERSONALRowByVNName(KPRow As dsRestruct.vProjekt_KundeRow) As dsPrins.PERSONALRow
    Dim PersRow As dsPrins.PERSONALRow = Nothing

    If Not KPRow.IscVornameNull AndAlso Not KPRow.IscNameNull Then
      PersRow = GetPERSONALRowByVNName(KPRow.cVorname, KPRow.cName)
    End If

    Return PersRow
  End Function


  Private Function GetPERSONALRowByVNName(VName As String, NName As String) As dsPrins.PERSONALRow
    Dim Result As dsPrins.PERSONALRow = Nothing

    DsPrins.PERSONAL.TableAdapter.FillByVNName(DsPrins.PERSONAL, VName, NName)
    If DsPrins.PERSONAL.Rows.Count > 0 Then
      Result = DsPrins.PERSONAL.Rows(0)
    End If

    Return Result
  End Function

  Private Sub ucRestruct2Prins_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    If Not IsNothing(ParentForm) Then

      AddHandler ParentForm.FormClosing, AddressOf ParentForm_FormClosing
      AddHandler ParentForm.FormClosed, AddressOf ParentForm_FormClosed
    End If
  End Sub

  Private Sub ParentForm_FormClosed(sender As Object, e As FormClosedEventArgs)
    RemoveHandler ParentForm.FormClosing, AddressOf ParentForm_FormClosing
  End Sub

  Private Sub ParentForm_FormClosing(sender As Object, e As FormClosingEventArgs)
    e.Cancel = CanCancel
  End Sub

  Private Sub ibCancel_Click(sender As Object, e As EventArgs) Handles ibCancel.Click
    _CancelImport = True
  End Sub

End Class
