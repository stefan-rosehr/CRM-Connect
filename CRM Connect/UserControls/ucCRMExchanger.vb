'Imports Newtonsoft.Json
Imports Microsoft.Office.Interop
Imports System.DirectoryServices
Imports System.Net
Imports System.Net.Http

Imports hpcFOExchange
Imports hpcFOExchange.clFOJournal


Public Class ucCRMExchanger

  Private ReadOnly SyncContext As Threading.SynchronizationContext = Threading.SynchronizationContext.Current

  Private _InvoiceQueue As Queue
  Private ReadOnly Property InvoiceQueue As Queue
    Get
      If IsNothing(_InvoiceQueue) Then
        _InvoiceQueue = New Queue
      End If
      Return _InvoiceQueue
    End Get
  End Property

  Private _FOInvoiceBooking As clInvoiceBooking
  Public ReadOnly Property FOInvoiceBooking As clInvoiceBooking
    Get
      If IsNothing(_FOInvoiceBooking) Then
        _FOInvoiceBooking = New clInvoiceBooking(My.Settings.PrinsDEConnection)
      End If
      Return _FOInvoiceBooking
    End Get
  End Property

  Private _FOJournal As clFOJournal
  Public ReadOnly Property FOJournal As clFOJournal
    Get
      If IsNothing(_FOJournal) Then
        _FOJournal = New clFOJournal(My.Settings.PrinsDEConnection)
      End If
      Return _FOJournal
    End Get
  End Property


  Private _FOErrors As clFOJournal
  Public ReadOnly Property FOErrors As clFOJournal
    Get
      If IsNothing(_FOErrors) Then
        _FOErrors = New clFOJournal(My.Settings.PrinsDEConnection)
      End If
      Return _FOErrors
    End Get
  End Property

  Private _bwDsPrins As dsPrins
  Private ReadOnly Property bwDsPrins As dsPrins
    Get
      If IsNothing(_bwDsPrins) Then
        _bwDsPrins = New dsPrins
      End If
      Return _bwDsPrins
    End Get
  End Property


  Private _FOSuccess As clFOJournal
  Public ReadOnly Property FOSuccess As clFOJournal
    Get
      If IsNothing(_FOSuccess) Then
        _FOSuccess = New clFOJournal(My.Settings.PrinsDEConnection)
      End If
      Return _FOSuccess
    End Get
  End Property

  Public Sub New()
    ' Dieser Aufruf ist für den Designer erforderlich.
    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    ibAzureBusQ.Tag = "Azure Service Bus Überwachung stoppen"
    ibAzureBusQ.Text = "Azure Service Bus Überwachung starten"

    AddHandler DsTransfer.dtTransfer.RowChanged, AddressOf dtTransferDataTable_RowChanged
    AddHandler DsTransfer.dtTransfer.RowDeleted, AddressOf dtTransferDataTable_RowDeleted

    Dim ht As New Hashtable

    ht.Add(1, "Prins -> CRM")
    ht.Add(2, "CRM -> Prins")

    flex.Cols("Direction").DataMap = ht
    'ibADRead.Enabled = Debugger.IsAttached

    hpcFOExchange.clBase.Initialize(My.Settings.PrinsDEConnection)
  End Sub


  Private _ButtonsEnabled As Boolean = True
  Private Property ButtonsEnabled As Boolean
    Set(value As Boolean)
      ibStartTransfer.Enabled = value
      ibADRead.Enabled = value
      _ButtonsEnabled = value
      inpTop.Update()
    End Set
    Get
      Return _ButtonsEnabled
    End Get
  End Property


  Private _vwKOSTENSTELLENByNummer As DataView
  Private ReadOnly Property vwKOSTENSTELLENByNummer As DataView
    Get
      If IsNothing(_vwKOSTENSTELLENByNummer) Then
        _vwKOSTENSTELLENByNummer = New DataView(DsPrins1.KOSTENSTELLEN)
        _vwKOSTENSTELLENByNummer.Sort = "NUMMER"
        ' laden
        If DsPrins1.KOSTENSTELLEN.Rows.Count = 0 Then DsPrins1.KOSTENSTELLEN.TableAdapter.Fill(DsPrins1.KOSTENSTELLEN)
      End If
      Return _vwKOSTENSTELLENByNummer
    End Get
  End Property

  Private ReadOnly Property KostenstellenRow(KstNummer As Integer) As dsPrins.KOSTENSTELLENRow
    Get
      Dim Result As dsPrins.KOSTENSTELLENRow = Nothing
      Dim Idx As Integer = vwKOSTENSTELLENByNummer.Find(KstNummer)

      If Idx > -1 Then
        Result = vwKOSTENSTELLENByNummer(Idx).Row
      End If

      Return Result
    End Get
  End Property

  Private Sub DisplayMessage([value] As String)
    Dim LogRow As dsLog.TBL_LOGRow = DsLog.TBL_LOG.NewTBL_LOGRow
    My.Settings.fgMessagesCounter += 1

    LogRow.MESSAGE = value
    LogRow.LOG_TS = Now
    DsLog.TBL_LOG.AddTBL_LOGRow(LogRow)

    fgMessages.AutoSizeCol(1)
    SaveMessages()
    Application.DoEvents()

  End Sub

  Private ReadOnly Property CRMFolderPath As String
    Get
      Dim Result As String = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)

      Result += System.IO.Path.DirectorySeparatorChar + My.Application.Info.ProductName
      If Not System.IO.Directory.Exists(Result) Then
        System.IO.Directory.CreateDirectory(Result)
      End If
      Return Result
    End Get
  End Property

  Private ReadOnly Property PrinsCRMFileName As String
    Get
      Dim Result As String = CRMFolderPath
      Result += System.IO.Path.DirectorySeparatorChar + DsTransfer.DataSetName + ".xml"
      Return Result
    End Get
  End Property

  Private _DsPrinsNET1 As hpcFOExchange.dsPrinsNET
  Private ReadOnly Property DsPrinsNET1 As hpcFOExchange.dsPrinsNET
    Get
      If IsNothing(_DsPrinsNET1) Then
        _DsPrinsNET1 = New hpcFOExchange.dsPrinsNET
      End If
      Return _DsPrinsNET1
    End Get
  End Property

  'Private ReadOnly Property PrinsNETFileName As String
  '  Get
  '    Dim Result As String = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)

  '    Result += System.IO.Path.DirectorySeparatorChar + My.Application.Info.ProductName
  '    If Not System.IO.Directory.Exists(Result) Then
  '      System.IO.Directory.CreateDirectory(Result)
  '    End If

  '    Result += System.IO.Path.DirectorySeparatorChar + DsPrinsNET1.DataSetName + ".xml"

  '    Return Result
  '  End Get
  'End Property


  Private _FirstLoaded As Boolean = False

  Private Sub ucCRMExchanger_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    If Not _FirstLoaded Then
      ' -----------------------------------------------------------
      fgMessages.Rows.Count = fgMessages.Rows.Fixed
      fgMessages.AutoSizeRow(fgMessages.Rows.Fixed - 1)
      ' -----------------------------------------------------------
      Dim fn As String = IO.Path.Combine(CRMFolderPath, DsLog.DataSetName + ".xml")
      If System.IO.File.Exists(fn) Then
        Try
          DsLog.ReadXml(fn)
        Catch ex As Exception
          DisplayMessage($"Fehler beim Laden von {fn} - {ex.Message}")
        End Try
      End If
      ' -----------------------------------------------------------
      If System.IO.File.Exists(PrinsCRMFileName) Then
        Try
          DsTransfer.ReadXml(PrinsCRMFileName)
        Catch ex As Exception
          DisplayMessage($"Fehler beim Laden von {PrinsCRMFileName} - {ex.Message}")
        End Try

      End If

      ''PrinsNETFileName
      'If System.IO.File.Exists(PrinsNETFileName) Then
      '  DisplayMessage($"Lade : {PrinsNETFileName}")
      '  Try
      '    DsPrinsNET1.ReadXml(PrinsNETFileName)
      '  Catch ex As Exception
      '    DisplayMessage($"Fehler beim Laden von {PrinsNETFileName} - {ex.Message}")
      '  End Try
      'End If
      ' -----------------------------------------------------------
      If My.Settings.btnAutoExecPressed Then
        ibAutoStart.Pressed = True
      End If
      ' -----------------------------------------------------------
      ' BW liefen bei App-Ende
      If My.Settings.PrjFolderAutoStart Then
        SwitchFolderJobOnOff()
      End If
      ' -----------------------------------------------------------
      If My.Settings.AzureBusServiceAutoStart Then
        AzureBusServiceStart()
      End If
      ' -----------------------------------------------------------
      If System.Diagnostics.Debugger.IsAttached Then
        tmAutoExec.Interval = 1000 * 10
      End If
      ' ----------------------------------------------------
      iProgress1.Tag = inpTop.Width - iProgress1.Width
      ' ----------------------------------------------------
      dtMain.SelectedTab = cdpTasks
      ' ----------------------------------------------------
      AddHandler ParentForm.FormClosing, AddressOf ParentForm_FormClosing
      ' ----------------------------------------------------
      dtMain.SelectedTab = cdpMessages
      ' ----------------------------------------------------
      SetupMailbox()
      ' ----------------------------------------------------
      bw_SetFOBuchung.RunWorkerAsync()
      ' ----------------------------------------------------
    End If

    _FirstLoaded = True
  End Sub

  Private Sub SetupMailbox()
    If Not String.IsNullOrEmpty(My.Settings.AppMailbox) Then
      Try
        Me.UcOutlookMail.CurrentMailboxName = My.Settings.AppMailbox

        If Not String.IsNullOrEmpty(My.Settings.AppMailFolder) Then
          Me.UcOutlookMail.CurrentFolderName = My.Settings.AppMailFolder
        End If
        ' ---------------------------------------------------------------------------------------------------------
        If Not String.IsNullOrEmpty(My.Settings.AppMailSession) Then
          Me.UcOutlookMail.CurrentSessionName = My.Settings.AppMailSession
        End If
        ' -------------------------------------------------
        UpdateMailTemplates()
      Catch ex As Exception
      End Try
    End If
  End Sub

  Private Sub UcOutlookMail_Validated(sender As Object, e As EventArgs)
    My.Settings.AppMailbox = UcOutlookMail.CurrentMailboxName
    My.Settings.AppMailFolder = UcOutlookMail.CurrentFolderName
    My.Settings.AppMailSession = UcOutlookMail.CurrentSessionName

    If Not String.IsNullOrEmpty(UcOutlookMail.CurrentFolderName) Then
      UpdateMailTemplates()
    End If

  End Sub

  Private Sub UpdateMailTemplates()
    'UpdateMailFolders()
    ' -----------------------------------------------------
    If IsNothing(UcOutlookMail.CurrentMailFolder) Then Exit Sub
    ' -----------------------------------------------------
    Dim Folder As Outlook.MAPIFolder = UcOutlookMail.CurrentMailFolder
    ' -----------------------------------------------------
    Dim MailList As ArrayList = UcOutlookMail.MailList(Folder)
    Dim MailsSorted As New SortedList
    Dim Mail As Outlook.MailItem

    cbEntwurf.SelectedIndex = -1
    cbEntwurf.Items.Clear()

    For i As Integer = 0 To MailList.Count - 1
      Mail = TryCast(MailList(i), Outlook.MailItem)

      If Not IsNothing(Mail) AndAlso Not String.IsNullOrEmpty(Mail.Subject) Then
        Dim cOption As New C1.Win.C1InputPanel.InputOption(Mail.Subject)
        cOption.Tag = Mail
        cbEntwurf.Items.Add(cOption)
        ' -----------------------------------------------------
        If My.Settings.ADUserExistsTemplateName = cOption.Text Then
          cbEntwurf.SelectedOption = cOption
        End If
      End If
    Next

  End Sub

  Private Sub cbEntwurf_ChangeCommitted(sender As Object, e As EventArgs) Handles cbEntwurf.ChangeCommitted
    If Not IsNothing(cbEntwurf.SelectedOption) Then
      My.Settings.ADUserExistsTemplateName = cbEntwurf.SelectedOption.Text
    End If
  End Sub

  Private Sub SaveMessages()
    ' -----------------------------------------------------------
    Dim fn As String = IO.Path.Combine(CRMFolderPath, DsLog.DataSetName + ".xml")
    DsLog.WriteXml(fn)
    ' -----------------------------------------------------------
    If bsTblLog.Count > 500 Then bsTblLog.RemoveAt(500)

  End Sub

  Private Sub SaveDsPrinsCRM()
    Try
      DsTransfer.WriteXml(PrinsCRMFileName)
      DsTransfer.AcceptChanges()
    Catch ex As Exception
      DisplayMessage($"Speichern [{PrinsCRMFileName}] - Error : " + ex.Message)
    End Try
  End Sub

  Private Sub dtTransferDataTable_RowChanged(sender As Object, e As DataRowChangeEventArgs)
    If e.Action = DataRowAction.Change Then
      'DisplayMessage("dtTransferDataTable_RowChanged(DataRowAction.Change)")
      SaveDsPrinsCRM()
    End If
  End Sub

  Private Sub dtTransferDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs)
    If e.Action = DataRowAction.Delete Then
      SaveDsPrinsCRM()
    End If
  End Sub

  Private Sub ibStartTransfer_Click(sender As Object, e As EventArgs) Handles ibStartTransfer.Click
    AutoExecute()
  End Sub


  Private ReadADExecutedTime As DateTime = DateTime.MinValue
  Private _LastTransferAt As DateTime = DateTime.MinValue

  Private Sub AutoExecute()
    Dim TransCount As Integer = 0

    For Each TransferRow As dsTransfer.dtTransferRow In DsTransfer.dtTransfer.Rows
      If TransferRow.IsSourceTableNull Then Continue For
      If TransferRow.IsTargetTableNull Then Continue For
      If TransferRow.IsTransferEnabledNull OrElse Not TransferRow.TransferEnabled Then Continue For
      ' ------------------------------------------------------------------------
      Dim Msg As String = "Starte : " + TransferRow.SourceTable + " --> " + TransferRow.TargetTable
      DisplayMessage(Msg)
      My.Application.Logging.Add(Msg, clMessageLog.MsgSourceEnum.CRMExchanger, clMessageLog.MsgSubTypeEnum.StartMessage)
      Try
        _LastTransferAt = Now
        TransCount = DoTransferOf(TransferRow)

        Msg = "Beendet : " + TransferRow.SourceTable + " --> " + TransferRow.TargetTable + " " + ControlChars.CrLf +
              TransCount.ToString + " Datensätze übertragen"

        My.Application.Logging.Add(Msg, clMessageLog.MsgSourceEnum.CRMExchanger, clMessageLog.MsgSubTypeEnum.FinalSummary)
        DisplayMessage(Msg)
        ' ------------------------------------------------------------------------
        TransferRow.BeginEdit()

        If _LastTransferAt > DateTime.MinValue Then
          TransferRow.LastTransferAt = _LastTransferAt
          TransferRow.LastTransferCount = TransCount
        End If

        If TransferRow.IsTotalTransferCountNull Then
          TransferRow.TotalTransferCount = TransCount
        Else
          TransferRow.TotalTransferCount += TransCount
        End If
        TransferRow.EndEdit()
        ' ------------------------------------------------------------------------

      Catch ex As Exception
        Msg = "Fehler : " + TransferRow.SourceTable + " --> " + TransferRow.TargetTable + " " + ControlChars.CrLf +
              ex.Message
        My.Application.Logging.Add(Msg, clMessageLog.MsgSourceEnum.CRMExchanger, clMessageLog.MsgSubTypeEnum.FinalSummary, clMessageLog.MsgSeverityEnum.NonRecoverableError)
        DisplayMessage(Msg)
      End Try
    Next
    ' ------------------------------------------------------------------------------------------------
  End Sub

  ''' <summary>
  ''' Private Function CheckTimeAndInterval(TransferRow) As Boolean
  ''' Liefer OK = True zurück, wenn die Funktions ausgeführt werden soll
  ''' </summary>
  ''' <param name="TransferRow"></param>
  ''' <returns></returns>
  Private Function CheckTimeAndInterval(TransferRow As dsTransfer.dtTransferRow) As Boolean
    Dim Ok As Boolean = False
    Dim Von As DateTime = TransferRow.TransferStart
    Dim Bis As DateTime = TransferRow.TransferEnd

    ' -----------------------------------------------------------------------------------
    If TransferRow.IsLastCheckTimeNull Then TransferRow.LastCheckTime = Now
    ' -----------------------------------------------------------------------------------
    ' Uhrzeiten checken
    ' ganzer Tag
    If (Von.Hour * 60) + Von.Minute = (Bis.Hour * 60) + Bis.Minute Then
      Ok = True
      ' Von ist größer als Bis ... also Bis auf nächsten Tag
    ElseIf (Von.Hour * 60) + Von.Minute > (Bis.Hour * 60) + Bis.Minute Then

      Ok = (Now.Hour * 60) + Now.Minute >= (Von.Hour * 60) + Von.Minute OrElse
           (Now.Hour * 60) + Now.Minute < (Bis.Hour * 60) + Bis.Minute
    Else

      Ok = (Now.Hour * 60) + Now.Minute >= (Von.Hour * 60) + Von.Minute AndAlso
           (Now.Hour * 60) + Now.Minute < (Bis.Hour * 60) + Bis.Minute
    End If

    ' -------------------------------------------------------------------
    If Ok Then
      Ok = TransferRow.RepeatCount > 0
      TransferRow.RepeatCount -= 1
      ' -------------------------------------------------------------------
    End If
    ' -------------------------------------------------------------------
    If (Now - TransferRow.LastCheckTime).TotalMinutes >= 60 Then
      TransferRow.RepeatCount = TransferRow.RepeatPerHour
    End If
    ' -------------------------------------------------------------------
    Return Ok
  End Function


  Private Function DoTransferOf(TransferRow As dsTransfer.dtTransferRow) As Integer
    Dim Result As Integer = 0
    ' -----------------------------------------------------------------
    If TransferRow.IsSourceTableNull Then Return Result
    If TransferRow.IsTargetTableNull Then Return Result
    If TransferRow.IsTransferEnabledNull OrElse Not TransferRow.TransferEnabled Then Return Result
    If TransferRow.IsRepeatPerHourNull Then TransferRow.RepeatPerHour = 4
    If TransferRow.IsRepeatCountNull Then TransferRow.RepeatCount = TransferRow.RepeatPerHour
    ' -----------------------------------------------------------------
    If CheckTimeAndInterval(TransferRow) Then
      ' mögliche Kombinationen checken


      'ReadADUsers
      If TransferRow.SourceTable = "ReadADUsers" Then

        Result = ReadADUsers()

      ElseIf TransferRow.SourceTable = "TransferGeneralJournal" Then

        Result = TransferGeneralJournal()

      ElseIf TransferRow.SourceTable = dsCRMStageing1.VW_TBL_CRM_Firma_IMP.TableName AndAlso
              TransferRow.TargetTable = DsPrins1.TBL_CRM_FIRMA_IMP.TableName Then

        Result = TransferFirma2Prins()

      ElseIf TransferRow.SourceTable = DsPrinsNET1.TBL_PRJ_STANDORT_CHANGES.TableName AndAlso
          TransferRow.TargetTable = DsELOArchiv1.hpc_projects_costcenter.TableName Then

        TransferPRJ_STANDORT_CHG2ELOArchiv()

      ElseIf TransferRow.SourceTable = DsPrins1.VW_CRM_DEBITOR_EXP.TableName AndAlso
          TransferRow.TargetTable = dsCRMStageing1.Debitor.TableName Then

        Result = TransferDebitor2CRM()

      ElseIf TransferRow.SourceTable = DsPrins1.VW_CRM_PROJEKT_EXP.TableName AndAlso
          TransferRow.TargetTable = dsCRMStageing1.Projekt.TableName Then

        Result = TransferProjekt2CRM()

      ElseIf TransferRow.SourceTable = DsPrins1.VW_CRM_PERIODENABR_EXP.TableName AndAlso
          TransferRow.TargetTable = dsCRMStageing1.Periodenabrechnung.TableName Then

        Result = TransferPeriodenabrechnung2CRM()

      ElseIf TransferRow.SourceTable = DsPrins3Central.VW_USER_ACCOUNTS.TableName AndAlso
          TransferRow.TargetTable = dsCRMStageing1.HPC_Mitarbeiter.TableName Then

        Result = TransferUserAccounts2CRM()

      ElseIf TransferRow.SourceTable = DsPrins1.VW_ANWESENHEIT.TableName AndAlso
          TransferRow.TargetTable = dsCRMStageing1.Anwesenheit.TableName Then

        Result = TransferAnwesenheit2CRM()

      ElseIf TransferRow.SourceTable = dsCRMStageing1.Abwesenheit.TableName AndAlso
          TransferRow.TargetTable = DsPrins1.WOCHENBERICHT.TableName Then

        Result = TransferAbwesenheit2PRINS()

      ElseIf TransferRow.SourceTable = dsCRMStageing1.VW_TBL_CRM_Firma_Debitor_LNK_IMP.TableName AndAlso
          TransferRow.TargetTable = DsPrins1.TBL_CRM_FIRMA_DEBITOR_LNK_IMP.TableName Then


        Result = TransferFirmaDebitor2PRINS()

      ElseIf TransferRow.SourceTable = dsCRMStageing1.Mitarbeiter.TableName AndAlso
          TransferRow.TargetTable.StartsWith("https://") Then


        Result = TransferMitarbeiter2ARM(TransferRow.TargetTable)


      ElseIf TransferRow.SourceTable = DsHPCReweSKR04.KHKBuchungByOp.TableName AndAlso
            TransferRow.TargetTable = DsPrins1.TBL_KHK_OPBUCHUNGEN.TableName Then

        Result = TransferKHKBuchung2Prins()


      ElseIf TransferRow.SourceTable = dsCRMStageing1.Kontakte.TableName AndAlso
          TransferRow.TargetTable.ToLower.IndexOf("pascom") > -1 Then

        If Not TransferRow.IsRemarkNull Then

          Dim sp As String()
          Dim Url As String = TransferRow.TargetTable
          Dim User As String
          Dim Pwd As String

          sp = TransferRow.Remark.Split(",")
          If sp.Length >= 2 Then
            User = sp(0)
            Pwd = sp(1)
            Result = TransferKontakte2Pascom(Url, User, Pwd)
            _LastTransferAt = Now
          End If
        End If

      End If
    End If

    Return Result
  End Function

  Private _TransferKHKBuchungByOpTmr As DateTime = DateTime.MinValue
  Private _TransferKontakte2PascomTmr As DateTime = DateTime.MinValue

  Private Function TransferKHKBuchung2Prins() As Integer
    Dim Result As Integer = 0
    Dim dtSource As dsHPCReweSKR04.KHKBuchungByOpDataTable = DsHPCReweSKR04.KHKBuchungByOp
    Dim dtTarget As dsPrins.TBL_KHK_OPBUCHUNGENDataTable = DsPrins1.TBL_KHK_OPBUCHUNGEN
    Dim SourceRow As dsHPCReweSKR04.KHKBuchungByOpRow
    Dim TargetRow As dsPrins.TBL_KHK_OPBUCHUNGENRow
    Dim MaxTan As Integer = 1784811 ' 1784812 ist die erste TAN nach dem 01.01.2020
    ' ---------------------------------------------------------------------
    taTBL_KHK_OPBUCHUNGEN.FillByMaxTan(dtTarget, 1)
    If dtTarget.Rows.Count >= 1 Then ' es können mehrere Datensätze zurückkommen, aber immer nur genau eine TAN (verschiedene OPs)
      MaxTan = dtTarget.Rows(0)("KHK_TAN")
      dtTarget.Clear()
    End If
    ' ---------------------------------------------------------------------
    taKhkBuchungByOp.FillByMdtAbTan(dtSource, MaxTan, 1)
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferKHKBuchung2Prins : " + dtSource.Rows.Count.ToString + " Änderungen")
    StartProgress(dtSource.Rows.Count)
    ' ---------------------------------------------------------------------
    For Each SourceRow In dtSource.Rows
      ' ---------------------------------------------------
      TargetRow = dtTarget.NewTBL_KHK_OPBUCHUNGENRow
      TargetRow.ID = My.Forms.frmMain.GetGeneratorValue("GEN_TBL_KHK_OPBUCHUNGEN_ID")
      TargetRow.KHK_TAN = SourceRow.Tan
      TargetRow.KHK_MANDANT = SourceRow.Mandant
      CopyDataRow(SourceRow, TargetRow)
      dtTarget.AddTBL_KHK_OPBUCHUNGENRow(TargetRow)
      taTBL_KHK_OPBUCHUNGEN.Update(dtTarget)
      dtTarget.Clear()
      ' ---------------------------------------------------------------------
      If System.Diagnostics.Debugger.IsAttached AndAlso Result > 100 Then Exit For
      ' ---------------------------------------------------------------------
      StepProgress()
      Result += 1
    Next
    ' ---------------------------------------------------------------------
    EndProgress()

    Return Result
  End Function

  Private Function TransferFirma2Prins() As Integer
    Dim Result As Integer = 0
    Dim dtSource As dsCRMStageing.VW_TBL_CRM_Firma_IMPDataTable = dsCRMStageing1.VW_TBL_CRM_Firma_IMP
    Dim dtTarget As dsPrins.TBL_CRM_FIRMA_IMPDataTable = DsPrins1.TBL_CRM_FIRMA_IMP
    Dim TargetRow As dsPrins.TBL_CRM_FIRMA_IMPRow
    Dim UpdateCount As Integer = 0
    Dim InsertCount As Integer = 0
    Dim TaggCommand As String = "UPDATE Firma SET TAGGED = 1"

    taVW_TBL_CRM_Firma_IMP.ExecuteNonQuery(TaggCommand)
    taVW_TBL_CRM_Firma_IMP.FillByTagged(dtSource)
    dtTarget.Clear()
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferFirma2Prins : " + dtSource.Rows.Count.ToString + " Änderungen")
    StartProgress(dtSource.Rows.Count) : Result = dtSource.Rows.Count
    ' ---------------------------------------------------------------------
    For Each Row As dsCRMStageing.VW_TBL_CRM_Firma_IMPRow In dtSource.Rows
      If Row.IsFIRMA_IDNull Then Continue For
      ' ---------------------------------------------------------------------
      taTBL_CRM_FIRMA_IMP.FillByFirmaID(dtTarget, Row.FIRMA_ID)
      If dtTarget.Rows.Count = 0 Then
        TargetRow = dtTarget.NewTBL_CRM_FIRMA_IMPRow
        InsertCount += 1
      Else
        TargetRow = dtTarget.Rows(0)
        UpdateCount += 1
      End If
      ' ----------------------------------------------------------------
      CopyDataRow(Row, TargetRow)
      If TargetRow.RowState = DataRowState.Detached Then
        dtTarget.AddTBL_CRM_FIRMA_IMPRow(TargetRow)
      End If
      ' ----------------------------------------------------------------
      taTBL_CRM_FIRMA_IMP.Update(dtTarget)
      StepProgress()
    Next
    ' ---------------------------------------------------------------------
    DisplayMessage("Eingefügt : " + InsertCount.ToString + ", geändert : " + UpdateCount.ToString)
    ' ---------------------------------------------------------------------
    TaggCommand = "DELETE FROM Firma WHERE TAGGED=1"
    taVW_TBL_CRM_Firma_IMP.ExecuteNonQuery(TaggCommand)
    ' ---------------------------------------------------------------------

    EndProgress()
    ' ---------------------------------------------------------------------
    Return Result
  End Function


  'TransferKontakte2Pascom
  Private Function TransferKontakte2Pascom(Url As String, User As String, Password As String) As Integer
    Dim Result As Integer = 0
    Dim client As System.Net.WebClient = New System.Net.WebClient()
    Dim uri As Uri = New Uri(Url) '"https://pascom.cloud/hpcag-test/services/connector/CSV-Import/push")
    Dim Fields As String() = {"telephone1", "telephone2", "firstname", "lastname", "firma", "emailaddress_1", "mobilephone", "homephone", "fax", "CRM_ID"}
    Dim Hdr As String = "phone,homephone,givenname,surname,organisation,email,mobile,homephone,fax,dynamicsID"
    Dim Contacts As New System.Text.StringBuilder
    Dim Utf8Data As Byte()
    Dim Tel2Generated As Integer = 0

    taKontakte.FillByAktiv(dsCRMStageing1.Kontakte)
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferKontakte2Pascom : " + dsCRMStageing1.Kontakte.Rows.Count.ToString + " Kontakte")
    StartProgress(dsCRMStageing1.Kontakte.Rows.Count)
    ' ---------------------------------------------------------------------

    Contacts.AppendLine(Hdr)
    For Each Row As dsCRMStageing.KontakteRow In dsCRMStageing1.Kontakte.Rows
      Dim tmp As String = String.Empty
      Dim HasPhone As Boolean = False
      Dim HasMobile As Boolean = False
      Dim IsHPCEMailAdress As Boolean = False

      _PhoneTruncated = False
      ' ---------------------------------------------------------------------
      ' HPC AG Mitarbeiter?
      'If Not Row.Isemailaddress_1Null AndAlso Row.emailaddress_1.IndexOf("hpc.ag") > -1 AndAlso
      '  (Row.Istelephone2Null OrElse String.IsNullOrEmpty(Row.telephone2)) Then

      '  DsPrins1.PERSONAL.TableAdapter.FillByEMail(DsPrins1.PERSONAL, Row.emailaddress_1)
      '  If DsPrins1.PERSONAL.Rows.Count = 1 Then
      '    Dim tel2 As String
      '    Dim PersRow As dsPrins.PERSONALRow = DsPrins1.PERSONAL.Rows(0)
      '    Dim KstRow As dsPrins.KOSTENSTELLENRow = KostenstellenRow(PersRow.KOSTENSTELLE)

      '    If Not IsNothing(KstRow) Then
      '      Dim Ext As String = GetPhoneExt(Row.telephone1, If(Not KstRow.IsTEL_EXT_LEN_MINNull, KstRow.TEL_EXT_LEN_MIN, 3), If(Not KstRow.IsTEL_EXT_LEN_MAXNull, KstRow.TEL_EXT_LEN_MAX, 3))
      '      Dim ExtLen As Integer = If(KstRow.IsTEL_PREFIXNull, 3, 5 - KstRow.TEL_PREFIX.ToString.Length)

      '      If Not String.IsNullOrEmpty(Ext) Then
      '        tel2 = If(KstRow.IsTEL_PREFIXNull, String.Empty, KstRow.TEL_PREFIX.ToString).PadLeft(2, "9") + Ext.PadLeft(ExtLen, "0")
      '        Row.telephone2 = tel2
      '        Tel2Generated += 1
      '      End If
      '    End If
      '  End If
      'End If
      ' ---------------------------------------------------------------------
      For i As Integer = 0 To Fields.Length - 1
        If i > 0 Then tmp += ","
        If Row.Table.Columns.Contains(Fields(i)) AndAlso
           Not Row.IsNull(Fields(i)) Then

          Select Case Fields(i)
            Case "telephone1"
              ' Leerzeichen aus Telefonnummer
              tmp += """" + FormatPhoneNumber(Row(Fields(i)).ToString) + """"
              HasPhone = True

            Case "mobilephone"
              ' Leerzeichen aus Telefonnummer
              tmp += """" + FormatPhoneNumber(Row(Fields(i)).ToString) + """"
              HasMobile = True

            Case "emailaddress_1"
              Dim EMail As String = Row(Fields(i)).ToString.Trim.ToLower
              If IsValidEMailAddress(EMail) Then
                tmp += EMail

                IsHPCEMailAdress = EMail.Contains("hpc.ag") OrElse EMail.Contains("hpc-ag.de") OrElse
                                   EMail.Contains("hpc-italia.it") OrElse
                                   EMail.Contains("hpc-austria.at") OrElse
                                   EMail.Contains("hpc-envirotec.com") OrElse
                                   EMail.Contains("hpc-international.com") OrElse
                                   EMail.Contains("hpc-polska.pl") OrElse
                                   EMail.Contains("hpc-bulgaria.bg")

              End If

            Case Else
              tmp += """" + ReplaceInvalidChars(Row(Fields(i)).ToString) + """"
          End Select
        End If
      Next
      ' ---------------------------------------------------------------------
      If Not IsHPCEMailAdress AndAlso (HasPhone OrElse HasMobile) Then
        Contacts.AppendLine(tmp)

        If _PhoneTruncated Then
          If Not Row.IsNull("telephone1") Then tmp += "," + Row("telephone1").ToString
          If Not Row.IsNull("mobilephone") Then tmp += "," + Row("mobilephone").ToString
          LinesTruncatedPhone.AppendLine(tmp)
        End If
      End If
      ' ---------------------------------------------------------------------
      StepProgress()
      ' ---------------------------------------------------------------------
      Result += 1
      ' ---------------------------------------------------------------------
      If Debugger.IsAttached AndAlso Result = 500 Then Exit For
    Next
    ' ---------------------------------------------------------------------
    Try
      Dim ReturnValue As Byte()
      ' --------------------------------------------------------------------
      Utf8Data = System.Text.Encoding.UTF8.GetBytes(Contacts.ToString)

      client.Credentials = New System.Net.NetworkCredential(User, Password)
      ReturnValue = client.UploadData(uri, "POST", System.Text.Encoding.UTF8.GetBytes(Contacts.ToString))
      Debug.WriteLine(System.Text.Encoding.UTF8.GetString(ReturnValue))
      ' --------------------------------------------------------------------
      If xbSaveCSV.Checked OrElse Debugger.IsAttached Then
        Dim p As String = Application.LocalUserAppDataPath
        Dim fn As String = "Pascom upload " + Now.Year.ToString + "-" + Now.Month.ToString + "-" + Now.Day.ToString + " " + Now.ToShortTimeString.Replace(":", "-") + ".csv"
        Dim fs As System.IO.FileStream
        Dim idx As Integer = p.IndexOf(Application.ProductVersion)

        If idx > -1 Then p = p.Substring(0, idx)
        fn = System.IO.Path.Combine(p, fn)

        If Not IsNothing(ReturnValue) Then
          Contacts.AppendLine("")
          Contacts.AppendLine(System.Text.Encoding.UTF8.GetString(ReturnValue))
          Utf8Data = System.Text.Encoding.UTF8.GetBytes(Contacts.ToString)
        End If

        'For i As Integer = 0 To Utf8Data.Length - 1
        '  If i > 0 AndAlso Hex(Utf8Data(i - 1)).ToString = "C3" AndAlso Hex(Utf8Data(i)).ToString = "7C" Then
        '    Debug.WriteLine(Hex(Utf8Data(i - 1)).ToString + " " + CInt(Utf8Data(i - 1)).ToString)
        '    Debug.WriteLine(Hex(Utf8Data(i)).ToString + " " + CInt(Utf8Data(i)).ToString)
        '  End If
        'Next

        fs = System.IO.File.Open(fn, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None)
        fs.Write(Utf8Data, 0, Utf8Data.Length)
        fs.Close()

        If Not IsNothing(_LinesTruncatedPhone) Then
          LinesTruncatedPhone.Insert(0, Hdr + ",OriginalTelephone1,OriginalMobilephone" + ControlChars.CrLf)
          fn = fn.Replace(".csv", ".truncated.csv")

          Utf8Data = System.Text.Encoding.UTF8.GetBytes(LinesTruncatedPhone.ToString)
          fs = System.IO.File.Open(fn, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None)
          fs.Write(Utf8Data, 0, Utf8Data.Length)
          fs.Close()
          Debug.WriteLine("Saved : " + fn)
        End If
      End If
    Finally
      _LinesTruncatedPhone = Nothing
      EndProgress()
    End Try
    ' ---------------------------------------------------------------------
    Debug.WriteLine("Tel2Generated=" + Tel2Generated.ToString)
    Debug.WriteLine("Result=" + Result.ToString)
    ' ---------------------------------------------------------------------
    Return Result
  End Function


  Private _LinesTruncatedPhone As System.Text.StringBuilder = Nothing
  Private ReadOnly Property LinesTruncatedPhone As System.Text.StringBuilder
    Get
      If IsNothing(_LinesTruncatedPhone) Then
        _LinesTruncatedPhone = New System.Text.StringBuilder
      End If
      Return _LinesTruncatedPhone
    End Get
  End Property


  Private Function GetPhoneExt(Value As String, MinLen As Integer, MaxLen As Integer) As String
    Dim Result As String = String.Empty
    Dim MyMaxLen As Integer = MaxLen
    Dim Tmp As String = ReplaceInvalidChars(Value).Trim

    For x As Integer = Tmp.Length - 1 To 0 Step -1
      If MyMaxLen = 0 Then Exit For
      If Not Char.IsDigit(Tmp.Substring(x, 1)) Then Exit For
      Result = Tmp.Substring(x, 1) + Result
      MyMaxLen -= 1
    Next

    ' die gewünschte Länge ist nicht vorhanden. dann gar keine
    If Result.Length < MinLen Then
      Result = String.Empty
    End If


    Return Result
  End Function



  Private _PhoneTruncated As Boolean = False
  Private Function FormatPhoneNumber(Value As String) As String
    Dim Result As String = String.Empty
    Dim Tmp As String = ReplaceInvalidChars(Value).Trim.Replace(" ", "").Replace("(0)", "").Replace("++", "+")
    Dim SepChars = "()-/."

    For x As Integer = 0 To Tmp.Length - 1
      Dim c As Char = Tmp.Substring(x, 1)

      ' Non-Digit order kein Sep-Char
      If c <> "+" AndAlso Not Char.IsDigit(c) AndAlso SepChars.IndexOf(c) = -1 Then
        _PhoneTruncated = True
        Exit For
      Else
        If SepChars.IndexOf(c) = -1 OrElse (c = "+" AndAlso x = 0) Then
          Result += c
        End If
      End If
    Next
    ' ---------------------------------------------------------------
    Result = Result.Substring(0, Math.Min(Result.Length, 15 + 1))
    ' ---------------------------------------------------------------
    Return Result
  End Function

  Private Function IsValidEMailAddress(Value As String) As Boolean
    Dim EMail As String = Value.ToString.Trim.ToLower
    Dim AtIdx As Integer = EMail.IndexOf("@")
    Dim LastDotIdx As Integer = EMail.LastIndexOf(".")
    Dim IsValid As Boolean

    ' Es muss ein @ enthalten sein und danach noch ein . kommen
    IsValid = AtIdx > -1 AndAlso LastDotIdx > AtIdx

    If IsValid Then
      For x As Integer = 0 To EMail.Length - 1
        Dim c As Char = EMail.Substring(x, 1)

        If (c < "a" OrElse c > "z") AndAlso
                    Not Char.IsDigit(c) AndAlso
                   ("@_-.").IndexOf(c) = -1 Then

          IsValid = False
          Exit For
        End If
      Next
    End If

    Return IsValid
  End Function


  Private Function ReplaceInvalidChars(Value As String) As String
    Dim Result As String = Value.Trim

    ' ----------------------------------------------------
    If Result.IndexOf("\") > -1 Then
      Debug.WriteLine(Result)
    End If
    ' ----------------------------------------------------
    Result = Result.Replace(",", ".")
    Result = Result.Replace(ControlChars.CrLf, " ")
    Result = Result.Replace(ControlChars.Cr, " ")
    Result = Result.Replace(ControlChars.Lf, " ")
    Result = Result.Replace("""", "'")
    Result = Result.Replace("\", "/")
    Result = Result.Replace("â", "a")
    Result = Result.Replace("á", "a")
    Result = Result.Replace("à", "a")
    Result = Result.Replace("é", "e")
    Result = Result.Replace("è", "e")
    Result = Result.Replace("ê", "e")
    Result = Result.Replace("í", "i")
    Result = Result.Replace("ì", "i")
    Result = Result.Replace("î", "i")
    Result = Result.Replace("ô", "o")
    Result = Result.Replace("ó", "o")
    Result = Result.Replace("ò", "o")
    Result = Result.Replace("û", "u")
    Result = Result.Replace("ú", "u")
    Result = Result.Replace("ù", "u")
    Result = Result.Replace("ù", "u")


    Return Result
  End Function


  'TransferFirmaDebitor2PRINS
  Private Function TransferFirmaDebitor2PRINS() As Integer
    Dim Result As Integer = 0
    Dim dtSource As dsCRMStageing.VW_TBL_CRM_Firma_Debitor_LNK_IMPDataTable = dsCRMStageing1.VW_TBL_CRM_Firma_Debitor_LNK_IMP
    Dim dtTarget As dsPrins.TBL_CRM_FIRMA_DEBITOR_LNK_IMPDataTable = DsPrins1.TBL_CRM_FIRMA_DEBITOR_LNK_IMP
    Dim SourceRow As dsCRMStageing.VW_TBL_CRM_Firma_Debitor_LNK_IMPRow
    Dim TargetRow As dsPrins.TBL_CRM_FIRMA_DEBITOR_LNK_IMPRow
    Dim TaggCommand As String = "UPDATE TBL_CRM_FIRMA_DEBITOR_LNK_IMP AW SET AW.TAGGED = true"

    taTBL_CRM_FIRMA_DEBITOR_LNK_IMP.ExecuteNonQuery(TaggCommand)
    ' ---------------------------------------------------------------------
    taVW_TBL_CRM_Firma_Debitor_LNK_IMP.Fill(dtSource)
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferFirmaDebitor2PRINS : " + dtSource.Rows.Count.ToString + " Änderungen")
    StartProgress(dtSource.Rows.Count)
    ' ---------------------------------------------------------------------

    For Each SourceRow In dtSource.Rows
      ' ---------------------------------------------------
      Dim DebitorNr As Integer = -1

      Try
        DebitorNr = CInt(SourceRow.DEBITORNUMMER)
      Catch ex As Exception
      End Try
      If String.IsNullOrEmpty(SourceRow.FIRMENNUMMER) OrElse DebitorNr <= 0 Then Continue For
      ' ---------------------------------------------------
      taTBL_CRM_FIRMA_DEBITOR_LNK_IMP.FillByFirmaDebitor(dtTarget, SourceRow.FIRMENNUMMER, DebitorNr)
      If dtTarget.Rows.Count > 0 Then
        TargetRow = dtTarget.Rows(0)
      Else
        TargetRow = dtTarget.NewTBL_CRM_FIRMA_DEBITOR_LNK_IMPRow
      End If
      ' ---------------------------------------------------------------------
      TargetRow.FIRMENNUMMER = SourceRow.FIRMENNUMMER
      TargetRow.DEBITORNUMMER = SourceRow.DEBITORNUMMER
      TargetRow.TAGGED = False
      If TargetRow.RowState = DataRowState.Detached Then
        TargetRow.ID = My.Forms.frmMain.GetGeneratorValue("GEN_TBL_CRM_FIRMA_DEBITOR_LNK_I")
        dtTarget.AddTBL_CRM_FIRMA_DEBITOR_LNK_IMPRow(TargetRow)
      End If
      ' ---------------------------------------------------------------------
      Try
        taTBL_CRM_FIRMA_DEBITOR_LNK_IMP.Update(dtTarget)
      Catch ex As Exception
        If ex.Message.IndexOf("UNQ1") > 0 Then
          DisplayMessage(String.Format("Frimennummer [{0}] : Debitor [{1}] - Kombination bereits vorhanden!", SourceRow.FIRMENNUMMER, DebitorNr))
        ElseIf ex.Message.IndexOf("TBL_CRM_FIRMA_DEBITOR_LNK__IDX1") > 0 Then
          DisplayMessage(String.Format("Debitor [{0}] - bereits einer anderen Firma zugeordnet!", DebitorNr))
        Else
          Throw New Exception(ex.Message)
        End If
      End Try
      ' ---------------------------------------------------------------------
      StepProgress()
      Result += 1
    Next
    ' ---------------------------------------------------------------------
    TaggCommand = "DELETE FROM TBL_CRM_FIRMA_DEBITOR_LNK_IMP AW WHERE AW.TAGGED = true"
    taTBL_CRM_FIRMA_DEBITOR_LNK_IMP.ExecuteNonQuery(TaggCommand)
    ' ---------------------------------------------------------------------
    EndProgress()

    Return Result
  End Function




  Private Function TransferAbwesenheit2PRINS() As Integer
    Dim Result As Integer = 0
    Dim dtSource As dsCRMStageing.AbwesenheitDataTable = dsCRMStageing1.Abwesenheit
    Dim dtTarget As dsPrins.WOCHENBERICHTDataTable = DsPrins1.WOCHENBERICHT
    Dim SourceRow As dsCRMStageing.AbwesenheitRow
    Dim TargetRow As dsPrins.WOCHENBERICHTRow

    taABWESENHEITSART.Fill(DsPrins1.TBL_ABWESENHEITSART)
    taAbwesenheit.FillByChanged(dtSource)
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferAbwesenheit2PRINS : " + dtSource.Rows.Count.ToString + " Änderungen")
    StartProgress(dtSource.Rows.Count) : Result = dtSource.Rows.Count
    ' ---------------------------------------------------------------------
    Dim Msg As String
    Dim PersNr As Integer = -1
    Dim MaID As Integer = -1

    For Each SourceRow In dtSource.Rows

      Do
        ' ---------------------------------------------------
        If MaID <> SourceRow.Mitarbeiter_ID Then
          taPersonal.FillBySapNummer(DsPrins1.PERSONAL, SourceRow.Mitarbeiter_ID)

          If DsPrins1.PERSONAL.Rows.Count = 0 Then
            Msg = String.Format("Mitarbeiter[{0}] - {1} - nicht gefunden!", SourceRow.Mitarbeiter_ID, SourceRow.Mitarbeiter)
            My.Application.Logging.Add(Msg, clMessageLog.MsgSourceEnum.CRMExchanger, clMessageLog.MsgSubTypeEnum.DetailedMessage, clMessageLog.MsgSeverityEnum.NonRecoverableError)
            DisplayMessage(Msg)
            Exit Do
          End If
          PersNr = DsPrins1.PERSONAL.Rows(0)("NUMMER")
          MaID = SourceRow.Mitarbeiter_ID
          ' ---------------------------------------------------
          Msg = String.Format("Mitarbeiter[{0}] - {1}", SourceRow.Mitarbeiter_ID, SourceRow.Mitarbeiter)
          DisplayMessage(Msg)
        End If
        ' ---------------------------------------------------
        taWochenbericht.FillByCrmID(dtTarget, SourceRow.CRM_ID)
        ' ---------------------------------------------------
        ' bestehende Sätze löschen
        While dtTarget.Rows.Count > 0
          TargetRow = dtTarget.Rows(0)
          TargetRow.Delete()
          taWochenbericht.Update(dtTarget)
          dtTarget.AcceptChanges()
        End While
        ' ---------------------------------------------------
        Dim AbRow As dsPrins.TBL_ABWESENHEITSARTRow = If(SourceRow.Feiertag = "Yes", AbwesenheitsArt(SourceRow.Abwesenheitsart, SourceRow.Datum), Nothing)
        Dim AbSplit As String()
        Dim AnteiligerFeiertag As Boolean = False

        If IsNothing(AbRow) Then
          AbSplit = If(SourceRow.Feiertag = "Yes", "Feiertag", SourceRow.Abwesenheitsart).Split(",")
        Else
          AbSplit = AbRow.PRINS_BEZEICHNUNG.Split(",")
          AnteiligerFeiertag = True
        End If

        For i As Integer = 0 To AbSplit.Length - 1
          AbRow = AbwesenheitsArt(AbSplit(i))
          If IsNothing(AbRow) Then Continue For
          ' ---------------------------------------------------------------------
          TargetRow = dtTarget.NewWOCHENBERICHTRow()
          TargetRow.ID = My.Forms.frmMain.GetGeneratorValue("GEN_WOCHENBERICHT_ID")
          TargetRow.CRM_ID = SourceRow.CRM_ID
          TargetRow.DATUM = SourceRow.Datum
          TargetRow.PERSONALNR = PersNr
          TargetRow.ABGERECHNET = "0"
          TargetRow.ABRECHENBAR = "0"
          TargetRow.VERBUCHT = "1"
          TargetRow.FREIGEGEBEN = "1"
          TargetRow.SELEKTIERT = "0"
          TargetRow.LETZTEAENDERUNGDATUM = Now
          TargetRow.LETZTEAENDERUNGUSER = "DYNAMICS"
          TargetRow.BTEXT = AbRow.PRINS_BEZEICHNUNG  'AbSplit(i)
          TargetRow.PROJEKTNUMMER = AbRow.PRJ_NUMMER
          TargetRow.STANDORT = AbRow.PRJ_STANDORT
          TargetRow.MERKMAL = AbRow.MERKMAL

          If AnteiligerFeiertag Then
            TargetRow.MENGE = SourceRow.Sollstunden / If(AbSplit.Length > 1, 2, 1)
          Else
            TargetRow.MENGE = If(CDec(SourceRow.Anzahl_Stunden) = 0, SourceRow.Sollstunden, SourceRow.Anzahl_Stunden) / If(AbSplit.Length > 1, 2, 1)
          End If

          If Not AbRow.IsTBL_ARTIKEL_IDNull Then
            TargetRow.ARTIKEL = AbRow.TBL_ARTIKEL_ID
          End If
          dtTarget.AddWOCHENBERICHTRow(TargetRow)
        Next
      Loop While False ' nur wg. Exit Do
      ' ---------------------------------------------------------------------
      If dtTarget.Rows.Count > 0 Then taWochenbericht.Update(dtTarget)
      ' ---------------------------------------------------------------------
      SourceRow.HasChanged = False
      ' ---------------------------------------------------------------------
      StepProgress()
    Next


    ' ---------------------------------------------------------------------
    taAbwesenheit.Update(dtSource)
    ' ---------------------------------------------------------------------
    EndProgress()

    Return Result
  End Function


  Private Function AbwesenheitsArt(AbBezeichnung As String, Optional Datum As Date = Nothing) As dsPrins.TBL_ABWESENHEITSARTRow
    Dim Result As dsPrins.TBL_ABWESENHEITSARTRow = Nothing
    If IsNothing(Datum) Then Datum = DateTime.MinValue

    ' ORDER BY FIX_DATUM
    For Each Row As dsPrins.TBL_ABWESENHEITSARTRow In DsPrins1.TBL_ABWESENHEITSART.Rows
      If Datum > DateTime.MinValue Then
        If Row.BEZEICHNUNG.ToUpper = AbBezeichnung.ToUpper AndAlso Not Row.IsFIX_DATUMNull AndAlso
         (Row.FIX_DATUM.Day = Datum.Day AndAlso Row.FIX_DATUM.Month = Datum.Month) Then

          Result = Row
          Exit For
        End If
      Else
        If Row.BEZEICHNUNG.ToUpper = AbBezeichnung.ToUpper AndAlso Row.IsFIX_DATUMNull Then
          Result = Row
          Exit For
        End If
      End If
    Next

    Return Result
  End Function


  Private Function TransferPRJ_STANDORT_CHG2ELOArchiv() As Integer
    Dim Result As Integer = 0
    Dim dtSource As dsPrinsNET.TBL_PRJ_STANDORT_CHANGESDataTable = DsPrinsNET1.TBL_PRJ_STANDORT_CHANGES
    Dim dtTarget As dsELOArchiv1.hpc_projects_costcenterDataTable = DsELOArchiv1.hpc_projects_costcenter
    Dim TargetRow As dsELOArchiv1.hpc_projects_costcenterRow
    Dim UpdateCount As Integer = 0
    Dim InsertCount As Integer = 0
    Dim TaggCommand As String = "UPDATE TBL_PRJ_STANDORT_CHANGES PC SET PC.TAGGED = 1"

    dtSource.TableAdapter.ExecuteNonQuery(TaggCommand)
    dtSource.TableAdapter.FillByTagged(dtSource)

    dtTarget.Clear()
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferPRJ_STANDORT_CHG2ELOArchiv : " + dtSource.Rows.Count.ToString + " Änderungen")
    StartProgress(dtSource.Rows.Count) : Result = dtSource.Rows.Count
    ' ---------------------------------------------------------------------
    For Each Row As dsPrinsNET.TBL_PRJ_STANDORT_CHANGESRow In dtSource.Rows
      ' ---------------------------------------------------------------------
      taHpc_projects_costcenter.FillByPrjNoCostcenter(dtTarget, Row.PROJECTNO, Row.COSTCENTER)
      ' ---------------------------------------------------------------------
      If dtTarget.Rows.Count = 1 Then
        TargetRow = dtTarget.Rows(0)
        UpdateCount += 1
        ' ---------------------------------------------------------------------
        If Not Row.IsIS_DELETEDNull And Row.IS_DELETED = 1 Then
          TargetRow.Delete()
        End If
      Else
        If Not Row.IsIS_DELETEDNull And Row.IS_DELETED = 1 Then
          StepProgress() : Continue For
        End If
        ' ----------------------------------------------------------------------------------
        TargetRow = dtTarget.Newhpc_projects_costcenterRow
        InsertCount += 1
        TargetRow.ProjectNo = Row.PROJECTNO
        TargetRow.Costcenter = Row.COSTCENTER
        dtTarget.Addhpc_projects_costcenterRow(TargetRow)
      End If
      ' ----------------------------------------------------------------
      taHpc_projects_costcenter.Update(dtTarget)
      StepProgress()
    Next
    ' ---------------------------------------------------------------------
    DisplayMessage("Eingefügt : " + InsertCount.ToString + ", geändert : " + UpdateCount.ToString)
    ' ---------------------------------------------------------------------
    TaggCommand = "DELETE FROM TBL_PRJ_STANDORT_CHANGES PC WHERE PC.TAGGED=1"
    dtSource.TableAdapter.ExecuteNonQuery(TaggCommand)
    ' ---------------------------------------------------------------------
    EndProgress()

    Return Result
  End Function




  Private Function TransferAnwesenheit2CRM() As Integer
    Dim Result As Integer = 0
    Dim dtSource As dsPrins.VW_ANWESENHEITDataTable = DsPrins1.VW_ANWESENHEIT
    Dim dtTarget As dsCRMStageing.AnwesenheitDataTable = dsCRMStageing1.Anwesenheit
    Dim TargetRow As dsCRMStageing.AnwesenheitRow
    Dim UpdateCount As Integer = 0
    Dim InsertCount As Integer = 0
    Dim TaggCommand As String = "UPDATE TBL_ANWESENHEIT AW SET AW.TAGGED = 1"
    Dim LogCommand As String = "insert into TBL_ANWESENHEIT_LOG (TAGGED_COUNT) values (0);"

    taVW_ANWESENHEIT.ExecuteNonQuery(TaggCommand)
    taVW_ANWESENHEIT.ExecuteNonQuery(LogCommand)
    taVW_ANWESENHEIT.Fill(dtSource)

    dtTarget.Clear()
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferAnwesenheit2CRM : " + dtSource.Rows.Count.ToString + " Änderungen")
    StartProgress(dtSource.Rows.Count) : Result = dtSource.Rows.Count
    ' ---------------------------------------------------------------------
    For Each Row As dsPrins.VW_ANWESENHEITRow In dtSource.Rows
      ' ---------------------------------------------------------------------
      TargetRow = dtTarget.NewAnwesenheitRow
      InsertCount += 1
      ' ----------------------------------------------------------------
      CopyDataRow(Row, TargetRow)
      If TargetRow.RowState = DataRowState.Detached Then
        dtTarget.AddAnwesenheitRow(TargetRow)
      End If

      ' ----------------------------------------------------------------
      taAnwesenheit.Update(dtTarget)
      StepProgress()
    Next
    ' ---------------------------------------------------------------------
    DisplayMessage("Eingefügt : " + InsertCount.ToString + ", geändert : " + UpdateCount.ToString)
    ' ---------------------------------------------------------------------
    TaggCommand = "DELETE FROM TBL_ANWESENHEIT AW WHERE AW.TAGGED=1"
    taVW_ANWESENHEIT.ExecuteNonQuery(TaggCommand)
    ' ---------------------------------------------------------------------
    EndProgress()

    Return Result
  End Function



  Private Function TransferDebitor2CRM() As Integer
    Dim Result As Integer = 0
    Dim dtSource As dsPrins.VW_CRM_DEBITOR_EXPDataTable = DsPrins1.VW_CRM_DEBITOR_EXP
    Dim dtTarget As dsCRMStageing.DebitorDataTable = dsCRMStageing1.Debitor
    Dim TargetRow As dsCRMStageing.DebitorRow
    Dim UpdateCount As Integer = 0
    Dim InsertCount As Integer = 0
    Dim TaggCommand As String = "UPDATE TBL_CRM_STAGEING CS SET CS.TAGGED = 1 WHERE CS.SOURCE_TABLE='KUNDEN'"

    taVW_CRM_DEBITOR_EXP.ExecuteNonQuery(TaggCommand)
    taVW_CRM_DEBITOR_EXP.Fill(dtSource)
    dtTarget.Clear()
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferDebitor2CRM : " + dtSource.Rows.Count.ToString + " Änderungen")
    StartProgress(dtSource.Rows.Count) : Result = dtSource.Rows.Count
    ' ---------------------------------------------------------------------
    For Each Row As dsPrins.VW_CRM_DEBITOR_EXPRow In dtSource.Rows
      If Row.IsDEBITORNUMMERNull Then Continue For
      ' ---------------------------------------------------------------------
      taDebitor.FillByDebitorNr(dtTarget, Row.DEBITORNUMMER)
      If dtTarget.Rows.Count = 0 Then
        TargetRow = dtTarget.NewDebitorRow
        InsertCount += 1
      Else
        TargetRow = dtTarget.Rows(0)
        UpdateCount += 1
      End If
      ' ----------------------------------------------------------------
      CopyDataRow(Row, TargetRow)
      If TargetRow.RowState = DataRowState.Detached Then
        dtTarget.AddDebitorRow(TargetRow)
      End If
      ' ----------------------------------------------------------------
      taDebitor.Update(dtTarget)
      StepProgress()
    Next
    ' ---------------------------------------------------------------------
    DisplayMessage("Eingefügt : " + InsertCount.ToString + ", geändert : " + UpdateCount.ToString)
    ' ---------------------------------------------------------------------
    TaggCommand = "DELETE FROM TBL_CRM_STAGEING CS WHERE CS.SOURCE_TABLE='KUNDEN' AND CS.TAGGED=1"
    taVW_CRM_DEBITOR_EXP.ExecuteNonQuery(TaggCommand)
    ' ---------------------------------------------------------------------
    EndProgress()

    Return Result
  End Function


  Private Function TransferProjekt2CRM() As Integer
    Dim Result As Integer = 0
    Dim dtSource As dsPrins.VW_CRM_PROJEKT_EXPDataTable = DsPrins1.VW_CRM_PROJEKT_EXP
    Dim dtTarget As dsCRMStageing.ProjektDataTable = dsCRMStageing1.Projekt
    Dim TargetRow As dsCRMStageing.ProjektRow
    Dim UpdateCount As Integer = 0
    Dim InsertCount As Integer = 0
    Dim TaggCommand As String = "UPDATE TBL_CRM_STAGEING CS SET CS.TAGGED = 1 WHERE CS.SOURCE_TABLE='PROJEKT'"

    taVW_CRM_PROJEKT_EXP.ExecuteNonQuery(TaggCommand)
    taVW_CRM_PROJEKT_EXP.Fill(dtSource)
    dtTarget.Clear()
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferProjekt2CRM : " + dtSource.Rows.Count.ToString + " Änderungen")
    StartProgress(dtSource.Rows.Count) : Result = dtSource.Rows.Count
    ' ---------------------------------------------------------------------
    For Each Row As dsPrins.VW_CRM_PROJEKT_EXPRow In dtSource.Rows
      If Row.IsPROJEKTNUMMERNull OrElse Row.IsKOSTENSTELLENull Then Continue For
      ' ---------------------------------------------------------------------
      taProjekt.FillByPrjNummerKst(dtTarget, Row.PROJEKTNUMMER, Row.KOSTENSTELLE)
      If dtTarget.Rows.Count = 0 Then
        TargetRow = dtTarget.NewProjektRow
        InsertCount += 1
      Else
        TargetRow = dtTarget.Rows(0)
        UpdateCount += 1
      End If
      ' ----------------------------------------------------------------
      CopyDataRow(Row, TargetRow)
      If TargetRow.RowState = DataRowState.Detached Then
        dtTarget.AddProjektRow(TargetRow)
      End If
      ' ----------------------------------------------------------------
      taProjekt.Update(dtTarget)
      StepProgress()
    Next
    ' ---------------------------------------------------------------------
    DisplayMessage("Eingefügt : " + InsertCount.ToString + ", geändert : " + UpdateCount.ToString)
    EndProgress()
    ' ---------------------------------------------------------------------
    TaggCommand = "DELETE FROM TBL_CRM_STAGEING CS WHERE CS.SOURCE_TABLE='PROJEKT' AND CS.TAGGED=1"
    taVW_CRM_DEBITOR_EXP.ExecuteNonQuery(TaggCommand)
    ' ---------------------------------------------------------------------
    EndProgress()

    Return Result
  End Function

  'TransferUserAccounts2CRM
  Private Function TransferUserAccounts2CRM() As Integer
    Dim Result As Integer = 0
    Dim dtSource As dsPrins3Central.VW_USER_ACCOUNTSDataTable = DsPrins3Central.VW_USER_ACCOUNTS
    Dim dtTarget As dsCRMStageing.HPC_MitarbeiterDataTable = dsCRMStageing1.HPC_Mitarbeiter
    Dim TargetRow As dsCRMStageing.HPC_MitarbeiterRow
    Dim tblAccounts As dsPrins3Central.TBL_USER_ACCOUNTSDataTable = DsPrins3Central.TBL_USER_ACCOUNTS
    Dim AccountRow As dsPrins3Central.TBL_USER_ACCOUNTSRow
    Dim UpdateCount As Integer = 0
    Dim InsertCount As Integer = 0

    ' ---------------------------------------------------------------------
    If Debugger.IsAttached Then
      '2655
      taVW_USER_ACCOUNTS.FillByID(dtSource, 2655)
    Else
      taVW_USER_ACCOUNTS.Fill(dtSource)
    End If
    ' ---------------------------------------------------------------------
    dtTarget.Clear()
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferUserAccounts2CRM : " + dtSource.Rows.Count.ToString + " Änderungen")
    StartProgress(dtSource.Rows.Count) : Result = dtSource.Rows.Count
    ' ---------------------------------------------------------------------
    For Each Row As dsPrins3Central.VW_USER_ACCOUNTSRow In dtSource.Rows
      ' ---------------------------------------------------------------------
      taHPC_Mitarbeiter.FillByPrinsID(dtTarget, Row.PRINS_ID)
      ' ---------------------------------------------------------------------
      If dtTarget.Rows.Count = 0 Then
        TargetRow = dtTarget.NewHPC_MitarbeiterRow
        InsertCount += 1
      Else
        TargetRow = dtTarget.Rows(0)
        UpdateCount += 1
      End If
      ' ----------------------------------------------------------------
      CopyDataRow(Row, TargetRow)
      TargetRow.HasChanged = True
      If TargetRow.RowState = DataRowState.Detached Then
        dtTarget.AddHPC_MitarbeiterRow(TargetRow)
      End If
      ' ----------------------------------------------------------------
      taHPC_Mitarbeiter.Update(dtTarget)
      ' ----------------------------------------------------------------
      tblAccounts.TableAdapter.FillByID(tblAccounts, Row.ID)
      If tblAccounts.Rows.Count = 1 Then
        AccountRow = tblAccounts.Rows(0)
        AccountRow.IS_CHANGED = False
        tblAccounts.SaveChanges()
      End If
      ' ----------------------------------------------------------------
      StepProgress()
    Next
    ' ---------------------------------------------------------------------
    DisplayMessage("Eingefügt : " + InsertCount.ToString + ", geändert : " + UpdateCount.ToString)
    ' ---------------------------------------------------------------------
    EndProgress()

    Return Result
  End Function

  Private Function TransferPeriodenabrechnung2CRM() As Integer
    Dim Result As Integer = 0
    Dim dtSource As dsPrins.VW_CRM_PERIODENABR_EXPDataTable = DsPrins1.VW_CRM_PERIODENABR_EXP
    Dim dtTarget As dsCRMStageing.PeriodenabrechnungDataTable = dsCRMStageing1.Periodenabrechnung
    Dim TargetRow As dsCRMStageing.PeriodenabrechnungRow
    Dim UpdateCount As Integer = 0
    Dim InsertCount As Integer = 0
    Dim TaggCommand As String = "UPDATE TBL_CRM_STAGEING CS SET CS.TAGGED = 1 WHERE CS.SOURCE_TABLE='MONATSABRECHNUNG_PERIODE'"

    taVW_CRM_PROJEKT_EXP.ExecuteNonQuery(TaggCommand)
    taVW_CRM_PERIODENABR_EXP.Fill(dtSource)

    dtTarget.Clear()
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferPeriodenabrechnung2CRM : " + dtSource.Rows.Count.ToString + " Änderungen")
    StartProgress(dtSource.Rows.Count) : Result = dtSource.Rows.Count
    ' ---------------------------------------------------------------------
    For Each Row As dsPrins.VW_CRM_PERIODENABR_EXPRow In dtSource.Rows
      If Row.IsPERIODENull OrElse Row.IsPRJ_NUMMERNull OrElse Row.IsKOSTENSTELLENull Then Continue For
      ' ---------------------------------------------------------------------
      taPeriodenabrechnung.FillByPKN(dtTarget, Row.PERIODE, Row.PRJ_NUMMER, Row.KOSTENSTELLE)
      If dtTarget.Rows.Count = 0 Then
        TargetRow = dtTarget.NewPeriodenabrechnungRow
        InsertCount += 1
      Else
        TargetRow = dtTarget.Rows(0)
        UpdateCount += 1
      End If
      ' ----------------------------------------------------------------
      If Row.PRJ_NUMMER = 2152196 Then
        Debug.WriteLine("Brechpunkt")
      End If

      CopyDataRow(Row, TargetRow)
      If TargetRow.RowState = DataRowState.Detached Then
        dtTarget.AddPeriodenabrechnungRow(TargetRow)
      End If
      ' ----------------------------------------------------------------
      taPeriodenabrechnung.Update(dtTarget)
      StepProgress()
    Next
    ' ---------------------------------------------------------------------
    DisplayMessage("Eingefügt : " + InsertCount.ToString + ", geändert : " + UpdateCount.ToString)
    ' ---------------------------------------------------------------------
    TaggCommand = "DELETE FROM TBL_CRM_STAGEING CS WHERE CS.SOURCE_TABLE='MONATSABRECHNUNG_PERIODE' AND CS.TAGGED=1"
    taVW_CRM_DEBITOR_EXP.ExecuteNonQuery(TaggCommand)
    ' ---------------------------------------------------------------------
    EndProgress()

    Return Result
  End Function


  Private Sub CopyDataRow(Source As DataRow, Target As DataRow)

    Target.BeginEdit()

    For Each Col As DataColumn In Target.Table.Columns
      If Source.Table.Columns.Contains(Col.ColumnName) AndAlso Not Col.ReadOnly Then

        Try
          If Source.IsNull(Col.ColumnName) Then
            Target(Col) = Global.System.Convert.DBNull
          Else
            If Col.DataType Is GetType(String) Then
              Target(Col) = Me.StringMaxLength(Source(Col.ColumnName), Col.MaxLength)
            Else
              Target(Col) = Source(Col.ColumnName)
            End If
          End If
        Catch ex As Exception
          Debug.WriteLine(ex.Message)
        End Try
      Else
        If Not Col.ReadOnly Then
          Debug.WriteLine("Target: " + Col.ColumnName + " - nicht im Source enthalten")
        End If
      End If
    Next

    Target.EndEdit()
  End Sub

  Private Sub StartProgress(MaxCount As Integer)
    iProgress1.Visibility = C1.Win.C1InputPanel.Visibility.Visible
    iProgress1.Value = 0
    iProgress1.Step = 1
    iProgress1.Maximum = MaxCount
    '------------------------------------------------------------------------------------
  End Sub

  Private Sub StepProgress()
    iProgress1.PerformStep() : inpTop.Update()
    'Application.DoEvents()
  End Sub

  Private Sub EndProgress()
    iProgress1.Visibility = C1.Win.C1InputPanel.Visibility.Hidden
  End Sub

  Private Function StringMaxLength([Value] As Object, Optional MaxLength As Integer = -1) As String
    Dim Result As String = String.Empty

    If Not IsDBNull(Value) AndAlso Not IsNothing(Value) Then

      Try
        Result = Value.ToString
      Catch ex As Exception
      End Try

    End If

    Result = Result.Trim

    If MaxLength > 0 Then
      Result = Result.Substring(0, If(Result.Length > MaxLength, MaxLength, Result.Length))
    End If

    Return Result
  End Function


  Private Sub ibAutoStart_PressedChanged(sender As Object, e As EventArgs) Handles ibAutoStart.PressedChanged
    My.Settings.btnAutoExecPressed = ibAutoStart.Pressed
    tmAutoExec.Enabled = ibAutoStart.Pressed
    ' -------------------------------------------------------------------------------------------
    ButtonsEnabled = Not ibAutoStart.Pressed
  End Sub

  Private Sub tmAutoExec_Tick(sender As Object, e As EventArgs) Handles tmAutoExec.Tick
    My.Application.RestartApplicationOnException = True
    tmAutoExec.Stop()
    AutoExecute()
    My.Application.RestartApplicationOnException = False
    tmAutoExec.Start()
  End Sub

  Private _ADReadAbort As Boolean = False

  Private Sub ibADRead_Click(sender As Object, e As EventArgs) Handles ibADRead.Click

    If IsNothing(ibADRead.Tag) OrElse Not CBool(ibADRead.Tag) Then
      Try
        ButtonsEnabled = False
        ibADRead.Text = "AD lesen abbrechen"
        ibADRead.Tag = True
        _ADReadAbort = False

        ReadADUsers()

      Finally
        ibADRead.Text = "AD lesen"
        ibADRead.Tag = False
        _ADReadAbort = False
        ButtonsEnabled = True
      End Try
    Else
      _ADReadAbort = True
      DisplayMessage("AD Lesen abgebrochen")
    End If

  End Sub

  'Private Sub CRM2Connect()

  '  Dim ConMan As New Microsoft.Xrm.Tooling.CrmConnectControl.CrmConnectionManager
  '  ConMan.HomeRealmServersList.Items.Add()

  '  If ConMan.RequireUserLogin Then

  '  End If


  'End Sub


  Private Function ReadADUsers() As Integer
    Dim Result As Integer = 0
    Dim Domains As ArrayList = GetSubDomains()
    Dim tblAccounts As dsPrins3Central.TBL_USER_ACCOUNTSDataTable = DsPrins3Central.TBL_USER_ACCOUNTS

    If Not Debugger.IsAttached Then
      tblAccounts.TableAdapter.Fill(tblAccounts)
    End If

    DisplayMessage("Geladen TBL_USER_ACCOUNTS - " + tblAccounts.Rows.Count.ToString + " Datensätze")
    StartProgress(tblAccounts.Rows.Count)

    For Each Domain As String In Domains
      If Debugger.IsAttached Then Domain = "eu.hpc.local"

      Dim entry As New System.DirectoryServices.DirectoryEntry("LDAP://" + Domain)
      Dim mySearcher As New System.DirectoryServices.DirectorySearcher(entry)
      'mySearcher.Filter = ("(objectClass=user)")
      'mySearcher.Filter = ("(sAMAccountType=805306368)")
      mySearcher.Filter = "(&(objectCategory=User)(objectClass=user)(sAMAccountType=805306368))"  'All user objects

      'mySearcher.Filter = "(&(objectCategory=Person)(sAMAccountname=SGRAN))"  'All user objects
      '"(!(userAccountControl:1.2.840.113556.1.4.803:=2)))" ' nur aktive Konten

      Console.WriteLine("Working Users in : " + Domain)

      For Each resEnt As System.DirectoryServices.SearchResult In mySearcher.FindAll()
        Try
          Dim de As System.DirectoryServices.DirectoryEntry = resEnt.GetDirectoryEntry()

#Region "Debugging Output"


          'If Not IsNothing(de.Properties("DisplayName").Value) AndAlso
          '   Not IsNothing(de.Properties("mail").Value) AndAlso
          '   Not IsNothing(de.Properties("sAMAccountName").Value) AndAlso
          '   Not IsNothing(de.Properties("GivenName").Value) AndAlso
          '   Not IsNothing(de.Properties("sn").Value) AndAlso
          '   Not IsNothing(de.Properties("Company").Value) Then

          '  Console.WriteLine("sAMAccountName : " + de.Properties("sAMAccountName").Value.ToString())
          '  Console.WriteLine("Display Name   : " + de.Properties("DisplayName").Value.ToString())

          '  'Console.WriteLine("Email         : " + de.Properties("mail").Value.ToString())
          '  'Console.WriteLine("User Name     : " + de.Properties("sAMAccountName").Value.ToString())
          '  'Console.WriteLine("First Name    : " + de.Properties("GivenName").Value.ToString())
          '  'Console.WriteLine("Last Name     : " + de.Properties("sn").Value.ToString())
          '  'Console.WriteLine("Company       : " + de.Properties("Company").Value.ToString())

          '  UserList.AppendLine(String.Format(UserFmt, Domain,
          '                                    de.Properties("DisplayName").Value.ToString(),
          '                                    de.Properties("mail").Value.ToString(),
          '                                    de.Properties("sAMAccountName").Value.ToString(),
          '                                    de.Properties("mail").Value.ToString(),
          '                                    de.Properties("sn").Value.ToString(),
          '                                    de.Properties("Company").Value.ToString()))

          '  'Console.WriteLine("===========    End of user : " + de.Properties("sAMAccountName").Value + " ===========")
          '  RealUserCount += 1
          'Else
          '  Console.WriteLine("===========    Skipping : " + de.Properties("sAMAccountName").Value + " ===========")
          '  SkippedUserCount += 1
          'End If

          ''If Not IsNothing(de.Properties("DisplayName").Value) Then Console.WriteLine("Display Name  : " + de.Properties("DisplayName").Value.ToString())
          ''If Not IsNothing(de.Properties("mail").Value) Then Console.WriteLine("Email         : " + de.Properties("mail").Value.ToString())
          ''If Not IsNothing(de.Properties("Title").Value) Then Console.WriteLine("Title         : " + de.Properties("Title").Value.ToString())
          ''If Not IsNothing(de.Properties("sAMAccountName").Value) Then Console.WriteLine("User Name     : " + de.Properties("sAMAccountName").Value.ToString())
          ''If Not IsNothing(de.Properties("GivenName").Value) Then Console.WriteLine("First Name    : " + de.Properties("GivenName").Value.ToString())
          ''If Not IsNothing(de.Properties("sn").Value) Then Console.WriteLine("Last Name     : " + de.Properties("sn").Value.ToString())
          ''If Not IsNothing(de.Properties("Initials").Value) Then Console.WriteLine("Initials      : " + de.Properties("Initials").Value.ToString())
          ''If Not IsNothing(de.Properties("Company").Value) Then Console.WriteLine("Company       : " + de.Properties("Company").Value.ToString())
          ''If Not IsNothing(de.Properties("Department").Value) Then Console.WriteLine("Department    : " + de.Properties("Department").Value.ToString())
          ''If Not IsNothing(de.Properties("TelephoneNumber").Value) Then Console.WriteLine("Telephone No. : " + de.Properties("TelephoneNumber").Value.ToString())

          'Console.WriteLine("===========    All Properties of " + de.Properties("DisplayName").Value)
          'For Each pprty As String In de.Properties.PropertyNames
          '  If Not IsNothing(de.Properties(pprty).Value) Then
          '    Console.WriteLine(pprty + " : " + de.Properties(pprty).Value.ToString())
          '    If IsNothing(pprty) Then Exit For ' Nur Brechpunkt
          '  End If
          'Next

          'Console.WriteLine("===========    All Properties : " + de.Properties("sAMAccountName").Value + " ===========")
#End Region

          ' ------------------------------------------------------------------------------------
          InsertOrUpdateAccount(Domain, resEnt, de)
          Result += 1
          ' ------------------------------------------------------------------------------------
          ' Abbruch ermöglichen
          Application.DoEvents()
          If _ADReadAbort Then Exit For
          ' ------------------------------------------------------------------------------------
        Catch ex As Exception
          DisplayMessage(ex.Message.Replace(ControlChars.CrLf, " - "))
        End Try

      Next
      If _ADReadAbort Then Exit For
    Next

    EndProgress()

    'Console.WriteLine("===================================")
    'Console.WriteLine("")
    'Console.WriteLine("RealUserCount    : " + RealUserCount.ToString)
    'Console.WriteLine("SkippedUserCount : " + SkippedUserCount.ToString)
    Return Result
  End Function


  Private Sub InsertOrUpdateAccount(DomainName As String, ResultEntry As System.DirectoryServices.SearchResult, DirEntry As System.DirectoryServices.DirectoryEntry)
    If IsNothing(DirEntry) OrElse Not DirEntry.Properties.Contains("SamAccountName") Then Exit Sub
    ' --------------------------------------------------------------------------
    Dim tblAccounts As dsPrins3Central.TBL_USER_ACCOUNTSDataTable = DsPrins3Central.TBL_USER_ACCOUNTS
    Dim UARow As dsPrins3Central.TBL_USER_ACCOUNTSRow = Nothing
    Dim AccountName As String = DirEntry.Properties("SamAccountName").Value
    Dim ObjectSID As String = My.Application.ByteToHexString(DirEntry.Properties("ObjectSID").Value)
    Dim sIDHistory As String = Nothing
    Dim RowChanged As Boolean = False
    Dim UASamAccount As String = String.Empty

    If Not IsNothing(DirEntry.Properties("sIDHistory").Value) Then
      sIDHistory = My.Application.ByteToHexString(If(DirEntry.Properties("sIDHistory").Value.GetType Is GetType(Object()), DirEntry.Properties("sIDHistory").Value(0), DirEntry.Properties("sIDHistory").Value))
    End If

    If AccountName.ToUpper = "NJ" Then
      Debug.WriteLine(AccountName)
    End If
    '
    ' -----------------------------------------------------------------------------
    ' NEU: wir suchen den Benutzer nach der SID
    tblAccounts.TableAdapter.FillByObjectSIDHex(tblAccounts, ObjectSID)
    If tblAccounts.Rows.Count = 1 Then
      UARow = tblAccounts.Rows(0)
    ElseIf Not String.IsNullOrEmpty(sIDHistory) Then
      'sIDHistory
      tblAccounts.TableAdapter.FillByObjectSIDHex(tblAccounts, sIDHistory)
    End If
    ' -----------------------------------------------------------------------------
    If IsNothing(UARow) Then
      ' nur für Übergang von AccountName auf SID

      Try
        tblAccounts.TableAdapter.FillBySamAccountDomain(tblAccounts, AccountName, DomainName)
      Catch ex As Exception
        DisplayMessage(ex.Message)
      End Try

      If tblAccounts.Rows.Count = 1 Then
        UARow = tblAccounts.Rows(0)
      Else
        UARow = tblAccounts.NewTBL_USER_ACCOUNTSRow
      End If
    End If
    ' -----------------------------------------------------------------------------
    UARow.BeginEdit()
    ' -----------------------------------------------------------------------------
    Dim HasDN As Boolean = False

    UASamAccount = UARow.AD_SAMACCOUNTNAME

    For Each pprty As String In DirEntry.Properties.PropertyNames
      Dim ColName As String = "AD_" + pprty.ToUpper
      If Not tblAccounts.Columns.Contains(ColName) Then Continue For
      ' -------------------------------------------------------------------------
      If ColName.ToUpper = "AD_DISTINGUISHEDNAME" Then HasDN = True
      ' -------------------------------------------------------------------------
      If Not IsNothing(DirEntry.Properties(pprty).Value) Then
        Console.WriteLine(pprty + " : " + DirEntry.Properties(pprty).Value.ToString())

        Try
          If pprty = "lastLogon" OrElse pprty = "accountExpires" Then
            Dim LongTimeStamp As Long = 0

            If ResultEntry.Properties(If(pprty = "lastLogon", "lastLogonTimestamp", pprty)).Count > 0 Then
              LongTimeStamp = ResultEntry.Properties(If(pprty = "lastLogon", "lastLogonTimestamp", pprty))(0)
            End If

            If LongTimeStamp > 0 AndAlso LongTimeStamp < Long.MaxValue Then
              UARow(ColName) = Date.FromFileTime(LongTimeStamp)
            Else
              UARow(ColName) = DBNull.Value
            End If

          ElseIf pprty.ToLower = "manager" Then
            Dim ma As String = DirEntry.Properties(pprty).Value
            Dim sp1 As String() = ma.Split(",")

            If sp1.Length > 0 AndAlso sp1(0).StartsWith("CN=") Then
              RowChanged = RowChanged Or SetRowValueIfChanged(UARow, ColName, sp1(0).Substring(3))
            End If
          Else
            If ColName = "AD_LOGONCOUNT" Then
              UARow(ColName) = DirEntry.Properties(pprty).Value
            Else
              RowChanged = RowChanged Or SetRowValueIfChanged(UARow, ColName, DirEntry.Properties(pprty).Value)

              If ColName = "AD_OBJECTSID" Then
                SetRowValueIfChanged(UARow, "AD_OBJECTSID_HEX", My.Application.ByteToHexString(UARow.AD_OBJECTSID))
              End If
            End If
          End If

        Catch ex As Exception
          Console.WriteLine(ex.Message)
        End Try

      Else
        RowChanged = RowChanged Or SetRowValueIfChanged(UARow, ColName, System.DBNull.Value)
      End If
    Next
    ' -----------------------------------------------------------------------------
    If Not HasDN Then
      If ResultEntry.Properties("DISTINGUISHEDNAME").Count > 0 Then
        RowChanged = RowChanged Or SetRowValueIfChanged(UARow, "AD_DISTINGUISHEDNAME", ResultEntry.Properties("DISTINGUISHEDNAME")(0).ToString)
      End If
    End If
    ' -----------------------------------------------------------------------------
    RowChanged = RowChanged Or SetRowValueIfChanged(UARow, "IS_ACTIVE", CBool(Not (UARow.AD_USERACCOUNTCONTROL And clActiveDirectory.UserAccountControlFlagsEnum.ACCOUNTDISABLE) > 0))
    RowChanged = RowChanged Or SetRowValueIfChanged(UARow, "PASSWD_NEVER_EXPIRES", CBool(UARow.AD_USERACCOUNTCONTROL And clActiveDirectory.UserAccountControlFlagsEnum.DONT_EXPIRE_PASSWORD))
    RowChanged = RowChanged Or SetRowValueIfChanged(UARow, "IS_NORMAL_ACCOUNT", CBool(UARow.AD_USERACCOUNTCONTROL And clActiveDirectory.UserAccountControlFlagsEnum.NORMAL_ACCOUNT))
    RowChanged = RowChanged Or SetRowValueIfChanged(UARow, "USER_DOMAIN", DomainName)
    RowChanged = RowChanged Or SetRowValueIfChanged(UARow, "SPRACHE", If(UARow.IsAD_CNull OrElse UARow.AD_C = "DE" OrElse UARow.AD_C = "AT", "deutsch", "englisch"))
    UARow.LAST_AD_READ = Now ' kein Changed merken
    ' -----------------------------------------------------------------------------
    Dim taPERSONAL As dsPrinsTableAdapters.PERSONALTableAdapter = GetPERSONLTableAdapter(If(Not UARow.IsAD_COMPANYNull, UARow.AD_COMPANY, String.Empty))

    If Not IsNothing(taPERSONAL) Then
      ' Nach ObjectSID suchen (neu ab 2020)
      taPERSONAL.FillByObjectSIDHex(DsPrins1.PERSONAL, ObjectSID)
      ' -----------------------------------------------------------------------------
      ' Wenn ObjectSID nicht da, dann nach USERNAME
      If DsPrins1.PERSONAL.Rows.Count = 0 Then
        taPERSONAL.FillByKuerzel(DsPrins1.PERSONAL, AccountName.ToUpper)
      End If
      ' -----------------------------------------------------------------------------
      If DsPrins1.PERSONAL.Rows.Count = 1 Then
        Dim PersRow As dsPrins.PERSONALRow = DsPrins1.PERSONAL.Rows(0)

        RowChanged = RowChanged Or SetRowValueIfChanged(UARow, "TBL_PRINS_PERSONAL_ID", PersRow.SAP_NUMMER)
        RowChanged = RowChanged Or SetRowValueIfChanged(UARow, "PRINS_USER_KST", PersRow.KOSTENSTELLE)
        RowChanged = RowChanged Or SetRowValueIfChanged(UARow, "ANREDE", PersRow.ANREDE)
        ' -----------------------------------------------------------------------------
        Dim Idx As Integer = vwKOSTENSTELLENByNummer.Find(PersRow.KOSTENSTELLE)
        If Idx > -1 Then
          Dim KstRow As dsPrins.KOSTENSTELLENRow = vwKOSTENSTELLENByNummer(Idx).Row
          If Not KstRow.IsFIRMENNUMMERNull Then
            RowChanged = RowChanged Or SetRowValueIfChanged(UARow, "FIRMENNUMMER", KstRow.FIRMENNUMMER)
          End If
        End If
        ' -----------------------------------------------------------------------------
        If PersRow.IsAD_OBJECTSID_HEXNull OrElse
          PersRow.AD_OBJECTSID_HEX <> ObjectSID Then PersRow.AD_OBJECTSID_HEX = ObjectSID
        ' -----------------------------------------------------------------------------
        If Not UARow.IS_ACTIVE AndAlso PersRow.AKTIV = "1" Then PersRow.AKTIV = "0"
        If PersRow.AKTIV = "1" Then
          Try
            SetRowValueIfChanged(PersRow, "KUERZEL", UARow.AD_SAMACCOUNTNAME.ToUpper)
            SetRowValueIfChanged(PersRow, "NACHNAME", UARow.AD_SN)
            SetRowValueIfChanged(PersRow, "VORNAME", UARow.AD_GIVENNAME)
            SetRowValueIfChanged(PersRow, "NAME", UARow.AD_DISPLAYNAME)
            If Not UARow.IsAD_MAILNull Then
              SetRowValueIfChanged(PersRow, "EMAIL", UARow.AD_MAIL.ToLower)
            End If
          Catch ex As Exception
            Dim tmp As String = $"SamAccount[{UASamAccount}] - Fehler: {ex.Message}"
            Throw New Exception(tmp)
          End Try

        End If
        ' -----------------------------------------------------------------------------
        If PersRow.RowState = DataRowState.Modified Then
          taPERSONAL.Update(PersRow)
        End If
      End If
    End If
    ' -----------------------------------------------------------------------------
    UARow.IS_CHANGED = RowChanged
    UARow.WARNINGS = DoAccountChecks(UARow)
    UARow.EndEdit()
    If UARow.RowState = DataRowState.Detached Then
      tblAccounts.AddTBL_USER_ACCOUNTSRow(UARow)
    End If
    ' -----------------------------------------------------------------------------
    tblAccounts.SaveChanges()
    'DisplayMessage(If(UARow.IsAD_DISPLAYNAMENull, String.Empty, UARow.AD_DISPLAYNAME) + " (" + UARow.ID.ToString + ") gelesen. IsChanged = " + UARow.IS_CHANGED.ToString + ", LAST_AD_READ = " + UARow.LAST_AD_READ.ToShortDateString + " " + UARow.LAST_AD_READ.ToLocalTime)
    ' -----------------------------------------------------------------------------
    If tblAccounts.HasErrors Then
      Dim ErrRow As DataRow = tblAccounts.GetErrors(0)
      Throw New Exception($"SamAccount[{UASamAccount}] - Fehler: {ErrRow.RowError}")
    End If

    StepProgress()
  End Sub


  Private _PERSONLTableAdapters As Hashtable
  Private ReadOnly Property PERSONLTableAdapters As Hashtable
    Get
      If IsNothing(_PERSONLTableAdapters) Then
        _PERSONLTableAdapters = New Hashtable
      End If

      Return _PERSONLTableAdapters
    End Get
  End Property


  Private Function GetPERSONLTableAdapter(Company As String) As dsPrinsTableAdapters.PERSONALTableAdapter
    Dim Result As dsPrinsTableAdapters.PERSONALTableAdapter = Nothing
    Dim MyCo As String = Company.ToUpper
    Dim Co As String = String.Empty
    ' ------------------------------------------------------------------------
    If MyCo.IndexOf("AUSTRIA") > -1 Then
      Co = "AT"
    ElseIf MyCo.IndexOf("ENVIROTEC") > -1 Then
      Co = "FR"
    ElseIf MyCo.IndexOf("ITALIA") > -1 Then
      Co = "IT"
    ElseIf MyCo.Replace(" ", "").IndexOf("HPCAG") > -1 Then
      Co = "DE"
    ElseIf MyCo.IndexOf("GEYSER") > -1 Then
      Co = "ES"
    ElseIf MyCo.IndexOf("INTERNATIONAL") > -1 Then
      Co = "INT"
    End If
    ' ------------------------------------------------------------------------
    If Not String.IsNullOrEmpty(Co) Then
      If PERSONLTableAdapters.Contains(Co) Then
        Result = PERSONLTableAdapters(Co)
      Else
        Result = New dsPrinsTableAdapters.PERSONALTableAdapter

        Select Case Co
          Case "AT"
            Result.Connection = New FirebirdSql.Data.FirebirdClient.FbConnection(My.Settings.PrinsATConnection)
          Case "FR"
            Result.Connection = New FirebirdSql.Data.FirebirdClient.FbConnection(My.Settings.PrinsFRConnection)
          Case "IT"
            Result.Connection = New FirebirdSql.Data.FirebirdClient.FbConnection(My.Settings.PrinsITConnection)
          Case "DE"
            Result.Connection = New FirebirdSql.Data.FirebirdClient.FbConnection(My.Settings.PrinsDEConnection)
          Case "ES"
            Result.Connection = New FirebirdSql.Data.FirebirdClient.FbConnection(My.Settings.PrinsESConnection)
          Case "INT"
            Result.Connection = New FirebirdSql.Data.FirebirdClient.FbConnection(My.Settings.PrinsINTConnection)
          Case Else
        End Select
        ' ------------------------------------------------------------------------
        PERSONLTableAdapters(Co) = Result
      End If
    End If
    ' ------------------------------------------------------------------------
    Return Result
  End Function



  Private Function DoAccountChecks(Row As dsPrins3Central.TBL_USER_ACCOUNTSRow) As String
    Dim Warnings As New System.Text.StringBuilder
    'Dim NeededADFields As String() = {"AD_COMPANY", "AD_DISPLAYNAME", "AD_MAIL", "AD_C", "AD_CO", "AD_CN", "AD_STREETADDRESS", "AD_TELEPHONENUMBER", "AD_GIVENNAME", "FIRMENNUMMER", "ANREDE", "PRINS_USER_KST", "TBL_PRINS_PERSONAL_ID", "AD_POSTALCODE"}
    Dim NeededADFields As String() = {"AD_COMPANY", "AD_DISPLAYNAME", "AD_MAIL", "AD_C", "AD_CO", "AD_CN", "AD_STREETADDRESS", "AD_TELEPHONENUMBER", "AD_GIVENNAME", "AD_POSTALCODE"}

    If Row.IsAD_LASTLOGONNull AndAlso (Today - Row.AD_WHENCREATED).TotalDays > 30 Then
      Warnings.AppendLine("Kein Login. Erstellt vor mehr als 30 Tagen")
    End If
    ' -----------------------------------------------------------------------------
    If Row.PASSWD_NEVER_EXPIRES Then
      Warnings.AppendLine("Passwort läuft nicht ab")
    End If
    ' -----------------------------------------------------------------------------
    If Not Row.IsAD_LASTLOGONNull AndAlso (Today - Row.AD_LASTLOGON).TotalDays > 30 Then
      Warnings.AppendLine("Letzter Login vor mehr als 30 Tagen")
    End If
    ' -----------------------------------------------------------------------------
    If Row.IS_ACTIVE AndAlso Not Row.IsAD_ACCOUNTEXPIRESNull AndAlso Row.AD_ACCOUNTEXPIRES <= Today Then
      Warnings.AppendLine("Benutzerkonto abgelaufen (expired) aber aktiv")
    End If
    ' -----------------------------------------------------------------------------
    For i As Integer = 0 To NeededADFields.Length - 1
      Dim ColName As String = NeededADFields(i)

      If Row.IsNull(ColName) OrElse String.IsNullOrEmpty(Row(ColName).ToString) Then
        Warnings.AppendLine(ColName + " ist leer")
      End If
    Next

    Return Warnings.ToString
  End Function

  Private Function SetRowValueIfChanged(Row As DataRow, ColName As String, [Value] As Object) As Boolean
    Return My.Application.SetRowValueIfChanged(Row, ColName, Value)
  End Function


  Private Function GetSubDomains() As ArrayList
    Dim Result As New ArrayList
    'Dim context As New PrincipalContext(ContextType.Domain)
    'Dim Filter As New GroupPrincipal(context)

    'Filter.IsSecurityGroup = True

    'Dim searcher As New PrincipalSearcher(Filter)
    'Dim results = searcher.FindAll

    'For Each group As GroupPrincipal In results
    Dim path As String = "LDAP://rootDSE"
    Dim searchRoot As DirectoryEntry = New DirectoryEntry(path)
    Dim configNC As String = searchRoot.Properties("configurationNamingContext").Value.ToString()
    Dim configSearchRoot As DirectoryEntry = New DirectoryEntry("LDAP://" + configNC)
    Dim configSearch As DirectorySearcher = New DirectorySearcher(configSearchRoot)

    configSearch.Filter = "(NETBIOSName=*)"
    configSearch.PropertiesToLoad.Add("dnsroot")
    configSearch.PropertiesToLoad.Add("ncname")
    configSearch.PropertiesToLoad.Add("NETBIOSName")
    Dim forestPartitionList As SearchResultCollection = configSearch.FindAll()

    For Each domainPartition As SearchResult In forestPartitionList
      Dim ncname As String = domainPartition.Properties("ncname")(0).ToString()
      Dim dnsroot As String = domainPartition.Properties("dnsroot")(0).ToString()
      Dim netBIOSName As String = domainPartition.Properties("NETBIOSName")(0).ToString()

      Console.WriteLine("dnsroot:      " + dnsroot)
      'Console.WriteLine("ncname:      " + ncname)
      'Console.WriteLine("netBIOSName: " + netBIOSName)
      Result.Add(dnsroot.ToLower)
    Next

    Return Result
  End Function

  Private Sub ucCRMExchanger_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged

    If _FirstLoaded Then
      If Not IsNothing(iProgress1.Tag) Then
        inpTop.Width = iProgress1.Width - CInt(iProgress1.Tag)
      End If
    End If

  End Sub

  Private _CookieContainer As CookieContainer
  Private _HttpClient As HttpClient
  Private _HttpClientHandler As HttpClientHandler

  Private ReadOnly Property HttpClient As HttpClient
    Get
      If IsNothing(_HttpClient) Then
        _CookieContainer = New CookieContainer
        _HttpClientHandler = New HttpClientHandler
        _HttpClientHandler.CookieContainer = _CookieContainer
        _HttpClient = New HttpClient(_HttpClientHandler)
        _HttpClient.Timeout = TimeSpan.FromSeconds(60)
      End If

      Return _HttpClient
    End Get
  End Property


  Public Sub UploadDataInBackground(ByVal address As String, ByVal Data As Byte())

    Dim uri As Uri = New Uri(address)

  End Sub


  Private Enum UploadFileCallbackStatus
    Ok = 0
    Failed = -1
    Unknown = 1
  End Enum


  Private _UploadStatus As UploadFileCallbackStatus
  Private Sub UploadDataCallback(ByVal sender As Object, ByVal e As System.Net.UploadDataCompletedEventArgs)
    If Not IsNothing(e.Error) Then
      Dim Msg As String = "UploadDataCallback Error : " + e.Error.Message
      Debug.WriteLine(Msg)
      _UploadStatus = UploadFileCallbackStatus.Failed
    Else
      Dim Result As String = "UploadDataCallback Result : " + System.Text.Encoding.UTF8.GetString(e.Result)
      _UploadStatus = UploadFileCallbackStatus.Ok
    End If
  End Sub

  'TransferMitarbeiter2ARM
  Private Function TransferMitarbeiter2ARM(ArmURL As String) As Integer
    Dim Result As Integer = 0
    Dim JsonTemp As String = System.Text.Encoding.UTF8.GetString(My.Resources.ArmMitarbeiter)
    Dim MaString As String
    Dim httpResponse As HttpResponseMessage

    taMitarbeiter.FillByNotTransferrd(dsCRMStageing1.Mitarbeiter)
    If dsCRMStageing1.Mitarbeiter.Count = 0 Then Return Result
    ' ----------------------------------------------------------
    UcOutlookMail.ResetOutlookApp()
    SetupMailbox()
    ' ----------------------------------------------------------
    Dim loginUri As String = My.Settings.ARMAuthData
    Dim idx As Integer = ArmURL.IndexOf("/", 9)
    Dim BaseUrl As String = ArmURL.Substring(0, idx)

    ArmURL = ArmURL.Substring(idx)

    HttpClient.BaseAddress = New Uri(BaseUrl)

    httpResponse = HttpClient.GetAsync(loginUri).Result
    Console.WriteLine(httpResponse.StatusCode)
    If httpResponse.StatusCode = HttpStatusCode.OK Then
      DisplayMessage($"Verbindung hergestellt: {BaseUrl}")
    Else
      DisplayMessage($"Fehler [{httpResponse.StatusCode.ToString}] bei Verbindung : {BaseUrl}")
      Return Result
    End If
    ' ----------------------------------------------------------
    For Each Row As dsCRMStageing.MitarbeiterRow In dsCRMStageing1.Mitarbeiter.Rows
      MaString = System.Text.Encoding.UTF8.GetString(My.Resources.ArmMitarbeiterShema)
      Dim MainError As Boolean = False

      If Row.Isprimaerer_arbeitsortNull OrElse String.IsNullOrEmpty(Row.primaerer_arbeitsort) Then
        Row.result = $"{Row.nachname}: Arbeitsort nicht gesetzt! "
        MainError = True
      End If

      If Row.IsvornameNull OrElse String.IsNullOrEmpty(Row.vorname) Then
        Row.result = $"{Row.nachname}: Vorname nicht gesetzt!"
        MainError = True
      End If

      If Row.IsvorgesetzterNull OrElse String.IsNullOrEmpty(Row.vorgesetzter) Then
        Row.result = $"{Row.nachname}: Vorgesetzter nicht gesetzt!"
        MainError = True
      End If

      If String.IsNullOrEmpty(Row.nachname) Then
        Row.result = $"{Row.nachname}: Nachname nicht gesetzt!"
        MainError = True
      End If

      If Row.IskuerzelNull OrElse String.IsNullOrEmpty(Row.kuerzel) Then
        Row.result = $"{Row.nachname}: Kuerzel nicht gesetzt!"
        MainError = True
      End If

      If Row.Isstart_datumNull OrElse Row.start_datum < Today.AddDays(-30) Then
        Row.result = $"{Row.nachname}: Startdatum vor dem [{Today.AddDays(-30).ToShortDateString}]!"
        MainError = True
      End If
      ' ---------------------------------------------------------------------------
      If MainError Then
        DisplayMessage(Row.result)
        Row.transferred = True ' nicht wieder anfassen
        SendADUserExistsMail(Row)
        Continue For
      End If
      ' ---------------------------------------------------------------------------
      Dim tblAccounts As dsPrins3Central.TBL_USER_ACCOUNTSDataTable = DsPrins3Central.TBL_USER_ACCOUNTS
      Dim IsInAD As Boolean = False

      tblAccounts.TableAdapter.FillBySamAccount(tblAccounts, Row.kuerzel)
      ' Account ist vorhanden. aufgrund von Domänen-Wechsel kann er auch mehrfach da sein
      If tblAccounts.Rows.Count >= 1 Then
        For Each UARow As dsPrins3Central.TBL_USER_ACCOUNTSRow In tblAccounts.Rows
          ' Account wurde innerhalb der letzten 48 Stunden gelesen. Ist also aktiv
          If (Now - UARow.LAST_AD_READ).TotalHours < 48 Then
            Row.result = $"Benutzerkürzel [{Row.kuerzel}] im AD vorhanden!"
            IsInAD = True
          End If
        Next
      End If
      ' ---------------------------------------------------------------------------
      ' Vor- & Nachnamen exakt suchen. (ohne Groß/Klein)
      If Not IsInAD Then
        tblAccounts.TableAdapter.FillByVorNachname(tblAccounts, Row.nachname, Row.vorname)
        If tblAccounts.Rows.Count > 0 Then
          Row.result = $"Kombination aus Vorname [{Row.vorname}] und Nachname [{Row.nachname}] gefunden!"
          IsInAD = True
        End If
      End If
      ' ---------------------------------------------------------------------------
      If Not IsInAD Then
        ' Vor- & Nachnamen LIKE in GIVENNAME. (ohne Groß/Klein)
        tblAccounts.TableAdapter.FillByLikeVorNachname(tblAccounts, $"%{Row.nachname}%", $"%{Row.vorname}%")
        If tblAccounts.Rows.Count > 0 Then
          Row.result = $"Vorname [{Row.vorname}] und Nachname [{Row.nachname}] in AD_GIVENNAME enthalten!"
          IsInAD = True
        End If
      End If
      ' ---------------------------------------------------------------------------
      If IsInAD Then
        Row.transferred = True ' nicht wieder anfassen
        SendADUserExistsMail(Row)
        Continue For
      End If
      ' ---------------------------------------------------------------------------
      MaString = MaString.Replace("<Mode>", "execute")
      MaString = MaString.Replace("<Comment>", "Created by HPC CRMConnect")
      MaString = MaString.Replace("<givenname>", Row.vorname)
      MaString = MaString.Replace("<sn>", Row.nachname)
      MaString = MaString.Replace("<samaccountname>", Row.kuerzel)
      MaString = MaString.Replace("<l>", Row.primaerer_arbeitsort)
      ' ---------------------------------------------------------------------------
      If Not Row.IsstellenbezeichnungNull AndAlso Not String.IsNullOrEmpty(Row.stellenbezeichnung) Then
        MaString = MaString.Replace("<extensionAttribute4>", Row.stellenbezeichnung)
      Else
        MaString = MaString.Replace("<extensionAttribute4>", "")
      End If
      ' ---------------------------------------------------------------------------
      If Not Row.IsvorgesetzterNull AndAlso Not String.IsNullOrEmpty(Row.vorgesetzter) Then

        tblAccounts.TableAdapter.FillByADDisplayName(tblAccounts, Row.vorgesetzter.Trim)

        If tblAccounts.Rows.Count = 1 Then
          Dim UARow As dsPrins3Central.TBL_USER_ACCOUNTSRow = tblAccounts.Rows(0)
          If Not UARow.IsAD_DISTINGUISHEDNAMENull AndAlso Not String.IsNullOrEmpty(UARow.AD_DISTINGUISHEDNAME) Then
            MaString = MaString.Replace("<manager>", UARow.AD_DISTINGUISHEDNAME)
            Row.vorgesetzter = UARow.AD_DISTINGUISHEDNAME
          Else
            DisplayMessage($"{Row.guid.ToString}: Manager [{Row.vorgesetzter.Trim}] AD_DISTINGUISHEDNAME nicht gesetzt!")
          End If
        Else
          DisplayMessage($"{Row.guid.ToString}: Manager [{Row.vorgesetzter.Trim}] nicht gefunden!")
        End If
      End If
      ' ---------------------------------------------------------------------------
      MaString = MaString.Replace("<manager>", "")
      ' ---------------------------------------------------------------------------
      Dim s As New System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(MaString))
      Dim Content As New System.Net.Http.StreamContent(s) 'System.Net.Http.Json.JsonContent 

      Content.Headers.ContentType = New Headers.MediaTypeHeaderValue("application/json")
      Content.Headers.ContentType.CharSet = System.Text.Encoding.UTF8.WebName

      httpResponse = HttpClient.GetAsync(ArmURL).Result
      httpResponse = HttpClient.PostAsync(ArmURL, Content).Result

      If httpResponse.StatusCode = HttpStatusCode.OK Then
        Row.transferred = True
        Row.result = String.Empty
        Result += 1
      End If
    Next
    ' -----------------------------------------------------------
    Dim dsChanges As DataTable = dsCRMStageing1.Mitarbeiter.GetChanges
    If Not IsNothing(dsChanges) Then taMitarbeiter.Update(dsChanges)
    ' -----------------------------------------------------------
    httpResponse = HttpClient.GetAsync("/Session/logout").Result

    If httpResponse.StatusCode = HttpStatusCode.OK Then
      DisplayMessage($"Logout erfolgreich : {HttpClient.BaseAddress.ToString}")
    Else
      DisplayMessage($"Fehler [{httpResponse.StatusCode.ToString}] bei Logout : {HttpClient.BaseAddress.ToString}")
    End If
    ' -----------------------------------------------------------
    _HttpClientHandler = Nothing
    _CookieContainer = Nothing
    _HttpClient = Nothing
    ' -----------------------------------------------------------

    Return Result
  End Function

  Private Sub SendADUserExistsMail(Row As dsCRMStageing.MitarbeiterRow)
    If cbEntwurf.SelectedIndex = -1 Then
      DisplayMessage("Kein Mail-Template für 'SendADUserExistsMail' ausgewählt!")
      Exit Sub
    End If
    ' ---------------------------------------------------------------------------------
    Dim TempMail As Outlook.MailItem = TryCast(cbEntwurf.SelectedOption.Tag, Outlook.MailItem)
    Dim Mail As Outlook.MailItem = Nothing
    If IsNothing(TempMail) Then Exit Sub
    '-----------------------------------------------------------------------------------------
    Dim HTMLBody As String = ReplaceTemplateCols(Row, TempMail.HTMLBody)
    Dim Subject As String = ReplaceTemplateCols(Row, TempMail.Subject)
    '-----------------------------------------------------------------------------------------
    Mail = UcOutlookMail.NewMailItem
    Mail.HTMLBody = HTMLBody
    Mail.Subject = Subject
    Mail.To = If(Debugger.IsAttached, "str@hpc.ag", "it.services@hpc.ag")
    Mail.CC = ""
    '-----------------------------------------------------------------------------------------
    DisplayMessage($"Mail [{Mail.Subject}] gesendet an: {Mail.To}")
    '-----------------------------------------------------------------------------------------
    Mail.Send()

  End Sub

  Private Function ReplaceTemplateCols(Row As DataRow, TemplateText As String) As String
    Dim Result As String = TemplateText
    '-----------------------------------------------------------------------------------------
    For Each Col As DataColumn In Row.Table.Columns
      If Not Row.IsNull(Col) Then
        Result = Result.Replace("%" + Col.ColumnName.ToUpper + "%", Row(Col).ToString)
      End If
    Next
    '-----------------------------------------------------------------------------------------
    Return Result
  End Function


  Private Sub ibAzureBusQ_Click(sender As Object, e As EventArgs) Handles ibAzureBusQ.Click
    If FOJournal.AzureBus(JournalTypeEnum.VendorInvoice).ReceiveMessagesStarted Then
      AzureBusServiceStop()
    Else
      AzureBusServiceStart()
    End If
  End Sub

  Private Sub AzureBusServiceStart()
    If Not FOJournal.AzureBus(JournalTypeEnum.VendorInvoice).ReceiveMessagesStarted Then
      AddHandler FOJournal.AzureBus(JournalTypeEnum.VendorInvoice).AzureDataReceived, AddressOf OnVendorInvoiceMessageReceive
      FOJournal.AzureBus(JournalTypeEnum.VendorInvoice).StartReceiveMessages()
      DisplayMessage($"Now listening on AzureBus [{FOJournal.AzureBus(JournalTypeEnum.VendorInvoice).QueueName}]")
    End If
    ' ------------------------------------------------------------------
    If Not FOErrors.AzureBus(JournalTypeEnum.Errors).ReceiveMessagesStarted Then
      AddHandler FOErrors.AzureBus(JournalTypeEnum.Errors).AzureDataReceived, AddressOf OnErrorMessageReceive
      FOErrors.AzureBus(JournalTypeEnum.Errors).StartReceiveMessages()
      DisplayMessage($"Now listening on AzureBus [{FOErrors.AzureBus(JournalTypeEnum.Errors).QueueName}]")
    End If
    ' ------------------------------------------------------------------
    If Not FOSuccess.AzureBus(JournalTypeEnum.CustomerInvoiceSuccess).ReceiveMessagesStarted Then
      AddHandler FOSuccess.AzureBus(JournalTypeEnum.CustomerInvoiceSuccess).AzureDataReceived, AddressOf OnCustomerInvoiceSuccessMessageReceive
      FOSuccess.AzureBus(JournalTypeEnum.CustomerInvoiceSuccess).StartReceiveMessages()
      DisplayMessage($"Now listening on AzureBus [{FOSuccess.AzureBus(JournalTypeEnum.CustomerInvoiceSuccess).QueueName}]")
    End If
    ' ------------------------------------------------------------------
    My.Application.Swap(ibAzureBusQ.Text, ibAzureBusQ.Tag)
    ' ------------------------------------------------------------------
    My.Settings.AzureBusServiceAutoStart = FOJournal.AzureBus(JournalTypeEnum.VendorInvoice).ReceiveMessagesStarted AndAlso
                                           FOErrors.AzureBus(JournalTypeEnum.Errors).ReceiveMessagesStarted AndAlso
                                           FOSuccess.AzureBus(JournalTypeEnum.CustomerInvoiceSuccess).ReceiveMessagesStarted

  End Sub

  Private _AzureBusServiceStopping As Boolean = False

  Private Async Sub AzureBusServiceStop()
    If _AzureBusServiceStopping Then Exit Sub
    ' -------------------------------------------------------
    Try
      _AzureBusServiceStopping = True

      If FOJournal.AzureBus(JournalTypeEnum.VendorInvoice).ReceiveMessagesStarted Then
        Await FOJournal.AzureBus(JournalTypeEnum.VendorInvoice).StopReceiveMessages()
        RemoveHandler FOJournal.AzureBus(JournalTypeEnum.VendorInvoice).AzureDataReceived, AddressOf OnVendorInvoiceMessageReceive
      End If
      ' ------------------------------------------------------------------
      'FOErrors.NewJournal(JournalTypeEnum.Errors)
      If FOErrors.AzureBus(JournalTypeEnum.Errors).ReceiveMessagesStarted Then
        Await FOErrors.AzureBus(JournalTypeEnum.Errors).StopReceiveMessages()
        RemoveHandler FOErrors.AzureBus(JournalTypeEnum.Errors).AzureDataReceived, AddressOf OnErrorMessageReceive
      End If
      ' ------------------------------------------------------------------
      My.Application.Swap(ibAzureBusQ.Text, ibAzureBusQ.Tag)
      DisplayMessage("Listening on AzureBus stopped!")
      My.Settings.AzureBusServiceAutoStart = False
    Finally
      _AzureBusServiceStopping = False
    End Try

  End Sub



  'Dim Utf8Bytes() As Byte = Nothing
  'Dim base64Out() As Byte = Nothing
  'Dim base64String As String
  'Dim base64char() As Char = Nothing

  '    Utf8Bytes = System.Text.Encoding.UTF8.GetBytes(MessageString)
  '    base64String = System.Convert.ToBase64String(Utf8Bytes)

  '    Array.Resize(base64char, base64String.Length)
  '    System.Convert.ToBase64CharArray(Utf8Bytes, 0, Utf8Bytes.Length, base64char, 0)

  '    Array.Resize(base64Out, base64char.Length)

  '    For i As Integer = 0 To base64char.Length - 1
  '      base64Out(i) = System.Convert.ToByte(base64char(i))
  '    Next

  'Dim Message As New Message(base64Out)

  Private Sub OnErrorMessageReceive(Sender As Object, e As hpcXRMConnect.clAzureBus.AzureDataReceivedEventArgs)
    '44130ec29a7f3fd1ea3fca1bb56c674133f60d392fc754925e7c88009aff399ed1e0c376968e7f6e79f29cc6854e6b956e117157fb2dfd0ac41dfb8653e565549332e3ee1e3edb592de723423f9ca9db0b140c77f88c7b27bf40b4d6651b203d356a9cb3644083858b2155a85d135aea8a76303c15fd6f09f2b68657c5af47b1259c2b87a8bdbd388ad612af416314de8913613259ed424cdb4eb1c08b1db5b10dcc43a094b3b7b8b5669ac2b7f92cc678c008ab56d3cf50566b808687ddeba10fce457a8fb1ca10c44500161d60059383c1b4ebabce5ca25ba659825233a2eca1a238c88428662b9afeb252a521ed50c9aa053d0c57c0757b952d6e13e16cbb
    Dim Data As String = System.Text.Encoding.UTF8.GetString(e.MessageBody)
    Dim Data64 As String = Base64ToString(e.MessageBody)

    If Not FOErrors.JournalStarted Then FOErrors.NewJournal(JournalTypeEnum.Errors)
    FOErrors.LogJournalString(Data)
    ' --------------------------------------------------------------------
    Dim ErrorHdr As clGeneralLedger.ErrorMessage = FOErrors.JsonDeSerializeObject(Data, GetType(clGeneralLedger.ErrorMessage))
    Try
      ErrorHdr = FOErrors.JsonDeSerializeObject(Data, GetType(clGeneralLedger.ErrorMessage))
    Catch ex As Exception
    End Try

    If Not IsNothing(ErrorHdr) Then
      Dim InvoiceHeader As clGeneralLedger.FreeTextInvoiceHeader
      Try
        InvoiceHeader = FOErrors.JsonDeSerializeObject(ErrorHdr.data, GetType(clGeneralLedger.FreeTextInvoiceHeader))

        If Not IsNothing(InvoiceHeader) AndAlso
            Not String.IsNullOrEmpty(InvoiceHeader.HPCExternalLedgerVoucher) AndAlso
            Not String.IsNullOrEmpty(InvoiceHeader.ExternalInvoiceId) Then

          Dim Row As dsPrins.TBL_FO_FREETEXT_ERRORSRow
          ' ----------------------------------------------------------------------
          ' Suchen, ob da.
          DsPrins1.TBL_FO_FREETEXT_ERRORS.TableAdapter.FillByBelegNr(DsPrins1.TBL_FO_FREETEXT_ERRORS, InvoiceHeader.HPCExternalLedgerVoucher)
          'InvoiceHeader.HPCExternalLedgerVoucher ist UNIQUE KEY
          If DsPrins1.TBL_FO_FREETEXT_ERRORS.Rows.Count = 1 Then
            Row = DsPrins1.TBL_FO_FREETEXT_ERRORS.Rows(0)
            If Row.IsERROR_COUNTNull Then Row.ERROR_COUNT = 1
            Row.ERROR_COUNT += 1
          Else
            Row = DsPrins1.TBL_FO_FREETEXT_ERRORS.NewTBL_FO_FREETEXT_ERRORSRow
          End If
          ' ----------------------------------------------------------------------
          With InvoiceHeader
            Row.DOCUMENTDATE = .DocumentDate.ToShortDateString
            Row.HPCEXTERNALLEDGERVOUCHER = .HPCExternalLedgerVoucher
            Row.EXTERNALINVOICEID = .ExternalInvoiceId
            Row.INVOICEDATE = .InvoiceDate
            SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Error Ausgangsbeleg Nr.[{ .ExternalInvoiceId}] / Beleg : [{ .HPCExternalLedgerVoucher}]")
          End With
          ' ----------------------------------------------------------------------
          If Row.RowState = DataRowState.Detached Then DsPrins1.TBL_FO_FREETEXT_ERRORS.AddTBL_FO_FREETEXT_ERRORSRow(Row)
          DsPrins1.TBL_FO_FREETEXT_ERRORS.SaveChanges()
          DsPrins1.TBL_FO_FREETEXT_ERRORS.Clear()
        End If

      Catch ex As Exception
      End Try
    End If


  End Sub

  Private Function Base64ToString(MessageBody As Byte()) As String
    Dim Data As String
    Dim Utf8Bytes() As Byte = Nothing
    Dim base64char() As Char = Nothing
    ' -------------------------------------------------------------------------------
    Array.Resize(base64char, MessageBody.Length)
    For i As Integer = 0 To base64char.Length - 1
      base64char(i) = System.Convert.ToChar(MessageBody(i))
    Next
    ' -------------------------------------------------------------------------------
    Try
      Utf8Bytes = System.Convert.FromBase64CharArray(base64char, 0, base64char.Length)
      Data = System.Text.Encoding.UTF8.GetString(Utf8Bytes)
    Catch ex As Exception
      Data = Nothing
    End Try

    Return Data
  End Function


  Private Sub OnVendorInvoiceMessageReceive(Sender As Object, e As hpcXRMConnect.clAzureBus.AzureDataReceivedEventArgs)
    Dim Data As String = Base64ToString(e.MessageBody)

    ' wenn als JSON kommt und nicht Base64 encoded.
    If IsNothing(Data) OrElse (Data.Length > 0 AndAlso Data.Substring(0, 1) <> "{") Then ' JSON beginnt immer mit {
      Try
        Data = System.Text.Encoding.UTF8.GetString(e.MessageBody)
      Catch ex As Exception
        Data = Nothing
      End Try
    End If
    ' -------------------------------------------------------------------------
    If Not IsNothing(Data) Then
      If Not FOJournal.JournalStarted Then FOJournal.NewJournal(JournalTypeEnum.VendorInvoice)
      FOJournal.LogJournalString(Data)
      ' -------------------------------------------------------------------------
      Dim InvoiceHeader As clGeneralLedger.FO3rdPartyInvoiceHeader
      Try
        InvoiceHeader = FOJournal.JsonDeSerializeObject(Data, GetType(clGeneralLedger.FO3rdPartyInvoiceHeader))
        SyncContext.Post(New Threading.SendOrPostCallback(AddressOf UpdateInvoiceQueue), InvoiceHeader)
      Catch ex As Exception
      End Try
    End If
  End Sub

  Private Sub UpdateInvoiceQueue(Data As Object)
    DisplayMessage($"Lieferantenbeleg empfangfen")
    InvoiceQueue.Enqueue(Data)
    ReadInvoicesAsync()
  End Sub
  Private Sub bwInvoiceReader_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwInvoiceReader.RunWorkerCompleted
    ReadInvoicesAsync()
  End Sub

  Private Sub ReadInvoicesAsync()
    If InvoiceQueue.Count > 0 AndAlso Not bwInvoiceReader.IsBusy Then
      bwInvoiceReader.RunWorkerAsync(InvoiceQueue.Dequeue)
    End If
  End Sub

  Private Sub bwInvoiceReader_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwInvoiceReader.DoWork
    Dim InvoiceHeader As clGeneralLedger.FO3rdPartyInvoiceHeader = e.Argument
    Dim VlgRow As dsPrinsNET.VORLAGEN_ERFASSUNGRow
    Dim Result As Object

    SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Verarbeite Vorlagen-Beleg [{InvoiceHeader.LedgerVoucher}]")

    Try
      Result = FOInvoiceBooking.CreateInvoice(InvoiceHeader)
      If Not IsNothing(Result) Then
        VlgRow = TryCast(Result, dsPrinsNET.VORLAGEN_ERFASSUNGRow)
        ' ---------------------------------------------------------------------------------------------------------------
        If Not IsNothing(VlgRow) Then
          SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Vorlagen-Rechnung [{VlgRow.BELEG.ToString}] wurde angelegt")
        Else
          Dim ErrorMsg As String = TryCast(Result, String)
          If Not String.IsNullOrEmpty(ErrorMsg) Then
            SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Vorlagen-Rechnung ERROR [{ErrorMsg}]")
          Else
            SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Vorlagen-Rechnung unbekannter Fehler - Beleg [{InvoiceHeader.LedgerVoucher}]")
          End If
        End If
      End If
    Catch ex As Exception
      SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"bwInvoiceReader Error [{ex.Message}]")
    End Try

  End Sub


  'Private Sub CheckMaxVORLAGEN_ERFASSUNGRows()
  '  If bsVORLAGEN.Count > 500 Then
  '    Dim Row As DataRow = bsVORLAGEN(bsVORLAGEN.Count - 1).row
  '    Row.Delete()
  '    DsPrinsNET1.VORLAGEN_ERFASSUNG.AcceptChanges()
  '  End If
  'End Sub

  Private Sub OnCustomerInvoiceSuccessMessageReceive(Sender As Object, e As hpcXRMConnect.clAzureBus.AzureDataReceivedEventArgs)
    '44130ec29a7f3fd1ea3fca1bb56c674133f60d392fc754925e7c88009aff399ed1e0c376968e7f6e79f29cc6854e6b956e117157fb2dfd0ac41dfb8653e565549332e3ee1e3edb592de723423f9ca9db0b140c77f88c7b27bf40b4d6651b203d356a9cb3644083858b2155a85d135aea8a76303c15fd6f09f2b68657c5af47b1259c2b87a8bdbd388ad612af416314de8913613259ed424cdb4eb1c08b1db5b10dcc43a094b3b7b8b5669ac2b7f92cc678c008ab56d3cf50566b808687ddeba10fce457a8fb1ca10c44500161d60059383c1b4ebabce5ca25ba659825233a2eca1a238c88428662b9afeb252a521ed50c9aa053d0c57c0757b952d6e13e16cbb
    Dim Data As String = System.Text.Encoding.UTF8.GetString(e.MessageBody)
    Dim Data64 As String = Base64ToString(e.MessageBody)
    ' ------------------------------------------------------------------------------
    If (String.IsNullOrEmpty(Data) OrElse Not Data.StartsWith("{")) AndAlso
        (Not String.IsNullOrEmpty(Data64) AndAlso Data64.StartsWith("{")) Then
      Data = Data64
    End If
    ' ------------------------------------------------------------------------------
    If Not FOSuccess.JournalStarted Then FOSuccess.NewJournal(JournalTypeEnum.CustomerInvoiceSuccess)
    FOSuccess.LogJournalString(Data)
    ' ------------------------------------------------------------------------------
    Dim ihl As hpcFOExchange.clGeneralLedger.FreeTextInvoiceHeader = Nothing

    Try
      ihl = FOSuccess.JsonDeSerializeObject(Data, GetType(clGeneralLedger.FreeTextInvoiceHeader))
    Catch ex As Exception
    End Try
    ' ------------------------------------------------------------------------------
    If IsNothing(ihl) Then Exit Sub
    ' ------------------------------------------------------------------------------
    Dim dtRechnung As dsPrinsNET.RECHNUNGDataTable = FOSuccess.dsPrinsNET.RECHNUNG
    Dim BelegNr As Integer = -1
    ' ------------------------------------------------------------------------------
    Try
      BelegNr = CInt(ihl.HPCExternalLedgerVoucher)
    Catch ex As Exception
    End Try
    ' ------------------------------------------------------------------------------
    dtRechnung.TableAdapter.FillByBelegNummer(dtRechnung, BelegNr)
    If dtRechnung.Rows.Count = 1 Then
      Dim RgRow As dsPrinsNET.RECHNUNGRow = dtRechnung.Rows(0)
      RgRow.FO_TRANSFERRED = 1
      dtRechnung.SaveChanges()
      ' ------------------------------------------------------------------------------
      SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Ausgangsbeleg [{BelegNr.ToString}] wurde auf übertragen (F&O) gesetzt!")
    End If

  End Sub

  Private Sub bw_PrjFolder_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bw_PrjFolder.DoWork
    Dim dtFolders As dsPrins.TBL_PRJ_FOLDERSDataTable = bwDsPrins.TBL_PRJ_FOLDERS
    Dim RetParas As New Hashtable

    SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Prozess Projektordner-Anlage gestartet")

    Do 'While Not bw_PrjFolder.CancellationPending
      Do ' While False (nur für Exit)
        dtFolders.TableAdapter.FillByToExecute(dtFolders)
        If dtFolders.Rows.Count = 0 Then Exit Do
        ' ---------------------------------------------------------------------
        Dim Row As dsPrins.TBL_PRJ_FOLDERSRow
        Dim CleanedProjectPath As String
        Dim ProjectNo As String
        Dim YearCreated As Integer = -1

        For Each Row In dtFolders.Rows
          Dim Cmd As String = My.Settings.ProjectScript

          'powershell.exe -executionPolicy bypass -file C:\Install\Scripts\HPC-Projects\HPCAG\HPCProject_fromPrins.ps1 -Servername ":HOMESERVER" -ProjectNumber ":PRJ_NUMMER" -Projectname ":PRJ_NAME1" -Members ":Members"
          CleanedProjectPath = StripNonFilePathChars(Row.PRJ_NAME1).Trim
          ' ---------------------------------------------------------------------
          ProjectNo = Row.PRJ_NUMMER.ToString
          If Not Row.IsYEAR_CREATEDNull Then YearCreated = Row.YEAR_CREATED
          ' ---------------------------------------------------------------------
          If Not Row.IsCREATED_TSNull AndAlso (Now - Row.CREATED_TS).TotalMinutes < 5 Then
            ' Anlage vor weniger als 5 Minuten ... einen Zyklus warten, falls Änderungen erfolgen
            Continue For
          End If
          ' ---------------------------------------------------------------------
          ' mind 7 Stellen und falls der User z.B. nur "." eingibt ... 
          If ProjectNo.Length > 6 AndAlso CleanedProjectPath.Length > 3 Then
            If ProjectNo.Substring(0, 1) = "2" Then
              ProjectNo = ProjectNo.Substring(1)
              If CInt(ProjectNo.Substring(0, 2)) < 20 Then
                ' Alte PRJ Nummern nicht anlegen
                Row.DO_EXECUTE = False
                dtFolders.SaveChanges()
                Continue For
              End If
              ' ---------------------------------------------------------------------
              YearCreated = 2000 + CInt(ProjectNo.Substring(0, 2))
            ElseIf ProjectNo.Substring(0, 1) = "4" Then ' BP Projekte
              If YearCreated = -1 Then YearCreated = Now.Year
            Else
              ' Alte PRJ Nummern nicht anlegen
              Row.DO_EXECUTE = False
              dtFolders.SaveChanges()
              Continue For
            End If
          Else
            ' Alte PRJ Nummern nicht anlegen
            Row.DO_EXECUTE = False
            dtFolders.SaveChanges()
            Continue For
          End If
          ' ---------------------------------------------------------------------
          Cmd = Cmd.Replace(":PRJ_NUMMER", ProjectNo)
          Cmd = Cmd.Replace(":PRJ_NAME1", CleanedProjectPath)
          Cmd = Cmd.Replace(":HOMESERVER", Row.HOMESERVER)
          Cmd = Cmd.Replace(":Year", YearCreated.ToString)
          ' ---------------------------------------------------------------------
          Dim Members As String = ""
          Dim Loops As Integer = 2
          Dim sp As String()

          While Loops > 0
            sp = String.Empty.Split(",") '.Length=0
            If Loops = 2 AndAlso Not Row.IsKST_LSTNull Then sp = Row.KST_LST.Split(",")
            If Loops = 1 AndAlso Not Row.IsUSER_LSTNull Then sp = Row.USER_LST.Split(",")
            ' ---------------------------------------------------------------------
            For i As Integer = 0 To sp.Length - 1
              If String.IsNullOrEmpty(sp(i)) Then Continue For
              ' ---------------------------------------------------------------------
              If Members.Length > 0 Then Members += ","

              If IsNumeric(sp(i)) Then
                Members += $"EST-{sp(i)}"
              Else
                Members += $"{sp(i)}"
              End If
            Next
            ' ---------------------------------------------------------------------
            Loops -= 1
          End While
          ' ---------------------------------------------------------------------
          Cmd = Cmd.Replace(":Members", Members)
          ' ---------------------------------------------------------------------
          'Dim fn As String = System.IO.Path.Combine(System.IO.Path.GetTempPath, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString + ".cmd")
          'Dim fs As System.IO.FileStream
          'Dim Utf8Data As Byte() = System.Text.Encoding.Default.GetBytes(Cmd)

          'fs = System.IO.File.Open(fn, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None)
          'fs.Write(Utf8Data, 0, Utf8Data.Length)
          'fs.Close()
          ' ---------------------------------------------------------------------
          Dim ps As New System.Diagnostics.Process

          ps.StartInfo.FileName = "powershell.exe"
          ps.StartInfo.Arguments = Cmd
          ps.StartInfo.UseShellExecute = False
          ps.StartInfo.RedirectStandardOutput = True
          ps.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
          ps.Start()
          ' ---------------------------------------------------------------------
          Dim js As New Newtonsoft.Json.JsonTextReader(ps.StandardOutput)
          Dim PropertyName As String = String.Empty
          Dim PropertyValue As String = String.Empty

          RetParas.Clear()

          While js.Read()
            ' -------------------------------------------------------------------------
            If Not IsNothing(js.Value) Then
              If js.TokenType = Newtonsoft.Json.JsonToken.PropertyName Then
                PropertyName = js.Value
              ElseIf js.TokenType = Newtonsoft.Json.JsonToken.String Then
                PropertyValue = js.Value
              End If
            End If
            ' -------------------------------------------------------------------------
            If Not String.IsNullOrEmpty(PropertyValue) AndAlso Not String.IsNullOrEmpty(PropertyName) AndAlso
              Not RetParas.Contains(PropertyName) Then
              RetParas.Add(PropertyName, PropertyValue)
              PropertyName = String.Empty
              PropertyValue = String.Empty
            End If
            ' -------------------------------------------------------------------------
          End While
          ' ---------------------------------------------------------------------
          ps.WaitForExit()
          ' ---------------------------------------------------------------------
          ' Muss zurück kommen
          If RetParas.Contains("Path") Then
            Dim Value As String = RetParas("Path")

            Row.EXECUTED_TS = Now
            Row.EXEC_CMD = Cmd
            Row.EXEC_RESULT = Value
            Row.EXEC_SUCCESS = True
            Row.DO_EXECUTE = False
            Row.PRJ_PATH = Value.Substring(0, Value.LastIndexOf("\") + 1) + ProjectNo + " - " + CleanedProjectPath

            dtFolders.SaveChanges()
          End If
          ' ---------------------------------------------------------------------
          SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Projektpfad {Row.PRJ_PATH} angelegt/aktualisiert")
          ' ---------------------------------------------------------------------
          If bw_PrjFolder.CancellationPending Then Exit Do
        Next
        ' ---------------------------------------------------------------------
      Loop While False
      ' ---------------------------------------------------------------------
      For Min As Integer = 0 To 5 - 1
        For Sec As Integer = 0 To 60 - 1
          System.Threading.Thread.Sleep(1000)
          If bw_PrjFolder.CancellationPending Then Exit Do
        Next
      Next
      ' ---------------------------------------------------------------------
    Loop While Not bw_PrjFolder.CancellationPending

  End Sub

  Private Function StripNonFilePathChars(Value As String) As String
    Dim Result As String = String.Empty
    Dim ipc As Char() = System.IO.Path.GetInvalidPathChars
    Dim ifc As Char() = System.IO.Path.GetInvalidFileNameChars()

    For i As Integer = 0 To Value.Length - 1
      Dim c As Char = Value(i)

      If ipc.Contains(c) OrElse ifc.Contains(c) Then c = Chr(32)
      Result += c
    Next

    Return Result
  End Function

  Private _PrjFolderJobsActive As Boolean = False
  Private Property PrjFolderJobsActive As Boolean
    Get
      Return _PrjFolderJobsActive
    End Get
    Set(value As Boolean)
      _PrjFolderJobsActive = value
    End Set
  End Property

  Private Sub ibPrjFolder_Click(sender As Object, e As EventArgs) Handles ibPrjFolder.Click
    SwitchFolderJobOnOff()
  End Sub

  Private Sub ParentForm_FormClosing(sender As Object, e As FormClosingEventArgs)
    ' wenn beim Beenden aktiv, dann wieder starten
    My.Settings.PrjFolderAutoStart = PrjFolderJobsActive
    ' --------------------------------------------------------------
    ParentForm.Enabled = False
    ' --------------------------------------------------------------
    If PrjFolderJobsActive Then
      ' Cancel gracefully Background-Jobs
      SwitchFolderJobOnOff()
      ParentForm.Text = "Anwendung wird beendet ..."
      ' --------------------------------------------------------------
      While bw_PrjFolder.IsBusy OrElse bw_ESTGroups.IsBusy
        System.Threading.Thread.Sleep(100)
        Application.DoEvents()
      End While
      ' --------------------------------------------------------------
    End If
    ' --------------------------------------------------------------
    If bw_SetFOBuchung.IsBusy Then bw_SetFOBuchung.CancelAsync()
    ' --------------------------------------------------------------
    While bw_SetFOBuchung.IsBusy
      System.Threading.Thread.Sleep(100)
      Application.DoEvents()
    End While
    ' --------------------------------------------------------------

  End Sub

  Private Sub SwitchFolderJobOnOff()
    If Not PrjFolderJobsActive Then
      If Not bw_PrjFolder.IsBusy AndAlso Not bw_PrjFolder.CancellationPending AndAlso
         Not bw_ESTGroups.IsBusy AndAlso Not bw_ESTGroups.CancellationPending Then

        bw_PrjFolder.RunWorkerAsync()
        bw_ESTGroups.RunWorkerAsync()

        ibPrjFolder.Text = "Projektordner Anlage stoppen"
        PrjFolderJobsActive = True
      End If
    Else
      ibPrjFolder.Text = "POA wird gestoppt ..."
      If bw_PrjFolder.IsBusy AndAlso Not bw_PrjFolder.CancellationPending Then bw_PrjFolder.CancelAsync()
      If bw_ESTGroups.IsBusy AndAlso Not bw_ESTGroups.CancellationPending Then bw_ESTGroups.CancelAsync()

    End If

  End Sub


  Private Sub bw_ESTGroups_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bw_ESTGroups.DoWork
    Dim dtPersonal As dsPrins.PERSONALDataTable = bwDsPrins.PERSONAL
    Dim dtKostenstellen As dsPrins.KOSTENSTELLENDataTable = bwDsPrins.KOSTENSTELLEN
    Dim dtADGroups As dsPrins.TBL_AD_ESTGROUPSDataTable = bwDsPrins.TBL_AD_ESTGROUPS
    Dim vwKST As New DataView(dtKostenstellen)

    vwKST.Sort = "NUMMER"

    SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Prozess Überwachung EST Gruppen gestartet")

    Do ' Outer Loop while Not bw_ESTGroups.CancellationPending
      Do ' While False -- Exit do
        dtADGroups.TableAdapter.FillByDoExec(dtADGroups)
        If dtADGroups.Rows.Count = 0 Then Exit Do ' nix zu tun
        ' ---------------------------------------------------------------------
        Dim ADGroupRow As dsPrins.TBL_AD_ESTGROUPSRow
        Dim KSTRow As dsPrins.KOSTENSTELLENRow
        Dim PersRow As dsPrins.PERSONALRow

        dtPersonal.TableAdapter.FillByAktiv(dtPersonal)
        dtKostenstellen.TableAdapter.Fill(dtKostenstellen)

        For Each ADGroupRow In dtADGroups.Rows
          Dim idx As Integer
          Dim Cmd As String = My.Settings.ESTGroupScript
          Dim Members As String = ""

          ' ---------------------------------------------------------------------
          vwKST.RowFilter = String.Empty
          idx = vwKST.Find(ADGroupRow.KST_NUMMER)
          If idx = -1 Then Continue For ' darf nicht passieren
          KSTRow = vwKST(idx).Row
          ' ---------------------------------------------------------------------
          For Each PersRow In dtPersonal.Rows
            Dim AddUser As Boolean
            If PersRow.IsKOSTENSTELLENull Then Continue For
            If PersRow.AKTIV = "0" OrElse PersRow.KUERZEL.IndexOf("$") > -1 Then Continue For
            ' ---------------------------------------------------------------------
            AddUser = (KSTRow.NUMMER = PersRow.KOSTENSTELLE)
            ' ---------------------------------------------------------------------
            ' SammelKST?
            If Not KSTRow.IsSAMMELKOSTENSTELLENull AndAlso KSTRow.NUMMER = KSTRow.SAMMELKOSTENSTELLE Then
              vwKST.RowFilter = $"SAMMELKOSTENSTELLE={KSTRow.NUMMER}"
              AddUser = vwKST.Find(PersRow.KOSTENSTELLE) > -1
            End If
            ' ---------------------------------------------------------------------
            If AddUser Then
              If Members.Length > 0 Then Members += ","
              Members += PersRow.KUERZEL.ToUpper.ToString
            End If
          Next
          ' ---------------------------------------------------------------------
          ' Keine Member
          If Members.Length = 0 Then
            ADGroupRow.DO_EXECUTE = False
            dtADGroups.SaveChanges()
            Continue For
          End If
          ' ---------------------------------------------------------------------
          Cmd = Cmd.Replace(":Members", Members)
          Cmd = Cmd.Replace(":Groupname", $"EST-{ADGroupRow.KST_NUMMER}")
          ' ---------------------------------------------------------------------
          'Dim fn As String = System.IO.Path.Combine(System.IO.Path.GetTempPath, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString + ".cmd")
          'Dim fs As System.IO.FileStream
          'Dim Utf8Data As Byte() = System.Text.Encoding.Default.GetBytes(Cmd)

          'fs = System.IO.File.Open(fn, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None)
          'fs.Write(Utf8Data, 0, Utf8Data.Length)
          'fs.Close()
          ' ---------------------------------------------------------------------
          Dim ps As New System.Diagnostics.Process

          ps.StartInfo.FileName = "powershell.exe"
          ps.StartInfo.Arguments = Cmd
          ps.StartInfo.UseShellExecute = False
          ps.StartInfo.RedirectStandardOutput = True
          ps.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
          ps.Start()
          ' ---------------------------------------------------------------------
          Dim RetValue As String = ps.StandardOutput.ReadToEnd
          ' ---------------------------------------------------------------------
          ps.WaitForExit()
          ' ---------------------------------------------------------------------
          ' Muss zurück kommen
          If Not IsNothing(RetValue) Then

            ADGroupRow.EXECUTED_TS = Now
            ADGroupRow.EXEC_CMD = Cmd
            ADGroupRow.EXEC_RESULT = RetValue
            ADGroupRow.EXEC_SUCCESS = (RetValue = String.Empty)
            ADGroupRow.DO_EXECUTE = False

            dtADGroups.SaveChanges()
          End If
          ' ---------------------------------------------------------------------
          SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"EST-{ADGroupRow.KST_NUMMER.ToString} aktualisiert")
          ' ---------------------------------------------------------------------
          If bw_ESTGroups.CancellationPending Then Exit Do
        Next
        ' ---------------------------------------------------------------------
      Loop While False
      ' ---------------------------------------------------------------------
      For Min As Integer = 0 To 5 - 1
        For Sec As Integer = 0 To 60 - 1
          System.Threading.Thread.Sleep(1000)
          If bw_ESTGroups.CancellationPending Then Exit Do
        Next
      Next
      ' ---------------------------------------------------------------------
    Loop While Not bw_ESTGroups.CancellationPending

  End Sub

  Private Sub bw_PrjFolder_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bw_PrjFolder.RunWorkerCompleted
    If Not bw_ESTGroups.IsBusy Then
      ibPrjFolder.Text = "Projektordner Anlage starten"
      PrjFolderJobsActive = False
    End If
  End Sub

  Private Sub bw_ESTGroups_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bw_ESTGroups.RunWorkerCompleted
    If Not bw_PrjFolder.IsBusy Then
      ibPrjFolder.Text = "Projektordner Anlage starten"
      PrjFolderJobsActive = False
    End If
  End Sub

  Private Function TransferGeneralJournal() As Integer
    Dim Result As Integer = 0
    Dim taAzGeneralJournal As New dsSQLDataLakeTableAdapters.GeneralJournalAccountEntryStagingTableAdapter
    Dim dtAzGeneralJournal As dsSQLDataLake.GeneralJournalAccountEntryStagingDataTable = DsSQLDataLake.GeneralJournalAccountEntryStaging
    Dim dtAzJournalDiff As dsSQLDataLake.VW_GeneralJournalEntryDiffDataTable = DsSQLDataLake.VW_GeneralJournalEntryDiff
    Dim dtLocalGeneralJournal As dsPrins.TBL_GENERALJOURNALACCOUNTENTRYDataTable = DsPrins1.TBL_GENERALJOURNALACCOUNTENTRY
    Dim TargetRow As dsPrins.TBL_GENERALJOURNALACCOUNTENTRYRow

    ' ---------------------------------------------------------------------
    ' nie zur vollen Stunde ausführen nur zwischen 15 und 45 ... 
    ' die Übertragung F&O -> DataLake läuft immer zur xx:00 alle 4 Stunden
    If Now.Minute < 15 OrElse Now.Minute > 45 Then
      _LastTransferAt = DateTime.MinValue
      Return Result
    End If
    ' ---------------------------------------------------------------------
    dtLocalGeneralJournal.Clear()
    ' ---------------------------------------------------------------------
    dtAzJournalDiff.TableAdapter.Fill(dtAzJournalDiff)
    ' ---------------------------------------------------------------------
    DisplayMessage("TransferGeneralJournal : " + dtAzJournalDiff.Rows.Count.ToString + " Änderungen")
    StartProgress(dtAzJournalDiff.Rows.Count) : Result = dtAzJournalDiff.Rows.Count
    ' ---------------------------------------------------------------------
    Dim LastJournalNo As String = String.Empty

    For Each DiffRow As dsSQLDataLake.VW_GeneralJournalEntryDiffRow In dtAzJournalDiff.Rows
      If LastJournalNo = DiffRow.JOURNALNUMBER Then Continue For
      ' ---------------------------------------------------------------------
      taAzGeneralJournal.FillByJournalNumber(dtAzGeneralJournal, DiffRow.JOURNALNUMBER)
      LastJournalNo = DiffRow.JOURNALNUMBER
      ' ---------------------------------------------------------------------
      For Each Row As dsSQLDataLake.GeneralJournalAccountEntryStagingRow In dtAzGeneralJournal.Rows
        ' ---------------------------------------------------------------------
        TargetRow = dtLocalGeneralJournal.NewTBL_GENERALJOURNALACCOUNTENTRYRow
        ' ----------------------------------------------------------------
        CopyDataRow(Row, TargetRow)
        ' ----------------------------------------------------------------
        dtLocalGeneralJournal.AddTBL_GENERALJOURNALACCOUNTENTRYRow(TargetRow)
        ' ----------------------------------------------------------------
        StepProgress()
      Next
    Next
    ' ---------------------------------------------------------------------
    dtLocalGeneralJournal.TableAdapter.Update(dtLocalGeneralJournal)
    'dtAzJournalDiff.TableAdapter.ExecuteNonQuery(My.Resources.SQL_DeleteAzGeneralJournalNo)
    dtAzJournalDiff.TableAdapter.ExecuteNonQuery(My.Resources.SQL_InsertAzGeneralJournalNo)
    ' ---------------------------------------------------------------------
    If Result > 0 Then
      DisplayMessage($"TransferGeneralJournal : {Result.ToString} eingefügt")
    End If
    ' ---------------------------------------------------------------------
    EndProgress()
    ' ---------------------------------------------------------------------
    taAzGeneralJournal.Dispose()
    taAzGeneralJournal = Nothing
    ' ---------------------------------------------------------------------
    Return Result
  End Function

  Private Sub bw_SetFOBuchung_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bw_SetFOBuchung.DoWork
    Dim dsPrinsNET As New hpcFOExchange.dsPrinsNET
    Dim taRECHNUNG As New hpcFOExchange.dsPrinsNETTableAdapters.RECHNUNGTableAdapter
    Dim dtRECHNUNG As hpcFOExchange.dsPrinsNET.RECHNUNGDataTable = dsPrinsNET.RECHNUNG
    Dim taAzGeneralJournal As New dsSQLDataLakeTableAdapters.GeneralJournalAccountEntryStagingTableAdapter
    Dim dtGeneralJournal As dsSQLDataLake.GeneralJournalAccountEntryStagingDataTable
    Dim MinWait As Integer

    'hpcFOExchange.clGeneralLedger()

    SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Prozess F&O Buchungsstatus setzen gestartet")

    Do 'While Not bw_PrjFolder.CancellationPending
      Do ' exit do
        Dim NoFOFound As Boolean = True
        MinWait = 15
        ' nur zwiscen xx:30 und xx:50 starten
        If Not (Now.Minute >= 30 AndAlso Now.Minute <= 50) Then Exit Do
        ' ---------------------------------------------------------------------
        Try
          taRECHNUNG.FillByFOTransferred(dtRECHNUNG)
          For Each Row As hpcFOExchange.dsPrinsNET.RECHNUNGRow In dtRECHNUNG.Rows
            If Row.IsEXTERNALINVOICEIDNull Then Continue For

            dtGeneralJournal = taAzGeneralJournal.GetDataByDocumentNo(Row.EXTERNALINVOICEID)
            If dtGeneralJournal.Rows.Count > 0 Then
              Row.FO_BUCHUNG = 1
              Row.FO_TRANSFERRED = 1
              NoFOFound = False
              ' ------------------------------------------------------------------------
              SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"Ausgangsbeleg [{Row.EXTERNALINVOICEID}] auf gebucht gesetzt!")
              ' ------------------------------------------------------------------------
            End If
          Next
          ' ---------------------------------------------------------------------
          dtRECHNUNG.SaveChanges()
        Catch ex As Exception
          SyncContext.Post(New Threading.SendOrPostCallback(AddressOf DisplayMessage), $"bw_SetFOBuchung_DoWork Exception: {ex.Message}")
        End Try
        ' ---------------------------------------------------------------------
        If NoFOFound Then MinWait = (60 * 4) - 15 ' Datalake wird alle 4 Stunden aktualisiert
      Loop While False
      ' ---------------------------------------------------------------------
      For Min As Integer = 0 To MinWait - 1
        For Sec As Integer = 0 To 60 - 1
          System.Threading.Thread.Sleep(1000)
          If bw_SetFOBuchung.CancellationPending Then Exit Do
        Next
      Next
      ' ---------------------------------------------------------------------
    Loop While Not bw_SetFOBuchung.CancellationPending

  End Sub
End Class
