Public Class frmMain
  Private Loaded As Boolean = False

  Public Sub New()
    ' -------------------------------------------------------------------------
    ' Dieser Aufruf ist für den Designer erforderlich.
    ' -------------------------------------------------------------------------
    InitializeComponent()
    ' -------------------------------------------------------------------------
    Dim uc As UserControl
    UserControls.Add(CInt(ucControls.FirmaListe), New ucFirma)
    UserControls.Add(CInt(ucControls.ProjektListe), New ucProjekt)
    UserControls.Add(CInt(ucControls.KundenPrins), New ucKunden2CRM)
    UserControls.Add(CInt(ucControls.Logging), New ucLogging)
    UserControls.Add(CInt(ucControls.CRMExchanger), New ucCRMExchanger)
    UserControls.Add(CInt(ucControls.Restruct2Prins), New ucRestruct2Prins)

    Me.pnlWorkspace.SuspendLayout()

    For Each Key As Integer In UserControls.Keys
      uc = UserControls(Key)
      uc.Visible = False
      Me.pnlWorkspace.Controls.Add(uc)
      uc.Dock = DockStyle.Fill
    Next
    ' --------------------------------------------------------------------------------------------------------------
    Me.pnlWorkspace.ResumeLayout()
  End Sub


  Public Enum ucControls
    FirmaListe = 0
    ProjektListe = 1
    KundenPrins = 2
    CRMExchanger = 100
    Restruct2Prins = 200
    Logging = 3
  End Enum


  Private _dsGenerator As DataSet
  Private ReadOnly Property dsGenerator As DataSet
    Get
      If IsNothing(_dsGenerator) Then
        _dsGenerator = New DataSet
      End If
      Return _dsGenerator
    End Get
  End Property


  Public ReadOnly Property GetGeneratorValue(GeneratorName As String) As Integer
    Get
      Dim Result As Integer = -1
      Dim Sql As String = String.Format("SELECT MAX(RDB$RELATION_ID) AS ID, GEN_ID( {0}, {1}) AS NEW_ID FROM RDB$DATABASE ", GeneratorName, 1)
      Dim da As IDbDataAdapter = Me.taGENERATOR_ID.Adapter

      If IsNothing(da.SelectCommand) Then
        da.SelectCommand = New FirebirdSql.Data.FirebirdClient.FbCommand(Sql, taGENERATOR_ID.Connection)
      Else
        da.SelectCommand.CommandText = Sql
      End If

      da.SelectCommand.Connection = Me.taGENERATOR_ID.Connection
      If dsGenerator.Tables.Count > 0 Then dsGenerator.Tables(0).Clear()
      da.Fill(dsGenerator)
      If dsGenerator.Tables.Count > 0 AndAlso dsGenerator.Tables(0).Rows.Count = 1 Then
        Result = dsGenerator.Tables(0).Rows(0)("NEW_ID")
      End If

      Return Result
    End Get
  End Property



  Public ReadOnly Property ActiveRecordEvents As clRecordEvents
    Get
      Dim Result As clRecordEvents = Nothing

      If Not IsNothing(ActiveUserControl) Then
        Dim intf As ifRecordEvent
        intf = TryCast(ActiveUserControl, ifRecordEvent)
        If Not IsNothing(intf) Then Result = intf.ActiveRecordEvents
      End If

      Return Result
    End Get
  End Property

  Private _InfoMessageForm As frmInfoMsg
  Public ReadOnly Property InfoMessageForm As frmInfoMsg
    Get
      If IsNothing(_InfoMessageForm) Then
        _InfoMessageForm = New frmInfoMsg
        _InfoMessageForm.TopMost = True
      End If
      Return _InfoMessageForm
    End Get
  End Property

  Private _UserControls As Hashtable
  Public ReadOnly Property UserControls As Hashtable
    Get
      If IsNothing(_UserControls) Then
        _UserControls = New Hashtable
      End If
      Return _UserControls
    End Get
  End Property

  Private _ActiveUserControl As UserControl
  Public Property ActiveUserControl As UserControl
    Set(value As UserControl)
      _ActiveUserControl = value
    End Set
    Get
      Return _ActiveUserControl
    End Get
  End Property

  Private _ActiveUserControlIndex As Integer = -1
  Public Property ActiveUserControlIndex As Integer
    Set(value As Integer)
      _ActiveUserControlIndex = value
      ActivateUserControl(_ActiveUserControlIndex)
      My.Settings.frmMain_ActiveUserControlIndex = _ActiveUserControlIndex
    End Set
    Get
      Return _ActiveUserControlIndex
    End Get
  End Property

  Public Sub ActivateUserControl(Index As Integer)
    Dim uc As UserControl = Me.UserControls(Index)
    ' -----------------------------------------------------------------------------
    For i As Integer = 0 To Me.pnlWorkspace.Controls.Count - 1
      Me.pnlWorkspace.Controls(i).Visible = False
    Next
    ' -----------------------------------------------------------------------------
    uc.Visible = True
    uc.Dock = DockStyle.Fill
    _ActiveUserControl = uc
    uc.Focus()
    ' -----------------------------------------------------------------------------
  End Sub

  Public WriteOnly Property InfoErrorMessage As String
    Set(value As String)
      Me.InfoMessageForm.Message = value.Replace("'nulls'", "<leer>")

      Me.InfoMessageForm.Width = Me.ActiveUserControl.Width
      Me.InfoMessageForm.Location = Me.PointToScreen(New Point(Me.pnlWorkspace.Left, Me.pnlWorkspace.Top + Me.pnlWorkspace.Height - Me.InfoMessageForm.Height))

      Me.InfoMessageForm.Show()
      Me.tmrMessage.Start()
    End Set
  End Property

  Private Sub tmrMessage_Tick(sender As Object, e As EventArgs) Handles tmrMessage.Tick
    Me.tmrMessage.Stop()
    Me.InfoMessageForm.Visible = False
  End Sub

  Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    ' --------------------------------------------------------------------------------------------------------------
    If My.Settings.frmMain_ActiveUserControlIndex > -1 Then
      Me.ActiveUserControlIndex = My.Settings.frmMain_ActiveUserControlIndex
    Else
      Me.ActiveUserControlIndex = ucControls.CRMExchanger
    End If
    ' --------------------------------------------------------------------------------------------------------------
    If My.Settings.frmMain_WindowState > -1 Then
      If My.Settings.frmMain_WindowState > -1 AndAlso My.Settings.frmMain_WindowState = FormWindowState.Maximized Then

        Try
          Me.Left = My.Settings.frmMain_Left
        Catch ex As Exception
        End Try
      End If

      Me.WindowState = My.Settings.frmMain_WindowState
      If WindowState = FormWindowState.Minimized Then WindowState = FormWindowState.Normal
    End If
    ' --------------------------------------------------------------------------------------------------------------
    If WindowState <> FormWindowState.Maximized Then
      Me.Top = If(My.Settings.frmMain_Top > 0, My.Settings.frmMain_Top, Me.Top)
      Me.Left = If(My.Settings.frmMain_Left > 0, My.Settings.frmMain_Left, Me.Left)
      Me.Width = If(My.Settings.frmMain_Width > 0, My.Settings.frmMain_Width, Me.Width)
      Me.Height = If(My.Settings.frmMain_Height > 0, My.Settings.frmMain_Height, Me.Height)

    End If
    ' --------------------------------------------------------------------------------------------------------------
    'AddHandler Application.Idle, AddressOf Application_Idle
    ' --------------------------------------------------------------------------------------------------------------
    Me.Loaded = True
    ' --------------------------------------------------------------------------------------------------------------
    tmrRibbonUpdate.Start()

  End Sub

  'Private Sub Application_Idle(sender As Object, e As EventArgs)

  'End Sub

  Private Sub rbFirma_Click(sender As Object, e As EventArgs)
    Me.ActiveUserControlIndex = ucControls.FirmaListe
  End Sub

  Private Sub rbAuftragIdentFolder_Click(sender As Object, e As EventArgs)
    Me.ActiveUserControlIndex = ucControls.ProjektListe
  End Sub


  Private Sub rbRecordAktualisieren_Click(sender As Object, e As EventArgs) Handles rbRecordAktualisieren.Click
    If IsNothing(ActiveRecordEvents) Then Exit Sub
    ' --------------------------------------------------
    Me.ActiveRecordEvents.Reload()
  End Sub

  Private Sub rbRecordNeu_Click(sender As Object, e As EventArgs) Handles rbRecordNeu.Click
    If IsNothing(ActiveRecordEvents) Then Exit Sub
    ' --------------------------------------------------
    Me.ActiveRecordEvents.NewRecord()
  End Sub

  Private Sub rbRecordSpeichern_Click(sender As Object, e As EventArgs) Handles rbRecordSpeichern.Click
    If IsNothing(ActiveRecordEvents) Then Exit Sub
    ' --------------------------------------------------
    Me.ActiveRecordEvents.Save()
  End Sub

  Private Sub rbRecordLoeschen_Click(sender As Object, e As EventArgs) Handles rbRecordLoeschen.Click
    If IsNothing(ActiveRecordEvents) Then Exit Sub
    ' --------------------------------------------------
    Me.ActiveRecordEvents.DeleteRecord()

  End Sub

  Private Sub rbRecordFehler_Click(sender As Object, e As EventArgs) Handles rbRecordFehler.Click
    If Not IsNothing(ActiveRecordEvents) Then
      InfoErrorMessage = ActiveRecordEvents.GetErrors
    End If
  End Sub

  Private Sub rbRecordVerwerfen_Click(sender As Object, e As EventArgs) Handles rbRecordVerwerfen.Click
    If IsNothing(ActiveRecordEvents) Then Exit Sub
    ' --------------------------------------------------
    Me.ActiveRecordEvents.RejectRowChanges()
  End Sub

  Private Sub rbLogging_Click(sender As Object, e As EventArgs) Handles rbLogging.Click
    Me.ActiveUserControlIndex = ucControls.Logging
  End Sub

  Private Sub frmMain_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
    If Not Me.Loaded Then Exit Sub
    ' ------------------------------------------------------------
    My.Settings.frmMain_WindowState = Me.WindowState
    My.Settings.frmMain_Width = Me.Width
    My.Settings.frmMain_Height = Me.Height
    My.Settings.frmMain_Top = Me.Top
    My.Settings.frmMain_Left = Me.Left

  End Sub

  Private Sub tmrRibbonUpdate_Tick(sender As Object, e As EventArgs) Handles tmrRibbonUpdate.Tick
    ' --------------------------------------------------
    If Not IsNothing(ActiveRecordEvents) Then
      Me.rbRecordNeu.Enabled = Not ActiveRecordEvents.ReadOnly AndAlso ActiveRecordEvents.MyBindingSource.AllowNew AndAlso Not ActiveRecordEvents.Changed
      Me.rbRecordLoeschen.Enabled = Not IsNothing(ActiveRecordEvents.MyBindingSource.Current) AndAlso Not ActiveRecordEvents.ReadOnly
      Me.rbRecordFehler.Enabled = ActiveRecordEvents.HasErrors
      Me.rbRecordSpeichern.Enabled = Not Me.rbRecordFehler.Enabled AndAlso ActiveRecordEvents.Changed
      Me.rbRecordVerwerfen.Enabled = ActiveRecordEvents.Changed
      Me.rbRecordAktualisieren.Enabled = Not Me.rbRecordFehler.Enabled
      ' --------------------------------------------------
    End If
  End Sub

  Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
    tmrRibbonUpdate.Stop()
    tmrMessage.Stop()
  End Sub

  Private Sub frmMain_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
    tmrRibbonUpdate.Stop()
  End Sub
  Private Sub frmMain_Activated(sender As Object, e As EventArgs) Handles Me.Activated
    tmrRibbonUpdate.Start()
  End Sub

  Private Sub rbCRMExchanger_Click(sender As Object, e As EventArgs) Handles rbCRMExchanger.Click
    Me.ActiveUserControlIndex = ucControls.CRMExchanger
  End Sub

  Private Sub rbRestruct2Prins_Click(sender As Object, e As EventArgs) Handles rbRestruct2Prins.Click
    Me.ActiveUserControlIndex = ucControls.Restruct2Prins
  End Sub

  Private Sub rbAZServiceBus_Click(sender As Object, e As EventArgs) Handles rbAZServiceBus.Click

  End Sub
End Class
