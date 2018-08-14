Imports System.Globalization

Public Class SpeakParam
    ''' <summary>
    ''' 取值范围：-10 ~ 20
    ''' </summary>
    Public Rate As SByte = 0
End Class
Public Interface ISpeakEngine
    Function Speak(Text As String, Param As SpeakParam) As PcmStreamWithInfo
    ReadOnly Property Name As String
    ReadOnly Property Culture As CultureInfo
End Interface
