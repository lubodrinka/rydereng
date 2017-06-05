Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        ListBox0.Items.Clear()
        Dim fileContentsd As String = My.Settings.priorita
        Dim fileContentscb1 As String = My.Settings.device
        Dim snameccb1() As String = Split(fileContentscb1, vbCrLf)
        For x = 0 To UBound(snameccb1)

            If Not fileContentsd.Contains(snameccb1(x)) Then
                ListBox0.Items.Add(snameccb1(x))
            End If
        Next
        Try


            Dim sNames() As String = Split(fileContentsd, "####")
            Dim main() As String = Split(sNames(0), vbCrLf)
            For x = 0 To UBound(main)
                If main(x) = Nothing Then GoTo dalsi
                ListBox0.Items.Add(main(x))
dalsi:
            Next

            Dim prvy() As String = Split(sNames(1), vbCrLf)
            For x = 0 To UBound(prvy)
                If prvy(x) = Nothing Then GoTo dalsi1
                ListBox1.Items.Add(prvy(x))
dalsi1:
            Next
            Dim druhy() As String = Split(sNames(2), vbCrLf)
            For x = 0 To UBound(druhy)
                If druhy(x) = vbCrLf Or Nothing Then GoTo dalsi2
                ListBox2.Items.Add(druhy(x))
dalsi2:
            Next

            Dim treti() As String = Split(sNames(3), vbCrLf)
            For x = 0 To UBound(treti)
                If treti(x) = Nothing Then GoTo dalsi3
                ListBox3.Items.Add(treti(x))
dalsi3:
            Next
            Dim štvrty() As String = Split(sNames(4), vbCrLf)
            For x = 0 To UBound(štvrty)
                If štvrty(x) = Nothing Then GoTo dalsi4
                ListBox4.Items.Add(štvrty(x))
dalsi4:
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Sub save()
        Try


            Dim Data0 As String = Nothing
            Dim Data1 As String = Nothing
            Dim Data2 As String = Nothing
            Dim Data3 As String = Nothing
            Dim Data4 As String = Nothing
            If ListBox0.Items.Count = 0 Then GoTo dalsi1
            Dim ItemArray(Me.ListBox0.Items.Count - 1) As Object
            Me.ListBox0.Items.CopyTo(ItemArray, 0)
            Data0 = Join(ItemArray, vbCrLf)
dalsi1:
            If ListBox1.Items.Count < 1 Then GoTo dalsi2
            Dim ItemArray1(Me.ListBox1.Items.Count - 1) As Object
            Me.ListBox1.Items.CopyTo(ItemArray1, 0)
            Data1 = Join(ItemArray1, vbCrLf)
dalsi2:
            If ListBox2.Items.Count < 1 Then GoTo dalsi3
            Dim ItemArray2(Me.ListBox2.Items.Count - 1) As Object
            Me.ListBox2.Items.CopyTo(ItemArray2, 0)
            Data2 = Join(ItemArray2, vbCrLf)
dalsi3:
            If ListBox3.Items.Count < 1 Then GoTo dalsi4
            Dim ItemArray3(Me.ListBox3.Items.Count - 1) As Object
            Me.ListBox3.Items.CopyTo(ItemArray3, 0)
            Data3 = Join(ItemArray3, vbCrLf)
dalsi4:

            If ListBox4.Items.Count < 1 Then GoTo dalsi
            Dim ItemArray4(Me.ListBox4.Items.Count - 1) As Object
            Me.ListBox4.Items.CopyTo(ItemArray4, 0)
            Data4 = Join(ItemArray4, vbCrLf)

dalsi:
            My.Settings.priorita = Data0 + "####" + vbCrLf + Data1 + "####" + vbCrLf + Data2 + "####" + vbCrLf + Data3 + "####" + vbCrLf + Data4
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button0_Click(sender As System.Object, e As System.EventArgs) Handles Button0.Click
        save()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            ListBox1.Items.Add(ListBox0.SelectedItem)
            ListBox0.Items.Remove(ListBox0.SelectedItem)
        Catch ex As Exception

        End Try


    End Sub
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Try
            ListBox2.Items.Add(ListBox0.SelectedItem)
            ListBox0.Items.Remove(ListBox0.SelectedItem)
        Catch ex As Exception
        End Try

    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles button3.Click
        Try
            ListBox3.Items.Add(ListBox0.SelectedItem)
            ListBox0.Items.Remove(ListBox0.SelectedItem)
        Catch ex As Exception
        End Try

    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Try
            ListBox4.Items.Add(ListBox0.SelectedItem)
            ListBox0.Items.Remove(ListBox0.SelectedItem)
        Catch ex As Exception
        End Try

    End Sub


    
   
    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        Try
            ListBox0.Items.Add(ListBox1.SelectedItem)
            ListBox1.Items.Remove(ListBox1.SelectedItem)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        Try
            ListBox0.Items.Add(ListBox2.SelectedItem)
            ListBox2.Items.Remove(ListBox2.SelectedItem)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        Try
            ListBox0.Items.Add(ListBox3.SelectedItem)
            ListBox3.Items.Remove(ListBox3.SelectedItem)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
        Try
            ListBox0.Items.Add(ListBox4.SelectedItem)
            ListBox4.Items.Remove(ListBox4.SelectedItem)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click
        For Each li As Object In Me.FlowLayoutPanel1.Controls
            li.Items.Remove(li.SelectedItem)
        Next
        ListBox0.Items.Remove(ListBox0.SelectedItem)
    End Sub
End Class