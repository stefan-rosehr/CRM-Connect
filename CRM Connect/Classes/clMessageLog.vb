Public Class clMessageLog
  Implements ifRecordEvent
  Implements ifTableAdapter

  Public Enum MsgSeverityEnum As Integer
    Informational = 0
    Warning = 1
    RecoverableError = 2
    NonRecoverableError = 3
    SevereError = 4
    FatalError = 5
  End Enum

  Public Enum MsgSourceEnum As Integer
    SystemDefault = 0
    SystemStartup = 1
    CRMImportExport = 100

    Laboratoy1 = 110
    Laboratoy2 = 120

    CRMExchanger = 200
  End Enum

  Public Enum MsgSubTypeEnum As Integer
    StartMessage = 1
    DetailedMessage = 2
    FinalSummary = 3
  End Enum


  Private _DsPrinsTables As dsPrinsTables
  Public ReadOnly Property DsPrinsTables As dsPrinsTables
    Get
      If IsNothing(_DsPrinsTables) Then
        _DsPrinsTables = New dsPrinsTables()
      End If
      Return _DsPrinsTables
    End Get
  End Property

  Public ReadOnly Property dtLOGGING As dsPrinsTables.TBL_LOGGINGDataTable
    Get
      Return Me.DsPrinsTables.TBL_LOGGING
    End Get
  End Property


  Private _taLOGGING As dsPrinsTablesTableAdapters.TBL_LOGGINGTableAdapter = Nothing
  Private ReadOnly Property taLOGGING As dsPrinsTablesTableAdapters.TBL_LOGGINGTableAdapter
    Get
      If IsNothing(_taLOGGING) Then
        _taLOGGING = New dsPrinsTablesTableAdapters.TBL_LOGGINGTableAdapter()
      End If
      Return _taLOGGING
    End Get
  End Property

  Private _dvLogging As DataView = Nothing
  Public ReadOnly Property dvLogging As DataView
    Get
      If IsNothing(_dvLogging) Then
        _dvLogging = New DataView(dtLOGGING)
      End If

      Return _dvLogging
    End Get
  End Property

  Private _bsLogging As BindingSource
  Private ReadOnly Property bsLogging As BindingSource
    Get
      If IsNothing(_bsLogging) Then
        _bsLogging = New BindingSource(DsPrinsTables, dtLOGGING.TableName)
      End If
      Return _bsLogging
    End Get
  End Property

  Private _LoggingRecordEvents As clRecordEvents
  Private ReadOnly Property LoggingRecordEvents As clRecordEvents
    Get
      If IsNothing(_LoggingRecordEvents) Then
        _LoggingRecordEvents = New clRecordEvents(Me.bsLogging, Me)
        AddHandler _LoggingRecordEvents.DataTableNewRow, AddressOf Logging_DataTableNewRow
        ' ------------------------------------------------------------------------
      End If
      Return _LoggingRecordEvents
    End Get
  End Property

  Public ReadOnly Property ActiveRecordEvents As clRecordEvents Implements ifRecordEvent.ActiveRecordEvents
    Get
      Return LoggingRecordEvents
    End Get
  End Property

  Private _LoggingID As Integer = -1
  Public Sub New()
    LoggingRecordEvents.Reload()
    'taLOGGING.FillByID(dtLOGGING, -1) 'Leermenge
  End Sub

  Public Sub Reload() Implements ifTableAdapter.Reload
    taLOGGING.FillByID(dtLOGGING, _LoggingID)
  End Sub

  Public Sub Save() Implements ifTableAdapter.Save
    taLOGGING.Update(dtLOGGING)
  End Sub

  Private Sub Logging_DataTableNewRow(sender As Object, e As System.Data.DataTableNewRowEventArgs)
    e.Row("ID") = My.Forms.frmMain.GetGeneratorValue("gen_tbl_logging_id")
  End Sub

  Public Sub Add(Message As String)
    Add(Message, MsgSourceEnum.SystemDefault, MsgSubTypeEnum.DetailedMessage, MsgSeverityEnum.Informational)
  End Sub

  Public Sub Add(Message As String, MsgSource As MsgSourceEnum)
    Add(Message, MsgSource, MsgSubTypeEnum.DetailedMessage, MsgSeverityEnum.Informational)
  End Sub

  Public Sub Add(Message As String, MsgSource As MsgSourceEnum, MsgSubType As MsgSubTypeEnum)
    Add(Message, MsgSource, MsgSubType, MsgSeverityEnum.Informational)
  End Sub

  Public Sub Add(Message As String, MsgSource As MsgSourceEnum, MsgSubType As MsgSubTypeEnum, MsgSeverity As MsgSeverityEnum)
    Dim Row As dsPrinsTables.TBL_LOGGINGRow = dtLOGGING.NewTBL_LOGGINGRow

    Row.SOURCE = MsgSource
    Row.SUBTYPE = MsgSubType
    Row.SEVERITY = MsgSeverity
    Row.MSG_TEXT = Message

    dtLOGGING.AddTBL_LOGGINGRow(Row)
    Save()

  End Sub


End Class
