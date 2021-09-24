Public Class clRecordEvents
  Implements ifDataTable

  Public Class RecordSaveErrorEventArgs
    Inherits System.EventArgs

    Private _ErrorMessage As String
    Public Property ErrorMessage As String
      Get
        Return _ErrorMessage
      End Get
      Set(value As String)
        _ErrorMessage = value
      End Set
    End Property

    Public Sub New(ErrorMessage As String)
      _ErrorMessage = ErrorMessage
    End Sub

  End Class

  Public Class RecordBeforeDeleteEventArgs
    Inherits System.EventArgs

    Private _CanDelete As Boolean = False
    Public Property CanDelete As Boolean
      Get
        Return _CanDelete
      End Get
      Set(value As Boolean)
        _CanDelete = value
      End Set
    End Property

    Private _Handled As Boolean = False
    Public Property Handled As Boolean
      Get
        Return _Handled
      End Get
      Set(value As Boolean)
        _Handled = value
      End Set
    End Property

    Public Sub New(CanDelete As Boolean, Optional Handled As Boolean = False)
      _Handled = Handled
      _CanDelete = CanDelete
    End Sub

  End Class

  'RecordSaveErrorEventArgs
  Public Event RecordSaveErrorEvent(Sender As Object, e As RecordSaveErrorEventArgs)
  Public Event RecordBeforeDeleteEvent(Sender As Object, e As RecordBeforeDeleteEventArgs)
  Public Event RecordBeforeAddNewEvent(Sender As Object, e As System.ComponentModel.CancelEventArgs)
  Public Event AfterReloadEvent(Sender As Object, e As System.EventArgs)
  Public Event DataTableNewRow(sender As Object, e As System.Data.DataTableNewRowEventArgs)

  Public Sub New(Src As BindingSource, dbAdapter As ifTableAdapter)
    _MyBindingSource = Src
    _MyDataAdapter = dbAdapter

    AddHandler _MyBindingSource.CurrentChanged, AddressOf MyBindingSource_CurrentChanged
    AddHandler dbTable.TableNewRow, AddressOf dbTable_TableNewRow
    AddHandler dbTable.RowChanging, AddressOf dbTable_RowChanging
    AddHandler dbTable.RowChanged, AddressOf dbTable_RowChanged
    AddHandler dbTable.ColumnChanging, AddressOf dbTable_ColumnChanging
    AddHandler dbTable.ColumnChanged, AddressOf dbTable_ColumnChanged
    AddHandler dbTable.RowDeleted, AddressOf dbTable_RowDeleted

  End Sub

  Private Sub dbTable_TableNewRow(sender As Object, e As System.Data.DataTableNewRowEventArgs)
    RaiseEvent DataTableNewRow(Me, e)
  End Sub

  Private Sub dbTable_RowDeleted(sender As Object, e As System.Data.DataRowChangeEventArgs)
    _Changed = True
    ' --------------------------------------------------------------------------------
    Me.Save()
  End Sub

  Private Sub dbTable_RowChanging(sender As Object, e As System.Data.DataRowChangeEventArgs)

  End Sub

  Private Sub dbTable_RowChanged(sender As Object, e As System.Data.DataRowChangeEventArgs)
    If Not Me._Saving AndAlso Not Me.MyBindingSource.IsBindingSuspended AndAlso _
      (e.Action = DataRowAction.Change OrElse e.Action = DataRowAction.Add) Then
      Me.Save()
    End If
  End Sub

  Private Sub MyBindingSource_CurrentChanged(sender As Object, e As System.EventArgs)
    If MyBindingSource.IsBindingSuspended Then Exit Sub
    ' ------------------------------------------------------------------------------------------
    If Not IsNothing(MyBindingSource.Current) AndAlso DirectCast(MyBindingSource.Current, DataRowView).Row.RowState = DataRowState.Detached Then
      _Changed = True
    End If
  End Sub

  '
  Private Sub dbTable_ColumnChanging(sender As Object, e As System.Data.DataColumnChangeEventArgs)
    If Me._Saving Then Exit Sub
    ' -----------------------------------------------------------------------------------
    If Not e.Row.Table.Columns(e.Column.ColumnName).AllowDBNull Then
      If IsDBNull(e.ProposedValue) OrElse String.IsNullOrEmpty(e.ProposedValue) Then
        e.Row.SetColumnError(e.Column, "In diesem Feld ist eine Eingabe erforderlich !")
      Else
        e.Row.SetColumnError(e.Column, String.Empty)
      End If
    End If
    ' -----------------------------------------------------------------------------------
    _Changed = True
  End Sub

  Private Sub dbTable_ColumnChanged(sender As Object, e As System.Data.DataColumnChangeEventArgs)

  End Sub

  Private _Changed As Boolean = False
  Public ReadOnly Property Changed As Boolean
    Get
      Return _Changed AndAlso Not Me.ReadOnly
    End Get
  End Property

  Private _MyBindingSource As System.Windows.Forms.BindingSource
  Public ReadOnly Property MyBindingSource As System.Windows.Forms.BindingSource
    Get
      Return _MyBindingSource
    End Get
  End Property

  Public ReadOnly Property dbTable() As DataTable
    Get
      Return DirectCast(Me.MyBindingSource.DataSource, DataSet).Tables(Me.MyBindingSource.DataMember)
    End Get
  End Property


  Private _MyDataAdapter As ifTableAdapter
  Private ReadOnly Property MyDataAdapter As ifTableAdapter
    Get
      Return _MyDataAdapter
    End Get
  End Property

  'Private _MyTable As DataTable
  'Private ReadOnly Property myTable As DataTable
  '  Get
  '    Return _MyTable
  '  End Get
  'End Property

  Public Property Position As Integer
    Get
      Return Me.MyBindingSource.Position
    End Get
    Set(value As Integer)
      If value > -1 AndAlso value < Me.MyBindingSource.Count Then
        Me.MyBindingSource.Position = value
      End If
    End Set
  End Property

  Private _ReadOnly As Boolean = False
  Public Property [ReadOnly] As Boolean
    Get
      Return _ReadOnly
    End Get
    Set(value As Boolean)
      _ReadOnly = value
    End Set
  End Property

  Private _DeleteQueryString As String = "Soll der aktuelle Datensatz wirklich gelöscht werden ?"

  Public Property DeleteQueryString As String
    Get
      Return _DeleteQueryString
    End Get
    Set(value As String)
      _DeleteQueryString = value
    End Set
  End Property


  Public Sub DeleteRecord() Implements ifDataTable.DeleteRecord
    If Not IsNothing(MyBindingSource.Current) AndAlso Not Me.ReadOnly Then
      Dim eva As New RecordBeforeDeleteEventArgs(False)

      RaiseEvent RecordBeforeDeleteEvent(Me, eva)
      If Not eva.Handled Then
        If My.Application.Ask(Me.DeleteQueryString, MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton1 Or MsgBoxStyle.Question, My.Resources.Bestätigung) = MsgBoxResult.Yes Then
          eva.CanDelete = True
        End If
      End If
      ' ------------------------------------------------------------------------------
      If eva.CanDelete Then
        Me.MyBindingSource.RemoveCurrent()
      End If

    End If
  End Sub

  Public Sub FirstRecord() Implements ifDataTable.FirstRecord
    Me.Save()
    If Not Me.HasErrors Then Me.MyBindingSource.MoveFirst()
  End Sub

  Public Sub LastRecord() Implements ifDataTable.LastRecord
    Me.Save()
    If Not Me.HasErrors Then Me.MyBindingSource.MoveLast()
  End Sub

  Public Sub NewRecord() Implements ifDataTable.NewRecord
    If Me.ReadOnly Then Exit Sub
    '--------------------------------------------------------------------------------------------------
    Me.Save()
    '--------------------------------------------------------------------------------------------------
    If Not Me.HasErrors Then
      Dim eva As New System.ComponentModel.CancelEventArgs(False)
      RaiseEvent RecordBeforeAddNewEvent(Me, eva)

      If Not eva.Cancel Then
        Me.MyBindingSource.AddNew()
      End If
    End If
  End Sub

  Public Sub NextRecord() Implements ifDataTable.NextRecord
    Me.Save()
    If Not Me.HasErrors Then Me.MyBindingSource.MoveNext()
  End Sub

  Public Sub PrevRecord() Implements ifDataTable.PrevRecord
    Me.Save()
    If Not Me.HasErrors Then Me.MyBindingSource.MovePrevious()
  End Sub

  Public Sub RejectChanges() Implements ifDataTable.RejectChanges
    dbTable.RejectChanges()
    _Changed = False
  End Sub

  Public Sub RejectRowChanges() Implements ifDataTable.RejectRowChanges
    Me.MyBindingSource.CancelEdit()

    If Not IsNothing(Me.MyBindingSource.Current) Then
      DirectCast(Me.MyBindingSource.Current, DataRowView).Row.RejectChanges()
      _Changed = False
      Me.MyBindingSource.SuspendBinding() : Me.MyBindingSource.ResumeBinding()
    Else
      Me.dbTable.RejectChanges()
    End If
    ClearAllErrors()
  End Sub

  Public Sub Reload() Implements ifDataTable.Reload
    If _Changed Then Me.Save()
    '-------------------------------------------------------------
    If Not HasErrors AndAlso Not Changed Then
      Dim LastId As Integer = -1

      If Not IsNothing(Me.MyBindingSource.Current) AndAlso Me.dbTable.PrimaryKey.Length = 1 AndAlso _
          DirectCast(Me.MyBindingSource.Current, DataRowView).Row.RowState <> DataRowState.Detached AndAlso _
          dbTable.Columns(Me.dbTable.PrimaryKey(0).ColumnName).DataType Is GetType(Integer) Then
        LastId = DirectCast(Me.MyBindingSource.Current, DataRowView).Row(Me.dbTable.PrimaryKey(0))
      End If
      '-------------------------------------------------------------
      Me.MyDataAdapter.Reload()
      '-------------------------------------------------------------
      If LastId > -1 Then
        Dim idx As Integer = Me.MyBindingSource.Find(Me.dbTable.PrimaryKey(0).ColumnName, LastId)
        If idx > -1 Then
          Me.MyBindingSource.Position = idx
        End If
      End If
      '-------------------------------------------------------------
      _Changed = False
      '-------------------------------------------------------------
      RaiseEvent AfterReloadEvent(Me, System.EventArgs.Empty)
    End If
  End Sub

  Private _Saving As Boolean = False
  Public Sub Save() Implements ifDataTable.Save
    If _Saving Or Not Changed Then Exit Sub
    ' -----------------------------------
    Try
      _Saving = True
      Me.MyBindingSource.EndEdit()
      Me.MyDataAdapter.Save()
      _Changed = False

    Catch ex As Exception
      Dim eva As New RecordSaveErrorEventArgs(ex.Message)
      RaiseEvent RecordSaveErrorEvent(Me, eva)

      If Not IsNothing(Me.MyBindingSource.Current) Then
        DirectCast(Me.MyBindingSource.Current, DataRowView).Row.RowError = eva.ErrorMessage
      Else
        If (Me.dbTable.Rows.Count > 0) Then
          Me.dbTable.Rows(0).RowError = eva.ErrorMessage
        End If
      End If
    Finally
      _Saving = False
    End Try
  End Sub

  Public ReadOnly Property HasErrors As Boolean
    Get
      Dim Result As Boolean = False

      For i As Integer = 0 To dbTable.Rows.Count - 1
        If dbTable.Rows(i).HasErrors Then
          Result = True : Exit For
        End If
      Next

      If Not Result Then
        If Not IsNothing(Me.MyBindingSource.Current) Then
          Result = DirectCast(Me.MyBindingSource.Current, DataRowView).Row.HasErrors
        End If
      End If

      Return Result
    End Get
  End Property

  Public ReadOnly Property GetErrors As String
    Get
      Dim Msg As String = String.Empty
      Dim i As Integer = 0
      Dim CurrentRow As DataRow = If(IsNothing(Me.MyBindingSource.Current), Nothing, DirectCast(Me.MyBindingSource.Current, DataRowView).Row)
      Dim Row As DataRow = CurrentRow

      Do
        If Not IsNothing(Row) Then
          Dim dcs As DataColumn() = Row.GetColumnsInError

          For c As Integer = 0 To dcs.Count - 1
            If c > 0 Then Msg += ControlChars.CrLf
            Msg += Row.GetColumnError(dcs(c))
          Next

          If Not String.IsNullOrEmpty(Row.RowError) Then
            If Msg.Length > 0 Then Msg += ControlChars.CrLf
            Msg += Row.RowError
          End If
        End If

        If i = Me.dbTable.Rows.Count Then Exit Do
        Row = Me.dbTable.Rows(i)
        ' ---------------------------------------------------------------
        ' Wenn Row bereits als CurretnRow geprüft wurde, überspringen
        If Not IsNothing(CurrentRow) AndAlso CurrentRow Is Row Then Row = Nothing
        i += 1
      Loop


      Return Msg
    End Get
  End Property

  Private Sub ClearAllErrors()
    For i As Integer = 0 To dbTable.Rows.Count - 1
      Dim dcs As DataColumn() = dbTable.Rows(i).GetColumnsInError
      For c As Integer = 0 To dcs.Count - 1
        dbTable.Rows(i).SetColumnError(dcs(c), String.Empty)
      Next
      dbTable.Rows(i).RowError = String.Empty
    Next

  End Sub


End Class
