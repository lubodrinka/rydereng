
'Imports Excel = Microsoft.Office.Interop.Excel
Public Class Form13
    'Dim APP As New Excel.Application
    'Dim worksheet As Excel.Worksheet
    'Dim workbook As Excel.Workbook
    Private Sub Form13_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lang()

        allspareparts()



    End Sub
    Public Sub allspareparts()

        Try
            'všetky ND prehlad
            ListView1.Items.Clear() : ListView2.Items.Clear() : FlowLayoutPanel1.Show()
            Me.Size = New Size(My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height)
            : ListView1.BringToFront() : ListView1.Show() : Button31.Show() : Button34.Show() : Button35.Show() : Button40.Show()

            Me.SetDesktopBounds(0, 0, My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height)
            ListView1.Size = New Size(Me.Width - 20, Me.Height - FlowLayoutPanel1.Height - 50)
            FlowLayoutPanel1.Location = New Point(Me.Width - FlowLayoutPanel1.Width, ListView1.Location.Y + ListView1.Height + 5)
            ListView1.Update()
            ProgressBar2.Location = New Point(FlowLayoutPanel1.Location.X - ProgressBar2.Width - 5, FlowLayoutPanel1.Location.Y + 5)
            ListView1.Name = ryder.Label11.Text
            ListView2.Name = Button34.Text
            ListView2.Size = ListView1.Size
            ListView2.Location = ListView1.Location
            ListView2.Columns(0).Text = ryder.Label13.Text
            ListView2.Columns(1).Text = ListView1.Columns(9).Text
            ListView2.Columns(2).Text = ListView1.Columns(10).Text
            Dim sumanaklzar1 As Decimal = 0
            Dim sumanaklzar2 As Decimal = 0
            Dim plansumnaklzar1 As Decimal = 0
            Dim plansumnaklzar2 As Decimal = 0
            Dim totalsuma As Decimal = 0
            Dim totalsumap As Decimal = 0


            For Each device As String In Dialog9.CheckedListBox1.CheckedItems
                sumanaklzar1 = 0
                plansumnaklzar1 = 0


                ListView1.Items.Add(Space(10) + ryder.Label13.Text + Space(10) + Trim(device)).BackColor = Color.LemonChiffon
                ListView2.Items.Add(Trim(device)).BackColor = Color.LemonChiffon

                Dim cestaparts As String = Nothing

                cestaparts = Application.StartupPath & "\devices\" & device & "\parts"
                Dim fileContentscb As String
                Dim fileExists As Boolean : If fileExists = My.Computer.FileSystem.FileExists(cestaparts) Then
                    My.Computer.FileSystem.WriteAllText(cestaparts, String.Empty, False)
                End If
                fileContentscb = My.Computer.FileSystem.ReadAllText(cestaparts)
                Dim sNames() As String
                sNames = Split(fileContentscb, vbCrLf)


                For x = 0 To UBound(sNames)
                    Try
                        If sNames(x) <> String.Empty Then
                            Dim riadok() As String = Split(sNames(x), "|")

                            Dim riadok1 As String = Trim(riadok(1))
                            Dim reafter As Integer = riadok(5)
                            Dim nextch As Integer = riadok(7)
                            Dim mot As String = reafter + nextch
                            Dim posdatumv As Date = Nothing
                            Dim datumvy As Integer = riadok(3)

                            Dim pocetriadkov As Integer = ListView1.Items.Count
                            Dim nakladzarok As Decimal = 0
                            Dim percenta As String = "100"
                            Dim intvymdni As Decimal = riadok(3)
                            Dim cena As Decimal = 0
                            Dim intvymmoth As Decimal = riadok(5)
                            Try
                                posdatumv = riadok(0)
                            Catch ex As Exception

                            End Try
                            Try
                                cena = riadok(9)
                            Catch ex As Exception

                            End Try

                            '"ND"
                            ListView1.Items.Add(riadok1).UseItemStyleForSubItems = False

                            Dim differ As String = "-"
                            If IsDate(posdatumv) = True Then

                                Dim oldDate As Date = posdatumv
                                Dim newDate As Date = Now
                                Dim differenceInDays As Double = DateDiff(DateInterval.Day, oldDate, newDate)
                                differ = DateAdd("d", datumvy, posdatumv)
                                '% 
                                If intvymdni > 0 Then
                                    If differenceInDays > intvymdni Then percenta = (200 - ((100 / intvymdni) * differenceInDays))
                                    If percenta < 0 Then percenta = "0,"
                                    Try
                                        nakladzarok = 365 / intvymdni * cena

                                    Catch ex As Exception

                                    End Try
                                End If

                            End If


                            Try
                                Dim labelstr As String = Application.StartupPath & "\devices\" & device & "\label.txt"
                                : Dim fileContentslab As String
                                Dim mt As Byte = 10 : Dim mtdenne As Byte = 11
                                Try
                                    If fileExists = My.Computer.FileSystem.FileExists(labelstr) Then
                                        My.Computer.FileSystem.WriteAllText(labelstr, ryder.day & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf, False)

                                    End If
                                    fileContentslab = My.Computer.FileSystem.ReadAllText(labelstr)
                                    Dim text() As String = Split(fileContentslab, "#")
                                    Dim nm1 As String = Trim(text(mt))
                                    Dim nm2 As String = Trim(text(mtdenne))

                                    Dim poslednyzapis As Integer = riadok(7)
                                    If IsNumeric(nm1) = True And nakladzarok = 0 And intvymmoth > 0 Then
                                        Try

                                            Dim motdene As Integer = nm2
                                            If motdene = 0 Then motdene = 24
                                            nakladzarok = (365 * nm2 / intvymmoth * cena)
                                            If poslednyzapis > nm1 Then nakladzarok = 0.0
                                        Catch ex As Exception

                                        End Try
                                        Try
                                            percenta = (100 - ((100 / intvymmoth) * (nm1 - riadok(7))))
                                            If percenta < 0 Then percenta = "0,"
                                        Catch ex As Exception
                                            percenta = "0,"
                                        End Try

                                    End If

                                    If poslednyzapis > nm1 Then percenta = "100,!"
                                Catch ex As Exception


                                End Try


                                Dim percentazaokruhlene() = Split(percenta, ",")
                                Dim farba As System.Drawing.Color
                                If percentazaokruhlene(0) > 99.4 Then farba = Color.Green
                                If percentazaokruhlene(0) > 50 And percentazaokruhlene(0) < 99.4 Then farba = Color.LightSteelBlue
                                If percentazaokruhlene(0) >= 0.5 And percentazaokruhlene(0) <= 50 Then farba = Color.IndianRed
                                If percentazaokruhlene(0) < 0.5 Then farba = Color.Red
                                Dim percento As String = percentazaokruhlene(0) & "%"
                                If percenta.Contains("!") Then farba = Color.Indigo : percento = "100%!"
                                ListView1.Items(pocetriadkov).SubItems.Add(percento).ForeColor = farba


                                '"výmena dni"
                                ListView1.Items(pocetriadkov).SubItems.Add(differ)
                                'posl. výmena datum"
                                ListView1.Items(pocetriadkov).SubItems.Add(posdatumv)
                                '"int. výmeny dni"
                                ListView1.Items(pocetriadkov).SubItems.Add(intvymdni)
                                '("výmena mth")
                                If riadok(5) <= 0 Then mot = 0
                                ListView1.Items(pocetriadkov).SubItems.Add((mot))
                                '"posl. výmena MTH.")
                                ListView1.Items(pocetriadkov).SubItems.Add(riadok(7))
                                '"int. výmeny MTH."
                                ListView1.Items(pocetriadkov).SubItems.Add(intvymmoth)

                                Try

                                    'cena
                                    ListView1.Items(pocetriadkov).SubItems.Add(cena)
                                    'skutocnynáklad)
                                    Dim Pocetvýmen As Integer = 0
                                    For Each line In ryder.TextBox5.Lines
                                        If line.Contains(riadok1) And line.Contains(Trim(device)) Then Pocetvýmen += +1
                                    Next

                                    'plán nákladov
                                    If Double.IsNaN(nakladzarok) = True Or Double.IsInfinity(nakladzarok) = True Then nakladzarok = 0.0
                                    Dim sknakl As Integer = Pocetvýmen * cena
                                    ListView1.Items(pocetriadkov).SubItems.Add(Format(sknakl, "0.000"))
                                    'plán nákladov

                                    ListView1.Items(pocetriadkov).SubItems.Add(Format(nakladzarok, "0.000"))


                                    sumanaklzar1 += sknakl
                                    plansumnaklzar1 += nakladzarok
                                Catch ex As Exception
                                End Try

                            Catch ex As Exception
                            End Try
                            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(sumanaklzar1, "0.000"))
                            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(plansumnaklzar1, "0.000"))
                            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(sumanaklzar1 - plansumnaklzar1, "0.000"))
                        End If
                    Catch ex As Exception
                    End Try
                Next
                Dim cestasuzarpriečinky As String = Application.StartupPath & "\devices\" & device & "\devparts"
                Dim fileContentsp As String = Nothing
                Dim fileExistsp As Boolean : If fileExistsp = My.Computer.FileSystem.FileExists(cestasuzarpriečinky) Then
                    My.Computer.FileSystem.WriteAllText(cestasuzarpriečinky, String.Empty, False)
                Else : fileContentsp = (My.Computer.FileSystem.ReadAllText(cestasuzarpriečinky))
                End If
                Dim sNamesp() As String : sNamesp = Split(fileContentsp, vbCrLf)
                If sNamesp.Count > 0 Then
                    For xxx As Integer = 0 To sNamesp.Count - 1
                        Dim devparts As String = sNamesp(xxx)
                        sumanaklzar2 = 0
                        plansumnaklzar2 = 0

                        If devparts <> String.Empty Then


                            cestaparts = Application.StartupPath & "\devices\" & device & "\" & devparts & "\parts"
                            ListView1.Items.Add(Space(10) + Trim(ryder.Label14.Text) + Space(10) + devparts).BackColor = Color.LightSteelBlue
                            ListView2.Items.Add(Trim(ryder.Label14.Text & Space(5) & devparts)).BackColor = Color.LightSteelBlue
                            Dim fileContentscb9 As String
                            Dim fileExists9 As Boolean : If fileExists9 = My.Computer.FileSystem.FileExists(cestaparts) Then
                                My.Computer.FileSystem.WriteAllText(cestaparts, String.Empty, False)
                            End If
                            fileContentscb9 = My.Computer.FileSystem.ReadAllText(cestaparts)
                            Dim sNames9() As String
                            sNames9 = Split(fileContentscb9, vbCrLf)
                            Try
                                For x9 = 0 To UBound(sNames9)

                                    If sNames9(x9).Contains("|") Then

                                        Dim riadok9() As String = Split(sNames9(x9), "|")
                                        Dim riadok91 As String = Trim(riadok9(1))
                                        Dim reafter9 As Integer = riadok9(5)
                                        Dim nextch9 As Integer = riadok9(7)
                                        Dim mot9 As String = reafter9 + nextch9
                                        Dim posdatumv9 As Date = Nothing
                                        Dim datumvy9 As Integer = riadok9(3)
                                        Dim pocetriadkov9 As Integer = ListView1.Items.Count
                                        Dim nakladzarok9 As Decimal = 0.0
                                        Dim percenta9 As String = "100"

                                        Dim intvymdni9 As Decimal = riadok9(3)
                                        Dim cena9 As Decimal = 0
                                        Dim intvymmoth9 As Decimal = riadok9(5)

                                        Try
                                            posdatumv9 = riadok9(0)
                                        Catch ex As Exception

                                        End Try
                                        Try
                                            cena9 = riadok9(9)
                                        Catch ex As Exception

                                        End Try
                                        Dim differ9 As String = "-"
                                        '"ND"
                                        ListView1.Items.Add(riadok91).UseItemStyleForSubItems = False
                                        If IsDate(posdatumv9) = True Then
                                            Dim oldDate9 As Date = posdatumv9
                                            Dim newDate9 As Date = Now
                                            Dim differenceInDays9 As Double = DateDiff(DateInterval.Day, oldDate9, newDate9)
                                            '% 
                                            If intvymdni9 > 0 Then
                                                If differenceInDays9 > intvymdni9 Then percenta9 = (200 - ((100 / intvymdni9) * differenceInDays9))
                                                Try
                                                    nakladzarok9 = 365.0 / intvymdni9 * cena9
                                                Catch ex As Exception

                                                End Try
                                            End If
                                            differ9 = DateAdd("d", datumvy9, posdatumv9)
                                            If percenta9 < 0 Then percenta9 = "0,"
                                        End If

                                        Dim labelstr9 As String = Application.StartupPath & "\devices\" & device & "\" & sNamesp(xxx) & "\label.txt"
                                        : Dim fileContentslab9 As String
                                        Dim mt9 As Byte = 10 : Dim mtdenne9 As Byte = 11
                                        'Try
                                        If fileExists = My.Computer.FileSystem.FileExists(labelstr9) Then
                                            My.Computer.FileSystem.WriteAllText(labelstr9, ryder.day & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf, False)

                                        End If
                                        fileContentslab9 = My.Computer.FileSystem.ReadAllText(labelstr9)
                                        Dim text() As String = Split(fileContentslab9, "#")
                                        Dim nm1 As String = Trim(text(mt9))
                                        Dim nm2 As String = Trim(text(mtdenne9))
                                        Dim poslednyzapis As Integer = riadok9(7)
                                        If IsNumeric(nm1) = True And nakladzarok9 = 0 And intvymmoth9 > 0 Then
                                            Try

                                                Dim motdene As Integer = nm2
                                                If motdene = 0 Then motdene = 24
                                                nakladzarok9 = (365 * nm2 / intvymmoth9 * cena9)
                                                If poslednyzapis > nm1 Then nakladzarok9 = 0.0
                                            Catch ex As Exception

                                            End Try
                                            Try
                                                percenta9 = (100 - ((100 / intvymmoth9) * (nm1 - riadok9(7))))
                                                If percenta9 < 0 Then percenta9 = "0,"
                                            Catch ex As Exception
                                                percenta9 = "0,"
                                            End Try

                                        End If

                                        If poslednyzapis > nm1 Then percenta9 = "100,!"
                                        '            Catch ex As Exception


                                        'End Try


                                        Dim percentazaokruhlene() = Split(percenta9, ",")
                                        Dim farba As System.Drawing.Color
                                        If percentazaokruhlene(0) > 99.4 Then farba = Color.Green
                                        If percentazaokruhlene(0) > 50 And percentazaokruhlene(0) < 99.4 Then farba = Color.LightSteelBlue
                                        If percentazaokruhlene(0) >= 0.5 And percentazaokruhlene(0) <= 50 Then farba = Color.IndianRed
                                        If percentazaokruhlene(0) < 0.5 Then farba = Color.Red

                                        Dim percento As String = percentazaokruhlene(0) & "%"
                                        If percenta9.Contains("!") Then farba = Color.Indigo : percento = "100%!"
                                        ListView1.Items(pocetriadkov9).SubItems.Add(percento).ForeColor = farba
                                        '"výmena dni"
                                        ListView1.Items(pocetriadkov9).SubItems.Add(differ9)
                                        'posl. výmena datum"
                                        ListView1.Items(pocetriadkov9).SubItems.Add(riadok9(0))
                                        '"int. výmeny dni"
                                        ListView1.Items(pocetriadkov9).SubItems.Add(intvymdni9)
                                        '("výmena mth")
                                        If riadok9(5) <= 0 Then mot9 = 0
                                        ListView1.Items(pocetriadkov9).SubItems.Add((mot9))
                                        '"posl. výmena MTH.")
                                        ListView1.Items(pocetriadkov9).SubItems.Add(riadok9(7))
                                        '"int. výmeny MTH."
                                        ListView1.Items(pocetriadkov9).SubItems.Add(intvymmoth9)
                                        'cena
                                        Try

                                            'cena
                                            ListView1.Items(pocetriadkov9).SubItems.Add(cena9)
                                            'skutocnynáklad)
                                            Dim Pocetvýmen As Integer = 0
                                            For Each line In ryder.TextBox5.Lines
                                                If line.Contains(riadok91) And line.Contains(Trim(sNames9(xxx))) Then Pocetvýmen += +1
                                            Next

                                            Dim sknakl As Integer = Pocetvýmen * cena9
                                            ListView1.Items(pocetriadkov9).SubItems.Add(Format(sknakl, "0.000"))
                                            'plán nákladov
                                            If Double.IsNaN(nakladzarok9) = True Or Double.IsInfinity(nakladzarok9) = True Then nakladzarok9 = 0.0
                                            ListView1.Items(pocetriadkov9).SubItems.Add(Format(nakladzarok9, "0.000"))
                                            'čas montáže
                                            ListView1.Items(pocetriadkov9).SubItems.Add(riadok9(11))
                                            sumanaklzar2 += sknakl
                                            plansumnaklzar2 += nakladzarok9
                                        Catch ex As Exception

                                        End Try


                                    End If
                                Next

                                ListView1.Update()
                            Catch ex As Exception
                            End Try
                            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(sumanaklzar2, "0.000"))
                            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(plansumnaklzar2, "0.000"))
                            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(sumanaklzar2 - plansumnaklzar2, "0.000"))
                        End If
                    Next
                End If
                totalsuma += sumanaklzar1 + sumanaklzar2
                totalsumap += plansumnaklzar1 + plansumnaklzar2
                ListView2.Items.Add("suma:").ForeColor = Color.MediumBlue

                ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(sumanaklzar1 + sumanaklzar2, "0.000")).Font = New Font("arial", 10, FontStyle.Italic)
                ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(plansumnaklzar1 + plansumnaklzar2, "0.000")).Font = New Font("arial", 10, FontStyle.Italic)
                ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(-plansumnaklzar1 - plansumnaklzar2 + sumanaklzar1 + sumanaklzar2, "0.00")).Font = New Font("arial", 10, FontStyle.Italic)

            Next
            ListView1.Items.Add("suma").BackColor = Color.LightGray
            ListView1.Items(ListView1.Items.Count - 1).Checked = True
            ListView2.Items.Add("Total:").ForeColor = Color.CornflowerBlue
            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(totalsuma, "0.000")).Font = New Font("arial", 10, FontStyle.Italic)
            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(totalsumap, "0.000")).Font = New Font("arial", 10, FontStyle.Italic)
            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(Format(totalsuma - totalsumap, "0.000")).Font = New Font("arial", 10, FontStyle.Italic)
        Catch ex As Exception

        End Try
    End Sub

    

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        Dialog11.ShowDialog()
    End Sub


    Private Sub Form12_Resize(sender As System.Object, e As System.EventArgs) Handles MyBase.Resize
        ListView1.Size = New Size(Me.Width - 5, Me.Height - FlowLayoutPanel1.Height - 45)
        FlowLayoutPanel1.Location = New Point(Me.Width - FlowLayoutPanel1.Size.Width - 10, ListView1.Location.Y + ListView1.Height + 5)
    End Sub



    Private Sub Button31_Click(sender As System.Object, e As System.EventArgs)
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
    Public Sub jazyk()


    End Sub
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
    Private Sub Button40_Click_1(sender As System.Object, e As System.EventArgs) Handles Button40.Click


        Try
            ProgressBar2.Maximum = (ListView1.Columns.Count * ListView1.Items.Count)
            ProgressBar2.Visible = True
            ProgressBar2.Value = 0
            Dim x As Integer = 0
            Dim r As Integer = 1
            ListView1.UseWaitCursor = True
            SaveFileDialog1.InitialDirectory = Environment.SpecialFolder.Desktop
            SaveFileDialog1.FileName = ColumnHeader1.Text & Format(Date.Now.Date, "dd/MM/yyyy ")
            appexcstart()
            APP = CreateObject("Excel.Application")
            workbook = APP.Workbooks.Add()
            worksheet = workbook.Sheets.Item(1)
            If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim bColor As Integer = 2
                If ListView1.Visible = True Then
                    For Each col As ColumnHeader In ListView1.Columns

                        x += 1
                        worksheet.Cells(1, x).value = col.Text
                        worksheet.Cells(1, x).Interior.Color = Color.LightGray
                        'worksheet.Cells(r, x).BorderAround(1, 2, xlcolorautomatic, Color.Black)
                    Next

                    For Each item As ListViewItem In ListView1.Items
                        r += 1
                        Try
                            ProgressBar2.Value = (r * x)
                        Catch ex As Exception

                        End Try

                        For count = 0 To item.SubItems.Count - 1
                            worksheet.Cells(r, count + 1).Value = item.SubItems(count).Text
                            For xx = 0 To ListView1.Columns.Count - 1

                                bColor = item.BackColor.ToArgb
                                Dim myBColor As Color = ColorTranslator.FromHtml(bColor)
                                worksheet.Cells(r, xx + 1).Interior.Color = myBColor
                                'worksheet.Cells(r, xx + 1).BorderAround(1, 2, xlcolorautomatic, Color.Black)
                            Next
                        Next
                    Next
                End If
                If ListView2.Visible = True Then
                    For Each col As ColumnHeader In ListView2.Columns
                        x += 1
                        worksheet.Cells(1, x).Value = col.Text
                        worksheet.Cells(1, x).Interior.Color = Color.LightGray
                        'worksheet.Cells(r, x).BorderAround(1, 2, xlcolorautomatic, Color.Black)
                    Next
                    For Each item As ListViewItem In ListView2.Items
                        r += 1
                        Try
                            ProgressBar2.Value = (r * x)
                        Catch ex As Exception

                        End Try
                        For count = 0 To item.SubItems.Count - 1
                            bColor = item.BackColor.ToArgb
                            Dim myBColor As Color = ColorTranslator.FromHtml(bColor)
                            worksheet.Cells(r, count + 1).Interior.Color = myBColor
                            worksheet.Cells(r, count + 1).Value = item.SubItems(count).Text
                            'worksheet.Cells(r, count + 1).BorderAround(1, 2, xlcolorautomatic, Color.Black)
                        Next

                    Next
                End If
                worksheet.UsedRange.Borders.Weight = xlthick
                worksheet.Columns("A:Q").EntireColumn.Autofit()
                Dim name As String = IO.Path.GetFullPath(SaveFileDialog1.FileName)
                APP.DisplayAlerts = True
                APP.ActiveWorkbook.SaveAs(Filename:=name)
                APP.ActiveWorkbook.Close()
                APP.Quit()
                releaseobject(APP)
                releaseobject(workbook)
                releaseobject(worksheet)
            End If
        Catch ex As Exception

        End Try
        ListView1.UseWaitCursor = False

        ProgressBar2.Visible = False

    End Sub

    Private Sub Button34_Click(sender As System.Object, e As System.EventArgs)
        Button30.Visible = True
        ListView2.Size = ListView1.Size
        ListView2.Location = ListView1.Location
        ListView2.Show()
        ListView2.BringToFront()
        ListView1.Hide()
    End Sub


    Private Sub Button31_Click_1(sender As System.Object, e As System.EventArgs)
        PrintDialog1.AllowSomePages = True
        PrintDialog1.ShowHelp = True
        PrintDialog1.AllowSelection = True
        PrintDialog1.AllowCurrentPage = True
        PrintDialog1.UseEXDialog = True
        PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
        PrintDocument2.PrinterSettings.DefaultPageSettings.Landscape = True
        If ListView2.Visible = True Then PrintDocument2.PrinterSettings.DefaultPageSettings.Landscape = False
        PrintDocument2.DefaultPageSettings.Margins.Left = 10

        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PrintDocument2.DefaultPageSettings.Margins.Left = 10
            PrintDocument2.DefaultPageSettings.Margins.Top = 10
            PrintDocument2.DefaultPageSettings.Margins.Bottom = 15
            PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings

            PrintPreviewDialog1.Document = PrintDocument2
            PrintPreviewDialog1.ShowDialog()
        End If

    End Sub
    Private Sub pdoc_PrintPage4(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        Static CurrentYPosition As Integer = 0
        If ListView1.View = View.Details Then
            PrintDetails4(e)
        End If
    End Sub
    Private Sub PrintDetails4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        If ListView1.Visible = True Then
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
        ElseIf ListView1.Visible = False Then
            Static LastIndex1 As Integer = 0
            Static CurrentPage1 As Integer = 0
            Dim DpiGraphics As Graphics = Me.CreateGraphics
            Dim DpiX As Integer = DpiGraphics.DpiX
            Dim DpiY As Integer = DpiGraphics.DpiY
            DpiGraphics.Dispose()
            Dim X, Y As Integer
            Dim ImageWidth As Integer
            Dim TextRect As Rectangle = Rectangle.Empty
            Dim TextLeftPad As Single = CSng(4 * (DpiX / 96)) '4 pixel pad on the left.
            Dim ColumnHeaderHeight As Single = CSng(ListView2.Font.Height + (10 * (DpiX / 96))) '5 pixel pad on the top an bottom
            Dim StringFormat As New StringFormat
            Dim PageNumberWidth As Single = e.Graphics.MeasureString(CStr(CurrentPage1), ListView2.Font).Width
            StringFormat.FormatFlags = StringFormatFlags.NoWrap
            StringFormat.Trimming = StringTrimming.EllipsisCharacter
            StringFormat.LineAlignment = StringAlignment.Center
            CurrentPage1 += 1
            X = CInt(e.MarginBounds.X)
            Y = CInt(e.MarginBounds.Y)
            For ColumnIndex As Integer = 0 To ListView2.Columns.Count - 1
                TextRect.X = X
                TextRect.Y = Y
                TextRect.Width = ListView2.Columns(ColumnIndex).Width
                TextRect.Height = ColumnHeaderHeight
                e.Graphics.FillRectangle(Brushes.LightGray, TextRect)
                e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
                TextRect.X += TextLeftPad
                TextRect.Width -= TextLeftPad
                e.Graphics.DrawString(ListView2.Columns(ColumnIndex).Text, ListView2.Font, Brushes.Black, TextRect, StringFormat)
                X += TextRect.Width + TextLeftPad
            Next
            Y += ColumnHeaderHeight
            For i = LastIndex1 To ListView2.Items.Count - 1
                With ListView2.Items(i)
                    X = CInt(e.MarginBounds.X)
                    If Y + .Bounds.Height > e.MarginBounds.Bottom Then
                        LastIndex1 = i - 1
                        e.HasMorePages = True
                        StringFormat.Dispose()
                        e.Graphics.DrawString(CStr(CurrentPage1), ListView2.Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - ListView2.Font.Height * 2)
                        Exit Sub
                    End If
                    ImageWidth = 0
                    If ListView2.SmallImageList IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ImageKey) Then
                            e.Graphics.DrawImage(ListView2.SmallImageList.Images(.ImageKey), X, Y)
                        ElseIf .ImageIndex >= 0 Then
                            e.Graphics.DrawImage(ListView2.SmallImageList.Images(.ImageIndex), X, Y)
                        End If
                        ImageWidth = ListView2.SmallImageList.ImageSize.Width
                    End If
                    For ColumnIndex As Integer = 0 To ListView2.Columns.Count - 1
                        TextRect.X = X
                        TextRect.Y = Y
                        TextRect.Width = ListView2.Columns(ColumnIndex).Width
                        TextRect.Height = .Bounds.Height
                        Dim bColor As Integer = ListView2.Items(i).BackColor.ToArgb
                        Dim myBColor As Color = ColorTranslator.FromHtml(bColor)
                        e.Graphics.FillRectangle(New SolidBrush(myBColor), TextRect)
                        If ListView2.GridLines Then
                            e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
                        End If
                        If ColumnIndex = 0 Then TextRect.X += ImageWidth
                        TextRect.X += TextLeftPad
                        TextRect.Width -= TextLeftPad
                        If ColumnIndex < .SubItems.Count Then
                            Dim TColor As Integer = ListView2.Items(i).SubItems(ColumnIndex).ForeColor.ToArgb
                            Dim myTColor As Color = ColorTranslator.FromHtml(TColor)
                            e.Graphics.DrawString(.SubItems(ColumnIndex).Text, ListView2.Font, New SolidBrush(myTColor), TextRect, StringFormat)
                        End If
                        X += TextRect.Width + TextLeftPad
                    Next

                    Y += .Bounds.Height
                End With
            Next
            e.Graphics.DrawString(CStr(CurrentPage1), ListView2.Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - ListView2.Font.Height * 2)
            StringFormat.Dispose()
            LastIndex1 = 0
            CurrentPage1 = 0

        End If


    End Sub

    Private Sub Button30_Click_1(sender As System.Object, e As System.EventArgs)
        If ListView2.Visible Then
            ListView2.Hide()
            ListView1.Show()
            Button30.Visible = False
        ElseIf ListView2.Visible = False Then
            ListView1.Hide()
            FlowLayoutPanel1.Hide()
            FlowLayoutPanel1.Location = New Point(Me.Location.X, ListView1.Bottom)

            Me.Location = New Point(((My.Computer.Screen.WorkingArea.Width / 2) - (Me.Width / 2)), 0)
        End If
    End Sub

    Private Sub Button35_Click(sender As System.Object, e As System.EventArgs) Handles Button35.Click


        If ListView1.CheckedItems.Count = 0 Then Exit Sub
        Dim cestaparts As String = String.Empty
        Dim zar() As String
        Dim szar() As String
        Dim NDlw As String = String.Empty
        Dim sučastzar As String = " "
        Dim cestazar As String = String.Empty
        Dim device As String = String.Empty
        Dim labelstr As String = String.Empty
        Dim datenow As Date = Date.Now
        APP = CreateObject("Excel.Application")
        workbook = APP.Workbooks.Add()
        worksheet = workbook.Sheets.Item(1)
        For Each li As ListViewItem In ListView1.Items

            If li.SubItems.Count < 2 And li.Text.Contains(ryder.Label13.Text) And Not li.Text.Contains(ryder.Label14.Text) Then
                zar = Split(li.Text, ryder.Label13.Text)
                device = (Trim(zar(1)))
                cestaparts = Application.StartupPath & "\devices\" & device & "\parts"
                labelstr = Application.StartupPath & "\devices\" & device & "\label.txt"

                sučastzar = " "
            End If


            If li.SubItems.Count < 2 And li.Text.Contains(ryder.Label14.Text) Then
                szar = Split(li.Text, ryder.Label14.Text)
                sučastzar = Trim(szar(1)).Replace(vbTab, "")
                cestaparts = Application.StartupPath & "\devices\" & device & "\" & sučastzar & "\parts"
                labelstr = Application.StartupPath & "\devices\" & device & "\" & sučastzar & "\label.txt"
            End If


            If ListView1.CheckedItems.Contains(li) And li.Index < ListView1.Items.Count - 1 Then
                'Try
                NDlw = Trim(li.Text)
                'tb = week & "|" & datenow.Date & "|" & Space(2) & zariadeniecb1 & vbTab & " " & vbTab & NDlw.ToString & vbTab & interval & vbTab & údržba & vbTab & Dialog6.TextBox1.Text & vbTab & user + vbNewLine
                Dim tb As String = ryder.week & "|" & datenow & "|" & Space(2) & device & vbTab & sučastzar & vbTab & NDlw & vbTab & " " & vbTab & " " & vbTab & " " & vbTab & ryder.user & vbNewLine
                My.Computer.FileSystem.WriteAllText(ryder.cesta, tb, True)
                cestazar = Application.StartupPath & "\devices\" & device & "\" & device
                My.Computer.FileSystem.WriteAllText(cestazar, tb, True)
                'Try
                If Dialog4.CheckedListBox1.GetItemCheckState(0) = CheckState.Unchecked Then
                    ryder.excelopenfile()
                    Dim LastRow As Long
                    'Dim Range As APP.Range("A1")
                    With worksheet
                        LastRow = .Cells(.Rows.Count, 2).End(XlDirectionxlUP).Row
                    End With
                    worksheet.Cells(LastRow + 1, 1).Value = ryder.week
                    worksheet.Cells(LastRow + 1, 2).Value = datenow
                    worksheet.Cells(LastRow + 1, 3).Value = device & sučastzar
                    'worksheet.Cells(LastRow + 1, 4).Value = lenudrzba(1)
                    If NDlw.Length > 1 Then
                        worksheet.Cells(LastRow + 1, 5).Value = NDlw
                    End If
                    'worksheet.Cells(LastRow + 1, 6).Value = TextBox1.Text
                    worksheet.Cells(LastRow + 1, 7).Value = ryder.user
                    worksheet.Cells(LastRow + 1, 8).Value = ""
                    worksheet.Columns("A:Q").EntireColumn.Autofit()
                    worksheet.UsedRange.Borders.Weight = xlthick
                    APP.DisplayAlerts = True
                    APP.ActiveWorkbook.Save()
                    APP.ActiveWorkbook.Close()
                    APP.Quit()
                    releaseobject(APP)
                    releaseobject(workbook)
                    releaseobject(worksheet)
                End If
                'Catch ex As Exception
                'End Try



                'Catch ex As Exception

                'End Try

                Dim fileExists As Boolean
                Dim fileContentslab As String
                Dim mt As Byte = 10
                'Try

                If fileExists = My.Computer.FileSystem.FileExists(labelstr) Then
                    My.Computer.FileSystem.WriteAllText(labelstr, ryder.day & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf, False)

                End If
                fileContentslab = My.Computer.FileSystem.ReadAllText(labelstr)
                Dim text() As String = Split(fileContentslab, "#")
                Dim nm1 As Integer = Trim(text(mt))
                Dim cenaačasvymeny As String = " |$|0 |TTE|0"
                Dim info As String = " |O.Nu| |MD|"
                Dim novydtND As String = ryder.day & "|" & " |IV|" & " |MH|" & "|MHL|" & cenaačasvymeny
                Dim fileContentsND As String = My.Computer.FileSystem.ReadAllText(cestaparts)
                My.Computer.FileSystem.WriteAllText(cestaparts, String.Empty, False)
                Dim snames() As String = Split(fileContentsND, vbCrLf)
                For linum = 0 To UBound(snames)
                    Dim line As String = snames(linum)
                    If line.Contains(Trim(NDlw)) And line <> String.Empty Then
                        Dim ND() As String = Split(line, "|")

                        If UBound(ND) >= 11 Then
                            cenaačasvymeny = " |$|" & ND(9) & " |TTE|" & ND(11)
                        End If


                        If UBound(ND) >= 15 Then
                            info = " |O.Nu|" & ND(13) & " |MD|" & ND(15)

                        End If
                        novydtND = ryder.day & "|" & ND(1) & " |IV|" & ND(3) & " |MH|" & ND(5) & "|MHL|" & nm1 & cenaačasvymeny & info
                        My.Computer.FileSystem.WriteAllText(cestaparts, novydtND & vbCrLf, True)

                    Else
                        If line.Contains("|IV|") Then
                            novydtND = line
                            My.Computer.FileSystem.WriteAllText(cestaparts, novydtND & vbCrLf, True)
                        End If


                    End If

                Next
                Button35.BackColor = Color.AliceBlue
                Timer1.Enabled = True
                'Catch ex As Exception
                'End Try


            End If
        Next

        allspareparts()
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Button35.BackColor = Color.Red
    End Sub
    Public Sub lang()
        Try
            Dim lang As String = Application.StartupPath & "\" & ryder.user & ".lng"
            Dim fileExists As Boolean
            fileExists = My.Computer.FileSystem.FileExists(lang)
            If fileExists = True Then
                Dim nahraj As String = My.Computer.FileSystem.ReadAllText(lang)
                Dim sNames() As String : Dim x As Long : sNames = Split(nahraj, vbCrLf)
                For x = 0 To UBound(sNames)
                    If sNames(x).Contains(vbTab) Then
                        Dim colums() = Split(sNames(x), vbTab)
                        Try
                            If colums(1).Contains(".") Then
                                Dim form() = Split(colums(1), ".")
                                Select Case form(0)
                                    Case "ryder"
                                        If "ColumnHeader1" = form(1) Then ColumnHeader1.Text = colums(0)
                                        If "ColumnHeader2" = form(1) Then ColumnHeader2.Text = colums(0)
                                        If "ColumnHeader3" = form(1) Then ColumnHeader3.Text = colums(0)
                                        If "ColumnHeader4" = form(1) Then ColumnHeader4.Text = colums(0)
                                        If "ColumnHeader5" = form(1) Then ColumnHeader5.Text = colums(0)
                                        If "ColumnHeader6" = form(1) Then ColumnHeader6.Text = colums(0)
                                        If "ColumnHeader7" = form(1) Then ColumnHeader7.Text = colums(0)
                                        If "ColumnHeader8" = form(1) Then ColumnHeader8.Text = colums(0)
                                        If "ColumnHeader9" = form(1) Then ColumnHeader9.Text = colums(0)
                                        If "ColumnHeader10" = form(1) Then ColumnHeader10.Text = colums(0)
                                        If "ColumnHeader11" = form(1) Then ColumnHeader11.Text = colums(0)
                                        If "ColumnHeader12" = form(1) Then ColumnHeader12.Text = colums(0)

                                    Case Else
                                End Select
                            End If
                        Catch ex As Exception
                        End Try
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub

  
End Class