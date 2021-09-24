

Partial Public Class dsPrinsCRM
  Partial Class dtTransfersDataTable

    Private Sub dtTransfersDataTable_dtTransfersRowChanging(sender As Object, e As dtTransfersRowChangeEvent) Handles Me.dtTransfersRowChanging

    End Sub

    Private _Updating As Integer = 0

    Private Sub dtTransfersDataTable_RowChanged(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowChanged
      Dim Row As dtTransfersRow = e.Row
      If _Updating = 0 AndAlso e.Action = DataRowAction.Change Then
        Try
          _Updating += 1
          Row.ChangedAt = Now
          If Row.IsCreatedAtNull Then Row.CreatedAt = Now
        Finally
          _Updating -= 1
        End Try
      End If
    End Sub

    Private Sub dtTransfersDataTable_RowDeleted(sender As Object, e As DataRowChangeEventArgs) Handles Me.RowDeleted

    End Sub

    Private Sub dtTransfersDataTable_TableNewRow(sender As Object, e As DataTableNewRowEventArgs) Handles Me.TableNewRow
      Dim Row As dtTransfersRow = e.Row
      Row.CreatedAt = Now
    End Sub
  End Class

End Class
