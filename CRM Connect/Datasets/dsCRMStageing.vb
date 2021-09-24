Partial Class dsCRMStageing
  Partial Public Class MitarbeiterDataTable
    Private Sub MitarbeiterDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
      If (e.Column.ColumnName = Me.guidColumn.ColumnName) Then
        'Benutzercode hier einfügen
      End If

    End Sub

  End Class

  Partial Public Class DebitorDataTable
    Private Sub DebitorDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
      If (e.Column.ColumnName = Me.WaehrungColumn.ColumnName) Then
        'Benutzercode hier einfügen
      End If

    End Sub

  End Class

  Partial Public Class AnwesenheitDataTable
    Private Sub AnwesenheitDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
      If (e.Column.ColumnName = Me.IDColumn.ColumnName) Then
        'Benutzercode hier einfügen
      End If

    End Sub

  End Class

  Partial Public Class ProjektDataTable
    Private Sub ProjektDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
      If (e.Column.ColumnName = Me.IdColumn.ColumnName) Then
        'Benutzercode hier einfügen
      End If

    End Sub

  End Class
End Class

Namespace dsCRMStageingTableAdapters
  Partial Public Class AnwesenheitTableAdapter
  End Class

  Partial Public Class VW_TBL_CRM_Firma_IMPTableAdapter

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
