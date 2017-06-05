
Public Class Form6
   
    Public Sub xlstext2()
        ListView3.Items.Clear()
        Me.Size = New Size(My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height - 10)
        ListView3.Size = New Size(Me.Width - 20, Me.Height - 100)
        Me.SetDesktopBounds(0, 0, My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height)

        Try
            APP = CreateObject("Excel.Application")
            Dim book1path As String = Application.StartupPath & "\book1." & excelextension
            workbook = APP.Workbooks.Open(book1path)
            worksheet = workbook.Sheets.Item(1)

            'workbook = APP.Workbooks.Open(Application.StartupPath & "\book1.xls")
            'worksheet = workbook.Sheets.Item(1)
            Dim LastRow As Long
            Dim LRi As Integer
            With worksheet
                LastRow = .Cells(.Rows.Count, 2).End(XlDirectionxlUP).Row
            End With
            LRi = LastRow
            For x = 1 To LRi
                Dim ic As Integer = ListView3.Items.Count
                'week
                Dim w As String = worksheet.Cells(x + 1, 1).Value
                ListView3.Items.Add(w)
                'Date.Now.Date
                Dim dat As String = worksheet.Cells(x + 1, 2).Value
                ListView3.Items(ic).SubItems.Add(dat)
                'ComboBox1.Text
                Dim zar1 As String = worksheet.Cells(x + 1, 3).Value
                ListView3.Items(ic).SubItems.Add(zar1)
                'checkedlistbox2.SelectedItem
                Dim udr As String = worksheet.Cells(x + 1, 4).Value
                ListView3.Items(ic).SubItems.Add(udr)
                'ListBox3.SelectedItem ND
                Dim ND As String = worksheet.Cells(x + 1, 5).Value
                ListView3.Items(ic).SubItems.Add(ND)
                'TextBox1.Text  poznámka
                Dim poz As String = worksheet.Cells(x + 1, 6).Value
                ListView3.Items(ic).SubItems.Add(poz)
                'ComboBox4.Text user
                Dim user As String = worksheet.Cells(x + 1, 7).Value
                ListView3.Items(ic).SubItems.Add(user)
                'kl()interval údržby
                Dim int As String = worksheet.Cells(x + 1, 8).Value
                ListView3.Items(ic).SubItems.Add(int)
            Next

            APP.DisplayAlerts = False
            workbook.Close()
            APP.ActiveWorkbook.Close()
            'APP.ActiveWorkbook.Close(SaveChanges:=True)
            APP.Quit()
            releaseobject(APP)
            releaseobject(workbook)
            releaseobject(worksheet)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Form6_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        xlstext2()
    End Sub

    Private Sub ListView3_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView3.ColumnClick
        Me.ListView3.ListViewItemSorter = New ListViewItemComparer(e.Column)
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
End Class