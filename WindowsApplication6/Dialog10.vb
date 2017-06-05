Imports System.Windows.Forms

Public Class Dialog10

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        My.Settings.adminheslo = InputBox("set password for admin")
    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged
        Try
            For Each item As ListViewItem In ListView1.Items

                If item.Text.Contains(TextBox1.Text) Then
                    item.BackColor = Color.LimeGreen
                    ListView1.EnsureVisible(item.Index)
                ElseIf TextBox1.Text = Nothing Then
                    item.BackColor = Color.Black

                Else
                    item.BackColor = Color.Black
                End If

            Next

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Dialog10_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'dev
        ColumnHeader1.Text = ryder.Label13.Text
        'typ
        ColumnHeader2.Text = ryder.Label15.Text
        'power
        ColumnHeader3.Text = ryder.Label8.Text
        'locat
        ColumnHeader4.Text = ryder.Label12.Text
        'sn
        ColumnHeader5.Text = ryder.Label5.Text
        ColumnHeader6.Text = ryder.Label9.Text
        'Label inf
        ColumnHeader7.Text = ryder.Label6.Text
        'manufacter
        ColumnHeader8.Text = ryder.Label7.Text
        'mail
        ColumnHeader9.Text = ryder.Label11.Text
        'wh
        ColumnHeader10.Text = ryder.Label16.Text
        ' 'dail work
        ColumnHeader11.Text = ryder.Label19.Text & " " & ryder.Label20.Text
        'dateupdate()
        ColumnHeader12.Text = ryder.Label16.Text
        'Date man
        ColumnHeader13.Text = ryder.Label4.Text


    End Sub

    
    Private Sub ListView1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub
End Class
