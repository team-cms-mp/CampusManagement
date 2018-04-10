Public Class Encryption

    '================= Main Variables ============
    Dim First_Prime_No As Integer = 11
    Dim Second_Prime_No As Integer = 31
    Dim Public_Key As Integer = 7
    Dim Private_Key As Integer
    Dim Prime_no_product As Integer
    Dim Main_Divident As Integer

    Sub Main()
        Dim First_Prime_No As Integer = 11
        Dim Second_Prime_No As Integer = 31
        Dim Public_Key As Integer = 7
    End Sub

    Function Encrypt_Main(ByVal str As String, ByVal Type As Boolean) As String
        First_Prime_No = 11
        Second_Prime_No = 31
        Public_Key = 7
        Dim Prime_no_product As Integer = First_Prime_No * Second_Prime_No
        Dim Main_Dividend As Integer = (First_Prime_No - 1) * (Second_Prime_No - 1)
        Dim Private_Key As Integer = f(Public_Key, Main_Dividend)
        Dim Return_String As String = ""
        If Type = True Then
            If IsNumeric(str.Replace("-", "")) = True And str.Length = 21 Then
                str = str.Replace("-", "")
            End If
            Return_String = new_Encry(str, Main_Dividend, Public_Key)
        ElseIf Type = False Then
            Return_String = D_String(str, Private_Key, Main_Dividend)
            If IsNumeric(Return_String) = True Then
                If Return_String.Length = 15 Then
                    Dim strLocation As String = Return_String.Substring(0, 2) + "-"
                    Dim strMonth As String = Return_String.Substring(2, 2) + "-"
                    Dim strYear As String = Return_String.Substring(4, 2) + "-"
                    Dim strGender As String = Return_String.Substring(6, 1) + "-"
                    Dim strProvince As String = Return_String.Substring(7, 2) + "-"
                    Dim strReceiption As String = Return_String.Substring(9, 2) + "-"
                    Dim strSerial As String = Right(Return_String, 4)
                    Return_String = strLocation + strMonth + strYear + strGender + strProvince + strReceiption + strSerial

                End If


            End If

        End If
        Return Return_String
    End Function

    '================================  Public Key Private Key Code ==========================================
    Function new_Encry(ByVal str As String, ByVal Main_Dividend As Integer, ByVal public_key As Integer) As String
        '============= split string char to thier ascii values and maintain the length 3 each ======
        Dim main_str As String = str
        Dim Ascii As String = ""
        For i As Integer = 0 To main_str.Length - 1
            Dim Asc_Value As Integer = Asc(main_str(i))
            If Asc_Value < 10 Then
                Ascii = Ascii & "00" & Asc_Value
            ElseIf Asc_Value > 9 And Asc_Value < 100 Then
                Ascii = Ascii & "0" & Asc_Value
            ElseIf Asc_Value > 99 And Asc_Value < 1000 Then
                Ascii = Ascii & Asc_Value
            End If
        Next
        '===========================================================================================
        Dim m As New ArrayList
        For i As Integer = 0 To Ascii.Length / 3
            m.Add(Mid(Ascii, (i * 3) + 1, 3))
        Next
        Return Encrypt(public_key, Main_Dividend, m)
    End Function
    '======================== Claculate The Private Key From The Public Key ===========================
    Function f(ByVal e As Integer, ByVal t As Integer) As Integer
        Dim i As Integer
        While (((i * e) Mod t) <> 1)
            i += 1
        End While
        Return i
    End Function
    '========================== Encrypt The String ===================================
    Function Encrypt(ByVal e As Integer, ByVal n As Integer, ByVal m As ArrayList) As String
        Dim a As String = ""
        Dim b As String
        For i As Integer = 0 To m.Count - 2
            b = (m.Item(i) * e) Mod n
            If CInt(b) < 10 Then
                b = "00" & b
            ElseIf CInt(b) > 9 And CInt(b) < 100 Then
                b = "0" & b
            ElseIf CInt(b) > 99 And CInt(b) < 1000 Then
                b = b
            End If
            a = a & b
        Next
        Return a
    End Function
    '========================== Decrypt The String ===================================
    Function D_String(ByVal str As String, ByVal Private_Key As Integer, ByVal Main_Dividend As Integer) As String
        Dim main_str As String = str
        Dim decrtpy As New ArrayList
        For i As Integer = 0 To (str.Length / 3) - 1
            decrtpy.Add(Mid(str, (i * 3) + 1, 3))
        Next
        Return Decrypt(Private_Key, Main_Dividend, decrtpy)
    End Function
    Function Decrypt(ByVal Private_Key As Integer, ByVal Main_Dividend As Integer, ByVal Str_Decrypt As ArrayList) As String
        Dim a As String = ""
        Dim b As Integer
        For j As Integer = 0 To Str_Decrypt.Count - 1
            Try
                b = (Str_Decrypt(j) * Private_Key) Mod Main_Dividend
                a = a & Convert.ToChar(b)
            Catch ex As Exception

            End Try
        Next
        Return a
    End Function
End Class
