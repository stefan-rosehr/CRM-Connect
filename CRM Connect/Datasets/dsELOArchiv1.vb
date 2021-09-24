Partial Class dsELOArchiv1

  'hpc_projects_costcenter
  Partial Public Class hpc_projects_costcenterDataTable


    Public Event AfterSaveChanges(sender As Object, e As System.EventArgs)
    Public ReadOnly Property TableAdapterObject As Object
      Get
        Return TableAdapter
      End Get
    End Property


    Private _TableAdapter As dsELOArchiv1TableAdapters.hpc_projects_costcenterTableAdapter
    Public ReadOnly Property TableAdapter As dsELOArchiv1TableAdapters.hpc_projects_costcenterTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsELOArchiv1TableAdapters.hpc_projects_costcenterTableAdapter
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

    Private _DoingSaveChanges As Boolean = False
    Public Sub SaveChanges()
      If _DoingSaveChanges Then Exit Sub
      ' ------------------------------------------------------------------------
      Dim ChangesSaved As Boolean = False

      Try
        _DoingSaveChanges = True

        Dim ChangedTable As hpc_projects_costcenterDataTable = Me.GetChanges()

        If Not IsNothing(ChangedTable) Then
          Dim Row As hpc_projects_costcenterRow

          For idx As Integer = ChangedTable.Rows.Count - 1 To 0 Step -1
            Dim ResultTable As hpc_projects_costcenterDataTable
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

                Dim SourceRow As hpc_projects_costcenterRow
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

    Public Sub Reload(Optional ReloadOptions As DataTableReloadOptions = DataTableReloadOptions.SaveChanges_AbortAtError)
      If HasChanges Then
        If ReloadOptions <> DataTableReloadOptions.RejectChanges Then
          Me.SaveChanges()
        End If
        '----------------------------------------------------------------------------------
      End If

      If Not HasChanges OrElse (ReloadOptions = DataTableReloadOptions.RejectChanges OrElse ReloadOptions = DataTableReloadOptions.SaveChanges_IgnoreErrors) Then
        TableAdapter.Fill(Me)
      End If

    End Sub

    Private Sub hpc_projects_costcenterDataTable_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanged
      _HasChanges = True
    End Sub

    Private Sub hpc_projects_costcenterDataTable_RowChanging(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanging
      If e.Action = DataRowAction.Change Then

        For Each Col As DataColumn In Me.Columns
          Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
          hpc_projects_costcenterDataTable_ColumnChanging(Me, eva)
        Next
      End If
    End Sub

    Private Sub hpc_projects_costcenterDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As hpc_projects_costcenterRow = e.Row

      For Each Col As DataColumn In Me.Columns
        Dim eva As New DataColumnChangeEventArgs(e.Row, Col, e.Row(Col))
        hpc_projects_costcenterDataTable_ColumnChanging(Me, eva)
      Next

    End Sub

    Private Sub hpc_projects_costcenter_hpc_projects_costcenterRowChanged(sender As Object, e As hpc_projects_costcenterRowChangeEvent) Handles Me.hpc_projects_costcenterRowChanged
      If e.Action = DataRowAction.Commit OrElse e.Action = DataRowAction.Rollback Then
        _HasChanges = False
      End If
    End Sub

    Private Sub hpc_projects_costcenterDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging

    End Sub

    Private Sub hpc_projects_costcenterDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted
      If e.Action = DataRowAction.Delete Then
        _HasChanges = Not IsNothing(Me.GetChanges)
      End If
    End Sub



  End Class

  Public Enum DataTableReloadOptions
    SaveChanges_IgnoreErrors
    SaveChanges_AbortAtError
    RejectChanges
  End Enum


End Class

