Imports System.Globalization
Imports System.IO

Public Class ImprovedSpeakEngine
    Implements ISpeakEngine

    Private Shared ReadOnly AudioFileExtensions As String() = "wav;flac;ape;m4a;opus;acc;mp3;mp2;mp1;ogg;wma;aif;mp4".Split(";"c)

    Private EngineForNoFile As ISpeakEngine
    Private ResForImproving As String
    Public Sub New(EngineForNoFile As ISpeakEngine, ResForImproving As String)
        Me.EngineForNoFile = EngineForNoFile
        Me.ResForImproving = ResForImproving
    End Sub

    Public ReadOnly Property Name As String Implements ISpeakEngine.Name
        Get
            Return "[Improved] " & EngineForNoFile.Name
        End Get
    End Property
    Public ReadOnly Property Culture As CultureInfo Implements ISpeakEngine.Culture
        Get
            Return EngineForNoFile.Culture
        End Get
    End Property


    Private Function FindFile(text As String) As String
        Dim filePath As String
        filePath = text
        filePath = filePath.Replace("\", "")
        filePath = filePath.Replace("/", "")
        filePath = filePath.Replace(":", "")
        filePath = filePath.Replace("*", "")
        filePath = filePath.Replace("?", "")
        filePath = filePath.Replace(Chr(34), "")
        filePath = filePath.Replace("<", "")
        filePath = filePath.Replace(">", "")
        filePath = filePath.Replace("|", "")
        filePath = Path.Combine(ResForImproving, filePath & ".")
        For Each extension In AudioFileExtensions
            If File.Exists(filePath & extension) Then
                Return filePath & extension
            End If
        Next
        Throw New FileNotFoundException($"未找到词组{text}对应的音频文件")
    End Function
    Public Function Speak(Text As String, Param As SpeakParam) As PcmStreamWithInfo Implements ISpeakEngine.Speak
        Try
            Return AudioFileDecodeStream.Create(FindFile(Text))
        Catch ex As Exception
            If EngineForNoFile Is Nothing Then
                Return PcmStreamWithInfo.Empty
            End If
            Return EngineForNoFile.Speak(Text, Param)
        End Try
    End Function
End Class
