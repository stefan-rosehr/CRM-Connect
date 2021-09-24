Partial Class dsTransfer
  Partial Class dtTransferDataTable

    Private _Updating As Integer = 0

    Private Sub dtTransferDataTable_RowChanged(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanged
      If _Updating = 0 AndAlso e.Action = DataRowAction.Change Then
        Dim Row As dtTransferRow = e.Row
        Try
          _Updating += 1
          Row.ChangedAt = Now
          If Row.IsCreatedAtNull Then Row.CreatedAt = Now
        Finally
          _Updating -= 1
        End Try
      End If
    End Sub

    Private Sub dtTransferDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As dtTransferRow = e.Row
      Row.CreatedAt = Now
    End Sub

    Private Sub dtTransferDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
      If (e.Column.ColumnName = Me.RepeatPerHourColumn.ColumnName) Then
        If IsDBNull(e.ProposedValue) OrElse CInt(e.ProposedValue) < 1 OrElse CInt(e.ProposedValue) > 4 Then
          e.Row.SetColumnError(e.Column, "Wert muss zwischen 1 und 4 liegen!")
        Else
          e.Row.SetColumnError(e.Column, String.Empty)
        End If
      End If

    End Sub

  End Class

End Class
