Imports System.Windows.Forms

Public Class Dialog1

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

        'zapíše do user2.txt z combo boxu bo zatlačení button 12.
        
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

     Private Sub Dialog1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If My.Settings.dokumenty = String.Empty Then
                My.Settings.dokumenty = ("ELECTRIC" + vbCrLf + "REVISiON" + vbCrLf + "MANUAL" + vbCrLf + "OTHER" + vbCrLf + "mainpicture" + vbCrLf)
            End If

            Dim sNames() As String
            Dim x As Long : sNames = Split(My.Settings.dokumenty, vbCrLf)
            For x = 0 To UBound(sNames)
                ComboBox1.Items.Add(sNames(x))
            Next
        Catch ex As Exception

        End Try

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        My.Settings.dokumenty.Replace(ComboBox1.SelectedItem & vbCrLf, "")
        ComboBox1.Items.Remove(ComboBox1.SelectedItem)

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim cbnewline As String = ComboBox1.Text & vbCrLf
        If ComboBox1.Text <> String.Empty Then
            My.Settings.dokumenty = My.Settings.dokumenty & cbnewline
            ComboBox1.Items.Add(ComboBox1.Text)

        End If
    End Sub
End Class
