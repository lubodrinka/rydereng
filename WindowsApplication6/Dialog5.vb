Imports System.Windows.Forms

Public Class Dialog5

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Form5.ListView1.LabelEdit = True
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Dim x As Integer = Label7.Text
            Form5.ListView1.Items(x).SubItems.Clear()
            Form5.ListView1.Items(x).BeginEdit()
            Form5.ListView1.Items(x).Text = TextBox1.Text
            Form5.ListView1.Items(x).SubItems.Add(N1.Value)
            Form5.ListView1.Items(x).SubItems.Add(N2.Value)
            Form5.ListView1.Items(x).SubItems.Add(TextBox4.Text)
            Form5.ListView1.Items(x).SubItems.Add(TextBox6.Text)
            Form5.ListView1.Items(x).SubItems.Add(N3.Value)
            Form5.ListView1.Items(x).SubItems.Add(Label8.Text)
        Catch ex As Exception

        End Try
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub N1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles N1.ValueChanged
        Try
            TextBox4.Text = N1.Value * N2.Value
            TextBox6.Text = N1.Value * N2.Value * (1 + (Form5.NumericUpDown3.Value / 100))
        Catch ex As Exception
        End Try
    End Sub
    Private Sub N2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles N2.ValueChanged
        Try
            TextBox4.Text = N1.Value * N2.Value
            TextBox6.Text = N1.Value * N2.Value * (1 + (Form5.NumericUpDown3.Value / 100))
        Catch ex As Exception
        End Try
    End Sub
End Class
