Partial Class dsPrins
  Partial Public Class VW_CRM_PROJEKT_EXPDataTable
    Private Sub VW_CRM_PROJEKT_EXPDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
      If (e.Column.ColumnName = Me.PROJEKTNUMMERColumn.ColumnName) Then
        'Benutzercode hier einfügen
      End If

    End Sub

  End Class

#Region "dsPrins-Extensions"
  Public Class clGenerator

    Private _Connection As FirebirdSql.Data.FirebirdClient.FbConnection
    Public Sub New(Connection As FirebirdSql.Data.FirebirdClient.FbConnection)
      _Connection = Connection
    End Sub


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
          _taGenerator.Connection = _Connection
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
        _Generator = New clGenerator(PERSONAL.TableAdapter.Connection)
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

  'TBL_FO_FREETEXT_ERRORS DataTable
  Partial Public Class TBL_FO_FREETEXT_ERRORSDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.TBL_FO_FREETEXT_ERRORSTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.TBL_FO_FREETEXT_ERRORSTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.TBL_FO_FREETEXT_ERRORSTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As TBL_FO_FREETEXT_ERRORSDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As TBL_FO_FREETEXT_ERRORSRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As TBL_FO_FREETEXT_ERRORSDataTable
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

                Dim SourceRow As TBL_FO_FREETEXT_ERRORSRow
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub TBL_FO_FREETEXT_ERRORSDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub TBL_FO_FREETEXT_ERRORSDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          TBL_FO_FREETEXT_ERRORSDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub TBL_FO_FREETEXT_ERRORSDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As TBL_FO_FREETEXT_ERRORSRow = e.Row
      Row.ID = DirectCast(Me.DataSet, dsPrins).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        TBL_FO_FREETEXT_ERRORSDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub TBL_FO_FREETEXT_ERRORS_TBL_FO_FREETEXT_ERRORSRowChanged(sender As Object, e As TBL_FO_FREETEXT_ERRORSRowChangeEvent) Handles Me.TBL_FO_FREETEXT_ERRORSRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub TBL_FO_FREETEXT_ERRORSDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub TBL_FO_FREETEXT_ERRORSDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub

  End Class



  'TBL_GENERALJOURNALACCOUNTENTRY
  Partial Public Class TBL_GENERALJOURNALACCOUNTENTRYDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.TBL_GENERALJOURNALACCOUNTENTRYTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.TBL_GENERALJOURNALACCOUNTENTRYTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.TBL_GENERALJOURNALACCOUNTENTRYTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As TBL_GENERALJOURNALACCOUNTENTRYDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As TBL_GENERALJOURNALACCOUNTENTRYRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As TBL_GENERALJOURNALACCOUNTENTRYDataTable
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

                Dim SourceRow As TBL_GENERALJOURNALACCOUNTENTRYRow
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub TBL_GENERALJOURNALACCOUNTENTRYDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub TBL_GENERALJOURNALACCOUNTENTRYDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          TBL_GENERALJOURNALACCOUNTENTRYDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub TBL_GENERALJOURNALACCOUNTENTRYDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As TBL_GENERALJOURNALACCOUNTENTRYRow = e.Row
      Row.ID = DirectCast(Me.DataSet, dsPrins).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        TBL_GENERALJOURNALACCOUNTENTRYDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub TBL_GENERALJOURNALACCOUNTENTRY_TBL_GENERALJOURNALACCOUNTENTRYRowChanged(sender As Object, e As TBL_GENERALJOURNALACCOUNTENTRYRowChangeEvent) Handles Me.TBL_GENERALJOURNALACCOUNTENTRYRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub TBL_GENERALJOURNALACCOUNTENTRYDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub TBL_GENERALJOURNALACCOUNTENTRYDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub

  End Class




  'TBL_AD_ESTGROUPS
  Partial Public Class TBL_AD_ESTGROUPSDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.TBL_AD_ESTGROUPSTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.TBL_AD_ESTGROUPSTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.TBL_AD_ESTGROUPSTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As TBL_AD_ESTGROUPSDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As TBL_AD_ESTGROUPSRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As TBL_AD_ESTGROUPSDataTable
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

                Dim SourceRow As TBL_AD_ESTGROUPSRow
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub TBL_AD_ESTGROUPSDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub TBL_AD_ESTGROUPSDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          TBL_AD_ESTGROUPSDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub TBL_AD_ESTGROUPSDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As TBL_AD_ESTGROUPSRow = e.Row
      Row.ID = DirectCast(Me.DataSet, dsPrins).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        TBL_AD_ESTGROUPSDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub TBL_AD_ESTGROUPS_TBL_AD_ESTGROUPSRowChanged(sender As Object, e As TBL_AD_ESTGROUPSRowChangeEvent) Handles Me.TBL_AD_ESTGROUPSRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub TBL_AD_ESTGROUPSDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub TBL_AD_ESTGROUPSDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub


  End Class



  'TBL_PRJ_FOLDERS
  Partial Public Class TBL_PRJ_FOLDERSDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.TBL_PRJ_FOLDERSTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.TBL_PRJ_FOLDERSTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.TBL_PRJ_FOLDERSTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As TBL_PRJ_FOLDERSDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As TBL_PRJ_FOLDERSRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As TBL_PRJ_FOLDERSDataTable
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

                Dim SourceRow As TBL_PRJ_FOLDERSRow
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub TBL_PRJ_FOLDERSDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub TBL_PRJ_FOLDERSDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          TBL_PRJ_FOLDERSDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub TBL_PRJ_FOLDERSDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As TBL_PRJ_FOLDERSRow = e.Row
      Row.ID = DirectCast(Me.DataSet, dsPrins).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        TBL_PRJ_FOLDERSDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub TBL_PRJ_FOLDERS_TBL_PRJ_FOLDERSRowChanged(sender As Object, e As TBL_PRJ_FOLDERSRowChangeEvent) Handles Me.TBL_PRJ_FOLDERSRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub TBL_PRJ_FOLDERSDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub TBL_PRJ_FOLDERSDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub


  End Class


  'TBL_KHK_OPBUCHUNGENDataTable
  Partial Public Class TBL_KHK_OPBUCHUNGENDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.TBL_KHK_OPBUCHUNGENTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.TBL_KHK_OPBUCHUNGENTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.TBL_KHK_OPBUCHUNGENTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As TBL_KHK_OPBUCHUNGENDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As TBL_KHK_OPBUCHUNGENRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As TBL_KHK_OPBUCHUNGENDataTable
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

                Dim SourceRow As TBL_KHK_OPBUCHUNGENRow
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub TBL_KHK_OPBUCHUNGENDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub TBL_KHK_OPBUCHUNGENDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          TBL_KHK_OPBUCHUNGENDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub TBL_KHK_OPBUCHUNGENDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As TBL_KHK_OPBUCHUNGENRow = e.Row
      Row.ID = DirectCast(Me.DataSet, dsPrins).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        TBL_KHK_OPBUCHUNGENDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub TBL_KHK_OPBUCHUNGEN_TBL_KHK_OPBUCHUNGENRowChanged(sender As Object, e As TBL_KHK_OPBUCHUNGENRowChangeEvent) Handles Me.TBL_KHK_OPBUCHUNGENRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub TBL_KHK_OPBUCHUNGENDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub TBL_KHK_OPBUCHUNGENDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub


  End Class


  'WOCHENBERICHT
  Partial Public Class WOCHENBERICHTDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.WOCHENBERICHTTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.WOCHENBERICHTTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.WOCHENBERICHTTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True
        Dim LastRowError As String = String.Empty
        Dim ChangedTable As WOCHENBERICHTDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As WOCHENBERICHTRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As WOCHENBERICHTDataTable
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
              LastRowError = ex.Message
            End Try
          Next
          ' ------------------------------------------------------------------------
          If ChangedTable.HasErrors Then
            For Each Row In ChangedTable.GetErrors
              If Row.RowState = DataRowState.Deleted Then
                Row.RejectChanges()

                Dim SourceRow As WOCHENBERICHTRow
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
            ' -------------------------------------------------------
            'Throw New Exception(LastRowError)
            ' -------------------------------------------------------
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub WOCHENBERICHTDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub WOCHENBERICHTDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          WOCHENBERICHTDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub WOCHENBERICHTDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As WOCHENBERICHTRow = e.Row
      Row.ID = DirectCast(Me.DataSet, dsPrins).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        WOCHENBERICHTDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub WOCHENBERICHT_WOCHENBERICHTRowChanged(sender As Object, e As WOCHENBERICHTRowChangeEvent) Handles Me.WOCHENBERICHTRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub WOCHENBERICHTDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub WOCHENBERICHTDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub

    Private Sub WOCHENBERICHTDataTable_WOCHENBERICHTRowChanging(sender As Object, e As WOCHENBERICHTRowChangeEvent) Handles Me.WOCHENBERICHTRowChanging

    End Sub

  End Class


  'PRJ_LEISTUNGSPHASE
  Partial Public Class PRJ_LEISTUNGSPHASEDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.PRJ_LEISTUNGSPHASETableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.PRJ_LEISTUNGSPHASETableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.PRJ_LEISTUNGSPHASETableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As PRJ_LEISTUNGSPHASEDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As PRJ_LEISTUNGSPHASERow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As PRJ_LEISTUNGSPHASEDataTable
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

                Dim SourceRow As PRJ_LEISTUNGSPHASERow
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub PRJ_LEISTUNGSPHASEDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub PRJ_LEISTUNGSPHASEDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          PRJ_LEISTUNGSPHASEDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub PRJ_LEISTUNGSPHASEDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As PRJ_LEISTUNGSPHASERow = e.Row
      Row.ID = DirectCast(Me.DataSet, dsPrins).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        PRJ_LEISTUNGSPHASEDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub PRJ_LEISTUNGSPHASE_PRJ_LEISTUNGSPHASERowChanged(sender As Object, e As PRJ_LEISTUNGSPHASERowChangeEvent) Handles Me.PRJ_LEISTUNGSPHASERowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub PRJ_LEISTUNGSPHASEDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub PRJ_LEISTUNGSPHASEDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub


  End Class


  'PROJEKT_STANDORT
  Partial Public Class PROJEKT_STANDORTDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.PROJEKT_STANDORTTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.PROJEKT_STANDORTTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.PROJEKT_STANDORTTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As PROJEKT_STANDORTDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As PROJEKT_STANDORTRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As PROJEKT_STANDORTDataTable
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

                Dim SourceRow As PROJEKT_STANDORTRow
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub PROJEKT_STANDORTDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub PROJEKT_STANDORTDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          PROJEKT_STANDORTDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub PROJEKT_STANDORTDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As PROJEKT_STANDORTRow = e.Row
      Row.ID = DirectCast(Me.DataSet, dsPrins).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        PROJEKT_STANDORTDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub PROJEKT_STANDORT_PROJEKT_STANDORTRowChanged(sender As Object, e As PROJEKT_STANDORTRowChangeEvent) Handles Me.PROJEKT_STANDORTRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub PROJEKT_STANDORTDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub PROJEKT_STANDORTDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub


  End Class


  'PROJEKT
  Partial Public Class PROJEKTDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.PROJEKTTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.PROJEKTTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.PROJEKTTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As PROJEKTDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As PROJEKTRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As PROJEKTDataTable
            Row = ChangedTable.Rows(idx)

            Try
              Row.ClearErrors()
              Me.TableAdapter.Update(Row)
              ChangesSaved = True

              If Row.RowState <> DataRowState.Detached Then
                ResultTable = TableAdapter.GetDataByNummer(Row.NUMMER)
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

                Dim SourceRow As PROJEKTRow
                For idx As Integer = Me.Rows.Count - 1 To 0 Step -1
                  SourceRow = Me.Rows(idx)
                  If SourceRow("NUMMER", DataRowVersion.Original) = Row.NUMMER Then
                    SourceRow.RejectChanges()
                    SourceRow.RowError = Row.RowError
                  End If
                Next
              Else
                Me.Rows.Find(Row.NUMMER).RowError = Row.RowError
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub PROJEKTDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub PROJEKTDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          PROJEKTDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub PROJEKTDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As PROJEKTRow = e.Row
      'Row.NUMMER = DirectCast(Me.DataSet, dsPrins3Central).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        PROJEKTDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub PROJEKT_PROJEKTRowChanged(sender As Object, e As PROJEKTRowChangeEvent) Handles Me.PROJEKTRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub PROJEKTDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub PROJEKTDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub


  End Class


  'KUNDEN
  Partial Public Class KUNDENDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.KUNDENTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.KUNDENTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.KUNDENTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As KUNDENDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As KUNDENRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As KUNDENDataTable
            Row = ChangedTable.Rows(idx)

            Try
              Row.ClearErrors()
              Me.TableAdapter.Update(Row)
              ChangesSaved = True

              If Row.RowState <> DataRowState.Detached Then
                ResultTable = TableAdapter.GetDataByNummer(Row.NUMMER)
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

                Dim SourceRow As KUNDENRow
                For idx As Integer = Me.Rows.Count - 1 To 0 Step -1
                  SourceRow = Me.Rows(idx)
                  If SourceRow("NUMMER", DataRowVersion.Original) = Row.NUMMER Then
                    SourceRow.RejectChanges()
                    SourceRow.RowError = Row.RowError
                  End If
                Next
              Else
                Me.Rows.Find(Row.NUMMER).RowError = Row.RowError
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub KUNDENDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub KUNDENDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          KUNDENDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub KUNDENDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As KUNDENRow = e.Row
      Row.NUMMER = DirectCast(Me.DataSet, dsPrins).Generator.GetGeneratorValue("KUNDEN_ID_5090")

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        KUNDENDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub KUNDEN_KUNDENRowChanged(sender As Object, e As KUNDENRowChangeEvent) Handles Me.KUNDENRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub KUNDENDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub KUNDENDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub


  End Class



  'KOSTENSTELLEN
  Partial Public Class KOSTENSTELLENDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.KOSTENSTELLENTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.KOSTENSTELLENTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.KOSTENSTELLENTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As KOSTENSTELLENDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As KOSTENSTELLENRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As KOSTENSTELLENDataTable
            Row = ChangedTable.Rows(idx)

            Try
              Row.ClearErrors()
              Me.TableAdapter.Update(Row)
              ChangesSaved = True

              If Row.RowState <> DataRowState.Detached Then
                ResultTable = TableAdapter.GetDataByNummer(Row.NUMMER)
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

                Dim SourceRow As KOSTENSTELLENRow
                For idx As Integer = Me.Rows.Count - 1 To 0 Step -1
                  SourceRow = Me.Rows(idx)
                  If SourceRow("NUMMER", DataRowVersion.Original) = Row.NUMMER Then
                    SourceRow.RejectChanges()
                    SourceRow.RowError = Row.RowError
                  End If
                Next
              Else
                Me.Rows.Find(Row.NUMMER).RowError = Row.RowError
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub KOSTENSTELLENDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub KOSTENSTELLENDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          KOSTENSTELLENDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub KOSTENSTELLENDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As KOSTENSTELLENRow = e.Row
      'Row.NUMMER = DirectCast(Me.DataSet, dsPrins3Central).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        KOSTENSTELLENDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub KOSTENSTELLEN_KOSTENSTELLENRowChanged(sender As Object, e As KOSTENSTELLENRowChangeEvent) Handles Me.KOSTENSTELLENRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub KOSTENSTELLENDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub KOSTENSTELLENDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub

  End Class




  'PERSONAL
  Partial Public Class PERSONALDataTable

    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsPrinsTableAdapters.PERSONALTableAdapter
    Public ReadOnly Property TableAdapter As dsPrinsTableAdapters.PERSONALTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsPrinsTableAdapters.PERSONALTableAdapter
        End If

        Return _TableAdapter
      End Get
    End Property

    Private _HasChanges As Boolean = False
    Public ReadOnly Property HasChanges As Boolean
      Get
        If Not _HasChanges Then
          _HasChanges = Not IsNothing(Me.GetChanges)
        End If

        Return _HasChanges
      End Get
    End Property

    Public ReadOnly Property HasRows As Boolean
      Get
        Return Me.Rows.Count > 0
      End Get
    End Property


    Public ReadOnly Property SavingChanges As Boolean
      Get
        Return _DoingSaveChanges
      End Get
    End Property

    Public ReadOnly Property Table As DataTable
      Get
        Return Me
      End Get
    End Property

    Private _UserPrivileges As dsPrins3Central.UserPrivilegeEnum = dsPrins3Central.UserPrivilegeEnum.ReadWriteInsertDelete

    Public Property UserPrivileges As dsPrins3Central.UserPrivilegeEnum
      Get
        Return _UserPrivileges
      End Get
      Set(value As dsPrins3Central.UserPrivilegeEnum)
        _UserPrivileges = value
      End Set
    End Property

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As PERSONALDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As PERSONALRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As PERSONALDataTable
            Row = ChangedTable.Rows(idx)

            Try
              Row.ClearErrors()
              Me.TableAdapter.Update(Row)
              ChangesSaved = True

              If Row.RowState <> DataRowState.Detached Then
                ResultTable = TableAdapter.GetDataByNummer(Row.NUMMER)
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

                Dim SourceRow As PERSONALRow
                For idx As Integer = Me.Rows.Count - 1 To 0 Step -1
                  SourceRow = Me.Rows(idx)
                  If SourceRow("NUMMER", DataRowVersion.Original) = Row.NUMMER Then
                    SourceRow.RejectChanges()
                    SourceRow.RowError = Row.RowError
                  End If
                Next
              Else
                Me.Rows.Find(Row.NUMMER).RowError = Row.RowError
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

    Public Sub Reload(Optional ReloadOptions As ReloadOptions = ReloadOptions.SaveChanges_AbortAtError)
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

    Private Sub PERSONALDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub PERSONALDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          PERSONALDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub PERSONALDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As PERSONALRow = e.Row
      'Row.NUMMER = DirectCast(Me.DataSet, dsPrins3Central).Generator.GetGeneratorValue(Me)

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        PERSONALDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub PERSONAL_PERSONALRowChanged(sender As Object, e As PERSONALRowChangeEvent) Handles Me.PERSONALRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub PERSONALDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub PERSONALDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub


  End Class




End Class

Namespace dsPrinsTableAdapters


  Partial Public Class TBL_CRM_FIRMA_DEBITOR_LNK_IMPTableAdapter
    Public Function ExecuteNonQuery(SQLCommand As String) As Integer
      Dim FBCom As FirebirdSql.Data.FirebirdClient.FbCommand
      Dim Count As Integer = 0

      Try
        If Me.Connection.State = ConnectionState.Closed Then Me.Connection.Open()
        Me.Transaction = Me.Connection.BeginTransaction()
        FBCom = New FirebirdSql.Data.FirebirdClient.FbCommand(SQLCommand, Me.Connection, Me.Transaction)
        Count = FBCom.ExecuteNonQuery()
        Me.Transaction.Commit()
      Catch ex As Exception
        Me.Transaction.Rollback()
        Throw New Exception(ex.Message, ex)
      End Try

      Return Count
    End Function

  End Class

  Partial Public Class PERSONALTableAdapter


  End Class

  Partial Public Class VW_ANWESENHEITTableAdapter
    Public Function ExecuteNonQuery(SQLCommand As String) As Integer
      Dim FBCom As FirebirdSql.Data.FirebirdClient.FbCommand
      Dim Count As Integer = 0

      Try
        If Me.Connection.State = ConnectionState.Closed Then Me.Connection.Open()
        Me.Transaction = Me.Connection.BeginTransaction()
        FBCom = New FirebirdSql.Data.FirebirdClient.FbCommand(SQLCommand, Me.Connection, Me.Transaction)
        Count = FBCom.ExecuteNonQuery()
        Me.Transaction.Commit()
      Catch ex As Exception
        Me.Transaction.Rollback()
        Throw New Exception(ex.Message, ex)
      End Try

      Return Count
    End Function

  End Class

  Partial Class TBL_CRM_FIRMA_IMPTableAdapter

    Public Function ExecuteNonQuery(SQLCommand As String) As Integer
      Dim FBCom As FirebirdSql.Data.FirebirdClient.FbCommand
      Dim Count As Integer = 0

      Try
        If Me.Connection.State = ConnectionState.Closed Then Me.Connection.Open()
        Me.Transaction = Me.Connection.BeginTransaction()
        FBCom = New FirebirdSql.Data.FirebirdClient.FbCommand(SQLCommand, Me.Connection, Me.Transaction)
        Count = FBCom.ExecuteNonQuery()
        Me.Transaction.Commit()
      Catch ex As Exception
        Me.Transaction.Rollback()
        Throw New Exception(ex.Message, ex)
      End Try

      Return Count
    End Function

  End Class

  Partial Class VW_CRM_PROJEKT_EXPTableAdapter

    Public Function ExecuteNonQuery(SQLCommand As String) As Integer
      Dim FBCom As FirebirdSql.Data.FirebirdClient.FbCommand
      Dim Count As Integer = 0

      Try
        If Me.Connection.State = ConnectionState.Closed Then Me.Connection.Open()
        Me.Transaction = Me.Connection.BeginTransaction()
        FBCom = New FirebirdSql.Data.FirebirdClient.FbCommand(SQLCommand, Me.Connection, Me.Transaction)
        Count = FBCom.ExecuteNonQuery()
        Me.Transaction.Commit()
      Catch ex As Exception
        Me.Transaction.Rollback()
        Throw New Exception(ex.Message, ex)
      End Try

      Return Count
    End Function

  End Class

  Partial Class VW_CRM_PERIODENABR_EXPTableAdapter

    Public Function ExecuteNonQuery(SQLCommand As String) As Integer
      Dim FBCom As FirebirdSql.Data.FirebirdClient.FbCommand
      Dim Count As Integer = 0

      Try
        If Me.Connection.State = ConnectionState.Closed Then Me.Connection.Open()
        Me.Transaction = Me.Connection.BeginTransaction()
        FBCom = New FirebirdSql.Data.FirebirdClient.FbCommand(SQLCommand, Me.Connection, Me.Transaction)
        Count = FBCom.ExecuteNonQuery()
        Me.Transaction.Commit()
      Catch ex As Exception
        Me.Transaction.Rollback()
        Throw New Exception(ex.Message, ex)
      End Try

      Return Count
    End Function

  End Class

  Partial Public Class VW_CRM_DEBITOR_EXPTableAdapter

    Public Function ExecuteNonQuery(SQLCommand As String) As Integer
      Dim FBCom As FirebirdSql.Data.FirebirdClient.FbCommand
      Dim Count As Integer = 0

      Try
        If Me.Connection.State = ConnectionState.Closed Then Me.Connection.Open()
        Me.Transaction = Me.Connection.BeginTransaction()
        FBCom = New FirebirdSql.Data.FirebirdClient.FbCommand(SQLCommand, Me.Connection, Me.Transaction)
        Count = FBCom.ExecuteNonQuery()
        Me.Transaction.Commit()
      Catch ex As Exception
        Me.Transaction.Rollback()
        Throw New Exception(ex.Message, ex)
      End Try

      Return Count
    End Function

  End Class
End Namespace
