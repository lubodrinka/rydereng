Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form9
    Dim datum, datum1, datum2, timestr As Date

    Public Sub selectdatum()
        If ListView3.Focused Or ListView4.Focused Or Label1.Text.Length > 0 Then

            Dim LV2dat1, LV2dat2 As Integer
            Try
                For o As Integer = 0 To ListView2.Items.Count - 1

                    '1 Date.Now                 1 date 2 time
                    '2 code                          1
                    '3:ComboBox4.Text user           2
                    '4 ComboBox1.Text                3
                    '5: ComboBox9.Text()             4   
                    datum = FormatDateTime(ListView2.Items(o).SubItems(0).Text, DateFormat.ShortDate)
                    datum1 = FormatDateTime(DateTimePicker1.Value, DateFormat.ShortDate)
                    datum2 = FormatDateTime(DateTimePicker2.Value, DateFormat.ShortDate)
                    timestr = FormatDateTime(ListView2.Items(o).SubItems(0).Text, DateFormat.ShortTime)
                    LV2dat1 = DateDiff(DateInterval.Day, datum1, datum)
                    LV2dat2 = DateDiff(DateInterval.Day, datum, datum2)

                    If LV2dat1 < 0 Or LV2dat2 < 0 Then
                        ListView2.Items.RemoveAt(o)
                    End If

                Next
            Catch ex As Exception

            End Try



        Else

            ListView2.Items.Clear()
            Dim dat1, dat2 As Integer
            For x As Integer = 0 To ListView1.Items.Count - 1
                Try
                    '1 Date.Now                 1 date 2 time
                    '2 code                          1
                    '3:ComboBox4.Text user           2
                    '4 ComboBox1.Text                3
                    '5: ComboBox9.Text()             4   
                    datum = FormatDateTime(ListView1.Items(x).SubItems(0).Text, DateFormat.ShortDate)
                    datum1 = FormatDateTime(DateTimePicker1.Value, DateFormat.ShortDate)
                    datum2 = FormatDateTime(DateTimePicker2.Value, DateFormat.ShortDate)
                    timestr = FormatDateTime(ListView1.Items(x).SubItems(0).Text, DateFormat.ShortTime)
                    dat1 = DateDiff(DateInterval.Day, datum1, datum)
                    dat2 = DateDiff(DateInterval.Day, datum, datum2)

                    If dat1 >= 0 AndAlso dat2 >= 0 Then
                        With ListView2
                            .Items.Add(datum)
                            .Items(ListView2.Items.Count - 1).SubItems.Add(timestr)
                            .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(1).Text)
                            .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(2).Text)
                            .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(3).Text)
                            .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(4).Text)

                        End With
                    End If


                Catch ex As Exception

                End Try

            Next
        End If
        ListView2.BringToFront()
        graf()
    End Sub


    Private Sub DateTimePicker1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        selectdatum()
    End Sub

    Private Sub DateTimePicker2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles DateTimePicker2.ValueChanged
        selectdatum()
    End Sub
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            DateTimePicker1.Value = Date.Parse(ListView1.Items(0).Text)
            DateTimePicker2.Value = Date.Parse(ListView1.Items(ListView1.Items.Count - 1).Text)
        Catch ex As Exception

        End Try
        'ListView1.BringToFront()

        'MsgBox(ListView1.Items(ListView1.Items.Count - 1).Text)

        Label1.ResetText()
    End Sub

    Public Sub graf()
        ListView3.Items.Clear()
        Dim graf1 As String = ColumnHeader1.Text
        Dim graf2 As String = ColumnHeader3.Text
        Dim x As String = 0
        Dim y As Integer = 0
        Try
            Chart2.Series.Clear()
            Chart2.Series.Add(graf2)
            Dim item1 As String
            Dim polozky As New List(Of String)
            For z As Integer = 0 To ListView2.Items.Count - 1
                item1 = ListView2.Items(z).SubItems(3).Text
                If polozky.Contains(item1) = False And item1 <> String.Empty Then
                    polozky.Add(item1)
                    ListView3.Items.Add(item1) : ListView3.Items(ListView3.Items.Count - 1).SubItems.Add("1")
                ElseIf polozky.Contains(item1) = True And item1 <> String.Empty Then
                    Dim pocet As Integer = 0
                    Dim i As Integer = ListView3.Items.IndexOf(ListView3.FindItemWithText(item1))

                    If ListView3.Items(i).SubItems.Count = 2 Then
                        'MsgBox("pocet subitems " & ListView3.Items.Count - 1 & "i " & i)
                        pocet = ListView3.Items(i).SubItems(1).Text
                        pocet += 1
                        ListView3.Items(i).SubItems(1).Text = pocet

                    Else
                        pocet += 1
                        ListView3.Items(i).SubItems.Add(pocet)
                    End If
                    'MsgBox(item1 & vbTab & pocet)
                End If

            Next
        Catch ex As Exception

        End Try
        Try
            'Uncomment this line if you want to show bar chart
            Chart2.Series(graf2).ChartType = SeriesChartType.Pie
            ' Set labels style
            Chart2.Series(graf2)("PieLabelStyle") = "inside"

            ' Show data points labels
            Chart2.Series(graf2).IsVisibleInLegend = True
            ' Set data points label style
            Chart2.Series(graf2)("BarLabelStyle") = "bottom"
            ' Show chart as 3D. Uncomment this line if you want to display your barchart as 3D
            Chart2.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            '' Draw chart as 3D Cylinder
            Chart2.Series(graf2)("DrawingStyle") = "Cylinder"
            For Each items As ListViewItem In ListView3.Items
                Chart2.Series(graf2).Points.AddXY(items.Text, items.SubItems(1).Text)
            Next

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

        Try
            Chart1.Series.Clear()

            Chart1.Series.Add(graf1)
            x = 0
            y = 0

            Dim olddate As Date = FormatDateTime(ListView2.Items(0).SubItems(0).Text, DateFormat.ShortDate)
            For z As Integer = 0 To ListView2.Items.Count - 1
                datum = FormatDateTime(ListView2.Items(z).SubItems(0).Text, DateFormat.ShortDate)
                If olddate <> datum Then
                    x = olddate
                    Chart1.Series(graf1).Points.AddXY(x, y)
                    y = 1
                Else
                    y += 1
                End If
                olddate = datum
            Next
            x = datum
            Chart1.Series(graf1).Points.AddXY(x, y)
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Form9_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'datum
        ColumnHeader1.Text = ryder.Label22.Text : C1.Text = ryder.Label22.Text
        'čas
        ColumnHeader2.Text = Form5.Label4.Text : C2.Text = Form5.Label4.Text
        'code
        'ColumnHeader3.Text = ryder.Label3.Text : C3.Text = 
        'užívateľ
        ColumnHeader4.Text = ryder.ColumnHeader13.Text : C4.Text = ryder.Label3.Text
        'zariadenia'
        ColumnHeader5.Text = ryder.Label14.Text : C5.Text = ryder.ColumnHeader13.Text
        'suč.zar()
        ColumnHeader6.Text = ryder.Label14.Text : C6.Text = ryder.Label14.Text

        'selectdatum()
        DateTimePicker1.Value = "1.1." & Date.Today.Year
        graf()
        'ListView1.Visible = True
        'ListView1.BringToFront()
    End Sub



    Private Sub ListView4_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView4.SelectedIndexChanged
        Try


            ListView2.Items.Clear()
            Dim selectedzariadenie As String = ListView4.FocusedItem.Text
            Label1.Text = selectedzariadenie
            Dim code1, code4 As Integer
            code4 = ListView4.FocusedItem.SubItems(2).Text
            For x As Integer = 0 To ListView1.Items.Count - 1
                'Try
                '1 Date.Now                 1 date 2 time
                '2 code                          1
                '3:ComboBox4.Text user           2
                '4 ComboBox1.Text                3
                '5: ComboBox9.Text()             4   
                datum = FormatDateTime(ListView1.Items(x).SubItems(0).Text, DateFormat.ShortDate)
                timestr = FormatDateTime(ListView1.Items(x).SubItems(0).Text, DateFormat.ShortTime)
                code1 = ListView1.Items(x).SubItems(1).Text
                'MsgBox("lv1 " & code1 & vbTab & ListView1.Items(x).SubItems(3).Text & vbCrLf & "lv4  " & code4 & vbTab & selectedzariadenie)

                If selectedzariadenie.Contains(ListView1.Items(x).SubItems(3).Text) AndAlso code1 = code4 Then
                    With ListView2
                        .Items.Add(datum)
                        .Items(ListView2.Items.Count - 1).SubItems.Add(timestr)
                        .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(1).Text)
                        .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(2).Text)
                        .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(3).Text)
                        Try
                            .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(4).Text)
                        Catch ex As Exception

                        End Try
                    End With
                End If
            Next
        Catch ex As Exception

        End Try
        graf()
    End Sub

    Private Sub ListView3_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView3.SelectedIndexChanged
        Try


            If ListView3.Focused Then
                ListView2.Items.Clear()
                Dim selecteduser As String = ListView3.FocusedItem.Text
                Label1.Text = selecteduser

                For x As Integer = 0 To ListView1.Items.Count - 1
                    'Try
                    '1 Date.Now                 1 date 2 time
                    '2 code                          1
                    '3:ComboBox4.Text user           2
                    '4 ComboBox1.Text                3
                    '5: ComboBox9.Text()             4   
                    datum = FormatDateTime(ListView1.Items(x).SubItems(0).Text, DateFormat.ShortDate)
                    timestr = FormatDateTime(ListView1.Items(x).SubItems(0).Text, DateFormat.ShortTime)

                    'MsgBox("lv1 " & vbTab & ListView1.Items(x).SubItems(2).Text & vbCrLf & "lv3 " & vbTab & selecteduser)

                    If selecteduser.Equals(ListView1.Items(x).SubItems(2).Text) Then
                        With ListView2
                            .Items.Add(datum)
                            .Items(ListView2.Items.Count - 1).SubItems.Add(timestr)
                            .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(1).Text)
                            .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(2).Text)
                            .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(3).Text)
                            Try
                                .Items(ListView2.Items.Count - 1).SubItems.Add(ListView1.Items(x).SubItems(4).Text)
                            Catch ex As Exception

                            End Try
                        End With
                    End If
                Next
                graf()
            End If
        Catch ex As Exception

        End Try
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

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
           Implements IComparer.Compare
            Return [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
        End Function
    End Class

    Private Sub ListView2_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView2.ColumnClick
        Me.ListView1.ListViewItemSorter = New ListViewItemComparer(e.Column)
        Me.ListView2.ListViewItemSorter = New ListViewItemComparer(e.Column)
    End Sub
End Class
