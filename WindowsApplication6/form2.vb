'Imports Excel = Microsoft.Office.Interop.Excel
Imports Rydereng.ryder
Imports System.Text

Public Class Form2
    Private currentpage As Integer
    Dim index As Integer


    Private Sub PrintToolStripButton_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripButton.Click
        '
        PrintDialog1.AllowSomePages = True
        PrintDialog1.ShowHelp = True
        PrintDialog1.AllowSelection = True
        PrintDialog1.AllowCurrentPage = True
        PrintDialog1.UseEXDialog = True
        Dim result As DialogResult = PrintDialog1.ShowDialog()


        If (result = DialogResult.OK) Then

            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
            If TabControl1.Width > 900 Then PrintDocument1.DefaultPageSettings.Landscape = True
            If TabControl1.Width < 900 Then PrintDocument1.DefaultPageSettings.Landscape = False
            PrintDocument1.DefaultPageSettings.Margins.Left = 70
            PrintDocument1.DefaultPageSettings.Margins.Top = 20
            PrintDocument1.DefaultPageSettings.Margins.Bottom = 160
           
            PrintPreviewDialog1.Document = PrintDocument1
            PrintPreviewDialog1.ShowDialog()
        End If


    End Sub


    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Try
            Dim stred As Integer = (e.MarginBounds.Top - e.PageBounds.Top) / 2

            'MsgBox("pageb " & e.PageBounds.Bottom & vbCrLf & "ebottom " & e.MarginBounds.Bottom)
            Dim reg As Boolean
            Dim regnumber As String = Nothing
            Dim X, Y As Integer
            Dim TextRect As Rectangle = Rectangle.Empty
            Dim StringFormat As New StringFormat
            Dim PageNumberWidth As Single = e.Graphics.MeasureString(CStr(currentpage), TabControl1.Font).Width
            StringFormat.FormatFlags = StringFormatFlags.NoWrap
            StringFormat.Trimming = StringTrimming.Character
            StringFormat.LineAlignment = StringAlignment.Near

            X = CInt(e.MarginBounds.X)
            Y = CInt(e.MarginBounds.Y)
            Dim nadpis As String = ryder.Button14.Text
            Try



                e.Graphics.DrawImage(Me.PictureBox1.Image, New Rectangle(e.MarginBounds.Left, e.PageBounds.Top, Me.PictureBox1.Width, Me.PictureBox1.Height))

            Catch ex As Exception

            End Try
            Try



                If ryder.CheckBox1.Checked Then
                    nadpis = My.Settings.isotext & vbTab & ryder.CheckBox1.Text
                    e.Graphics.DrawString(My.Settings.regday, TabControl1.Font, Brushes.DimGray, e.MarginBounds.Width - e.Graphics.MeasureString(CStr(My.Settings.regday), TabControl1.Font).Width, stred)
                ElseIf ryder.CheckBox6.Checked Then
                    nadpis = My.Settings.isotext & vbTab & ryder.CheckBox6.Text
                    e.Graphics.DrawString(My.Settings.regweek, TabControl1.Font, Brushes.DimGray, e.MarginBounds.Width - e.Graphics.MeasureString(CStr(My.Settings.regday), TabControl1.Font).Width, stred)
                ElseIf ryder.CheckBox2.Checked Then
                    nadpis = My.Settings.isotext & vbTab & ryder.CheckBox2.Text
                    e.Graphics.DrawString(My.Settings.regmonth, TabControl1.Font, Brushes.DimGray, e.MarginBounds.Width - e.Graphics.MeasureString(CStr(My.Settings.regday), TabControl1.Font).Width, stred)
                ElseIf ryder.CheckBox3.Checked Then
                    nadpis = My.Settings.isotext & vbTab & ryder.CheckBox3.Text
                    e.Graphics.DrawString(My.Settings.regyear, TabControl1.Font, Brushes.DimGray, e.MarginBounds.Width - e.Graphics.MeasureString(CStr(My.Settings.regday), TabControl1.Font).Width, stred)
                End If
            Catch ex As Exception

            End Try
            'Keep track of the Line Y postition to determine if there are more pages
            Dim currentyposition As Integer
            For i = index To TabControl1.TabPages.Count - 1

                If nadpis <> String.Empty Then

                    Dim stredh As Integer = (e.MarginBounds.Width - e.Graphics.MeasureString(CStr(nadpis), TabControl1.Font).Width) / 2
                    e.Graphics.DrawString(nadpis, TabControl1.Font, Brushes.DimGray, stredh, stred)

                End If
                Dim biggery As Integer = 0
                Dim biggestlocotiany As Integer = 0
                If TabControl1.TabPages(i).Controls.Count > 10 Then
                    For Each Control As Control In TabControl1.TabPages(i).Controls
                        Dim yyy As Integer = Control.Location.Y
                        If yyy > biggestlocotiany And yyy < e.MarginBounds.Bottom Then
                            biggestlocotiany = yyy
                        End If

                    Next

                    If currentyposition + biggestlocotiany > e.MarginBounds.Bottom Then

                        'the next page won't fit.
                        e.HasMorePages = True
                        'biggestx = e.MarginBounds.Left

                        index = i
                        'index2 = ColumnIndex + 1
                        currentpage += 1
                        e.Graphics.DrawString(CStr(currentpage), TabControl1.TabPages(i).Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - TabControl1.TabPages(i).Font.Height * 3)
                        biggery = 0
                        currentyposition = e.MarginBounds.Top
                        reg = False
                        Exit Sub
                    Else
                        e.HasMorePages = False

                    End If
                    For ColumnIndex = 0 To TabControl1.TabPages(i).Controls.Count - 1

                        TextRect.X = TabControl1.TabPages(i).Controls(ColumnIndex).Location.X + X


                        TextRect.Y = TabControl1.TabPages(i).Controls(ColumnIndex).Location.Y + currentyposition

                        TextRect.Width = TabControl1.TabPages(i).Controls(ColumnIndex).Width
                        TextRect.Height = TabControl1.TabPages(i).Controls(ColumnIndex).Height

                        Dim TColor As Integer = TabControl1.TabPages(i).Controls(ColumnIndex).ForeColor.ToArgb
                        Dim myTColor As Color = ColorTranslator.FromHtml(TColor)

                        If TabControl1.TabPages(i).Controls(ColumnIndex).ToString.Contains("TextBox") Then
                            Dim bColor As Integer = TabControl1.TabPages(i).Controls(ColumnIndex).BackColor.ToArgb
                            Dim myBColor As Color = ColorTranslator.FromHtml(bColor)

                            e.Graphics.FillRectangle(New SolidBrush(myBColor), TextRect)
                            e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)

                        End If



                        e.Graphics.DrawString(TabControl1.TabPages(i).Controls(ColumnIndex).Text, TabControl1.TabPages(i).Controls(ColumnIndex).Font, New SolidBrush(myTColor), TextRect.X, TextRect.Y, StringFormat)




                        If biggery < TextRect.Y Then biggery = TextRect.Y

                        'toto zariadi pokračovanie draw na stránke
                        If ColumnIndex = TabControl1.TabPages(i).Controls.Count - 1 Then currentyposition = biggery


                        If currentyposition >= e.MarginBounds.Bottom Then
                            'MsgBox("true" & currentyposition)
                            'the next page won't fit.
                            e.HasMorePages = True
                            'biggestx = e.MarginBounds.Left

                            index = i + 1

                            currentpage += 1
                            e.Graphics.DrawString(CStr(currentpage), TabControl1.TabPages(i).Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - TabControl1.TabPages(i).Font.Height * 3)
                            biggery = 0
                            currentyposition = e.MarginBounds.Top
                            reg = False
                            Exit Sub
                        Else
                            e.HasMorePages = False

                        End If

                    Next

                    StringFormat.Dispose()

                End If

            Next
            currentpage += 1
            e.Graphics.DrawString(CStr(currentpage), TabControl1.TabPages(TabControl1.TabPages.Count - 1).Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - TabControl1.TabPages(TabControl1.TabPages.Count - 1).Font.Height * 3)
        Catch ex As Exception

        End Try
        currentpage = 0
        index = 0
    End Sub

    'Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
    '    'keep a static index to reference if the process has more pages.

    '    'keep a static index to reference if the process has more pages.

    '    Try
    '        Static index As Integer
    '        'Keep track of the Line Y postition to determine if there are more pages
    '        Dim CurrentYPosition As Integer = e.MarginBounds.Top
    '        Dim mheight As Integer
    '        For i = index To TabControl1.TabPages.Count - 1
    '            ToolStripComboBox1.SelectedIndex = i
    '            'Create a bitmap to the size of the current tab page and draw the
    '            'current tab page to it.
    '            Me.TabControl1.TabPages(i).Show()
    '            If TabControl1.TabPages(i).Controls.Count > 10 Then
    '                If TabControl1.TabPages(i).Width > 900 Then PrintDocument1.DefaultPageSettings.Landscape = True
    '                If TabControl1.TabPages(i).Width < 900 Then PrintDocument1.DefaultPageSettings.Landscape = False
    '                mheight = ((ToolStripComboBox1.Text + 3) * 23)
    '                Dim TabBitmap As New Bitmap(TabControl1.TabPages(i).Width, mheight)
    '                Me.TabControl1.TabPages(i).DrawToBitmap(TabBitmap, New Rectangle(Point.Empty, TabBitmap.Size))
    '                'Draw the tab image at the margin left and the current y position
    '                e.Graphics.DrawImage(TabBitmap, New Point(e.MarginBounds.X - 60, CurrentYPosition))
    '                'Add the current tab page height to the line position
    '                CurrentYPosition += mheight
    '                'Shit can the tab image.
    '                TabBitmap.Dispose()
    '                TabBitmap = Nothing
    '                'If the next tab page will not fit on the current page then there
    '            End If
    '            If CurrentYPosition > e.MarginBounds.Bottom - mheight Then
    '                'the next page won't fit.
    '                index = i + 1
    '                CurrentYPosition = e.MarginBounds.Top
    '                e.HasMorePages = True

    '                Exit Sub
    '            End If
    '        Next
    '        e.HasMorePages = False
    '        'All done reset the static index.
    '        index = 0
    '    Catch ex As Exception

    '    End Try
    'End Sub


    Private Sub NewToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripButton.Click
        Me.TabControl1.TabPages.Clear()
    End Sub





    Private Sub ToolStripButton1_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton1.Click
        Me.TabControl1.TabPages.Remove(Me.TabControl1.SelectedTab)
    End Sub
    Private Sub Form2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Update()
        Try

            Dim fileExists As Boolean
            fileExists = My.Computer.FileSystem.FileExists(Application.StartupPath & "\logo")
            If fileExists = True Then

                Me.PictureBox1.Load(Application.StartupPath & "\logo")
            End If
        Catch ex As Exception

        End Try
    End Sub
    

   
End Class