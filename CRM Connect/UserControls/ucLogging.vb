Public Class ucLogging
  Implements ifTableAdapter
  Implements ifRecordEvent


  Private Sub ucLogging_Load(sender As Object, e As EventArgs) Handles Me.Load
    Reload()
  End Sub

  Public Sub Reload() Implements ifTableAdapter.Reload
    AddHandler LoggingRecordEvents.DataTableNewRow, AddressOf ucLogging_DataTableNewRow
    ' ------------------------------------------------------------------------
    Dim dt As New DataTable()
    Dim et As clMessageLog.MsgSourceEnum

    dt.Columns.Add("ID", GetType(Integer))
    dt.Columns.Add("TEXT", GetType(String))

    For Each i As Integer In System.Enum.GetValues(et.GetType())
      dt.Rows.Add({i, System.Enum.GetName(et.GetType, i)})
    Next

    Me.cbSOURCE.DataSource = dt.Copy
    Me.cbSOURCE.ValueMember = "ID"
    Me.cbSOURCE.DisplayMember = "TEXT"
    ' ------------------------------------------------------------------------
    Dim mse As clMessageLog.MsgSeverityEnum

    dt.Clear()
    For Each i As Integer In System.Enum.GetValues(mse.GetType())
      dt.Rows.Add({i, System.Enum.GetName(mse.GetType, i)})
    Next

    Me.cbSEVERITY.DataSource = dt.Copy
    Me.cbSEVERITY.ValueMember = "ID"
    Me.cbSEVERITY.DisplayMember = "TEXT"

    Me.taLOGGING.Fill(Me.DsPrinsTables.TBL_LOGGING)

  End Sub

  Public Sub Save() Implements ifTableAdapter.Save

    Dim dt As dsPrinsTables.TBL_LOGGINGDataTable = Me.DsPrinsTables.TBL_LOGGING.GetChanges

    For Each Row As dsPrinsTables.TBL_LOGGINGRow In dt.Rows
      Me.taLOGGING.Update(Row)
      Me.DsPrinsTables.TBL_LOGGING.Merge(Me.taLOGGING.GetDataByID(Row.ID))
    Next
    Me.DsPrinsTables.TBL_LOGGING.AcceptChanges()
  End Sub

  Private _LoggingRecordEvents As clRecordEvents
  Public ReadOnly Property LoggingRecordEvents As clRecordEvents Implements ifRecordEvent.ActiveRecordEvents
    Get
      If IsNothing(_LoggingRecordEvents) Then
        _LoggingRecordEvents = New clRecordEvents(Me.bsLOGGING, Me)
      End If
      Return _LoggingRecordEvents
    End Get
  End Property

  Private Sub ucLogging_DataTableNewRow(sender As Object, e As System.Data.DataTableNewRowEventArgs)
    e.Row("ID") = My.Forms.frmMain.GetGeneratorValue("gen_tbl_logging_id")
  End Sub

End Class
