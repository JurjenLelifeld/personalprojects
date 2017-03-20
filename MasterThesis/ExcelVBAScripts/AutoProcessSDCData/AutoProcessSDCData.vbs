' Main method of the program
Public Sub SDCWorkbench()
    ' Initializing variables
    Application.ScreenUpdating = False
    Dim year As String
    Dim columnToSearchIn As String
    Dim companyNamesArray() As Variant
    Dim fileNamesArray() As Variant
    Dim i As Long
    Dim x As Long
    Dim sheetName As String
    
    i = 0
    x = 0
    ' Get current year
    year = GetCurrentYear()
    ' Get column to search in
    columnToSearchIn = GetColumnToSearchIn()
    ' Get array of company names from the temp sheet
    companyNamesArray = CreateCompanyArray("A")
    ' Get array of file names from the temp sheet
    fileNamesArray = CreateCompanyArray("B")
    
    ' Loop through all companies to find and process the data
    For Each company In companyNamesArray
    
        ' Retrieve the current sheetname from the array
        For Each tmpName In fileNamesArray
            If x = i Then
                sheetName = tmpName
                Exit For
            End If
            x = x + 1
        Next tmpName
        ' First create new sheets to work in
        Call CreateNewSheets(company, year, sheetName)
        ' Now find the company data from SDC and copy it to the RES sheet
        Call FindInRowAndCopyToOtherSheet(company, columnToSearchIn)
        ' Now split the data on the RES sheet
        Call SplitCompanyData
        ' Now copy the UNRES data from the RES to the UNRES sheet
        Call CheckAndCopyToUnres
        ' Save both sheets as CSV files
        Call SaveSheets
        ' Delete the temp company sheets
        Call DeleteTempSheets
        
        i = i + 1
        x = 0
    Next company
    
    MsgBox ("Done!")
    
    Application.ScreenUpdating = True
    Application.CutCopyMode = False
End Sub

' Create array from company name data in the temp worksheet
Private Function CreateCompanyArray(columnLetter) As Variant
    ' Initializing variables
    Dim companyNamesArray() As Variant
    Dim tempSheet As Worksheet
    Dim lastRow As Integer
     
    ' Select the right workbook and count the number of rows to select the range
    Set tempSheet = ActiveWorkbook.Sheets("Temp")
    lastRow = tempSheet.Cells(Rows.Count, "A").End(xlUp).row
    
    ' Create the array of string from the given range
    If lastRow = 2 Then ' If only one company is given, normale range usage would give an error
        Dim stringForOneCompany As String
        stringForOneCompany = tempSheet.Range(columnLetter & 2).Text
        companyNamesArray = Array(stringForOneCompany)
    Else
        companyNamesArray = tempSheet.Range(columnLetter & 2 & ":" & columnLetter & lastRow)
    End If
    
    CreateCompanyArray = companyNamesArray
End Function


' Sub handles the creation of temp company worksheets and the naming through user input
Private Sub CreateNewSheets(company, year, sheetName)
    ' Initializing variables
    Dim ws1 As Worksheet
    Dim ws2 As Worksheet
    Dim src As Worksheet
    'Dim sheetName As Variant
    
    ' Optional: Create an inputbox to ask for the name of the sheet based on the company
    'sheetName = Application.InputBox("Enter name for file for company: " & company)
    
    ' Create the RES workbook with the given name and year
    With ThisWorkbook
        Set ws1 = .Sheets.Add(After:=.Sheets(.Sheets.Count))
        ws1.Name = year & "_" & sheetName & "_" & "RES"
    End With
    
    ' Create the UNRES workbook with the given name and year
    With ThisWorkbook
        Set ws2 = .Sheets.Add(After:=.Sheets(.Sheets.Count))
        ws2.Name = year & "_" & sheetName & "_" & "UNRES"
    End With
    
    ' Add the header row from the temp worksheet to both sheets
    Set src = Sheets("Temp")
    Call CopyPaste(src, 1, ws1, False)
    Call CopyPaste(src, 1, ws2, False)
    
End Sub

' Sub finds the company data in the original SDC data and copies it to the temp sheet
Private Sub FindInRowAndCopyToOtherSheet(ByVal company As String, columnToSearchIn)
    ' Initializing variables
    Dim src As Worksheet
    Dim trgt As Worksheet
    Dim i As Long
    
    ' Setting source and target sheets
    Set src = Sheets(1)
    Set trgt = Sheets(4)
    
    ' Loop through all rows in the source
    For i = 1 To src.Range(columnToSearchIn & Rows.Count).End(xlUp).row
        ' If the cell equals the company name
        If InStr(src.Range(columnToSearchIn & i), company) Then
            ' Match has been found, copy this row
            Call CopyPaste(src, i, trgt, True)
        End If
    Next i
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
        nxtRow = trgt.Range("A" & Rows.Count).End(xlUp).row + 1
    Else
        nxtRow = trgt.Range("A" & Rows.Count).End(xlUp).row
    End If
    
    ' Paste the data
    trgt.Rows(nxtRow & ":" & nxtRow).PasteSpecial _
        Paste:=xlPasteAll, Operation:=xlNone, SkipBlanks:=False, Transpose:=False
End Sub

' Handles the splitting of the data after results were copied
Private Sub SplitCompanyData()
    ' Initializing variables
    Dim trgt As Worksheet
    
    ' Setting target sheets
    Set trgt = Sheets(4)
    
    ' First split cells and make two rows
    Call SplitCells(trgt)
    
    ' Then cross copy some cells (switch participant and focal company data)
    ' TODO!!!
    
    ' Then fill in the 1 and 0's at the end indicating if a cell is empty
    Call FillEmptyIndicators(trgt)
    
End Sub

' Sub splits the cells based on the alt+enter value
Private Sub SplitCells(trgt As Worksheet)
    ' Initializing variables
    Dim lastRow As Long
    Dim row As Long
    Dim numberOfColumns As Long
    Dim splittedValues As Variant
    Dim arrayLength As Long
    Dim i As Integer
    Dim columnLetterToCheckSplit As String
    Dim columnsToSkip As Variant
    Dim replacedString As String
    Dim columnShouldBeSkipped As Boolean
    Dim idString As String
    Dim idStringToPaste As String
    Dim tmpArrayLength As Long
    
    ' Make worksheet active
    trgt.Activate
    columnLetterToCheckSplit = "N"
    columnsToSkip = Array(8, 11, 18, 21, 22, 85, 93)
    columnShouldBeSkipped = False
             
    ' Determine ranges
    lastRow = Range("A" & Rows.Count).End(xlUp).row
    numberOfColumns = Cells(1, Columns.Count).End(xlToLeft).Column
    
    ' Loop through data from down to up, skip header
    For row = lastRow To 2 Step -1
        ' Check the value
        splittedValues = Split(Cells(row, columnLetterToCheckSplit), Chr(10))
        arrayLength = UBound(splittedValues, 1) - LBound(splittedValues, 1) + 1
    
        ' Add empty rows above the current row, based on the number of splits needed
        Range("A" & row).EntireRow.Resize(arrayLength - 1).Insert Shift:=xlDown
        
        ' Go through all columns in the current row
        For Column = 1 To numberOfColumns
            ' Some cells should not be split, but the alt+enter has to be changed to a :
            For Each x In columnsToSkip
                If x = Column Then
                    columnShouldBeSkipped = True
                End If
            Next x
            ' If cells have to be changed to a then:
            If columnShouldBeSkipped Then
                replacedString = Replace(Cells(row + arrayLength - 1, Column), Chr(10), ":")
                i = 0
                Do While i < arrayLength
                    Cells(row + i, Column) = replacedString
                    i = i + 1
                Loop
                columnShouldBeSkipped = False
            ' Create unique ID values
            ElseIf Column = 95 Then
                idString = Cells(row + arrayLength - 1, 86).Value
                i = 0
                Do While i < arrayLength
                    idStringToPaste = idString & "," & (i + 1)
                    Cells(row + i, Column) = idStringToPaste
                    i = i + 1
                Loop
            ' If the data has to be split, split it with an array
            ElseIf InStr(1, Cells(row + arrayLength - 1, Column), Chr(10)) <> 0 Then
                tmpArr = Split(Cells(row + arrayLength - 1, Column), Chr(10))
                tmpArrayLength = UBound(tmpArr, 1) - LBound(tmpArr, 1) + 1
                ' If both arrays do not have the same length, data is missing.
                ' Make both cells blank in that case
                If tmpArrayLength = arrayLength Then
                    i = 0
                    ' Loop through the array and past each value in a new row
                    Do While i < arrayLength
                        Cells(row + i, Column) = tmpArr(i)
                        i = i + 1
                    Loop
                Else
                    i = 0
                    ' Loop through the array and past each value in a new row
                    Do While i < arrayLength
                        Cells(row + i, Column) = ""
                        i = i + 1
                    Loop
                End If
            ' Just copy the complete data to the rows. Loop through all empty rows to paste.
            Else
                i = arrayLength - 1
                Do While i > 0
                    Cells(row + i, Column).Copy Cells(row + i - 1, Column)
                    i = i - 1
                Loop
            End If
        Next Column
    Next row
End Sub

' Fill the cells at the end that indicate whether certain cells are empty or not
Private Sub FillEmptyIndicators(trgt As Worksheet)
    ' Initializing variables
    Dim lastRow As Long
    Dim numberOfColumns As Long
    Dim checkEmpty As Boolean
    Dim cellsToCheck As Variant
    Dim i As Integer
    
    ' Create data to check
    trgt.Activate
    cellsToCheck = Array(6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19, 20, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 65, 67)
    lastRow = Range("A" & Rows.Count).End(xlUp).row
    numberOfColumns = Cells(1, Columns.Count).End(xlToLeft).Column
    checkEmpty = False
    
    ' Go through all rows
    For row = lastRow To 2 Step -1
        i = 0
        ' Go through all columns to fill
        For Column = 96 To numberOfColumns
            ' Check if the corresponding cell is empty
            checkEmpty = IsEmpty(Cells(row, cellsToCheck(i)).Value)
            ' If it is empty, then fill in 0, otherwise 1
            If checkEmpty Then
                Cells(row, Column) = 0
            Else
                Cells(row, Column) = 1
            End If
            i = i + 1
        Next Column
    Next row
End Sub

' Checks if a row should be moved to the UNRES sheet
Private Sub CheckAndCopyToUnres()
    ' Initializing variables
    Dim src As Worksheet
    Dim trgt As Worksheet
    Dim rowsToDelete As New Collection
    Dim columnsThatCanBeEmpty As Variant
    Dim checkOtherDeals As Integer
    Dim skipRow As Boolean
    Dim skipColumn As Boolean
    Dim numberOfColumns As Integer
    Dim vTemp As Variant
    Dim i As Long, j As Long
    
    ' Setting source and target sheets
    Set src = Sheets(4)
    Set trgt = Sheets(5)
    skipRow = False
    skipColumn = False
    numberOfColumns = src.Cells(1, src.Columns.Count).End(xlToLeft).Column
    
    ' Populating array with columns to check
    columnsThatCanBeEmpty = Array(3, 4, 16, 34, 35, 36, 37, 43, 45, 46, 50, 52, 67, 68, 69, 70, 71, 73, 76, 77, 81)
    
    ' Loop through all rows in the source
    For i = 1 To src.Range("A" & Rows.Count).End(xlUp).row
        
        ' Skip rows that will be deleted, this prevents doulbe copying
        For Each rowToDelete In rowsToDelete
            If rowToDelete = i Then
                skipRow = True
            End If
        Next rowToDelete
        
        ' Loop through column array to check
        If Not skipRow Then
            For Column = 1 To numberOfColumns
                
                ' Skip columns that can be empty
                For Each columnThatCanBeEmpty In columnsThatCanBeEmpty
                    If columnThatCanBeEmpty = Column Then
                        skipColumn = True
                    End If
                Next columnThatCanBeEmpty
            
                If Not skipColumn Then
                    ' If the cell equals the value 0, copy it to unres
                    If src.Cells(i, Column) = 0 Or IsEmpty(src.Cells(i, Column)) Then
                        checkOtherDeals = src.Cells(i, Column).Value
                        ' Copy this row to unres, keep a record to delete the row later and break
                        Call CopyPaste(src, i, trgt, True)
                        rowsToDelete.Add (i)
                        
                        ' Also check for the same deals in other rows, these rows also have to be copied
                        For x = 1 To src.Range("A" & Rows.Count).End(xlUp).row
                            If src.Cells(x, 86) = checkOtherDeals Then
                                Call CopyPaste(src, i, trgt, True)
                                rowsToDelete.Add (i)
                            End If
                        Next x
                        
                        Exit For
                    End If
                End If
            
            Next Column
        End If
        
        skipColumn = False
        skipRow = False
    Next i
    
    ' Sort the collection ascending so that rows will be deleted in the right order
    For i = 1 To rowsToDelete.Count - 1
        For j = i + 1 To rowsToDelete.Count
            If rowsToDelete(i) > rowsToDelete(j) Then
                'store the lesser item
               vTemp = rowsToDelete(j)
                'remove the lesser item
               rowsToDelete.Remove j
                're-add the lesser item before the
               'greater Item
               rowsToDelete.Add vTemp, vTemp, i
            End If
        Next j
    Next i
    
    ' Now delete the copied rows from the RES sheet from below to start
    For rowToDelete = rowsToDelete.Count To 1 Step -1
        src.Rows(rowsToDelete.Item(rowToDelete)).EntireRow.Delete
    Next rowToDelete
End Sub

' Handles the saving as CSV files of the sheet
Private Sub SaveSheets()
    Dim src As Worksheet
    
    ' Save res sheet in current folder with sheetname
    Set src = Sheets(4)
    src.SaveAs Application.ActiveWorkbook.Path & "\" & src.Name & ".csv", xlCSV
    
    ' Save unres sheet in current folder with sheetname
    Set src = Sheets(5)
    src.SaveAs Application.ActiveWorkbook.Path & "\" & src.Name & ".csv", xlCSV

End Sub

' Handles the deletion of the temporary sheets when everything is handled for the company to save space and memory
Private Sub DeleteTempSheets()
    Dim trgt As Worksheet
    
    ' Turn of alerts to prevent confirmation asking
    Application.DisplayAlerts = False
    
    ' Delete the unres sheet
    Set trgt = Sheets(5)
    trgt.Delete
    
    ' Delete the res sheet
    Set trgt = Sheets(4)
    trgt.Delete
    
    ' Turn the alerts back on
    Application.DisplayAlerts = True

End Sub

' Ask the user for the current year for naming purposes
Private Function GetCurrentYear() As String
    ' Initializing variables
    Dim year As String
    
    year = Left(ActiveWorkbook.Name, (InStrRev(ActiveWorkbook.Name, ".", -1, vbTextCompare) - 1))
    
    ' Optional: Create an inputbox to ask for the current year
    ' year = Application.InputBox("Enter current year:")
    
    GetCurrentYear = year
End Function

' Ask the user for the column to search in
Private Function GetColumnToSearchIn() As String
    ' Initializing variables
    Dim columnLetter As String
    
    ' Optional: Create an inputbox to ask for the column letter to use
    ' columnLetter = Application.InputBox("Enter column letter to search in:")
    
    columnLetter = "N"
    
    GetColumnToSearchIn = columnLetter
End Function