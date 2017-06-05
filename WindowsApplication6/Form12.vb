
Public Class Form12
    Public Sub appexcstart()

        Try
            APP = CreateObject("Excel.Application")
            If APP.VERSION.replace(".", ",") > 11.9 Then
                excelextension = "xlsx"
                SaveFileDialog1.Filter = "excel files (*.xlsx)|.xlsx|(*.xls)|.xls|All files (*.*)|*.*"
            Else
                SaveFileDialog1.Filter = "excel files (*.xls)|.xls|All files (*.*)|*.*"
                excelextension = "xls"
            End If
            SaveFileDialog1.DefaultExt = excelextension
            releaseobject(APP)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Form12_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        appexcstart()
        Dim cb() As String
        Dim newDate As Date = Now
        Dim newdiff As Integer = 365

        Dim differenceInDays As Integer = 0
        cb = Split(ryder.CBpermision.ToString, vbCrLf)
        ListView1.Items.Clear() : ListView1.Visible = True : ListView1.BringToFront()
        Dim totalplanhours As Decimal = 0
        Dim totalplancost As Decimal = 0
        Dim totalrealhours As Decimal = 0
        Dim totalrealcost As Decimal = 0
        Dim totalworktime As Double = 0
        Dim totalworkprice As Decimal = 0
        Dim totaldiffhsum As Double = 0
        Dim totaldiffcsum As Decimal = 0
        Me.Location = New Point(((My.Computer.Screen.WorkingArea.Width / 2) - (Me.Width / 2)), 40)
        Dim pr As Integer = ListView1.Items.Count - 1
        Dim kl As String = Nothing
        Try

            For x As Long = 0 To UBound(cb)
                If cb(x) <> Nothing And cb(x) <> String.Empty Then
                    ListView1.Items.Add(cb(x)).UseItemStyleForSubItems = True
                    ListView1.Items(ListView1.Items.Count - 1).BackColor = Color.LightSlateGray
                    Dim denna1 As String = Application.StartupPath & "\devices\" + cb(x) + "\daily.txt"
                    Dim tyzdenna1 As String = Application.StartupPath & "\devices\" + cb(x) + "\weekly.txt"
                    Dim mesa1 As String = Application.StartupPath & "\devices\" + cb(x) + "\monthly.txt"
                    Dim rocna1 As String = Application.StartupPath & "\devices\" + cb(x) + "\yearly.txt"
                    Dim fileexist As Boolean
                    If fileexist = My.Computer.FileSystem.FileExists(denna1) Then
                        My.Computer.FileSystem.WriteAllText(denna1, String.Empty, False)
                    End If
                    If fileexist = My.Computer.FileSystem.FileExists(tyzdenna1) Then
                        My.Computer.FileSystem.WriteAllText(tyzdenna1, String.Empty, False)
                    End If

                    If fileexist = My.Computer.FileSystem.FileExists(mesa1) Then
                        My.Computer.FileSystem.WriteAllText(mesa1, String.Empty, False)
                    End If

                    If fileexist = My.Computer.FileSystem.FileExists(rocna1) Then
                        My.Computer.FileSystem.WriteAllText(rocna1, String.Empty, False)
                    End If

                    Dim freqmain As String = Nothing
                    Dim settfreq As String = Nothing

                    Dim interval As Integer = 0
                    Dim planhours As Decimal = 0
                    Dim plancost As Decimal = 0
                    Dim realhours As Decimal = 0
                    Dim realcost As Decimal = 0
                    Dim findcount As Integer = 0
                    Dim worktime As Double = 0
                    Dim workprice As Decimal = 0
                    Dim diffhsum As Double = 0
                    Dim diffcsum As Decimal = 0
                    Try

                        Dim sNames1() As String = Split((My.Computer.FileSystem.ReadAllText(denna1)), vbCrLf)
                        For xx = 0 To UBound(sNames1)
                            worktime = 0
                            workprice = 0
                            interval = 1 : Dim percenta As Integer = 0

                            If sNames1(xx) <> String.Empty Then

                                Dim devrep() As String = Split(My.Settings.devreptimed, vbCrLf)
                                For Each saved As String In devrep

                                    Dim saveddev() As String = Split(saved, vbTab)
                                    Try
                                        If sNames1(xx) = saveddev(0) Then
                                            worktime = saveddev(1)
                                            workprice = saveddev(2)
                                        End If
                                    Catch ex As Exception

                                    End Try

                                Next

                                Dim farba As System.Drawing.Color = Color.Gray
                                Dim olddate As Date = Nothing
                                For Each line In IO.File.ReadLines(Application.StartupPath & "\devices\" + cb(x) + "\" + cb(x))
                                    If line <> String.Empty Then
                                        Dim Dat() As String = Split(line, "|")

                                        If line.Contains(sNames1(xx)) Then

                                            olddate = Dat(1)
                                            findcount += +1
                                            differenceInDays = DateDiff(DateInterval.Day, olddate, newDate)
                                            If differenceInDays >= interval Then percenta = (100 / (differenceInDays + 1 / interval))
                                            If differenceInDays = 0 Then percenta = 100

                                            If percenta >= 99.4 Then farba = Color.Green
                                            If percenta >= 50 And percenta < 99.4 Then farba = Color.LightSteelBlue
                                            If percenta >= 0.5 And percenta < 50 Then farba = Color.IndianRed
                                            If percenta < 0.5 Then farba = Color.Red
                                        End If
                                    End If
                                Next

                                ListView1.Items.Add(sNames1(xx)).UseItemStyleForSubItems = False
                                pr = ListView1.Items.Count - 1
                                ListView1.Items(pr).SubItems.Add(ryder.CheckBox1.Text)
                                ListView1.Items(pr).SubItems.Add(percenta & " %").ForeColor = farba
                                ListView1.Items(pr).SubItems.Add(olddate)
                                Dim workhourspcount As Double = 0
                                Dim worktimepcost As Decimal = 0
                                Dim workhoursrcount As Double = 0
                                Dim worktimercost As Decimal = 0
                                Dim diffh As Double = 0
                                Dim diffc As Decimal = 0
                                'planhours
                                If Date.IsLeapYear(Date.Now.Year) = True Then
                                    workhourspcount = (366 * worktime)
                                Else
                                    workhourspcount = (365 * worktime)
                                End If
                                ListView1.Items(pr).SubItems.Add(workhourspcount)
                                planhours += workhourspcount
                                'realhours
                                workhoursrcount = findcount * worktime
                                ListView1.Items(pr).SubItems.Add(workhoursrcount)
                                realhours += workhoursrcount
                                'diffhours
                                diffh = workhoursrcount - workhourspcount
                                ListView1.Items(pr).SubItems.Add(Format(diffh, "0.00"))
                                diffhsum += diffh
                                'plancost 
                                worktimepcost = workhourspcount * workprice
                                ListView1.Items(pr).SubItems.Add(Format(worktimepcost, "0.000"))
                                plancost += worktimepcost
                                'realcost 
                                worktimercost = workhoursrcount * workprice
                                ListView1.Items(pr).SubItems.Add(Format(worktimercost, "0.000"))
                                realcost += worktimercost
                                'diffcost
                                diffc = worktimercost - worktimepcost
                                ListView1.Items(pr).SubItems.Add(Format(diffc, "0.000"))
                                diffcsum += diffc
                                'workhours
                                ListView1.Items(pr).SubItems.Add(Format(worktime, "0.00"))
                                'workhoursprice
                                ListView1.Items(pr).SubItems.Add(Format(workprice, "0.000"))


                            End If

                        Next

                    Catch ex As Exception

                    End Try
                    Try


                        Dim sNames2() As String = Split((My.Computer.FileSystem.ReadAllText(tyzdenna1)), vbCrLf)
                        For xx = 0 To UBound(sNames2)
                            worktime = 0
                            workprice = 0
                            interval = 7 : Dim percenta As Integer = 0
                            If sNames2(xx) <> String.Empty Then
                                Dim devrep() As String = Split(My.Settings.devreptimew, vbCrLf)
                                For Each saved As String In devrep

                                    Dim saveddev() As String = Split(saved, vbTab)
                                    Try
                                        If sNames2(xx) = saveddev(0) Then
                                            worktime = saveddev(1)
                                            workprice = saveddev(2)
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Next
                                Dim farba As System.Drawing.Color = Color.Gray
                                Dim olddate As Date = Nothing
                                For Each line In IO.File.ReadLines(Application.StartupPath & "\devices\" + cb(x) + "\" + cb(x))
                                    If line <> String.Empty Then

                                        Dim Dat() As String = Split(line, "|")

                                        If line.Contains(sNames2(xx)) Then
                                            olddate = Dat(1)
                                            findcount += +1
                                            differenceInDays = DateDiff(DateInterval.Day, olddate, newDate)
                                            If differenceInDays >= interval Then percenta = (100 / (differenceInDays / interval))
                                            If differenceInDays = 0 Then percenta = 100
                                            If percenta >= 99.4 Then farba = Color.Green
                                            If percenta >= 50 And percenta < 99.4 Then farba = Color.LightSteelBlue
                                            If percenta >= 0.5 And percenta < 50 Then farba = Color.IndianRed
                                            If percenta < 0.5 Then farba = Color.Red
                                        End If
                                    End If
                                Next
                                ListView1.Items.Add(sNames2(xx)).UseItemStyleForSubItems = False
                                pr = ListView1.Items.Count - 1
                                ListView1.Items(pr).SubItems.Add(ryder.CheckBox6.Text)
                                ListView1.Items(pr).SubItems.Add(percenta & " %").ForeColor = farba
                                ListView1.Items(pr).SubItems.Add(olddate)
                                Dim workhourspcount As Double = 0
                                Dim worktimepcost As Decimal = 0
                                Dim workhoursrcount As Double = 0
                                Dim worktimercost As Decimal = 0
                                Dim diffh As Double = 0
                                Dim diffc As Decimal = 0
                                'planhoursWEEK
                                If Date.IsLeapYear(Date.Now.Year) = True Then
                                    workhourspcount = (366 / 7 * worktime)
                                Else
                                    workhourspcount = (365 / 7 * worktime)
                                End If
                                ListView1.Items(pr).SubItems.Add(workhourspcount)
                                planhours += workhourspcount
                                'realhours
                                workhoursrcount = findcount * worktime
                                ListView1.Items(pr).SubItems.Add(workhoursrcount)
                                realhours += workhoursrcount
                                'diffhours
                                diffh = workhoursrcount - workhourspcount
                                ListView1.Items(pr).SubItems.Add(Format(diffh, "0.00"))
                                diffhsum += diffh
                                'plancost 
                                worktimepcost = workhourspcount * workprice
                                ListView1.Items(pr).SubItems.Add(Format(worktimepcost, "0.000"))
                                plancost += worktimepcost
                                'realcost 
                                worktimercost = workhoursrcount * workprice
                                ListView1.Items(pr).SubItems.Add(Format(worktimercost, "0.000"))
                                realcost += worktimercost
                                'diffcost
                                diffc = worktimercost - worktimepcost
                                ListView1.Items(pr).SubItems.Add(Format(diffc, "0.000"))
                                diffcsum += diffc
                                'workhours
                                ListView1.Items(pr).SubItems.Add(Format(worktime, "0.00"))
                                'workhoursprice
                                ListView1.Items(pr).SubItems.Add(Format(workprice, "0.000"))

                            End If
                        Next
                    Catch ex As Exception

                    End Try


                    Try


                        Dim sNames3() As String = Split((My.Computer.FileSystem.ReadAllText(mesa1)), vbCrLf)
                        For xx = 0 To UBound(sNames3)
                            worktime = 0
                            workprice = 0
                            Dim percenta As Integer = 0
                            interval = Date.DaysInMonth(Date.Now.Year, Date.Now.Month)
                            If sNames3(xx) <> String.Empty Then
                                Dim devrep() As String = Split(My.Settings.devreptimem, vbCrLf)
                                For Each saved As String In devrep

                                    Dim saveddev() As String = Split(saved, vbTab)
                                    Try
                                        If sNames3(xx) = saveddev(0) Then
                                            worktime = saveddev(1)
                                            workprice = saveddev(2)
                                        End If

                                    Catch ex As Exception

                                    End Try
                                Next

                                Dim farba As System.Drawing.Color = Color.Gray
                                Dim olddate As Date = Nothing
                                For Each line In IO.File.ReadLines(Application.StartupPath & "\devices\" + cb(x) + "\" + cb(x))
                                    If line <> String.Empty Then
                                        Dim Dat() As String = Split(line, "|")


                                        If line.Contains(sNames3(xx)) Then
                                            olddate = Dat(1)
                                            findcount += +1
                                            differenceInDays = DateDiff(DateInterval.Day, olddate, newDate)
                                            If differenceInDays >= interval Then percenta = (100 / (differenceInDays / interval))
                                            If differenceInDays = 0 Then percenta = 100
                                            If percenta >= 99.4 Then farba = Color.Green
                                            If percenta >= 50 And percenta < 99.4 Then farba = Color.LightSteelBlue
                                            If percenta >= 0.5 And percenta < 50 Then farba = Color.IndianRed
                                            If percenta < 0.5 Then farba = Color.Red
                                        End If
                                    End If
                                Next

                                ListView1.Items.Add(sNames3(xx)).UseItemStyleForSubItems = False
                                pr = ListView1.Items.Count - 1
                                ListView1.Items(pr).SubItems.Add(ryder.CheckBox2.Text)
                                ListView1.Items(pr).SubItems.Add(percenta & " %").ForeColor = farba
                                ListView1.Items(pr).SubItems.Add(olddate)
                                Dim workhourspcount As Double = 0
                                Dim worktimepcost As Decimal = 0
                                Dim workhoursrcount As Double = 0
                                Dim worktimercost As Decimal = 0
                                Dim diffh As Double = 0
                                Dim diffc As Decimal = 0
                                'planhoursMONTH

                                workhourspcount = 12 * worktime

                                ListView1.Items(pr).SubItems.Add(workhourspcount)
                                planhours += workhourspcount
                                'realhours
                                workhoursrcount = findcount * worktime
                                ListView1.Items(pr).SubItems.Add(workhoursrcount)
                                realhours += workhoursrcount
                                'diffhours
                                diffh = workhoursrcount - workhourspcount
                                ListView1.Items(pr).SubItems.Add(Format(diffh, "0.00"))
                                diffhsum += diffh
                                'plancost 
                                worktimepcost = workhourspcount * workprice
                                ListView1.Items(pr).SubItems.Add(Format(worktimepcost, "0.000"))
                                plancost += worktimepcost
                                'realcost 
                                worktimercost = workhoursrcount * workprice
                                ListView1.Items(pr).SubItems.Add(Format(worktimercost, "0.000"))
                                realcost += worktimercost
                                'diffcost
                                diffc = worktimercost - worktimepcost
                                ListView1.Items(pr).SubItems.Add(Format(diffc, "0.000"))
                                diffcsum += diffc
                                'workhours
                                ListView1.Items(pr).SubItems.Add(Format(worktime, "0.00"))
                                'workhoursprice
                                ListView1.Items(pr).SubItems.Add(Format(workprice, "0.000"))


                            End If
                        Next
                    Catch ex As Exception

                    End Try
                    Try


                        Dim sNames4() As String = Split((My.Computer.FileSystem.ReadAllText(rocna1)), vbCrLf)
                        For xx = 0 To UBound(sNames4)
                            worktime = 0
                            workprice = 0
                            interval = 365.25 : Dim percenta As Integer = 0
                            If sNames4(xx) <> String.Empty Then
                                Dim devrep() As String = Split(My.Settings.devreptimey, vbCrLf)
                                For Each saved As String In devrep

                                    Dim saveddev() As String = Split(saved, vbTab)
                                    Try
                                        If sNames4(xx) = saveddev(0) Then
                                            worktime = saveddev(1)
                                            workprice = saveddev(2)
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Next
                                Dim farba As System.Drawing.Color = Color.Gray
                                Dim olddate As Date = Nothing
                                For Each line In IO.File.ReadLines(Application.StartupPath & "\devices\" + cb(x) + "\" + cb(x))
                                    If line <> String.Empty Then
                                        Dim Dat() As String = Split(line, "|")

                                        If line.Contains(sNames4(xx)) Then
                                            olddate = Dat(1)
                                            findcount += +1
                                            differenceInDays = DateDiff(DateInterval.Day, olddate, newDate)
                                            If differenceInDays >= interval Then percenta = (100 / (differenceInDays / interval))
                                            If differenceInDays = 0 Then percenta = 100
                                            If percenta >= 99.4 Then farba = Color.Green
                                            If percenta >= 50 And percenta < 99.4 Then farba = Color.LightSteelBlue
                                            If percenta >= 0.5 And percenta < 50 Then farba = Color.IndianRed
                                            If percenta < 0.5 Then farba = Color.Red
                                        End If
                                    End If
                                Next
                                ListView1.Items.Add(sNames4(xx)).UseItemStyleForSubItems = False
                                pr = ListView1.Items.Count - 1
                                ListView1.Items(pr).SubItems.Add(ryder.CheckBox3.Text)
                                ListView1.Items(pr).SubItems.Add(percenta & " %").ForeColor = farba
                                ListView1.Items(pr).SubItems.Add(olddate)
                                Dim workhourspcount As Double = 0
                                Dim worktimepcost As Decimal = 0
                                Dim workhoursrcount As Double = 0
                                Dim worktimercost As Decimal = 0
                                Dim diffh As Double = 0
                                Dim diffc As Decimal = 0
                                'planhours YEAR
                                workhourspcount = 1 * worktime
                                ListView1.Items(pr).SubItems.Add(workhourspcount)
                                planhours += workhourspcount
                                'realhours
                                workhoursrcount = findcount * worktime
                                ListView1.Items(pr).SubItems.Add(workhoursrcount)
                                realhours += workhoursrcount
                                'diffhours
                                diffh = workhoursrcount - workhourspcount
                                ListView1.Items(pr).SubItems.Add(Format(diffh, "0.00"))
                                diffhsum += diffh
                                'plancost 
                                worktimepcost = workhourspcount * workprice
                                ListView1.Items(pr).SubItems.Add(Format(worktimepcost, "0.000"))
                                plancost += worktimepcost
                                'realcost 
                                worktimercost = workhoursrcount * workprice
                                ListView1.Items(pr).SubItems.Add(Format(worktimercost, "0.000"))
                                realcost += worktimercost
                                'diffcost
                                diffc = worktimercost - worktimepcost
                                ListView1.Items(pr).SubItems.Add(Format(diffc, "0.000"))
                                diffcsum += diffc
                                'workhours
                                ListView1.Items(pr).SubItems.Add(Format(worktime, "0.00"))
                                'workhoursprice
                                ListView1.Items(pr).SubItems.Add(Format(workprice, "0.000"))

                            End If
                        Next
                    Catch ex As Exception

                    End Try
                    ListView1.Items.Add("suma").UseItemStyleForSubItems = False

                    pr = ListView1.Items.Count - 1
                    ListView1.Items(pr).BackColor = Color.LightGoldenrodYellow
                    ListView1.Items(pr).SubItems.Add("-").BackColor = Color.LightGoldenrodYellow : ListView1.Items(pr).SubItems.Add("-").BackColor = Color.LightGoldenrodYellow : ListView1.Items(pr).SubItems.Add("-").BackColor = Color.LightGoldenrodYellow
                    ListView1.Items(pr).SubItems.Add(Format(planhours, "0.00")).BackColor = Color.LightGoldenrodYellow
                    ListView1.Items(pr).SubItems.Add(Format(realhours, "0.00")).BackColor = Color.LightGoldenrodYellow
                    ListView1.Items(pr).SubItems.Add(Format(diffhsum, "0.00")).BackColor = Color.LightGoldenrodYellow
                    ListView1.Items(pr).SubItems.Add(Format(plancost, "0.000")).BackColor = Color.LightGoldenrodYellow
                    ListView1.Items(pr).SubItems.Add(Format(realcost, "0.000")).BackColor = Color.LightGoldenrodYellow
                    ListView1.Items(pr).SubItems.Add(Format(diffcsum, "0.000")).BackColor = Color.LightGoldenrodYellow
                    totalplanhours += planhours : planhours = 0 : totalplancost += plancost : plancost = 0
                    totalrealhours += realhours : realhours = 0 : totalrealcost += realcost : realcost = 0
                    totaldiffhsum += diffhsum : diffhsum = 0 : totaldiffcsum += diffcsum : diffcsum = 0
                End If
            Next
            ListView1.Items.Add("TOTAL").BackColor = Color.Tan
            pr = ListView1.Items.Count - 1
            ListView1.Items(pr).SubItems.Add("_") : ListView1.Items(pr).SubItems.Add("_") : ListView1.Items(pr).SubItems.Add("_")
            ListView1.Items(pr).SubItems.Add(Format(totalplanhours, "0.00"))
            ListView1.Items(pr).SubItems.Add(Format(totalrealhours, "0.00"))
            ListView1.Items(pr).SubItems.Add(Format(totaldiffhsum, "0.00"))
            ListView1.Items(pr).SubItems.Add(Format(totalplancost, "0.000"))
            ListView1.Items(pr).SubItems.Add(Format(totalrealcost, "0.000"))
            ListView1.Items(pr).SubItems.Add(Format(totaldiffcsum, "0.000"))

        Catch ex As Exception

        End Try
        For y As Integer = 0 To ListView1.Items.Count - 1

            For x As Integer = 1 To ListView1.Items(y).SubItems.Count - 1
                If IsNumeric(ListView1.Items(y).SubItems(x).Text) Then
                    If CDec(ListView1.Items(y).SubItems(x).Text) < 0 Then ListView1.Items(y).SubItems(x).ForeColor = Color.Red
                End If
            Next

        Next



    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dialog11.ShowDialog()
    End Sub


    Private Sub Form12_Resize(sender As System.Object, e As System.EventArgs) Handles MyBase.Resize
        ListView1.Size = New Size(Me.Width - 5, Me.Height - FlowLayoutPanel1.Height - 45)
        FlowLayoutPanel1.Location = New Point(Me.Width - FlowLayoutPanel1.Size.Width - 10, ListView1.Location.Y + ListView1.Height + 5)
    End Sub

    Private Sub Button40_Click(sender As System.Object, e As System.EventArgs) Handles Button40.Click

        Try
            ProgressBar1.Maximum = (ListView1.Columns.Count * ListView1.Items.Count)
            ProgressBar1.Visible = True
            ProgressBar1.Value = 0
            Dim x As Integer = 0
            Dim r As Integer = 1
            ListView1.UseWaitCursor = True
            APP = CreateObject("Excel.Application")
            workbook = APP.Workbooks.Add()
            worksheet = workbook.Sheets.Item(1)
            SaveFileDialog1.InitialDirectory = Environment.SpecialFolder.Desktop

            SaveFileDialog1.FileName = ColumnHeader1.Text & Format(Date.Now.Date, "dd/MM/yyyy ")
            If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim bColor As Integer = 2
                If ListView1.Visible = True Then
                    For Each col As ColumnHeader In ListView1.Columns

                        x += 1
                        worksheet.Cells(1, x).value = col.Text
                        worksheet.Cells(1, x).Interior.Color = Color.LightGray

                    Next

                    For Each item As ListViewItem In ListView1.Items
                        r += 1
                        Try
                            ProgressBar1.Value = (r * x)
                        Catch ex As Exception

                        End Try

                        For count = 0 To item.SubItems.Count - 1
                            worksheet.Cells(r, count + 1).Value = item.SubItems(count).Text
                            For xx = 0 To ListView1.Columns.Count - 1

                                bColor = item.BackColor.ToArgb
                                Dim myBColor As Color = ColorTranslator.FromHtml(bColor)
                                worksheet.Cells(r, xx + 1).Interior.Color = myBColor

                            Next
                        Next
                    Next
                End If
                worksheet.UsedRange.Borders.Weight = xlthick
                worksheet.Columns("A:Q").EntireColumn.Autofit()
                Dim name As String = IO.Path.GetFullPath(SaveFileDialog1.FileName)
                APP.DisplayAlerts = False
                APP.ActiveWorkbook.SaveAs(Filename:=name)
                APP.ActiveWorkbook.Close()
                APP.Quit()
                releaseobject(APP)
                releaseobject(workbook)
                releaseobject(worksheet)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        ListView1.UseWaitCursor = False

        ProgressBar1.Visible = False
    End Sub

    Private Sub Button31_Click(sender As System.Object, e As System.EventArgs) Handles Button31.Click
        PrintDialog1.ShowHelp = True
        PrintDialog1.UseEXDialog = True
        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        PrintDocument1.DefaultPageSettings.Margins.Left = 25
        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PrintDocument1.DefaultPageSettings.Margins.Left = 25
            PrintDocument1.DefaultPageSettings.Margins.Top = 10
            PrintDocument1.DefaultPageSettings.Margins.Bottom = 15
            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
            PrintDocument1.PrinterSettings.DefaultPageSettings.Landscape = True
            PrintPreviewDialog1.Document = PrintDocument1
            PrintPreviewDialog1.ShowDialog()
        End If

    End Sub
    Private Sub pdoc_PrintPage1(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Static CurrentYPosition As Integer = 0
        If ListView1.View = View.Details Then
            PrintDetails(e)
        End If
    End Sub
    Private Sub PrintDetails(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Static LastIndex As Integer = 0
        Static CurrentPage As Integer = 0
        Dim DpiGraphics As Graphics = Me.CreateGraphics
        Dim DpiX As Integer = DpiGraphics.DpiX
        Dim DpiY As Integer = DpiGraphics.DpiY
        DpiGraphics.Dispose()
        Dim X, Y As Integer
        Dim ImageWidth As Integer
        Dim TextRect As Rectangle = Rectangle.Empty
        Dim TextLeftPad As Single = CSng(4 * (DpiX / 96)) '4 pixel pad on the left.
        Dim ColumnHeaderHeight As Single = CSng(ListView1.Font.Height + (10 * (DpiX / 96))) '5 pixel pad on the top an bottom
        Dim StringFormat As New StringFormat
        Dim PageNumberWidth As Single = e.Graphics.MeasureString(CStr(CurrentPage), ListView1.Font).Width
        StringFormat.FormatFlags = StringFormatFlags.NoWrap
        StringFormat.Trimming = StringTrimming.EllipsisCharacter
        StringFormat.LineAlignment = StringAlignment.Center
        CurrentPage += 1
        X = CInt(e.MarginBounds.X)
        Y = CInt(e.MarginBounds.Y)
        For ColumnIndex As Integer = 0 To ListView1.Columns.Count - 1
            TextRect.X = X
            TextRect.Y = Y
            TextRect.Width = ListView1.Columns(ColumnIndex).Width
            TextRect.Height = ColumnHeaderHeight
            e.Graphics.FillRectangle(Brushes.LightGray, TextRect)
            e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
            TextRect.X += TextLeftPad
            TextRect.Width -= TextLeftPad
            e.Graphics.DrawString(ListView1.Columns(ColumnIndex).Text, ListView1.Font, Brushes.Black, TextRect, StringFormat)
            X += TextRect.Width + TextLeftPad
        Next
        Y += ColumnHeaderHeight
        For i = LastIndex To ListView1.Items.Count - 1
            With ListView1.Items(i)
                X = CInt(e.MarginBounds.X)
                If Y + .Bounds.Height > e.MarginBounds.Bottom Then
                    LastIndex = i - 1
                    e.HasMorePages = True
                    StringFormat.Dispose()
                    e.Graphics.DrawString(CStr(CurrentPage), ListView1.Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - ListView1.Font.Height * 2)
                    Exit Sub
                End If
                ImageWidth = 0
                If ListView1.SmallImageList IsNot Nothing Then
                    If Not String.IsNullOrEmpty(.ImageKey) Then
                        e.Graphics.DrawImage(ListView1.SmallImageList.Images(.ImageKey), X, Y)
                    ElseIf .ImageIndex >= 0 Then
                        e.Graphics.DrawImage(ListView1.SmallImageList.Images(.ImageIndex), X, Y)
                    End If
                    ImageWidth = ListView1.SmallImageList.ImageSize.Width
                End If
                For ColumnIndex As Integer = 0 To ListView1.Columns.Count - 1
                    TextRect.X = X
                    TextRect.Y = Y
                    TextRect.Width = ListView1.Columns(ColumnIndex).Width
                    TextRect.Height = .Bounds.Height
                    Dim bColor As Integer = ListView1.Items(i).BackColor.ToArgb
                    Dim myBColor As Color = ColorTranslator.FromHtml(bColor)
                    e.Graphics.FillRectangle(New SolidBrush(myBColor), TextRect)
                    If ListView1.GridLines Then
                        e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
                    End If
                    If ColumnIndex = 0 Then TextRect.X += ImageWidth
                    TextRect.X += TextLeftPad
                    TextRect.Width -= TextLeftPad
                    If ColumnIndex < .SubItems.Count Then
                        Dim TColor As Integer = ListView1.Items(i).SubItems(ColumnIndex).ForeColor.ToArgb
                        Dim myTColor As Color = ColorTranslator.FromHtml(TColor)
                        e.Graphics.DrawString(.SubItems(ColumnIndex).Text, ListView1.Font, New SolidBrush(myTColor), TextRect, StringFormat)
                    End If
                    X += TextRect.Width + TextLeftPad
                Next

                Y += .Bounds.Height
            End With
        Next
        e.Graphics.DrawString(CStr(CurrentPage), ListView1.Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - ListView1.Font.Height * 2)
        StringFormat.Dispose()
        LastIndex = 0
        CurrentPage = 0
    End Sub

  
End Class