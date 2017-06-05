Imports System.Windows.Forms
Imports System.Text
Imports System.Drawing.Text
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports Microsoft.Win32.SafeHandles



Module CustomFont

    'PRIVATE FONT COLLECTION TO HOLD THE DYNAMIC FONT

    Private _pfc As PrivateFontCollection = Nothing

    Public ReadOnly Property GetInstance(ByVal Size As Single, ByVal style As FontStyle) As Font

        Get

            'IF THIS IS THE FIRST TIME GETTING AN INSTANCE

            'LOAD THE FONT FROM RESOURCES

            If _pfc Is Nothing Then LoadFont()

            'RETURN A NEW FONT OBJECT BASED ON THE SIZE AND STYLE PASSED IN

            Return New Font(_pfc.Families(0), Size, style)

        End Get

    End Property

    Private Sub LoadFont()

        Try

            'INIT THE FONT COLLECTION

            _pfc = New PrivateFontCollection

            'LOAD MEMORY POINTER FOR FONT RESOURCE

            Dim fontMemPointer As IntPtr = Marshal.AllocCoTaskMem(My.Resources.FRE3OF9X.Length)

            'COPY THE DATA TO THE MEMORY LOCATION

            Marshal.Copy(My.Resources.FRE3OF9X, 0, fontMemPointer, My.Resources.FRE3OF9X.Length)

            'LOAD THE MEMORY FONT INTO THE PRIVATE FONT COLLECTION

            _pfc.AddMemoryFont(fontMemPointer, My.Resources.FRE3OF9X.Length)

            'FREE UNSAFE MEMORY
            Marshal.FreeCoTaskMem(fontMemPointer)
        Catch ex As Exception
            MsgBox(" Font must  be installed manually" & ex.ToString)
            'ERROR LOADING FONT. HANDLE EXCEPTION HERE
        End Try
    End Sub

End Module

Public Class Dialog8
    Dim alreadygenerated As Boolean
    Dim list As New List(Of String)
    Dim ItemCount As Integer
    Dim focu As Integer
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        My.Settings.horizontal = NumericUpDown3.Value
        My.Settings.vertical = NumericUpDown4.Value
        My.Settings.leftmargin = NumericUpDown5.Value
        My.Settings.topmargin = NumericUpDown6.Value
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub barcode()
        Try


            PictureBox1.Refresh()
            Dim drawFormat As New StringFormat()
            Dim txt As String = RemoveDiacritics(TextBox2.Text.ToUpperInvariant)
            Dim codestr As String = ("*" & NumericUpDown2.Value & "/" & txt & "*")
            Dim strtxt As String = NumericUpDown2.Value & "/" & txt
            Using formGraphics As Graphics = Me.PictureBox1.CreateGraphics(), _
                drawFont As New System.Drawing.Font(CustomFont.GetInstance(NumericUpDown1.Value, 0), FontStyle.Regular), _
                drawBrush As New SolidBrush(Color.Black)
                Dim arialFont As New Font("arial", NumericUpDown1.Value / 4)
                formGraphics.DrawString(codestr, drawFont, drawBrush, _
                    1, PictureBox1.Location.Y, drawFormat)
                formGraphics.DrawString(strtxt, arialFont, drawBrush, _
                    strtxt.Length / 2, PictureBox1.Location.Y + drawFont.Height)
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        barcode()
    End Sub
    Private Sub Dialog8_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        NumericUpDown3.Value = My.Settings.horizontal
        NumericUpDown4.Value = My.Settings.vertical
        NumericUpDown5.Value = My.Settings.leftmargin
        NumericUpDown6.Value = My.Settings.topmargin
        list.Clear()

        TextBox2.Text = RemoveDiacritics(TextBox2.Text)
        NumericUpDown1.Value = 20
        barcodesaved()
        Try
            If alreadygenerated = False Then

                Dim x As Integer = list.Min
                Dim cislo As Integer = 1

                'Array.Sort(kody)
                list.Sort()
                For z = 0 To list.Count - 1
                    If IsNumeric(list(z)) Then
                        cislo = list(z)
                        If cislo <> x Then
                            NumericUpDown2.Value = x
                            Exit For
                        Else
                            x += 1
                        End If
                    End If
                Next
            End If
            barcode()
            ColumnHeader1.Text = Label1.Text
            ColumnHeader2.Text = Label2.Text
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click

        PrintDialog1.AllowSelection = True
        If PrintDialog1.ShowDialog = DialogResult.OK Then
            PrintDocument1.DefaultPageSettings.Margins.Left = My.Settings.leftmargin
            PrintDocument1.DefaultPageSettings.Margins.Top = My.Settings.topmargin
            PrintDocument1.DefaultPageSettings.Margins.Bottom = 20
            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
            PrintPreviewDialog1.Document = PrintDocument1
            PrintDocument1.Print()
        End If
    End Sub
    Private Sub pdoc_PrintPage1(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim drawFont As New System.Drawing.Font(CustomFont.GetInstance(NumericUpDown1.Value, 0), FontStyle.Regular)
        Dim txt As String = RemoveDiacritics(TextBox2.Text.ToUpperInvariant)
        Dim arialFont As New Font("arial", NumericUpDown1.Value / 4)
        Dim codestr As String = ("*" & NumericUpDown2.Value & "/" & txt & "*")
        e.Graphics.DrawString(codestr, drawFont, Brushes.Black, e.MarginBounds.X, e.MarginBounds.Y)
        e.Graphics.DrawString(NumericUpDown2.Value & "/" & txt, arialFont, Brushes.Black, e.MarginBounds.X + 30, e.MarginBounds.Y + NumericUpDown1.Value + 20)

    End Sub
    Dim devicelist As New List(Of String)
    Public Sub devicefind()

        For Each founddir As String In My.Computer.FileSystem.GetDirectories(Application.StartupPath & "\devices\", FileIO.SearchOption.SearchTopLevelOnly)
            Dim dirinfo As System.IO.DirectoryInfo = My.Computer.FileSystem.GetDirectoryInfo(founddir)
            Dim trestfilname = dirinfo.Name
            If Not trestfilname.Contains("user file") Then
                devicelist.Add(trestfilname)

            End If

        Next
    End Sub


    Public Sub barcodesaved()
        Dim code As Integer

        Dim listtext As New List(Of List(Of String))
        Dim cestacb9 As String
        Dim labelpath As String = Nothing
        Try
            devicefind()
            For Each linecb In devicelist
                Dim c As New List(Of String)
                labelpath = (Application.StartupPath & "\devices\" & linecb & "\label.txt")
                If IO.File.Exists(labelpath) Then
                    Dim stringlabel() As String = Nothing
                    Try

                        stringlabel = Split((IO.File.ReadAllText(labelpath)), "#")

                        If IsNumeric(Trim(stringlabel(12))) Then
                            'vyskúšajčíslovsúborelabel()
                            code = Trim(stringlabel(12))

                            ListView1.Items.Add(code)
                            Try
                                If Trim(stringlabel(13)) <> String.Empty Then
                                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Replace(Trim(stringlabel(13)), vbCrLf, Nothing))
                                Else
                                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(RemoveDiacritics(LSet(linecb, 7).ToUpper)))
                                End If
                            Catch ex As Exception
                                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(RemoveDiacritics(LSet(linecb, 7).ToUpper)))
                            End Try
                            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(labelpath)
                            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(linecb)
                            If (LSet(linecb.ToUpper, 7).Equals(TextBox2.Text)) Then
                                alreadygenerated = True
                            End If
                            list.Add(Int(code))
                        Else

                            If linecb <> String.Empty And linecb <> Nothing Then
                                c.AddRange({linecb, labelpath})
                                listtext.Add(c)
                            End If

                        End If

                    Catch ex As Exception
                    End Try
                    cestacb9 = Application.StartupPath & "\devices\" & linecb & "\devparts"
                    If IO.File.Exists(cestacb9) Then
                        For Each linecb9 In IO.File.ReadAllLines(cestacb9)
                            Dim c9 As New List(Of String)
                            linecb9 = Replace(linecb9, vbCrLf, Nothing)
                            labelpath = Application.StartupPath & "\devices\" & linecb & "\" & linecb9 & "\label.txt"
                            Dim stringlabel9() As String = Nothing
                            If IO.File.Exists(labelpath) Then
                                stringlabel9 = Split((IO.File.ReadAllText(labelpath)), "#")
                                If IsNumeric(Trim(stringlabel9(12))) Then
                                    code = Trim(stringlabel9(12))
                                    ListView1.Items.Add(Int(code))
                                    Try
                                        If Trim(stringlabel9(13)) <> String.Empty Then
                                            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Replace(Trim(stringlabel9(13)), vbCrLf, Nothing))
                                        Else
                                            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(RemoveDiacritics(LSet(linecb9, 7).ToUpper)))
                                        End If

                                    Catch ex As Exception
                                        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(RemoveDiacritics(Trim(LSet(linecb9, 7).ToUpper)))
                                    End Try
                                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(labelpath)
                                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(linecb9)
                                    If (LSet(linecb9.ToUpper, 7).Equals(TextBox2.Text)) Then
                                        alreadygenerated = True
                                    End If
                                    list.Add(Int(code))
                                Else


                                    If linecb9 <> String.Empty And linecb9 <> Nothing Then
                                        c.AddRange({linecb, labelpath})
                                        listtext.Add(c)
                                    End If

                                End If

                            End If
                        Next
                    End If
                End If
            Next
        Dim cislo As Integer

        If list.Count = 0 Then list.Add(0)
        For y = 0 To listtext.Count - 1
            'Dim nazvy() As String = Split(nazvyapath(y), vbTab)

            Dim nazov As String = listtext(y)(0)
            If nazov <> String.Empty And nazov <> Nothing Then
                Dim x As Integer = 1
                list.Sort()
                For z = 0 To list.Count - 1
                    If IsNumeric(z) Then
                        cislo = list(z)
                        If cislo <> x And Not list.Contains(x) Then
                            'vložínepoužitýcode
                            list.Add(x)
                            ListView1.Items.Add(x)
                            'vložíkrátkytext
                            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(RemoveDiacritics(LSet(nazov, 7))))
                            'vloží(cestuksúboru)
                            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(listtext(y)(1))
                            'vložínázov()
                            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(nazov)
                            Exit For
                        Else
                            x += 1
                        End If

                    End If
                Next

            End If

        Next



        Catch ex As Exception

        End Try


    End Sub


    Private Sub NumericUpDown1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        barcode()
    End Sub
    Private Sub PrintDocument2_BeginPrint(sender As System.Object, e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        ItemCount = 0
    End Sub
    Public Sub barcodesave()
        Try
            Dim labelpath As String = Label3.Text
            Dim stringlabel() As String = Split((My.Computer.FileSystem.ReadAllText(labelpath)), "#")
            Dim stitok As New StringBuilder
            For x = 0 To 11
                stitok.Append(stringlabel(x) & "#")

            Next
            stitok.Append(NumericUpDown2.Value & vbCrLf & "#" & vbCrLf & TextBox2.Text & vbCrLf & "#")
            My.Computer.FileSystem.WriteAllText(labelpath, stitok.ToString, False)
        Catch ex As Exception

        End Try

    End Sub
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click


        If PrintDialog1.ShowDialog = DialogResult.OK Then
            PrintDocument2.DefaultPageSettings.Landscape = False
            PrintDocument2.OriginAtMargins = True
            ItemCount = 0
            PrintPreviewDialog1.Document = PrintDocument2
            PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
            PrintPreviewDialog1.ShowDialog()

        End If

        For Each item As ListViewItem In ListView1.Items
            Dim labelpath As String = item.SubItems(2).Text

            Dim stringlabel() As String = Split((My.Computer.FileSystem.ReadAllText(labelpath)), "#")

            Dim stitok As New StringBuilder
            For x = 0 To 11
                stitok.Append(stringlabel(x) & "#")
            Next
            stitok.Append(item.Text & vbCrLf & "#")

            My.Computer.FileSystem.WriteAllText(labelpath, stitok.ToString, False)
        Next

    End Sub
    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Try
            For Each item As ListViewItem In ListView1.Items
                Dim labelpath As String = item.SubItems(2).Text
                Dim stringlabel() As String = Split((My.Computer.FileSystem.ReadAllText(labelpath)), "#")

                Dim stitok As New StringBuilder
                For x = 0 To 11
                    stitok.Append(stringlabel(x) & "#")
                Next
                stitok.Append(item.Text & vbCrLf & "#" & vbCrLf & item.SubItems(1).Text & vbCrLf & "#")
                Try


                    My.Computer.FileSystem.WriteAllText(labelpath, stitok.ToString, False)
                Catch ex As Exception

                End Try
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private Sub pdoc_PrintPage2(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage


        Dim text7 As String = String.Empty
        Dim fontsize As Integer = NumericUpDown1.Value
        Dim drawFont As New System.Drawing.Font(CustomFont.GetInstance(fontsize, 0), FontStyle.Regular)
        Dim arialFont As New Font("arial", fontsize / 4)
        Dim Y As Integer
        Dim x As Integer = 0
        Dim linelenght As Integer = e.MarginBounds.Width - fontsize * 2
        While ItemCount < ListView1.Items.Count
            'For ItemCount = ItemCount To ListView1.Items.Count - 1
            Dim Item As ListViewItem = ListView1.Items(ItemCount)
            text7 = RemoveDiacritics(LSet(Item.SubItems(1).Text, 7))
            Dim codestr As String = ("*" & Item.Text.ToUpperInvariant & "/" & text7.ToUpperInvariant & "*")
            If Y + drawFont.Height + arialFont.Height > e.MarginBounds.Bottom Then Exit While

            e.Graphics.DrawString(codestr.ToUpperInvariant, drawFont, Brushes.Black, x + 0, Y)
            e.Graphics.DrawString(Item.Text & "/" & text7.ToUpperInvariant, arialFont, Brushes.Black, x + 30, Y + drawFont.Height)
            x += fontsize * 8 + NumericUpDown3.Value
            If x >= linelenght Then
                Y += drawFont.Height + arialFont.Height + NumericUpDown4.Value
                x = 0
            End If
            ItemCount += 1
        End While
        'Next
        If ItemCount < ListView1.Items.Count Then e.HasMorePages = True
    End Sub




    Private Sub ListView1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Try


            Dim lv As ListView = ListView1
            focu = lv.FocusedItem.Index
            NumericUpDown2.Value = lv.FocusedItem.SubItems.Item(0).Text
            TextBox2.Text = lv.FocusedItem.SubItems.Item(1).Text
            Label3.Text = lv.FocusedItem.SubItems.Item(2).Text




            barcode()

        Catch ex As Exception

        End Try


    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        If ListView1.SelectedItems.Count > 0 Then
            ListView1.Items(focu).Text = NumericUpDown2.Value
        End If
    End Sub
    Private Sub TextBox2_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox2.TextChanged
        If ListView1.SelectedItems.Count > 0 Then
            ListView1.Items(focu).SubItems(1).Text = TextBox2.Text
        End If
    End Sub
    Public Shared Function RemoveDiacritics(s As String) As String
        s = s.Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder()
        For i As Integer = 0 To s.Length - 1
            If CharUnicodeInfo.GetUnicodeCategory(s(i)) <> UnicodeCategory.NonSpacingMark Then
                sb.Append(s(i))
            End If
        Next
        Return sb.ToString()
    End Function


    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        barcodesave()
    End Sub





    'Private Sub Button6_Click(sender As System.Object, e As System.EventArgs)

    '    PageSetupDialog1.Document = PrintDocument2
    '    PageSetupDialog1.ShowDialog()

    'End Sub
End Class
