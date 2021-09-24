Imports C1.C1Excel
Imports System.Drawing

Public Class clFlexExcel

  Private _XLStyles As Hashtable
  Private ReadOnly Property XLStyles() As Hashtable
    Get
      If IsNothing(_XLStyles) Then
        _XLStyles = New Hashtable
      End If
      Return _XLStyles
    End Get
  End Property

  Private _XLBook As C1XLBook
  Public ReadOnly Property XLBook() As C1.C1Excel.C1XLBook
    Get
      If IsNothing(_XLBook) Then
        _XLBook = New C1XLBook
      End If
      Return _XLBook
    End Get
  End Property

  Public Sub Close()
    _XLBook.Clear()
    _XLBook.Dispose() : _XLBook = Nothing
    If Not isnothing(_XLStyles) Then
      _XLStyles.Clear() : _XLStyles = Nothing
    End If
  End Sub

  Public Sub SaveSheet(ByVal flex As C1.Win.C1FlexGrid.C1FlexGrid, ByVal sheet As XLSheet, ByVal fixedCells As Boolean)
    SaveSheet(String.Empty, flex, sheet, fixedCells)
  End Sub

  Private ReadOnly Property FlexVisibleColCount(ByVal Flex As C1.Win.C1FlexGrid.C1FlexGrid) As Integer
    Get
      Dim Ct As Integer = 0
      For i As Integer = 0 To Flex.Cols.Count - 1
        If Flex.Cols(i).Visible Then Ct += 1
      Next
      Return Ct
    End Get
  End Property


  Dim rtf As New System.Windows.Forms.RichTextBox
  Public Sub SaveSheet(ByVal Title As String, ByVal flex As C1.Win.C1FlexGrid.C1FlexGrid, ByVal sheet As XLSheet, ByVal fixedCells As Boolean)
    Dim xlr, xlc As Integer
    Dim frows As Integer = flex.Rows.Fixed
    Dim fcols As Integer = flex.Cols.Fixed
    Dim WordRapStyleBold As New XLStyle(sheet.Book)
    Dim WordRapStyleNormal As New XLStyle(sheet.Book)

    WordRapStyleBold.WordWrap = True
    WordRapStyleBold.Font = New Font(flex.Font, FontStyle.Bold)

    WordRapStyleNormal.WordWrap = True
    WordRapStyleNormal.Font = New Font(flex.Font, FontStyle.Regular)

    If fixedCells Then
      frows = 0 : fcols = 0
    End If

    '  copy dimensions
    Dim lastRow As Integer = flex.Rows.Count - frows - 1
    Dim lastCol As Integer = FlexVisibleColCount(flex) - fcols - 1
    If lastRow < 0 OrElse lastCol < 0 Then Exit Sub

    Dim cell As XLCell = sheet(lastRow, lastCol)

    '  set default properties
    sheet.Book.DefaultFont = flex.Font
    sheet.DefaultRowHeight = C1XLBook.PixelsToTwips(flex.Rows.DefaultSize)
    sheet.DefaultColumnWidth = C1XLBook.PixelsToTwips(flex.Cols.DefaultSize)

    '  set row/column properties
    xlr = frows
    For r As Integer = frows To flex.Rows.Count - 1
      '  size/visibility
      Dim fr As C1.Win.C1FlexGrid.Row = flex.Rows(r)
      Dim xr As XLRow = sheet.Rows(xlr - frows)
      If fr.Visible Then
        If fr.Height >= 0 Then xr.Height = C1XLBook.PixelsToTwips(fr.Height)
        xr.Visible = fr.Visible

        '  style
        Dim xs As XLStyle = StyleFromFlex(fr.Style, True)
        'If fr.IsNode Then
        '    xs = StyleFromFlex(flex.Styles(C1.Win.C1FlexGrid.CellStyleEnum.Subtotal0 + fr.Node.Level))
        '    cell.Style = xs
        'End If

        If Not IsNothing(xs) Then xr.Style = xs
        xlr += 1
      End If
    Next

    xlc = fcols
    For c As Integer = fcols To flex.Cols.Count - 1
      '  size/visibility
      Dim fc As C1.Win.C1FlexGrid.Column = flex.Cols(c)
      Dim xc As XLColumn = sheet.Columns(xlc - fcols)

      If fc.Visible Then
        If fc.Width >= 0 Then xc.Width = C1XLBook.PixelsToTwips(fc.Width)
        xc.Visible = fc.Visible

        '  style
        Dim xs As XLStyle = StyleFromFlex(fc.Style, False)
        If Not IsNothing(xs) Then xc.Style = xs
        xlc += 1
      End If
    Next

    '  load cells
    Dim tOffset As Integer = 0
    xlr = frows

    If Not IsNothing(Title) AndAlso Title.Length > 0 Then
      Dim ts As New XLStyle(sheet.Book)
      ts.WordWrap = False : ts.Font = New Font(flex.Font, FontStyle.Bold)

      cell = sheet(xlr - frows, flex.Cols.Fixed)
      cell.Value = Title
      cell.Style = ts
      xlr += 1 : tOffset = 1
    End If

    For r As Integer = frows To flex.Rows.Count - 1
      If flex.Rows(r).Visible Then

        xlc = fcols
        For c As Integer = fcols To flex.Cols.Count - 1
          If flex.Cols(c).Visible Then
            Dim fr As C1.Win.C1FlexGrid.Row = flex.Rows(r)
            '  get cell
            cell = sheet(xlr - frows, xlc - fcols)
            '  apply content
            If xlr - tOffset < flex.Rows.Fixed Then
              If r = 0 AndAlso Not IsNothing(flex.Rows(r).DataMap) AndAlso Not IsNothing(flex(r, c)) _
                       AndAlso flex.Rows(r).DataMap.Contains(flex(r, c)) Then
                cell.Value = My.Application.DBString(flex.Rows(r).DataMap(flex(r, c))).Replace(ControlChars.CrLf, " ")
              Else
                cell.Value = My.Application.DBString(flex(r, c)).Replace(ControlChars.CrLf, ControlChars.Lf)
              End If
              cell.Style = WordRapStyleBold
            Else
              If fr.IsNode AndAlso c = fr.Node.Level Then
                cell.Value = flex(r, flex.Cols.Fixed)
              ElseIf Not fr.IsNode OrElse c > fr.Node.Level + flex.Cols.Fixed Then
                If flex.Cols(c).DataType Is GetType(System.Boolean) AndAlso r >= flex.Rows.Fixed Then
                  cell.Value = My.Application.BoolToYesNo(My.Application.DBBoolean(flex(r, c)))
                ElseIf flex.Cols(c).DataType Is GetType(Byte()) AndAlso r >= flex.Rows.Fixed Then
                  Dim obj As Object = flex(r, c)
                  If Not IsNothing(obj) AndAlso Not IsDBNull(obj) Then
                    Dim ar As Array = obj
                    Dim tmp As String = System.Text.Encoding.UTF7.GetString(ar, 0, ar.Length)

                    If tmp.StartsWith("{\rtf") Then

                      Dim txt As String = String.Empty
                      rtf.Rtf = tmp

                      For i As Integer = 0 To rtf.Lines.Length - 1
                        If rtf.Lines(i).Trim.Length > 0 Then
                          If txt.Length > 0 Then txt += ControlChars.CrLf
                          txt += rtf.Lines(i)
                        End If
                      Next
                      cell.Value = txt.Replace(ControlChars.CrLf, ControlChars.Lf)
                      If CType(cell.Value, String).IndexOf(ControlChars.Lf) > 0 Then cell.Style = WordRapStyleNormal
                    End If

                  End If
                Else
                  If Not IsNothing(flex(r, c)) AndAlso TypeOf flex(r, c) Is String Then
                    cell.Value = My.Application.DBString(flex(r, c)).Replace(ControlChars.CrLf, ControlChars.Lf)
                    If CType(cell.Value, String).IndexOf(ControlChars.Lf) > 0 Then cell.Style = WordRapStyleNormal
                  Else
                    If Not IsNothing(flex.Cols(c).DataMap) Then
                      Dim Value As Object = flex(r, c)
                      Try
                        If Not IsNothing(Value) Then
                          Value = CType(flex.Cols(c).DataMap, IDictionary)(Value)
                        End If
                      Catch ex As Exception
                        Debug.WriteLine(ex.Message)
                      End Try
                      cell.Value = Value
                    Else
                      cell.Value = flex(r, c)
                    End If

                  End If

                End If
              End If
            End If

            '  apply style
            Dim xs As XLStyle = StyleFromFlex(flex.GetCellStyle(r, c), False)
            If IsNothing(xs) Then
              xs = StyleFromFlex(fr.Style, True)
              If Not IsNothing(xs) Then
                Dim cs As XLStyle = StyleFromFlex(flex.Cols(c).Style, False)
                If Not IsNothing(cs) Then xs.Format = cs.Format
              End If
            End If
            If Not IsNothing(xs) Then cell.Style = xs
            xlc += 1
          End If
        Next

        xlr += 1
      End If
    Next
  End Sub

  Private Function StyleFromExcel(ByVal flex As C1.Win.C1FlexGrid.C1FlexGrid, ByVal style As XLStyle) As C1.Win.C1FlexGrid.CellStyle
    ' sanity
    If IsNothing(style) Then Return Nothing

    ' look it up on list
    'If (Me.XLStyles.Contains(style)) Then Return Me.XLStyles(style)

    ' create new flex style
    Dim cs As C1.Win.C1FlexGrid.CellStyle = flex.Styles.Add(Me.XLStyles.Count.ToString)

    ' set up new style
    If Not IsNothing(style.Font) Then cs.Font = style.Font
    If Not style.ForeColor.Equals(Color.Transparent) Then cs.ForeColor = style.ForeColor
    If Not style.BackColor.Equals(Color.Transparent) Then cs.BackColor = style.BackColor
    If (style.Rotation = 90) Then cs.TextDirection = C1.Win.C1FlexGrid.TextDirectionEnum.Up
    If (style.Rotation = 180) Then cs.TextDirection = C1.Win.C1FlexGrid.TextDirectionEnum.Down
    If (Not IsNothing(style.Format) AndAlso style.Format.Length > 0) Then cs.Format = XLStyle.FormatXLToDotNet(style.Format)

    Select Case style.AlignHorz

      Case XLAlignHorzEnum.Center
        cs.WordWrap = style.WordWrap
        Select Case style.AlignVert
          Case XLAlignVertEnum.Top
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterTop

          Case XLAlignVertEnum.Center
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

          Case Else
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterBottom
        End Select

      Case XLAlignHorzEnum.Right
        cs.WordWrap = style.WordWrap
        Select Case style.AlignVert
          Case XLAlignVertEnum.Top
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightTop

          Case XLAlignVertEnum.Center
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter

          Case XLAlignVertEnum.Bottom
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightBottom

        End Select

      Case XLAlignHorzEnum.Left
        cs.WordWrap = style.WordWrap
        Select Case style.AlignVert
          Case XLAlignVertEnum.Top
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftTop
          Case XLAlignVertEnum.Center
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter

          Case Else
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftBottom
        End Select

      Case Else
        cs.WordWrap = style.WordWrap
        Select Case style.AlignVert
          Case XLAlignVertEnum.Top
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.GeneralTop

          Case XLAlignVertEnum.Center
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.GeneralCenter

          Case Else
            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.GeneralBottom
        End Select
    End Select

    ' save it
    'Me.XLStyles.Add(style, cs)

    ' return it
    Return cs
  End Function

  Private Function StyleFromFlex(ByVal style As C1.Win.C1FlexGrid.CellStyle, ByVal CloneColor As Boolean) As XLStyle

    ' sanity
    If IsNothing(style) Then Return Nothing

    ' look it up on list
    'If Me.XLStyles.Contains(style) Then Return Me.XLStyles(style)

    ' create new Excel style
    Dim xs As XLStyle = New XLStyle(Me.XLBook)

    ' set up new style
    xs.Font = style.Font
    If CloneColor AndAlso Not style.BackColor.Equals(SystemColors.Window) Then
      xs.BackColor = style.BackColor
    End If

    xs.WordWrap = style.WordWrap
    xs.Format = XLStyle.FormatDotNetToXL(style.Format)
    Select Case style.TextDirection
      Case C1.Win.C1FlexGrid.TextDirectionEnum.Up
        xs.Rotation = 90
      Case C1.Win.C1FlexGrid.TextDirectionEnum.Down
        xs.Rotation = 180
    End Select

    Select Case style.TextAlign
      Case C1.Win.C1FlexGrid.TextAlignEnum.CenterBottom
        xs.AlignHorz = XLAlignHorzEnum.Center
        xs.AlignVert = XLAlignVertEnum.Bottom

      Case C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        xs.AlignHorz = XLAlignHorzEnum.Center
        xs.AlignVert = XLAlignVertEnum.Center

      Case C1.Win.C1FlexGrid.TextAlignEnum.CenterTop
        xs.AlignHorz = XLAlignHorzEnum.Center
        xs.AlignVert = XLAlignVertEnum.Top

      Case C1.Win.C1FlexGrid.TextAlignEnum.GeneralBottom
        xs.AlignHorz = XLAlignHorzEnum.General
        xs.AlignVert = XLAlignVertEnum.Bottom

      Case C1.Win.C1FlexGrid.TextAlignEnum.GeneralCenter
        xs.AlignHorz = XLAlignHorzEnum.General
        xs.AlignVert = XLAlignVertEnum.Center

      Case C1.Win.C1FlexGrid.TextAlignEnum.GeneralTop
        xs.AlignHorz = XLAlignHorzEnum.General
        xs.AlignVert = XLAlignVertEnum.Top

      Case C1.Win.C1FlexGrid.TextAlignEnum.LeftBottom
        xs.AlignHorz = XLAlignHorzEnum.Left
        xs.AlignVert = XLAlignVertEnum.Bottom

      Case C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        xs.AlignHorz = XLAlignHorzEnum.Left
        xs.AlignVert = XLAlignVertEnum.Center

      Case C1.Win.C1FlexGrid.TextAlignEnum.LeftTop
        xs.AlignHorz = XLAlignHorzEnum.Left
        xs.AlignVert = XLAlignVertEnum.Top

      Case C1.Win.C1FlexGrid.TextAlignEnum.RightBottom
        xs.AlignHorz = XLAlignHorzEnum.Right
        xs.AlignVert = XLAlignVertEnum.Bottom

      Case C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        xs.AlignHorz = XLAlignHorzEnum.Right
        xs.AlignVert = XLAlignVertEnum.Center

      Case C1.Win.C1FlexGrid.TextAlignEnum.RightTop
        xs.AlignHorz = XLAlignHorzEnum.Right
        xs.AlignVert = XLAlignVertEnum.Top

      Case Else
        Debug.Assert(False)
    End Select

    ' save it
    'Me.XLStyles.Add(style, xs)

    ' return it
    Return xs
  End Function


End Class
