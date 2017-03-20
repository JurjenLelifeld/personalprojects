Sub ImportCSVs()
'Author:    Jerry Beaucaire
'Date:      8/16/2010
'Summary:   Import all CSV files from a folder into separate sheets
'           named for the CSV filenames
'Update:    2/8/2013   Macro replaces existing sheets if they already exist in master workbook
'Source:    https://sites.google.com/a/madrocketscientist.com/jerrybeaucaires-excelassistant/merge-functions/csvs-to-sheets

Dim fPath   As String
Dim fCSV    As String
Dim wbCSV   As Workbook
Dim wbMST   As Workbook

Set wbMST = ThisWorkbook
'fPath = "c:\Users\Jurjen\OneDrive\Documents\RUG\MSc SIM\Master thesis\Data\XX_XXX\SDC Raw Data\"                  'path to CSV files, include the final \
fPath = Application.ActiveWorkbook.Path & "\"
Application.ScreenUpdating = False  'speed up macro
Application.DisplayAlerts = False   'no error messages, take default answers
fCSV = Dir(fPath & "*.csv")         'start the CSV file listing

    On Error Resume Next
    Do While Len(fCSV) > 0
        Set wbCSV = Workbooks.Open(fPath & fCSV)                    'open a CSV file
        wbMST.Sheets(ActiveSheet.Name).Delete                       'delete sheet if it exists
        ActiveSheet.Move After:=wbMST.Sheets(wbMST.Sheets.Count)    'move new sheet into Mstr
        Columns.AutoFit             'clean up display
        fCSV = Dir                  'ready next CSV
    Loop
    
Call DeleteSheet1
 
Application.ScreenUpdating = True
Set wbCSV = Nothing
Application.ActiveWorkbook.Close(True)
End Sub

' Handles the deletion of the temporary sheets when everything is handled for the company to save space and memory
Private Sub DeleteSheet1()
    Dim trgt As Worksheet
    
    ' Turn of alerts to prevent confirmation asking
    Application.DisplayAlerts = False
    
    ' Delete the empty sheet
    Set trgt = Sheets(1)
    trgt.Delete
    
    ' Turn the alerts back on
    Application.DisplayAlerts = True

End Sub