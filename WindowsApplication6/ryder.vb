Imports System.Windows.Forms.DataVisualization.Charting
Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Imports System
Imports System.Text
Imports System.Collections.ObjectModel
Imports System.Net.Mail
Imports System.Globalization
Imports System.Threading
Imports System.IO.Ports
Imports System.Runtime.InteropServices
Imports System.Management
'Imports System.Security.Cryptography
Imports System.Xml


Public Class ryder
    'Dim driveSerial As String = GetDriveSerialNumber("C:")
    Dim mousepos As String
    Dim timestart As Double
    Dim timestop As Double
    Dim barcodelisti1i2 As New Dictionary(Of Integer, List(Of String))
    Dim ok As String
    Dim notok As String
    Dim indexlb2 As Integer
    Dim listtext As New List(Of String)
    Dim selectedcontrol As String
    Dim month As String = Date.Now.Month
    Public Shared day As String = Date.Now.Date
    Dim year As String = Date.Now.Year
    Public Shared cesta As String = Application.StartupPath & "\devices\1"
    Dim cestauser As String = Application.StartupPath & "\user"
    Public Shared week As String = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(Now(), Globalization.CalendarWeekRule.FirstFourDayWeek, vbFirstJan1)
    Public Shared aktivnycombobox As Byte
    Public Shared pathimgtodg2 As String = Nothing
    Public Shared hesla As New Dictionary(Of Integer, String)
    Public Shared povolenie As Boolean = False
    Public Shared user As String = Login.ComboBox1.Text
    Dim logpath As String = Application.StartupPath & "\logbarcode.txt"
    Dim notcode() As Char = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890."
    Dim deccode() As Char = "1§MNOPQhi378jklmnR5ST4UGHI20JKVWXd9eYZABCD6EFLpqrstuvwxyzabcfgo"
    Dim mainbackground As String = Application.StartupPath & "\devices\mainbackground"
    Public Shared fileContentscb As New ArrayList
    Public Shared CBpermision As New StringBuilder
    'Dim open As Boolean = False




    Public Sub polozky()
        Try


            Dim folderExists As Boolean
            ListBox6.Items.Clear()
            Dim appzar As String = Nothing
            If aktivnycombobox = 1 Then
                appzar = Application.StartupPath & "\devices\" + ComboBox1.Text + "\user file"
            ElseIf aktivnycombobox = 2 Then
                appzar = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + ComboBox9.Text + "\user file"
            End If
            If folderExists = My.Computer.FileSystem.DirectoryExists(appzar) Then
                My.Computer.FileSystem.CreateDirectory(appzar) : End If
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(appzar)
                Dim testFile As System.IO.FileInfo
                testFile = My.Computer.FileSystem.GetFileInfo(foundFile)
                ListBox6.Items.Add(testFile.Name)

            Next
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(appzar)
                If foundFile.Contains("mainpicture") And Not foundFile.Contains(".gif") And Not foundFile.Contains(".ico") Then
                    Try
                        PictureBox1.Load(foundFile)
                        SetAutoSizeMode(PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage)
                        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                    Catch ex As Exception

                    End Try
                    Exit Sub
                Else

                    Try
                        If foundFile.Contains(".gif") Or foundFile.Contains(".ico") Then GoTo dalsi
                        PictureBox1.Load(foundFile)
                        SetAutoSizeMode(PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage)

                        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                    Catch ex As Exception
                    End Try
                End If
dalsi:
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Sub barcodeini()

        Dim sNames() As String
        sNames = Split((My.Settings.device), vbCrLf)
        For Each cb As String In sNames


            Dim sssstxt As String = Application.StartupPath & "\devices\" & cb & "\label.txt"
            TextBox10.Clear() : Dim fileContentscb As String
            Dim bcode As Byte = 12
            Try
                If My.Computer.FileSystem.FileExists(sssstxt) = True Then
                    fileContentscb = My.Computer.FileSystem.ReadAllText(sssstxt)
                    Dim text() As String = Split(fileContentscb, "#")
                    Try
                        Dim bacodeint As Integer = CInt(Trim(text(bcode)))
                        Dim cb1codes As New List(Of String)
                        cb1codes.AddRange({"cb1", cb})
                        barcodelisti1i2.Add(bacodeint, cb1codes)
                    Catch ex As Exception
                        '       
                    End Try
                End If
            Catch ex As Exception

            End Try
            Try
                Dim pathcb9names = Application.StartupPath & "\devices\" & cb & "\devparts"
                If My.Computer.FileSystem.FileExists(pathcb9names) = True Then
                    Dim fileContentscb9 As String = My.Computer.FileSystem.ReadAllText(pathcb9names)
                    Dim cb9Names() As String
                    Dim x9 As Long : cb9Names = Split(fileContentscb9, vbCrLf)
                    For x9 = 0 To UBound(cb9Names)
                        If cb9Names(x9) <> String.Empty Then
                            Dim pathlabel9 As String = Application.StartupPath & "\devices\" & cb & "\" & cb9Names(x9) & "\label.txt"
                            If My.Computer.FileSystem.FileExists(pathlabel9) Then
                                Try
                                    Dim text9() As String = Split(My.Computer.FileSystem.ReadAllText(pathlabel9), "#")
                                    Dim cb9codes As New List(Of String)
                                    cb9codes.AddRange({"cb9", cb, cb9Names(x9)})
                                    barcodelisti1i2.Add(Trim(text9(bcode)), cb9codes)
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
            End Try
        Next
    End Sub
    Public Sub cbread()

        Try
            fileContentscb.Clear()
            ComboBox1.Items.Clear() : ComboBox9.Items.Clear()
            CBpermision.Clear()


            For Each founddir As String In My.Computer.FileSystem.GetDirectories(Application.StartupPath & "\devices\", FileIO.SearchOption.SearchTopLevelOnly)
                Dim dirinfo As System.IO.DirectoryInfo = My.Computer.FileSystem.GetDirectoryInfo(founddir)
                Dim trestfilname = dirinfo.Name
                If Not trestfilname.Contains("user file") Then

                    fileContentscb.Add(trestfilname)

                End If

            Next
            fileContentscb.Sort()

            Dim ItemArray1(fileContentscb.Count - 1) As Object
            fileContentscb.CopyTo(ItemArray1, 0)



            My.Settings.device = Join(ItemArray1, vbCrLf)
            Dim sNames() As String
            sNames = Split((My.Settings.device), vbCrLf)
            'MsgBox(user & vbCrLf & My.Settings.devpermision)


            If My.Settings.devpermision <> Nothing Then


                If My.Settings.devpermision.Contains(vbCrLf) And My.Settings.devpermision.Contains(user) Then
                    Dim devset() As String = Split(My.Settings.devpermision, vbCrLf)
                    For x0 = 0 To UBound(devset)
                        Dim devsetstored() As String = Split(devset(x0), "|")
                        Dim ss As Boolean = Replace(Trim(devsetstored(0)), vbCrLf, "") = Replace(Trim(user), vbCrLf, "")
                        If ss Then
                            Dim cbdevperm As New StringBuilder
                            For x = 1 To UBound(devsetstored)
                                cbdevperm.AppendLine(devsetstored(x))
                            Next
                            'MsgBox(cbdevperm.ToString)
                            For c = 0 To UBound(sNames)
                                For Each per As String In Split(cbdevperm.ToString, vbCrLf)
                                    If per = sNames(c) Then
                                        ComboBox1.Items.Add(per)
                                        CBpermision.AppendLine(per)
                                        Exit For
                                    End If
                                Next
                            Next
                        End If
                    Next

                End If
            Else
                For x = 0 To UBound(sNames)
                    If sNames(x) <> "" Then
                        ComboBox1.Items.Add(sNames(x))
                        'CBpermision.AppendLine(sNames(x))
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub partsread()
       Try
            CheckedListBox2.Items.Clear()
            CheckedListBox2.Controls.Clear()
            ListBox1.Items.Clear()
            Dim cestaparts As String = Nothing
            If aktivnycombobox = 1 Then
                cestaparts = Application.StartupPath & "\devices\" & ComboBox1.Text & "\parts"
            ElseIf aktivnycombobox = 2 Then
                cestaparts = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\parts"
            End If
            Dim fileContentscb As String
            Dim fileExists As Boolean : If fileExists = My.Computer.FileSystem.FileExists(cestaparts) Then
                My.Computer.FileSystem.WriteAllText(cestaparts, String.Empty, False)
            End If
            fileContentscb = My.Computer.FileSystem.ReadAllText(cestaparts)
            Dim sNames() As String
            Dim x As Long : sNames = Split(fileContentscb, vbCrLf)
            Try
                For x = 0 To UBound(sNames)
                    If sNames(x).Contains("|") Then
                        ListBox1.Items.Add(sNames(x))
                        Dim riadok() As String = Split(sNames(x), "|")
                        CheckedListBox2.Items.Add(riadok(1))
                        Dim label As New Label
                        Dim percenta As Integer = 100 : label.ForeColor = Color.Green
                        With label
                            .AutoSize = True
                            .Location = New Point(300, (CheckedListBox2.Items.Count - 1) * 12.9)
                            .BackColor = Color.Transparent
                        End With
                        If IsDate(riadok(0)) And IsNumeric(riadok(3)) Then
                            Dim oldDate As Date = riadok(0)
                            Dim newDate As Date = Now

                            Dim differenceInDays As Short = DateDiff(DateInterval.Day, oldDate, newDate)

                            If differenceInDays > riadok(3) Then percenta = (200 - ((100 / riadok(3)) * differenceInDays)) : label.ForeColor = Color.Firebrick
                            If percenta < 0 Then percenta = 0 : label.ForeColor = Color.Red


                            label.Text = Format(percenta, "g") & "%"
                            CheckedListBox2.Controls.Add(label)

                        End If
                        Try
                            Dim interval As Integer = riadok(5)
                            Dim poslednyzapis As Integer = riadok(7)
                            If IsNumeric(NumericUpDown1.Value) = True Then
                                percenta = (100 - ((100 / interval) * (NumericUpDown1.Text - poslednyzapis))) : label.ForeColor = Color.DodgerBlue
                                If percenta < 0 Then percenta = 0 : label.ForeColor = Color.Red

                                label.Text = Format(percenta, "g") & "%"
                                If poslednyzapis > NumericUpDown1.Value Then
                                    label.Text = "100%!"
                                    label.ForeColor = Color.Indigo
                                End If
                                CheckedListBox2.Controls.Add(label)
                            End If
                        Catch ex As Exception
                        End Try
                    Else
                    End If
                Next
            Catch ex As Exception
            End Try
        Catch ex As Exception
        End Try
    End Sub
    Public Sub cbread2()
        Try
            ComboBox9.Items.Clear() : ComboBox9.Text = Nothing
            Dim cestasuzarpriečinky As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\devparts"
            Dim fileContentslb As String = Nothing
            Dim fileExists As Boolean : If fileExists = My.Computer.FileSystem.FileExists(cestasuzarpriečinky) Then
                My.Computer.FileSystem.WriteAllText(cestasuzarpriečinky, String.Empty, False)
            Else : fileContentslb = (My.Computer.FileSystem.ReadAllText(cestasuzarpriečinky))
            End If
            Dim sNames() As String : Dim x As Long : sNames = Split(fileContentslb, vbCrLf)
            For x = 0 To UBound(sNames) : ComboBox9.Items.Add(sNames(x))

            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Sub pozadie()

        Try
            Me.Panel1.BackgroundImage = Image.FromFile(mainbackground)
        Catch ex As Exception
        End Try

    End Sub
    Public Sub colofbgandtext()
        TextBox5.BackColor = My.Settings.ryderbackgroundcolortexboxu
        ListBox5.BackColor = My.Settings.ryderbackgroundcolortexboxu
        ListView3.BackColor = My.Settings.ryderbackgroundcolortexboxu
        TextBox5.ForeColor = My.Settings.ryderforecolortext
        ListBox5.ForeColor = My.Settings.ryderforecolortext
        ListView3.ForeColor = My.Settings.ryderforecolortext
        TextBox5.Font = My.Settings.ryderfont
        ListBox5.Font = My.Settings.ryderfont
        ListView3.Font = My.Settings.ryderfont
    End Sub
    Public Sub cbuseursread()
        Try
            hesla.Clear()
            Login.ComboBox1.Items.Clear()
            ComboBox4.Items.Clear()
            Dim sNames() As String = Split(My.Settings.user, vbCrLf)
            For x = 0 To UBound(sNames)
                If sNames(x) <> String.Empty Then
                    Dim userpass() As String = Split(sNames(x), vbTab)
                    Login.ComboBox1.Items.Add(userpass(0)) : ComboBox4.Items.Add(userpass(0))
                    hesla.Add(x, userpass(1))
                End If
            Next

        Catch ex As Exception

        End Try

    End Sub
    Public Sub textovýlistb()
        Try


            Dim fileExists As Boolean : If fileExists = My.Computer.FileSystem.FileExists(cesta) Then
                My.Computer.FileSystem.WriteAllText(cesta, String.Empty, False)
            End If
            aktivnycombobox = 1
            Dim fileContents As String : fileContents = My.Computer.FileSystem.ReadAllText(cesta)
            'Dim cb1 As String = My.Settings.device
            ListBox5.Items.Clear() : listtext.Clear()
            Dim riadky() As String = Split(fileContents, vbCrLf)
            For x = 0 To UBound(riadky)

                If riadky(x) <> String.Empty Then
                    listtext.Add(riadky(x))
                    ListBox5.Items.Add(riadky(x))
                End If
            Next
            TextBox5.Text = fileContents

            Try

                ListView3.Items.Clear()
                Dim sa() = Split(fileContents, vbCrLf)

                For x As Integer = 0 To UBound(sa)
                    Try


                        If sa(x) <> String.Empty Then


                            Dim splb5() As String = Split(sa(x), "|")
                            'Dim weekdatum() As String = Split(splb5(0), "|")
                            Dim week As String = splb5(0)
                            'datum()
                            Dim datum As String = splb5(1)
                            'MsgBox(datum)
                            'od: polozky koniec
                            Try
                                If splb5(2).Contains(vbTab) Then
                                    Dim t() As String = Split(splb5(2), vbTab)
                                    'zariadenie
                                    'Dim zariadenie As String = cb1atext(0)
                                    Dim zariadenie As String = t(0)
                                    'súčasť zar " " ND " " frekvencia " "údržba " " text " "user
                                    Dim sucastzar As String = t(1)
                                    Dim ND As String = t(2)
                                    Dim frekvencia As String = t(3)
                                    Dim údržba As String = t(4)
                                    Dim text As String = t(5)
                                    Dim user As String = t(6)
                                    Dim i As Integer = ListView3.Items.Count
                                    With ListView3
                                        .Items.Add(week)
                                        .Items(i).SubItems.Add(datum)
                                        .Items(i).SubItems.Add(zariadenie)
                                        .Items(i).SubItems.Add(sucastzar)
                                        .Items(i).SubItems.Add(ND)
                                        .Items(i).SubItems.Add(frekvencia)
                                        .Items(i).SubItems.Add(údržba)
                                        .Items(i).SubItems.Add(text)
                                        .Items(i).SubItems.Add(user)
                                    End With
                                End If
                            Catch ex As Exception

                            End Try
                        End If
                    Catch ex As Exception

                    End Try
                Next
            Catch ex As Exception

            End Try
            Try
                ListBox5.SetSelected(ListBox5.Items.Count - 1, True)
                ListView3.Items(ListView3.Items.Count - 1).EnsureVisible()
            Catch ex As Exception

            End Try
            ListView3.BringToFront()
            selectedcontrol = TextBox5.Text

        Catch ex As Exception

        End Try
    End Sub
    Public Sub suboryexist()
        Dim fileexist As Boolean
        Dim denna1 As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\daily.txt"
        Dim tyzdenna1 As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\weekly.txt"
        Dim mesa1 As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\monthly.txt"
        Dim rocna1 As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\yearly.txt"
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

    End Sub
    Public Sub ryderstart()

        My.Settings.rydersizeonload = Me.Size
        If My.Settings.timebarcodeidle = Nothing Then My.Settings.timebarcodeidle = 10000
        If My.Settings.timebarcodeidle = 0 Then My.Settings.timebarcodeidle = 10000
        Timer1.Interval = My.Settings.timebarcodeidle
        Dialog4.NumericUpDown1.Value = Timer1.Interval
        If My.Settings.scanerinput = 0 Then
            bothscanners()
        ElseIf My.Settings.scanerinput = 1 Then
            directscannerinitalization()
        ElseIf My.Settings.scanerinput = 2 Then
            Timer1.Start()
        End If
        colofbgandtext()
        Me.AutoScaleMode = Windows.Forms.AutoScaleMode.Dpi
        Me.PerformAutoScale()

        If My.Computer.Network.IsAvailable = False Then LinkLabel4.Hide()
        Dim fileexist As Boolean = My.Computer.FileSystem.FileExists(Application.StartupPath & "/startup.wav")
        If fileexist = True Then
            My.Computer.Audio.Play(Application.StartupPath & "/startup.wav", AudioPlayMode.Background)
        End If
        'vytvorí novú zložku devices
        Dim apppath As String = Application.StartupPath & "\devices"
        Dim folderExists As Boolean : If folderExists = My.Computer.FileSystem.DirectoryExists(apppath) Then
            My.Computer.FileSystem.CreateDirectory(apppath)
        End If
        aktivnycombobox = 1
        cbuseursread()

        pozadie()

        For x As Integer = Date.Now.Year - 100 To Date.Now.Year : ComboBox7.Items.Add(x) : Next
        ComboBox7.SelectedIndex = 100
        ListBox5.Show()
        TextBox5.Hide()
        Me.AllowDrop = True
        'Try


        '    Dim fileExists1 As Boolean
        '    fileExists1 = My.Computer.FileSystem.FileExists(Application.StartupPath & "\book1.xls")
        '    If fileExists1 = False Then

        '        workbook = APP.Workbooks.Add(System.Reflection.Missing.Value)
        '        workbook.UnprotectSharing()
        '        APP.ActiveWorkbook.SaveAs(Filename:=Application.StartupPath & "\book1.xls")
        '        APP.ActiveWorkbook.Close()
        '        APP.Quit()

        '    End If
        '    fileExists1 = My.Computer.FileSystem.FileExists(Application.StartupPath & "\book2.xls")
        '    If fileExists1 = False Then

        '        workbook = APP.Workbooks.Add(System.Reflection.Missing.Value)
        '        APP.ActiveWorkbook.SaveAs(Filename:=Application.StartupPath & "\book2.xls")
        '        APP.ActiveWorkbook.Close()
        '        APP.Quit()
        '    End If
        'Catch ex As Exception

        'End Try

        Login.ShowDialog()



        Try
            If Dialog4.CheckedListBox1.GetItemCheckState(1) = CheckState.Unchecked Then motohodinyodhad()
            ComboBox11.Items.Add(CheckBox1.Text)
            Dialog11.ComboBox1.Items.Add(CheckBox1.Text)
            ComboBox11.Items.Add(CheckBox2.Text)
            Dialog11.ComboBox1.Items.Add(CheckBox6.Text)
            ComboBox11.Items.Add(CheckBox6.Text)
            Dialog11.ComboBox1.Items.Add(CheckBox2.Text)
            ComboBox11.Items.Add(CheckBox3.Text)
            Dialog11.ComboBox1.Items.Add(CheckBox3.Text)
            ComboBox11.Items.Add("other")
        Catch ex As Exception

        End Try
        motohodhistory()
        'Dim comporty() As String = IO.Ports.SerialPort.GetPortNames()

        'For Each com As String In comporty
        '    ComboBox12.Items.Add(com)
        'Next



        mousepos = MousePosition.ToString

        user = ComboBox4.Text
        cbread()
        'Try
        '    APP.ActiveWorkbook.Close()
        '    APP.Quit()
        'Catch ex As Exception

        'End Try
        Try
            ComboBox1.SelectedIndex = My.Settings.lstselect
        Catch ex As Exception

        End Try
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
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        appexcstart()
        If My.Settings.verzia <> My.Application.Info.Version.ToString Then
            My.Settings.Upgrade()
            My.Settings.verzia = My.Application.Info.Version.ToString
        End If
        Try
            Dim myname() As String = Nothing
            Dim executablename As String
            If Process.GetCurrentProcess().ProcessName.Contains(".") Then
                myname = Split(Process.GetCurrentProcess().ProcessName, ".")
                executablename = myname(0)
            Else
                executablename = Process.GetCurrentProcess().ProcessName
            End If

            If My.Application.Info.AssemblyName.ToString() <> executablename Then
                MsgBox(executablename & " is not valid" & vbCrLf & "please rename application file to " & vbCrLf & My.Application.Info.AssemblyName.ToString())
                Me.Close()
            End If

        Catch ex As Exception

        End Try

        ryderstart()
        'Try


        '    'textovýlistb()
        '    'Dim fdatte As Date
        '    'Dim olddate As Integer : Dim i As Integer
        '    'Dim datebool As Boolean = True
        '    'For Each item As String In listtext
        '    '    Dim line() As String = Split(item, "|")
        '    '    If IsDate(line(1)) = True Then
        '    '        fdatte = line(1)
        '    '        i = DateDiff(DateInterval.Day, fdatte, Date.Now.Date)
        '    '        If i > olddate Then olddate = i
        '    '    End If
        '    '    If i > 90 Then datebool = False
        '    'Next

        '    If My.Settings.firstday = Nothing Then
        '        My.Settings.firstday = Date.Now.Date
        '        'first let's check if there is a file MyXML.xml into our application folder
        '        'if there wasn't a file something like that, then let's create a new one.

        '        If IO.File.Exists("MyXML.xml") = False Then

        '            'declare our xmlwritersettings object
        '            Dim settings As New XmlWriterSettings()

        '            'lets tell to our xmlwritersettings that it must use indention for our xml
        '            settings.Indent = True

        '            'lets create the MyXML.xml document, the first parameter was the Path/filename of xml file
        '            ' the second parameter was our xml settings
        '            Dim XmlWrt As XmlWriter = XmlWriter.Create("MyXML.xml", settings)

        '            With XmlWrt

        '                ' Write the Xml declaration.
        '                .WriteStartDocument()

        '                ' Write a comment.
        '                .WriteComment("XML Database.")

        '                ' Write the root element.
        '                .WriteStartElement("Data")

        '                ' Start our first person.
        '                .WriteStartElement("Person")

        '                ' The person nodes.

        '                .WriteStartElement("FirstName")
        '                .WriteString("Alleo")
        '                .WriteEndElement()

        '                .WriteStartElement("LastName")
        '                .WriteString("Indong")
        '                .WriteEndElement()


        '                ' The end of this person.
        '                .WriteEndElement()

        '                ' Close the XmlTextWriter.
        '                .WriteEndDocument()
        '                .Close()

        '            End With

        '            MessageBox.Show("XML file saved.")
        '        End If

        '    End If

        '    Dim password As String = driveSerial
        '    Dim wrapper As New Simple3Des(password)
        '    Dim cipherText As String = wrapper.DecryptData(wrapper.DecryptData(My.Settings.hesloback.ToLower))

        '    Dim diffdays As Integer = (DateDiff(DateInterval.Day, My.Settings.firstday, Date.Now.Date))
        '    If My.Settings.heslo <> cipherText Or My.Settings.heslo = Nothing Then
        '        Button37.Text = Button37.Text & vbCrLf & diffdays & " days in use "

        '        If diffdays > 90 Or My.Settings.pocetspusteni > 100 Or diffdays < 0 Then

        '            Me.Opacity = 10 : Me.SendToBack() : Form10.Show() : Form10.BringToFront()
        '            'ElseIf datebool = False Then
        '            Me.Opacity = 10 : Me.SendToBack() : Form10.Show() : Form10.BringToFront()

        '        Else

        '            Button37.Visible = True
        '            ryderstart()

        '        End If
        '    ElseIf My.Settings.heslo = cipherText Then
        '        Button37.Visible = False

        '        ryderstart()

        '    End If
        'Catch ex As Exception

        'End Try
    End Sub

    'Private Function GetDriveSerialNumber(ByVal drive As String) As String

    '    Dim driveSerial As String = String.Empty
    '    Dim driveFixed As String = System.IO.Path.GetPathRoot(drive)
    '    driveFixed = Replace(driveFixed, "\", String.Empty)

    '    Using querySearch As New ManagementObjectSearcher("SELECT VolumeSerialNumber FROM Win32_LogicalDisk Where Name = '" & driveFixed & "'")

    '        Using queryCollection As ManagementObjectCollection = querySearch.Get()

    '            Dim moItem As ManagementObject

    '            For Each moItem In queryCollection

    '                driveSerial = CStr(moItem.Item("VolumeSerialNumber"))

    '                Exit For
    '            Next
    '        End Using
    '    End Using
    '    Return driveSerial
    'End Function
    'Public NotInheritable Class Simple3Des
    '    Private TripleDes As New TripleDESCryptoServiceProvider
    '    Private Function TruncateHash(
    '        ByVal key As String,
    '        ByVal length As Integer) As Byte()

    '        Dim sha1 As New SHA1CryptoServiceProvider

    '        ' Hash the key.
    '        Dim keyBytes() As Byte =
    '            System.Text.Encoding.Unicode.GetBytes(key)
    '        Dim hash() As Byte = sha1.ComputeHash(keyBytes)

    '        ' Truncate or pad the hash.
    '        ReDim Preserve hash(length - 1)
    '        Return hash
    '    End Function
    '    Sub New(ByVal key As String)
    '        ' Initialize the crypto provider.
    '        TripleDes.Key = TruncateHash(key, TripleDes.KeySize \ 8)
    '        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)
    '    End Sub

    '    Public Function EncryptData(
    '    ByVal plaintext As String) As String

    '        ' Convert the plaintext string to a byte array.
    '        Dim plaintextBytes() As Byte =
    '            System.Text.Encoding.Unicode.GetBytes(plaintext)

    '        ' Create the stream.
    '        Dim ms As New System.IO.MemoryStream
    '        ' Create the encoder to write to the stream.
    '        Dim encStream As New CryptoStream(ms,
    '            TripleDes.CreateEncryptor(),
    '            System.Security.Cryptography.CryptoStreamMode.Write)

    '        ' Use the crypto stream to write the byte array to the stream.
    '        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
    '        encStream.FlushFinalBlock()

    '        ' Convert the encrypted stream to a printable string.
    '        Return Convert.ToBase64String(ms.ToArray)
    '    End Function
    '    Public Function DecryptData(
    '    ByVal encryptedtext As String) As String

    '        ' Convert the encrypted text string to a byte array.
    '        Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

    '        ' Create the stream.
    '        Dim ms As New System.IO.MemoryStream
    '        ' Create the decoder to write to the stream.
    '        Dim decStream As New CryptoStream(ms,
    '            TripleDes.CreateDecryptor(),
    '            System.Security.Cryptography.CryptoStreamMode.Write)

    '        ' Use the crypto stream to write the byte array to the stream.
    '        decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
    '        decStream.FlushFinalBlock()

    '        ' Convert the plaintext stream to a string.
    '        Return System.Text.Encoding.Unicode.GetString(ms.ToArray)
    '    End Function
    'End Class
    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        Dim appzar As String = Application.StartupPath & "\devices\" + ComboBox1.Text + "\user file"
        Dim item = ListBox6.SelectedItem

        Dim folderExists As Boolean

        Dim cestasuboru As String = appzar + "\" + item
        If folderExists = My.Computer.FileSystem.FileExists(cestasuboru) Then
            MsgBox("file not exist")
        End If
        Try
            System.Diagnostics.Process.Start(cestasuboru)
        Catch ex As Exception
            MsgBox("program for this filetype is not installed")
        End Try

    End Sub
    Public Shared Sub forcecloseexcel()
        Dim objExcel = GetObject(, "Excel.Application")
        For Each w In objExcel.Workbooks
            If w.name.contains("book1") Then
                w.Application.Save()
                w.Application.Close(SaveChanges:=True)
                w.Quit()

                Marshal.ReleaseComObject(w)
                w = Nothing
            End If
        Next
        Try
            For Each wb In APP.Workbooks
                For Each ws In wb.Worksheets
                    Marshal.ReleaseComObject(ws)
                    ws = Nothing
                Next
                wb.Close(False)
                Marshal.ReleaseComObject(wb)
                wb = Nothing
            Next
        Catch ex As Exception

        End Try


    End Sub
    Public Shared Sub excelopenfile()

        Try
            Try
                Dim objExcel = GetObject(, "EXCEL.Application")
                If objExcel.Workbooks.Count > 0 Then
                    forcecloseexcel()
                End If
            Catch ex As Exception

            End Try

            APP = CreateObject("Excel.Application")
            Dim book1path As String = Application.StartupPath & "\book1." & excelextension
            If My.Computer.FileSystem.FileExists(book1path) = False Then
                If My.Computer.FileSystem.FileExists(Application.StartupPath & "\book1.xls") And excelextension = "xlsx" Then
                    workbook = APP.Workbooks.Open(Application.StartupPath & "\book1.xls")
                    workbook.SaveAs(book1path)
                    My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\book1.xls")
                Else
                    workbook = APP.Workbooks.Add
                    workbook.SaveAs(book1path)
                End If

            End If
            If My.Computer.FileSystem.FileExists(book1path) = False Then
                workbook = APP.Workbooks.Add
                workbook.SaveAs(book1path)
            End If


            workbook = APP.Workbooks.Open(book1path)
            worksheet = workbook.Sheets.Item(1)

            Try
                Dim licol As Integer = Form6.ListView3.Columns.Count - 1
                For x As Integer = 0 To licol
                    worksheet.Cells(1, x + 1).Value = Form6.ListView3.Columns(x).Text
                Next

            Catch ex As Exception

            End Try
        Catch ex As Exception


        End Try
    End Sub
    Public Sub zapistb2()
        Try
            Dim datenow As Date = Date.Now
            Dim vybraneitems As New StringBuilder
            'zapíše do text 1 spravu s dátumom a zariadením
            'ListBox5.Show() : ListBox5.BringToFront() : TextBox5.SendToBack()
            Dim cb As String = ComboBox1.Text : If cb = " " Then GoTo 30
            Dim kl As String = Nothing
            If CheckBox1.Checked() = True Then : kl = CheckBox1.Text
            ElseIf CheckBox6.Checked() = True Then : kl = CheckBox6.Text
            ElseIf CheckBox2.Checked() = True Then : kl = CheckBox2.Text
            ElseIf CheckBox3.Checked() = True Then : kl = CheckBox3.Text
            Else : kl = "outplan"
            End If
            Dim cestaparts As String = Nothing
            If aktivnycombobox = 1 Then
                cestaparts = Application.StartupPath & "\devices\" & ComboBox1.Text & "\parts"
            ElseIf aktivnycombobox = 2 Then
                cestaparts = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\parts"
            End If


            Dim novydtND As String = Nothing
            Dim fileExists As Boolean : If fileExists = My.Computer.FileSystem.FileExists(cestaparts) Then
                My.Computer.FileSystem.WriteAllText(cestaparts, String.Empty, False)
            End If
            Dim fileContentsND As String = Nothing

            For Each itemChecked As String In CheckedListBox2.CheckedItems
                fileContentsND = My.Computer.FileSystem.ReadAllText(cestaparts)

                Dim snames() As String = Split(fileContentsND, vbCrLf)

                Dim cenaačasvymeny As String = " |$|0 |TTE|0"
                Dim info As String = " |O.Nu| |MD|"
                vybraneitems.Append(itemChecked)
                Dim i As Integer = CheckedListBox2.Items.IndexOf(itemChecked).ToString()
                Try
                    My.Computer.FileSystem.WriteAllText(cestaparts, String.Empty, False)
                    For pozicia = 0 To UBound(snames)
                        If snames(Trim(pozicia)).Contains(Trim(itemChecked)) And pozicia.Equals(i) Then
                            Dim ND() As String = Split(snames(pozicia), "|")

                            Try
                                cenaačasvymeny = " |$|" & ND(9) & " |TTE|" & ND(11)
                            Catch ex As Exception
                            End Try
                            Try
                                info = " |O.Nu|" & ND(13) & " |MD|" & ND(15)
                            Catch ex As Exception

                            End Try
                            novydtND = datenow.Date & "|" & ND(1) & " |IV|" & ND(3) & " |MH|" & ND(5) & "|MHL|" & NumericUpDown1.Value & cenaačasvymeny & info
                        Else : novydtND = snames(pozicia)
                        End If
                        If novydtND <> Nothing Then My.Computer.FileSystem.WriteAllText(cestaparts, novydtND + vbCrLf, True)
                    Next
                Catch ex As Exception
                End Try

            Next

            Dim tb As String = week & "|" & datenow & "|" & Space(2) & ComboBox1.Text & vbTab & ComboBox9.Text & vbTab & vybraneitems.ToString & vbTab & kl & vbTab & ListBox3.SelectedItem & vbTab & TextBox1.Text & vbTab & user & vbNewLine
            Try
                If Dialog4.CheckedListBox1.GetItemCheckState(0) = CheckState.Unchecked Then
                    excelopenfile()
                    Dim LastRow As Long
                    With worksheet
                        LastRow = .Cells(.Rows.Count, 2).End(XlDirectionxlUP).Row
                    End With
                    worksheet.Cells(LastRow + 1, 1).Value = week
                    worksheet.Cells(LastRow + 1, 2).Value = datenow
                    worksheet.Cells(LastRow + 1, 3).Value = ComboBox1.Text & vbTab & ComboBox9.Text
                    worksheet.Cells(LastRow + 1, 4).Value = ListBox3.SelectedItem
                    worksheet.Cells(LastRow + 1, 5).Value = vybraneitems.ToString
                    worksheet.Cells(LastRow + 1, 6).Value = TextBox1.Text
                    worksheet.Cells(LastRow + 1, 7).Value = user
                    worksheet.Cells(LastRow + 1, 8).Value = kl
                    worksheet.Columns("A:Q").EntireColumn.Autofit()
                    APP.DisplayAlerts = True
                    APP.ActiveWorkbook.Save()
                    APP.ActiveWorkbook.Close()
                    APP.Quit()
                    releaseobject(APP)
                    releaseobject(workbook)
                    releaseobject(worksheet)

                End If
            Catch ex As Exception

            End Try

            TextBox1.Clear()
            My.Computer.FileSystem.WriteAllText(cesta, tb, True)
            'vytvorá nový textfile pre zariadenie
            Dim cestazar As String = Application.StartupPath & "\devices\" & cb & "\" & cb
            My.Computer.FileSystem.WriteAllText(cestazar, tb, True)
            'natiahne text do text boxu

30:
            textovýlistb()
            partsread()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        zapistb2()
    End Sub
    Public Sub newpermisiondev()
        Try
            Dim zhoda As Boolean = False
            Dim newpermision As New StringBuilder
            If My.Settings.devpermision.Contains(vbCrLf) Then
                Dim devper() As String = Split(My.Settings.devpermision, vbCrLf)
                Dim i As Integer = 0
                For y As Integer = 0 To UBound(devper)
                    Dim userdev() As String = Split(devper(y), "|")
                    If userdev(0) = (Trim(user)) And userdev(0) <> String.Empty Then
                        newpermision.Append(devper(y) & "|" & ComboBox1.Text & vbCrLf)
                        zhoda = True
                    Else
                        newpermision.Append(devper(y) & vbCrLf)
                    End If
                Next
                If zhoda = False Then newpermision.Append(user & "|" & ComboBox1.Text & vbCrLf)
                My.Settings.devpermision = newpermision.ToString
            ElseIf My.Settings.devpermision = Nothing Then
                My.Settings.devpermision = user & "|" & ComboBox1.Text & vbCrLf

            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'zapíše do text2.txt z combo boxu bo zatlačení button 2.
        Dim datum As String = day
        Dim ssss As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\user file"
        Dim sssstxt As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\label.txt"
        Dim cbnewline As String = ComboBox1.Text
        'Dim selectindexa As Integer = ComboBox1.SelectedIndex
        For Each c In IO.Path.GetInvalidFileNameChars
            If cbnewline.Contains(c) Then cbnewline = cbnewline.Replace(c, "-")
        Next

        Dim folderExists As Boolean
        Dim cestazarpriečinkycb As String = Application.StartupPath & "\devices\" & cbnewline
        If folderExists = My.Computer.FileSystem.DirectoryExists(cestazarpriečinkycb) Then
            My.Computer.FileSystem.CreateDirectory(cestazarpriečinkycb)
            My.Computer.FileSystem.CreateDirectory(ssss)
        End If
        Dim textz As String = ComboBox1.Text
        Dim fileContents As String
        Dim fileexistss As Boolean
        Try
            If fileexistss = My.Computer.FileSystem.FileExists(sssstxt) = False Then
                My.Computer.FileSystem.WriteAllText(sssstxt, day & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf, False)
            End If
            fileContents = My.Computer.FileSystem.ReadAllText(sssstxt)

            Dim text() As String = Split(fileContents, "#")
            Dim dat As Byte = 0 : Dim mt As Byte = 10 : Dim dennemth As Byte = 11
            If Trim(text(mt)) >= NumericUpDown1.Value Then
                datum = Trim(text(dat))
            Else
                datum = day
            End If
        Catch ex As Exception
        End Try
        newpermisiondev()
        Dim stitok As String = datum & vbCrLf & "#" & Trim(TextBox10.Text) & vbCrLf & "#" & Trim(TextBox11.Text) & vbCrLf & "#" & Trim(TextBox12.Text) & vbCrLf & "#" & Trim(TextBox13.Text) & vbCrLf & "#" & Trim(TextBox14.Text) & vbCrLf & "#" & Trim(TextBox15.Text) & vbCrLf & "#" & Trim(TextBox16.Text) & vbCrLf & "#" & Trim(TextBox17.Text) & vbCrLf & "#" & Trim(TextBox19.Text) & vbCrLf & "#" & NumericUpDown1.Value & vbCrLf & "#" & NumericUpDown2.Value & vbCrLf & "#"
        If cbnewline = " " Then GoTo 30

        Dim fileContentscb As String = My.Settings.device
        Dim sNames() As String
        Dim x As Long : sNames = Split(fileContentscb, vbCrLf)
        For x = 0 To UBound(sNames)
            If sNames(x).Equals(textz) Then GoTo 30
        Next
        If ComboBox1.Text <> String.Empty Then
            ComboBox1.Items.Add(ComboBox1.Text)
            My.Settings.device = My.Settings.device & cbnewline & vbCrLf
        End If
30:
        My.Computer.FileSystem.WriteAllText(sssstxt, stitok, False)
        Button2.Visible = False : Button21.Visible = False
        suboryexist()
        cbread()
        'If selectindexa < 0 Then selectindexa = ComboBox1.Items.Count - 1

        ComboBox1.SelectedIndex = (ComboBox1.FindStringExact(cbnewline))
    End Sub
    Public Sub cb1vypis()
          Try

            TextBox10.Clear() : TextBox11.Clear() : TextBox12.Clear() : TextBox13.Clear() : TextBox14.Clear()
            TextBox15.Clear() : TextBox16.Clear() : TextBox17.Clear() : TextBox19.Clear() : TextBox17.Clear()
            Label17.Text = "" : NumericUpDown1.Value = 0 : NumericUpDown2.Value = 24
            If CheckBox1.Checked() = True Then : denna()
            ElseIf CheckBox6.Checked() = True Then : tyzdenna()
            ElseIf CheckBox2.Checked() = True Then : mesacna()
            ElseIf CheckBox3.Checked() = True Then : rocna()
            ElseIf CheckBox1.Checked() = False And CheckBox2.Checked() = False And CheckBox3.Checked() = False And CheckBox6.Checked() = False Then
                GoTo ndd
            End If
ndd:
            Button4.Visible = True
            aktivnycombobox = 1
            'vypíše záznamy zariadenie vybraté v comboboxe
            ListBox5.Items.Clear() : listtext.Clear() : CheckedListBox2.Items.Clear() : Chart1.Visible = False
            PictureBox1.Image = Nothing
            Dim cb As String = ComboBox1.Text
            If cb.Contains("/") Or cb.Contains("<") Or cb.Contains(">") Or cb.Contains("*") Or cb.Contains(":") Or cb.Contains("!") Or cb.Contains("?") Then MsgBox("/<>*:!?: not posible") : Exit Sub
            If cb <> String.Empty Then
                Dim cestazar As String = Application.StartupPath & "\devices\" & cb & "\" & cb
                Dim fileContents As String
                Dim fileExists As Boolean
                'Dim directoryExists As Boolean
                'If directoryExists = My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\devices\" & cb) Then
                '    MsgBox(cb & "doesnt exist)
                '    'My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\devices\" & cb)
                'End If

                Try
                    If fileExists = My.Computer.FileSystem.FileExists(cestazar) Then
                        My.Computer.FileSystem.WriteAllText(cestazar, String.Empty, False)
                    End If
                Catch ex As Exception

                End Try
                Try
                    ListView3.Items.Clear()

                    fileContents = My.Computer.FileSystem.ReadAllText(cestazar)
                    Dim riadky() As String = Split(fileContents, vbCrLf)
                    For x = 0 To UBound(riadky)
                        Try


                            If riadky(x) <> String.Empty Then
                                ListBox5.Items.Add(riadky(x))
                                listtext.Add(riadky(x))
                                Dim splb5() As String = Split(riadky(x), "|")
                                Dim week As String = splb5(0) : Dim datumlv3 As String = splb5(1)
                                If splb5(2).Contains(vbTab) Then
                                    Dim t() As String = Split(splb5(2), vbTab)
                                    If t.Count > 2 Then
                                        Dim zariadenie As String = t(0) : Dim i As Integer = ListView3.Items.Count
                                        Dim sucastzar As String = t(1) : Dim ND As String = t(2) : Dim frekvencia As String = t(3)
                                        Dim údržba As String = t(4) : Dim text As String = t(5) : Dim user As String = t(6)
                                        With ListView3 : .Items.Add(week) : .Items(i).SubItems.Add(datumlv3) : .Items(i).SubItems.Add(zariadenie)
                                            .Items(i).SubItems.Add(sucastzar) : .Items(i).SubItems.Add(ND) : .Items(i).SubItems.Add(frekvencia)
                                            .Items(i).SubItems.Add(údržba) : .Items(i).SubItems.Add(text) : .Items(i).SubItems.Add(user)
                                        End With
                                    End If : End If : End If : Catch ex As Exception : End Try : Next
                    TextBox5.Clear()
                    TextBox5.Text = fileContents
                    'štitokvé údaje
                Catch ex As Exception
                    '    MsgBox(ex.ToString)
                End Try
                Dim sssstxt As String = Application.StartupPath & "\devices\" & cb & "\label.txt"
                TextBox10.Clear() : Dim fileContentscb As String
                Dim datum As Byte = 0 : Dim dat As Byte = 1 : Dim SN As Byte = 2 : Dim výkon As Byte = 3
                Dim loc As Byte = 4 : Dim ST As Byte = 5 : Dim adr1 As Byte = 6 : Dim adr2 As Byte = 7
                Dim mail As Byte = 8 : Dim typ As Byte = 9 : Dim mt As Byte = 10 : Dim mtdenne As Byte = 11
                Dim bcode As Byte = 12
                Try


                    If fileExists = My.Computer.FileSystem.FileExists(sssstxt) Then
                        My.Computer.FileSystem.WriteAllText(sssstxt, _
                     day & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & _
                     vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & _
                     vbCrLf & "#" & vbCrLf & "#" & vbCrLf, False)
                    End If
                    fileContentscb = My.Computer.FileSystem.ReadAllText(sssstxt)
                    Dim text() As String = Split(fileContentscb, "#")

                    Label17.Text = Trim(text(datum)) : TextBox10.Text = Trim(text(dat)) : TextBox11.Text = Trim(text(SN))
                    TextBox12.Text = Trim(text(výkon)) : TextBox13.Text = Trim(text(loc)) : TextBox14.Text = Trim(text(ST))
                    TextBox15.Text = Trim(text(adr1)) : TextBox16.Text = Trim(text(adr2)) : TextBox17.Text = Trim(text(mail))
                    : TextBox19.Text = Trim(text(typ)) : Try : NumericUpDown1.Value = CInt(Trim(text(mt))) : Catch ex As Exception : End Try : Try : NumericUpDown2.Value = CInt(Trim(text(mtdenne))) : Catch ex As Exception : End Try

                Catch ex As Exception

                End Try

                ''do lb2 vypíše parts
                partsread()
                ''vypíše záznamy sučastí devices do cb9
                cbread2()
                ''vypíše subory kpriložené  zariadeniu
                polozky()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ComboBox9.Items.Clear()
        ComboBox9.Text = Nothing
        cb1vypis()

    End Sub

    Private Sub ComboBox1_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        ComboBox9.Items.Clear()
        ComboBox9.Text = Nothing
        cb1vypis()

    End Sub


    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'vymaže zariadenie file a vytvorí nový text 2. s novými údajmi z combo.boxu
        Dim cb1 As String = ComboBox1.SelectedItem
        If MsgBox(Button4.Text & vbTab & cb1 & " ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            Dim cestazar As String = Application.StartupPath & "\devices\" & cb1
            Dim dirExists As Boolean = My.Computer.FileSystem.DirectoryExists(cestazar)
            If dirExists = True Then
                Try
                    My.Computer.FileSystem.DeleteDirectory(Application.StartupPath & "\devices\" & cb1, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
            End If
            ComboBox1.Items.Remove(cb1)

            My.Settings.device.Replace(vbCrLf & cb1 & vbCrLf, vbCrLf)
            Button4.Visible = False
        End If
    End Sub
    'tu začin chechbox 
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim cb As String = ComboBox1.Text : Dim kl As String = Nothing : Dim denna1 As String = "daily.txt"
        Dim mesa1 As String = "monthly.txt" : Dim rocna1 As String = "yearly.txt"
        Dim tyzdenna1 As String = "weekly.txt" : Dim textúdržby As String = TextBox3.Text
        If CheckBox1.Checked() = True Then : kl = denna1
        ElseIf CheckBox6.Checked() = True Then : kl = tyzdenna1
        ElseIf CheckBox2.Checked() = True Then : kl = mesa1
        ElseIf CheckBox3.Checked() = True Then : kl = rocna1
        ElseIf CheckBox1.Checked() = False And CheckBox2.Checked() = False And CheckBox3.Checked() = False And CheckBox6.Checked() = False Then
            GoTo eb
        End If
        Dim cestazarpriečinkykl As String = Application.StartupPath & "\devices\" + cb + "\" + kl
        Dim pocetriadkov As Integer = TextBox3.Lines.Length
        If pocetriadkov > 50 Then MsgBox("limit 50 " & " < " & pocetriadkov) : Exit Sub
        My.Computer.FileSystem.WriteAllText(cestazarpriečinkykl, textúdržby, False)
        ListBox3.Show() : TextBox3.Hide() : Button6.Hide()
        If CheckBox1.Checked() = True Then : denna()
        ElseIf CheckBox6.Checked() = True Then : tyzdenna()
        ElseIf CheckBox2.Checked() = True Then : mesacna()
        ElseIf CheckBox3.Checked() = True Then : rocna()
        End If
eb:
    End Sub
    Public Sub denna()
        Try
            Dim cestazarpriečinky As String = Application.StartupPath & "\devices\" + ComboBox1.Text
            Dim cestazarkld As String = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + "daily.txt"
            ListBox3.Items.Clear() : TextBox3.Clear()
            Dim folderExists As Boolean
            Dim fileExists As Boolean
            If folderExists = My.Computer.FileSystem.DirectoryExists(cestazarpriečinky) Then
                My.Computer.FileSystem.CreateDirectory(cestazarpriečinky)
            End If
            If fileExists = My.Computer.FileSystem.FileExists(cestazarkld) Then
                My.Computer.FileSystem.WriteAllText(cestazarkld, String.Empty, False)
            End If
            TextBox3.Text = My.Computer.FileSystem.ReadAllText(cestazarkld)
            TextBox3.SendToBack() : ListBox3.BringToFront()
            Dim fileContentslb As String : fileContentslb = (My.Computer.FileSystem.ReadAllText(cestazarkld))
            Dim sNames() As String : Dim x As Long : sNames = Split(fileContentslb, vbCrLf)
            For x = 0 To UBound(sNames) : ListBox3.Items.Add(sNames(x))
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Sub tyzdenna()
        Try
            Dim cestazarpriečinky As String = Application.StartupPath & "\devices\" + ComboBox1.Text
            Dim cestazarkld As String = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + "weekly.txt"
            ListBox3.Items.Clear() : TextBox3.Clear()
            Dim folderExists As Boolean
            Dim fileExists As Boolean
            If folderExists = My.Computer.FileSystem.DirectoryExists(cestazarpriečinky) Then
                My.Computer.FileSystem.CreateDirectory(cestazarpriečinky)
            End If
            If fileExists = My.Computer.FileSystem.FileExists(cestazarkld) Then
                My.Computer.FileSystem.WriteAllText(cestazarkld, String.Empty, False)
            End If
            TextBox3.Text = My.Computer.FileSystem.ReadAllText(cestazarkld)
            TextBox3.SendToBack() : ListBox3.BringToFront()
            Dim fileContentslb As String : fileContentslb = (My.Computer.FileSystem.ReadAllText(cestazarkld))
            Dim sNames() As String : Dim x As Long : sNames = Split(fileContentslb, vbCrLf)
            For x = 0 To UBound(sNames) : ListBox3.Items.Add(sNames(x))
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Sub mesacna()
        Try
            Dim cestazarpriečinky As String = Application.StartupPath & "\devices\" + ComboBox1.Text
            Dim cestazarklm As String = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + "monthly.txt"
            ListBox3.Items.Clear() : TextBox3.Clear()
            Dim folderExists As Boolean
            Dim fileExists As Boolean
            If folderExists = My.Computer.FileSystem.DirectoryExists(cestazarpriečinky) Then
                My.Computer.FileSystem.CreateDirectory(cestazarpriečinky)
            End If
            If fileExists = My.Computer.FileSystem.FileExists(cestazarklm) Then
                My.Computer.FileSystem.WriteAllText(cestazarklm, String.Empty, False)
            End If
            TextBox3.Text = My.Computer.FileSystem.ReadAllText(cestazarklm)
            TextBox3.SendToBack() : ListBox3.BringToFront()
            Dim fileContentslb As String : fileContentslb = (My.Computer.FileSystem.ReadAllText(cestazarklm))
            Dim sNames() As String : Dim x As Long : sNames = Split(fileContentslb, vbCrLf)
            For x = 0 To UBound(sNames) : ListBox3.Items.Add(sNames(x))
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Sub rocna()
        Try
            Dim cestazarpriečinky As String = Application.StartupPath & "\devices\" + ComboBox1.Text
            Dim cestazarklr As String = Application.StartupPath & "\devices\" + ComboBox1.Text & "\" + "yearly.txt"
            ListBox3.Items.Clear() : TextBox3.Clear()
            Dim folderExists As Boolean
            Dim fileExists As Boolean
            If folderExists = My.Computer.FileSystem.DirectoryExists(cestazarpriečinky) Then
                My.Computer.FileSystem.CreateDirectory(cestazarpriečinky)
            End If
            If fileExists = My.Computer.FileSystem.FileExists(cestazarklr) Then
                My.Computer.FileSystem.WriteAllText(cestazarklr, String.Empty, False)
            End If
            TextBox3.Text = My.Computer.FileSystem.ReadAllText(cestazarklr)
            TextBox3.SendToBack() : ListBox3.BringToFront()
            Dim fileContentslb As String : fileContentslb = (My.Computer.FileSystem.ReadAllText(cestazarklr))
            Dim sNames() As String : Dim x As Long : sNames = Split(fileContentslb, vbCrLf)
            For x = 0 To UBound(sNames) : ListBox3.Items.Add(sNames(x))
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Try
            If CheckBox1.Checked = True Then
                CheckBox2.CheckState = CheckState.Unchecked
                CheckBox3.CheckState = CheckState.Unchecked : CheckBox6.CheckState = CheckState.Unchecked
                denna()
                ComboBox8.Items.Clear() : For x As Integer = 1 To 31 : ComboBox8.Items.Add(x) : Next : ComboBox8.SelectedIndex = ComboBox8.Items.Count - 1
                ComboBox5.Items.Clear() : For x As Integer = 1 To 31 : ComboBox5.Items.Add(x) : Next : ComboBox5.SelectedIndex = 0
                ComboBox2.SelectedIndex = 1
            End If
        Catch ex As Exception
        End Try

    End Sub
    Private Sub CheckBox6_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox6.CheckedChanged
        Try
            If CheckBox6.Checked = True Then
                CheckBox1.CheckState = CheckState.Unchecked
                CheckBox2.CheckState = CheckState.Unchecked : CheckBox3.CheckState = CheckState.Unchecked
                ComboBox8.Items.Clear() : For x As Integer = 1 To 52 : ComboBox8.Items.Add(x) : Next : ComboBox8.SelectedIndex = ComboBox8.Items.Count - 1
                ComboBox5.Items.Clear() : For x As Integer = 1 To 52 : ComboBox5.Items.Add(x) : Next : ComboBox5.SelectedIndex = ComboBox8.Items.Count - 52
                tyzdenna() : ComboBox2.SelectedIndex = 2
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        Try
            If CheckBox2.Checked = True Then
                CheckBox1.CheckState = CheckState.Unchecked
                CheckBox3.CheckState = CheckState.Unchecked : CheckBox6.CheckState = CheckState.Unchecked
                mesacna() : ComboBox2.SelectedIndex = 3
                ComboBox8.Items.Clear() : For x As Integer = 1 To 12 : ComboBox8.Items.Add(x) : Next : ComboBox8.SelectedIndex = ComboBox8.Items.Count - 1
                ComboBox5.Items.Clear() : For x As Integer = 1 To 12 : ComboBox5.Items.Add(x) : Next : ComboBox5.SelectedIndex = 0
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        Try
            If CheckBox3.Checked = True Then
                CheckBox1.CheckState = CheckState.Unchecked
                CheckBox2.CheckState = CheckState.Unchecked : CheckBox6.CheckState = CheckState.Unchecked
                rocna()
                ComboBox8.Items.Clear() : For x As Integer = Date.Now.Year - 100 To Date.Now.Year : ComboBox8.Items.Add(x) : Next : ComboBox8.SelectedIndex = ComboBox8.Items.Count - 1
                ComboBox5.Items.Clear() : For x As Integer = Date.Now.Year - 100 To Date.Now.Year : ComboBox5.Items.Add(x) : Next : ComboBox5.SelectedIndex = ComboBox8.Items.Count - 10
                ComboBox2.SelectedIndex = 4
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CheckBox8_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.CheckState = CheckState.Unchecked Then TextBox6.Hide() : TextBox6.SendToBack() : TextBox7.Hide() : TextBox7.SendToBack() : CheckBox8.BackColor = Color.Transparent
        If CheckBox8.CheckState = CheckState.Checked Then TextBox6.Show() : TextBox6.BringToFront() : TextBox7.Show() : TextBox7.BringToFront() : CheckBox8.BackColor = Color.Brown
    End Sub



    Private Sub ListBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox3.DoubleClick
        ListBox3.Visible = False : TextBox3.Visible = True : Button6.Visible = True
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        'vymaže zariadenmie s listbox 2
        Dim pozicia As Integer = CheckedListBox2.SelectedIndex
        CheckedListBox2.Items.Remove(CheckedListBox2.SelectedItem)
        ListBox1.SetSelected(pozicia, True)
        ListBox1.Items.Remove(ListBox1.SelectedItem)
        Dim cestasuzarpriečinky As String = Nothing
        If aktivnycombobox = 1 Then
            cestasuzarpriečinky = Application.StartupPath & "\devices\" + ComboBox1.Text + "\parts"
        ElseIf aktivnycombobox = 2 Then
            cestasuzarpriečinky = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + ComboBox9.Text + "\parts"
        End If

        My.Computer.FileSystem.WriteAllText(cestasuzarpriečinky, String.Empty, False)
        Dim pocet As Integer = CheckedListBox2.Items.Count : For x As Integer = 0 To pocet - 1 : ListBox1.SetSelected(x, True)

            My.Computer.FileSystem.WriteAllText(cestasuzarpriečinky, ListBox1.SelectedItem + vbCrLf, True)
        Next
        Button10.Hide()
        partsread()
    End Sub



    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'urobi kopiu file do priečinku devices 
        Try
            Dim jj As String = Nothing
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim fileloc As String = OpenFileDialog1.FileName
                Dim pripona() As String = Split(My.Computer.FileSystem.GetName(fileloc), ".")
                Dialog1.ComboBox1.Items.Add(pripona(0))
                If CheckBox9.CheckState = CheckState.Unchecked Then
                    If Dialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                        jj = Dialog1.ComboBox1.Text
                        For Each c In IO.Path.GetInvalidFileNameChars
                            If jj.Contains(c) Then jj = jj.Replace(c, "-")
                        Next
                        Dim cestazarpriečinkyjj As String = Nothing
                        If aktivnycombobox = 1 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & "user file" & "\" & jj & "." & pripona(1)
                        ElseIf aktivnycombobox = 2 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\" & "user file" & "\" & jj & "." & pripona(1)
                        End If
                        FileCopy(fileloc, cestazarpriečinkyjj)
                        Dialog1.ComboBox1.Items.Clear()
                    End If
                    'MsgBox(fileloc) : MsgBox(cestazarpriečinkyjj)
                ElseIf CheckBox9.CheckState = CheckState.Checked Then
                    jj = pripona(0)
                    Dim cestazarpriečinkyjj As String = Nothing
                    If aktivnycombobox = 1 Then
                        cestazarpriečinkyjj = Application.StartupPath & "\devices\" & ComboBox1.Text + "\" & "user file" & "\" & jj & "." & pripona(1)
                    ElseIf aktivnycombobox = 2 Then
                        cestazarpriečinkyjj = Application.StartupPath & "\devices\" & ComboBox1.Text + "\" & ComboBox9.Text & "\" & "user file" & "\" & jj & "." & pripona(1)
                    End If
                    FileCopy(fileloc, cestazarpriečinkyjj)
                Else : GoTo Koniec
                End If
            End If
koniec:
        Catch ex As Exception

        End Try
        polozky()
    End Sub


    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        TextBox5.Clear()


        Dim filter As String = Nothing
        If ComboBox2.SelectedIndex = 1 Then : ComboBox3.Items.Clear() : ComboBox7.Show() : ComboBox10.Items.Clear()
            For x As Integer = 1 To Date.DaysInMonth(year, month) : ComboBox3.Items.Add(x) : Next
            ComboBox3.SelectedIndex = Date.Now.Day - 1
            ComboBox10.Items.Clear()
            For x As Integer = 1 To 12 : ComboBox10.Items.Add(x) : Next
            ComboBox10.SelectedIndex = Date.Now.Month - 1
        ElseIf ComboBox2.SelectedIndex = 2 Then : ComboBox3.Items.Clear() : ComboBox10.Show() : ComboBox10.Items.Clear()
            For x As Integer = 1 To 52 : ComboBox3.Items.Add(x) : ComboBox10.Items.Add(x) : Next
            ComboBox3.SelectedIndex = week - 1 : ComboBox10.SelectedIndex = week - 1

        ElseIf ComboBox2.SelectedIndex = 3 Then : ComboBox3.Items.Clear() : ComboBox7.Show() : ComboBox10.Items.Clear()
            For x As Integer = 1 To 12 : ComboBox3.Items.Add(x) : ComboBox10.Items.Add(x) : Next
            ComboBox3.SelectedIndex = Date.Now.Month - 1
            : ComboBox10.SelectedIndex = Date.Now.Month - 1

        ElseIf ComboBox2.SelectedIndex = 4 Then : ComboBox3.Items.Clear() : ComboBox7.Hide() : ComboBox10.Items.Clear()
            For x As Integer = Date.Now.Year - 100 To Date.Now.Year : ComboBox3.Items.Add(x)
                ComboBox10.Items.Add(x) : Next
            ComboBox3.SelectedIndex = 100 : ComboBox10.SelectedIndex = 100
        ElseIf ComboBox2.SelectedIndex = 0 Then : ComboBox3.Items.Clear() : ComboBox10.Items.Clear() : ComboBox7.Show()
            : ComboBox3.Text = Nothing : ComboBox10.Text = Nothing
            textovýlistb()


        End If
        selectedcontrol = TextBox5.Text

    End Sub
    Private Sub ComboBox11_TextUpdate(sender As System.Object, e As System.EventArgs) Handles ComboBox11.TextUpdate
        Button41.Visible = True
    End Sub

    Private Sub Button41_Click(sender As System.Object, e As System.EventArgs) Handles Button41.Click
        freqfilter()
        Button41.Visible = False
    End Sub
    Private Sub ComboBox11_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox11.SelectedIndexChanged
        freqfilter()
    End Sub
    Public Sub freqfilter()
        TextBox5.Clear() : ListView3.Items.Clear() : ListBox5.Items.Clear()
        Dim cb2 As String = ComboBox2.Text : Dim xx3 As Integer = ComboBox3.SelectedItem
        Dim xx10 As Integer = ComboBox10.SelectedItem : Dim filter As String = Nothing
        filter = ComboBox11.Text.ToLower
        For Each item As String In listtext
            Dim rovnasa As Boolean = False
            Dim vyhladavac As Boolean
            If ComboBox11.SelectedIndex < 4 Then
                vyhladavac = (item.ToLower.Contains(filter))

            ElseIf ComboBox11.SelectedIndex = 4 Then

                'MsgBox(ComboBox11.Items(0).ToString.ToLower)

                vyhladavac = (item.ToLower.Contains(ComboBox11.Items(0).ToString.ToLower)) Or (item.ToLower.Contains(ComboBox11.Items(1).ToString.ToLower)) Or (item.ToLower.Contains(ComboBox11.Items(2).ToString.ToLower)) Or (item.ToLower.Contains(ComboBox11.Items(3).ToString.ToLower))
            End If


            If vyhladavac = True Then

                For Each word In item.Split(" ")

                    If word.ToLower.Equals(filter) Then rovnasa = True
                Next
                For Each word In item.Split(vbTab)

                    If word.ToLower.Equals(filter) Then rovnasa = True
                Next
                If rovnasa = True Then
                    TextBox5.AppendText(item & vbCrLf)
                    ListBox5.Items.Add(item)
                    Dim splb5() As String = Split(item, "|")
                    Dim week As String = splb5(0) : Dim datum As String = splb5(1)
                    Try
                        If splb5(2).Contains(vbTab) Then
                            Dim t() As String = Split(splb5(2), vbTab)
                            If t.Count > 2 Then
                                Dim zariadenie As String = t(0) : Dim i As Integer = ListView3.Items.Count
                                Dim sucastzar As String = t(1) : Dim ND As String = t(2) : Dim frekvencia As String = t(3)
                                Dim údržba As String = t(4) : Dim text As String = t(5) : Dim user As String = t(6)
                                With ListView3 : .Items.Add(week) : .Items(i).SubItems.Add(datum) : .Items(i).SubItems.Add(zariadenie)
                                    .Items(i).SubItems.Add(sucastzar) : .Items(i).SubItems.Add(ND) : .Items(i).SubItems.Add(frekvencia)
                                    .Items(i).SubItems.Add(údržba) : .Items(i).SubItems.Add(text) : .Items(i).SubItems.Add(user)
                                End With
                            End If : End If : Catch ex As Exception : End Try : End If
            ElseIf vyhladavac = False And ComboBox11.SelectedIndex = 4 Then


                TextBox5.AppendText(item & vbCrLf)
                ListBox5.Items.Add(item)
                Dim splb5() As String = Split(item, "|")
                Dim week As String = splb5(0) : Dim datum As String = splb5(1)
                Try
                    If splb5(2).Contains(vbTab) Then

                        Dim t() As String = Split(splb5(2), vbTab)
                        If t.Count > 2 Then
                            Dim zariadenie As String = t(0) : Dim i As Integer = ListView3.Items.Count
                            Dim sucastzar As String = t(1) : Dim ND As String = t(2) : Dim frekvencia As String = t(3)
                            Dim údržba As String = t(4) : Dim text As String = t(5) : Dim user As String = t(6)
                            With ListView3 : .Items.Add(week) : .Items(i).SubItems.Add(datum) : .Items(i).SubItems.Add(zariadenie)
                                .Items(i).SubItems.Add(sucastzar) : .Items(i).SubItems.Add(ND) : .Items(i).SubItems.Add(frekvencia)
                                .Items(i).SubItems.Add(údržba) : .Items(i).SubItems.Add(text) : .Items(i).SubItems.Add(user)
                            End With
                        End If : End If : Catch ex As Exception : End Try : End If

            : Next
        selectedcontrol = TextBox5.Text


    End Sub
    Public Sub vyhladavacdatum()
        TextBox5.Clear() : ListView3.Items.Clear() : ListBox5.Items.Clear()
        'deň()

        Dim cb2 As String = ComboBox2.Text
        Dim xx3 As Integer = ComboBox3.SelectedItem
        Dim xx10 As Integer = ComboBox10.SelectedItem
        Dim filter As String = Nothing

        If ComboBox2.SelectedIndex = 1 Then

            filter = "|" & xx3 & ". " & ComboBox10.Text & ". " & ComboBox7.SelectedItem

            For Each item As String In listtext

                Dim vyhladavac As Boolean = (item.Contains(filter) Or item.Contains(filter.Replace(Space(1), "")))
                If vyhladavac = True Then
                    TextBox5.AppendText(item & vbCrLf) : ListBox5.Items.Add(item)
                    Dim splb5() As String = Split(item, "|")
                    Dim week As String = splb5(0) : Dim datum As String = splb5(1)
                    Try
                        If splb5(2).Contains(vbTab) Then
                            Dim t() As String = Split(splb5(2), vbTab)
                            If t.Count > 2 Then
                                Dim zariadenie As String = t(0) : Dim i As Integer = ListView3.Items.Count
                                Dim sucastzar As String = t(1) : Dim ND As String = t(2) : Dim frekvencia As String = t(3)
                                Dim údržba As String = t(4) : Dim text As String = t(5) : Dim user As String = t(6)
                                With ListView3 : .Items.Add(week) : .Items(i).SubItems.Add(datum) : .Items(i).SubItems.Add(zariadenie)
                                    .Items(i).SubItems.Add(sucastzar) : .Items(i).SubItems.Add(ND) : .Items(i).SubItems.Add(frekvencia)
                                    .Items(i).SubItems.Add(údržba) : .Items(i).SubItems.Add(text) : .Items(i).SubItems.Add(user)
                                End With
                            End If : End If : Catch ex As Exception : Finally : ListView3.Visible = True
                        ListView3.BringToFront() : End Try : End If : Next


            'týždeň
        ElseIf ComboBox2.SelectedIndex = 2 Then
            For filter1 As Integer = xx3 To xx10
                filter = filter1 & "|"
                For Each item As String In listtext

                    Dim vyhladavac As Boolean = item.Contains(ComboBox7.SelectedItem) And item.StartsWith(filter)
                    If vyhladavac = True Then
                        TextBox5.AppendText(item & vbCrLf) : ListBox5.Items.Add(item)
                        Dim splb5() As String = Split(item, "|")
                        Dim week As String = splb5(0) : Dim datum As String = splb5(1)
                        Try
                            If splb5(2).Contains(vbTab) Then
                                Dim t() As String = Split(splb5(2), vbTab)
                                If t.Count > 2 Then
                                    Dim zariadenie As String = t(0) : Dim i As Integer = ListView3.Items.Count
                                    Dim sucastzar As String = t(1) : Dim ND As String = t(2) : Dim frekvencia As String = t(3)
                                    Dim údržba As String = t(4) : Dim text As String = t(5) : Dim user As String = t(6)
                                    With ListView3 : .Items.Add(week) : .Items(i).SubItems.Add(datum) : .Items(i).SubItems.Add(zariadenie)
                                        .Items(i).SubItems.Add(sucastzar) : .Items(i).SubItems.Add(ND) : .Items(i).SubItems.Add(frekvencia)
                                        .Items(i).SubItems.Add(údržba) : .Items(i).SubItems.Add(text) : .Items(i).SubItems.Add(user)
                                    End With
                                End If : End If : Catch ex As Exception : Finally : ListView3.Visible = True
                            ListView3.BringToFront() : End Try : End If : Next

            Next
            GoTo exitsub
            'mesiac
        ElseIf ComboBox2.SelectedIndex = 3 Then
            For filter1 As Integer = xx3 To xx10
                filter = ". " & filter1 & ". " & ComboBox7.SelectedItem
                For Each item As String In listtext
                    Dim vyhladavac As Boolean = (item.Contains(filter) Or item.Contains(filter.Replace(Space(1), "")))
                    If vyhladavac = True Then
                        TextBox5.AppendText(item & vbCrLf) : ListBox5.Items.Add(item)
                        Dim splb5() As String = Split(item, "|")
                        Dim week As String = splb5(0) : Dim datum As String = splb5(1)
                        Try
                            If splb5(2).Contains(vbTab) Then
                                Dim t() As String = Split(splb5(2), vbTab)
                                If t.Count > 2 Then
                                    Dim zariadenie As String = t(0) : Dim i As Integer = ListView3.Items.Count
                                    Dim sucastzar As String = t(1) : Dim ND As String = t(2) : Dim frekvencia As String = t(3)
                                    Dim údržba As String = t(4) : Dim text As String = t(5) : Dim user As String = t(6)
                                    With ListView3 : .Items.Add(week) : .Items(i).SubItems.Add(datum) : .Items(i).SubItems.Add(zariadenie)
                                        .Items(i).SubItems.Add(sucastzar) : .Items(i).SubItems.Add(ND) : .Items(i).SubItems.Add(frekvencia)
                                        .Items(i).SubItems.Add(údržba) : .Items(i).SubItems.Add(text) : .Items(i).SubItems.Add(user)
                                    End With
                                End If : End If : Catch ex As Exception : Finally : ListView3.Visible = True
                            ListView3.BringToFront() : End Try : End If : Next

            Next
            GoTo exitsub
            'rok()
        ElseIf ComboBox2.SelectedIndex = 4 Then

            For filter1 As Integer = xx3 To xx10
                filter = ". " & filter1
                For Each item As String In listtext
                    Dim vyhladavac As Boolean = (item.Contains(filter) Or item.Contains(filter.Replace(Space(1), "")))
                    If vyhladavac = True Then
                        TextBox5.AppendText(item & vbCrLf) : ListBox5.Items.Add(item)
                        Dim splb5() As String = Split(item, "|")
                        Dim week As String = splb5(0) : Dim datum As String = splb5(1)
                        Try
                            If splb5(2).Contains(vbTab) Then
                                Dim t() As String = Split(splb5(2), vbTab)
                                If t.Count > 2 Then
                                    Dim zariadenie As String = t(0) : Dim i As Integer = ListView3.Items.Count
                                    Dim sucastzar As String = t(1) : Dim ND As String = t(2) : Dim frekvencia As String = t(3)
                                    Dim údržba As String = t(4) : Dim text As String = t(5) : Dim user As String = t(6)
                                    With ListView3 : .Items.Add(week) : .Items(i).SubItems.Add(datum) : .Items(i).SubItems.Add(zariadenie)
                                        .Items(i).SubItems.Add(sucastzar) : .Items(i).SubItems.Add(ND) : .Items(i).SubItems.Add(frekvencia)
                                        .Items(i).SubItems.Add(údržba) : .Items(i).SubItems.Add(text) : .Items(i).SubItems.Add(user)
                                    End With
                                End If : End If : Catch ex As Exception : Finally : ListView3.Visible = True
                            ListView3.BringToFront() : End Try : End If : Next
            Next
            GoTo exitsub
        End If
exitsub:

        selectedcontrol = TextBox5.Text
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        vyhladavacdatum()
    End Sub
    Private Sub ComboBox10_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox10.SelectedIndexChanged
        vyhladavacdatum()
    End Sub
    Private Sub ComboBox7_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox7.SelectedIndexChanged
        vyhladavacdatum()
    End Sub
    'graf


    Public Sub graf()

        'Try
        Chart1.Series("Series1").Points.Clear()
        TextBox8.Show() : TextBox8.BringToFront() : TextBox7.BringToFront() : CheckBox11.Visible = True
        Dim startdatum As String = Nothing : Dim enddatum As String = Nothing
        Dim listfiltrov As New List(Of String) : Dim datum1 As Date : Dim datumli As Date : Dim datum2 As Date
        If CheckBox8.CheckState = CheckState.Unchecked Then TextBox6.Hide() : TextBox7.Hide()
        If CheckBox8.CheckState = CheckState.Checked Then TextBox6.Show() : TextBox7.Show()
        TextBox8.Clear() : TextBox6.Clear() : Chart1.Visible = True : Button11.BringToFront()
        TextBox7.Clear() : Dim values18 As New List(Of String) : CheckedListBox1.Visible = True : Button2.Visible = False
        Button11.Visible = True : Button8.Visible = True : CheckedListBox1.Items.Clear() : Label1.Visible = False
        Dim z, k, x1, x2, x3 As Integer
        For Each device As String In Dialog9.CheckedListBox1.CheckedItems
            Dim intudrzby As String = Nothing
            Dim kl As String = Nothing
            suboryexist()
            Dim denna As String = Application.StartupPath & "\devices\" & device & "\daily.txt"
            Dim tyzdenna As String = Application.StartupPath & "\devices\" & device & "\weekly.txt"
            Dim mesa As String = Application.StartupPath & "\devices\" & device & "\monthly.txt"
            Dim rocna As String = Application.StartupPath & "\devices\" & device & "\yearly.txt"
            If My.Settings.priorita = String.Empty Then My.Settings.priorita = vbCrLf & "####" & vbCrLf & "####" & vbCrLf & "####" & vbCrLf & "####" & vbCrLf
            Dim prio() As String = Split(My.Settings.priorita, "####")
            Dim prioritavmesiaci As Byte = Nothing : Dim porovnanie As String = String.Empty
            Dim pyeas7 As Boolean = False
            If Dialog9.CheckBox7.CheckState = CheckState.Checked Then pyeas7 = True

            If pyeas7 And CheckBox1.Checked = True Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(4) & vbCrLf & prio(0) : GoTo dalej
            If pyeas7 And CheckBox2.Checked = True And Date.Now.Day <= 7 Then porovnanie = prio(1) & vbCrLf & prio(0)
            If pyeas7 And CheckBox2.Checked = True And Date.Now.Day > 7 AndAlso Date.Now.Day <= 14 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(0)
            If pyeas7 And CheckBox2.Checked = True And Date.Now.Day > 14 AndAlso Date.Now.Day <= 21 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(0)
            If pyeas7 And CheckBox2.Checked = True And Date.Now.Day > 21 AndAlso Date.Now.Day <= 31 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(4) & vbCrLf & prio(0)
            If pyeas7 And CheckBox6.Checked = True And Date.Now.DayOfWeek <= DayOfWeek.Tuesday Then porovnanie = prio(1) & vbCrLf & prio(0)
            If pyeas7 And CheckBox6.Checked = True And Date.Now.DayOfWeek = DayOfWeek.Wednesday Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(0)
            If pyeas7 And CheckBox6.Checked = True And Date.Now.DayOfWeek = DayOfWeek.Thursday Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(0)
            If pyeas7 And CheckBox6.Checked = True And Date.Now.DayOfWeek >= DayOfWeek.Friday Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(4) & vbCrLf & prio(0)
            If pyeas7 And CheckBox3.Checked = True And Date.Now.DayOfYear <= 92 Then porovnanie = prio(1) & vbCrLf & prio(0)
            If pyeas7 And CheckBox3.Checked = True And Date.Now.DayOfYear > 92 AndAlso Date.Now.Day <= 183 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(0)
            If pyeas7 And CheckBox3.Checked = True And Date.Now.DayOfYear > 183 AndAlso Date.Now.Day <= 275 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(0)
            If pyeas7 And CheckBox3.Checked = True And Date.Now.DayOfYear > 275 AndAlso Date.Now.Day <= 366 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(4) & vbCrLf & prio(0)

dalej:
            If CheckBox1.Checked() Then : z = 0 : k = 0
            ElseIf CheckBox6.Checked() Then : z = 1 : k = 1
            ElseIf CheckBox2.Checked() Then : z = 2 : k = 2
            ElseIf CheckBox3.Checked() Then : z = 3 : k = 3
            Else : z = 0 : k = 3
            End If
            For ci As Integer = z To k


                If ci = 0 Then : intudrzby = CheckBox1.Text : kl = denna

                ElseIf ci = 1 Then : intudrzby = CheckBox6.Text : kl = tyzdenna

                ElseIf ci = 2 Then : intudrzby = CheckBox2.Text : kl = mesa

                ElseIf ci = 3 Then : intudrzby = CheckBox3.Text : kl = rocna

                End If
                Dim rok As Integer = Dialog9.ComboBox7.Text


                ' denná

                If ci = 0 Then
                    Dim dayg As String = Date.Now.Day
                    If IsNumeric(ComboBox3.Text) Then
                        dayg = ComboBox3.Text
                    End If
                    Dim monthg As String = Date.Now.Month
                    If IsNumeric(ComboBox10.Text) Then
                        monthg = ComboBox10.Text
                    End If
                    startdatum = dayg & ". " & monthg & ". " & rok : enddatum = dayg & ". " & monthg & ". " & rok

                    'týždenná
                ElseIf ci = 1 Then
                    Dim weekg As String = week
                    Dim weekgend As String = week
                    If IsNumeric(ComboBox3.Text) Then
                        weekg = ComboBox3.Text
                    End If
                    If IsNumeric(ComboBox10.Text) Then
                        weekgend = ComboBox10.Text
                    End If
                    Dim myDT As New DateTime(rok, 1, 1, New GregorianCalendar)
                    Dim firstdayinyearfirstweek As Integer = (DateTimeFormatInfo.CurrentInfo.Calendar.GetDayOfWeek(myDT))
                    If firstdayinyearfirstweek = 0 Then firstdayinyearfirstweek = 7
                    Dim firstdayinyearfirstweekdatetime As Date = DateAdd(DateInterval.Day, -firstdayinyearfirstweek, CDate("1.1." & rok))
                    Dim frule As Date = DateAdd(DateInterval.WeekOfYear, CDbl((week)), firstdayinyearfirstweekdatetime)
                    startdatum = DateAdd(DateInterval.Day, -6, frule)
                    enddatum = DateAdd(DateInterval.WeekOfYear, CDbl((weekgend)), firstdayinyearfirstweekdatetime)
                    'mesačná
                ElseIf ci = 2 Then
                    Dim monthg As String = Date.Now.Month
                    If IsNumeric(ComboBox3.Text) Then
                        monthg = ComboBox3.Text
                    End If
                    Dim monthgend As String = Date.Now.Month
                    If IsNumeric(ComboBox10.Text) Then
                        monthgend = ComboBox10.Text
                    End If
                    startdatum = ". " & monthg & ". " & rok : enddatum = FormatDateTime(Date.DaysInMonth(rok, monthgend) & " ." & monthgend & ". " & ComboBox7.Text, DateFormat.ShortDate)
                    'ročná

                ElseIf ci = 3 Then
                    Dim yearg As String = Date.Now.Year
                    If IsNumeric(ComboBox3.Text) Then
                        yearg = ComboBox3.Text
                    End If
                    Dim yeargend As String = Date.Now.Year
                    If IsNumeric(ComboBox10.Text) Then
                        yeargend = ComboBox10.Text : startdatum = "1. 1. " & yearg : enddatum = "31. 12." & yeargend
                    End If
                End If
                datum2 = enddatum
                Dim fileContentslb As String : listfiltrov.Clear()
                fileContentslb = (My.Computer.FileSystem.ReadAllText(kl))
                Dim line() As String : Dim x As Long : line = Split(fileContentslb, vbCrLf)
                For x = 0 To UBound(line)
                    If line(x) <> String.Empty Then
                        listfiltrov.Add(line(x))
                    End If

                Next

                Dim cb1path As String = Application.StartupPath & "/devices/" & device & "/" & device

                For Each filter As String In listfiltrov
                    'ListBox5.SendToBack()
                    For Each li As String In IO.File.ReadAllLines(cb1path)
                        Try


                            If li <> String.Empty Then
                                'Dim splli() As String = Split(li, ":")
                                Dim da() As String = Split(li, "|")
                                Dim dtst() As String = Split(da(1), ".")
                                Dim dd As Integer = dtst(0)
                                Dim mm As Integer = dtst(1)
                                Dim YYYY As Integer = dtst(2).Replace(dtst(2).Remove(0, 5), "")
                                Dim datum3 As String = dd & "." & mm & "." & YYYY
                                If ci = 1 Then
                                    datum3 = startdatum

                                End If
                                datum1 = FormatDateTime(startdatum, DateFormat.ShortDate)
                                datumli = FormatDateTime(datum3, DateFormat.ShortDate)

                                Dim dat1i As Integer = DateDiff(DateInterval.Day, datumli, datum1)
                                Dim dat2i As Integer = DateDiff(DateInterval.Day, datumli, datum2)

                                If dat1i <= 0 And 0 <= dat2i Then
                                    'test.AppendLine(li & vbCrLf & datum1 & vbTab & datumli & vbTab & datum2 & vbCrLf & dat1i & vbTab & dat2i)
                                    If li.Contains(filter) = True AndAlso TextBox8.Text.Contains(filter) = False Then

                                        TextBox8.AppendText(device & " " & filter & vbTab & "O.k." & vbCrLf)
                                        TextBox6.AppendText(li & vbCrLf) : x3 += 1
                                    ElseIf li.Contains(filter) = True Then
                                        TextBox6.AppendText(li & vbCrLf) : x3 += 1
                                        'mimo(planu)    
                                    ElseIf (li.Contains("mimoplan") = True Or li.Contains("outplan")) = True And Not TextBox7.Text.Contains(li) Then
                                        TextBox7.AppendText(li & vbNewLine) : x1 += 1

                                    End If

                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    Next

                    Dim tb8 As String = TextBox8.Text
                    Dim neurobene As Boolean = Not tb8.Contains(filter) And Not values18.Contains(filter)
                    Select Case pyeas7
                        Case False
                            If neurobene = True Then CheckedListBox1.Items.Add(device & vbTab & filter & vbTab & intudrzby) _
                                : values18.Add(filter & vbTab & "undone" & vbCrLf) : x2 += 1
                        Case True
                            If porovnanie.Contains(device) And neurobene = True Then CheckedListBox1.Items.Add(device & vbTab & filter & vbTab & intudrzby) _
                                : values18.Add(filter & vbTab & "undone" & vbCrLf) : x2 += 1
                    End Select
                    Label10.Visible = True
                Next
            Next

            'If x1 Or x2 Or x3 Or x4 < 0 Then Chart1.Series("Series1").Points.Clear()
            Dim xValues As String() = {"outplan", "undone", "O.k."}
            Dim yValues As Double() = {x1, x2, x3}
            Chart1.Series("Series1").Points.DataBindXY(xValues, yValues)

            'Uncomment this line if you want to show bar chart
            Chart1.Series("Series1").ChartType = SeriesChartType.Doughnut
            ' Set labels style
            Chart1.Series("Series1")("PieLabelStyle") = "inside"
            ' Set Doughnut radius percentage
            'Chart1.Series("Series1")("DoughnutRadius") = "40"
            ' Set drawing style
            Chart1.Series("Series1")("PieDrawingStyle") = "SoftEdge"
            ' Show data points labels
            Chart1.Series("Series1").IsVisibleInLegend = True
            ' Set data points label style
            Chart1.Series("Series1")("BarLabelStyle") = "bottom"
            ' Show chart as 3D. Uncomment this line if you want to display your barchart as 3D
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            '' Draw chart as 3D Cylinder
            Chart1.Series("Series1")("DrawingStyle") = "Cylinder"
            selectedcontrol = TextBox6.Text
        Next


        'Catch ex As Exception

        'End Try

    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        graf()
    End Sub


    Private Sub Button12_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        'zapíše do user2.txt z combo boxu bo zatlačení button 12.

        Try
            Dim cbnewline As String = ComboBox4.Text
            If cbnewline <> String.Empty Then

                My.Settings.user = My.Settings.user & cbnewline & vbTab & InputBox("User Password") & vbTab & "|True" & "|True" & "|True" & vbCrLf
                Button12.Hide() : cbuseursread()
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub
    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        Try

            Dim cb4 As String = ComboBox4.Text
            'Dim usertxt As New StringBuilder
            If MsgBox(Button4.Text & vbTab & cb4 & " ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                Dim user() As String = Split(My.Settings.user, vbCrLf)
                Dim s As New StringBuilder

                For x As Integer = 0 To UBound(user)
                    Dim userpass() As String = Split(user(x), vbTab)
                    If userpass(0) <> String.Empty And userpass(0) <> cb4 Then
                        s.AppendLine(Trim(Replace(user(x), vbCrLf, "")))
                    End If
                Next
                My.Settings.user = Nothing
                My.Settings.user = s.ToString
                Button12.Hide()
            End If

            ComboBox4.Items.Clear()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        cbuseursread()
    End Sub
    Delegate Sub InvokeDelegate2()
    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        If Dialog9.ShowDialog = Windows.Forms.DialogResult.OK Then
            If Dialog9.CheckBox1.Checked Then
                Try
                    If My.Settings.reporti = 0 Then
                        Invoke(New InvokeDelegate2(AddressOf reportlv))
                    ElseIf My.Settings.reporti = 1 Then
                        report()
                    End If
                Catch ex As Exception

                End Try
            ElseIf Dialog9.CheckBox2.Checked Then
                graf()
            ElseIf Dialog9.CheckBox3.Checked Then
                Form12.ShowDialog()
            ElseIf Dialog9.CheckBox6.Checked Then
                Me.ListView1.BeginInvoke(New allsp(AddressOf Form13.Show))
            End If
        End If
    End Sub
    Delegate Sub allsp()
      Public Sub report()
        Form2.Show() '  
        'If Dialog9.ShowDialog = Windows.Forms.DialogResult.OK Then
        Form2.ToolStripProgressBar1.Visible = True

        Dim zapisnoexcel As Boolean = False
        Dim listfiltrov As New List(Of String)
        Dim širka As Integer = 17
        ': Dim regnumber As String = Nothing
        Dim writetoexcel As Boolean = Dialog9.CheckBox4.Checked : Dim intudrzby As String = Nothing : Dim mesiac As String = ComboBox10.Text
        Dim k1 As Integer = 0 : Dim k2 As Integer = 0 : Dim z, k, firstm, lastm As Integer : Dim exnbrriadok As Integer = 0 : Dim tabwidth As Integer
        If CheckBox1.Checked() = True Or CheckBox6.Checked() = True Or CheckBox2.Checked() = True Or CheckBox3.Checked() = True Then
            k1 = ComboBox5.Text : k2 = ComboBox8.Text : End If
        Dim oni As String = String.Empty : Dim wk As String = String.Empty : Dim ylocationdat As Integer = 51
        Dim farbaok = My.Settings.okbackcolor : Dim farbanotok = My.Settings.notokbackcolor
        Dim farbaforeok = My.Settings.okforecolor : Dim farbaforenotok = My.Settings.notokforecolor
        ok = Dialog4.TextBox1.Text : notok = Dialog4.TextBox2.Text : Form2.ToolStripProgressBar1.Maximum = Dialog9.CheckedListBox1.CheckedItems.Count : Form2.ToolStripProgressBar1.Value = 0
        If ok.Length <= 1 Then ok = " " & Dialog4.TextBox1.Text
        If notok.Length <= 1 Then notok = " " & Dialog4.TextBox2.Text
        Dim mesiacjeviacakonothing As Boolean
        If mesiac = Nothing Then : mesiacjeviacakonothing = False
        ElseIf mesiac <> Nothing Then : mesiacjeviacakonothing = True
        End If
        If mesiacjeviacakonothing = False Then : firstm = 1 : lastm = 12
        ElseIf mesiacjeviacakonothing = True Then : firstm = mesiac : lastm = mesiac
        End If
        Dim koe As Integer = 12
        Try
            If writetoexcel Then
                APP = CreateObject("Excel.Application")
                workbook = APP.Workbooks.Add()
                worksheet = workbook.Sheets.Item(1)
            End If
            'workbook = APP.Workbooks.Open(Application.StartupPath & "\book2.xls") : worksheet = workbook.Sheets.Item(1)
        Catch ex As Exception

        End Try
        Dim popiska As String = Nothing
        For Each device As String In Dialog9.CheckedListBox1.CheckedItems
            Dim pocetpred As Integer = listfiltrov.Count

            Dim denna As String = Application.StartupPath & "\devices\" & device & "\daily.txt"
            Dim tyzdenna As String = Application.StartupPath & "\devices\" & device & "\weekly.txt"
            Dim mesa As String = Application.StartupPath & "\devices\" & device & "\monthly.txt"
            Dim rocna As String = Application.StartupPath & "\devices\" & device & "\yearly.txt"
            Dim kl As String = Nothing : Dim textúdržby As String = TextBox3.Text
            If CheckBox1.Checked() Then : z = 0 : k = 0
            ElseIf CheckBox6.Checked() Then : z = 1 : k = 1
            ElseIf CheckBox2.Checked() Then : z = 2 : k = 2
            ElseIf CheckBox3.Checked() Then : z = 3 : k = 3
            Else : z = 0 : k = 3
            End If
            For ci As Integer = z To k


                If ci = 0 Then : intudrzby = CheckBox1.Text : If z = 0 And k = 3 Then k1 = 1 : k2 = 31
                    kl = denna : tabwidth = 500
                    'regnumber = My.Settings.regday
                ElseIf ci = 1 Then : intudrzby = CheckBox6.Text : If z = 0 And k = 3 Then k1 = 1 : k2 = 52
                    kl = tyzdenna : tabwidth = 1100
                    'regnumber = My.Settings.regweek
                ElseIf ci = 2 Then : intudrzby = CheckBox2.Text : If z = 0 And k = 3 Then k1 = 1 : k2 = 12
                    kl = mesa : tabwidth = 500
                    'regnumber = My.Settings.regmonth
                ElseIf ci = 3 Then : intudrzby = CheckBox3.Text : If z = 0 And k = 3 Then k1 = Date.Now.Year - 10 : k2 = Date.Now.Year
                    kl = rocna : tabwidth = 500
                    'regnumber = My.Settings.regyear
                End If
                If tabwidth > Form2.TabControl1.Width Then Form2.TabControl1.Width = tabwidth
                Dim fileExists As Boolean : listfiltrov.Clear()
                If fileExists = My.Computer.FileSystem.FileExists(kl) Then My.Computer.FileSystem.WriteAllText(kl, String.Empty, False)
                Dim fileContentslb As String = (My.Computer.FileSystem.ReadAllText(kl))
                Dim sNames() As String : sNames = Split(fileContentslb, vbCrLf)
                For x = 0 To UBound(sNames)
                    If sNames(x) <> Nothing Then listfiltrov.Add(sNames(x))
                Next
                If listfiltrov.Count > 0 Then


                    Dim TabPage As New Windows.Forms.TabPage()
                    With TabPage
                        .Text = device & " " & intudrzby
                        .BackColor = Color.White
                        .AutoScroll = True
                        .Width = tabwidth
                    End With



                    If mesiacjeviacakonothing = False Then : koe = 12 : Else : koe = 1 : End If
                    Dim filterpocet As Integer = listfiltrov.Count : Dim filterint As Integer = 0
                    Dim velkosťtab As Integer = (koe * (filterpocet + 3) * 23 + 50)

                    If pocetpred < filterpocet And Form2.Height < velkosťtab Then
                        If velkosťtab > My.Computer.Screen.WorkingArea.Height Then
                            Form2.TabControl1.Height = My.Computer.Screen.WorkingArea.Height - 10
                            TabPage.Height = My.Computer.Screen.WorkingArea.Height - 10


                        Else
                            Form2.TabControl1.Height = velkosťtab
                            TabPage.Height = velkosťtab
                            Form2.Height = velkosťtab
                        End If
                    End If

                    Form2.TabControl1.Controls.Add(TabPage)




                    For Each filter As String In listfiltrov
                        Dim poloha As Integer = 0
                        'zapisnoexcel = False
                        If mesiacjeviacakonothing = False Then firstm = 1 : lastm = 12
                        If ci <> 0 Then firstm = mesiac : lastm = mesiac
                        For m As Integer = firstm To lastm
                            poloha = filterint
                            If mesiacjeviacakonothing = False Then
                                If ci = 0 Then
                                    poloha += (m * (filterpocet + 1) - filterpocet - 1)
                                    mesiac = m

                                    If TabPage.Controls.ContainsKey(m) = False Then
                                        Dim mes As New Label()
                                        With mes
                                            .Location = New Point(100, (poloha + 2) * 23)
                                            .AutoSize = True
                                            .Name = m
                                            .Text = MonthName(m, False)
                                            .ForeColor = Color.DarkBlue
                                        End With
                                        TabPage.Controls.Add(mes)
                                    End If

                                End If
                            Else : ylocationdat = 51
                            End If

                            If writetoexcel Then worksheet.Cells(exnbrriadok + filterint + 3, 1).Value = filter

                            Dim TB1 As New TextBox()
                            With TB1
                                .Location = New Point(1, (3 + poloha) * 23)

                                .TabIndex = 0
                                .Multiline = True
                                .Text = filter
                                .WordWrap = True

                            End With
                            If ci = 3 Then
                                TB1.Size = New Size(363, 23)
                            Else
                                TB1.Size = New Size(205, 23)
                            End If
                            TabPage.Controls.Add(TB1)

                            If filter.Length <= 33 Then TB1.Font = New Font(TB1.Font.Name, 8, FontStyle.Regular)
                            If filter.Length > 33 Then TB1.Font = New Font(TB1.Font.Name, 7, FontStyle.Regular)
                            If filter.Length > 47 Then TB1.Font = New Font(TB1.Font.Name, 6, FontStyle.Regular)
                            If filter.Length > 60 Then TB1.Font = New Font(TB1.Font.Name, 5.6, FontStyle.Regular)




                            For x22 As Integer = k1 To k2

                                Dim x10 As Integer = filterint + 3
                                Dim x11 As Integer = +x22 + 1

                                If ci = 3 Then
                                    x11 = x22 - k1 : širka = 35.5
                                Else
                                    širka = 17
                                End If

                                'tb2
                                Dim TB2 As New TextBox()
                                With TB2
                                    .Location = New Point((x11 + 10.1) * širka, (poloha + 3) * 23)
                                    .Size = New Size(širka, 23)
                                    .TabIndex = 0
                                    .Multiline = True
                                    .Text = notok
                                    .BackColor = farbanotok
                                    .ForeColor = farbaforenotok
                                End With
                                TabPage.Controls.Add(TB2)

                                Dim rok As Integer = Dialog9.ComboBox7.Text

                                ' denná
                                If ci = 0 Then : oni = "|" & x22 & ". " & mesiac & ". " & rok
                                    wk = String.Empty
                                    popiska = device & " " & CheckBox1.Text & "                     " & ComboBox5.Text & "." & ComboBox10.Text & "." & "-" & ComboBox8.Text & "." & ComboBox10.Text & ".  " & rok
                                    'týždenná
                                ElseIf ci = 1 Then : wk = x22 & "|" : TabPage.Width = 1200
                                    popiska = device & " " & CheckBox6.Text & "                     " & k1 & "." & "-" & k2 & ".  " & rok
                                    'mesačná
                                ElseIf ci = 2 Then : oni = ". " & x22 & ". " & rok
                                    wk = String.Empty
                                    popiska = device & " " & CheckBox2.Text & "                      " & k1 & "." & "-" & k2 & ".  " & rok
                                    'ročná
                                ElseIf ci = 3 Then : oni = ". " & x22
                                    wk = String.Empty
                                    popiska = device & " " & CheckBox3.Text & "                     " & k1 & "." & "-" & k2 & ".  "

                                End If
                                Dim pravda As Boolean = False
                                Dim cb1path As String = Application.StartupPath & "/devices/" & device & "/" & device

                                For Each li As String In IO.File.ReadAllLines(cb1path)
                                    If ci = 1 Then
                                        pravda = li.Contains(filter) AndAlso li.StartsWith(wk)
                                        If pravda = True Then Exit For
                                    Else
                                        pravda = li.Contains(filter) AndAlso (li.Contains(oni) Or oni.Contains(oni.Replace(Space(1), "")))
                                        If pravda = True Then Exit For
                                    End If


                                Next
                                If pravda = True Then
                                    If writetoexcel Then
                                        With worksheet.Cells(exnbrriadok + x10, x11)
                                            .Value = ok
                                            .Interior.Color = farbaok
                                            .Font.Color() = farbaforeok
                                        End With
                                        'worksheet.Cells(exnbrriadok + x10, x11 + 1).Value = ok
                                    End If
                                    TB2.Text = ok
                                    With TB2
                                        .BackColor = farbaok
                                        .ForeColor = farbaforenotok
                                    End With
                                ElseIf pravda = False Then
                                    If writetoexcel Then
                                        With worksheet.Cells(exnbrriadok + x10, x11)
                                            .Value = notok
                                            .Interior.Color = farbanotok
                                            .Font.Color() = farbaforenotok
                                        End With
                                        'worksheet.Cells(exnbrriadok + x10, x11 + 1).Value = notok
                                        'zapisnoexcel = True
                                    End If

                                End If
                                If TB2.Text = String.Empty Then TB2.Text = notok


                                TB2.ReadOnly = True
                            Next
                            'TB1() názov vykonnenej údržby

                        Next

                        filterint += 1
                    Next

                    For x22 As Integer = k1 To k2
                        Dim x11 As Integer = +x22 + 1
                        If ci = 3 Then
                            x11 = x22 - k1 : širka = 35.5
                        Else
                            širka = 17
                        End If
                        If writetoexcel Then worksheet.Cells(exnbrriadok + 2, x22 + 1).Value = x22
                        Dim TB3 As New TextBox()
                        'tb3 datumy
                        With TB3
                            .Location = New Point((x11 + 10.1) * širka, ylocationdat)
                            .Size = New Size(širka, 23)
                            .TabIndex = 0
                            .Font = New Font(TB3.Font.Name, 7, FontStyle.Italic)
                            .WordWrap = True
                            .Text = x22
                        End With
                        TabPage.Controls.Add(TB3)
                    Next


                    Dim TBpopiska As New Label()
                    With TBpopiska
                        .Location = New Point(180, 30)
                        .AutoSize = True
                        .Text = popiska
                        .ForeColor = Color.DarkBlue
                    End With
                    TabPage.Controls.Add(TBpopiska)
                    If writetoexcel Then worksheet.Cells(exnbrriadok + 1, 1).Value = popiska

                    exnbrriadok += filterint + 2
                    Form2.ToolStripComboBox1.Items.Add((filterint * koe) + koe)
                End If
            Next

            Form2.ToolStripProgressBar1.Increment("1")

        Next
        If writetoexcel Then
            Try
                worksheet.UsedRange.Borders.Weight = xlthick
                worksheet.Columns("A:BD").EntireColumn.Autofit()
                SaveFileDialog1.InitialDirectory = Environment.SpecialFolder.Desktop
                Dim ddd As String = CheckBox4.Text & " " & ComboBox2.Text + " " & ComboBox3.Text & Format(Date.Now.Date, "dd/mm/yyyy ") & Date.Now.Second
                For Each c In IO.Path.GetInvalidFileNameChars
                    If ddd.Contains(c) Then ddd = ddd.Replace(c, "-")
                Next
                If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.CombinePath(Environment.SpecialFolder.Desktop, ddd)) = True Then
                    For x As Integer = 0 To 100
                        ddd += x
                        If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.CombinePath(Environment.SpecialFolder.Desktop, ddd)) = False Then Exit For
                    Next
                End If

                SaveFileDialog1.FileName = ddd

                If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    APP.ActiveWorkbook.SaveAs(Filename:=(IO.Path.GetFullPath(SaveFileDialog1.FileName)))
                End If
                APP.ActiveWorkbook.Close()
                APP.Quit()
                releaseobject(APP)
                releaseobject(workbook)
                releaseobject(worksheet)
            Catch ex As Exception

            End Try
        End If
        Form2.ToolStripProgressBar1.Value = Form2.ToolStripProgressBar1.Maximum

        Form2.ToolStripProgressBar1.Visible = False
    End Sub
    Dim mesiace() = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"}

    Public Sub reportlv()
        Dim rok As Integer = Dialog9.ComboBox7.Text
        Form11.ToolStripProgressBar1.Visible = True
        Form11.Show()
        Me.Update()
        Dim t As Integer = 20 : Dim t1 As Integer = 20 : Dim t2 As Integer = 20 : Dim t3 As Integer = 20 : Dim t4 As Integer = 20
        Dim lvlocx As Integer = 0
        Dim listviewwidth As Integer = 200
        'Dim repmaximusize As Boolean = My.Settings.Setreportsize
        Dim zapisnoexcel As Boolean = False
        Dim listfiltrov As New List(Of String)
        Dim širka As Integer = 17
        Dim writetoexcel As Boolean = Dialog9.CheckBox4.Checked : Dim intudrzby As String = Nothing : Dim mesiac As String = String.Empty
        Dim k1 As Integer = 0 : Dim k2 As Integer = 0 : Dim z, k, firstm, lastm As Integer : Dim exnbrriadok As Integer = 0
        If CheckBox1.Checked() = True Or CheckBox6.Checked() = True Or CheckBox2.Checked() = True Or CheckBox3.Checked() = True Then
            k1 = ComboBox5.Text : k2 = ComboBox8.Text : End If
        Dim oni As String = String.Empty : Dim wk As String = String.Empty : Dim ylocationdat As Integer = 51

        Dim farbabok = My.Settings.okbackcolor : Dim farbabnotok = My.Settings.notokbackcolor
        Dim farbaforeok = My.Settings.okforecolor : Dim farbaforenotok = My.Settings.notokforecolor
        ok = Dialog4.TextBox1.Text : notok = Dialog4.TextBox2.Text : Form11.ToolStripProgressBar1.Maximum = Dialog9.CheckedListBox1.CheckedItems.Count : Form11.ToolStripProgressBar1.Value = 0
        Dim viacmesiacov As Boolean = False
        If ComboBox2.SelectedIndex = 1 Then
            If mesiace.Contains(ComboBox10.Text) Then
                mesiac = ComboBox10.Text
            Else
                mesiac = Date.Now.Month
            End If
        ElseIf ComboBox2.SelectedIndex = 3 Then
            If mesiace.Contains(ComboBox10.Text) And Not mesiace.Contains(ComboBox3.Text) Then
                firstm = ComboBox10.Text : lastm = ComboBox10.Text
            ElseIf mesiace.Contains(ComboBox10.Text) And mesiace.Contains(ComboBox3.Text) Then
                firstm = ComboBox3.Text : lastm = ComboBox10.Text
                viacmesiacov = True
            ElseIf Not mesiace.Contains(ComboBox10.Text) And mesiace.Contains(ComboBox3.Text) Then
                firstm = ComboBox3.Text : lastm = ComboBox3.Text
            End If
        End If
        Dim koe As Integer = 12
        Try
            If writetoexcel Then
                APP = CreateObject("Excel.Application")
                workbook = APP.Workbooks.Add()
                worksheet = workbook.Sheets.Item(1)
            End If
            'workbook = APP.Workbooks.Open(Application.StartupPath & "\book2.xls") : worksheet = workbook.Sheets.Item(1)
        Catch ex As Exception

        End Try
        Dim popiska As String = Nothing

        For Each device As String In Dialog9.CheckedListBox1.CheckedItems
            Dim pocetpred As Integer = listfiltrov.Count

            Dim denna As String = Application.StartupPath & "\devices\" & device & "\daily.txt"
            Dim tyzdenna As String = Application.StartupPath & "\devices\" & device & "\weekly.txt"
            Dim mesa As String = Application.StartupPath & "\devices\" & device & "\monthly.txt"
            Dim rocna As String = Application.StartupPath & "\devices\" & device & "\yearly.txt"
            Dim kl As String = Nothing

            If CheckBox1.Checked() Then : z = 0 : k = 0
            ElseIf CheckBox6.Checked() Then : z = 1 : k = 1
            ElseIf CheckBox2.Checked() Then : z = 2 : k = 2
            ElseIf CheckBox3.Checked() Then : z = 3 : k = 3
            Else : z = 0 : k = 3
            End If


            For ci As Integer = z To k


                If ci = 0 Then : intudrzby = CheckBox1.Text : If z = 0 And k = 3 Then k1 = 1 : k2 = 31
                    kl = denna : t = t1
                    listviewwidth = 1100
                ElseIf ci = 1 Then : intudrzby = CheckBox6.Text : If z = 0 And k = 3 Then k1 = 1 : k2 = 52
                    kl = tyzdenna : t = t2
                    listviewwidth = 1300
                ElseIf ci = 2 Then : intudrzby = CheckBox2.Text : If z = 0 And k = 3 Then k1 = 1 : k2 = 12
                    kl = mesa : t = t3
                    listviewwidth = 600
                ElseIf ci = 3 Then : intudrzby = CheckBox3.Text : If z = 0 And k = 3 Then k1 = Date.Now.Year - 10 : k2 = Date.Now.Year
                    kl = rocna : t = t4
                    listviewwidth = 700

                End If


                Form11.TabControl1.TabPages(ci).Text = intudrzby

                Dim fileExists As Boolean : listfiltrov.Clear()
                If fileExists = My.Computer.FileSystem.FileExists(kl) Then My.Computer.FileSystem.WriteAllText(kl, String.Empty, False)
                Dim fileContentslb As String = (My.Computer.FileSystem.ReadAllText(kl))
                Dim sNames() As String : sNames = Split(fileContentslb, vbCrLf)
                For x = 0 To UBound(sNames)
                    If sNames(x) <> Nothing Then listfiltrov.Add(sNames(x))
                Next
                If listfiltrov.Count > 0 Then

                    If viacmesiacov = True Then : koe = 12 : Else : koe = 1 : End If
                    Dim filterpocet As Integer = listfiltrov.Count : Dim filterint As Integer = 0
                    Dim velkosťtab As Integer = (koe * (filterpocet + 3) * 23 + 50)

                    Dim lv As New Windows.Forms.ListView
                    With lv
                        '.Font = New Font(lv.Font.Name, 8)
                        .View = View.Details
                        .GridLines = True
                        .Text = device & " " & intudrzby
                        .BackColor = Color.White
                        .Location = New Point(lvlocx, t)
                        .Size = New Size(listviewwidth, (filterpocet) * 14 + 0)
                        .Scrollable = True
                    End With

                    Select Case ci
                        Case 0
                            t1 += lv.Height + 22
                        Case 1
                            t2 += lv.Height + 22
                        Case 2
                            t3 += lv.Height + 22
                        Case 3
                            t4 += lv.Height + 22
                    End Select

                    '.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    'For Each filter As String In listfiltrov
                    '   
                    '    zapisnoexcel = False
                    'If viacmesiacov = True Then firstm = 1 : lastm = 12
                    'If ci <> 0 Then firstm = mesiac : lastm = mesiac
                    For m As Integer = firstm To lastm
                        Dim poloha As Integer = 0
                        poloha = filterint

                        If viacmesiacov = True Then
                            If ci = 0 Then
                                poloha += (m * (filterpocet + 1) - filterpocet - 1)
                                mesiac = m

                                If lv.Items.ContainsKey(m) = False Then
                                    lv.Items.Add(MonthName(m, False))
                                End If
                            End If
                        End If
                        For Each filter As String In listfiltrov
                            'Dim poloha As Integer = 0
                            zapisnoexcel = False
                            If writetoexcel Then worksheet.Cells(exnbrriadok + filterint + 3, 2).Value = filter
                            lv.Items.Add(filter).UseItemStyleForSubItems = False
                            Try
                                If ci = 0 Then k2 = Date.DaysInMonth(rok, mesiac)
                            Catch ex As Exception
                                mesiac = Date.Now.Month
                            End Try


                            For x22 As Integer = k1 To k2

                                Dim x10 As Integer = filterint + 3
                                Dim x11 As Integer = +x22 + 1

                                If ci = 3 Then
                                    x11 = x22 - k1 : širka = 40

                                ElseIf ci = 1 Then
                                    širka = 20
                                Else
                                    širka = 27
                                End If

                                Dim startdatum As Date
                                Dim enddatum As Date
                                Dim datum2, datum1, datumli As Date
                                Dim pravda As Boolean = False
                                Dim find As String = notok
                                ' denná
                                If ci = 0 Then
                                    popiska = device & " " & CheckBox1.Text & "                     " & ComboBox5.Text & "." & ComboBox10.Text & "." & "-" & ComboBox8.Text & "." & ComboBox10.Text & ".  " & rok
                                    Try
                                        startdatum = Date.Parse(x22 & ". " & mesiac & ". " & rok)
                                        enddatum = startdatum
                                    Catch ex As Exception
                                    End Try
                                    'týždenná
                                ElseIf ci = 1 Then
                                    popiska = device & " " & CheckBox6.Text & "                     " & k1 & "." & "-" & k2 & ".  " & rok

                                    Dim myDT As New DateTime(rok, 1, 1, New GregorianCalendar)
                                    Dim firstdayinyearfirstweek As Integer = (DateTimeFormatInfo.CurrentInfo.Calendar.GetDayOfWeek(myDT))
                                    If firstdayinyearfirstweek = 0 Then firstdayinyearfirstweek = 7
                                    Dim firstdayinyearfirstweekdatetime As Date = DateAdd(DateInterval.Day, -firstdayinyearfirstweek, CDate("1.1." & rok))
                                    Dim frule As Date = DateAdd(DateInterval.WeekOfYear, CDbl((x22)), firstdayinyearfirstweekdatetime)
                                    startdatum = DateAdd(DateInterval.Day, -6, frule)
                                    enddatum = DateAdd(DateInterval.WeekOfYear, CDbl((x22)), firstdayinyearfirstweekdatetime)
                                    'mesačná
                                ElseIf ci = 2 Then
                                    popiska = device & " " & CheckBox2.Text & "                      " & k1 & "." & "-" & k2 & ".  " & rok

                                    startdatum = FormatDateTime("1 . " & x22 & ". " & rok)
                                    enddatum = FormatDateTime(Date.DaysInMonth(rok, x22) & " ." & x22 & ". " & ComboBox7.Text, DateFormat.ShortDate)
                                    'ročná

                                ElseIf ci = 3 Then
                                    popiska = device & " " & CheckBox3.Text & "                     " & k1 & "." & "-" & k2 & ".  "
                                    startdatum = FormatDateTime("1. 1. " & x22, DateFormat.ShortDate)
                                    enddatum = FormatDateTime("31. 12. " & x22, DateFormat.ShortDate)
                                End If
                                datum2 = enddatum
                                Dim cb1path As String = Application.StartupPath & "/devices/" & device & "/" & device
                                Dim lines() As String = Split(My.Computer.FileSystem.ReadAllText(cb1path), vbCrLf)
                                For Each li As String In lines



                                    If li.Contains("|") Then
                                        Dim da() As String = Split(li, "|")
                                        Dim dtst() As String = Split(da(1), ".")
                                        Dim dd As Integer = dtst(0)
                                        Dim mm As Integer = dtst(1)
                                        Dim YYYY As Integer = dtst(2).Replace(dtst(2).Remove(0, 5), "")
                                        Dim datum3 As String = dd & "." & mm & "." & YYYY

                                        datum1 = FormatDateTime(startdatum, DateFormat.ShortDate)
                                        datumli = FormatDateTime(datum3, DateFormat.ShortDate)

                                        Dim dat1i As Integer = DateDiff(DateInterval.Day, datumli, datum1)
                                        Dim dat2i As Integer = DateDiff(DateInterval.Day, datumli, datum2)

                                        If dat1i <= 0 And 0 <= dat2i Then
                                            pravda = li.Contains(filter)
                                        End If

                                        If pravda = True Then
                                            'ak nájde zapíš a ukončí cyklus

                                            find = ok : Exit For
                                            'ElseIf pravda = False Then
                                            '    If writetoexcel And zapisnoexcel = False Then


                                            '        zapisnoexcel = True
                                            '    End If

                                        End If
                                    End If
                                Next

                                '
                                'vloží konečnérozhodnotie do listview
                                If pravda = True Then
                                    lv.Items(lv.Items.Count - 1).SubItems.Add(find).BackColor = My.Settings.okbackcolor
                                    lv.Items(lv.Items.Count - 1).SubItems(lv.Items(lv.Items.Count - 1).SubItems.Count - 1).ForeColor = My.Settings.okforecolor
                                    lv.Items(lv.Items.Count - 1).SubItems(lv.Items(lv.Items.Count - 1).SubItems.Count - 1).Font = My.Settings.okfont
                                    If writetoexcel Then
                                        With worksheet.Cells(exnbrriadok + x10, x11 + 1)
                                            .Value = ok
                                            .Interior.Color = farbabok
                                            .Font.Color() = farbaforeok
                                        End With

                                    End If
                                ElseIf pravda = False Then
                                    lv.Items(lv.Items.Count - 1).SubItems.Add(find).BackColor = My.Settings.notokbackcolor
                                    lv.Items(lv.Items.Count - 1).SubItems(lv.Items(lv.Items.Count - 1).SubItems.Count - 1).ForeColor = My.Settings.notokforecolor
                                    lv.Items(lv.Items.Count - 1).SubItems(lv.Items(lv.Items.Count - 1).SubItems.Count - 1).Font = My.Settings.notokfont
                                    If writetoexcel Then
                                        With worksheet.Cells(exnbrriadok + x10, x11 + 1)
                                            .Value = notok
                                            .Interior.Color = farbabnotok
                                            .Font.Color() = farbaforenotok
                                        End With
                                    End If
                                End If


                            Next
                            'TB1() názov vykonnenej údržby
                            filterint += 1
                        Next


                    Next
                    lv.Columns.Add(device, 230)

                    For x22 As Integer = k1 To k2
                        If writetoexcel Then worksheet.Cells(exnbrriadok + 2, x22 + 2).Value = x22

                        lv.Font = New Font(lv.Font.Name, 8, FontStyle.Regular)
                        lv.Columns.Add(x22.ToString, širka, HorizontalAlignment.Right)
                        'tb3 datumy
                    Next
                    Dim TBpopiska As New Label()
                    With TBpopiska
                        .Location = New Point(180, t - 15)
                        .AutoSize = True
                        .Text = popiska
                        .ForeColor = Color.DarkBlue
                    End With
                    lv.Text = popiska

                    Form11.TabControl1.TabPages(ci).Controls.Add(lv)
                    Dim maxItemsToShow As Integer = lv.Items.Count - 1
                    lv.TopItem.EnsureVisible()
                    Dim positionBefore = lv.Items(maxItemsToShow).Position.Y
                    lv.Items(maxItemsToShow).EnsureVisible()
                    Dim positionAfter = lv.Items(maxItemsToShow).Position.Y
                    lv.Height += positionBefore - positionAfter
                    'Form11.Hide()

                    If writetoexcel Then worksheet.Cells(exnbrriadok + 1, 2).Value = popiska

                    exnbrriadok += filterint + 2

                End If

            Next

            Form11.ToolStripProgressBar1.Increment("1")

        Next
        'Form11.Show()
        If writetoexcel Then
            Try
                worksheet.UsedRange.Borders.Weight = xlthick
                worksheet.Columns("A:BD").EntireColumn.Autofit()
                SaveFileDialog1.InitialDirectory = Environment.SpecialFolder.Desktop
                Dim ddd As String = CheckBox4.Text & " " & ComboBox2.Text + " " & ComboBox3.Text & Format(Date.Now.Date, "dd/mm/yyyy ") & Date.Now.Second
                For Each c In IO.Path.GetInvalidFileNameChars
                    If ddd.Contains(c) Then ddd = ddd.Replace(c, "-")
                Next
                If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.CombinePath(Environment.SpecialFolder.Desktop, ddd)) = True Then
                    For x As Integer = 0 To 100
                        ddd += x
                        If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.CombinePath(Environment.SpecialFolder.Desktop, ddd)) = False Then Exit For
                    Next
                End If
                SaveFileDialog1.FileName = ddd
                If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    APP.ActiveWorkbook.SaveAs(Filename:=(IO.Path.GetFullPath(SaveFileDialog1.FileName)))
                End If
                APP.ActiveWorkbook.Close()
                APP.Quit()
                releaseobject(APP)
                releaseobject(workbook)
                releaseobject(worksheet)
            Catch ex As Exception
                Form11.ToolStripProgressBar1.Visible = False
            End Try
        End If
        Form11.ToolStripProgressBar1.Value = Form11.ToolStripProgressBar1.Maximum
        Form11.ToolStripProgressBar1.Visible = False
    End Sub





    Private Sub ComboBox4_TextUpdate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.TextUpdate
        Button12.Show() : Button13.Show()
    End Sub
    Private Sub ComboBox1_TextUpdate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.TextUpdate
        Button2.Visible = True
    End Sub
    Private Sub Listbox6_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ListBox6.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Listbox6_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ListBox6.DragDrop
        Dim jj As String = Nothing
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then

            Dim filePaths As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())

            For Each fileLoc As String In filePaths
                Dim pripona() As String = Split(My.Computer.FileSystem.GetName(fileLoc), ".")
                Dialog1.ComboBox1.Items.Add(Path.GetFileName(pripona(0)))
                If File.Exists(fileLoc) Then
                    'urobi kopiu file do priečinku devices

                    If CheckBox9.CheckState = CheckState.Unchecked Then
                        If Dialog1.ShowDialog = Windows.Forms.DialogResult.OK Then jj = Dialog1.ComboBox1.Text
                        For Each c In IO.Path.GetInvalidFileNameChars
                            If jj.Contains(c) Then jj = jj.Replace(c, "-")
                        Next
                        Dim cestazarpriečinkyjj As String = Nothing
                        If aktivnycombobox = 1 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" & ComboBox1.Text & "\user file\" & jj & "." & pripona(1)
                        ElseIf aktivnycombobox = 2 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\user file\" & jj & "." & pripona(1)
                        End If
                        FileCopy(Path.GetFullPath(fileLoc), cestazarpriečinkyjj)
                        polozky()
                        Dialog1.ComboBox1.Items.Clear()
                        Dialog1.Hide()
                    ElseIf CheckBox9.CheckState = CheckState.Checked Then
                        jj = pripona(0)
                        Dim cestazarpriečinkyjj As String = Nothing
                        If aktivnycombobox = 1 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" & ComboBox1.Text & "\user file\" & jj & "." & pripona(1)
                        ElseIf aktivnycombobox = 2 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\user file\" & jj & "." & pripona(1)
                        End If
                        FileCopy(Path.GetFullPath(fileLoc), cestazarpriečinkyjj)
                        polozky()


                    Else : GoTo Koniec
                    End If
                End If
koniec:
            Next
        End If
    End Sub
    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        Dim appzar As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\user file\"
        Dim folderExists As Boolean

        If folderExists = My.Computer.FileSystem.DirectoryExists(appzar) Then
            My.Computer.FileSystem.CreateDirectory(appzar)
        End If

        Dim cestasuboru As String = appzar & ListBox6.SelectedItem
        If folderExists = My.Computer.FileSystem.FileExists(cestasuboru) Then
            MsgBox("file not exists")
        Else
            Try
                My.Computer.FileSystem.DeleteFile(cestasuboru)
                ListBox6.Items.Remove(ListBox6.SelectedItem)
            Catch ex As Exception
            End Try
        End If

    End Sub
    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        mail.Show()

    End Sub
    Private Sub TextBox10_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox10.MouseDown
        Button2.Show() : Button21.Show()
    End Sub
    Private Sub TextBox11_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox11.MouseDown
        Button2.Show() : Button21.Show()
    End Sub
    Private Sub TextBox12_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox12.MouseDown
        Button2.Show() : Button21.Show()
    End Sub
    Private Sub TextBox13_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox13.MouseDown
        Button2.Show() : Button21.Show()
    End Sub

    Private Sub TextBox14_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox14.MouseDown
        Button2.Show() : Button21.Show()
    End Sub
    Private Sub TextBox15_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox15.MouseDown
        Button2.Show() : Button21.Show()
    End Sub
    Private Sub TextBox16_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox16.MouseDown
        Button2.Show() : Button21.Show()
    End Sub

    Private Sub TextBox17_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox17.MouseDown
        Button2.Show() : Button21.Show()
    End Sub
    Private Sub TextBox19_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox19.MouseDown
        Button2.Show() : Button21.Show()
    End Sub
    Private Sub NumericUpDown1_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles NumericUpDown1.MouseDown
        Button2.Show() : Button21.Show()
    End Sub
    Private Sub NumericUpDown2_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles NumericUpDown2.MouseDown
        Button2.Show() : Button21.Show()
    End Sub
    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click

        Try

            If Dialog4.CheckedListBox1.GetItemCheckState(3) = CheckState.Checked Then
                Form6.Show()

            Else
                System.Diagnostics.Process.Start(Application.StartupPath & "\book1." & excelextension)
            End If
        Catch ex As Exception
            Form6.Show()
        End Try
    End Sub
    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        PrintDialog1.UseEXDialog = True
        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        PrintDocument1.DefaultPageSettings.Margins.Left = 20
        PrintDocument1.DefaultPageSettings.Margins.Top = 20
        PrintDocument1.DefaultPageSettings.Margins.Bottom = 20
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()
    End Sub
    Private Sub pdoc_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If ListView3.Visible = True Then
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
            Dim ColumnHeaderHeight As Single = CSng(ListView3.Font.Height + (10 * (DpiX / 96))) '5 pixel pad on the top an bottom
            Dim StringFormat As New StringFormat
            Dim PageNumberWidth As Single = e.Graphics.MeasureString(CStr(CurrentPage), ListView3.Font).Width
            StringFormat.FormatFlags = StringFormatFlags.NoWrap
            StringFormat.Trimming = StringTrimming.EllipsisCharacter
            StringFormat.LineAlignment = StringAlignment.Center
            CurrentPage += 1
            X = CInt(e.MarginBounds.X)
            Y = CInt(e.MarginBounds.Y)
            For ColumnIndex As Integer = 0 To ListView3.Columns.Count - 1
                TextRect.X = X
                TextRect.Y = Y
                TextRect.Width = ListView3.Columns(ColumnIndex).Width
                TextRect.Height = ColumnHeaderHeight
                e.Graphics.FillRectangle(Brushes.LightGray, TextRect)
                e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
                TextRect.X += TextLeftPad
                TextRect.Width -= TextLeftPad
                e.Graphics.DrawString(ListView3.Columns(ColumnIndex).Text, ListView3.Font, Brushes.Black, TextRect, StringFormat)
                X += TextRect.Width + TextLeftPad
            Next
            Y += ColumnHeaderHeight
            For i = LastIndex To ListView3.Items.Count - 1
                With ListView3.Items(i)
                    X = CInt(e.MarginBounds.X)
                    If Y + .Bounds.Height > e.MarginBounds.Bottom Then
                        LastIndex = i - 1
                        e.HasMorePages = True
                        StringFormat.Dispose()
                        e.Graphics.DrawString(CStr(CurrentPage), ListView3.Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - ListView3.Font.Height * 2)
                        Exit Sub
                    End If
                    ImageWidth = 0
                    If ListView3.SmallImageList IsNot Nothing Then
                        If Not String.IsNullOrEmpty(.ImageKey) Then
                            e.Graphics.DrawImage(ListView3.SmallImageList.Images(.ImageKey), X, Y)
                        ElseIf .ImageIndex >= 0 Then
                            e.Graphics.DrawImage(ListView3.SmallImageList.Images(.ImageIndex), X, Y)
                        End If
                        ImageWidth = ListView3.SmallImageList.ImageSize.Width
                    End If
                    For ColumnIndex As Integer = 0 To ListView3.Columns.Count - 1
                        TextRect.X = X
                        TextRect.Y = Y
                        TextRect.Width = ListView3.Columns(ColumnIndex).Width
                        TextRect.Height = .Bounds.Height
                        Dim bColor As Integer = ListView3.Items(i).BackColor.ToArgb
                        Dim myBColor As Color = ColorTranslator.FromHtml(bColor)
                        e.Graphics.FillRectangle(New SolidBrush(myBColor), TextRect)
                        If ListView3.GridLines Then
                            e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
                        End If
                        If ColumnIndex = 0 Then TextRect.X += ImageWidth
                        TextRect.X += TextLeftPad
                        TextRect.Width -= TextLeftPad
                        If ColumnIndex < .SubItems.Count Then
                            Dim TColor As Integer = ListView3.Items(i).SubItems(ColumnIndex).ForeColor.ToArgb
                            Dim myTColor As Color = ColorTranslator.FromHtml(TColor)
                            e.Graphics.DrawString(.SubItems(ColumnIndex).Text, ListView3.Font, New SolidBrush(myTColor), TextRect, StringFormat)
                        End If
                        X += TextRect.Width + TextLeftPad
                    Next

                    Y += .Bounds.Height
                End With
            Next
            e.Graphics.DrawString(CStr(CurrentPage), ListView3.Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - ListView3.Font.Height * 2)
            StringFormat.Dispose()
            LastIndex = 0
            CurrentPage = 0
        Else

            Try

                Static intCurrentChar As Int32

                Dim font As New Font(My.Settings.ryderfont.Name, My.Settings.ryderfont.Size)
                Dim intPrintAreaHeight, intPrintAreaWidth, marginLeft, marginTop As Int32
                With PrintDocument1.DefaultPageSettings

                    intPrintAreaHeight = .PaperSize.Height - .Margins.Top - .Margins.Bottom
                    intPrintAreaWidth = .PaperSize.Width - .Margins.Left - .Margins.Right
                    marginLeft = .Margins.Left ' X coordinate
                    marginTop = .Margins.Top ' Y coordinate
                End With

                If PrintDocument1.DefaultPageSettings.Landscape Then
                    Dim intTemp As Int32
                    intTemp = intPrintAreaHeight
                    intPrintAreaHeight = intPrintAreaWidth
                    intPrintAreaWidth = intTemp
                End If

                Dim intLineCount As Int32 = CInt(intPrintAreaHeight / font.Height)

                Dim rectPrintingArea As New RectangleF(marginLeft, marginTop, intPrintAreaWidth, intPrintAreaHeight)

                Dim fmt As New StringFormat(StringFormatFlags.LineLimit)

                Dim intLinesFilled, intCharsFitted As Int32
                e.Graphics.MeasureString(Mid(selectedcontrol, intCurrentChar + 1), font, _
                            New SizeF(intPrintAreaWidth, intPrintAreaHeight), fmt, _
                            intCharsFitted, intLinesFilled)

                e.Graphics.DrawString(Mid(selectedcontrol, intCurrentChar + 1), font, _
                    Brushes.Black, rectPrintingArea, fmt)

                intCurrentChar += intCharsFitted

                If intCurrentChar < selectedcontrol.Length Then
                    e.HasMorePages = True
                Else
                    e.HasMorePages = False

                    intCurrentChar = 0
                End If


            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        'zapíše do parts.txt z combo boxu bo zatlačení button 2.
        Dim datum As String = Nothing
        Dim cestaparts As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\devparts"
        Dim ssss As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\user file"
        Dim sssstxt As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\label.txt"
        Dim cbnewline As String = ComboBox9.Text
        For Each c In IO.Path.GetInvalidFileNameChars
            If cbnewline.Contains(c) Then cbnewline = cbnewline.Replace(c, "-")
        Next
        Dim folderExists As Boolean
        Dim cestazarpriečinkycb As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & cbnewline
        If folderExists = My.Computer.FileSystem.DirectoryExists(cestazarpriečinkycb) Then
            My.Computer.FileSystem.CreateDirectory(cestazarpriečinkycb)
            My.Computer.FileSystem.CreateDirectory(ssss)
        End If
        Dim textz As String = ComboBox9.Text
        Dim fileContents As String
        Dim fileexistss As Boolean
        Try
            If fileexistss = My.Computer.FileSystem.FileExists(sssstxt) Then
                My.Computer.FileSystem.WriteAllText(sssstxt, day & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf, False)
            End If
            fileContents = My.Computer.FileSystem.ReadAllText(sssstxt)
            Dim text() As String = Split(fileContents, "#")
            Dim dat As Byte = 0 : Dim mt As Byte = 10 : Dim mtold As Byte = 11 : Dim denneMTH As Byte = 12
            If Trim(text(mt)) >= NumericUpDown1.Value Then
                datum = Trim(text(dat))
            Else
                datum = day
            End If
        Catch ex As Exception
        End Try

        Dim stitok As String = datum & vbCrLf & "#" & Trim(TextBox10.Text) & vbCrLf & "#" & Trim(TextBox11.Text) & vbCrLf & "#" & Trim(TextBox12.Text) & vbCrLf & "#" & Trim(TextBox13.Text) & vbCrLf & "#" & Trim(TextBox14.Text) & vbCrLf & "#" & Trim(TextBox15.Text) & vbCrLf & "#" & Trim(TextBox16.Text) & vbCrLf & "#" & Trim(TextBox17.Text) & vbCrLf & "#" & Trim(TextBox19.Text) & vbCrLf & "#" & NumericUpDown1.Value & vbCrLf & "#" & NumericUpDown2.Value & vbCrLf & "#"
        If cbnewline = Nothing Then GoTo 30
        Dim fileExists As Boolean : If fileExists = My.Computer.FileSystem.FileExists(cestaparts) Then
            My.Computer.FileSystem.WriteAllText(cestaparts, String.Empty, False)
        End If
        Dim fileContentscb As String = My.Computer.FileSystem.ReadAllText(cestaparts)
        Dim sNames() As String
        Dim x As Long : sNames = Split(fileContentscb, vbCrLf)
        For x = 0 To UBound(sNames)
            If sNames(x).Equals(textz) Then GoTo 30
        Next
        If ComboBox9.Text <> String.Empty Then
            ComboBox9.Items.Add(ComboBox9.Text)
            My.Computer.FileSystem.WriteAllText(cestaparts, cbnewline + vbCrLf, True)
        End If
30:
        My.Computer.FileSystem.WriteAllText(sssstxt, stitok, False)

        Button21.Visible = False : Button2.Visible = False
    End Sub



    Private Sub panel1_enter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Panel1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub panel1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Panel1.DragDrop

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim fileExists As Boolean
            fileExists = My.Computer.FileSystem.FileExists(mainbackground)
            If fileExists = True Then
                Me.Panel1.BackgroundImage.Dispose()
            End If

        End If
        Dim filePaths As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
        For Each fileLoc As String In filePaths
            If File.Exists(fileLoc) Then
                'urobi kopiu file do priečinku 

                FileCopy(Path.GetFullPath(fileLoc), mainbackground)
                Me.Panel1.BackgroundImage = Image.FromFile(mainbackground)

            End If
        Next

    End Sub


    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim fileExists As Boolean
        fileExists = My.Computer.FileSystem.FileExists(mainbackground)
        If fileExists = True Then
            Me.Panel1.BackgroundImage.Dispose()
            My.Computer.FileSystem.DeleteFile(mainbackground, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            Me.Panel1.BackgroundImage = Nothing
        End If
    End Sub


    Private Sub Listbox_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox6.SelectedIndexChanged

        'zobrazí obrázok vybratý  v comboboxe6 súbory k zariadeniu
        Dim folderExists As Boolean

        Dim appzar As String = Nothing
        If aktivnycombobox = 1 Then
            appzar = Application.StartupPath & "\devices\" & ComboBox1.Text & "\user file"
        ElseIf aktivnycombobox = 2 Then
            appzar = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\user file"
        End If
        If folderExists = My.Computer.FileSystem.DirectoryExists(appzar) Then
            My.Computer.FileSystem.CreateDirectory(appzar) : End If


        Dim foundfile As String = appzar + "/" + ListBox6.SelectedItem
        Try
            If foundfile.Contains(".gif") Or foundfile.Contains(".ico") Then GoTo dalsi
            PictureBox1.Load(foundfile)
            SetAutoSizeMode(PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage)

            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Catch ex As Exception
        End Try

dalsi:



    End Sub
    Private Sub Panel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel1.DoubleClick
        obnova()
    End Sub


    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        Button13.Show() : Button4.Visible = True
        Button44.Show()
    End Sub
    Public Sub cb9výpis()
        ListBox5.Items.Clear() : CheckedListBox2.Items.Clear() : ListBox6.Items.Clear() : ListView3.Items.Clear() : Button20.Visible = True : listtext.Clear()
        aktivnycombobox = 2
        PictureBox1.Image = Nothing
        Dim cb As String = ComboBox1.Text
        Dim cb9 As String = ComboBox9.Text
        If cb9.Contains("/") Or cb.Contains("<") Or cb.Contains(">") Or cb.Contains("*") Or cb.Contains(":") Or cb.Contains("!") Or cb.Contains("?") Then MsgBox("/<>*:!?: not posible") : Exit Sub
        Dim cestazar As String = Application.StartupPath & "\devices\" & cb + "\" & cb
        Dim cestadrirectorysúčaťzar As String = Application.StartupPath & "\devices\" & cb & "\" & cb9
        Dim fileContents As String
        Dim directoryExists As Boolean
        'natiahne zo spoločného zapísu zariadenia ale vyberie do lb5 len tie ktoré sa týkajú súčasti zariadenia

        Try
            If cb <> String.Empty Or cb9 <> String.Empty Then

                If directoryExists = My.Computer.FileSystem.DirectoryExists(cestadrirectorysúčaťzar) Then
                    My.Computer.FileSystem.CreateDirectory(cestadrirectorysúčaťzar)
                End If
                Dim fileExists As Boolean
                If fileExists = My.Computer.FileSystem.FileExists(cestazar) Then
                    My.Computer.FileSystem.WriteAllText(cestazar, String.Empty, False)
                End If
                fileContents = My.Computer.FileSystem.ReadAllText(cestazar)
                Dim riadky() As String = Split(fileContents, vbCrLf)
                For x = 0 To UBound(riadky)
                    If riadky(x).Contains(cb9) Then ListBox5.Items.Add(riadky(x)) : listtext.Add(riadky(x))
                    Dim splb5() As String = Split(riadky(x), "|")
                    Dim week As String = splb5(0) : Dim datumlv3 As String = splb5(1)

                    If splb5(2).Contains(vbTab) Then
                        Dim t() As String = Split(splb5(2), vbTab)
                        If t.Count > 2 Then
                            Dim zariadenie As String = t(0) : Dim i As Integer = ListView3.Items.Count
                            Dim sucastzar As String = t(1) : Dim ND As String = t(2) : Dim frekvencia As String = t(3)
                            Dim údržba As String = t(4) : Dim text As String = t(5) : Dim user As String = t(6)
                            With ListView3 : .Items.Add(week) : .Items(i).SubItems.Add(datumlv3) : .Items(i).SubItems.Add(zariadenie)
                                .Items(i).SubItems.Add(sucastzar) : .Items(i).SubItems.Add(ND) : .Items(i).SubItems.Add(frekvencia)
                                .Items(i).SubItems.Add(údržba) : .Items(i).SubItems.Add(text) : .Items(i).SubItems.Add(user)
                            End With
                        End If : End If : Next
                'štitokvé údaje

                Dim labelpath As String = Application.StartupPath & "\devices\" & cb & "\" & cb9 & "\label.txt"
                TextBox10.Clear() : Dim fileContentscb As String
                Try

                    If fileExists = My.Computer.FileSystem.FileExists(labelpath) Then
                        My.Computer.FileSystem.WriteAllText(labelpath, day & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf & "#" & vbCrLf, False)

                    End If
                    fileContentscb = My.Computer.FileSystem.ReadAllText(labelpath)
                    Dim text() As String = Split(fileContentscb, "#")
                    Dim datum As Byte = 0 : Dim dat As Byte = 1 : Dim SN As Byte = 2 : Dim výkon As Byte = 3 : Dim loc As Byte = 4 : Dim ST As Byte = 5 : Dim adr1 As Byte = 6 : Dim adr2 As Byte = 7 : Dim mail As Byte = 8 : Dim typ As Byte = 9 : Dim mt As Byte = 10 : Dim mtdenne As Byte = 11 : Dim bcode As Byte = 12
                    Label17.Text = Trim(text(datum)) : TextBox10.Text = Trim(text(dat)) : TextBox11.Text = Trim(text(SN)) : TextBox12.Text = Trim(text(výkon)) : TextBox13.Text = Trim(text(loc)) : TextBox14.Text = Trim(text(ST)) : TextBox15.Text = Trim(text(adr1)) : TextBox16.Text = Trim(text(adr2)) : TextBox17.Text = Trim(text(mail))
                    TextBox19.Text = Trim(text(typ)) : NumericUpDown1.Value = Trim(text(mt)) : Try : NumericUpDown2.Value = Trim(text(mtdenne)) : Catch ex As Exception : End Try
                    Try
                        Dim cb9codes As New List(Of String)
                        cb9codes.AddRange({"cb9", cb, cb9})
                        barcodelisti1i2.Add(Trim(text(bcode)), cb9codes)
                    Catch ex As Exception
                    End Try
                Catch ex As Exception
                End Try

            End If
        Catch ex As Exception

        End Try
        ''vypíše subory kpriložené  zariadeniu

        polozky()
        partsread()
    End Sub
    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        cb9výpis()
    End Sub


    Private Sub ComboBox9_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox9.SelectedIndexChanged
        cb9výpis()
    End Sub
    Private Sub ComboBox9_txtupdate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox9.TextUpdate
        Button21.Visible = True

    End Sub
    Private Sub ComboBox9_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ComboBox9.MouseClick
        TextBox10.Clear() : TextBox11.Clear() : TextBox12.Clear() : TextBox13.Clear() : TextBox14.Clear() : TextBox15.Clear() : TextBox16.Clear() : TextBox17.Clear() : TextBox19.Clear()
    End Sub


    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        '        vymaže zariadenie a file a vytvorí nový textparts  s novými údajmi z combo.boxu9
        Dim cb9 As String = ComboBox9.SelectedItem
        If MsgBox(Button4.Text & vbTab & cb9 & " ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Dim cestasucasti As String = Application.StartupPath & "\devices\" & ComboBox1.Text & "\devparts"
            Dim dirExists As Boolean = My.Computer.FileSystem.DirectoryExists(cestasucasti)
            If dirExists = True Then
                Try
                    My.Computer.FileSystem.DeleteDirectory(Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & cb9, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                Catch ex As Exception

                End Try
            End If
            ComboBox9.Items.Remove(cb9)
            Dim cbtext9 As String = My.Computer.FileSystem.ReadAllText(cestasucasti)
            My.Computer.FileSystem.WriteAllText(cestasucasti, cbtext9.Replace(vbCrLf & cb9 & vbCrLf, vbCrLf), False)
            Button20.Visible = False
        End If
    End Sub
    Public Sub hromadnýzápis()
        Try
            If CheckedListBox1.CheckedItems.Count = 0 Then Exit Sub
            Dim cestaparts As String = Nothing
            Dim zar() As String = Nothing
            For Each li In CheckedListBox1.CheckedItems
                TextBox8.AppendText(li & "O.k." & vbNewLine)
                Dim datenow As Date = Date.Now
                'apíše do text 1 spravu s dátumom a zariadením
                ListBox5.Show() : ListBox5.BringToFront() : TextBox5.SendToBack() : Button11.Visible = False
                zar = Split(li, vbTab)
                Dim zariadeniecb1 As String = Trim(zar(0))
                Dim údržba As String = zar(1)
                Dim interval As String = zar(2)
                Dim NDlw As New StringBuilder
                'Dim tb As String = week & "|" & Date.Now.Date & ":" & Space(2) & ComboBox1.Text & vbTab & ComboBox9.Text & vbTab & vybraneitems.ToString & vbTab & kl & vbTab & ListBox3.SelectedItem & vbTab & TextBox1.Text & vbTab & ComboBox4.Text & vbNewLine
                Dim tb As String = week & "|" & DateNow & "|" & Space(2) & zariadeniecb1 & vbTab & " " & vbTab & " " & vbTab & interval & vbTab & údržba & vbTab & " " & vbTab & user & vbNewLine
                Try
                    cestaparts = Application.StartupPath & "\devices\" & zariadeniecb1 & "\parts"
                    If CheckBox11.Checked = True Then
                        Dialog6.CheckedListBox1.Items.Clear()
                        Dim fileContentscb As String = My.Computer.FileSystem.ReadAllText(cestaparts)
                        Dim sNames() As String = Split(fileContentscb, vbCrLf)
                        Dim nd() As String
                        For x = 0 To UBound(sNames) - 1
                            nd = Split(sNames(x), "|")
                            If sNames(x).Contains("|") Then Dialog6.CheckedListBox1.Items.Add(nd(1))

                        Next
                        Dialog6.Text = li.ToString
                        If Dialog6.ShowDialog = Windows.Forms.DialogResult.OK Then For Each ndch In Dialog6.CheckedListBox1.CheckedItems : NDlw.Append(ndch) : Next
                        My.Computer.FileSystem.WriteAllText(cestaparts, String.Empty, False)
                        Dim NDN() As String
                        For x = 0 To UBound(sNames) - 1
                            NDN = Split(sNames(x), "|")
                            Dim cenaačasvymeny As String = " |$|0 |TTE|0"

                            Try
                                cenaačasvymeny = " |$|" & NDN(9) & " |TTE|" & NDN(11)
                            Catch ex As Exception
                            End Try
                            Dim info As String = " |O.Nu| |MD|"
                            Try
                                info = " |O.Nu|" & NDN(13) & " |MD|" & NDN(15)
                            Catch ex As Exception

                            End Try
                            If sNames(x).Contains("|") And Not Dialog6.CheckedListBox1.CheckedItems.Contains(NDN(1)) Then My.Computer.FileSystem.WriteAllText(cestaparts, sNames(x) & vbCrLf, True)
                            If sNames(x).Contains("|") And Dialog6.CheckedListBox1.CheckedItems.Contains(NDN(1)) Then Dim novydtND As String = Date.Now.Date & "|" & NDN(1) & " |IV|" & NDN(3) & " |MH|" & NDN(5) & "|MHL|" & Me.NumericUpDown1.Value & cenaačasvymeny & info : My.Computer.FileSystem.WriteAllText(cestaparts, novydtND & vbCrLf, True)
                        Next
                        tb = week & "|" & datenow & "|" & Space(2) & zariadeniecb1 & vbTab & " " & vbTab & NDlw.ToString & vbTab & interval & vbTab & údržba & vbTab & Dialog6.TextBox1.Text & vbTab & user + vbNewLine
                    End If
                Catch ex As Exception

                End Try
                If Dialog4.CheckedListBox1.GetItemCheckState(0) = CheckState.Unchecked Then
                    excelopenfile()
                    Dim LastRow As Long
                    With worksheet
                        LastRow = .Cells(.Rows.Count, 2).End(XlDirectionxlUP).Row
                    End With
                    worksheet.Cells(LastRow + 1, 1).Value = week
                    worksheet.Cells(LastRow + 1, 2).Value = Date.Now
                    Try
                        worksheet.Cells(LastRow + 1, 3).Value = zar(0)
                        worksheet.Cells(LastRow + 1, 4).Value = zar(1)
                        worksheet.Cells(LastRow + 1, 5).Value = NDlw.ToString
                        worksheet.Cells(LastRow + 1, 6).Value = Dialog6.TextBox1.Text
                        worksheet.Cells(LastRow + 1, 7).Value = user
                        worksheet.Cells(LastRow + 1, 8).Value = zar(2)
                    Catch ex As Exception
                    End Try


                    '                Catch ex As Exception
                    'End Try
                End If
                My.Computer.FileSystem.WriteAllText(cesta, tb, True)
                'vytvorí nový textfile pre zariadenie
                Try
                    Dim cestazar As String = Application.StartupPath & "\devices\" + zar(0) + "\" + zar(0)

                    My.Computer.FileSystem.WriteAllText(cestazar, tb, True)
                Catch ex As Exception

                End Try


30:
            Next
            APP.DisplayAlerts = False
            'workbook.Save()
            Try
                APP.ActiveWorkbook.Save()
            Catch ex As Exception
                forcecloseexcel()
                APP.ActiveWorkbook.Save()
            End Try

            Try
                APP.ActiveWorkbook.Close()
                APP.Quit()
            Catch ex As Exception

            End Try

            releaseobject(APP)
            releaseobject(workbook)
            releaseobject(worksheet)
            'natiahne text do text boxu
            textovýlistb()

            Button8.Visible = False
            TextBox8.Clear() : TextBox7.Clear() : TextBox6.Clear()
            TextBox8.Visible = False
            CheckedListBox1.Items.Clear()
            CheckedListBox1.Visible = False
            Label10.Visible = False
            Label1.Visible = True
            CheckBox11.Visible = False
        Catch ex As Exception
            CheckBox11.Visible = False
        End Try
        Try



        Catch ex As Exception

        End Try
    End Sub
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        hromadnýzápis()
       
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        Dim pocet As Integer = CheckedListBox1.Items.Count
        For x = 0 To pocet - 1
            CheckedListBox1.SetItemChecked(x, True)
        Next
    End Sub

    Private Sub Button23_Click(sender As System.Object, e As System.EventArgs) Handles Button23.Click
        obnova()
    End Sub
    Public Sub obnova()
        Button41.Hide() : CheckBox11.Visible = False : TextBox4.Focus()
        TextBox8.Hide() : ListBox3.Show() : TextBox3.Hide() : Button11.BringToFront() : ListView1.Hide() : ListView2.Hide()
        TextBox7.Visible = False : TextBox7.SendToBack() : TextBox6.Visible = False : TextBox6.SendToBack()
        Button6.Hide() : ListBox5.Show() : CheckBox1.Show() : CheckBox2.Show() : CheckBox3.Show() : CheckBox6.Show()
        Button10.Hide() : Button12.Hide() : Button13.Hide() : CheckedListBox1.Visible = False : Button8.Visible = False : Button11.Visible = False
        Label10.Visible = False : TextBox7.Visible = False : ComboBox7.Show() : TableLayoutPanel1.Hide()
        ListBox5.BringToFront() : selectedcontrol = TextBox5.Text : Button2.Visible = False : Button21.Visible = False
        Label1.Visible = True : Chart1.Visible = False : Chart2.Visible = False : Chart2.SendToBack()
        TextBox7.Clear() : TextBox6.Clear()

    End Sub
    Private Sub Button26_Click(sender As System.Object, e As System.EventArgs) Handles Button26.Click
        deleterecord()
    End Sub
    Public Sub deleterecord()
        Dim lb5 As String = Nothing
        Try


            If ListBox5.Visible = True Then
                If ListBox5.SelectedIndex > -1 Then

                    lb5 = ListBox5.SelectedItem
                    If Dialog3.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Dim cbline() As String = Split(My.Settings.device, vbCrLf)
                        Dim cestazar As String = Nothing
                        Try
                            For xx = 0 To UBound(cbline)
                                If cbline(xx) <> String.Empty Then
                                    cestazar = Application.StartupPath & "\devices\" + cbline(xx) + "\" + cbline(xx)

                                    Dim sazarfile = My.Computer.FileSystem.ReadAllText(cestazar)
                                    If sazarfile.Contains(lb5) Then
                                        Try
                                            'nedarí sa mi vymazť jeden riadok

                                            If sazarfile.Split(vbCrLf).Count <= 1 Then

                                                My.Computer.FileSystem.WriteAllText(cestazar, sazarfile.Replace(lb5, Nothing), False)
                                            Else
                                                My.Computer.FileSystem.WriteAllText(cestazar, sazarfile.Replace(vbCrLf & lb5 & vbCrLf, vbCrLf), False)

                                            End If

                                        Catch ex As Exception
                                        End Try

                                    End If
                                End If
                            Next
                        Catch ex As Exception
                        End Try
                        Try
                            Dim textas As String = My.Computer.FileSystem.ReadAllText(cesta)

                            If textas.Contains(lb5) Then
                                My.Computer.FileSystem.WriteAllText(cesta, String.Empty, False)
                                Try
                                    If textas.Split(vbCrLf).Count <= 1 Then
                                        My.Computer.FileSystem.WriteAllText(cesta, textas.Replace(lb5 & vbCrLf, Nothing), False)
                                    Else

                                        My.Computer.FileSystem.WriteAllText(cesta, textas.Replace(vbCrLf & lb5 & vbCrLf, vbCrLf), False)
                                    End If

                                Catch ex As Exception

                                End Try

                            End If
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try
                        Try
                            ListBox5.Items.Remove(lb5)
                        Catch ex As Exception

                        End Try

                    End If
                End If
            End If
        Catch ex As Exception

        End Try
        Try


            If ListView3.Visible = True Then

                If ListView3.FocusedItem.Index > -1 Then
                    Dim i As Integer = ListView3.FocusedItem.Index

                    Dim l3 As ListView = ListView3
                    If Dialog3.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Dim cbcontest As String = My.Settings.device
                        Dim cbline() As String = Split(cbcontest, vbCrLf)
                        Dim cestazar As String = Nothing
                        Try

                            For xx = 0 To UBound(cbline)


                                If cbline(xx) <> String.Empty Then
                                    cestazar = Application.StartupPath & "\devices\" + cbline(xx) + "\" + cbline(xx)
                                    Dim cbf() = Split(My.Computer.FileSystem.ReadAllText(cestazar), vbCrLf)
                                    For xxx = 0 To UBound(cbf)
                                        Dim contall As Boolean = True
                                        For c = 0 To l3.Items(i).SubItems.Count - 1
                                            If cbf(xxx).Contains(l3.Items(i).SubItems(c).Text) = False Then
                                                contall = False
                                            End If
                                        Next
                                        If contall = True Then
                                            lb5 = cbf(xxx)
                                            Try

                                                My.Computer.FileSystem.WriteAllText(cestazar, Join(cbf, vbCrLf).Replace(vbCrLf & cbf(xxx) & vbCrLf, vbCrLf), False)
                                            Catch ex As Exception

                                            End Try
                                            Exit For
                                        End If
                                    Next


                                End If
                            Next
                        Catch ex As Exception
                        End Try
                        Try
                            Dim textas As String = My.Computer.FileSystem.ReadAllText(cesta)
                            Dim f() = Split(textas, vbCrLf)
                            For xxx = 0 To UBound(f)
                                Dim contall As Boolean = True
                                For c = 0 To l3.Items(i).SubItems.Count - 1
                                    If f(xxx).Contains(l3.Items(i).SubItems(c).Text) = False Then
                                        contall = False
                                    End If
                                Next
                                If contall = True Then
                                    Try
                                        ListView3.FocusedItem.Remove()
                                        lb5 = f(xxx)
                                        My.Computer.FileSystem.WriteAllText(cesta, Join(f, vbCrLf).Replace(f(xxx) & vbCrLf, Nothing), False)
                                    Catch ex As Exception

                                    End Try
                                    Exit For
                                End If
                            Next
                        Catch ex As Exception

                        End Try

                    End If

                End If
            End If

        Catch ex As Exception

        End Try

        Me.Update()
        Try
            If lb5 <> String.Empty Then
                excelopenfile()


                'workbook = APP.Workbooks.Open(Application.StartupPath & "\book1.xls")
                'worksheet = workbook.Sheets.Item(1)
                Dim LastRow As Long
                Dim splb5() As String = Split(lb5, "|")


                LastRow = worksheet.Cells(worksheet.Rows.Count, 2).End(XlDirectionxlUP).Row
                For i = 1 To LastRow + 1 Step 1



                    Try
                        Dim rest As String = splb5(2)
                        'Date.Now.Date
                        Dim dat As String = Trim(worksheet.Cells(i + 1, 2).Value)
                        'ComboBox1.Text
                        Dim zar1 As String = Trim(worksheet.Cells(i + 1, 3).Value)
                        'checkedlistbox2.SelectedItem
                        Dim udr As String = Trim(worksheet.Cells(i + 1, 4).Value)
                        'ListBox3.SelectedItem ND
                        Dim ND As String = Trim(worksheet.Cells(i + 1, 5).Value)
                        'TextBox1.Text  poznámka
                        Dim poz As String = Trim(worksheet.Cells(i + 1, 6).Value)
                        'ComboBox4.Text user
                        Dim user As String = Trim(worksheet.Cells(i + 1, 7).Value)
                        'kl()interval údržby
                        Dim int As String = Trim(worksheet.Cells(i + 1, 8).Value)
                        Dim diffd As Integer = DateDiff(DateInterval.Day, CDate(dat), CDate(splb5(1)))
                        If diffd = 0 And _
           rest.Contains(zar1) And rest.Contains(udr) And _
            rest.Contains(ND) And rest.Contains(poz) And _
         rest.Contains(user) And rest.Contains(int) Then

                            worksheet.Rows(i + 1).EntireRow.Delete(XlDeleteShiftDirectionxlShiftUp)
                            Exit For
                        End If
                    Catch ex As Exception

                    End Try
                Next i
                APP.DisplayAlerts = False
                APP.ActiveWorkbook.Save()
                APP.ActiveWorkbook.Close()
                APP.Quit()
                releaseobject(APP)
                releaseobject(workbook)
                releaseobject(worksheet)
            End If
        Catch ex As Exception

        End Try
    End Sub

   

    Private Sub pomoc_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles pomoc.CheckedChanged
        Try

            Dim isAvailable = My.Computer.Network.IsAvailable
            If isAvailable = True Then WebBrowser1.Navigate("https://sites.google.com/site/ryderhelp/home2")
            TextBox2.Text = WebBrowser1.Url.AbsolutePath
            If pomoc.CheckState = CheckState.Checked Then WebBrowser1.BringToFront() : WebBrowser1.Show() : Button27.Show() : Button28.Show() : Button29.Show() : TextBox2.Show() : CheckBox9.Hide()
            If pomoc.CheckState = CheckState.Unchecked Then WebBrowser1.Hide() : Button27.Hide() : Button28.Hide() : Button29.Hide() : TextBox2.Hide() : CheckBox9.Show()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button28_Click(sender As System.Object, e As System.EventArgs) Handles Button28.Click
        WebBrowser1.GoForward()
    End Sub

    Private Sub Button27_Click(sender As System.Object, e As System.EventArgs) Handles Button27.Click
        WebBrowser1.GoBack()
    End Sub

    Private Sub Button29_Click(sender As System.Object, e As System.EventArgs) Handles Button29.Click
        WebBrowser1.Navigate(TextBox2.Text)
    End Sub





    Private Sub Button31_Click(sender As System.Object, e As System.EventArgs)

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
        If ListView4.View = View.Details Then
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
    Private Sub checkedlistbox2_DoubleClick(sender As System.Object, e As System.EventArgs) Handles CheckedListBox2.DoubleClick

        Try


            Dim pozicia As Integer = CheckedListBox2.SelectedIndex
            CheckedListBox2.Items.Remove(CheckedListBox2.SelectedItem)
            ListBox1.SetSelected(pozicia, True)
            Dim riadok() As String = Split(ListBox1.SelectedItem, "|")
            Dialog2.TextBox1.Text = Trim(riadok(1))
            Try
                Dialog2.NumericUpDown1.Value = riadok(3)
                Dialog2.NumericUpDown2.Value = riadok(5)
                Dialog2.NumericUpDown3.Value = riadok(9)
                Dialog2.NumericUpDown4.Value = riadok(11)
            Catch ex As Exception

            End Try
            If Dialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
                ListBox1.Items.Remove(ListBox1.SelectedItem)
                Dim cestasuzarpriečinky As String = Nothing
                If aktivnycombobox = 1 Then
                    cestasuzarpriečinky = Application.StartupPath & "\devices\" + ComboBox1.Text + "\parts"
                ElseIf aktivnycombobox = 2 Then
                    cestasuzarpriečinky = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + ComboBox9.Text + "\parts"
                End If

                My.Computer.FileSystem.WriteAllText(cestasuzarpriečinky, String.Empty, False)
                Dim pocet As Integer = CheckedListBox2.Items.Count : For x As Integer = 0 To pocet - 1 : ListBox1.SetSelected(x, True)

                    My.Computer.FileSystem.WriteAllText(cestasuzarpriečinky, ListBox1.SelectedItem + vbCrLf, True)
                Next
                Dialog2.NumericUpDown1.ReadOnly = False
                Dialog2.NumericUpDown2.ReadOnly = False



                If aktivnycombobox = 1 Then
                    cestasuzarpriečinky = Application.StartupPath & "\devices\" + ComboBox1.Text + "\parts"
                ElseIf aktivnycombobox = 2 Then
                    cestasuzarpriečinky = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + ComboBox9.Text + "\parts"
                End If
                Dim dnivýmeny As String = Dialog2.NumericUpDown1.Value
                Dim motohodiny As String = Dialog2.NumericUpDown2.Value
                Dim partsi As String = riadok(0) & "|" & RTrim(Dialog2.TextBox1.Text) & " |IV|" & dnivýmeny & " |MH|" & motohodiny & " |MHL|" & NumericUpDown1.Value & " |$|" & Dialog2.NumericUpDown3.Value & " |TTE|" & Dialog2.NumericUpDown4.Value

                My.Computer.FileSystem.WriteAllText(cestasuzarpriečinky, RTrim(partsi) & vbCrLf, True)

            End If


            : partsread()
        Catch ex As Exception

        End Try


    End Sub





    Public Sub popisNd()


        PictureBox1.Image = Nothing
        Try
            If CheckedListBox2.SelectedIndex >= 0 Then
                Button10.Show() : TextBox20.BringToFront() : TextBox20.Show()
                ListBox1.SetSelected(CheckedListBox2.SelectedIndex, True)
                Dim riadok() As String = Split(ListBox1.SelectedItem, "|")
                Dim replaceafter As Integer = riadok(5)
                Dim dalsiavymmena As Integer = riadok(7)
                Dim posdatumv As Date = riadok(0)
                Dim datumvy As Double = riadok(3)
                Dim differ = DateAdd("d", datumvy, posdatumv)
                TextBox20.Location = New Point(CheckedListBox2.Location.X, MousePosition.Y - 10)
                TextBox20.Text = Me.ColumnHeader4.Text & vbTab & riadok(0) & vbCrLf & Me.ColumnHeader3.Text & vbTab & differ & vbCrLf & Me.ColumnHeader5.Text & vbTab & riadok(3) & " dní" & vbCrLf & Me.ColumnHeader8.Text & vbTab & replaceafter & " motohod." & vbCrLf & Me.ColumnHeader7.Text & vbTab & dalsiavymmena & " motohod." & vbCrLf & Me.ColumnHeader6.Text & vbTab & (replaceafter + dalsiavymmena) & " motohod." & vbCrLf
                Try
                    TextBox20.AppendText(Me.ColumnHeader9.Text & vbTab & vbTab & riadok(9) & vbCrLf & Me.ColumnHeader12.Text & vbTab & Trim(riadok(11)) & "  hod")
                Catch ex As Exception

                End Try

            End If

            Dim folderExists As Boolean
            Dim appzar As String = Nothing
            If aktivnycombobox = 1 Then
                appzar = Application.StartupPath & "\devices\" + ComboBox1.Text + "\"
            ElseIf aktivnycombobox = 2 Then
                appzar = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + ComboBox9.Text + "\"
            End If
            If folderExists = My.Computer.FileSystem.DirectoryExists(appzar) Then
                My.Computer.FileSystem.CreateDirectory(appzar) : End If
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(appzar)
                Dim testFile As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(foundFile)
                If foundFile.Contains(".gif") Or foundFile.Contains(".ico") Then GoTo dalsi
                Dim splitfoundfile() As String = Split(testFile.Name, ".")
                Try


                    If splitfoundfile(0) = CheckedListBox2.Items.Item(indexlb2) Then
                        PictureBox1.Load(foundFile)
                        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage

                    End If
                Catch ex As Exception

                End Try
dalsi:
            Next


        Catch ex As Exception

        End Try

    End Sub
    Private Sub checkedlistbox2_selectedindex(sender As System.Object, e As System.EventArgs) Handles CheckedListBox2.SelectedIndexChanged
        popisNd()

    End Sub

    Private Sub checkedlistbox2_MouseEnter(sender As System.Object, e As System.EventArgs) Handles CheckedListBox2.MouseEnter
        popisNd()
    End Sub
    Private Sub checkedlistbox2_Mouseleave(sender As System.Object, e As System.EventArgs) Handles CheckedListBox2.MouseLeave
        If CheckedListBox2.SelectedItem = Nothing Then Button10.Hide()
        TextBox20.Hide()
    End Sub
    Private Sub checkedlistbox2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CheckedListBox2.MouseMove
        Dim index As Integer = Me.CheckedListBox2.IndexFromPoint(e.X, e.Y)

        If index >= 0 Then
            indexlb2 = index

        End If
        If CheckedListBox2.SelectedIndex < 0 Then
            popisNd()
        End If

    End Sub

    Private Sub checkedlistbox2_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles CheckedListBox2.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub checkedlistbox2_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles CheckedListBox2.DragDrop
        Try


            Dim jj As String = Nothing
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then

                jj = CheckedListBox2.Items.Item(indexlb2).ToString()
                Dim filePaths As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())

                For Each fileLoc As String In filePaths
                    Dim pripona() As String = Split(My.Computer.FileSystem.GetName(fileLoc), ".")

                    If File.Exists(fileLoc) Then
                        'urobi kopiu file do priečinku devices

                        Dim cestazarpriečinkyjj As String = Nothing
                        If aktivnycombobox = 1 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + jj + "." + pripona(1)
                        ElseIf aktivnycombobox = 2 Then
                            cestazarpriečinkyjj = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + ComboBox9.Text + "\" + jj + "." + pripona(1)
                        End If

                        FileCopy(Path.GetFullPath(fileLoc), cestazarpriečinkyjj)
                        Try
                            PictureBox1.BackgroundImage = Image.FromFile(cestazarpriečinkyjj)
                            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                        Catch ex As Exception

                        End Try

                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub LinkLabel2_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Form4.ShowDialog()
    End Sub


    Private Sub Button32_Click(sender As System.Object, e As System.EventArgs) Handles Button32.Click




        If Dialog2.ShowDialog = Windows.Forms.DialogResult.OK Then

            Dim cestasuzarpriečinky As String = Nothing
            If aktivnycombobox = 1 Then
                cestasuzarpriečinky = Application.StartupPath & "\devices\" + ComboBox1.Text + "\parts"
            ElseIf aktivnycombobox = 2 Then
                cestasuzarpriečinky = Application.StartupPath & "\devices\" + ComboBox1.Text + "\" + ComboBox9.Text + "\parts"
            End If
            Dim dnivýmeny As String = Dialog2.NumericUpDown1.Value
            Dim motohodiny As String = Dialog2.NumericUpDown2.Value
            Dim partsi As String = Date.Now.Date & "|" & " " & Trim(Dialog2.TextBox1.Text) & " |IV|" & dnivýmeny & " |MH|" & motohodiny & " |MHL|" & NumericUpDown1.Value & " |$|" & Dialog2.NumericUpDown3.Value & " |TTE|" & Dialog2.NumericUpDown4.Value & " |O.Nu|" & RTrim(Dialog2.TextBox2.Text) & " |MD|" & RTrim(Dialog2.TextBox3.Text)
            For Each s As String In IO.File.ReadLines(cestasuzarpriečinky)
                If s = vbCrLf Then
                    s.Replace(vbCrLf, "")
                End If
            Next
            My.Computer.FileSystem.WriteAllText(cestasuzarpriečinky, partsi & vbCrLf, True)
        End If


        partsread()
    End Sub






    Private Sub Button33_Click(sender As System.Object, e As System.EventArgs) Handles Button33.Click
        Form5.Show()
        Dim listfiltrov As New List(Of String)
        Dim sumaceny As Decimal = 0
        Dim čas As Decimal = 0
        Dim dph As Decimal = (1 + (Form5.NumericUpDown3.Value / 100))
        Dim pr As Integer = Form5.ListView1.Items.Count
        Dim cb As String = ComboBox1.Text : Dim kl As String = Nothing
        suboryexist()
        Dim denna1 As String = Application.StartupPath & "\devices\" + cb + "\daily.txt"
        Dim tyzdenna1 As String = Application.StartupPath & "\devices\" + cb + "\weekly.txt"
        Dim mesa1 As String = Application.StartupPath & "\devices\" + cb + "\monthly.txt"
        Dim rocna1 As String = Application.StartupPath & "\devices\" + cb + "\yearly.txt"
        If My.Settings.priorita = String.Empty Then My.Settings.priorita = vbCrLf + "####" + vbCrLf + "####" + vbCrLf + "####" + vbCrLf + "####" + vbCrLf
        Dim prio() As String = Split(My.Settings.priorita, "####")
        Dim porovnanie As String = Nothing
        Dim yeas7 As Boolean = Dialog9.CheckBox7.CheckState = CheckState.Checked
        If yeas7 And CheckBox1.Checked = True Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(4) & vbCrLf & prio(0) : GoTo dalej
        If yeas7 And CheckBox2.Checked = True And Date.Now.Day <= 7 Then porovnanie = prio(1) & vbCrLf & prio(0)
        If yeas7 And CheckBox2.Checked = True And Date.Now.Day > 7 AndAlso Date.Now.Day <= 14 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(0)
        If yeas7 And CheckBox2.Checked = True And Date.Now.Day > 14 AndAlso Date.Now.Day <= 21 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(0)
        If yeas7 And CheckBox2.Checked = True And Date.Now.Day > 21 AndAlso Date.Now.Day <= 31 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(4) & vbCrLf & prio(0)
        If yeas7 And CheckBox6.Checked = True And Date.Now.DayOfWeek <= DayOfWeek.Tuesday Then porovnanie = prio(1) & vbCrLf & prio(0)
        If yeas7 And CheckBox6.Checked = True And Date.Now.DayOfWeek = DayOfWeek.Wednesday Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(0)
        If yeas7 And CheckBox6.Checked = True And Date.Now.DayOfWeek = DayOfWeek.Thursday Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(0)
        If yeas7 And CheckBox6.Checked = True And Date.Now.DayOfWeek >= DayOfWeek.Friday Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(4) & vbCrLf & prio(0)
        If yeas7 And CheckBox3.Checked = True And Date.Now.DayOfYear <= 92 Then porovnanie = prio(1) & vbCrLf & prio(0)
        If yeas7 And CheckBox3.Checked = True And Date.Now.DayOfYear > 92 AndAlso Date.Now.Day <= 183 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(0)
        If yeas7 And CheckBox3.Checked = True And Date.Now.DayOfYear > 183 AndAlso Date.Now.Day <= 275 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(0)
        If yeas7 And CheckBox3.Checked = True And Date.Now.DayOfYear > 275 AndAlso Date.Now.Day <= 366 Then porovnanie = prio(1) & vbCrLf & prio(2) & vbCrLf & prio(3) & vbCrLf & prio(4) & vbCrLf & prio(0)
dalej:
        If CheckBox1.Checked() = True Then : kl = denna1
        ElseIf CheckBox6.Checked() = True Then : kl = tyzdenna1
        ElseIf CheckBox2.Checked() = True Then : kl = mesa1
        ElseIf CheckBox3.Checked() = True Then : kl = rocna1
        ElseIf CheckBox1.Checked() = False And CheckBox2.Checked() = False And CheckBox3.Checked() = False And CheckBox6.Checked() = False Then
            'Natiahne všetky aj dennu aj mesacnu aj tyzdennu aj rocnu urobí celkový prehlad 
            Dim xx As Integer
            Dim sNames1() As String = Split((My.Computer.FileSystem.ReadAllText(denna1)), vbCrLf) : For xx = 0 To UBound(sNames1) : listfiltrov.Add(sNames1(xx)) : Next
            Dim sNames2() As String = Split((My.Computer.FileSystem.ReadAllText(tyzdenna1)), vbCrLf) : For xx = 0 To UBound(sNames2) : listfiltrov.Add(sNames2(xx)) : Next
            Dim sNames3() As String = Split((My.Computer.FileSystem.ReadAllText(mesa1)), vbCrLf) : For xx = 0 To UBound(sNames3) : listfiltrov.Add(sNames3(xx)) : Next
            Dim sNames4() As String = Split((My.Computer.FileSystem.ReadAllText(rocna1)), vbCrLf) : For xx = 0 To UBound(sNames4) : listfiltrov.Add(sNames4(xx)) : Next
            GoTo jj
        End If
        'natiahne iba vybrané v checkboxw
        Dim fileContentslb As String
        fileContentslb = (My.Computer.FileSystem.ReadAllText(kl))
        Dim sNames() As String : Dim x As Long : sNames = Split(fileContentslb, vbCrLf)
        For x = 0 To UBound(sNames) : listfiltrov.Add(sNames(x))
        Next
jj:


        For Each filter As String In listfiltrov

            If filter <> String.Empty Then


                Dim li As String = TextBox5.Text

                If Dialog9.CheckBox7.Checked = False Then
                    If li.Contains(filter) = False And TextBox8.Text.Contains(filter) = False Then

                        TextBox8.AppendText(filter)
                        Form5.ListView1.Items.Add(filter)
                        Dim jedcena As Decimal = 0
                        Dim množstvo As Integer = 0
                        Dim wtime As Decimal = 0
                        Dim wprice As Double = 0
                        Try
                            Dim devrep() As String = Split(My.Settings.devreptimed, vbCrLf)
                            For Each saved As String In devrep
                                Dim saveddev() As String = Split(saved, vbTab)
                                If filter = saveddev(0) Then
                                    wtime = saveddev(1)
                                    wprice = saveddev(2)
                                End If
                            Next
                        Catch ex As Exception
                        End Try
                        pr = Form5.ListView1.Items.Count - 1

                        Form5.ListView1.Items(pr).SubItems.Add(jedcena)
                        Form5.ListView1.Items(pr).SubItems.Add(množstvo)
                        Form5.ListView1.Items(pr).SubItems.Add((jedcena * množstvo))
                        Form5.ListView1.Items(pr).SubItems.Add((jedcena * množstvo * dph))
                        Form5.ListView1.Items(pr).SubItems.Add((wtime))
                        Form5.ListView1.Items(pr).SubItems.Add((wprice))
                    Else
                        TextBox8.AppendText(filter)
                        Form5.ListView1.Items.Add(filter)

                        pr = Form5.ListView1.Items.Count - 1
                        Dim jedcena As Decimal = 0
                        Dim množstvo As Integer = 0
                        Dim wtime As Decimal = 0
                        Dim wprice As Double = 0
                        Try
                            Dim devrep() As String = Split(My.Settings.devreptimed, vbCrLf)
                            For Each saved As String In devrep
                                Dim saveddev() As String = Split(saved, vbTab)
                                If filter = saveddev(0) Then
                                    wtime = saveddev(1)
                                    wprice = saveddev(2)
                                End If
                            Next
                        Catch ex As Exception
                        End Try
                        Form5.ListView1.Items(pr).SubItems.Add(jedcena)
                        Form5.ListView1.Items(pr).SubItems.Add(množstvo)
                        Form5.ListView1.Items(pr).SubItems.Add((jedcena * množstvo))
                        Form5.ListView1.Items(pr).SubItems.Add((jedcena * množstvo * dph))
                        Form5.ListView1.Items(pr).SubItems.Add((wtime))
                        Form5.ListView1.Items(pr).SubItems.Add((wprice))
                        Form5.ListView1.Items(pr).Checked = True
                    End If

                ElseIf Dialog9.CheckBox7.Checked = True Then

                    If porovnanie <> Nothing Then
                        If TextBox8.Text.Contains(filter) = False And li.Contains(filter) = False And porovnanie.Contains(cb) Then
                            TextBox8.AppendText(filter)
                            Form5.ListView1.Items.Add(filter)
                            pr = Form5.ListView1.Items.Count - 1
                            Dim jedcena As Decimal = 0
                            Dim množstvo As Integer = 0
                            Dim wtime As Decimal = 0
                            Dim wprice As Double = 0
                            Try
                                Dim devrep() As String = Split(My.Settings.devreptimed, vbCrLf)
                                For Each saved As String In devrep
                                    Dim saveddev() As String = Split(saved, vbTab)
                                    If filter = saveddev(0) Then
                                        wtime = saveddev(1)
                                        wprice = saveddev(2)
                                    End If
                                Next
                            Catch ex As Exception
                            End Try
                            Form5.ListView1.Items(pr).SubItems.Add(jedcena)
                            Form5.ListView1.Items(pr).SubItems.Add(množstvo)
                            Form5.ListView1.Items(pr).SubItems.Add((jedcena * množstvo))
                            Form5.ListView1.Items(pr).SubItems.Add((jedcena * množstvo * dph))
                            Form5.ListView1.Items(pr).SubItems.Add((wtime))
                            Form5.ListView1.Items(pr).SubItems.Add((wprice))
                        Else
                            TextBox8.AppendText(filter)
                            Form5.ListView1.Items.Add(filter)
                            pr = Form5.ListView1.Items.Count - 1
                            Dim jedcena As Decimal = 0
                            Dim množstvo As Integer = 0
                            Dim wtime As Decimal = 0
                            Dim wprice As Double = 0
                            Try
                                Dim devrep() As String = Split(My.Settings.devreptimed, vbCrLf)
                                For Each saved As String In devrep
                                    Dim saveddev() As String = Split(saved, vbTab)
                                    If filter = saveddev(0) Then
                                        wtime = saveddev(1)
                                        wprice = saveddev(2)
                                    End If
                                Next
                            Catch ex As Exception
                            End Try
                            Form5.ListView1.Items(pr).SubItems.Add(jedcena)
                            Form5.ListView1.Items(pr).SubItems.Add(množstvo)
                            Form5.ListView1.Items(pr).SubItems.Add((jedcena * množstvo))
                            Form5.ListView1.Items(pr).SubItems.Add((jedcena * množstvo * dph))
                            Form5.ListView1.Items(pr).SubItems.Add((wtime))
                            Form5.ListView1.Items(pr).SubItems.Add((wprice))
                            Form5.ListView1.Items(pr).Checked = True
                        End If
                    End If
                End If
            End If
        Next
        'tu začinajú náhradne diely

        Dim cestaparts As String = Application.StartupPath & "\devices\" & cb & "\parts"
        Dim fileContentscb As String
        Dim fileExists1 As Boolean : If fileExists1 = My.Computer.FileSystem.FileExists(cestaparts) Then
            My.Computer.FileSystem.WriteAllText(cestaparts, String.Empty, False)
        End If
        fileContentscb = My.Computer.FileSystem.ReadAllText(cestaparts)
        Dim sNames11() As String
        sNames11 = Split(fileContentscb, vbCrLf)
        Try
            For x = 0 To UBound(sNames11)
                If sNames11(x) <> Nothing Then
                    Dim riadok() As String = Split(sNames11(x), "|")
                    Dim riadok1 As String = Trim(riadok(1))
                    Dim reafter As Integer = riadok(5)
                    Dim nextch As Integer = riadok(7)
                    Dim mot As String = reafter + nextch
                    Dim posdatumv As Date = riadok(0)
                    Dim datumvy As Integer = riadok(3)
                    Dim differ = DateAdd("d", datumvy, posdatumv)

                    Dim percenta As String = "100"
                    Dim intvymdni As Decimal = riadok(3)
                    Dim cena As Decimal = riadok(9)
                    Dim intvymmoth As Decimal = riadok(5)
                    If riadok(0) = Nothing Or 0 Then GoTo presko6
                    If intvymdni = Nothing Or 0 Then GoTo presko6
                    Dim oldDate As Date = riadok(0)
                    Dim newDate As Date = Now
                    Dim differenceInDays As Short = DateDiff(DateInterval.Day, oldDate, newDate)

                    '% 

                    If differenceInDays > intvymdni Then percenta = (200 - ((100 / intvymdni) * differenceInDays))

                    If percenta < 0 Then percenta = "0,"
presko6:
                    Try

                        If NumericUpDown1.Value = Nothing Then
                            GoTo presko7
                        ElseIf riadok(5) = 0 Then
                            GoTo presko7
                        ElseIf IsNumeric(NumericUpDown1.Value) = True Then
                            percenta = (100 - ((100 / intvymmoth) * (NumericUpDown1.Value - riadok(7))))
                            If percenta < 0 Then percenta = "0,"

presko7:

                        End If
                    Catch ex As Exception

                    End Try
                    '"ND"

                    Dim percentazaokruhlene() = Split(percenta, ",")
                    Dim farba As System.Drawing.Color
                    If percentazaokruhlene(0) > 99.4 Then farba = Color.Green
                    If percentazaokruhlene(0) > 50 And percentazaokruhlene(0) < 99.4 Then farba = Color.LightSteelBlue
                    If percentazaokruhlene(0) >= 0.5 And percentazaokruhlene(0) <= 50 Then farba = Color.IndianRed
                    If percentazaokruhlene(0) < 0.5 Then farba = Color.Red


                    Try
                        Form5.ListView1.Items.Add(riadok1).UseItemStyleForSubItems = False
                        pr = Form5.ListView1.Items.Count - 1

                        Dim množstvo As Integer = 1
                        'jed cena2
                        Form5.ListView1.Items(pr).SubItems.Add(cena)
                        'množstvo3()
                        Form5.ListView1.Items(pr).SubItems.Add(množstvo)
                        'celkovacdna4
                        Form5.ListView1.Items(pr).SubItems.Add((cena * množstvo))
                        Form5.ListView1.Items(pr).SubItems.Add((cena * množstvo * dph))
                        sumaceny += (cena * množstvo)
                        'čas montáže5
                        Form5.ListView1.Items(pr).SubItems.Add(riadok(11))
                        čas += riadok(11)
                        Form5.ListView1.Items(pr).SubItems.Add("")
                        '%%%6
                        Form5.ListView1.Items(pr).SubItems.Add(percentazaokruhlene(0) & "%").ForeColor = farba
                    Catch ex As Exception

                    End Try

                End If
            Next
        Catch ex As Exception
        End Try

        For xxx As Integer = 0 To ComboBox9.Items.Count - 2
            ComboBox9.SelectedIndex = xxx
            If ComboBox9.Items.Count > 0 Then cestaparts = Application.StartupPath & "\devices\" & cb & "\" & ComboBox9.Text & "\parts"
            ListView1.Items.Add(Space(10) + Trim(Label14.Text) + Space(10) + ComboBox9.Text).BackColor = Color.LightSteelBlue
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

                        Dim riadok() As String = Split(sNames9(x9), "|")
                        Dim riadok1 As String = Trim(riadok(1))
                        Dim reafter As Integer = riadok(5)
                        Dim nextch As Integer = riadok(7)
                        Dim mot As String = reafter + nextch
                        Dim posdatumv As Date = riadok(0)
                        Dim datumvy As Integer = riadok(3)
                        Dim differ = DateAdd("d", datumvy, posdatumv)
                        Dim percenta As String = "100"
                        Dim intvymdni As Decimal = riadok(3)
                        Dim cena As Decimal = riadok(9)
                        Dim intvymmoth As Decimal = riadok(5)
                        If riadok(0) = Nothing Or 0 Then GoTo presko4
                        If intvymdni = Nothing Or 0 Then GoTo presko4
                        Dim oldDate As Date = riadok(0)
                        Dim newDate As Date = Now
                        Dim differenceInDays As Short = DateDiff(DateInterval.Day, oldDate, newDate)
                        '% 

                        If differenceInDays > riadok(3) Then percenta = (200 - ((100 / intvymdni) * differenceInDays))

                        If percenta < 0 Then percenta = "0,"
presko4:
                        Try

                            If NumericUpDown1.Text = Nothing Then
                                GoTo presko5
                            ElseIf intvymmoth = 0 Then
                                GoTo presko5
                            ElseIf IsNumeric(NumericUpDown1.Value) = True Then

                                percenta = (100 - ((100 / intvymmoth) * (NumericUpDown1.Value - riadok(7))))
                                If percenta < 0 Then percenta = "0,"
presko5:
                            End If
                        Catch ex As Exception

                        End Try
                        Dim percentazaokruhlene() = Split(percenta, ",")

                        Dim farba As System.Drawing.Color
                        If percentazaokruhlene(0) > 99.4 Then farba = Color.Green
                        If percentazaokruhlene(0) > 50 And percentazaokruhlene(0) < 99.4 Then farba = Color.LightSteelBlue
                        If percentazaokruhlene(0) >= 0.5 And percentazaokruhlene(0) <= 50 Then farba = Color.IndianRed
                        If percentazaokruhlene(0) < 0.5 Then farba = Color.Red

                        'cena
                        Try

                            '"ND1"
                            Form5.ListView1.Items.Add(riadok1).UseItemStyleForSubItems = False
                            pr = Form5.ListView1.Items.Count - 1
                            Dim množstvo As Integer = 1
                            'jed cena2
                            Form5.ListView1.Items(pr).SubItems.Add(cena)
                            'množstvo3()
                            Form5.ListView1.Items(pr).SubItems.Add(množstvo)
                            'celkovacdna4
                            Form5.ListView1.Items(pr).SubItems.Add((cena * množstvo))
                            Form5.ListView1.Items(pr).SubItems.Add((cena * množstvo * dph))
                            sumaceny += (cena * množstvo)
                            'čas montáže5
                            Form5.ListView1.Items(pr).SubItems.Add(riadok(11))
                            čas += riadok(11)
                            Form5.ListView1.Items(pr).SubItems.Add("")
                            '%%%6
                            Form5.ListView1.Items(pr).SubItems.Add(percentazaokruhlene(0) & "%").ForeColor = farba
                        Catch ex As Exception

                        End Try

                    Else
                    End If
                Next
            Catch ex As Exception
            End Try


        Next
        Dim wsumaceny As Decimal = 0
        Dim wčas As Double


        For Each item In Form5.ListView1.Items

            Try
                wsumaceny += item.SubItems.Item(6).Text * item.SubItems.Item(5).Text
                wčas += item.SubItems.Item(5).Text
            Catch ex As Exception

            End Try



        Next

        Dim num2 As Decimal = 0
        If wsumaceny > 0 And wčas > 0 Then num2 = wsumaceny / wčas
        Form5.Label20.Text = wčas
        Form5.Label21.Text = num2
        Form5.Label22.Text = wsumaceny
        Try
            Form5.Label8.Text = čas * Form5.NumericUpDown2.Value
            Form5.Label3.Text = Format(sumaceny, "0.000")
            Form5.Label4.Text = Format(čas, "0.000")
            Form5.Label16.Text = Format(sumaceny * dph, "0.000")

            Form5.Label23.Text = Format((CDec(Form5.Label8.Text) + CDec(Form5.Label22.Text)), "0.000")

            Form5.Label15.Text = Format((Form5.Label23.Text) * dph, "0.000")
            Form5.TextBox77.Text = Format((Form5.Label23.Text + sumaceny), "0.000")
            Form5.TextBox66.Text = Format(((Form5.Label23.Text + sumaceny) * dph), "0.000")
        Catch ex As Exception

        End Try


    End Sub

    Private Sub Button34_Click(sender As System.Object, e As System.EventArgs) Handles Button34.Click

        ListView2.Size = ListView1.Size
        ListView2.Location = ListView1.Location
        ListView2.Show()
        ListView2.BringToFront()
        ListView1.Hide()
    End Sub

    Private Sub ListView1_ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckedEventArgs) Handles ListView1.ItemChecked
        If e.Item.Index = ListView1.Items.Count - 1 And e.Item.Checked = True Then
            ListView1.Items(ListView1.Items.Count - 1).SubItems.Clear()
            Dim čas As Double = 0
            Dim sumaceny As Decimal = 0
            Dim exp As Decimal = 0
            Dim expplan As Decimal = 0

            For Each item As ListViewItem In ListView1.Items
                Try
                    If item.Index < ListView1.Items.Count - 1 Then

                        sumaceny += item.SubItems.Item(8).Text
                        exp += item.SubItems.Item(9).Text
                        expplan += item.SubItems.Item(10).Text
                        čas += item.SubItems.Item(11).Text
                    End If
                Catch ex As Exception

                End Try
            Next
            Try
                For x = 1 To 11
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("___")
                Next
                ListView1.Items(ListView1.Items.Count - 1).SubItems(8).Text = sumaceny
                ListView1.Items(ListView1.Items.Count - 1).SubItems(9).Text = exp
                ListView1.Items(ListView1.Items.Count - 1).SubItems(10).Text = expplan
                ListView1.Items(ListView1.Items.Count - 1).SubItems(11).Text = čas
            Catch ex As Exception

            End Try

            ListView1.Items(ListView1.Items.Count - 1).Text = "suma"
            Exit Sub
        ElseIf e.Item.Index = ListView1.Items.Count - 1 And e.Item.Checked = False Then
            For Each item As ListViewItem In ListView1.CheckedItems
                item.Checked = False
            Next
            Exit Sub

        End If

        If ListView1.CheckedItems.Count > 0 And e.Item.Index < ListView1.Items.Count - 1 Then
            Dim čas As Double = 0
            Dim sumaceny As Decimal = 0
            Dim exp As Decimal = 0
            Dim expplan As Decimal = 0

            For Each item As ListViewItem In ListView1.CheckedItems

                Try
                    sumaceny += item.SubItems.Item(8).Text
                    exp += item.SubItems.Item(9).Text
                    expplan += item.SubItems.Item(10).Text
                    čas += item.SubItems.Item(11).Text

                Catch ex As Exception

                End Try
            Next
            Try

                For x = 1 To 11
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("___")
                Next

                ListView1.Items(ListView1.Items.Count - 1).SubItems(8).Text = sumaceny
                ListView1.Items(ListView1.Items.Count - 1).SubItems(9).Text = exp
                ListView1.Items(ListView1.Items.Count - 1).SubItems(10).Text = expplan
                ListView1.Items(ListView1.Items.Count - 1).SubItems(11).Text = čas
            Catch ex As Exception

            End Try

            ListView1.Items(ListView1.Items.Count - 1).Text = "suma"
        End If


    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        If WebBrowser1.Visible Then
            WebBrowser1.Hide()
        Else
            Dim adres As New StringBuilder
            adres.Append("https://maps.google.com/maps?q= ")
            Dim line1 As New StringBuilder
            For Each line In TextBox15.Lines
                If line <> String.Empty Then
                    line.ToString.Replace(" ", "+")
                    adres.Append(line.ToString & "+")
                End If
            Next
            WebBrowser1.Navigate(adres.ToString)
            WebBrowser1.BringToFront()
            WebBrowser1.Show()
        End If
    End Sub

    Private Sub Label1_Click(sender As System.Object, e As System.EventArgs) Handles Label1.Click
        Form7.ShowDialog()
    End Sub



    Private Sub CheckBox4_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            If CInt(ListView3.Size.Width - FlowLayoutPanel2.Width) > My.Settings.rydersizeonload.Width Then
                For x1 As Integer = 1 To ListView3.Columns.Count - 1

                    ListView3.Columns(x1).Width = ListView3.Columns(x1).Width * ((Me.Width - FlowLayoutPanel2.Width - 20) / ListView3.Width)
                Next
                ListView3.Width = Me.Width - FlowLayoutPanel2.Width - 20 - ListView4.Width
                ListView3.Height = Me.Height - ListView3.Location.Y - 20
                ListView3.Height = FlowLayoutPanel2.Height
            End If
            CheckBox10.Text = Label1.Text : Button39.Text = Button30.Text
            TableLayoutPanel1.Visible = True
            'zobrazí na boku všetku plánovanú údržbu

            ColumnHeader29.Text = ColumnHeader4.Text
            ColumnHeader30.Text = ColumnHeader22.Text
            Dim cb() As String
            Dim newDate As Date = Now
            Dim newdiff As Integer = 365
            'MsgBox(CBpermision.ToString)
            Dim differenceInDays As Integer = 0
            Dim x As Long : cb = Split(CBpermision.ToString, vbCrLf)
            ListView4.Items.Clear() : ListView4.Visible = True : ListView4.BringToFront() : Button38.Visible = True : Button39.Visible = True
            Me.Size = New Size(Me.Width + ListView4.Width, Me.Height)
            Me.Location = New Point(((My.Computer.Screen.WorkingArea.Width / 2) - (Me.Width / 2)), 0)
            Dim pr As Integer = ListView4.Items.Count - 1
            Dim kl As String = Nothing
            Try

                For x = 0 To UBound(cb)
                    If cb(x) = " " Then GoTo next1
                    ListView4.Items.Add(cb(x)).BackColor = Color.LemonChiffon
                    Dim denna1 As String = Application.StartupPath & "\devices\" & cb(x) & "\daily.txt"
                    Dim tyzdenna1 As String = Application.StartupPath & "\devices\" & cb(x) & "\weekly.txt"
                    Dim mesa1 As String = Application.StartupPath & "\devices\" & cb(x) & "\monthly.txt"
                    Dim rocna1 As String = Application.StartupPath & "\devices\" & cb(x) & "\yearly.txt"
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


                    Dim interval As Integer = 0
                    Try
                        Dim sNames1() As String = Split((My.Computer.FileSystem.ReadAllText(denna1)), vbCrLf)
                        For xx = 0 To UBound(sNames1)
                            interval = 1 : Dim percenta As Integer = 0
                            If sNames1(xx) <> String.Empty Then
                                Dim farba As System.Drawing.Color = Color.Gray
                                Dim olddate As Date = Nothing
                                For Each line In IO.File.ReadLines(Application.StartupPath & "\devices\" + cb(x) + "\" + cb(x))
                                    If line <> String.Empty Then
                                        Dim Dat() As String = Split(line, "|")

                                        If line.Contains(sNames1(xx)) Then
                                            olddate = Dat(1)
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

                                ListView4.Items.Add(sNames1(xx)).ForeColor = farba
                                pr = ListView4.Items.Count - 1
                                ListView4.Items(pr).SubItems.Add(CheckBox1.Text)
                                ListView4.Items(pr).SubItems.Add(percenta & " %")
                                ListView4.Items(pr).SubItems.Add(olddate)
                            End If

                        Next
                    Catch ex As Exception

                    End Try
                    Try


                        Dim sNames2() As String = Split((My.Computer.FileSystem.ReadAllText(tyzdenna1)), vbCrLf)
                        For xx = 0 To UBound(sNames2)
                            interval = 7 : Dim percenta As Integer = 0
                            If sNames2(xx) <> String.Empty Then
                                Dim farba As System.Drawing.Color = Color.Gray
                                Dim olddate As Date = Nothing
                                For Each line In IO.File.ReadLines(Application.StartupPath & "\devices\" + cb(x) + "\" + cb(x))
                                    If line <> String.Empty Then

                                        Dim Dat() As String = Split(line, "|")

                                        If line.Contains(sNames2(xx)) Then
                                            olddate = Dat(1)
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
                                ListView4.Items.Add(sNames2(xx)).ForeColor = farba
                                pr = ListView4.Items.Count - 1
                                ListView4.Items(pr).SubItems.Add(CheckBox6.Text)
                                ListView4.Items(pr).SubItems.Add(percenta & " %")
                                ListView4.Items(pr).SubItems.Add(olddate)

                            End If
                        Next
                    Catch ex As Exception

                    End Try


                    Try


                        Dim sNames3() As String = Split((My.Computer.FileSystem.ReadAllText(mesa1)), vbCrLf)
                        For xx = 0 To UBound(sNames3) : Dim percenta As Integer = 0
                            interval = Date.DaysInMonth(Date.Now.Year, Date.Now.Month)
                            If sNames3(xx) <> String.Empty Then
                                Dim farba As System.Drawing.Color = Color.Gray
                                Dim olddate As Date = Nothing
                                For Each line In IO.File.ReadLines(Application.StartupPath & "\devices\" + cb(x) + "\" + cb(x))
                                    If line <> String.Empty Then
                                        Dim Dat() As String = Split(line, "|")


                                        If line.Contains(sNames3(xx)) Then
                                            olddate = Dat(1)
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

                                ListView4.Items.Add(sNames3(xx)).ForeColor = farba
                                pr = ListView4.Items.Count - 1
                                ListView4.Items(pr).SubItems.Add(CheckBox2.Text)
                                ListView4.Items(pr).SubItems.Add(percenta & " %")
                                ListView4.Items(pr).SubItems.Add(olddate)

                            End If
                        Next
                    Catch ex As Exception

                    End Try
                    Try


                        Dim sNames4() As String = Split((My.Computer.FileSystem.ReadAllText(rocna1)), vbCrLf)
                        For xx = 0 To UBound(sNames4)
                            interval = 365.25 : Dim percenta As Integer = 0
                            If sNames4(xx) <> String.Empty Then
                                Dim farba As System.Drawing.Color = Color.Gray
                                Dim olddate As Date = Nothing
                                For Each line In IO.File.ReadLines(Application.StartupPath & "\devices\" + cb(x) + "\" + cb(x))
                                    If line <> String.Empty Then
                                        Dim Dat() As String = Split(line, "|")

                                        If line.Contains(sNames4(xx)) Then
                                            olddate = Dat(1)
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
                                ListView4.Items.Add(sNames4(xx)).ForeColor = farba
                                pr = ListView4.Items.Count - 1
                                ListView4.Items(pr).SubItems.Add(CheckBox3.Text)
                                ListView4.Items(pr).SubItems.Add(percenta & " %")
                                ListView4.Items(pr).SubItems.Add(olddate)
                            End If
                        Next
                    Catch ex As Exception

                    End Try
Next1:
                Next


            Catch ex As Exception

            End Try
        Else
            ListView4.Visible = False
            TableLayoutPanel1.Visible = False
            Me.Size = My.Settings.rydersizeonload
            Me.Location = New Point(((My.Computer.Screen.WorkingArea.Width / 2) - (Me.Width / 2)), 0)
            textovýlistb()
            partsread()
        End If
    End Sub

    Private Sub Button38_Click(sender As System.Object, e As System.EventArgs) Handles Button38.Click
        TableLayoutPanel1.Visible = False
        Dim cestaparts As String = Nothing
        Dim zar As String = Nothing
        Dim lenudrzba As String = Nothing

        Dim cestazar As String
        Dim interval As String = Nothing
        Try

            Dim datenow As Date = Date.Now
            For Each li As ListViewItem In ListView4.Items
                If li.SubItems.Count = 1 Then zar = Trim(li.Text) : cestaparts = Application.StartupPath & "\devices\" & Trim(zar) + "\parts"
                If li.SubItems.Count > 1 And li.Checked Then
                    Dim NDlw As New StringBuilder
                    Dialog6.TextBox1.Clear()
                    If CheckBox10.Checked = True Then
                        Dialog6.CheckedListBox1.Items.Clear()
                        Dim fileContentscb As String = My.Computer.FileSystem.ReadAllText(cestaparts)
                        Dim sNames() As String = Split(fileContentscb, vbCrLf)
                        Dim nd() As String
                        For x = 0 To UBound(sNames) - 1
                            nd = Split(sNames(x), "|")
                            If sNames(x).Contains("|") Then Dialog6.CheckedListBox1.Items.Add(nd(1))

                        Next
                        Dialog6.Text = li.Text
                        If Dialog6.ShowDialog = Windows.Forms.DialogResult.OK Then For Each ndch In Dialog6.CheckedListBox1.CheckedItems : NDlw.Append(ndch) : Next
                        My.Computer.FileSystem.WriteAllText(cestaparts, String.Empty, False)
                        Dim NDN() As String
                        For x = 0 To UBound(sNames) - 1
                            NDN = Split(sNames(x), "|")
                            Dim cenaačasvymeny As String = " |$|0 |TTE|0"
                            Try
                                cenaačasvymeny = " |$|" & NDN(9) & " |TTE|" & NDN(11)
                            Catch ex As Exception
                            End Try
                            Dim info As String = " |O.Nu| |MD|"
                            Try
                                info = " |O.Nu|" & NDN(13) & " |MD|" & NDN(15)
                            Catch ex As Exception

                            End Try

                            If sNames(x).Contains("|") And Not Dialog6.CheckedListBox1.CheckedItems.Contains(NDN(1)) Then My.Computer.FileSystem.WriteAllText(cestaparts, sNames(x) & vbCrLf, True)
                            If sNames(x).Contains("|") And Dialog6.CheckedListBox1.CheckedItems.Contains(NDN(1)) Then Dim novydtND As String = datenow.Date & "|" & NDN(1) & " |IV|" & NDN(3) & " |MH|" & NDN(5) & "|MHL|" & Me.NumericUpDown1.Value & cenaačasvymeny & info : My.Computer.FileSystem.WriteAllText(cestaparts, novydtND & vbCrLf, True)
                        Next
                    End If

                    lenudrzba = li.Text
                    interval = li.SubItems(1).Text
                    Dim tb As String = week & "|" & datenow & "|" & Space(2) & zar & vbTab & " " & vbTab & NDlw.ToString & vbTab & interval & vbTab & lenudrzba & vbTab & Dialog6.TextBox1.Text & vbTab & user & vbNewLine
                    My.Computer.FileSystem.WriteAllText(cesta, tb, True)
                    cestazar = Application.StartupPath & "\devices\" & zar & "\" + zar
                    My.Computer.FileSystem.WriteAllText(cestazar, tb, True)
                    If Dialog4.CheckedListBox1.GetItemCheckState(0) = CheckState.Unchecked Then
                        Try
                            excelopenfile()
                            Dim LastRow As Long
                            With worksheet
                                LastRow = .Cells(.Rows.Count, 2).End(XlDirectionxlUP).Row
                            End With
                            worksheet.Cells(LastRow + 1, 1).Value = week
                            worksheet.Cells(LastRow + 1, 2).Value = datenow
                            worksheet.Cells(LastRow + 1, 3).Value = zar
                            worksheet.Cells(LastRow + 1, 4).Value = lenudrzba
                            If NDlw.Length > 0 Then
                                worksheet.Cells(LastRow + 1, 5).Value = NDlw
                            End If
                            worksheet.Cells(LastRow + 1, 6).Value = Dialog6.TextBox1.Text
                            worksheet.Cells(LastRow + 1, 7).Value = user
                            worksheet.Cells(LastRow + 1, 8).Value = interval
                            worksheet.UsedRange.Borders.Weight = xlthick
                            worksheet.Columns("A:Q").EntireColumn.Autofit()
                            APP.DisplayAlerts = True
                            APP.ActiveWorkbook.Save()
                            APP.ActiveWorkbook.Close()
                            APP.Quit()
                        releaseobject(APP)
                        releaseobject(workbook)
                        releaseobject(worksheet)
                        Catch ex As Exception

        End Try
                    End If
                End If
            Next
        Catch ex As Exception

        End Try
        ListView4.Hide() : CheckBox10.Hide()

        Me.Size = New Size(Me.Width - ListView4.Width, Me.Height)
        Me.Location = New Point(((My.Computer.Screen.WorkingArea.Width / 2) - (Me.Width / 2)), 0)
        textovýlistb()
        partsread()
    End Sub

    Private Sub Button39_Click(sender As System.Object, e As System.EventArgs) Handles Button39.Click
        'Button38.Visible = False : Button39.Visible = False : Button7.Visible = False
        'ListView4.Hide() : CheckBox10.Hide()
        ListView4.Visible = False
        TableLayoutPanel1.Visible = False
        Me.Size = My.Settings.rydersizeonload
        Me.Location = New Point(((My.Computer.Screen.WorkingArea.Width / 2) - (Me.Width / 2)), 0)
        textovýlistb()
        partsread()
    End Sub



    Private Sub Button40_Click(sender As System.Object, e As System.EventArgs)


        Try
            ProgressBar2.Maximum = (ListView1.Columns.Count * ListView1.Items.Count)
            ProgressBar2.Visible = True
            ProgressBar2.Value = 0
            Dim x As Integer = 0
            Dim r As Integer = 1
            ListView1.UseWaitCursor = True
            APP = CreateObject("Excel.Application")
            workbook = APP.Workbooks.Add()
            worksheet = workbook.Sheets.Item(1)
            SaveFileDialog1.InitialDirectory = Environment.SpecialFolder.Desktop
            SaveFileDialog1.DefaultExt = ".xls"

            SaveFileDialog1.FileName = Format(Date.Now.Date, "dd/MM/yyyy ")
            SaveFileDialog1.Filter = "excel files (*.xls)|.xls|All files (*.*)|*.*"
            If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim bColor As Integer = 2
                If ListView1.Visible = True Then
                    For Each col As ColumnHeader In ListView1.Columns

                        x += 1
                        worksheet.Cells(1, x).value = col.Text
                        worksheet.Cells(1, x).Interior.Color = Color.LightGray
                        worksheet.Cells(r, x).BorderAround(1, 2, xlcolorautomatic, Color.Black)
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
                                worksheet.Cells(r, xx + 1).BorderAround(1, 2, xlcolorautomatic, Color.Black)
                            Next
                        Next
                    Next
                End If
                If ListView2.Visible = True Then
                    For Each col As ColumnHeader In ListView2.Columns
                        x += 1
                        worksheet.Cells(1, x).Value = col.Text
                        worksheet.Cells(1, x).Interior.Color = Color.LightGray
                        worksheet.Cells(r, x).BorderAround(1, 2, xlcolorautomatic, Color.Black)
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
                            worksheet.Cells(r, count + 1).BorderAround(1, 2, xlcolorautomatic, Color.Black)
                        Next

                    Next
                End If
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
            APP.DisplayAlerts = False
            APP.Quit()
            MsgBox(ex.ToString)
        End Try
        ListView1.UseWaitCursor = False

        ProgressBar2.Visible = False

    End Sub


    Public Sub motohodinyodhad()
        Dim dat As Byte = 0 : Dim mt As Byte = 10 : Dim mtdenne As Byte = 11
        Try
            Dim fileContentscb As String = My.Settings.device
            Dim cbNames() As String
            Dim x As Long : cbNames = Split(fileContentscb, vbCrLf)
            For x = 0 To UBound(cbNames)
                If cbNames(x) <> String.Empty Then
                    Dim pathcb9names = Application.StartupPath & "\devices\" & cbNames(x) & "\devparts"
                    Dim fileContentscb9 As String = My.Computer.FileSystem.ReadAllText(pathcb9names)
                    Dim cb9Names() As String
                    Dim x9 As Long : cb9Names = Split(fileContentscb9, vbCrLf)
                    For x9 = 0 To UBound(cb9Names)
                        If cb9Names(x9) <> String.Empty Then
                            Dim pathlabel9 As String = Application.StartupPath & "\devices\" & cbNames(x) & "\" & cb9Names(x9) & "\label.txt"
                            Dim label9 As String = My.Computer.FileSystem.ReadAllText(pathlabel9)
                            Dim text9() As String = Split(label9, "#")
                            Dim motohodnm9 = Trim(text9(mt))
                            If motohodnm9 <> String.Empty And motohodnm9 > 0 Then
                                Dim motodenne As Double = 24
                                If IsNumeric(Trim(text9(mtdenne))) Then
                                    motodenne = Trim(text9(mtdenne))
                                    If IsDate(Trim(text9(dat))) = True Then
                                        Dim posldat As Date = Date.Parse((text9(dat)))
                                        If DateDiff(DateInterval.Day, posldat, Date.Now.Date) > Date.DaysInMonth(Date.Now.Year, Date.Now.Month - 1) Then
                                            Dim odhadmotohodiny As String = motohodnm9 + motodenne * DateDiff(DateInterval.Day, posldat, Date.Now.Date)
                                            Dim stitok As String = day & vbCrLf & "#" & text9(1) & vbCrLf & "#" & text9(2) & vbCrLf & "#" & text9(3) & vbCrLf & "#" & text9(4) & vbCrLf & "#" & text9(5) & vbCrLf & "#" & text9(6) & vbCrLf & "#" & text9(7) & vbCrLf & "#" & text9(8) & vbCrLf & "#" & text9(9) & vbCrLf & "#" & odhadmotohodiny & vbCrLf & "#" & motodenne & vbCrLf & "#"
                                            If Dialog4.CheckedListBox1.GetItemCheckState(2) = CheckState.Unchecked Then
                                                Dialog7.NumericUpDown1.Value = odhadmotohodiny
                                                Dialog7.Label2.Text = Me.Label13.Text
                                                Dialog7.Label3.Text = cbNames(x) & vbCrLf & cb9Names(x9)
                                                Dialog7.Label1.Text = Me.Label16.Text & " + " & (motodenne * DateDiff(DateInterval.Day, posldat, Date.Now.Date))
                                                If Dialog7.ShowDialog = Windows.Forms.DialogResult.OK Then
                                                    stitok = day & vbCrLf & "#" & text9(1) & vbCrLf & "#" & text9(2) & vbCrLf & "#" & text9(3) & vbCrLf & "#" & text9(4) & vbCrLf & "#" & text9(5) & vbCrLf & "#" & text9(6) & vbCrLf & "#" & text9(7) & vbCrLf & "#" & text9(8) & vbCrLf & "#" & text9(9) & vbCrLf & "#" & Dialog7.NumericUpDown1.Value & vbCrLf & "#" & motodenne & vbCrLf & "#"
                                                    My.Computer.FileSystem.WriteAllText(pathlabel9, stitok, False)
                                                End If
                                            ElseIf Dialog4.CheckedListBox1.GetItemCheckState(2) = CheckState.Checked Then
                                                My.Computer.FileSystem.WriteAllText(pathlabel9, stitok, False)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next
                    Dim pathlabel As String = Application.StartupPath & "\devices\" & cbNames(x) & "\label.txt"
                    Dim label As String = My.Computer.FileSystem.ReadAllText(pathlabel)
                    Dim text() As String = Split(label, "#")
                    Dim motohodnm1 = Trim(text(mt))
                    If motohodnm1 <> String.Empty And motohodnm1 > 0 Then
                        Dim motodenne As Double = 24
                        If IsNumeric(Trim(text(mtdenne))) Then
                            motodenne = Trim(text(mtdenne))
                            If IsDate(Trim(text(dat))) = True Then
                                Dim posldat As Date = Date.Parse((text(dat)))
                                If DateDiff(DateInterval.Day, posldat, Date.Now.Date) > Date.DaysInMonth(Date.Now.Year, Date.Now.Month - 1) Then
                                    Dim odhadmotohodiny As String = motohodnm1 + motodenne * DateDiff(DateInterval.Day, posldat, Date.Now.Date)
                                    Dim stitok As String = day & vbCrLf & "#" & text(1) & vbCrLf & "#" & text(2) & vbCrLf & "#" & text(3) & vbCrLf & "#" & text(4) & vbCrLf & "#" & text(5) & vbCrLf & "#" & text(6) & vbCrLf & "#" & text(7) & vbCrLf & "#" & text(8) & vbCrLf & "#" & text(9) & vbCrLf & "#" & odhadmotohodiny & vbCrLf & "#" & motodenne & vbCrLf & "#"
                                    If Dialog4.CheckedListBox1.GetItemCheckState(2) = CheckState.Unchecked Then
                                        Dialog7.NumericUpDown1.Value = odhadmotohodiny
                                        Dialog7.Label2.Text = Me.Label13.Text
                                        Dialog7.Label3.Text = cbNames(x)
                                        Dialog7.Label1.Text = Me.Label16.Text & " + " & (motodenne * DateDiff(DateInterval.Day, posldat, Date.Now.Date))
                                        If Dialog7.ShowDialog = Windows.Forms.DialogResult.OK Then
                                            stitok = day & vbCrLf & "#" & text(1) & vbCrLf & "#" & text(2) & vbCrLf & "#" & text(3) & vbCrLf & "#" & text(4) & vbCrLf & "#" & text(5) & vbCrLf & "#" & text(6) & vbCrLf & "#" & text(7) & vbCrLf & "#" & text(8) & vbCrLf & "#" & text(9) & vbCrLf & "#" & Dialog7.NumericUpDown1.Value & vbCrLf & "#" & motodenne & vbCrLf & "#"
                                            My.Computer.FileSystem.WriteAllText(pathlabel, stitok, False)
                                        End If
                                    ElseIf Dialog4.CheckedListBox1.GetItemCheckState(2) = CheckState.Checked Then
                                        My.Computer.FileSystem.WriteAllText(pathlabel, stitok, False)
                                    End If
                                End If
                            End If
                        End If
                    End If

                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Public Sub motohodhistory()
        Dim dat As Byte = 0 : Dim mt As Byte = 10 : Dim mtdenne As Byte = 11
        Try
            Dim fileContentscb As String = My.Settings.device
            Dim cbNames() As String
            Dim x As Long : cbNames = Split(fileContentscb, vbCrLf)
            For x = 0 To UBound(cbNames)
                If cbNames(x) <> String.Empty Then
                    Dim pathcb9names = Application.StartupPath & "\devices\" & cbNames(x) & "\devparts"
                    Dim fileContentscb9 As String = My.Computer.FileSystem.ReadAllText(pathcb9names)
                    Dim cb9Names() As String
                    Dim x9 As Long : cb9Names = Split(fileContentscb9, vbCrLf)
                    For x9 = 0 To UBound(cb9Names)
                        If cb9Names(x9) <> String.Empty Then
                            Dim pathlabel9 As String = Application.StartupPath & "\devices\" & cbNames(x) & "\" & cb9Names(x9) & "\label.txt"
                            If My.Computer.FileSystem.FileExists(pathlabel9) Then
                                Dim label9 As String = My.Computer.FileSystem.ReadAllText(pathlabel9)
                                Dim text9() As String = Split(label9, "#")
                                Dim motohodnm9 = Replace(text9(mt), vbCrLf, " ")
                                Dim historymot9 As String = Application.StartupPath & "\devices\" & cbNames(x) & "\" & cb9Names(x9) & "\Historymth.txt"
                                Dim fileExists9 As Boolean = My.Computer.FileSystem.FileExists(historymot9)
                                If fileExists9 = False Then
                                    My.Computer.FileSystem.WriteAllText(historymot9, String.Empty, False)
                                End If
                                If My.Computer.FileSystem.ReadAllText(historymot9).Contains(year) = False Then
                                    My.Computer.FileSystem.WriteAllText(historymot9, day & vbTab & motohodnm9 & vbCrLf, True)
                                End If
                            End If
                        End If
                    Next
                    Dim label As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\devices\" & cbNames(x) & "\label.txt")
                    Dim text() As String = Split(label, "#")
                    Dim motohodnm1 = Replace(text(mt), vbCrLf, " ")
                    Dim historymot As String = Application.StartupPath & "\devices\" & cbNames(x) & "\Historymth.txt"
                    Dim fileExists As Boolean = My.Computer.FileSystem.FileExists(historymot)
                    If fileExists = False Then
                        My.Computer.FileSystem.WriteAllText(historymot, String.Empty, False)
                    End If
                    If My.Computer.FileSystem.ReadAllText(historymot).Contains(year) = False Then
                        My.Computer.FileSystem.WriteAllText(historymot, day & vbTab & motohodnm1 & vbCrLf, True)
                    End If
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Public Sub motohodhistoryshow()
        Try
            Dim mt As Byte = 10
            Dim historymotpath As String = Nothing
            Dim labelpath As String = Nothing
            Dim motohohod As Double = Nothing
            Dim cbwhixch As String = Nothing
            If aktivnycombobox = 1 Then
                historymotpath = Application.StartupPath & "\devices\" & ComboBox1.Text & "\Historymth.txt"
                labelpath = Application.StartupPath & "\devices\" & ComboBox1.Text & "\label.txt"
                cbwhixch = ComboBox1.Text
            ElseIf aktivnycombobox = 2 Then
                historymotpath = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\Historymth.txt"
                labelpath = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\label.txt"
                cbwhixch = ComboBox1.Text & vbCrLf & ComboBox9.Text
            End If
            Dim motdene As Double = NumericUpDown2.Value
            If motdene = 0 Then motdene = 24
            Dim motohodinyrocne As Integer = 0
            Dim historymotstring As String = (My.Computer.FileSystem.ReadAllText(historymotpath))
            Dim stringlabel() As String = Split((My.Computer.FileSystem.ReadAllText(labelpath)), "#")
            Dim roky() As String = Split(historymotstring, vbCrLf)
            Dim datestring As Date = Nothing
            For x As Integer = 0 To UBound(roky)

                If roky(x) <> String.Empty And roky(x).Contains(vbTab) Then

                    Dim riadokroky() As String = Split(roky(x), vbTab)
                    If IsNumeric(riadokroky(1)) Then
                        datestring = Date.Parse(riadokroky(0))
                        Dim olddatestring As Date = Date.Parse("31.12." & datestring.Year)
                        Dim rozdieldatumov As Integer = DateDiff(DateInterval.Day, datestring, olddatestring)
                        Dim newmotdenne As Integer = (riadokroky(1) - motohodinyrocne) / rozdieldatumov
                        Form8.ListView1.Items.Add(DateAdd(DateInterval.Year, -1, datestring).Year)
                        Form8.ListView1.Items(Form8.ListView1.Items.Count - 1).SubItems.Add(riadokroky(1) - motohodinyrocne)
                        Form8.ListView1.Items(Form8.ListView1.Items.Count - 1).SubItems.Add(newmotdenne)
                        Form8.ListView1.Items(Form8.ListView1.Items.Count - 1).SubItems.Add(datestring)
                        Form8.ListView1.Items(Form8.ListView1.Items.Count - 1).SubItems.Add(riadokroky(1))
                        olddatestring = datestring
                        motohodinyrocne = riadokroky(1)
                    End If
                End If
            Next
            Dim labeldate As Date = Date.Parse(stringlabel(0))
            Dim rozdieldatumov1 As Integer = DateDiff(DateInterval.Day, labeldate, datestring)
            If rozdieldatumov1 = 0 Then rozdieldatumov1 = 1
            Dim aktualnemotohodiny As Integer = stringlabel(mt)
            Dim newmotdenne1 As Double = (aktualnemotohodiny - motohodinyrocne) / rozdieldatumov1
            Form8.ListView1.Items.Add(Date.Now.Year)
            Form8.ListView1.Items(Form8.ListView1.Items.Count - 1).SubItems.Add(stringlabel(mt) - motohodinyrocne)
            Form8.ListView1.Items(Form8.ListView1.Items.Count - 1).SubItems.Add(newmotdenne1)
            Form8.ListView1.Items(Form8.ListView1.Items.Count - 1).SubItems.Add(labeldate)
            Form8.ListView1.Items(Form8.ListView1.Items.Count - 1).SubItems.Add(aktualnemotohodiny)
            Form8.Label1.Text = historymotpath
            Form8.Text = cbwhixch
            Form8.Show()
        Catch ex As Exception

        End Try
    End Sub




    Private Sub Label16_Click(sender As System.Object, e As System.EventArgs) Handles Label16.Click
        motohodhistoryshow()
    End Sub



    Private Sub LinkLabel5_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        Dim fo As New Dialog8
        Dim labelpath As String = Nothing
        Dim barcode As Byte = 12
        Dim texcodustr As String = Nothing
        Dim codustr As String = Nothing
        Try


            If aktivnycombobox = 1 Then
                labelpath = Application.StartupPath & "\devices\" & ComboBox1.Text & "\label.txt"
                codustr = ComboBox1.Text
            ElseIf aktivnycombobox = 2 Then
                labelpath = Application.StartupPath & "\devices\" & ComboBox1.Text & "\" & ComboBox9.Text & "\label.txt"
                codustr = ComboBox9.Text
            End If
            Dim stringlabel() As String = Split((My.Computer.FileSystem.ReadAllText(labelpath)), "#")
            Try
                texcodustr = stringlabel(barcode)
                fo.NumericUpDown2.Value = Trim(texcodustr)
            Catch ex As Exception

            End Try

            fo.Label3.Text = labelpath.ToString
            fo.TextBox2.Text = LSet(codustr.ToUpper, 7)
        Catch ex As Exception

        End Try
        fo.ShowDialog()
    End Sub
    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Try
            If mousepos = MousePosition.ToString Then
                If ActiveForm.Name = "ryder" Then
                    TextBox4.Focus()
                    RadioButton1.Checked = True
                    Timer3.Stop()
                    Timer3.Enabled = False
                End If

            Else
                Timer1.Stop()
                Timer1.Start()
                mousepos = MousePosition.ToString
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Function cas(ByVal i As Integer) As String
        Dim cs As String ' budoucí obsah pro TextBox
        Dim ss As String = i Mod 1000 ' milisekundy
        i = i \ 1000
        Dim h As String = CStr(i \ 3600) ' hodiny
        Dim m As String = CStr((i Mod 3600) \ 60) ' minuty
        Dim s As String = CStr(i Mod 60) ' sekundy
        If Len(m) = 1 Then m = "0" & m
        If Len(s) = 1 Then s = "0" & s
        cs = h & ":" & m & ":" & s & "." & ss
        Return cs
    End Function

    Private Sub textbox4_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox4.KeyPress
        timestart = Environment.TickCount()
        Timer2.Stop()

        'keypressed = e.KeyChar
    End Sub

    Private Sub textbox4_Keyup(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles TextBox4.KeyUp

        timestop = Environment.TickCount()

        If timestop - timestart < NumericUpDown3.Value Then
            'codebuilder.Append(keypressed)
            Timer2.Start()
        Else
            TextBox4.Clear()

        End If


    End Sub


    Private Sub ryder_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        My.Settings.lastuser = user
        My.Settings.lstselect = ComboBox1.SelectedIndex
        

        Try


            APP.DisplayAlerts = False
            APP.ActiveWorkbook.Close()
            workbook.Close()

            APP.Quit()
            releaseobject(APP)
            releaseobject(workbook)
            releaseobject(worksheet)
        Catch ex As Exception

        End Try

        Try

            For Each wb In APP.Workbooks
                For Each ws In wb.Worksheets
                    Marshal.ReleaseComObject(ws)
                    ws = Nothing
                Next
                wb.Close(False)
                Marshal.ReleaseComObject(wb)
                wb = Nothing
            Next
        Catch ex As Exception

        End Try
        Try
            APP.Quit()
            workbook = Nothing
            worksheet = Nothing
            releaseobject(APP)
            releaseobject(workbook)
            releaseobject(worksheet)
        Catch ex As Exception

        End Try

    End Sub
    Public Declare Function GetAsyncKeyState Lib "user32.dll" (ByVal key As Integer) As Integer
    Dim barcodelist As New StringBuilder
    Dim barcodedecode As New StringBuilder
    Dim fkeypresss As Double = 0
    Dim buffer As String = String.Empty
    Private Sub Timer4_Tick(sender As System.Object, e As System.EventArgs) Handles Timer4.Tick
        scanerevaluation()
    End Sub
    Private Sub Timer3_Tick(sender As System.Object, e As System.EventArgs) Handles Timer3.Tick
        For Each k In cisla
            If GetAsyncKeyState(k) Then
                Timer4.Stop()
                Timer4.Enabled = False
                Dim diff As Double = (Date.Now.Second - fkeypresss)
                fkeypresss = Date.Now.Second
                If diff < time3 Then
                    buffer = code(k)
                    barcodelist.AppendLine(buffer)
                    RadioButton1.Checked = True
                    Timer4.Start()
                    Timer4.Enabled = True
                End If
            End If
        Next
    End Sub

    Public Sub scanerevaluation()
        Dim scansysi As Integer = My.Settings.scanerinput
        Dim textlogdev As String = Nothing
        Try
            Dim fullcode As String = String.Empty
            If scansysi = 1 Then
                'cez textbox
                fullcode = TextBox4.Text
                If fullcode.Contains("/") Then
                    Dim scan() As String = Split(fullcode, "/")
                    fullcode = scan(0)
                Else
                    fullcode = TextBox4.Text
                End If
            ElseIf scansysi = 0 Then
                'priamo zo scanera
                Dim bcodedata As String = barcodelist.ToString
                Timer4.Stop()
                Timer4.Enabled = False
                RadioButton1.Checked = False
                If bcodedata.Contains(vbCrLf) Then
                    Try
                        Dim tx As String() = Split(bcodedata, vbCrLf)
                        'eangs128 nula 96 oddeluje dve čísla 100 105 podľa keys ke 4 a 9 čo ASCII 49 =1
                        Dim fv = String.Empty
                        Dim sv = String.Empty
                        Dim nula As Boolean = False
                        For Each ch As String In tx
                            If IsNumeric(ch) Then
                                If CInt(ch).Equals(0) And nula = False Then
                                    nula = True
                                ElseIf nula = True Then
                                    If fv = String.Empty Then
                                        fv = ch
                                    Else
                                        sv = ch
                                        nula = False
                                        barcodedecode.Append(Chr(fv & sv))
                                        fv = String.Empty : sv = String.Empty
                                    End If
                                End If
                            End If
                        Next

                    Catch ex As Exception

                    End Try

                End If
                fullcode = barcodedecode.ToString
                fullcode = fullcode.Remove(fullcode.IndexOf("/"))
            End If



            If fullcode <> Nothing Then
                Dim box As String = String.Empty
                Dim codeint As Integer = CInt(fullcode)
                Dim cb1 As String = String.Empty

                If barcodelisti1i2.ContainsKey(codeint) Then
                    box = barcodelisti1i2(codeint)(0)
                    cb1 = barcodelisti1i2(codeint)(1)
                    ComboBox1.SelectedIndex = ComboBox1.FindStringExact(cb1)
                    Me.Activate()
                    Me.BringToFront()
                    Me.WindowState = FormWindowState.Normal
                    textlogdev = ComboBox1.Text
                    If box.Contains("cb9") Then
                        ComboBox9.SelectedIndex = ComboBox9.FindStringExact(barcodelisti1i2(codeint)(2))
                        textlogdev = ComboBox1.Text & vbTab & ComboBox9.Text
                    End If
                End If
            End If
            If scansysi = 1 Then Timer2.Stop()
            Try


                Dim fileExists As Boolean : If fileExists = My.Computer.FileSystem.FileExists(logpath) Then
                    My.Computer.FileSystem.WriteAllText(logpath, String.Empty, False)
                End If

                Dim log As String = Date.Now & vbTab & fullcode & vbTab & user & vbTab & textlogdev

                Dim newlog As New StringBuilder
                Dim find As Boolean = False
                For Each li As Char In log
                    find = False
                    For i As Integer = 0 To notcode.Length - 1
                        If li = notcode(i) Then newlog.Append(deccode(i)) : find = True
                    Next
                    If find = False Then newlog.Append(li)
                Next
                My.Computer.FileSystem.WriteAllText(logpath, newlog.ToString & vbCrLf, True)
            Catch ex As Exception
            End Try
        Catch ex As Exception
            If scansysi = 1 Then
                TextBox4.Clear()
                Timer2.Dispose()
            Else
                barcodelist.Clear()
                barcodedecode.Clear()
                buffer = String.Empty
            End If

        End Try
        If scansysi = 1 Then
            TextBox4.Clear()
            Timer2.Dispose()
        Else
            barcodelist.Clear()
            barcodedecode.Clear()
            buffer = String.Empty
        End If
    End Sub
    Private Sub Timer2_Tick(sender As System.Object, e As System.EventArgs) Handles Timer2.Tick
        scanerevaluation()
    End Sub
    Private Sub LinkLabel6_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel6.LinkClicked
        Try
            Dim fileExists As Boolean : If fileExists = My.Computer.FileSystem.FileExists(logpath) Then
                My.Computer.FileSystem.WriteAllText(logpath, String.Empty, False)
            End If
            Dim log As String = My.Computer.FileSystem.ReadAllText(logpath)

            Dim newlog As New StringBuilder
            Dim find As Boolean = False
            Dim logline() As String = Split(log, vbCrLf)
            Dim listitem() As String
            Try
                For x As Integer = 0 To UBound(logline)
                    newlog.Clear()
                    For Each li As Char In logline(x)
                        find = False


                        For i As Integer = 0 To deccode.Length - 1
                            If li = deccode(i) Then
                                newlog.Append(notcode(i)) : find = True

                            End If

                        Next
                        If find = False Then newlog.Append(li)
                    Next
                    listitem = Split(newlog.ToString, vbTab)
                    If listitem(0) <> String.Empty Then
                        Form9.ListView1.Items.Add(listitem(0))
                        For s As Integer = 1 To UBound(listitem)

                            Form9.ListView1.Items(Form9.ListView1.Items.Count - 1).SubItems.Add(listitem(s))
                        Next

                    End If


                Next
                Dim r As Integer = Form9.ListView4.Items.Count - 1
                For Each li In barcodelisti1i2
                    'Dim devicestr() As String = Split(li.Value, "##")
                    If li.Value.Count > 2 Then
                        Form9.ListView4.Items.Add(li.Value(1))
                        r = Form9.ListView4.Items.Count - 1
                        Form9.ListView4.Items(r).SubItems.Add(li.Value(2))
                        Form9.ListView4.Items(r).SubItems.Add(li.Key)
                    Else
                        Form9.ListView4.Items.Add(li.Value(1))
                        r = Form9.ListView4.Items.Count - 1
                        Form9.ListView4.Items(r).SubItems.Add("")
                        Form9.ListView4.Items(r).SubItems.Add(li.Key)
                    End If

                Next
                Form9.Show()
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub


    Private Sub TextBox4_Leave(sender As System.Object, e As System.EventArgs) Handles TextBox4.Leave
        RadioButton1.Checked = False
    End Sub
    Private Sub TextBox4_enter(sender As System.Object, e As System.EventArgs) Handles TextBox4.Enter
        RadioButton1.Checked = True
    End Sub


    Private Sub Button24_Click(sender As System.Object, e As System.EventArgs) Handles Button24.Click
        Form1.Show()
    End Sub



    Private Sub Button43_Click(sender As System.Object, e As System.EventArgs) Handles Button43.Click
        If ListView3.Visible = True Then
            ListView3.Visible = False
        Else
            ListView3.Visible = True
            ListView3.BringToFront()
        End If

    End Sub


    Private Sub Button42_Click(sender As System.Object, e As System.EventArgs) Handles Button42.Click
        Try


            If Chart1.Visible = True And Chart2.Visible = True Then
                Chart1.Visible = False
                Chart2.Visible = False : Chart2.SendToBack()
                Exit Sub
            ElseIf Chart1.Visible = False And Chart2.Visible = False Then


                Dim datum As Date
                Dim fileContentscb = My.Settings.user
                Dim list() As String : list = Split(fileContentscb, vbCrLf)

                If ListBox5.Visible = True Or TextBox5.Visible = True Then
                    ListView5.Items.Clear()
                    Dim graf1 As String = Label3.Text
                    Dim graf2 As String = ColumnHeader32.Text
                    Dim x As String = 0
                    Dim y As Integer = 0
                    Try
                        Chart1.Series.Clear()
                        Chart1.Series.Add(graf1)
                        Chart1.Visible = True

                        For xu = 0 To UBound(list)
                            If list(xu) <> String.Empty Then
                                ListView5.Items.Add(list(xu))
                                Dim pocet As Integer = 0
                                For Each line As String In TextBox5.Lines

                                    If line.Contains(list(xu)) Then
                                        Dim i As Integer = ListView5.Items.IndexOf(ListView5.FindItemWithText(list(xu)))
                                        If ListView5.Items(i).SubItems.Count < 1 Then
                                            pocet = 1
                                            ListView5.Items(i).SubItems.Add(pocet)
                                        ElseIf ListView5.Items(i).SubItems.Count > 1 Then
                                            pocet += 1
                                            ListView5.Items(i).SubItems(1).Text = pocet
                                        End If

                                    Else
                                        ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(pocet)
                                    End If

                                Next
                            End If

                        Next
                    Catch ex As Exception

                    End Try
                    Try
                        'Uncomment this line if you want to show bar chart
                        Chart1.Series(graf1).ChartType = SeriesChartType.Pie
                        ' Set labels style
                        Chart1.Series(graf1)("PieLabelStyle") = "inside"
                        ' Show data points labels
                        Chart1.Series(graf1).IsVisibleInLegend = True
                        ' Set data points label style
                        Chart1.Series(graf1)("BarLabelStyle") = "bottom"
                        ' Show chart as 3D. Uncomment this line if you want to display your barchart as 3D
                        Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
                        '' Draw chart as 3D Cylinder
                        Chart1.Series(graf1)("DrawingStyle") = "Cylinder"
                        For Each item As ListViewItem In ListView5.Items
                            If item.SubItems(1).Text > 0 Then Chart1.Series(graf1).Points.AddXY(item.Text, item.SubItems(1).Text)
                        Next
                    Catch ex As Exception
                        '
                    End Try

                    Try
                        Chart2.Series.Clear()

                        Chart2.Series.Add(graf2)
                        x = 0
                        y = 0

                        Dim olddate As Date = "1.1.2000"
                        For Each line As String In TextBox5.Lines
                            If line <> String.Empty Then
                                'až keď spočíta všetk s rovnakým dátumom vloží do grafu pri zmene dátumu posldné hodnoty oldadate a y

                                Dim splb5() As String = Split(line, "|")
                                'Dim da() As String = Split(splb5(0), "|")

                                x = datum
                                datum = FormatDateTime(splb5(1), DateFormat.ShortDate)
                                If olddate <> datum Then
                                    x = olddate
                                    Chart2.Series(graf2).Points.AddXY(x, y)
                                    y = 1
                                Else
                                    y += 1
                                End If
                                olddate = datum
                            End If
                        Next
                        x = datum
                        Chart2.Series(graf2).Points.AddXY(x, y)
                        Chart2.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
                        Chart2.Visible = True
                        Chart2.BringToFront()
                    Catch ex As Exception

                    End Try

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Button36_Click(sender As System.Object, e As System.EventArgs) Handles Button36.Click
        Dialog4.ShowDialog()
    End Sub
    Private Sub LinkLabel3_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel8.LinkClicked
        If FontDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.ryderforecolortext = FontDialog1.Color

            My.Settings.ryderfont = FontDialog1.Font

        End If
        colofbgandtext()
    End Sub
    Private Sub LinkLabel7_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked

        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.ryderbackgroundcolortexboxu = ColorDialog1.Color
        End If

        colofbgandtext()

    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click

        PrintDialog1.AllowSomePages = True
        PrintDialog1.ShowHelp = True
        PrintDialog1.AllowSelection = True
        PrintDialog1.AllowCurrentPage = True
        PrintDialog1.UseEXDialog = True
        PrintDocument3.PrinterSettings = PrintDialog1.PrinterSettings

        'If ListView2.Visible = True Then 
        PrintDocument3.DefaultPageSettings.Margins.Left = 10

        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PrintDocument3.DefaultPageSettings.Margins.Left = 10
            PrintDocument3.DefaultPageSettings.Margins.Top = 10
            PrintDocument3.DefaultPageSettings.Margins.Bottom = 15
            PrintDocument3.PrinterSettings = PrintDialog1.PrinterSettings

            PrintPreviewDialog1.Document = PrintDocument3
            PrintPreviewDialog1.ShowDialog()
        End If

    End Sub
    Private Sub pdoc_PrintPage1(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument3.PrintPage
        Static CurrentYPosition As Integer = 0
        If ListView4.View = View.Details Then
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
        Dim ColumnHeaderHeight As Single = CSng(ListView4.Font.Height + (10 * (DpiX / 96))) '5 pixel pad on the top an bottom
        Dim StringFormat As New StringFormat
        Dim PageNumberWidth As Single = e.Graphics.MeasureString(CStr(CurrentPage), ListView4.Font).Width
        StringFormat.FormatFlags = StringFormatFlags.NoWrap
        StringFormat.Trimming = StringTrimming.EllipsisCharacter
        StringFormat.LineAlignment = StringAlignment.Center
        CurrentPage += 1
        X = CInt(e.MarginBounds.X)
        Y = CInt(e.MarginBounds.Y)
        For ColumnIndex As Integer = 0 To ListView4.Columns.Count - 1
            TextRect.X = X
            TextRect.Y = Y
            TextRect.Width = ListView4.Columns(ColumnIndex).Width
            TextRect.Height = ColumnHeaderHeight
            e.Graphics.FillRectangle(Brushes.LightGray, TextRect)
            e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
            TextRect.X += TextLeftPad
            TextRect.Width -= TextLeftPad
            e.Graphics.DrawString(ListView4.Columns(ColumnIndex).Text, ListView4.Font, Brushes.Black, TextRect, StringFormat)
            X += TextRect.Width + TextLeftPad
        Next
        Y += ColumnHeaderHeight
        For i = LastIndex To ListView4.Items.Count - 1
            With ListView4.Items(i)
                X = CInt(e.MarginBounds.X)
                If Y + .Bounds.Height > e.MarginBounds.Bottom Then
                    LastIndex = i - 1
                    e.HasMorePages = True
                    StringFormat.Dispose()
                    e.Graphics.DrawString(CStr(CurrentPage), ListView4.Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - ListView4.Font.Height * 2)
                    Exit Sub
                End If
                ImageWidth = 0
                If ListView4.SmallImageList IsNot Nothing Then
                    If Not String.IsNullOrEmpty(.ImageKey) Then
                        e.Graphics.DrawImage(ListView4.SmallImageList.Images(.ImageKey), X, Y)
                    ElseIf .ImageIndex >= 0 Then
                        e.Graphics.DrawImage(ListView4.SmallImageList.Images(.ImageIndex), X, Y)
                    End If
                    ImageWidth = ListView4.SmallImageList.ImageSize.Width
                End If
                For ColumnIndex As Integer = 0 To ListView4.Columns.Count - 1
                    TextRect.X = X
                    TextRect.Y = Y
                    TextRect.Width = ListView4.Columns(ColumnIndex).Width
                    TextRect.Height = .Bounds.Height
                    Dim bColor As Integer = ListView4.Items(i).BackColor.ToArgb
                    Dim myBColor As Color = ColorTranslator.FromHtml(bColor)
                    e.Graphics.FillRectangle(New SolidBrush(myBColor), TextRect)
                    If ListView4.GridLines Then
                        e.Graphics.DrawRectangle(Pens.DarkGray, TextRect)
                    End If
                    If ColumnIndex = 0 Then TextRect.X += ImageWidth
                    TextRect.X += TextLeftPad
                    TextRect.Width -= TextLeftPad
                    If ColumnIndex < .SubItems.Count Then
                        Dim TColor As Integer = ListView4.Items(i).SubItems(ColumnIndex).ForeColor.ToArgb
                        Dim myTColor As Color = ColorTranslator.FromHtml(TColor)
                        e.Graphics.DrawString(.SubItems(ColumnIndex).Text, ListView4.Font, New SolidBrush(myTColor), TextRect, StringFormat)
                    End If
                    X += TextRect.Width + TextLeftPad
                Next

                Y += .Bounds.Height
            End With
        Next
        e.Graphics.DrawString(CStr(CurrentPage), ListView4.Font, Brushes.Black, (e.PageBounds.Width - PageNumberWidth) / 2, e.PageBounds.Bottom - ListView4.Font.Height * 2)
        StringFormat.Dispose()
        LastIndex = 0
        CurrentPage = 0
    End Sub


    Private Sub Button44_Click(sender As System.Object, e As System.EventArgs) Handles Button44.Click
        If hesla.ContainsKey(ComboBox4.SelectedIndex) And hesla.Count > 1 Then
            'MsgBox(hesla(ComboBox4.SelectedIndex))

            If InputBox("password") = hesla(ComboBox4.SelectedIndex) Then

                povolenie = True
                user = ComboBox4.Text
                cbread()
            Else
                povolenie = False
                MsgBox("false password")
            End If

        End If

        If povolenie = True Then

            Dim meno1 As String = user
            Dim myuser() As String = Split(My.Settings.user, vbCrLf)
            For x = 0 To UBound(myuser)
                Dim names() As String = Split(myuser(x), vbTab)
                If names(0) = meno1 Then
                    If myuser(x).Contains("|") Then
                        Dim check() As String = Split(myuser(x), "|")

                        If check(1).Contains("True") = False Then

                            'deletebutton
                            Button4.Enabled = False : Button20.Enabled = False : Button16.Enabled = False : Button13.Enabled = False
                        End If
                        If check(2).Contains("True") = False Then
                            'Savebutton
                            Button2.Enabled = False : Button21.Enabled = False : Button6.Enabled = False : Button12.Enabled = False
                            Button36.Enabled = False

                        End If
                        If check(3).Contains("True") = False Then
                            'Write records
                            Button1.Enabled = False : Button8.Enabled = False : Form13.Button35.Enabled = False : Button38.Enabled = False
                        End If
                        If check(1).Contains("True") = True Then

                            'deletebutton
                            Button4.Enabled = True : Button20.Enabled = True : Button16.Enabled = True : Button13.Enabled = True
                        End If
                        If check(2).Contains("True") = True Then
                            'Savebutton
                            Button2.Enabled = True : Button21.Enabled = True : Button6.Enabled = True : Button12.Enabled = True

                            Button36.Enabled = True
                        End If
                        If check(3).Contains("True") = True Then
                            'Write records
                            Button1.Enabled = True : Button8.Enabled = True : Form13.Button35.Enabled = True : Button38.Enabled = True
                        End If
                    End If
                End If
            Next
            Button44.Hide()
            cbread()
        End If

    End Sub


    Private Sub ryder_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If Not APP Is Nothing Then
                APP.Quit()
                Marshal.ReleaseComObject(APP)
            End If
        Catch ex As Exception

        End Try


        Try
            Try


                APP.DisplayAlerts = False
                APP.ActiveWorkbook.Close()
                workbook.Close()

                APP.Quit()
                releaseobject(APP)
                releaseobject(workbook)
                releaseobject(worksheet)
            Catch ex As Exception

            End Try
            For Each wb In APP.Workbooks
                For Each ws In wb.Worksheets
                    Marshal.ReleaseComObject(ws)
                    ws = Nothing
                Next
                wb.Close(False)
                Marshal.ReleaseComObject(wb)
                wb = Nothing
            Next

        Catch ex As Exception

        End Try
        'Try
        '    APP.Quit()
        '    workbook = Nothing
        '    worksheet = Nothing
        '    releaseobject(APP)
        '    releaseobject(workbook)
        '    releaseobject(worksheet)
        'Catch ex As Exception

        'End Try

    End Sub





    Private Sub ListView3_Resize(sender As System.Object, e As System.EventArgs) Handles ListView3.Resize
        Dim lvh As Integer = ListView3.Height
        FlowLayoutPanel2.Location = New Point(ListView3.Width + 5, ListView3.Location.Y)
        TextBox5.Size = ListView3.Size
        ListBox5.Size = ListView3.Size
        TextBox6.Width = ListView3.Width
        TextBox7.Width = ListView3.Width
        TextBox6.Height = lvh / 2
        TextBox7.Height = lvh / 2
        TextBox6.Location = New Point(ListView3.Location.X, TextBox7.Location.Y + TextBox7.Height)

    End Sub

    Private Sub me_Resize(sender As System.Object, e As System.EventArgs) Handles Me.Resize
        Try

            'Me.MaximumSize = (My.Computer.Screen.WorkingArea.Size)
            Me.AutoScaleMode = Windows.Forms.AutoScaleMode.Dpi
            Me.PerformAutoScale()

            WebBrowser1.Size = New Size(Me.Width - 2, Me.Height - 20)
            If Me.Size.Height > My.Computer.Screen.WorkingArea.Height Or Me.Size.Width > My.Computer.Screen.WorkingArea.Width Then
                Me.Panel1.AutoScroll = True
            Else
                Me.Panel1.AutoScroll = False
            End If
            If ListView1.Visible = False And ListView4.Visible = False And ListView3.Visible = True Then
                Me.Location = New Point(0, 0)
                Dim newwidth As Double
                ListView3.Height = Me.Height - ListView3.Location.Y - 80

                ListView3.Width = Me.Width - FlowLayoutPanel2.Width - 20

                Dim sw As Integer
                For x = 1 To ListView3.Columns.Count - 1

                    sw += +ListView3.Columns(x).Width
                Next
                sw += 50
                newwidth = ListView3.Width / sw
                For x = 1 To ListView3.Columns.Count - 1
                    ListView3.Columns(x).Width = ListView3.Columns(x).Width * newwidth
                Next

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button37_Click(sender As System.Object, e As System.EventArgs) 
        Form10.Show()
    End Sub
    Class ListViewItemComparer
        Implements IComparer

        Private col As Integer

        Public Sub New()
            col = 0
        End Sub

        Public Sub New(ByVal column As Integer)
            col = column
        End Sub
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            If IsDate(CType(x, ListViewItem).SubItems(col).Text) Then
                Return [Date].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
            Else
                Return [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
            End If



        End Function
    End Class


    Private Sub ListView3_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView3.ColumnClick
        Me.ListView3.ListViewItemSorter = New ListViewItemComparer(e.Column)
    End Sub




    Private Sub Button45_Click(sender As System.Object, e As System.EventArgs)
        Form12.ShowDialog()
    End Sub
    Private Sub ListView3_ItemSelectionChanged(sender As System.Object, e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs)
        Try


            Dim excelclupboard As New StringBuilder
            For Each li As ListViewItem In ListView3.SelectedItems
                For x = 0 To li.SubItems.Count - 1
                    excelclupboard.Append(li.SubItems(x).Text & vbTab)
                Next
                excelclupboard.AppendLine(vbCrLf)
            Next
            My.Computer.Clipboard.SetText(excelclupboard.ToString)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Label2_Click(sender As System.Object, e As System.EventArgs) Handles Label2.Click
        Try


            Dim excelclupboard As New StringBuilder
            For Each li As ListViewItem In ListView3.Items
                For x = 0 To li.SubItems.Count - 1
                    excelclupboard.Append(li.SubItems(x).Text & vbTab)
                Next
                excelclupboard.AppendLine(vbCrLf)
            Next
            My.Computer.Clipboard.SetText(excelclupboard.ToString)
        Catch ex As Exception

        End Try
    End Sub

    Sub releaseobject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        End Try
    End Sub
   
End Class


















