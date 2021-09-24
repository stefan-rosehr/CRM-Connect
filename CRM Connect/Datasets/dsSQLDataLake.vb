Partial Class dsSQLDataLake
  Partial Public Class VW_GeneralJournalEntryDiffDataTable

    Private _TableAdapter As dsSQLDataLakeTableAdapters.VW_GeneralJournalEntryDiffTableAdapter
    Public ReadOnly Property TableAdapter As dsSQLDataLakeTableAdapters.VW_GeneralJournalEntryDiffTableAdapter
      Get
        If IsNothing(_TableAdapter) Then
          _TableAdapter = New dsSQLDataLakeTableAdapters.VW_GeneralJournalEntryDiffTableAdapter
        End If
        Return _TableAdapter
      End Get
    End Property

  End Class

  Partial Public Class GeneralJournalAccountEntryStagingDataTable
  End Class
End Class

Namespace dsSQLDataLakeTableAdapters

  Partial Public Class VW_GeneralJournalEntryDiffTableAdapter
    Public Function ExecuteNonQuery(SQLCommand As String) As Integer
      Dim SqlCom As SqlClient.SqlCommand
      Dim Count As Integer = 0

      Try
        If Me.Connection.State = ConnectionState.Closed Then Me.Connection.Open()
        Me.Transaction = Me.Connection.BeginTransaction()
        SqlCom = New SqlClient.SqlCommand(SQLCommand, Me.Connection, Me.Transaction)
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
