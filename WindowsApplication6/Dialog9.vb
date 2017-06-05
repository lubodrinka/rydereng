Imports System.Windows.Forms

Public Class Dialog9

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.SendToBack()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub Dialog9_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If ComboBox7.Items.Count = 0 Then
            For x As Integer = Date.Now.Year - 100 To Date.Now.Year : ComboBox7.Items.Add(x) : Next
        End If
        ComboBox7.SelectedIndex = ryder.ComboBox7.SelectedIndex
        CheckBox7.Text = ryder.CheckBox7.Text
        CheckBox5.Text = ryder.CheckBox5.Text
        CheckedListBox1.Items.Clear()
        Dim cestacb() As String = Split(My.Settings.device, vbCrLf)
        Try
            For Each line As String In cestacb
                If line <> String.Empty Then CheckedListBox1.Items.Add(line)
                If line = ryder.ComboBox1.Text Then CheckedListBox1.SetItemChecked(CheckedListBox1.Items.Count - 1, True)
            Next
        Catch ex As Exception

        End Try
        If CheckBox5.Checked Then
            For x As Integer = 0 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(x, True)
            Next
        Else
            For x As Integer = 0 To CheckedListBox1.Items.Count - 1
                If CheckedListBox1.Items(x) = ryder.ComboBox1.Text Then CheckedListBox1.SetItemChecked((x), True)
            Next
        End If

    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked Then
            For x As Integer = 0 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(x, True)
            Next
        ElseIf CheckBox5.Checked = False Then
            For x As Integer = 0 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(x, False)
            Next

        End If
    End Sub
    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            CheckBox2.CheckState = CheckState.Unchecked
            CheckBox3.CheckState = CheckState.Unchecked : CheckBox6.CheckState = CheckState.Unchecked
            CheckBox4.Visible = True : CheckBox7.Visible = True
        Else
            CheckBox4.Visible = False : CheckBox7.Visible = False
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            CheckBox1.CheckState = CheckState.Unchecked
            CheckBox3.CheckState = CheckState.Unchecked : CheckBox6.CheckState = CheckState.Unchecked
            CheckBox4.Visible = False : CheckBox7.Visible = True
        Else
            CheckBox4.Visible = True : CheckBox7.Visible = False
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            CheckBox1.CheckState = CheckState.Unchecked
            CheckBox2.CheckState = CheckState.Unchecked : CheckBox6.CheckState = CheckState.Unchecked
            CheckBox4.Visible = False : CheckBox7.Visible = False
        Else
            CheckBox4.Visible = True : CheckBox7.Visible = True
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked = True Then
            : CheckBox1.CheckState = CheckState.Unchecked : CheckBox2.CheckState = CheckState.Unchecked
            CheckBox3.CheckState = CheckState.Unchecked
            CheckBox4.Visible = False : CheckBox7.Visible = False
        Else
            CheckBox4.Visible = True : CheckBox7.Visible = True
        End If
    End Sub
End Class
