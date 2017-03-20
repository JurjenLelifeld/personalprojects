Sub CountValues()
    Dim src As Worksheet
    Dim trgt As Worksheet

    Dim numberOfOriginal As Integer
    Dim numberOfAcquired As Integer
    Dim numberOfIntegrated As Integer
    Dim numberOfPreserved As Integer
    
    Dim biv As Double
    Dim iqv As Double

    Dim sic4 As Integer
    Dim sic3 As Integer
    Dim sic2 As Integer
    Dim sic1 As Integer
    Dim sic0 As Integer

    Dim sic4p2 As Double
    Dim sic3p2 As Double
    Dim sic2p2 As Double
    Dim sic1p2 As Double
    Dim sic0p2 As Double

    Dim lastIdChecked As LongLong
    Dim tmpId As LongLong

    Dim acquisitionYear As Integer
    Dim endDate As Integer
    Dim focalYear As Integer
    
    Call RecalculateSicValues
    
    ' Initialize variables
    Set trgt = Sheets(1)
    focalYear = 2000
    
    ' Loop through all focal years (2000-2005)
    For x = 1 To 6
        ' Reset all numbers for new year
        numberOfOriginal = 0
        numberOfAcquired = 0
        numberOfIntegrated = 0
        numberOfPreserved = 0
        lastIdChecked = -1

        sic4 = 0
        sic3 = 0
        sic2 = 0
        sic1 = 0
        sic0 = 0
    
        sic4p2 = 0
        sic3p2 = 0
        sic2p2 = 0
        sic1p2 = 0
        sic0p2 = 0
    
        biv = 0
        iqv = 0
       
        ' Loop through all previous years (sheets) before current to check alliances
        For s = 2 To ((focalYear - 1995) + 1)
            Set src = Sheets(s)
            
            ' On each sheet, go through all rows
            For i = 2 To src.Range("A" & Rows.Count).End(xlUp).Row
                    tmpId = CLngLng(src.Cells(i, 28).Value)
                    endDate = CInt(src.Cells(i, 4).Value)
                    acquisitionYear = CInt(src.Cells(i, 3).Value)
                    
                    ' If unique deal number is checked before, skip since alliance is already counted
                    If Not lastIdChecked = tmpId Then
                        lastIdChecked = tmpId ' Set lastIdChecked to make sure it isn't counted again
                        
                        ' If original alliance and alliance is not yet ended, increase number
                        If CInt(src.Cells(i, 1).Value) = 0 And focalYear <= endDate Then
                                numberOfOriginal = numberOfOriginal + 1
                        ' If acquired and alliance is not yet ended and the firm has also been acquired in the focal  year or later then increase number
                        ElseIf CInt(src.Cells(i, 1).Value) = 1 And focalYear <= endDate And focalYear >= acquisitionYear Then
                            numberOfAcquired = numberOfAcquired + 1
                            
                            ' Check preserved or integrated
                            If CInt(src.Cells(i, 2).Value) = 1 Then
                                numberOfPreserved = numberOfPreserved + 1
                            Else
                                numberOfIntegrated = numberOfIntegrated + 1
                            End If
                        End If
                    End If

                ' Calculate diversity occurences of portfolio. Do not count own company (-1)
                If CInt(src.Cells(i, 1).Value) = 0 And focalYear <= endDate Then
                    If CInt(src.Cells(i, 5).Value) = 4 Then
                        sic4 = sic4 + 1
                    ElseIf CInt(src.Cells(i, 5).Value) = 3 Then
                        sic3 = sic3 + 1
                    ElseIf CInt(src.Cells(i, 5).Value) = 2 Then
                        sic2 = sic2 + 1
                    ElseIf CInt(src.Cells(i, 5).Value) = 1 Then
                        sic1 = sic1 + 1
                    ElseIf CInt(src.Cells(i, 5).Value) = 0 Then
                        sic0 = sic0 + 1
                    End If
                ElseIf CInt(src.Cells(i, 1).Value) = 1 And focalYear <= endDate And focalYear >= acquisitionYear Then
                    If CInt(src.Cells(i, 5).Value) = 4 Then
                        sic4 = sic4 + 1
                    ElseIf CInt(src.Cells(i, 5).Value) = 3 Then
                        sic3 = sic3 + 1
                    ElseIf CInt(src.Cells(i, 5).Value) = 2 Then
                        sic2 = sic2 + 1
                    ElseIf CInt(src.Cells(i, 5).Value) = 1 Then
                        sic1 = sic1 + 1
                    ElseIf CInt(src.Cells(i, 5).Value) = 0 Then
                        sic0 = sic0 + 1
                    End If
                End If
            Next i
        Next s
    
        If sic4 = 0 And sic3 = 0 And sic2 = 0 And sic1 = 0 And sic0 = 0 Then
            iqv = -1
        Else
            'Calculate diversity scores
            sic4p2 = (sic4 / (sic4 + sic3 + sic2 + sic1 + sic0)) ^ 2
            sic3p2 = (sic3 / (sic4 + sic3 + sic2 + sic1 + sic0)) ^ 2
            sic2p2 = (sic2 / (sic4 + sic3 + sic2 + sic1 + sic0)) ^ 2
            sic1p2 = (sic1 / (sic4 + sic3 + sic2 + sic1 + sic0)) ^ 2
            sic0p2 = (sic0 / (sic4 + sic3 + sic2 + sic1 + sic0)) ^ 2
    
            biv = 1 - (sic4p2 + sic3p2 + sic2p2 + sic1p2 + sic0p2)
            iqv = biv * 1.25
        End If
       
        ' Fill in the final numbers before moving to next focal year
        trgt.Cells(x + 1, 3).Value = numberOfOriginal
        trgt.Cells(x + 1, 4).Value = numberOfAcquired
        trgt.Cells(x + 1, 5).Value = numberOfPreserved
        trgt.Cells(x + 1, 6).Value = numberOfIntegrated
        trgt.Cells(x + 1, 7).Value = iqv
    
        ' Go to next focal year
        focalYear = focalYear + 1
    Next x
    
End Sub

Private Sub RecalculateSicValues()
    Dim src As Worksheet
    Dim sic As Integer
    Dim tmp As Integer
    
    sic = Application.InputBox("SIC code of focal firm:")
    
    For s = 2 To 11
        Set src = Sheets(s)
        
        For i = 2 To src.Range("A" & Rows.Count).End(xlUp).Row
            
            If Not src.Cells(i, 5).Value = -1 Then
            
                If TypeName(src.Cells(i, 8).Value) = "Double" Then
                    tmp = CInt(src.Cells(i, 8).Value)
                Else
                    tmp = -1
                End If
                
                If tmp = sic Then
                    src.Cells(i, 5).Value = 4
                ElseIf Int(tmp / 10) = Int(sic / 10) Then
                    src.Cells(i, 5).Value = 3
                ElseIf Int(tmp / 100) = Int(sic / 100) Then
                    src.Cells(i, 5).Value = 2
                ElseIf Int(tmp / 1000) = Int(sic / 1000) Then
                    src.Cells(i, 5).Value = 1
                Else
                    src.Cells(i, 5).Value = 0
                End If
            End If
        Next i
    Next s
End Sub