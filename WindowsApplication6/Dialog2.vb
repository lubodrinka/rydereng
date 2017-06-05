Imports System.Windows.Forms
Imports System.IO

Public Class Dialog2

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click



        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        NumericUpDown2.ReadOnly = True
        Try
            Dim výsledok As Decimal = (365 / NumericUpDown1.Value * NumericUpDown3.Value)
            Label7.Text = Format(výsledok, "0.000")
        Catch ex As Exception
        End Try
        NumericUpDown1.ReadOnly = False

        NumericUpDown2.Value = 0

    End Sub
    Private Sub NumericUpDown2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        NumericUpDown1.ReadOnly = True
        Try
            Dim výsledok As Decimal = (8760 / NumericUpDown2.Value * NumericUpDown3.Value)
            Label7.Text = Format(výsledok, "0.000")
        Catch ex As Exception
        End Try
        NumericUpDown2.ReadOnly = False

        NumericUpDown1.Value = 0
    End Sub
    Private Sub NumericUpDown3_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown3.ValueChanged
        Try
            If NumericUpDown2.Value > 0 Then
                Dim výsledok As Decimal = (8760 / NumericUpDown2.Value * NumericUpDown3.Value)
                Label7.Text = Format(výsledok, "0.000")
            ElseIf NumericUpDown1.Value > 0 Then
                Dim výsledok As Decimal = (365 / NumericUpDown1.Value * NumericUpDown3.Value)
                Label7.Text = Format(výsledok, "0.000")
            End If
        Catch ex As Exception

        End Try



    End Sub






    Private Sub Dialog2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            If NumericUpDown2.Value > 0 Then
                Dim výsledok As Decimal = (8760 / NumericUpDown2.Value * NumericUpDown3.Value)
                Label7.Text = Format(výsledok, "0.000")

            ElseIf NumericUpDown1.Value > 0 Then
                Dim výsledok As Decimal = (365 / NumericUpDown1.Value * NumericUpDown3.Value)
                Label7.Text = Format(výsledok, "0.000")
            End If
        Catch ex As Exception

        End Try
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
                                Case "Dialog2"
                                    For Each menco As Control In Me.Controls
                                        If menco.Name = form(1) Then menco.Text = colums(0)
                                    Next
                                Case Else
                            End Select
                        End If
                    End If
                Next
            End If

        Catch ex As Exception

        End Try
        Try
            PictureBox1.Image = Nothing
            Dim folderExists As Boolean
            Dim appzar As String = Nothing
            If ryder.aktivnycombobox = 1 Then
                appzar = Application.StartupPath & "\devices\" + ryder.ComboBox1.Text + "\"
            ElseIf ryder.aktivnycombobox = 2 Then
                appzar = Application.StartupPath & "\devices\" + ryder.ComboBox1.Text + "\" + ryder.ComboBox9.Text + "\"
            End If
            If folderExists = My.Computer.FileSystem.DirectoryExists(appzar) Then
                My.Computer.FileSystem.CreateDirectory(appzar) : End If
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(appzar)
                Dim testFile As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(foundFile)
                If foundFile.Contains(".gif") Or foundFile.Contains(".ico") Then GoTo dalsi
                Dim splitfoundfile() As String = Split(testFile.Name, ".")

                If Trim(splitfoundfile(0)) = Trim(TextBox1.Text) Then
                    PictureBox1.Load(foundFile)
                    PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                End If
dalsi:
            Next
        Catch ex As Exception

        End Try
        Try


            ListView5.Items.Clear()
            ': ComboBox6.Items.Clear()
            'ComboBox6.UseWaitCursor = True
            Dim cb1 As String = Nothing
            Dim riadok() As String = Nothing
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\devices\", FileIO.SearchOption.SearchAllSubDirectories, "parts")
                Dim fileContentscb As String
                Dim fileExists As Boolean : If fileExists = My.Computer.FileSystem.FileExists(foundFile) = False Then
                    fileContentscb = My.Computer.FileSystem.ReadAllText(foundFile)
                    Dim parts() As String = Split(fileContentscb, vbCrLf)
                    For x = 0 To UBound(parts) - 1
                        Try
                            riadok = Split(parts(x), "|")
                            Dim i As Integer = ListView5.Items.Count
                            ListView5.Items.Add(riadok(1))
                            '"int. výmeny dni 2"
                            ListView5.Items(i).SubItems.Add(riadok(3))
                            '"int. výmeny MTH.4"
                            ListView5.Items(i).SubItems.Add(riadok(5))
                            Try
                                'cena5
                                ListView5.Items(i).SubItems.Add(riadok(9))
                                'čas montáže6
                                ListView5.Items(i).SubItems.Add(riadok(11))
                            Catch ex As Exception
                            End Try
                            Try
                                'objednkové číslo
                                Me.ListView5.Items(i).SubItems.Add(riadok(13))
                                'info
                                Me.ListView5.Items(i).SubItems.Add(riadok(15))
                            Catch ex As Exception
                            End Try
                        Catch ex As Exception
                        End Try
                    Next
                End If
            Next


        Catch ex As Exception

        End Try
    End Sub





    Private Sub PictureBox1_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles PictureBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If

    End Sub


    Private Sub Dialog2_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Dialog2_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
        Try


            Dim jj As String = Nothing
            PictureBox1.Image = Nothing
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim filePaths As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
                For Each fileLoc As String In filePaths
                    'FileCopy(Path.GetFullPath(fileLoc), cestazarpriečinkyjj)
                    Try
                        PictureBox1.Image = Image.FromFile(fileLoc)
                        PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
                        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                    Catch ex As Exception
                        '    MsgBox(ex.ToString)
                    End Try




                    Dim pripona() As String = Split(My.Computer.FileSystem.GetName(fileLoc), ".")
                    jj = TextBox1.Text
                    If File.Exists(fileLoc) Then
                        'urobi kopiu file do priečinku devices
                        Dim cestasuzarpriečinky As String = Nothing
                        Dim cestazarpriečinkyjj As String = Nothing
                        If ryder.aktivnycombobox = 1 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" + ryder.ComboBox1.Text + "\" + jj + "." + pripona(1)
                        ElseIf ryder.aktivnycombobox = 2 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" + ryder.ComboBox1.Text + "\" + ryder.ComboBox9.Text + "\" + jj + "." + pripona(1)
                        End If
                        FileCopy(Path.GetFullPath(fileLoc), cestazarpriečinkyjj)
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ListView5_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView5.SelectedIndexChanged

        Try
            NumericUpDown1.Value = ListView5.FocusedItem.SubItems(1).Text
            NumericUpDown2.Value = ListView5.FocusedItem.SubItems(2).Text
            NumericUpDown3.Value = ListView5.FocusedItem.SubItems(3).Text
            NumericUpDown4.Value = ListView5.FocusedItem.SubItems(4).Text


            TextBox1.Text = ListView5.FocusedItem.Text
            TextBox2.Text = ListView5.FocusedItem.SubItems(5).Text
            TextBox3.Text = ListView5.FocusedItem.SubItems(6).Text
            Label7.Text = "0"
        Catch ex As Exception

        End Try
    End Sub

  

    Private Sub TextBox1_TextChanged_1(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged
        Try
            For Each item As ListViewItem In ListView5.Items

                If item.Text.Contains(TextBox1.Text) Then
                    item.BackColor = Color.LimeGreen
                    ListView5.EnsureVisible(item.Index)
                ElseIf TextBox1.Text = Nothing Then
                    item.BackColor = Color.Black

                Else
                    item.BackColor = Color.Black
                End If

            Next

        Catch ex As Exception

        End Try
    End Sub

    Private Sub PictureBox1_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles PictureBox1.DragDrop
        Try


            Dim jj As String = Nothing
            PictureBox1.Image = Nothing
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim filePaths As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
                For Each fileLoc As String In filePaths
                    'FileCopy(Path.GetFullPath(fileLoc), cestazarpriečinkyjj)
                    Try
                        PictureBox1.Image = Image.FromFile(fileLoc)
                        PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
                        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                    Catch ex As Exception
                        '    MsgBox(ex.ToString)
                    End Try




                    Dim pripona() As String = Split(My.Computer.FileSystem.GetName(fileLoc), ".")
                    jj = TextBox1.Text
                    If File.Exists(fileLoc) Then
                        'urobi kopiu file do priečinku devices
                        Dim cestasuzarpriečinky As String = Nothing
                        Dim cestazarpriečinkyjj As String = Nothing
                        If ryder.aktivnycombobox = 1 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" + ryder.ComboBox1.Text + "\" + jj + "." + pripona(1)
                        ElseIf ryder.aktivnycombobox = 2 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" + ryder.ComboBox1.Text + "\" + ryder.ComboBox9.Text + "\" + jj + "." + pripona(1)
                        End If
                        FileCopy(Path.GetFullPath(fileLoc), cestazarpriečinkyjj)
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class
