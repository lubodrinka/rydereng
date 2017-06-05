

Public Class Form8

    Private Sub Form8_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        NumericUpDown1.Value = Date.Now.Year
        NumericUpDown2.Value = ryder.NumericUpDown1.Value
        grafmth()
        ColumnHeader1.Text = ryder.Label21.Text
        ColumnHeader2.Text = ryder.Label16.Text & "/" & ryder.Label21.Text
        ColumnHeader3.Text = ryder.Label19.Text & " " & ryder.Label16.Text
        ColumnHeader4.Text = ryder.Label22.Text
        ColumnHeader5.Text = ryder.Label16.Text
    End Sub
    Public Sub grafmth()
        Dim graf1 As String = ColumnHeader2.Text
        Dim graf2 As String = ColumnHeader3.Text
        Dim graf3 As String = ColumnHeader5.Text
        Chart1.Series.Clear()
        Chart2.Series.Clear()
        Chart3.Series.Clear()
        Chart1.Series.Add(graf1)
        Chart2.Series.Add(graf2)
        Chart3.Series.Add(graf3)
        Dim x As String = 0
        Dim y As String = 0
        Dim y2 As String = 0
        Dim y3 As String = 0
        For Each item As ListViewItem In ListView1.Items
            x = item.SubItems.Item(0).Text
            y = item.SubItems.Item(1).Text
            y2 = item.SubItems.Item(2).Text
            y3 = item.SubItems.Item(4).Text

            Chart1.Series(graf1).Points.AddXY(x, y)
            Chart2.Series(graf2).Points.AddXY(x, y2)
            Chart3.Series(graf3).Points.AddXY(x, y3)
        Next

        Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
        Chart2.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
        Chart3.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
    End Sub


   

    Private Sub ListView1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        For Each item As ListViewItem In ListView1.Items
            item.BackColor = Color.White
        Next
        For Each item As ListViewItem In ListView1.SelectedItems
            item.BackColor = Color.Aquamarine
            NumericUpDown1.Value = item.SubItems.Item(0).Text
            NumericUpDown2.Value = item.SubItems.Item(4).Text
        Next
    End Sub

    

    Private Sub ToolStripButton1_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton1.Click
        'Add new value for new  line
        Dim year As Integer = NumericUpDown1.Value
        Dim iyear As Integer = Nothing
        Dim i As Integer = 0
        Dim oldrozdiel As Integer = 0
        For Each item As ListViewItem In ListView1.Items
            iyear = item.Text
            Dim rozdiel As Integer = year - iyear
            If rozdiel < 0 And rozdiel > oldrozdiel Then
                i = item.Index
            ElseIf rozdiel > 0 And rozdiel < oldrozdiel Then
                i = item.Index + 1
            End If
            oldrozdiel = rozdiel
        Next
        Dim nm2 As Integer = NumericUpDown2.Value
        ListView1.Items.Insert(i, year)
        Dim motohodinyročne As Integer = 0
        motohodinyročne = ListView1.Items(i - 1).SubItems(1).Text
        ListView1.Items(i).SubItems.Add(nm2 - motohodinyročne)
        ListView1.Items(i).SubItems.Add(Format((nm2 / 365), "."))
        ListView1.Items(i).SubItems.Add("1. 1. " & NumericUpDown1.Value)
        ListView1.Items(i).SubItems.Add(nm2)
        grafmth()
    End Sub

    Private Sub ToolStripButton3_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton3.Click
        'save
        My.Computer.FileSystem.WriteAllText(Label1.Text, String.Empty, False)
        For Each item As ListViewItem In ListView1.Items
            If item.SubItems.Item(4).Text.Contains(ryder.NumericUpDown1.Value) = False Then
                Dim historystr As String = item.SubItems.Item(3).Text & vbTab & item.SubItems.Item(4).Text & vbCrLf
                My.Computer.FileSystem.WriteAllText(Label1.Text, historystr, True)
            End If
        Next
    End Sub

    Private Sub ToolStripButton2_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton2.Click
        For Each item As ListViewItem In ListView1.SelectedItems
            item.Remove()
        Next



    End Sub

    Private Sub ToolStripButton4_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton4.Click
        'uprava
        If ListView1.SelectedItems.Count > 0 Then
            Dim motohodinyročne As Integer = 0


            For Each item As ListViewItem In ListView1.SelectedItems
                Dim datsplt() As String = Split(item.SubItems(3).Text, ".")
                item.SubItems.Item(0).Text = NumericUpDown1.Value
                Try
                    motohodinyročne = ListView1.Items(item.Index - 1).SubItems(1).Text
                Catch ex As Exception

                End Try


                item.SubItems.Item(1).Text = motohodinyročne
                item.SubItems.Item(3).Text = datsplt(0) & "." & datsplt(1) & ". " & NumericUpDown1.Value
                item.SubItems.Item(4).Text = NumericUpDown2.Value
            Next
        End If
    End Sub
End Class