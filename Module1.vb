'INSERT INTO aspnet_Membership (UserId, Password, PasswordSalt, PasswordAnswer) VALUES(@UserId, @Password, @PasswordSalt, @PasswordAnswer);
'UPDATE aspnet_Membership set Password='', PasswordSalt='', PasswordAnswer='' where UserId='65DAA6BC-381A-4E6C-A2B7-54500E1CCEC9'

'    Generate a random salt (16 bytes).
'    Combine the salt And password (Or answer) into a Single Byte array.
'    Compute the HMAC-SHA1 hash Of the combined Byte array.
'    Combine the salt And the HMAC-SHA1 hash into a Single Byte array.
'    Encode the final Byte array In Base64.

Imports System
Imports System.Security.Cryptography
Imports System.Text
Module Module1
    Sub Main()
        Dim password As String = "YourMomIsALovelyLady"
        Dim passwordAnswer As String = "YourMomIsALovelyLady"

        ' Generate a salt
        Dim salt As String = GenerateSalt()

        ' Hash the password with the salt
        Dim hashedPassword As String = HashPasswordOrAnswer(password, salt)

        ' Hash the password answer
        Dim hashedPasswordAnswer As String = HashPasswordOrAnswer(passwordAnswer, salt)

        Console.WriteLine("Password: " & hashedPassword)
        Console.WriteLine("PasswordSalt: " & salt)
        Console.WriteLine("PasswordAnswer: " & hashedPasswordAnswer)
        Console.ReadLine()
    End Sub

    ' Generate a random 16-byte salt
    Public Function GenerateSalt() As String
        Dim saltBytes(15) As Byte ' 16 bytes for the salt
        Using rng As New RNGCryptoServiceProvider()
            rng.GetBytes(saltBytes)
        End Using
        Return Convert.ToBase64String(saltBytes)
    End Function

    ' Hash the password or answer using the ASP.NET Membership algorithm
    Public Function HashPasswordOrAnswer(input As String, salt As String) As String
        ' Decode the salt from Base64
        Dim saltBytes As Byte() = Convert.FromBase64String(salt)

        ' Convert the input (password or answer) to bytes (Unicode encoding)
        Dim inputBytes As Byte() = Encoding.Unicode.GetBytes(input)

        ' Combine the salt and input bytes
        Dim combinedBytes(saltBytes.Length + inputBytes.Length - 1) As Byte
        Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length)
        Buffer.BlockCopy(inputBytes, 0, combinedBytes, saltBytes.Length, inputBytes.Length)

        ' Compute the SHA-1 hash of the combined bytes
        Using sha1 As SHA1 = SHA1.Create()
            Dim hashBytes As Byte() = sha1.ComputeHash(combinedBytes)

            ' Encode only the hash in Base64 (not the salt)
            Return Convert.ToBase64String(hashBytes)
        End Using
    End Function
End Module