Module Module1
    Public CiZuZengQiangMuLu As String

    Public Function IsVoiceLanguageIDEquals(Voice As SpeechLib.SpObjectToken, ID As Int32) As Boolean
        Try
            Dim Languages() As String
            Languages = Split(Voice.GetAttribute("Language"), ";")
            For Each Language As String In Languages
                If Convert.ToInt32(Language, 16) = ID Then
                    Return True
                End If
            Next
        Catch ex As Exception

        End Try
        Return False
    End Function
    Public Function GetChineseGrade(VoiceName As String) As Integer
        Select Case VoiceName
            Case "VW Lily"
                GetChineseGrade = 1
            Case "VW Hui"
                GetChineseGrade = 2
            Case "VW Liang"
                GetChineseGrade = 3
            Case "VW Wang"
                GetChineseGrade = 4
            Case "Girl XiaoKun"
                GetChineseGrade = 5
            Case Else
                GetChineseGrade = 0
        End Select
    End Function
    Public Function GetEnglishGrade(VoiceName As String) As Integer
        Select Case VoiceName
            Case "VW Julie"
                GetEnglishGrade = 1
            Case "VW Paul"
                GetEnglishGrade = 2
            Case "VW Kate"
                GetEnglishGrade = 3
            Case Else
                GetEnglishGrade = 0
        End Select
    End Function
End Module
