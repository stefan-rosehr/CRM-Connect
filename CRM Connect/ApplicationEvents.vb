Imports C1.C1Excel
Imports C1.Win.C1FlexGrid.Extensions

Namespace My


  ' Für MyApplication sind folgende Ereignisse verfügbar:
  ' 
  ' Startup: Wird beim Starten der Anwendung noch vor dem Erstellen des Startformulars ausgelöst.
  ' Shutdown: Wird nach dem Schließen aller Anwendungsformulare ausgelöst. Dieses Ereignis wird nicht ausgelöst, wenn die Anwendung nicht normal beendet wird.
  ' UnhandledException: Wird ausgelöst, wenn in der Anwendung ein Ausnahmefehler auftritt.
  ' StartupNextInstance: Wird beim Starten einer Einzelinstanzanwendung ausgelöst, wenn diese bereits aktiv ist. 
  ' NetworkAvailabilityChanged: Wird beim Herstellen oder Trennen der Netzwerkverbindung ausgelöst.
  Partial Friend Class MyApplication

    Private _Logging As clMessageLog = Nothing
    Public ReadOnly Property Logging As clMessageLog
      Get
        If IsNothing(_Logging) Then
          _Logging = New clMessageLog
        End If

        Return _Logging
      End Get
    End Property


    Public Function Ask(ByVal Message As String, ByVal Buttons As Microsoft.VisualBasic.MsgBoxStyle, ByVal Title As String) As Microsoft.VisualBasic.MsgBoxResult
      Return MsgBox(Me.ReplaceCrLf(Message), Buttons, Title)
    End Function

    Public Function ReplaceCrLf(ByVal Value As String) As String
      Dim result As String = String.Empty
      result = Value.Replace(" \CrLf ", ControlChars.CrLf)
      result = result.Replace(" \CrLf", ControlChars.CrLf)
      result = result.Replace("\CrLf ", ControlChars.CrLf)
      result = result.Replace("\CrLf", ControlChars.CrLf)
      Return result
    End Function

    Public Function ByteToHexString(BinaryHex As System.Byte()) As String
      Dim Result As String = String.Empty

      If Not IsNothing(BinaryHex) AndAlso Not IsDBNull(BinaryHex) Then
        For i As Integer = 0 To BinaryHex.Length - 1
          Result += Hex(BinaryHex(i)).PadLeft(2, "0")
        Next
      End If
      Return Result
    End Function

    Public Function SetRowValueIfChanged(Row As DataRow, ColName As String, [Value] As Object) As Boolean
      Dim IsChanged As Boolean = False

      If Not Row.IsNull(ColName) AndAlso Not IsDBNull(Value) Then
        If Row.Table.Columns(ColName).DataType Is GetType(System.Byte()) Then
          If Not ByteArrayCompare(Row(ColName), Value) Then
            IsChanged = True
            Row(ColName) = Value
          End If
        ElseIf Row(ColName) <> Value Then
          IsChanged = True
          Row(ColName) = Value
        End If
      Else
        ' ein Wert gesetzt
        If Not Row.IsNull(ColName) OrElse Not IsDBNull(Value) Then
          IsChanged = True
          Row(ColName) = Value
        End If
      End If

      Return IsChanged
    End Function


    Public Function ByteArrayCompare(a1 As System.Byte(), a2 As System.Byte()) As Boolean
      Dim IsEqual As Boolean = True

      If Not IsNothing(a1) AndAlso Not IsNothing(a2) Then
        If a1.Length <> a2.Length Then
          IsEqual = False
        Else
          For i As Integer = 0 To a1.Length - 1
            If a1(i) <> a2(i) Then
              IsEqual = False : Exit For
            End If
          Next
        End If
      End If

      Return IsEqual
    End Function

    Public ReadOnly Property DBCurrency(ByVal FieldValue As Object) As String
      Get
        Return String.Format("{0:C}", Me.DBDouble(FieldValue))
      End Get
    End Property

    Public ReadOnly Property MaxString(ByVal Field As Object, ByVal MaxLength As Integer) As String
      Get
        Dim tmp As String = Me.DBString(Field)
        Return tmp.Substring(0, Math.Min(tmp.Length, MaxLength))
      End Get
    End Property


    Public ReadOnly Property DBInteger(ByVal Field As Object) As Integer
      Get
        Dim Value As Integer = 0
        Try
          If Not IsNothing(Field) AndAlso Not IsDBNull(Field) AndAlso _
             (Not TypeOf Field Is String OrElse Not String.IsNullOrEmpty(Field)) Then
            Value = CType(Field, Integer)
          End If
        Catch
          Value = 0
        End Try
        Return Value
      End Get
    End Property


    Public ReadOnly Property DBDouble(ByVal Field As Object) As Double
      Get
        Dim Value As Double = 0
        Try
          If Not IsNothing(Field) AndAlso Not IsDBNull(Field) Then
            If TypeOf (Field) Is String Then
              If Not String.IsNullOrEmpty(Field) Then
                Dim tmp As String = Me.DBString(Field)
                Dim a1, a2 As Integer
                Dim Sep As Integer
                ' ------------------------------------------------------------------------------------------------------------
                tmp = Me.StripNonDecimal(tmp) ' läßt - oder , stehen
                a1 = tmp.LastIndexOf(".") : a2 = tmp.LastIndexOf(",")
                Sep = IIf(a1 > a2, a1, a2)
                ' ------------------------------------------------------------------------------------------------------------
                If Sep > 0 Then
                  tmp = tmp.Substring(0, Sep).Replace(".", "").Replace(",", "") + System.Globalization.CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator + _
                        tmp.Substring(Sep + 1)
                End If
                If Me.HasDigit(tmp) Then Value = CDbl(tmp)
              End If
            Else
              Value = CType(Field, Double)
            End If

          End If
        Catch
          Value = 0
        End Try

        Return Value
      End Get
    End Property

    Public ReadOnly Property HasDigit(ByVal StringValue As String) As Boolean
      Get
        Dim Result As Boolean = False
        Dim n As Integer, ch As Char

        For n = 1 To Len(StringValue)
          ch = Mid(StringValue, n, 1).Chars(0)
          If Me.IsDigit(ch) Then
            Result = True : Exit For
          End If
        Next n
        Return Result
      End Get
    End Property

    Public ReadOnly Property IsDigit(ByVal CharValue As Char) As Boolean
      Get
        Return Char.IsDigit(CharValue)
      End Get
    End Property

    Public ReadOnly Property BoolToYesNo(ByVal BoolValue As Boolean) As String
      Get
        Dim Result As String
        Result = If(BoolValue, "Ja", "Nein")
        Return Result
      End Get
    End Property


    Public Function StripNonDecimal(ByVal NonNumeric As String) As String
      Dim Numeric As String = ""
      Dim n As Integer, ch As Char

      For n = 1 To Len(NonNumeric)
        ch = Mid(NonNumeric, n, 1).Chars(0)
        If Char.IsDigit(ch) Or ch = "," Or ch = "." Or ch = "-" Then
          Numeric += ch
        End If
      Next n

      Return Numeric
    End Function

    Public Function StripNonNumeric(ByVal NonNumeric As String) As String
      Dim Numeric As String = ""
      Dim n As Integer, ch As Char

      For n = 1 To Len(NonNumeric)
        ch = Mid(NonNumeric, n, 1).Chars(0)
        If Char.IsDigit(ch) Then
          Numeric += ch
        End If
      Next n

      Return Numeric
    End Function


    Public Function StripControlChars(ByVal NonNumeric As String) As String
      Dim Result As String = String.Empty
      Dim n As Integer, ch As Char

      For n = 1 To Len(NonNumeric)
        ch = Mid(NonNumeric, n, 1).Chars(0)
        If Not Char.IsControl(ch) AndAlso Not Char.IsSymbol(ch) Then
          Result += ch
        End If
      Next n

      Return Result
    End Function


    Public Function StripNonLetterOrDigit(ByVal NonNumeric As String) As String
      Dim Result As String = String.Empty
      Dim n As Integer, ch As Char

      For n = 1 To Len(NonNumeric)
        ch = Mid(NonNumeric, n, 1).Chars(0)
        If Char.IsLetterOrDigit(ch) Or ch = " " Then
          Result += ch
        End If
      Next n

      Return Result
    End Function

    Public Function StripDigit(ByVal StringValue As String) As String
      Dim Result As String = String.Empty
      Dim n As Integer, ch As Char

      For n = 1 To Len(StringValue)
        ch = Mid(StringValue, n, 1).Chars(0)
        If Not Char.IsDigit(ch) Then
          Result += ch
        End If
      Next n

      Return Result
    End Function

    Public Sub Swap(ByRef String1 As String, ByRef String2 As String)
      Dim tmp As String = String1
      String1 = String2
      String2 = tmp
    End Sub


    Public ReadOnly Property DBString(ByVal Field As Object) As String
      Get
        Dim Value As String = String.Empty
        Try
          If Not IsNothing(Field) AndAlso Not IsDBNull(Field) Then
            Value = CType(Field, String)
          End If
        Catch
          Value = String.Empty
        End Try
        Return Value
      End Get
    End Property

    Public ReadOnly Property DBBoolean(ByVal Field As Object) As Boolean
      Get
        Dim Value As Boolean = False
        Try
          If Not IsNothing(Field) AndAlso Not IsDBNull(Field) AndAlso _
             (Not TypeOf Field Is String OrElse Not String.IsNullOrEmpty(Field)) Then
            Value = CType(Field, Boolean)
          End If
        Catch
          Value = False
        End Try
        Return Value
      End Get
    End Property



    Private _FlexExcel As clFlexExcel
    Public ReadOnly Property FlexExcel() As clFlexExcel
      Get
        If IsNothing(_FlexExcel) Then
          _FlexExcel = New clFlexExcel()
        End If
        Return _FlexExcel
      End Get
    End Property


    Public Sub SaveAsExcelAndStart(FlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, Optional ByVal UseGridDefaultExport As Boolean = False)
      Dim FName As String = System.IO.Path.GetTempFileName + ".xls"
      SaveGridAsExcel(FlexGrid, FName, True, UseGridDefaultExport)
      ' -----------------------------------------------------------------------------------
      Try
        System.Diagnostics.Process.Start(FName)
      Catch ex As Exception
        Debug.WriteLine(ex.Message)
        My.Application.Ask("Der Prozess konnte nicht gestartet werden", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, My.Resources.Bestätigung)
      End Try
      ' -----------------------------------------------------------------------------------
    End Sub

    Public Sub SaveGridAsExcel(FlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, Optional ByVal UseGridDefaultExport As Boolean = False)
      Dim Dlg As SaveFileDialog = New SaveFileDialog

      Dlg.DefaultExt = "xls"
      Dlg.FileName = "*.xls"
      Dlg.Filter = "Microsoft Excel (*.xls)|*.xls"
      Dlg.FilterIndex = 0
      Dlg.AddExtension = True
      Dlg.CheckPathExists = True
      Dlg.OverwritePrompt = True

      If Dlg.ShowDialog() = DialogResult.OK Then
        SaveGridAsExcel(FlexGrid, Dlg.FileName, True, UseGridDefaultExport)
        ' -----------------------------------------------------------------------------------
        ' Tabelle wurde erfolgreich gespeichert : {0}
        My.Forms.frmMain.InfoErrorMessage = String.Format("Tabelle wurde erfolgreich gespeichert : {0}", Dlg.FileName)
      End If
    End Sub

    Public Sub SaveGridAsExcel(FlexGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal FullFileName As String, ByVal OverwriteExisting As Boolean, Optional ByVal UseGridDefaultExport As Boolean = False)
      Dim FName As String = FullFileName
      If Not FName.ToLower.EndsWith(".xls") Then FName = FName + ".xls"

      ' -----------------------------------------------------------------------------------
      If System.IO.File.Exists(FName) Then
        If OverwriteExisting Then
          System.IO.File.Delete(FName)
        Else
          Throw New Exception("File already exisist : " + FName)
        End If
      End If
      ' -----------------------------------------------------------------------------------
      If UseGridDefaultExport Then
        FlexGrid.SaveExcel(FullFileName, C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells Or C1.Win.C1FlexGrid.FileFlags.AsDisplayed)

      Else
        My.Application.FlexExcel.XLBook.Clear()
        My.Application.FlexExcel.XLBook.Sheets.Clear()
        Dim sheet As XLSheet = My.Application.FlexExcel.XLBook.Sheets.Add()

        ' -----------------------------------------------------------------------------------
        If Not IsNothing(FlexGrid.Tag) AndAlso My.Application.DBString(FlexGrid.Tag).Length > 0 Then
          Dim tmp As String = My.Application.StripNonLetterOrDigit(My.Application.DBString(FlexGrid.Tag))
          sheet.Name = tmp.Substring(0, Math.Min(tmp.Length, 31))
        End If
        ' -----------------------------------------------------------------------------------
        My.Application.FlexExcel.SaveSheet(My.Application.DBString(FlexGrid.Tag), FlexGrid, sheet, True)
        sheet.PrintSettings.Landscape = True
        sheet.PrintSettings.Header = "&C&A"
        sheet.PrintSettings.Footer = "&L&P / &N&R&Z&F"
        sheet.PrintSettings.FitPagesAcross = 1
        sheet.PrintSettings.FitPagesDown = 0
        sheet.PrintSettings.MarginLeft = 0.78740157480315
        sheet.PrintSettings.MarginRight = 0.78740157480315
        sheet.PrintSettings.MarginBottom = 0.78740157480315
        sheet.Columns.Remove(sheet.Columns(0))
        ' -----------------------------------------------------------------------------------
        My.Application.FlexExcel.XLBook.Save(FName)
        My.Application.FlexExcel.Close()

      End If
    End Sub

    Private _RestartApplicationOnException As Boolean = False
    Public Property RestartApplicationOnException As Boolean
      Get
        Return _RestartApplicationOnException
      End Get
      Set(value As Boolean)
        _RestartApplicationOnException = value
      End Set
    End Property


    Private Sub MyApplication_UnhandledException(sender As Object, e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
      Dim errorMsg As System.IO.StringWriter

      errorMsg = New System.IO.StringWriter
      errorMsg.WriteLine("Ein Fehler ist in der Anwendung aufgetreten, bitte drucken Sie diese Seite aus oder")
      errorMsg.WriteLine("kopieren den Bildschirminhalt in eine Datei und übermitteln sie dem Programm-Administrator:")
      errorMsg.WriteLine("")
      errorMsg.WriteLine(e.Exception.Message)
      errorMsg.WriteLine("")
      errorMsg.WriteLine("Stack Trace:")
      errorMsg.WriteLine(e.Exception.StackTrace)
      Console.WriteLine(errorMsg.ToString)

      If Not RestartApplicationOnException AndAlso e.Exception.Message.Length > 0 Then
        MessageBox.Show(errorMsg.ToString, "Anwendungs-Fehler", MessageBoxButtons.OK, MessageBoxIcon.Stop)
      End If
      e.ExitApplication = RestartApplicationOnException

      If RestartApplicationOnException Then

        Dim cmd As String = System.Windows.Forms.Application.ExecutablePath
        Dim si As New System.Diagnostics.ProcessStartInfo(cmd)
        si.UseShellExecute = True
        si.WorkingDirectory = System.Environment.CurrentDirectory

        System.Diagnostics.Process.Start(si)
        Application.DoEvents()

      End If
    End Sub

  End Class


End Namespace

