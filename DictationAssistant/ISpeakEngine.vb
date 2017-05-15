Imports System.Globalization

Public Interface ISpeakEngine
    Function Speak(Text As String) As ISpeakStateControler
    ReadOnly Property Name As String
    Property Volume As Byte
    Property Rate As SByte
    ReadOnly Property Culture As CultureInfo
End Interface
Public Interface ISpeakStateControler
    Sub StopSpeak()
    ''' <summary>
    ''' 注意，该事件可能并不在UI线程触发。
    ''' </summary>
    Event EndSpeak()
End Interface