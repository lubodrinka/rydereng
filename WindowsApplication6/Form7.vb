Imports System.Text
Imports System.Net
Imports System.Web

Public Class Form7



    Private Sub Form7_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim adres As New StringBuilder
        adres.Append("https://google.com/search?q= ")
        Dim line As String = ryder.CheckedListBox2.SelectedItem
        If line <> String.Empty Then
            line.ToString.Replace(" ", "+" & ryder.TextBox19.Text)
            adres.Append(line.ToString)
        End If
        WebBrowser1.Navigate(adres.ToString)
        WebBrowser1.BringToFront()
        WebBrowser1.Show()
    End Sub
    'Private Sub WebBrowser1_Documentdownload(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated

    '    Dim adres As String = String.Empty
    '    Dim olink As HtmlElement
    '    Dim olinks As HtmlElementCollection = WebBrowser1.Document.All

    '    For Each olink In olinks

    '        adres = olink.InnerText
    '        If adres <> Nothing Then
    '            MsgBox(adres)
    '            If adres.Contains("<a href=") Then MsgBox(adres)

    '        End If



    '        'Dim strHTML = GetPageHTML(adres)

    '    Next
    '    '    Dim arrLinks As ArrayList = ParseLinks("<a href=""http://www.marksandler.com/"">" & "Visit MarkSandler.com</a>")
    '    ' Loop through results
    '    Dim shtCount As Integer
    '    For shtCount = 0 To arrLinks.Count - 1
    '        MessageBox.Show(arrLinks(shtCount).ToString)
    '    Next

    'End Sub

    'Public Function GetPageHTML(ByVal URL As String) As String
    '    ' Retrieves the HTML from the specified URL
    '    Dim objWC As New System.Net.WebClient()
    '    Return New System.Text.UTF8Encoding().GetString( _
    '       objWC.DownloadData(URL))
    'End Function

    'Public Function ParseLinks(ByVal HTML As String) As ArrayList
    '    ' Remember to add the following at top of class:
    '    ' - Imports System.Text.RegularExpressions
    '    Dim objRegEx As System.Text.RegularExpressions.Regex
    '    Dim objMatch As System.Text.RegularExpressions.Match
    '    Dim arrLinks As New System.Collections.ArrayList()
    '    ' Create regular expression
    '    objRegEx = New System.Text.RegularExpressions.Regex( _
    '        "a.*href\s*=\s*(?:""(?<1>[^""]*)""|(?<1>\S+))", _
    '        System.Text.RegularExpressions.RegexOptions.IgnoreCase Or _
    '        System.Text.RegularExpressions.RegexOptions.Compiled)
    '    ' Match expression to HTML
    '    objMatch = objRegEx.Match(HTML)
    '    ' Loop through matches and add <1> to ArrayList
    '    While objMatch.Success
    '        Dim strMatch As String
    '        strMatch = objMatch.Groups(1).ToString
    '        arrLinks.Add(strMatch)
    '        objMatch = objMatch.NextMatch()
    '    End While
    '    ' Pass back results
    '    Return arrLinks
    'End Function



End Class