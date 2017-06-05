Imports System.Text

Public Class Form4





    Private Sub Form4_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ListView1.Items.Add(My.Forms.ryder.Text).BackColor = Color.Gray
        For xx = 0 To My.Forms.ryder.ComboBox2.Items.Count - 1
            My.Forms.ryder.ComboBox2.SelectedIndex = xx
            ListView1.Items.Add(My.Forms.ryder.ComboBox2.Text)
            ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & "." & My.Forms.ryder.Panel1.Name + "." + "combobox2")
        Next

        For Each Control As Control In My.Forms.ryder.Panel1.Controls
            If Control.Name <> Control.Text And Not Control.Name.Contains("tBox") And Not Control.Name.Contains("oBox") And Not Control.Name.Contains("Numeric") And Control.Name <> " " Then
                ListView1.Items.Add(Control.Text)
                ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & "." & My.Forms.ryder.Panel1.Name + "." + Control.Name)
            End If
        Next
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader1.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader1")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader2.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader2")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader3.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader3")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader4.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader4")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader5.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader5")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader6.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader6")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader18.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader18")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader19.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader19")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader20.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader20")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader21.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader21")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader22.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader22")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader23.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader23")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader24.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader24")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader25.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader25")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader26.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader26")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader29.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.ryder.Name & ".ColumnHeader29")

        ListView1.Items.Add(My.Forms.mail.Name).BackColor = Color.Gray
        For Each Control As Control In My.Forms.mail.Controls
            If Control.Name <> Control.Text And Not Control.Name.Contains("Box") And Control.Name <> Nothing Then
                ListView1.Items.Add(Control.Text)
                ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.mail.Text & "." & Control.Name)
            End If
        Next
        ListView1.Items.Add(Me.Text).BackColor = Color.Gray
        For Each Control As Control In Me.Controls
            If Control.Name <> Control.Text And Not Control.Name.Contains("Box") And Control.Name <> Nothing Then

                ListView1.Items.Add(Control.Text)
                ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(Me.Name & "." & Control.Name)
            End If
        Next


        ListView1.Items.Add(Me.ColumnHeader1.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(Me.Name & ".ColumnHeader1")
        ListView1.Items.Add(Me.ColumnHeader2.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(Me.Name & ".ColumnHeader2")
        ListView1.Items.Add(My.Forms.Dialog1.Name).BackColor = Color.Gray
        For Each Control As Control In My.Forms.Dialog1.TableLayoutPanel1.Controls
            If Control.Name <> Control.Text And Not Control.Name.Contains("Box") And Control.Name <> Nothing Then
                ListView1.Items.Add(Control.Text)
                ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.Dialog1.Name & "." & Control.Name)
            End If
        Next

        ListView1.Items.Add(My.Forms.Dialog2.Name).BackColor = Color.Gray
        For Each Control As Control In My.Forms.Dialog2.Controls

            If Control.Name <> Control.Text And Not Control.Name.Contains("Box") And Control.Name <> Nothing Then
                ListView1.Items.Add(Control.Text)
                ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.Dialog2.Name & "." & Control.Name)
            End If
        Next

        ListView1.Items.Add(My.Forms.Dialog3.Text).BackColor = Color.Gray
        For Each Control As Control In My.Forms.Dialog3.Controls

            If Control.Name <> Control.Text And Not Control.Name.Contains("Box") And Control.Name <> Nothing Then
                ListView1.Items.Add(Control.Text)
                ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.Dialog3.Name & "." & Control.Name)
            End If
        Next

        ListView1.Items.Add(My.Forms.Form5.Text).BackColor = Color.Gray
        For Each Control As Control In My.Forms.Form5.Panel1.Controls

            If Control.Name <> Control.Text And Not Control.Name.Contains("Box") And Control.Name <> Nothing Then
                ListView1.Items.Add(Control.Text)
                ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.Form5.Name & "." & My.Forms.Form5.Panel1.Name & "." & Control.Name)
            End If
        Next
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader1.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.Form5.Name & ".ColumnHeader1")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader2.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.Form5.Name & ".ColumnHeader2")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader3.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.Form5.Name & ".ColumnHeader3")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader4.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.Form5.Name & ".ColumnHeader4")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader5.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.Form5.Name & ".ColumnHeader5")
        ListView1.Items.Add(My.Forms.ryder.ColumnHeader6.Text)
        ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(My.Forms.Form5.Name & ".ColumnHeader6")

    End Sub






    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs)

    End Sub





    Private Sub NewToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles NewToolStripButton.Click
        ListView1.Items.Clear()
    End Sub

    Private Sub SaveToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripButton.Click

        My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\" & ToolStripComboBox1.Text & ".lng", String.Empty, False)


        For i As Integer = 0 To ListView1.Items.Count - 1
            My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\" & ToolStripComboBox1.Text & ".lng", ListView1.Items(i).Text, True)


            For subItemIndex As Integer = 1 To ListView1.Items(i).SubItems.Count - 1
                My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\" & ToolStripComboBox1.Text & ".lng", vbTab & ListView1.Items(i).SubItems(subItemIndex).Text, True)
            Next
            My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\" & ToolStripComboBox1.Text & ".lng", Environment.NewLine, True)




        Next



    End Sub

    Private Sub OpenToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles OpenToolStripButton.Click
        Try
            ListView1.Items.Clear()

            Dim lang As String = Application.StartupPath & "\" & ToolStripComboBox1.Text & ".lng"
            Dim fileExists As Boolean
            fileExists = My.Computer.FileSystem.FileExists(lang)
            If fileExists = False Then
                If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then lang = OpenFileDialog1.FileName
            End If
            Dim testFile As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(lang)
            Dim nahraj As String = My.Computer.FileSystem.ReadAllText(lang)
            Dim jazyk() As String = Split(testFile.Name, ".")
            ToolStripComboBox1.Items.Add(jazyk(0))
            ToolStripComboBox1.SelectedItem = jazyk(0)

            Dim sNames() As String : Dim x As Integer : sNames = Split(nahraj, vbCrLf)
            For x = 0 To UBound(sNames)
                Dim colums() As String = Split(sNames(x), vbTab)
                ListView1.Items.Add(colums(0))
                Try
                    ListView1.Items.Item(ListView1.Items.Count - 1).SubItems.Add(colums(1))
                Catch ex As Exception

                End Try

            Next


        Catch ex As Exception

        End Try
    End Sub

    Private Sub CutToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles CutToolStripButton.Click
        Dim list As New StringBuilder
        For Each item As ListViewItem In ListView1.Items
            list.AppendLine(item.Text)
            item.Text = ""
        Next
        My.Computer.Clipboard.SetText(list.ToString)
    End Sub

    Private Sub CopyToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles CopyToolStripButton.Click
        Dim list As New StringBuilder
        For Each item As ListViewItem In ListView1.Items
            list.AppendLine(item.Text)
        Next
        My.Computer.Clipboard.SetText(list.ToString)
    End Sub


    Private Sub PasteToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles PasteToolStripButton.Click
        Dim text As String = Nothing
        Dim i As Integer = 0
        If My.Computer.Clipboard.ContainsText Then
            text = My.Computer.Clipboard.GetText
        End If
        Dim list() As String = Split(text, vbCrLf)
        For Each line As String In list
            If i < ListView1.Items.Count - 1 Then
                ListView1.Items(i).Text = Trim(line.Replace(vbCrLf, ""))
                i += 1
            End If
        Next

    End Sub

    Private Sub PrintToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles PrintToolStripButton.Click
        PrintDialog1.AllowSomePages = True
        PrintDialog1.ShowHelp = True
        PrintDialog1.AllowSelection = True
        PrintDialog1.AllowCurrentPage = True
        PrintDialog1.UseEXDialog = True
        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        PrintDocument1.PrinterSettings.DefaultPageSettings.Landscape = True
        If ListView1.Visible = True Then PrintDocument1.PrinterSettings.DefaultPageSettings.Landscape = False
        PrintDocument1.DefaultPageSettings.Margins.Left = 10

        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PrintDocument1.DefaultPageSettings.Margins.Left = 10
            PrintDocument1.DefaultPageSettings.Margins.Top = 10
            PrintDocument1.DefaultPageSettings.Margins.Bottom = 15
            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

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
                    Dim myBColor As Color = ColorTranslator.FromHtml(CStr(bColor))
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