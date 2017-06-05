Public Class Form11
    Dim index As Integer
    Dim LastIndex As Integer
    Dim CurrentPage As Integer
    Dim listindex As Integer
    Dim popiska As String

    Private Sub PrintToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles PrintToolStripButton.Click
        Dim k1 = ryder.ComboBox5.Text
        Dim k2 = ryder.ComboBox8.Text
        If ryder.CheckBox1.Checked Then
            popiska = ryder.ComboBox5.Text + "." + ryder.ComboBox10.Text + "." + "-" + ryder.ComboBox8.Text + "." + ryder.ComboBox10.Text + ".  " + ryder.ComboBox7.Text
            nadpis = My.Settings.isotext & vbCrLf & ryder.CheckBox1.Text
        ElseIf ryder.CheckBox6.Checked Then
            popiska = k1 & "." & "-" & k2 & ".  " & ryder.ComboBox7.Text
            nadpis = My.Settings.isotext & vbCrLf & ryder.CheckBox6.Text
        ElseIf ryder.CheckBox2.Checked Then
            popiska = k1 & "." & "-" & k2 & ".  " & ryder.ComboBox7.Text
            nadpis = My.Settings.isotext & vbCrLf & ryder.CheckBox2.Text
        ElseIf ryder.CheckBox3.Checked Then
            popiska = k1 & "." & "-" & k2 & ".  "
            nadpis = My.Settings.isotext & vbCrLf & ryder.CheckBox3.Text
        Else
            nadpis = My.Settings.isotext & vbCrLf & ryder.CheckBox1.Text
        End If
        PrintDialog1.UseEXDialog = True
        index = 0
        listindex = 0
        LastIndex = 0
        CurrentPage = 0
        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
            PrintDocument1.DefaultPageSettings.Margins.Left = 20
            PrintDocument1.DefaultPageSettings.Margins.Right = 20
            PrintDocument1.DefaultPageSettings.Margins.Top = 45
            PrintDocument1.DefaultPageSettings.Margins.Bottom = 20

            PrintPreviewDialog1.Document = PrintDocument1
            PrintPreviewDialog1.ShowDialog()
            Dim stred As Integer = (PrintDocument1.DefaultPageSettings.Margins.Top) / 2
            Me.PictureBox1.Width = Me.PictureBox1.Width * ((stred * 2 - 5) / Me.PictureBox1.Height)
            Me.PictureBox1.Height = stred * 2 - 10 '
        End If


    End Sub
    Dim nadpis As String
    Private Sub PrintDocument1_PrintPage(sender As System.Object, e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim fontpage As Font = TabControl1.Font
        Dim lvy As Integer = 0
        'Dim fontlv As Font = TabControl1.Font
        Dim PageNumberWidth As Single
        Dim stredh As Integer
        Dim stred As Integer = (e.MarginBounds.Top - e.PageBounds.Top) / 2
        e.Graphics.DrawRectangle(Pens.Black, e.MarginBounds.X, e.MarginBounds.Y, e.MarginBounds.Width, e.MarginBounds.Height)

        For tb = index To TabControl1.TabPages.Count - 1
            If tb = 1 Then
                e.PageSettings.Landscape = True
            Else
                e.PageSettings.Landscape = False
            End If
            If nadpis <> My.Settings.isotext & vbCrLf & TabControl1.TabPages(tb).Text _
                And TabControl1.TabPages(tb).Controls.Count > 1 Then
                index = tb
                CurrentPage += 1
                e.HasMorePages = True
                nadpis = My.Settings.isotext & vbCrLf & TabControl1.TabPages(tb).Text

                e.Graphics.DrawString(CStr(CurrentPage), fontpage, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, _
                                                      e.PageBounds.Bottom - (e.PageBounds.Bottom - e.MarginBounds.Bottom) / 2 - fontpage.Height / 2)
                Exit Sub
            End If
            Try
                e.Graphics.DrawImage(Me.PictureBox1.Image, New Rectangle(e.MarginBounds.Left, e.PageBounds.Top + 5, Me.PictureBox1.Width, Me.PictureBox1.Height))
            Catch ex As Exception
            End Try
            Try

                If ryder.CheckBox1.Checked Then
                    nadpis = My.Settings.isotext & vbCrLf & ryder.CheckBox1.Text
                    e.Graphics.DrawString(My.Settings.regday, TabControl1.Font, Brushes.DimGray, e.MarginBounds.Width - e.Graphics.MeasureString(CStr(My.Settings.regday), TabControl1.Font).Width, stred)
                ElseIf ryder.CheckBox6.Checked Then
                    nadpis = My.Settings.isotext & vbCrLf & ryder.CheckBox6.Text
                    e.Graphics.DrawString(My.Settings.regweek, TabControl1.Font, Brushes.DimGray, e.MarginBounds.Width - e.Graphics.MeasureString(CStr(My.Settings.regday), TabControl1.Font).Width, stred)
                ElseIf ryder.CheckBox2.Checked Then
                    nadpis = My.Settings.isotext & vbCrLf & ryder.CheckBox2.Text
                    e.Graphics.DrawString(My.Settings.regmonth, TabControl1.Font, Brushes.DimGray, e.MarginBounds.Width - e.Graphics.MeasureString(CStr(My.Settings.regday), TabControl1.Font).Width, stred)
                ElseIf ryder.CheckBox3.Checked Then
                    nadpis = My.Settings.isotext & vbCrLf & ryder.CheckBox3.Text
                    e.Graphics.DrawString(My.Settings.regyear, TabControl1.Font, Brushes.DimGray, e.MarginBounds.Width - e.Graphics.MeasureString(CStr(My.Settings.regday), TabControl1.Font).Width, stred)
                End If
                If nadpis <> String.Empty Then
                    stredh = (e.MarginBounds.Width - e.Graphics.MeasureString(CStr(nadpis & popiska), TabControl1.Font).Width) / 2
                    e.Graphics.DrawString(nadpis & popiska, TabControl1.Font, Brushes.DimGray, stredh, stred / 4)
                End If

            Catch ex As Exception

            End Try
            For f = listindex To TabControl1.TabPages(tb).Controls.Count - 1

                If TypeOf TabControl1.TabPages(tb).Controls(f) Is ListView Then


                    Dim listview1 As ListView = TabControl1.TabPages(tb).Controls(f)
                    Dim corectonx As Double = 1

                    Dim DpiGraphics As Graphics = Me.CreateGraphics
                    Dim DpiX As Integer = DpiGraphics.DpiX
                    Dim DpiY As Integer = DpiGraphics.DpiY
                    DpiGraphics.Dispose()
                    Dim X, Y As Integer
                    X = 0

                    Dim TextRect As Rectangle = Rectangle.Empty
                    Dim TextLeftPad As Single = CSng(1 * (DpiX / 96)) '4 pixel pad on the left.
                    Dim ColumnHeaderHeight As Single = CSng(listview1.Font.Height + (10 * (DpiX / 96))) '5 pixel pad on the top an bottom
                    Dim StringFormat As New StringFormat
                    PageNumberWidth = e.Graphics.MeasureString(CStr(CurrentPage), listview1.Font).Width
                    Font = listview1.Font
                    StringFormat.FormatFlags = StringFormatFlags.LineLimit
                    StringFormat.Trimming = StringTrimming.EllipsisCharacter
                    StringFormat.LineAlignment = StringAlignment.Center

                    X = CInt(e.MarginBounds.X)
                    Y = CInt(e.MarginBounds.Y)
                    If listview1.Columns(1).Width * listview1.Columns.Count - 1 > e.MarginBounds.Width Then
                        If e.PageSettings.Landscape = True Then
                            corectonx = ((e.MarginBounds.Width - listview1.Columns(0).Width) / listview1.Columns.Count) / listview1.Columns(1).Width
                        ElseIf e.PageSettings.Landscape = False Then


                            corectonx = ((e.MarginBounds.Width - listview1.Columns(0).Width) / listview1.Columns.Count + 1) / listview1.Columns(1).Width

                        End If

                    End If
                    For ColumnIndex As Integer = 0 To listview1.Columns.Count - 1
                        TextRect.X = X
                        TextRect.Y = Y + lvy
                        Dim fonts As Integer = 8
                        TextRect.Width = listview1.Columns(ColumnIndex).Width
                        If ColumnIndex > 0 Then TextRect.Width = TextRect.Width * corectonx : fonts = 6

                        TextRect.Height = ColumnHeaderHeight
                        e.Graphics.FillRectangle(Brushes.LightGray, TextRect)
                        e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
                        TextRect.X += TextLeftPad
                        TextRect.Width -= TextLeftPad
                        e.Graphics.DrawString(listview1.Columns(ColumnIndex).Text, New Font("arial", fonts, FontStyle.Italic), Brushes.Black, TextRect, StringFormat)
                        X += TextRect.Width + TextLeftPad
                    Next
                    Y += ColumnHeaderHeight
                    For i = LastIndex To listview1.Items.Count - 1
                        With listview1.Items(i)

                            X = CInt(e.MarginBounds.X)
                            If Y + lvy + .Bounds.Height * 2 > e.MarginBounds.Bottom Then
                                listindex = f
                                index = tb
                                LastIndex = i
                                CurrentPage += 1
                                e.HasMorePages = True
                                StringFormat.Dispose()

                                e.Graphics.DrawString(CStr(CurrentPage), fontpage, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, _
                                                      e.PageBounds.Bottom - (e.PageBounds.Bottom - e.MarginBounds.Bottom) / 2 - fontpage.Height / 2)
                                Exit Sub
                            End If

                            For ColumnIndex As Integer = 0 To listview1.Columns.Count - 1
                                TextRect.X = X
                                TextRect.Y = Y + lvy
                                TextRect.Width = listview1.Columns(ColumnIndex).Width
                                If ColumnIndex > 0 Then TextRect.Width = TextRect.Width * corectonx
                                TextRect.Height = .Bounds.Height
                                Dim bColor As Integer = listview1.Items(i).BackColor.ToArgb
                                Dim myBColor As Color = ColorTranslator.FromHtml(bColor)
                                e.Graphics.FillRectangle(New SolidBrush(myBColor), TextRect)
                                If listview1.GridLines Then
                                    e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
                                End If
                                TextRect.X += TextLeftPad

                                TextRect.Width -= TextLeftPad
                                TextRect.Height -= TextLeftPad

                                If ColumnIndex < .SubItems.Count Then
                                    bColor = listview1.Items(i).SubItems(ColumnIndex).BackColor.ToArgb
                                    myBColor = ColorTranslator.FromHtml(bColor)
                                    If myBColor = Color.White Or myBColor = Color.GhostWhite Then myBColor = Color.Transparent
                                    e.Graphics.FillRectangle(New SolidBrush(myBColor), TextRect)
                                    Dim TColor As Integer = listview1.Items(i).SubItems(ColumnIndex).ForeColor.ToArgb
                                    Dim myTColor As Color = ColorTranslator.FromHtml(TColor)
                                    If myTColor = Color.White Or myTColor = Color.GhostWhite Or myTColor = Color.Transparent Then myTColor = Color.Black
                                    e.Graphics.DrawString(.SubItems(ColumnIndex).Text, .SubItems(ColumnIndex).Font, New SolidBrush(myTColor), TextRect, StringFormat)

                                End If

                                X += TextRect.Width + TextLeftPad
                            Next

                            Y += .Bounds.Height
                        End With

                    Next
                    listindex = 0
                    LastIndex = 0

                    StringFormat.Dispose()

                    lvy += Y - e.MarginBounds.Top
                End If

            Next
        Next
        CurrentPage += 1
        e.Graphics.DrawString(CStr(CurrentPage), fontpage, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, _
                                          e.PageBounds.Bottom - (e.PageBounds.Bottom - e.MarginBounds.Bottom) / 2 - fontpage.Height / 2)
    End Sub

    Private Sub ToolStripButton1_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton1.Click
        Try


            For Each lv As Control In TabControl1.SelectedTab.Controls
                If lv.Focused = True Then
                    TabControl1.SelectedTab.Controls.Remove(lv)
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Form11Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Try

            Dim fileExists As Boolean
            fileExists = My.Computer.FileSystem.FileExists(Application.StartupPath & "\logo")
            If fileExists = True Then

                Me.PictureBox1.Load(Application.StartupPath & "\logo")
            End If
        Catch ex As Exception

        End Try
        For Each TAB As TabPage In TabControl1.TabPages
            If TAB.Text <> Nothing Then
                TabControl1.SelectedTab = TAB
                Exit For
            End If
        Next

    End Sub


    'Private Sub TabPage1_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles TabPage1.MouseClick
    '    If My.Settings.Setreportsize = False Then
    '        If e.Button = Windows.Forms.MouseButtons.Left Then
    '            Dim mx As Integer = MousePosition.X - Me.Location.X
    '            Dim mi As Integer = MousePosition.Y - Me.Location.Y + TabControl1.SelectedTab.Location.Y + 2
    '            For Each ctl As Control In TabControl1.SelectedTab.Controls
    '                ' If (ctl.Location.X - 10) < mx And mx < (ctl.Location.X + ctl.Width + 10) And _
    '                '(ctl.Location.Y - 10) < mi And mi < (ctl.Location.Y + ctl.Size.Height + 10) And TypeOf ctl Is ListView Then
    '                If ctl.Width < 201 And TypeOf ctl Is ListView Then
    '                    Dim lv As ListView = ctl
    '                    If lv.Focused = True Then
    '                        'MsgBox(ctl.Location.X & vbTab & mx & vbTab & (ctl.Location.X + ctl.Width) & vbCrLf _
    '                        ' & ctl.Location.Y & vbTab & mi & vbTab & (ctl.Location.Y + ctl.Height))
    '                        lv.Size = New Size((lv.TopItem.SubItems.Count + 1) * lv.Columns(2).Width + lv.Columns(0).Width, (lv.Items.Count) * lv.TopItem.Bounds.Height + lv.Font.Height * 2)
    '                    End If
    '                Else
    '                    Timer1.Enabled = True
    '                    Timer1.Start()
    '                    SuspendLayout()
    '                    ctl.Size = (New Size(200, 100))
    '                End If

    '            Next
    '        End If
    '    End If
    'End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        ResumeLayout()
    End Sub

   End Class