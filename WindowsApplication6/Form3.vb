Imports System.Net.Mail
Imports System.Text

Public Class mail
    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ND As New StringBuilder
        For Each checkitem As String In ryder.CheckedListBox2.CheckedItems
            ND.AppendLine(checkitem)
        Next

        Dim dlzka5 = 40 - ryder.Label5.Text.Length
        Dim dlzka4 = 40 - ryder.Label4.Text.Length
        Dim dlzka8 = 40 - ryder.Label8.Text.Length
        Dim dlzka9 = 40 - ryder.Label9.Text.Length
        Dim dlzka1 = 40 - ryder.Label1.Text.Length
        TextBox1.Text = "your mail"
        TextBox2.Text = ryder.TextBox17.Text
        TextBox5.Text = vbCrLf + _
            "spare parts" + vbCrLf _
 + ryder.ComboBox1.Text + vbCrLf _
+ Trim(ryder.Label5.Text) + Space(dlzka5) + Trim(ryder.TextBox11.Text) + vbCrLf _
 + ryder.Label4.Text + Space(dlzka4) + (ryder.TextBox10.Text) + vbCrLf _
 + ryder.Label8.Text + Space(dlzka8) + Trim(ryder.TextBox12.Text) + vbCrLf _
 + ryder.Label9.Text + Space(dlzka9) + Trim(ryder.TextBox14.Text) + vbCrLf _
 + ryder.Label1.Text + Space(dlzka1) + Trim(ND.ToString)
        If My.Settings.mymail <> Nothing Then TextBox1.Text = My.Settings.mymail
        If My.Settings.mserver <> Nothing Then TextBox4.Text = My.Settings.mserver
        For Each li As String In TextBox5.Lines

            If li <> String.Empty Or li <> Nothing Then
                TextBox3.AppendText(li + vbCrLf)

            End If
        Next
        Button2.Visible = False
        Try
            Dim fileExist1 As Boolean
            fileExist1 = My.Computer.FileSystem.FileExists(Application.StartupPath & "\logo")
            If fileExist1 = True Then Me.PictureBox1.Load(Application.StartupPath & "\logo")
            PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage

            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage

        Catch ex As Exception

        End Try
        Me.BackColor = My.Settings.mailformbgcolor
        Button1.BackColor = My.Settings.mailformbgcolor
        Button2.BackColor = My.Settings.mailformbgcolor
        TextBox3.AppendText(vbCrLf & vbCrLf & Dialog4.TextBox3.Text)
        TextBox3.BackColor = My.Settings.mailfarbapozadiatb3
        TextBox3.Font = My.Settings.mailfontname
        TextBox3.ForeColor = My.Settings.mailfarbafontu
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try


            Dim f = My.Settings.mailfontname
            Dim fcolor = My.Settings.mailfarbafontu
            Dim bcolor = My.Settings.mailfarbapozadiatb3.ToArgb
            Dim message As New MailMessage(TextBox2.Text, TextBox1.Text, "Subject", _
                                          "<p><font face=" & f.Name & " size=" & f.Size / 4 & " style=" & f.Style & _
                                           " color=" & fcolor.Name & "><p>" & TextBox3.Text & "<body bgcolor=" & bcolor & ">" & _
                                          "<p><font face= arial & size= 2 <p>")

            Dim emailClient As New SmtpClient(TextBox4.Text)
            With message
                .IsBodyHtml = True

            End With
            Try
                emailClient.Send(message)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        Button2.Visible = True
    End Sub



    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Button2.Visible = False
        My.Settings.mserver = TextBox4.Text
        My.Settings.mymail = TextBox1.Text
    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged
        Button2.Visible = True
    End Sub


    Private Sub Button3_Click_1(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then


            TextBox3.BackColor = ColorDialog1.Color
            My.Settings.mailfarbapozadiatb3 = ColorDialog1.Color

        End If
    End Sub

    Private Sub Button4_Click_1(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        If FontDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox3.Font = New Font(FontDialog1.Font.Name, FontDialog1.Font.Size, FontDialog1.Font.Style)
            TextBox3.ForeColor = FontDialog1.Color
            My.Settings.mailfontname = FontDialog1.Font
            My.Settings.mailfarbafontu = FontDialog1.Color

        End If
    End Sub



    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

            Me.BackColor = ColorDialog1.Color
            My.Settings.mailformbgcolor = ColorDialog1.Color

        End If
    End Sub
End Class