Imports System.Windows.Forms
Imports System.Text

Public Class Dialog11

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click


        save()

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Public Sub save()

        Dim items As New StringBuilder
        For y As Integer = 0 To ListView1.Items.Count - 1
            items.Append(ListView1.Items(y).Text & vbTab)
            For x As Integer = 1 To ListView1.Items(y).SubItems.Count - 1
                items.Append(ListView1.Items(y).SubItems(x).Text & vbTab)
            Next
            items.Append(vbCrLf)
        Next

        If ComboBox1.SelectedIndex = 0 Then
            My.Settings.devreptimed = items.ToString
        ElseIf ComboBox1.SelectedIndex = 1 Then
            My.Settings.devreptimew = items.ToString
        ElseIf ComboBox1.SelectedIndex = 2 Then
            My.Settings.devreptimem = items.ToString
        ElseIf ComboBox1.SelectedIndex = 3 Then
            My.Settings.devreptimey = items.ToString
        End If




    End Sub
    Public Sub loadmain()
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDown
        Me.BackColor = ryder.BackColor
        Try

            ListView1.Items.Clear()
            Dim devstr() As String = Nothing
            If ryder.CBpermision.ToString <> Nothing Then
                devstr = Split(ryder.CBpermision.ToString, vbCrLf)
            Else
                devstr = Split(My.Settings.device, vbCrLf)
            End If

            'Try
            'Dim freq() As String =  & vbTab & "\weekly.txt" & vbTab & "\monthly.txt" & vbTab & "\yearly.txt"
            Dim freqmain As String = Nothing
            Dim settfreq As String = Nothing
            If ComboBox1.SelectedIndex = 0 Then
                freqmain = "\daily.txt" : settfreq = My.Settings.devreptimed
            ElseIf ComboBox1.SelectedIndex = 1 Then
                freqmain = "\weekly.txt" : settfreq = My.Settings.devreptimew
            ElseIf ComboBox1.SelectedIndex = 2 Then
                freqmain = "\monthly.txt" : settfreq = My.Settings.devreptimem
            ElseIf ComboBox1.SelectedIndex = 3 Then
                freqmain = "\yearly.txt" : settfreq = My.Settings.devreptimey
            End If
            For Each cbdevice As String In devstr

                cbdevice = Trim(cbdevice)
                Dim cestazarkld As String = Application.StartupPath & "\devices\" & cbdevice & "\" & freqmain
                If My.Computer.FileSystem.FileExists(cestazarkld) = True Then
                    Dim fileContentslb As String : fileContentslb = (My.Computer.FileSystem.ReadAllText(cestazarkld))
                    Dim sNames() As String = Split(fileContentslb, vbCrLf)
                    For x = 0 To UBound(sNames)
                        If sNames(x) <> String.Empty Then

                            Dim subit() As String = Split(sNames(x), vbTab)
                            ListView1.Items.Add(subit(0)).UseItemStyleForSubItems = False

                        End If
                    Next
                End If
            Next

            Dim devrep() As String = Split(settfreq, vbCrLf)
            For Each saved As String In devrep
                Dim saveddev() As String = Split(saved, vbTab)
                Dim findboo As Boolean = False
                For Each it As ListViewItem In ListView1.Items
                    'MsgBox(it.SubItems(0).Text & vbCrLf & saveddev(0))
                    Try
                        If it.SubItems(0).Text = saveddev(0) Then


                            findboo = False
                            Try
                                'whours
                                it.SubItems.Add(saveddev(1)).ForeColor = NumericUpDown1.ForeColor
                            Catch ex As Exception

                            End Try
                            Try
                                'wprice
                                it.SubItems.Add(saveddev(2)).ForeColor = NumericUpDown2.ForeColor
                            Catch ex As Exception

                            End Try


                        End If
                    Catch ex As Exception

                    End Try
                Next
                If findboo = True Then ListView1.Items.Add(saveddev(0))

            Next
        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try

    End Sub
    
    Private Sub ListView1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged

        Try


            For Each item As ListViewItem In ListView1.Items

            Next
            For Each item As ListViewItem In ListView1.SelectedItems
                NumericUpDown1.Value = item.SubItems.Item(1).Text
                NumericUpDown2.Value = item.SubItems.Item(2).Text
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        For Each item As ListViewItem In ListView1.SelectedItems
            item.UseItemStyleForSubItems = True
            item.BackColor = Color.Blue
            If item.SubItems.Count < 2 Then
                item.SubItems.Add(NumericUpDown1.Value)
            Else

                item.SubItems.Item(1).Text = NumericUpDown1.Value
            End If

        Next
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        For Each item As ListViewItem In ListView1.SelectedItems
            item.UseItemStyleForSubItems = True
            item.BackColor = Color.Blue
            If item.SubItems.Count < 2 Then
                item.SubItems.Add("0")
                item.SubItems.Add(NumericUpDown2.Value)
            ElseIf item.SubItems.Count < 3 Then

                item.SubItems.Add(NumericUpDown2.Value)
            Else
                item.SubItems.Item(2).Text = NumericUpDown2.Value
            End If

        Next
    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        loadmain()
    End Sub

    Private Sub ComboBox1_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles ComboBox1.MouseClick
        save()
    End Sub
End Class
