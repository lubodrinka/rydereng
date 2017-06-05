Imports System.Windows.Forms

Public Class Dialog3

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub Dialog3_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
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
                                Case "Dialog3"
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
    End Sub
End Class
