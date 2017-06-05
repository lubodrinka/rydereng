Imports System.Windows.Forms
Imports System.Text
Imports System.IO

Public Class Dialog4

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Dim line As New StringBuilder
        Dim x As Integer = 0
        For Each li In CheckedListBox1.Items
            x = +1
            If CheckedListBox1.CheckedItems.Contains(li) Then
                line.AppendLine(x & " " & li & vbTab & "True")
            ElseIf CheckedListBox1.CheckedItems.Contains(li) = False Then
                line.AppendLine(x & " " & li & vbTab & "false")
            End If

        Next

        My.Settings.dialog4 = line.ToString
        My.Settings.oktext = TextBox1.Text
        My.Settings.notoktext = TextBox2.Text
        My.Settings.mailsign = TextBox3.Text
        My.Settings.regday = TextBox4.Text
        My.Settings.regweek = TextBox5.Text
        My.Settings.regmonth = TextBox6.Text
        My.Settings.regyear = TextBox7.Text
        My.Settings.mena = TextBox8.Text
        My.Settings.isotext = TextBox9.Text
        Try
            My.Settings.timebarcodeidle = NumericUpDown1.Value
            ryder.Timer1.Interval = NumericUpDown1.Value
        Catch ex As Exception

        End Try
        My.Settings.scanerinput = ComboBox3.SelectedIndex
        If ComboBox3.SelectedIndex = 0 Then
            bothscanners()
        ElseIf ComboBox3.SelectedIndex = 1 Then
            directscannerinitalization()
        ElseIf ComboBox3.SelectedIndex = 2 Then
            textboxdirectscanerinitalization()
        End If
        ryder.cbread()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.BackColor = ColorDialog1.Color
            My.Settings.okbackcolor = ColorDialog1.Color
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox2.BackColor = ColorDialog1.Color
            My.Settings.notokbackcolor = ColorDialog1.Color

        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        AboutBox1.Show()
    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        My.Settings.reporti = ComboBox1.SelectedIndex
        'MsgBox(My.Settings.reporti)
    End Sub

    Private Sub Dialog4_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = My.Settings.reporti
        ComboBox2.Items.Clear()
        ComboBox3.SelectedIndex = My.Settings.scanerinput
        Me.AllowDrop = True
        Try

            Dim user() As String = Split(My.Settings.user, vbCrLf)
            For x As Integer = 0 To UBound(user)

                Dim name() As String = Split(user(x), vbTab)
                ComboBox2.Items.Add(name(0))
            Next
        Catch ex As Exception

        End Try
        ComboBox2.SelectedIndex = ryder.ComboBox4.SelectedIndex
        ToolTip1.SetToolTip(Button31, "kill all open excel application")
    End Sub

    Public Sub ulozprava()
        Try
            Dim s As New StringBuilder

            Dim myuser() As String = Split(My.Settings.user, vbCrLf)

            For x = 0 To UBound(myuser)
                Dim names() As String = Split(myuser(x), vbTab)

                If names(0) = ComboBox2.Text Then
                    s.AppendLine(names(0) & vbTab & names(1) & vbTab & "|" & CheckBox1.Checked & "|" & CheckBox2.Checked & "|" & CheckBox4.Checked)
                Else
                    If myuser(x) <> String.Empty Then

                        s.AppendLine(myuser(x))

                    End If
                End If

            Next

            My.Settings.user = s.ToString
            'MsgBox(My.Settings.user)
        Catch ex As Exception
            '    MsgBox(ex.ToString)
        End Try
    End Sub




    Private Sub ComboBox2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Try


            Dim myuser() As String = Split(My.Settings.user, vbCrLf)
            For x = 0 To UBound(myuser)
                Dim names() As String = Split(myuser(x), vbTab)
                If names(0) = ComboBox2.Text Then
                    Dim check() As String = Split(myuser(x), "|")

                    If check(1).Contains("True") Then
                        'MsgBox(check(1) & check(2))
                        CheckBox1.Checked = True
                    Else
                        CheckBox1.Checked = False
                    End If

                    If check(2).Contains("True") Then
                        CheckBox2.Checked = True
                    Else
                        CheckBox2.Checked = False
                    End If
                    If check(3).Contains("True") Then
                        CheckBox4.Checked = True
                    Else
                        CheckBox4.Checked = False
                    End If



                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button21_Click(sender As System.Object, e As System.EventArgs) Handles Button21.Click
        ulozprava()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If FontDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.okforecolor = FontDialog1.Color
            My.Settings.okfont = FontDialog1.Font
            TextBox1.ForeColor = My.Settings.okforecolor
            TextBox1.Font = My.Settings.okfont
        End If
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        If FontDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.notokforecolor = FontDialog1.Color
            My.Settings.notokfont = FontDialog1.Font
            TextBox2.ForeColor = My.Settings.okforecolor
            TextBox2.Font = My.Settings.okfont
        End If
    End Sub
    Private Sub Dialog4_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Dialog4_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
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
                    Me.PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        'MsgBox(My.Settings.user)
        If ryder.hesla.ContainsKey(ComboBox2.SelectedIndex) And ryder.hesla.Count > 1 Then
            'MsgBox(hesla(ComboBox4.SelectedIndex))

            If InputBox("password") = ryder.hesla(ComboBox2.SelectedIndex) Then

                Dim cbnewline As String = ComboBox2.Text
                If cbnewline <> String.Empty Then
                    Dim user() As String = Split(My.Settings.user, vbCrLf)
                    Dim s As New StringBuilder

                    For x As Integer = 0 To UBound(user)
                        Dim names() As String = Split(user(x), vbTab)

                        If names(0) <> String.Empty And names(0) <> cbnewline Then

                            s.AppendLine(user(x))
                        ElseIf names(0) = cbnewline Then
                            Dim re As String = names(0) & vbTab & InputBox("new password", "change password") & vbTab & "|" & CheckBox1.Checked & "|" & CheckBox2.Checked & "|" & CheckBox4.Checked
                            s.AppendLine(Trim(re))

                        End If
                    Next
                    'MsgBox(s.ToString)
                    My.Settings.user = Nothing
                    My.Settings.user = s.ToString
                    'MsgBox(My.Settings.user)
                    ryder.cbuseursread()
                End If
            Else
                MsgBox("false password")
            End If
        End If
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click

        If My.Settings.adminheslo = Nothing Then My.Settings.adminheslo = InputBox("set password for admin")
        If My.Settings.adminheslo <> Nothing Then
            If My.Settings.adminheslo = InputBox(" Type admin password") Then

                Dialog10.ListView1.Items.Clear()
              
                For Each it As String In ryder.fileContentscb

                    Dialog10.ListView1.Items.Add(it)
                    Dim sssstxt As String = Application.StartupPath & "\devices\" & it + "\label.txt"
                    Dim fileContentscb As String
                    Dim datum As Byte = 0 : Dim dat As Byte = 1 : Dim SN As Byte = 2 : Dim výkon As Byte = 3 : Dim loc As Byte = 4 : Dim ST As Byte = 5 : Dim adr1 As Byte = 6 : Dim adr2 As Byte = 7 : Dim mail As Byte = 8 : Dim typ As Byte = 9 : Dim mt As Byte = 10 : Dim mtdenne As Byte = 11 : Dim bcode As Byte = 12
                    Try

                        fileContentscb = My.Computer.FileSystem.ReadAllText(sssstxt)

                        fileContentscb = Replace(fileContentscb, vbCrLf, " ")

                        Dim text() As String = Split(fileContentscb, "#")
                        Dim ic As Integer = Dialog10.ListView1.Items.Count - 1
                        With Dialog10.ListView1.Items(ic).SubItems
                            .Add(Trim(text(typ))) : .Add(Trim(text(výkon))) : .Add(Trim(text(loc))) : .Add(Trim(text(SN)))
                            .Add(Trim(text(ST))) : .Add(Trim(text(adr2))) : .Add(Trim(text(adr1))) : .Add(Trim(text(mail)))
                            Try : .Add(CInt(Trim(text(mt)))) : Catch ex As Exception : End Try : Try : .Add(CInt(Trim(text(mtdenne)))) : Catch ex As Exception : End Try
                            : .Add(Trim(text(datum))) : .Add(Trim(text(dat)))
                        End With

                    Catch ex As Exception

                    End Try
                Next

                If My.Settings.devpermision <> Nothing Then


                    If My.Settings.devpermision.Contains(vbCrLf) Then
                        For Each devset As String In My.Settings.devpermision.Split(vbCrLf)
                            If devset.Contains(Trim(ComboBox2.Text)) Then
                                Dim devsetstored() As String = Split(devset, "|")
                                For x As Integer = 0 To Dialog10.ListView1.Items.Count - 1
                                    For z As Integer = 1 To UBound(devsetstored)
                                        If Dialog10.ListView1.Items(x).Text = devsetstored(z) Then Dialog10.ListView1.Items(x).Checked = True
                                    Next
                                Next
                            End If

                        Next

                    End If
                End If
                Dialog10.Text = Button6.Text & "  " & ComboBox2.Text
                If Dialog10.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim devpnew As New StringBuilder
                    devpnew.Append(ComboBox2.Text)
                    Dim zhoda As Boolean = False
                    For Each device As ListViewItem In Dialog10.ListView1.CheckedItems
                        devpnew.Append("|" & device.Text)
                    Next
                    Dim devper() As String = Nothing
                    Dim userdev() As String = Nothing
                    Dim newpermision As New StringBuilder
                    If My.Settings.devpermision = Nothing Then
                        My.Settings.devpermision = devpnew.ToString & vbCrLf
                        GoTo enx
                    Else


                        If My.Settings.devpermision.Contains(vbCrLf) Then
                            'modifikovanie(povolení)
                            devper = Split(My.Settings.devpermision, vbCrLf)
                            For y As Integer = 0 To UBound(devper)
                                'vlastne pre každé usera
                                userdev = Split(devper(y), "|")
                                Dim user As String = userdev(0)
                                If user <> String.Empty Then
                                    If user = (Trim(ComboBox2.Text)) Then
                                        ' najde user potom k uživateľovi pripoji všetky check user "|" device
                                        newpermision.Append(devpnew.ToString & vbCrLf)
                                        'MsgBox("2 " & newpermision.ToString)
                                        zhoda = True
                                    ElseIf user <> (Trim(ComboBox2.Text)) Then
                                        newpermision.Append(vbCrLf & devper(y))
                                        'opíše ostatných užívateľov povolne device to je devepr(y) 
                                    End If
                                End If
                            Next
                            'ak ešte nie zaznamenaý prilepí na koniec nové user
                        End If
                        'novépovolenie
                        If zhoda = False Then newpermision.Append(vbCrLf & devpnew.ToString & vbCrLf)
                        My.Settings.devpermision = newpermision.ToString
                    End If

                    End If
            End If
            ryder.cbread()
                Else
                    MsgBox("false password")
                End If

enx:
        'MsgBox(My.Settings.devpermision)
    End Sub

   
    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        If My.Settings.adminheslo = Nothing Then My.Settings.adminheslo = InputBox("set password for admin")
        If My.Settings.adminheslo <> Nothing Then
            If My.Settings.adminheslo = InputBox(" Type admin password") Then
                My.Settings.devpermision = Nothing
            End If
        End If

    End Sub

    Private Sub Button45_Click(sender As System.Object, e As System.EventArgs) Handles Button45.Click
        Dialog11.ShowDialog()
    End Sub



    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
        If MsgBox("are you sure", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            My.Settings.heslo = Nothing

        End If


    End Sub

    Private Sub PictureBox1_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles PictureBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If

    End Sub

    Private Sub Button31_Click(sender As System.Object, e As System.EventArgs) Handles Button31.Click
        For Each process In System.Diagnostics.Process.GetProcesses()
            If process.ProcessName.Contains("EXCEL") Then
                process.Kill()
            End If

        Next

    End Sub
End Class
