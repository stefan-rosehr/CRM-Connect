Partial Class dsHPCReweSKR04
End Class

Namespace dsHPCReweSKR04TableAdapters

  Partial Public Class KHKBuchungByOpTableAdapter

    Public Function ExecuteNonQuery(SQLCommand As String) As Integer
      Dim SqlCom As Data.SqlClient.SqlCommand
      Dim Count As Integer = 0

      Try
        If Me.Connection.State = ConnectionState.Closed Then Me.Connection.Open()
        Me.Transaction = Me.Connection.BeginTransaction()
        SqlCom = New Data.SqlClient.SqlCommand(SQLCommand, Me.Connection, Me.Transaction)
        Count = SqlCom.ExecuteNonQuery()
        Me.Transaction.Commit()
      Catch ex As Exception
        Me.Transaction.Rollback()
        Throw New Exception(ex.Message, ex)
      End Try

      Return Count
    End Function
  End Class

End Namespace
