Sub CountAlliances()
    Dim src As Worksheet
    Dim tmpId As LongLong
    Dim lastIdChecked As LongLong
    Dim numberOfOriginal As Integer
    Dim numberOfAcquired As Integer
    Dim total As Integer
    
    numberOfOriginal = 0
    numberOfAcquired = 0
    
    ' Loop through all alliance sheets
    For s = 2 To 11
        Set src = Sheets(s)
        lastIdChecked = -1
                
        ' On each sheet, go through all rows
        For i = 2 To src.Range("A" & Rows.Count).End(xlUp).Row
            tmpId = CLngLng(src.Cells(i, 28).Value)
                    
            ' If unique deal number is checked before, skip since alliance is already counted
            If Not lastIdChecked = tmpId Then
                lastIdChecked = tmpId ' Set lastIdChecked to make sure it isn't counted again
                
                ' If original alliance and alliance is not yet ended, increase number
                If CInt(src.Cells(i, 1).Value) = 0 Then
                    numberOfOriginal = numberOfOriginal + 1
                ' If acquired alliance and alliance is not yet ended, increase number
                ElseIf CInt(src.Cells(i, 1).Value) = 1 Then
                    numberOfAcquired = numberOfAcquired + 1
                End If
            End If
        Next i
    Next s
    
    total = numberOfOriginal + numberOfAcquired
    
    MsgBox ("Original alliances: " & CStr(numberOfOriginal))
    MsgBox ("Acquired alliances: " & CStr(numberOfAcquired))
    MsgBox ("Total alliances: " & CStr(total))
End Sub