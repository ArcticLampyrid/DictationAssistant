Imports System.Globalization

Public Interface ISpeakEngine
    Sub Speak(Text As String)
    Sub StopSpeak()
    ReadOnly Property Name As String
    Property Volume As Byte
    Property Rate As SByte
    ReadOnly Property Culture As CultureInfo
    Event EndSpeak()
End Interface