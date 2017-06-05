Imports System.Collections.ObjectModel

Public Class Login

    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See http://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc.

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        ryder.povolenie = False
        Dim userexist As Boolean = False
        If ComboBox1.Text <> String.Empty Then


            If ryder.hesla.ContainsKey(ComboBox1.SelectedIndex) And ryder.hesla.Count >= 1 Then


                If TextBox1.Text = ryder.hesla(ComboBox1.SelectedIndex) Then
                    ryder.povolenie = True

                Else
                    ryder.povolenie = False

                End If

            ElseIf ryder.hesla.Count = 0 Then
                ryder.povolenie = True

            End If

            If ryder.povolenie = True Then

                Dim meno1 As String = ComboBox1.Text
                Dim myuser() As String = Split(My.Settings.user, vbCrLf)
                For x = 0 To UBound(myuser)
                    Dim names() As String = Split(myuser(x), vbTab)
                    If names(0) = meno1 Then
                        userexist = True
                        If myuser(x).Contains("|") Then
                            Dim check() As String = Split(myuser(x), "|")

                            If check(1).Contains("True") = False Then

                                'deletebutton
                                ryder.Button4.Enabled = False : ryder.Button20.Enabled = False : ryder.Button16.Enabled = False : ryder.Button13.Enabled = False
                            End If
                            If check(2).Contains("True") = False Then
                                'Savebutton
                                ryder.Button2.Enabled = False : ryder.Button21.Enabled = False : ryder.Button6.Enabled = False : ryder.Button12.Enabled = False
                            End If
                            If check(3).Contains("True") = False Then
                                'Write records
                                ryder.Button1.Enabled = False : ryder.Button8.Enabled = False : Form13.Button35.Enabled = False : ryder.Button38.Enabled = False


                            End If
                        End If
                    End If
                Next
                If userexist = False Then MsgBox(UsernameLabel.Text & vbCrLf & ryder.povolenie.ToString)
                ComboBox1.SelectedItem = ComboBox1.Text
                ryder.ComboBox4.Text = Me.ComboBox1.SelectedItem




                If ComboBox2.Text <> "" Then
                    zamenajazyka()
                End If
                Form4.ToolStripComboBox1.SelectedItem = ComboBox2.SelectedItem
                My.Settings.language = ComboBox2.SelectedItem
                Me.Close()
            ElseIf ryder.povolenie = False Then

                MsgBox(Label2.Text & vbCrLf & ryder.povolenie.ToString)
            End If


        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
        ryder.Close()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.TextUpdate
        If My.Settings.user = String.Empty Then

            Button12.Visible = True : OK.Enabled = False
        End If
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        'zapíše do user2.txt z combo boxu bo zatlačení button 12.

        
        Try
            Dim cbnewline As String = ComboBox1.Text
            If cbnewline <> String.Empty Then
                If My.Settings.user <> String.Empty Then
                    My.Settings.user = My.Settings.user & cbnewline & vbTab & TextBox1.Text & vbTab & "|False" & "|False" & "|False" & vbCrLf
                ElseIf My.Settings.user = String.Empty Then
                    My.Settings.user = cbnewline & vbTab & TextBox1.Text & vbTab & "|True" & "|True" & "|True" & vbCrLf
                End If
                Button12.Hide() : ryder.cbuseursread()
                ComboBox1.SelectedItem = ComboBox1.Text
                OK.Enabled = True

            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Private Sub Label1_Click(sender As System.Object, e As System.EventArgs) Handles Label1.Click
        Try

            Dim isAvailable = My.Computer.Network.IsAvailable
            If isAvailable = True Then WebBrowser1.Navigate("https://sites.google.com/site/ryderhelp/home2/how-to-start")
            WebBrowser1.Show()
            Button2.Show()
            Button1.Show()
            ComboBox2.Hide()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        WebBrowser1.Hide()
        Button1.Hide()
        Me.Size = New Size(411, 386)
        Button2.Hide()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        WebBrowser1.GoBack()
    End Sub

    Private Sub Login_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load



        Dim files As ReadOnlyCollection(Of String)
        files = My.Computer.FileSystem.FindInFiles(Application.StartupPath & "\", "", True, FileIO.SearchOption.SearchTopLevelOnly, "*.lng")
        For Each jazyk As String In files
            Dim testFile As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(jazyk)
            Dim fileName As String = testFile.Name
            Dim withoutParts As String = Replace(testFile.Name, ".lng", "")
            ComboBox2.Items.Add(withoutParts)
            If ComboBox2.Items.Count > 0 Then ComboBox2.Show()
            Form4.ToolStripComboBox1.Items.Add(withoutParts)

            ComboBox2.Items.Add(My.Settings.language)
            ComboBox2.SelectedIndex = ComboBox2.Items.Count - 1

        Next
        'My.Settings.user = String.Empty
        setup1()
        Try

            ComboBox1.SelectedIndex = ComboBox1.FindStringExact(My.Settings.lastuser)
        Catch ex As Exception

        End Try

    End Sub

    Public Sub zamenajazyka()
        My.Forms.ryder.ComboBox2.Items.Clear()
        Try
            Dim lang As String = Application.StartupPath & "\" & ComboBox2.Text & ".lng"
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
                                        Try
                                            If form(2) = "combobox2" Then My.Forms.ryder.ComboBox2.Items.Add(colums(0))
                                        Catch ex As Exception
                                        End Try
                                        If "ColumnHeader1" = form(1) Then My.Forms.ryder.ColumnHeader1.Text = colums(0)
                                        If "ColumnHeader2" = form(1) Then My.Forms.ryder.ColumnHeader2.Text = colums(0)
                                        If "ColumnHeader3" = form(1) Then My.Forms.ryder.ColumnHeader3.Text = colums(0)
                                        If "ColumnHeader4" = form(1) Then My.Forms.ryder.ColumnHeader4.Text = colums(0)
                                        If "ColumnHeader5" = form(1) Then My.Forms.ryder.ColumnHeader5.Text = colums(0)
                                        If "ColumnHeader6" = form(1) Then My.Forms.ryder.ColumnHeader6.Text = colums(0)
                                        If "ColumnHeader7" = form(1) Then My.Forms.ryder.ColumnHeader7.Text = colums(0)
                                        If "ColumnHeader8" = form(1) Then My.Forms.ryder.ColumnHeader8.Text = colums(0)
                                        If "ColumnHeader9" = form(1) Then My.Forms.ryder.ColumnHeader9.Text = colums(0)
                                        If "ColumnHeader10" = form(1) Then My.Forms.ryder.ColumnHeader10.Text = colums(0)
                                        If "ColumnHeader11" = form(1) Then My.Forms.ryder.ColumnHeader11.Text = colums(0)
                                        If "ColumnHeader12" = form(1) Then My.Forms.ryder.ColumnHeader12.Text = colums(0)
                                        If "ColumnHeader18" = form(1) Then My.Forms.ryder.ColumnHeader18.Text = colums(0)
                                        If "ColumnHeader19" = form(1) Then My.Forms.ryder.ColumnHeader19.Text = colums(0)
                                        If "ColumnHeader20" = form(1) Then My.Forms.ryder.ColumnHeader20.Text = colums(0)
                                        If "ColumnHeader21" = form(1) Then My.Forms.ryder.ColumnHeader21.Text = colums(0)
                                        If "ColumnHeader22" = form(1) Then My.Forms.ryder.ColumnHeader22.Text = colums(0)
                                        If "ColumnHeader23" = form(1) Then My.Forms.ryder.ColumnHeader23.Text = colums(0)
                                        If "ColumnHeader24" = form(1) Then My.Forms.ryder.ColumnHeader24.Text = colums(0)
                                        If "ColumnHeader25" = form(1) Then My.Forms.ryder.ColumnHeader25.Text = colums(0)
                                        If "ColumnHeader26" = form(1) Then My.Forms.ryder.ColumnHeader26.Text = colums(0)
                                        If "ColumnHeader29" = form(1) Then My.Forms.ryder.ColumnHeader29.Text = colums(0)
                                        Try
                                            For Each menco As Control In My.Forms.ryder.Panel1.Controls
                                                If menco.Name = form(2) And menco.Name <> "WebBrowser1" Then menco.Text = colums(0)
                                            Next
                                        Catch ex As Exception
                                        End Try
                                    Case "mail"
                                        For Each menco As Control In My.Forms.mail.Controls
                                            If menco.Name = form(1) Then menco.Text = colums(0)
                                        Next
                                    Case "Form4"
                                        For Each menco As Control In My.Forms.Form4.Controls
                                            If menco.Name = form(1) Then menco.Text = colums(0)
                                        Next

                                        If My.Forms.Form4.Name & ".ColumnHeader1" = form(0) & "." & form(1) Then My.Forms.Form4.ColumnHeader1.Text = colums(0)
                                        If My.Forms.Form4.Name & ".ColumnHeader2" = form(0) & "." & form(1) Then My.Forms.Form4.ColumnHeader2.Text = colums(0)
                                    Case "Dialog1"
                                        For Each menco As Control In My.Forms.Dialog1.TableLayoutPanel1.Controls
                                            If menco.Name = form(1) Then menco.Text = colums(0)
                                        Next
                                    Case "Dialog2"
                                        For Each menco As Control In My.Forms.Dialog2.Controls
                                            If menco.Name = form(1) Then menco.Text = colums(0)
                                        Next
                                    Case "Dialog3"
                                        For Each menco As Control In My.Forms.Dialog3.Controls
                                            If menco.Name = form(1) Then menco.Text = colums(0)
                                        Next
                                    Case "Form5"
                                        Try
                                            For Each menco As Control In My.Forms.Form5.Panel1.Controls
                                                If menco.Name = form(2) Then menco.Text = colums(0)
                                            Next
                                        Catch ex As Exception
                                        End Try
                                        If "ColumnHeader1" = form(1) Then My.Forms.Form5.ColumnHeader1.Text = colums(0)
                                        If "ColumnHeader2" = form(1) Then My.Forms.Form5.ColumnHeader2.Text = colums(0)
                                        If "ColumnHeader3" = form(1) Then My.Forms.Form5.ColumnHeader3.Text = colums(0)
                                        If "ColumnHeader4" = form(1) Then My.Forms.Form5.ColumnHeader4.Text = colums(0)
                                        If "ColumnHeader5" = form(1) Then My.Forms.Form5.ColumnHeader5.Text = colums(0)
                                        If "ColumnHeader6" = form(1) Then My.Forms.Form5.ColumnHeader6.Text = colums(0)
                                        If "ColumnHeader7" = form(1) Then My.Forms.Form5.ColumnHeader7.Text = colums(0)

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
    Public Sub setup1()

        If My.Settings.dialog4 <> Nothing Then
            Dim checklist() As String = Split(My.Settings.dialog4, vbCrLf)
            Try
                For x = 0 To UBound(checklist)
                    If checklist(x).EndsWith("True") Then Dialog4.CheckedListBox1.SetItemChecked(x, True)
                    If checklist(x).EndsWith("False") Then Dialog4.CheckedListBox1.SetItemChecked(x, False)
                Next
            Catch ex As Exception

            End Try



        End If
        If My.Settings.mena = String.Empty Then
            My.Settings.mena = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol

        End If
        Dialog4.TextBox1.Text = My.Settings.oktext
        Dialog4.TextBox1.BackColor = My.Settings.okbackcolor
        Dialog4.TextBox1.ForeColor = My.Settings.okforecolor
        Dialog4.TextBox1.Font = My.Settings.okfont
        Dialog4.TextBox2.BackColor = My.Settings.notokbackcolor
        Dialog4.TextBox2.Text = My.Settings.notoktext
        Dialog4.TextBox2.ForeColor = My.Settings.notokforecolor
        Dialog4.TextBox2.Font = My.Settings.notokfont
        Dialog4.TextBox3.Text = My.Settings.mailsign
        Dialog4.TextBox4.Text = My.Settings.regday
        Dialog4.TextBox5.Text = My.Settings.regweek
        Dialog4.TextBox6.Text = My.Settings.regmonth
        Dialog4.TextBox7.Text = My.Settings.regyear
        Dialog4.TextBox8.Text = My.Settings.mena

        Dialog4.ComboBox1.SelectedIndex = My.Settings.reporti
        Dialog4.NumericUpDown1.Value = My.Settings.timebarcodeidle
        If Dialog4.CheckedListBox1.GetItemCheckState(4) = CheckState.Checked Then
            Dialog9.CheckBox7.CheckState = CheckState.Checked
        End If
        
        My.Settings.pocetspusteni += 1


    End Sub
    Private Sub Login_Resize(sender As System.Object, e As System.EventArgs) Handles MyBase.Resize
        If WebBrowser1.Visible Then
            WebBrowser1.Size = New Size(My.Computer.Screen.Bounds.Width - 10, My.Computer.Screen.Bounds.Height)
            Me.SetDesktopBounds(0, 0, My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height)
            'Button3.Hide()
        End If
    End Sub
    Private Sub ComboBox1_SelectedValueChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedValueChanged
        TextBox1.Focus()
    End Sub
End Class
