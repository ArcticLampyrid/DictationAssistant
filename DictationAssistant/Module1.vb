Module Module1
    Public DCZQML As String
    Public Enum VoiceLanguage
        英文 = 1
        中文 = 2
        其他 = 0
    End Enum
    Public Function GetVoiceLanguage(Voice As SpeechLib.SpObjectToken) As VoiceLanguage
        Dim Language As String
        Dim Chinese As Boolean
        Dim English As Boolean
        Language = Voice.GetAttribute("Language")
        Chinese = Language = "804" Or Language Like "*;804" Or Language Like "804;*" Or Language Like "*;804;*"
        English = Language = "409" Or Language Like "*;409" Or Language Like "409;*" Or Language Like "*;409;*"
        If Chinese Then '当发音人物语言为中文时
            Return VoiceLanguage.中文
        ElseIf English Then '当发音人物语言为英文时
            Return VoiceLanguage.英文
        Else
            Return VoiceLanguage.其他
        End If
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
