Public Sub AlterData()
    
    Application.ScreenUpdating = False
    
    Dim acquisitionYear As Integer
    Dim integration As Integer
    Dim original As Integer
    Dim sic As Integer
    Dim focalname As Variant
        
    acquisitionYear = Application.InputBox("Enter acquisition year:")
    integration = Application.InputBox("Enter integration (0) or preservation (1):")
    original = Application.InputBox("Original (0) or acquired (1):")
    sic = Application.InputBox("SIC code of focal firm:")
    focalname = Application.InputBox("Ultimate parent name of current firm:")
    
    Call MoveFromUnres
    
    Call DeleteUnres
    
    Call MoveColumnsOnEverySheet
    
    Call AddColumnsWithValues(acquisitionYear, integration, original, sic, focalname)
    
    Application.ScreenUpdating = True

End Sub

Private Sub MoveFromUnres()
    Dim src As Worksheet
    Dim trgt As Worksheet
    Dim numberOfSheets As Integer
    Dim i As Long
    
    numberOfSheets = Worksheets.Count
    
    For s = 2 To numberOfSheets Step 2
        Set src = Sheets(s)
        Set trgt = Sheets(s - 1)
        
        For i = 2 To src.Range("A" & Rows.Count).End(xlUp).Row
            Call CopyPaste(src, i, trgt, True)
        Next i
    Next s
End Sub

Private Sub DeleteUnres()
    Dim numberOfSheets As Integer
    Dim trgt As Worksheet
    
    numberOfSheets = Worksheets.Count
    
    ' Turn of alerts to prevent confirmation asking
    Application.DisplayAlerts = False
    
    For s = numberOfSheets To 2 Step -2
        ' Delete the unres sheet
        Set trgt = Sheets(s)
        trgt.Delete
    Next s
    
    ' Turn the alerts back on
    Application.DisplayAlerts = True

End Sub

Private Sub MoveColumnsOnEverySheet()
    Dim OriginalColumnNumber As Integer
    Dim ColumnToPlace As Integer
    
    ' Declare Current as a worksheet object variable.
    Dim Current As Worksheet
    
    ' Define the columns to be moved in a sheet.
    Dim ColumnsToMove As Variant
    ColumnsToMove = Array("A", "B", "I", "J", "M", "N", "O", "R", "S", "T", "U", "Z", "AA", "AD", "AE", "AG", "AL", "AN", "AO", "BA", "BG", "CG", "CH", "CO", "CQ")
    
    ' Loop through all selected sheets.
    For Each Current In Worksheets
        ' Find the last column that is used
        OriginalColumnNumber = Current.UsedRange.Columns.Count
        ' Use the first empty column on the right
        ColumnToPlace = Current.UsedRange.Columns.Count + 1
        
        ' Loop through all columns that have to be moved.
        For Each ColumnToMove In ColumnsToMove
            ' Copy the selected column and place it on the new column
            Current.Columns(ColumnToMove).Copy
            Current.Columns(ColumnToPlace).Insert
            ' Add one to change the next placing position one to the right
            ColumnToPlace = ColumnToPlace + 1
        Next
        
        ' Delete all empty columns at the beginning to move everything to the left
        Current.Range(Current.Columns(1), Current.Columns(OriginalColumnNumber)).EntireColumn.Delete
    Next

End Sub

Private Sub AddColumnsWithValues(acquisitionYear As Integer, integration As Integer, original As Integer, sic As Integer, focalname As Variant)
    Dim Current As Worksheet
    Dim firstColumn As Boolean
    Dim endYear As Integer
    Dim tmp As Integer
    Dim x As Integer
    
    firstColumn = True
    endYear = 1996 + 5
        
    For Each Current In Worksheets
        Current.Columns("A:E").Insert Shift:=xlToRight, _
            CopyOrigin:=xlFormatFromLeftOrAbove 'or xlFormatFromRightOrBelow
    
        Current.Cells(1, 1).Value = "Original_Alliance"
        Current.Cells(1, 2).Value = "Integrated_Alliance"
        Current.Cells(1, 3).Value = "Acquisition_Year"
        Current.Cells(1, 4).Value = "Alliance_EndDate"
        Current.Cells(1, 5).Value = "Industrial_Diversity"
        
        For i = 2 To Current.Range("F" & Rows.Count).End(xlUp).Row
            
            Current.Cells(i, 1).Value = original
            Current.Cells(i, 2).Value = integration
            Current.Cells(i, 3).Value = acquisitionYear
            Current.Cells(i, 4).Value = endYear
            
            If TypeName(Current.Cells(i, 15).Value) = "Double" Then
                
                tmp = Int(Current.Cells(i, 15).Value)
                tmp = tmp / 100
                x = sic / 100
                
                If tmp = 48 Then
                    x = 5
                End If
                
                tmp = Int(Current.Cells(i, 15).Value)
                
        If StrComp(Current.Cells(i, 11).Value, focalname) = 0 Then
            Current.Cells(i, 5).Value = -1
        Else
                    If tmp = sic Then
                            Current.Cells(i, 5).Value = 4
                    ElseIf Int(tmp / 10) = Int(sic / 10) Then
                                Current.Cells(i, 5).Value = 3
                    ElseIf Int(tmp / 100) = Int(sic / 100) Then
                            Current.Cells(i, 5).Value = 2
                    ElseIf Int(tmp / 1000) = Int(sic / 1000) Then
                        Current.Cells(i, 5).Value = 1
                    Else
                            Current.Cells(i, 5).Value = 0
                    End If
        End If
            Else
                Current.Cells(i, 5).Value = 0
            End If
            
        Next i
        
        endYear = endYear + 1
    Next

End Sub

' This sub copoes and pastes the entire row into a different sheet below the last used row
Private Sub CopyPaste(ByRef src As Worksheet, ByVal i As Long, ByRef trgt As Worksheet, pastOnNextRow As Boolean)
    ' Initializing variables
    Dim nxtRow As Long
    
    ' Activate source sheet and copy data
    src.Activate
    src.Rows(i & ":" & i).Copy
    
    ' Now activate target sheet
    trgt.Activate
    
    ' If data has to be pasted below the last row, add one to nextRow
    If pastOnNextRow Then
        nxtRow = trgt.Range("A" & Rows.Count).End(xlUp).Row + 1
    Else
        nxtRow = trgt.Range("A" & Rows.Count).End(xlUp).Row
    End If
    
    ' Paste the data
    trgt.Rows(nxtRow & ":" & nxtRow).PasteSpecial _
        Paste:=xlPasteAll, Operation:=xlNone, SkipBlanks:=False, Transpose:=False
End Sub