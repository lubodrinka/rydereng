Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Text
Imports System.Drawing.Text

Public Class Form5
    Private lastin As Decimal
    Private Sub ToolStripButton2_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton2.Click
        PrintDialog1.AllowSomePages = True
        PrintDialog1.ShowHelp = True
        PrintDialog1.AllowSelection = True
        PrintDialog1.AllowCurrentPage = True
        PrintDialog1.UseEXDialog = True
        ListView1.CheckBoxes = False
        If ToolStripComboBox2.Text <> Nothing Then
            lastin = ListView1.Height
        Else

            lastin = CDec(ListView1.Height / (ListView1.TopItem.Font.Height + 3))
        End If


        PrintDialog1.PrinterSettings.DefaultPageSettings.Landscape = False
        Dim okraj As Integer = CInt((PrintDialog1.PrinterSettings.DefaultPageSettings.PaperSize.Width - Panel1.Width) / 2)
        PrintDialog1.PrinterSettings.DefaultPageSettings.Margins.Left = okraj
        PrintDialog1.PrinterSettings.DefaultPageSettings.Margins.Right = okraj
        PrintDialog1.PrinterSettings.DefaultPageSettings.Margins.Top = 20
        PrintDialog1.PrinterSettings.DefaultPageSettings.Margins.Bottom = 10

        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then


            PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
            PrintPreviewDialog1.Document = PrintDocument2
            PrintPreviewDialog1.ShowDialog()
        End If
        'End If
    End Sub
    Private Sub PrintDocument2_PrintPage(sender As System.Object, e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        Try

            Dim arialFont As Font = Me.ListView1.Font

            If ToolStripComboBox1.Text > Nothing And ToolStripComboBox1.Text > Nothing Then
                Try
                    Dim userFont As New Font(ToolStripComboBox1.Text, CInt(ToolStripComboBox2.Text))
                    arialFont = userFont
                Catch ex As Exception

                End Try
            Else
                arialFont = (ListView1.Font)
            End If



            Static index As Integer
            Dim CurrentYPosition As Integer
            Dim CurrentPage As Integer = 0
            Dim X, Y As Integer
            Dim TextRect As Rectangle = Rectangle.Empty
            Dim PageNumberWidth As Single = e.Graphics.MeasureString(CStr(CurrentPage), Panel1.Font).Width
            Dim StringFormat As New StringFormat
            X = CInt(e.MarginBounds.X)
            Y = CInt(e.MarginBounds.Y)

            e.Graphics.DrawRectangle(Pens.Silver, New Rectangle(X + 200, Y + ListView1.Bottom + 10, 110, 150))
            e.Graphics.DrawRectangle(Pens.Silver, New Rectangle(X + 315, Y + ListView1.Bottom + 10, 110, 150))
            TextRect = e.MarginBounds

            e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
            TextRect = Rectangle.Empty
            For i = 0 To Me.Panel1.Controls.Count - 1

                TextRect.X = Me.Panel1.Controls(i).Location.X + X
                TextRect.Y = Me.Panel1.Controls(i).Location.Y + Y
                TextRect.Width = Me.Panel1.Controls(i).Width
                TextRect.Height = Me.Panel1.Controls(i).Height


                If Me.Panel1.Controls(i).ToString.Contains("Label") = False Then
                    Dim bColor As Integer = Me.Panel1.Controls(i).BackColor.ToArgb
                    Dim myBColor As Color = ColorTranslator.FromHtml(CStr(bColor))

                    e.Graphics.FillRectangle(New SolidBrush(myBColor), TextRect)
                    e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
                    StringFormat.FormatFlags = StringFormatFlags.FitBlackBox
                    StringFormat.Trimming = StringTrimming.EllipsisPath
                    StringFormat.LineAlignment = StringAlignment.Near
                End If

                Dim font1 As New Font(ToolStripComboBox1.Text, Me.Panel1.Controls(i).Font.Size, Panel1.Controls(i).Font.Style)
                If Me.Panel1.Controls(i).ToString.Contains("Label") Then
                    TextRect.Width = Me.Panel1.Controls(i).MaximumSize.Width
                    TextRect.Height = font1.Height
                    StringFormat.FormatFlags = StringFormatFlags.NoClip
                    StringFormat.Trimming = StringTrimming.None
                    StringFormat.LineAlignment = StringAlignment.Near
                End If

                Dim TColor As Integer = Me.Panel1.Controls(i).ForeColor.ToArgb
                Dim myTColor As Color = ColorTranslator.FromHtml(TColor)


                e.Graphics.DrawString(Me.Panel1.Controls(i).Text, font1, New SolidBrush(myTColor), TextRect, StringFormat)


            Next
            Try
                e.Graphics.DrawImage(Me.PictureBox1.Image, New Rectangle(Me.PictureBox1.Location.X + X, Me.PictureBox1.Location.Y + Y, Me.PictureBox1.Width, Me.PictureBox1.Height))
            Catch ex As Exception

            End Try


            Dim widthsum As Integer
            Dim lcy As Integer = ListView1.Location.Y + e.MarginBounds.Top
            Dim lcx As Integer = ListView1.Location.X + e.MarginBounds.X
            Dim crect As Rectangle = Rectangle.Empty


            For i = 0 To ListView1.Columns.Count - 2
                crect.X = lcx + widthsum
                crect.Y = lcy
                crect.Width = ListView1.Columns(i).Width
                crect.Height = arialFont.Height * 2

                e.Graphics.DrawRectangle(Pens.DarkGray, crect)
                e.Graphics.FillRectangle(New SolidBrush(Color.LightGray), crect)
                e.Graphics.DrawString(ListView1.Columns(i).Text, arialFont, Brushes.Black, crect, StringFormat)
                widthsum += ListView1.Columns(i).Width

            Next


            'MsgBox(lastin)

            CurrentYPosition = CInt(arialFont.Height + 0.4)
            For ItemCount = index To ListView1.Items.Count - 1
                CurrentYPosition = CInt(CurrentYPosition + (arialFont.Height + 0.4))
                Dim Item As ListViewItem = ListView1.Items(ItemCount)
                widthsum = 0
                For i = 0 To ListView1.Columns.Count - 2
                    crect.X = lcx + widthsum
                    crect.Width = ListView1.Columns(i).Width
                    crect.Height = arialFont.Height + CInt(0.5)
                    crect.Y = lcy + CurrentYPosition
                    e.Graphics.DrawRectangle(Pens.DarkGray, crect)
                    widthsum += ListView1.Columns(i).Width
                Next

                For a As Integer = 0 To Item.SubItems.Count - 1
                    If Item.SubItems(a).Text.Contains("%") = False Then
                        e.Graphics.DrawString(Item.SubItems(a).Text, arialFont, Brushes.Black, Item.SubItems(a).Bounds.X + lcx, CurrentYPosition + lcy, StringFormat)
                    End If
                Next

                If CurrentYPosition > lastin - (arialFont.Height + 0.4) * 2 Then
                    index = ItemCount + 1
                    e.HasMorePages = True
                    e.Graphics.DrawString(CStr(CurrentPage), Me.Panel1.Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - Me.Panel1.Font.Height * 2)
                    CurrentPage += 1
                    CurrentYPosition = CInt(arialFont.Height + 0.4)
                    Exit Sub
                End If
            Next


            index = 0
        Catch ex As Exception

        End Try
    End Sub
  

    Private Sub Form5_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            Dim formauhrady As String = Nothing
            Dim ItemArray(Me.ComboBox1.Items.Count - 1) As Object
            Me.ComboBox1.Items.CopyTo(ItemArray, 0)
            formauhrady = Join(ItemArray, "##")
            Dim spdopravy As String = Nothing
            Dim ItemArray1(Me.ComboBox2.Items.Count - 1) As Object
            Me.ComboBox2.Items.CopyTo(ItemArray1, 0)
            spdopravy = Join(ItemArray1, "##")

            If My.Settings.order = Nothing Then
                Dim starytext As String = TextBox1.Text & "///" & TextBox2.Text & "///" & TextBox3.Text & "///" & TextBox4.Text & "///" & TextBox5.Text & "///" & TextBox6.Text & "///" & TextBox7.Text & "///" & TextBox8.Text & "///" & NumericUpDown1.Value & "///" & NumericUpDown2.Value & "///" & NumericUpDown3.Value & "///" & TextBox11.Text & "///" & TextBox16.Text & "///" & TextBox14.Text & "///" & TextBox13.Text & "///" & TextBox15.Text & "///" & TextBox18.Text & "///" & TextBox19.Text & "///" & ComboBox1.SelectedIndex & "##" & formauhrady & "///" & ComboBox2.SelectedIndex & "##" & spdopravy
                My.Settings.order = starytext
            End If
            Dim text() As String = Split(My.Settings.order, "///")
            text(15) = TextBox15.Text
            Dim xxx As New StringBuilder
            For Each li In text
                xxx.Append(li & "///")
            Next

            My.Settings.order = xxx.ToString
        Catch ex As Exception

        End Try
        My.Settings.form5font = ListView1.Font
    End Sub
    Private Sub SaveToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripButton.Click
        Dim formauhrady As String = Nothing
        Dim ItemArray(Me.ComboBox1.Items.Count - 1) As Object
        Me.ComboBox1.Items.CopyTo(ItemArray, 0)
        formauhrady = Join(ItemArray, "##")
        Dim spdopravy As String = Nothing
        Dim ItemArray1(Me.ComboBox2.Items.Count - 1) As Object
        Me.ComboBox2.Items.CopyTo(ItemArray1, 0)
        spdopravy = Join(ItemArray1, "##")

        Dim text As String = TextBox1.Text & "///" & TextBox2.Text & "///" & TextBox3.Text & "///" & TextBox4.Text & "///" & TextBox5.Text & "///" & TextBox6.Text & "///" & TextBox7.Text & "///" & TextBox8.Text & "///" & NumericUpDown1.Value & "///" & NumericUpDown2.Value & "///" & NumericUpDown3.Value & "///" & TextBox11.Text & "///" & TextBox16.Text & "///" & TextBox14.Text & "///" & TextBox13.Text & "///" & TextBox15.Text & "///" & TextBox18.Text & "///" & TextBox19.Text & "///" & ComboBox1.SelectedIndex & "##" & formauhrady & "///" & ComboBox2.SelectedIndex & "##" & spdopravy
        My.Settings.order = text
    End Sub
    Private Sub panel1_enter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Panel1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub panel1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Panel1.DragDrop
        Try
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim fileExists As Boolean
                fileExists = My.Computer.FileSystem.FileExists(Application.StartupPath & "\logo")
                If fileExists = True Then
                    Me.PictureBox1.Dispose()
                End If

            End If
            Dim filePaths As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            For Each fileLoc As String In filePaths
                If File.Exists(fileLoc) Then
                    'urobi kopiu file do priečinku 

                    FileCopy(Path.GetFullPath(fileLoc), Application.StartupPath & "\logo")
                    Me.PictureBox1.Load(Application.StartupPath & "\logo")

                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Form5_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Dim fnts As New InstalledFontCollection
        For Each f As FontFamily In fnts.Families()
            ToolStripComboBox1.Items.Add(f.Name)
        Next
        For h As Integer = 8 To 12
            ToolStripComboBox2.Items.Add(h)
        Next

        'ToolStripComboBox1.Text = ListView1.Font.Name
        'ToolStripComboBox2.Text = ListView1.Font.Size
        ToolStripComboBox1.Text = My.Settings.form5font.Name
        ToolStripComboBox2.Text = My.Settings.form5font.Size
        Try
            Dim lang As String = Application.StartupPath & "\" & My.Forms.Login.ComboBox2.Text & ".lng"
            Dim fileExists1 As Boolean
            fileExists1 = My.Computer.FileSystem.FileExists(lang)
            If fileExists1 = True Then
                Dim nahraj As String = My.Computer.FileSystem.ReadAllText(lang)
                Dim sNames() As String : Dim x As Long : sNames = Split(nahraj, vbCrLf)
                For x = 0 To UBound(sNames)
                    If sNames(x).Contains(vbTab) Then
                        Dim colums() = Split(sNames(x), vbTab)
                        If colums(1).Contains(".") Then
                            Dim form() = Split(colums(1), ".")
                            Select Case form(0)
                                Case "Form5"
                                    Try
                                        For Each menco As Control In Panel1.Controls
                                            If menco.Name = form(2) Then menco.Text = colums(0)
                                        Next
                                    Catch ex As Exception

                                    End Try

                                    If "ColumnHeader1" = form(1) Then ColumnHeader1.Text = colums(0)

                                    If "ColumnHeader2" = form(1) Then ColumnHeader2.Text = colums(0) : ColumnHeader2.Width = ColumnHeader2.Text.Length * ListView1.Font.Size
                                    If "ColumnHeader3" = form(1) Then ColumnHeader3.Text = colums(0) : ColumnHeader3.Width = ColumnHeader3.Text.Length * ListView1.Font.Size
                                    If "ColumnHeader4" = form(1) Then ColumnHeader4.Text = colums(0) : ColumnHeader4.Width = ColumnHeader4.Text.Length * ListView1.Font.Size
                                    If "ColumnHeader5" = form(1) Then ColumnHeader5.Text = colums(0) : ColumnHeader5.Width = ColumnHeader5.Text.Length * ListView1.Font.Size
                                    If "ColumnHeader6" = form(1) Then ColumnHeader6.Text = colums(0) : ColumnHeader6.Width = ColumnHeader6.Text.Length * ListView1.Font.Size
                                    If "ColumnHeader7" = form(1) Then ColumnHeader7.Text = colums(0) : ColumnHeader7.Width = ColumnHeader7.Text.Length * ListView1.Font.Size
                                Case Else
                            End Select
                        End If
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
        Label11.Text = My.Settings.mena
        Label19.Text = My.Settings.mena




        Try
            Me.Panel1.AllowDrop = True
            Dim fileExists As Boolean
            fileExists = My.Computer.FileSystem.FileExists(Application.StartupPath & "\logo")
            If fileExists = True Then

                Me.PictureBox1.Load(Application.StartupPath & "\logo")
            End If
        Catch ex As Exception

        End Try
        Dim la14 As String = ryder.TextBox19.Text.Replace(vbCrLf, "")

        TextBox5.Text = Trim(ryder.TextBox15.Text)
        Label13.Text = Trim(ryder.Label13.Text & Space(5) & Trim(ryder.ComboBox1.Text))
        Label14.Text = Trim(ryder.Label15.Text & Space(ryder.Label13.Text.Length + 6) & Trim(la14.ToString))
        Dim uniquenumber As Integer = 0
        Try
            Label12.Text = ryder.ComboBox4.Text

            'Dim text As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\OR")

            Dim snames() As String = Split(My.Settings.order, "///")

            TextBox1.Text = snames(0) : TextBox2.Text = snames(1) : TextBox3.Text = snames(2)
            TextBox4.Text = snames(3) : TextBox5.Text = snames(4) : TextBox6.Text = snames(5)
            TextBox7.Text = snames(6) : TextBox8.Text = snames(7) : NumericUpDown1.Value = snames(8)
            NumericUpDown2.Value = snames(9) : NumericUpDown3.Value = snames(10) : TextBox11.Text = snames(11)
            TextBox16.Text = snames(12) : TextBox14.Text = snames(13) : TextBox13.Text = snames(14)
            'TextBox17.Text = Date.Now.Date

            If snames(16) <> String.Empty Then TextBox18.Text = snames(16)
            If snames(17) <> String.Empty Then TextBox19.Text = snames(17)
            Try : Dim cb1() As String = Split(snames(18), "##") : For x = 1 To UBound(cb1) : If cb1(x) <> String.Empty Then ComboBox1.Items.Add(cb1(x))
                Next : ComboBox1.SelectedIndex = (cb1(0))
            Catch ex As Exception : End Try
            Try : Dim cb2() As String = Split(snames(19), "##") : For x = 1 To UBound(cb2) : If cb2(x) <> String.Empty Then ComboBox2.Items.Add(cb2(x))
                    : Next : ComboBox2.SelectedIndex = (cb2(0))
                : Catch ex As Exception : End Try
            If IsNumeric(snames(15)) Then uniquenumber = CInt(snames(15))
            uniquenumber += 1
            TextBox15.Text = CStr(uniquenumber)
        Catch ex As Exception


        End Try
        If TextBox5.Text = String.Empty Then TextBox5.Text = Trim(ryder.TextBox15.Text)
        Try
            TextBox12.Text = CStr(DateAdd(DateInterval.Day, NumericUpDown1.Value, Date.Now.Date))
        Catch ex As Exception
        End Try
        TextBox9.Text = CStr(Date.Now.Date)
       
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        TextBox12.Text = CStr(DateAdd(DateInterval.Day, NumericUpDown1.Value, Date.Now.Date))
    End Sub

    Private Sub TextBox9_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox9.TextChanged
        Try
            TextBox12.Text = CStr(DateAdd(DateInterval.Day, NumericUpDown1.Value, Date.Now.Date))
        Catch ex As Exception

        End Try

    End Sub


    Private Sub CutToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles CutToolStripButton.Click
        If ComboBox1.Focused = True Then ComboBox1.Items.Remove(ComboBox1.SelectedItem)
        If ComboBox2.Focused = True Then ComboBox2.Items.Remove(ComboBox2.SelectedItem)

        Dim čas As Double = 0
        Dim sumaceny As Decimal = 0
        Dim sumacenydph As Decimal = 0
        Dim dph As Decimal = (1 + (NumericUpDown3.Value / 100))
        For Each item In ListView1.CheckedItems
            If item.checked Then item.Remove()
        Next



        For Each item In ListView1.Items
            Try

                sumaceny += item.SubItems.Item(3).Text
                sumacenydph += item.SubItems.Item(4).Text
                čas += item.SubItems.Item(5).Text
            Catch ex As Exception
            End Try

        Next
        'dffdfdf
        Label4.Text = CStr(čas)
        Label8.Text = CStr(čas * NumericUpDown2.Value)
        Label3.Text = Format(sumaceny, "0.000")
        Label16.Text = Format(sumaceny * dph, "0.000")
        Label23.Text = Format((Label8.Text + Label22.Text), "0.000")
        Label15.Text = Format((Label8.Text + Label22.Text) * dph, "0.000")
        TextBox77.Text = Format(((čas * NumericUpDown2.Value) + sumaceny), "0.000")
        TextBox66.Text = Format((((čas * NumericUpDown2.Value) + sumaceny) * dph), "0.000")


    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView1.ItemActivate
        Dim dph As Decimal = (1 + (NumericUpDown3.Value / 100))
        Try

            Dialog5.Label7.Text = ListView1.FocusedItem.Index
            Dialog5.Label1.Text = ListView1.Columns(0).Text
            Dialog5.Label2.Text = ListView1.Columns(1).Text
            Dialog5.Label3.Text = ListView1.Columns(2).Text
            Dialog5.Label4.Text = ListView1.Columns(3).Text
            Dialog5.Label5.Text = ListView1.Columns(4).Text
            Dialog5.Label6.Text = ListView1.Columns(5).Text

            Dialog5.TextBox1.Text = ListView1.FocusedItem.SubItems.Item(0).Text
            Dialog5.N1.Value = ListView1.FocusedItem.SubItems.Item(1).Text
            Dialog5.N2.Value = ListView1.FocusedItem.SubItems.Item(2).Text
            Dialog5.TextBox4.Text = ListView1.FocusedItem.SubItems.Item(3).Text
            Dialog5.TextBox6.Text = ListView1.FocusedItem.SubItems.Item(4).Text
            Dialog5.N3.Value = ListView1.FocusedItem.SubItems.Item(5).Text
            Dialog5.Label8.Text = ListView1.FocusedItem.SubItems.Item(6).Text


        Catch ex As Exception

        End Try


        If Dialog5.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim čas As Double = 0
            Dim sumaceny As Decimal = 0
            Dim sumacenydph As Decimal = 0
            For Each item In ListView1.Items
                Try
                    sumaceny += item.SubItems.Item(3).Text
                    sumacenydph += item.SubItems.Item(4).Text
                    čas += item.SubItems.Item(5).Text
                Catch ex As Exception

                End Try



            Next

            Label4.Text = čas
            Label8.Text = čas * NumericUpDown2.Value
            Label3.Text = Format(sumaceny, "0.000")
            Label16.Text = Format(sumaceny * dph, "0.000")
            Label23.Text = Format((CDec(Label8.Text) + CDec(Label22.Text)), "0.000")
            Label15.Text = Format((Label23.Text) * dph, "0.000")
            TextBox77.Text = Format(((Label23.Text) + sumaceny), "0.000")
            TextBox66.Text = Format((((Label23.Text) + sumaceny) * dph), "0.000")
        End If

    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        Dim dph As Decimal = (1 + (NumericUpDown3.Value / 100))
        Try
            Label8.Text = (Label4.Text * NumericUpDown2.Value)
            Label23.Text = Format((CDec(Label8.Text) + CDec(Label22.Text)), "0.000")
            Label15.Text = Format((Label23.Text) * dph, "0.000")
            TextBox77.Text = Format(Label23.Text + Label3.Text, "0.000")
            TextBox66.Text = Format(((Label23.Text) + Label3.Text) * (1 + (NumericUpDown3.Value / 100)), "0.000")
        Catch ex As Exception

        End Try

    End Sub

    Private Sub NumericUpDown3_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown3.ValueChanged
        Dim dph As Decimal = (1 + (NumericUpDown3.Value / 100))
        Try
            Label16.Text = Format(Label3.Text * dph, "0.000")
            Label23.Text = Format((CDec(Label8.Text) + CDec(Label22.Text)), "0.000")
            Label15.Text = Format((Label23.Text) * dph, "0.000")
            TextBox66.Text = Format(((Label23.Text) + Label3.Text) * (1 + (NumericUpDown3.Value / 100)), "0.000")
        Catch ex As Exception

        End Try

    End Sub


    Private Sub ToolStripButton1_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton1.Click
        Try
            APP = CreateObject("Excel.Application")
            If APP.VERSION.replace(".", ",") > 11.9 Then
                excelextension = "xlsx"
                SaveFileDialog1.Filter = "excel files (*.xlsx)|.xlsx|(*.xls)|.xls|All files (*.*)|*.*"
            Else
                SaveFileDialog1.Filter = "excel files (*.xls)|.xls|All files (*.*)|*.*"
                excelextension = "xls"
            End If
            workbook = APP.Workbooks.Add()
            worksheet = workbook.Sheets.Item(1)
            ToolStripProgressBar1.Maximum = (ListView1.Columns.Count * ListView1.Items.Count)
            ToolStripProgressBar1.Visible = True
            ToolStripProgressBar1.Value = 0
            Dim x As Integer = 1
            Dim r As Integer = 7
            Dim i As Integer = 0
            ListView1.UseWaitCursor = True
            SaveFileDialog1.InitialDirectory = Environment.SpecialFolder.Desktop
            SaveFileDialog1.DefaultExt = excelextension
            SaveFileDialog1.FileName = Me.Text & Format(Date.Now, "dd_MM_yyyy_hh_mm_ss ")
            ToolStripProgressBar1.Style = ProgressBarStyle.Marquee
            If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                Try
                    worksheet.Shapes.AddPicture(Application.StartupPath & "\logo", MicrosoftOfficeCoreMsoTriStatemsoFalse, _
                              MicrosoftOfficeCoreMsoTriStatemsoCTrue, 50, 32, 68, 58)
                Catch ex As Exception

                End Try
                'worksheet.Range("A2").RowHeight = 50
                Dim bColor As Integer = 2
                worksheet.Range("A1:F1").Borders.Weight = xlthick
                worksheet.Range("A1:F1").Merge()
                worksheet.Range("A2:B2").Merge() : worksheet.Range("D2:F2").Merge()
                worksheet.Range("A3:A6").Merge() : worksheet.Range("A3:A6").VerticalAlignment = XlVAlignxlVAlignTop
                worksheet.Range("B3:B6").Merge() : worksheet.Range("B3:B6").VerticalAlignment = XlVAlignxlVAlignTop
                worksheet.Range("D3:F6").Merge() : worksheet.Range("D3:F6").VerticalAlignment = XlVAlignxlVAlignTop
                worksheet.Range("D11:F11").Merge()
                worksheet.Range("E8:F8").Merge()
                worksheet.Range("E9:F9").Merge()
                worksheet.Range("E10:F10").Merge()
                worksheet.Cells(1, x).value = ComboBox3.Text
                worksheet.Cells(2, 1).value = TextBox13.Text
                worksheet.Range("A2:B2").Borders.Weight = xlthick
                worksheet.Range("D2:F2").Value = TextBox8.Text
                worksheet.Range("D2:F2").Borders.Weight = xlthick
                worksheet.Cells(3, x).value = TextBox1.Text : worksheet.Cells(3, x + 3).value = TextBox5.Text
                worksheet.Range("A3:A6").Borders.Weight = xlthick : worksheet.Cells(3, x + 3).Borders.Weight = xlthick : worksheet.Range("D3:F6").Borders.Weight = xlthick
                worksheet.Range("B3:B6").Borders.Weight = xlthick : worksheet.Range("B3:B6").Borders.Weight = xlthick
                worksheet.Cells(7, x).value = "číslo objednávky" : worksheet.Cells(7, x).Borders.Weight = xlthick : worksheet.Cells(7, x + 1).value = TextBox15.Text : worksheet.Cells(7, x + 1).Borders.Weight = xlthick
                worksheet.Cells(8, x).value = TextBox2.Text : worksheet.Cells(8, x + 1).value = TextBox3.Text : worksheet.Cells(8, x + 3).value = TextBox11.Text : worksheet.Cells(8, x + 4).value = TextBox7.Text
                worksheet.Cells(8, x).Borders.Weight = xlthick : worksheet.Cells(8, x + 1).Borders.Weight = xlthick : worksheet.Cells(8, x + 3).Borders.Weight = xlthick : worksheet.Range("E8:F8").Borders.Weight = xlthick
                worksheet.Cells(9, x).value = TextBox4.Text : worksheet.Cells(9, x + 1).value = TextBox9.Text : worksheet.Cells(9, x + 3).value = TextBox18.Text : worksheet.Cells(9, x + 4).value = ComboBox1.Text
                worksheet.Cells(9, x).Borders.Weight = xlthick : worksheet.Cells(9, x + 1).Borders.Weight = xlthick : worksheet.Cells(9, x + 3).Borders.Weight = xlthick : worksheet.Range("E9:F9").Borders.Weight = xlthick
                worksheet.Cells(10, x).value = TextBox16.Text : worksheet.Cells(10, x + 1).value = NumericUpDown1.Value : worksheet.Cells(10, x + 3).value = TextBox19.Text : worksheet.Cells(10, x + 4).value = ComboBox2.Text
                worksheet.Cells(10, x).Borders.Weight = xlthick : worksheet.Cells(10, x + 1).Borders.Weight = xlthick : worksheet.Cells(10, x + 3).Borders.Weight = xlthick : worksheet.Range("E10:F10").Borders.Weight = xlthick
                worksheet.Cells(11, x).value = TextBox6.Text : worksheet.Cells(11, x + 1).value = TextBox12.Text : worksheet.Cells(11, x + 3).value = Label13.Text
                worksheet.Cells(11, x).Borders.Weight = xlthick : worksheet.Cells(11, x + 1).Borders.Weight = xlthick : worksheet.Range("D11:F11").Borders.Weight = xlthick
                worksheet.Cells(12, x).value = TextBox14.Text : worksheet.Cells(12, x + 1).value = DateTimePicker1.Value : worksheet.Cells(12, x + 3).value = Label14.Text
                worksheet.Cells(12, x).Borders.Weight = xlthick : worksheet.Cells(12, x + 1).Borders.Weight = xlthick : worksheet.Range("D12:F12").Borders.Weight = xlthick
                For Each col As ColumnHeader In ListView1.Columns
                    If col.Index <> 6 Then
                        worksheet.Cells(14, x).value = col.Text
                        worksheet.Cells(14, x).Interior.Color = Color.LightGray
                        worksheet.Cells(14, x).Borders.Weight = xlthick
                        x += 1
                    End If

                Next
                r = 14
                For Each item As ListViewItem In ListView1.Items
                    r += 1 : i += 1
                    ToolStripProgressBar1.Value = (i * 6)
                    For count = 0 To item.SubItems.Count - 2
                        worksheet.Cells(r, count + 1).Value = item.SubItems(count).Text
                        For xx = 0 To ListView1.Columns.Count - 2
                            bColor = item.BackColor.ToArgb
                            Dim myBColor As Color = ColorTranslator.FromHtml(bColor)
                            worksheet.Cells(r, xx + 1).Interior.Color = myBColor
                            worksheet.Cells(r, xx + 1).Borders.Weight = xlthick
                        Next
                    Next
                Next


                r += 2
                worksheet.Cells(r, 1).value = Label9.Text : worksheet.Cells(r, 2).value = Label7.Text : worksheet.Cells(r, 4).value = Label2.Text : worksheet.Cells(r, 6).value = Label10.Text
                r += 1
                worksheet.Cells(r, 1).value = Label4.Text : worksheet.Cells(r, 2).value = NumericUpDown2.Value : worksheet.Cells(r, 4).value = Label8.Text : worksheet.Cells(r, 6).value = Label3.Text
                r += 2
                worksheet.Cells(r, 1).value = Label1.Text : worksheet.Cells(r, 2).value = TextBox77.Text : worksheet.Cells(r, 3).value = NumericUpDown3.Value & " " & daň.Text : worksheet.Cells(r, 5).value = TextBox66.Text : worksheet.Cells(r, 6).value = Label11.Text
                worksheet.Range("A" & r & ":B" & r).Borders.Weight = xlthick
                : worksheet.Range("A" & r & ":B" & r).Interior.Color = Color.LightYellow
                worksheet.Range("C" & r & ":D" & r).Borders.Weight = xlthick
                worksheet.Range("E" & r & ":F" & r).Borders.Weight = xlthick
                : worksheet.Range("E" & r & ":F" & r).Interior.Color = Color.LightYellow
                ToolStripProgressBar1.Value = ToolStripProgressBar1.Maximum

                r += 3
                worksheet.Cells(r, 1).value = Label12.Text
                r += 1
                worksheet.Cells(r, 1).value = Label6.Text : worksheet.Cells(r, 6).value = Label5.Text
                worksheet.Columns("A:Q").EntireColumn.Autofit()
                Dim name As String = IO.Path.GetFullPath(SaveFileDialog1.FileName)
                APP.DisplayAlerts = False
                APP.ActiveWorkbook.SaveAs(Filename:=name)
                workbook.Close()
                APP.Quit()
                releaseobject(APP)
                releaseobject(workbook)
                releaseobject(worksheet)
                Try
                    System.Diagnostics.Process.Start(name)
                Catch ex As Exception

                End Try
                '
            End If
        Catch ex As Exception

        End Try

        ListView1.UseWaitCursor = False

        ToolStripProgressBar1.Visible = False
    End Sub





    Private Sub ComboBox_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles ComboBox1.KeyUp
        If e.KeyCode = Keys.Enter Then ComboBox1.Items.Add(ComboBox1.Text)


    End Sub

    Private Sub ComboBox2_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles ComboBox2.KeyUp
        If e.KeyCode = Keys.Enter Then ComboBox2.Items.Add(ComboBox2.Text)
    End Sub

    Private Sub Form5_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        'Dim formGraphics As Graphics = Me.Panel1.CreateGraphics()


    End Sub

    Private Sub Panel1_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint
        e.Graphics.DrawRectangle(Pens.Black, New Rectangle(200, ListView1.Bottom + 10, 110, 150))
        e.Graphics.DrawRectangle(Pens.Black, New Rectangle(315, ListView1.Bottom + 10, 110, 150))
    End Sub


    Public Sub zmenafontu()
        Dim font As Font = ListView1.Font

        Try


            If IsNumeric(ToolStripComboBox2.Text) = True Then

                If ToolStripComboBox2.Text > 6 Then
                    For Each control As Control In Me.Panel1.Controls

                        Dim size As Single
                        If CSng(control.Font.Size) < 14 Then
                            size = CSng(ToolStripComboBox2.Text)
                        Else
                            size = CSng(control.Font.Size)

                        End If


                        font = New Font(ToolStripComboBox1.Text, size)
                        control.Font = font
                    Next

                End If

            End If
            Dim biggesttext As Integer
            'ColumnHeader1.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
            For Each item As ListViewItem In ListView1.Items
                If biggesttext < item.Text.Length Then
                    biggesttext = item.Text.Length
                End If
            Next

            If ColumnHeader1.Width < biggesttext * ListView1.Font.Size Then ColumnHeader1.Width = biggesttext * ListView1.Font.Size
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Private Sub ToolStripComboBox2_TextChanged(sender As System.Object, e As System.EventArgs) Handles ToolStripComboBox2.TextChanged
        zmenafontu()
    End Sub
    Private Sub ToolStripComboBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles ToolStripComboBox1.TextChanged
        zmenafontu()
    End Sub




    Private Sub Form5_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop

    End Sub








































































































End Class