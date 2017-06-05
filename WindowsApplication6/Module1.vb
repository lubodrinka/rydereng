
Module Module1
    Public APP As Object
    Public worksheet As Object
    Public workbook As Object
    Public excelextension As String
    Public code(111) As String
    Public time3 As Integer = 100
    Dim num As Integer = Keys.OemBackslash
    Public cisla() As Integer = {96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 111}
    Public Sub ini2()

        'code(16) = "Shift"
        'code(65) = "A"
        'code(66) = "B"
        'code(67) = "C"
        'code(68) = "D"
        'code(69) = "E"
        'code(70) = "F"
        'code(71) = "G"
        'code(72) = "H"
        'code(73) = "I"
        'code(74) = "J"
        'code(75) = "K"
        'code(76) = "L"
        'code(77) = "M"
        'code(78) = "N"
        'code(79) = "O"
        'code(80) = "P"
        'code(81) = "Q"
        'code(82) = "R"
        'code(83) = "S"
        'code(84) = "T"
        'code(85) = "U"
        'code(86) = "V"
        'code(87) = "W"
        'code(88) = "X"
        'code(89) = "Y"
        'code(90) = "Z"

        code(96) = 0
        code(97) = 1
        code(98) = 2
        code(99) = 3
        code(100) = 4
        code(101) = 5
        code(102) = 6
        code(103) = 7
        code(104) = 8
        code(105) = 9
        'code(111) = "/"
        'code(188) = ","
        'code(189) = "-"
        'code(189) = "."
        'code(222) = "§"
    End Sub

    Public Sub bothscanners()
        If ryder.WindowState = FormWindowState.Minimized Then
            If ryder.Timer3.Enabled = False Then
                directscannerinitalization()
            End If

        Else
            If ryder.Timer1.Enabled = False Then
                textboxdirectscanerinitalization()
            End If
        End If
    End Sub
    Public Sub directscannerinitalization()
        ini2()
        ryder.Timer3.Enabled = True
        ryder.Timer3.Start()
        ryder.Timer1.Stop()
        ryder.Timer1.Enabled = False
    End Sub
    Public Sub textboxdirectscanerinitalization()
        ryder.Timer1.Enabled = True
        ryder.Timer1.Start()
    End Sub
    'Excel.XlDirection.xlDownn-4121
    Public Const XlDirectionxlDownn = -4121
    'Excel.XlDirection.xlUP-4162
    Public Const XlDirectionxlUP = -4162
    'Excel.XlDirection.xlToLeft -4159
    Public Const XlDirectionxltoLeft = -4159
    'Excel.XlDirection.xlToRight -4161
    Public Const XlDirectionxlRight = -4161
    'Excel.Range
    ' Excel.XlColorIndex.xlColorIndexAutomatic,-4105
    Public Const xlcolorautomatic = -4105
    'Excel.XlBorderWeight.xlThin 2
    Public Const xlthin = 2
    Public Const xlthick = 2
    'Excel.XlVAlign.xlVAlignTop -4160
    Public Const XlVAlignxlVAlignTop = -4160
    Public Const XlVAlignxlVAlignJustify = -4130
    Public Const XlVAlignxlVAlignDistributed = -4117
    Public Const XlVAlignxlVAlignCenter = -4108
    Public Const XlVAlignxlVAlignBottom = -4107
    Public Const XlDeleteShiftDirectionxlShiftUp = -4162
    'Microsoft.Office.Core.MsoTriState.msoFalse 0
    Public Const MicrosoftOfficeCoreMsoTriStatemsoFalse = 0

    Public Const MicrosoftOfficeCoreMsoTriStatemsoCTrue = 1
    Public Sub releaseobject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        End Try
    End Sub
End Module
