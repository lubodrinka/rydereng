Imports System.Security.Cryptography
Imports System.IO
Imports System.Management
Imports System.Net.Mail
Imports System.Text

Public Class Form10


    Dim driveSerial As String = GetDriveSerialNumber("C:")
    Private Sub Form10_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        BringToFront()
        If My.Settings.company <> Nothing Then TextBox1.Text = My.Settings.company

        If My.Settings.registrationdate <> Nothing Then
            TextBox5.Text = My.Settings.registrationdate
        Else
            TextBox5.Text = Date.Now.Date
        End If
        If My.Settings.heslo = Nothing Then Button1.Enabled = False
    End Sub
    Public Sub vytvoreniekluca()
        Label3.Text = driveSerial
        Dim plaintext As String = CStr(TextBox5.Text) & vbTab & My.Application.Info.ProductName & vbTab & Trim(TextBox1.Text)

        Dim password As String = driveSerial
        Dim wrapper As New Simple3Des(password)
        Dim cipherText As String = wrapper.EncryptData(wrapper.EncryptData(plaintext.ToLower))
        
        My.Settings.heslo = cipherText

        MsgBox(My.Settings.heslo)
    End Sub
  
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            If TextBox4.Text <> Nothing Then
                Dim wrapper As New Simple3Des(driveSerial)



                Dim hh As String = wrapper.DecryptData(TextBox4.Text)
                Dim gen As String = wrapper.DecryptData(wrapper.DecryptData(My.Settings.heslo))


                If gen = hh Then
                    My.Settings.hesloback = hh
                    MsgBox("serial key is correct", MsgBoxStyle.MsgBoxSetForeground)
                    My.Settings.Save()
                    ryder.ryderstart()
                    Me.Close()
                Else
                    MsgBox("serial key is incorrect", MsgBoxStyle.Critical)
                End If
            End If
        Catch ex As Exception
            MsgBox("serial key is incorrect", MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Function GetDriveSerialNumber(ByVal drive As String) As String

        Dim driveSerial As String = String.Empty
        Dim driveFixed As String = System.IO.Path.GetPathRoot(drive)
        driveFixed = Replace(driveFixed, "\", String.Empty)

        Using querySearch As New ManagementObjectSearcher("SELECT VolumeSerialNumber FROM Win32_LogicalDisk Where Name = '" & driveFixed & "'")

            Using queryCollection As ManagementObjectCollection = querySearch.Get()

                Dim moItem As ManagementObject

                For Each moItem In queryCollection

                    driveSerial = CStr(moItem.Item("VolumeSerialNumber"))

                    Exit For
                Next
            End Using
        End Using
        Return driveSerial
    End Function
    Public NotInheritable Class Simple3Des
        Private TripleDes As New TripleDESCryptoServiceProvider
        Private Function TruncateHash(
            ByVal key As String,
            ByVal length As Integer) As Byte()

            Dim sha1 As New SHA1CryptoServiceProvider

            ' Hash the key.
            Dim keyBytes() As Byte =
                System.Text.Encoding.Unicode.GetBytes(key)
            Dim hash() As Byte = sha1.ComputeHash(keyBytes)

            ' Truncate or pad the hash.
            ReDim Preserve hash(length - 1)
            Return hash
        End Function
        Sub New(ByVal key As String)
            ' Initialize the crypto provider.
            TripleDes.Key = TruncateHash(key, TripleDes.KeySize \ 8)
            TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)
        End Sub

        Public Function EncryptData(
        ByVal plaintext As String) As String

            ' Convert the plaintext string to a byte array.
            Dim plaintextBytes() As Byte =
                System.Text.Encoding.Unicode.GetBytes(plaintext)

            ' Create the stream.
            Dim ms As New System.IO.MemoryStream
            ' Create the encoder to write to the stream.
            Dim encStream As New CryptoStream(ms,
                TripleDes.CreateEncryptor(),
                System.Security.Cryptography.CryptoStreamMode.Write)

            ' Use the crypto stream to write the byte array to the stream.
            encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
            encStream.FlushFinalBlock()

            ' Convert the encrypted stream to a printable string.
            Return Convert.ToBase64String(ms.ToArray)
        End Function
        Public Function DecryptData(
        ByVal encryptedtext As String) As String

            ' Convert the encrypted text string to a byte array.
            Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

            ' Create the stream.
            Dim ms As New System.IO.MemoryStream
            ' Create the decoder to write to the stream.
            Dim decStream As New CryptoStream(ms,
                TripleDes.CreateDecryptor(),
                System.Security.Cryptography.CryptoStreamMode.Write)

            ' Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
            decStream.FlushFinalBlock()

            ' Convert the plaintext stream to a string.
            Return System.Text.Encoding.Unicode.GetString(ms.ToArray)
        End Function
    End Class
    '    Shared Function zakodovatdoAes(ByVal plainText As String, ByVal Key() As Byte, ByVal IV() As Byte) As Byte()
    '        ' Check arguments.
    '        If plainText Is Nothing OrElse plainText.Length <= 0 Then
    '            Throw New ArgumentNullException("plainText")
    '        End If
    '        If Key Is Nothing OrElse Key.Length <= 0 Then
    '            Throw New ArgumentNullException("Key")
    '        End If
    '        If IV Is Nothing OrElse IV.Length <= 0 Then
    '            Throw New ArgumentNullException("Key")
    '        End If
    '        Dim encrypted() As Byte
    '        ' Create an Aes object
    '        ' with the specified key and IV.
    '        Using aesAlg As Aes = Aes.Create()

    '            aesAlg.Key = Key
    '            aesAlg.IV = IV

    '            ' Create a decrytor to perform the stream transform.
    '            Dim encryptor As ICryptoTransform = aesAlg.CreateEncryptor(aesAlg.
    'Key, aesAlg.IV)
    '            ' Create the streams used for encryption.
    '            Using msEncrypt As New MemoryStream()
    '                Using csEncrypt As New CryptoStream(msEncrypt, encryptor,
    'CryptoStreamMode.Write)
    '                    Using swEncrypt As New StreamWriter(csEncrypt)

    '                        'Write all data to the stream.
    '                        swEncrypt.Write(plainText)
    '                    End Using
    '                    encrypted = msEncrypt.ToArray()
    '                End Using
    '            End Using
    '        End Using

    '        ' Return the encrypted bytes from the memory stream.
    '        Return encrypted

    '    End Function 'EncryptStringToBytes_Aes

    '    Shared Function dekodovaťzAes(ByVal cipherText() As Byte, ByVal Key() As Byte, ByVal IV() As Byte) As String
    '        ' Check arguments.
    '        If cipherText Is Nothing OrElse cipherText.Length <= 0 Then
    '            Throw New ArgumentNullException("cipherText")
    '        End If
    '        If Key Is Nothing OrElse Key.Length <= 0 Then
    '            Throw New ArgumentNullException("Key")
    '        End If
    '        If IV Is Nothing OrElse IV.Length <= 0 Then
    '            Throw New ArgumentNullException("Key")
    '        End If
    '        ' Declare the string used to hold
    '        ' the decrypted text.
    '        Dim plaintext As String = Nothing

    '        ' Create an Aes object
    '        ' with the specified key and IV.
    '        Using aesAlg As Aes = Aes.Create()
    '            aesAlg.Key = Key
    '            aesAlg.IV = IV

    '            ' Create a decrytor to perform the stream transform.
    '            Dim decryptor As ICryptoTransform = aesAlg.CreateDecryptor(aesAlg.
    '        Key, aesAlg.IV)

    '            ' Create the streams used for decryption.
    '            Using msDecrypt As New MemoryStream(cipherText)

    '                Using csDecrypt As New CryptoStream(msDecrypt, decryptor,
    'CryptoStreamMode.Read)

    '                    Using srDecrypt As New StreamReader(csDecrypt)


    '                        ' Read the decrypted bytes from the decrypting stream
    '                        ' and place them in a string.
    '                        plaintext = srDecrypt.ReadToEnd()
    '                    End Using
    '                End Using
    '            End Using
    '        End Using

    '        Return plaintext

    '    End Function 'DecryptStringFromBytes_Aes 






    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If TextBox1.Text <> String.Empty Then
            TextBox3.Text = TextBox5.Text & vbCrLf & _
                TextBox1.Text & vbCrLf & _
            driveSerial
            vytvoreniekluca()
            Button3.Visible = True
        Else
            MsgBox(Label4.Text)
        End If
        My.Settings.company = TextBox1.Text
        My.Settings.registrationdate = TextBox5.Text
        My.Settings.Save()
        Button1.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If TextBox2.Text <> String.Empty Then

            Dim server As String = My.Settings.mserver
            Dim message As New MailMessage
            If server <> Nothing Then
            ElseIf server = String.Empty Then
                server = InputBox("mailserver")
                My.Settings.mserver = server
            End If

            Dim emailClient As New SmtpClient(server)


            Try
                emailClient.Send(TextBox2.Text, "lubodrinka@gmail.com", "ryder registration", (My.Application.Info.ProductName & My.Application.Info.Version.ToString & vbCrLf & TextBox5.Text & vbCrLf & TextBox1.Text & vbCrLf & driveSerial) & My.Settings.registrationdate)
                'My.Settings.registrationdate = TextBox5.Text
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        ElseIf TextBox2.Text = String.Empty Then
            MsgBox(Label6.Text & "?")
        End If

    End Sub

    Private Sub TextBox5_Leave(sender As System.Object, e As System.EventArgs) Handles TextBox5.Leave
        TextBox5.Text = CDate(TextBox5.Text)

    End Sub

    Private Sub Form10_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        My.Settings.Save()
    End Sub


End Class