Partial Class dsPrins3Central


#Region "dsPrins3Central-Extensions"
  Public Class clGenerator

    Private _dsGenerator As dsGenerator
    Private ReadOnly Property dsGenerator As dsGenerator
      Get
        If IsNothing(_dsGenerator) Then
          _dsGenerator = New dsGenerator
        End If
        Return _dsGenerator
      End Get
    End Property

    Private _taGenerator As dsGeneratorTableAdapters.GENERATOR_IDTableAdapter
    Private ReadOnly Property taGenerator As dsGeneratorTableAdapters.GENERATOR_IDTableAdapter
      Get
        If IsNothing(_taGenerator) Then
          _taGenerator = New dsGeneratorTableAdapters.GENERATOR_IDTableAdapter
        End If
        Return _taGenerator
      End Get
    End Property

    Public ReadOnly Property GetGeneratorValue(Table As DataTable) As Integer
      Get
        Dim GName As String = "GEN_" + Table.TableName + "_ID"
        GName = GName.Substring(0, Math.Min(31, GName.Length))
        Return GetGeneratorValue(GName)
      End Get
    End Property

    Public ReadOnly Property GetGeneratorValue(GeneratorName As String) As Integer
      Get
        Dim Result As Integer = -1
        Dim Sql As String = String.Format("SELECT MAX(RDB$RELATION_ID) AS ID, GEN_ID( {0}, {1}) AS NEW_ID FROM RDB$DATABASE ", GeneratorName, 1)
        Dim da As IDbDataAdapter = taGenerator.Adapter

        If IsNothing(da.SelectCommand) Then
          da.SelectCommand = New FirebirdSql.Data.FirebirdClient.FbCommand(Sql, taGenerator.Connection)
        Else
          da.SelectCommand.CommandText = Sql
        End If

        da.SelectCommand.Connection = taGenerator.Connection
        If dsGenerator.Tables.Count > 0 Then dsGenerator.Tables.Clear()

        da.Fill(dsGenerator)
        If dsGenerator.Tables.Count > 0 AndAlso dsGenerator.Tables(0).Rows.Count = 1 Then
          Result = dsGenerator.Tables(0).Rows(0)("NEW_ID")
        End If

        Return Result
      End Get
    End Property
  End Class

  Private _Generator As clGenerator
  Public ReadOnly Property Generator As clGenerator
    Get
      If IsNothing(_Generator) Then
        _Generator = New clGenerator
      End If

      Return _Generator
    End Get
  End Property


  Public Enum UserPrivilegeEnum
    None = 0
    Read = 1
    Write = 2
    ReadWrite = 3
    Insert = 4
    ReadWriteInsert = 7
    Delete = 8
    ReadWriteInsertDelete = 15
  End Enum

  Public Enum ReloadOptions
    SaveChanges_IgnoreErrors
    SaveChanges_AbortAtError
    RejectChanges
  End Enum

#End Region


  'TBL_USER_ACCOUNTS
  Partial Public Class TBL_USER_ACCOUNTSDataTable
    Implements ifDataTableDef3

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs) Implements ifDataTableDef3.AfterSaveChanges
    Public ReadOnly Property TableAdapterObject As Object Implements ifDataTableDef3.TableAdapter
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrins3CentralTableAdapters.TBL_USER_ACCOUNTSTableAdapter
    Public ReadOnly Property TableAdapter As dsPrins3CentralTableAdapters.TBL_USER_ACCOUNTSTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrins3CentralTableAdapters.TBL_USER_ACCOUNTSTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean Implements ifDataTableDef3.HasChanges
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean Implements ifDataTableDef3.HasRows
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean Implements ifDataTableDef3.SavingChanges
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable Implements ifDataTableDef3.Table
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum Implements ifDataTableDef3.UserPrivileges
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges() Implements ifDataTableDef3.SaveChanges
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As TBL_USER_ACCOUNTSDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As TBL_USER_ACCOUNTSRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As TBL_USER_ACCOUNTSDataTable
            Row = ChangedTable.Rows(idx)

            Try
              Row.ClearErrors()
              Me.TableAdapter.Update(Row)
              ChangesSaved = True

              If Row.RowState <> DataRowState.Detached Then
                ResultTable = TableAdapter.GetDataByID(Row.ID)
                Me.Merge(ResultTable, False)
              End If

            Catch ex As Exception
              Row.RowError = ex.Message
            End Try
          Next
          ' ------------------------------------------------------------------------
          If ChangedTable.HasErrors Then
            For Each Row In ChangedTable.GetErrors
              If Row.RowState = DataRowState.Deleted Then
                Row.RejectChanges()

                Dim SourceRow As TBL_USER_ACCOUNTSRow
                For idx As Integer = Me.Rows.Count - 1 To 0 Step -1
                  SourceRow = Me.Rows(idx)
                  If SourceRow("ID", DataRowVersion.Original) = Row.ID Then
                    SourceRow.RejectChanges()
                    SourceRow.RowError = Row.RowError
                  End If
                Next
              Else
                Me.Rows.Find(Row.ID).RowError = Row.RowError
              End If
            Next
          Else
            _HasChanges = False
            Me.AcceptChanges()
          End If
          ' ------------------------------------------------------------------------
        End If
      Finally
        _DoingSaveChanges = False
      End Try
      ' ------------------------------------------------------------------------
      If ChangesSaved Then
        RaiseEvent AfterSaveChanges(Me, System.EventArgs.Empty)
      End If


    End Sub

    Public Sub Reload(Optional ReloadOptions As dsPrins3Central.ReloadOptions = ReloadOptions.SaveChanges_AbortAtError) Implements ifDataTableDef3.Reload
      If HasChanges Then
        If ReloadOptions <> ReloadOptions.RejectChanges Then
          Me.SaveChanges()
        End If
        '----------------------------------------------------------------------------------
      End If

      If Not HasChanges OrElse (ReloadOptions = ReloadOptions.RejectChanges OrElse ReloadOptions = ReloadOptions.SaveChanges_IgnoreErrors) Then
        TableAdapter.Fill(Me)
      End If

    End Sub

    Private Sub TBL_USER_ACCOUNTSDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub TBL_USER_ACCOUNTSDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          TBL_USER_ACCOUNTSDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub TBL_USER_ACCOUNTSDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As TBL_USER_ACCOUNTSRow = e.Row
      Row.ID = DirectCast(Me.DataSet, dsPrins3Central).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        TBL_USER_ACCOUNTSDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub TBL_USER_ACCOUNTS_TBL_USER_ACCOUNTSRowChanged(sender As Object, e As TBL_USER_ACCOUNTSRowChangeEvent) Handles Me.TBL_USER_ACCOUNTSRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub TBL_USER_ACCOUNTSDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub TBL_USER_ACCOUNTSDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub

    Private Sub TBL_USER_ACCOUNTSDataTable_TBL_USER_ACCOUNTSRowChanging(sender As Object, e As TBL_USER_ACCOUNTSRowChangeEvent) Handles Me.TBL_USER_ACCOUNTSRowChanging

    End Sub

  End Class




End Class


