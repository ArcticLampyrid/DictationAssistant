Imports 自动默写
Imports SpeechLib
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports System.IO

Public Class MicrosoftSpeechEngine
    Implements ISpeakEngine
    Private Shared AllNativeEngines As ReadOnlyCollection(Of Object)
    Shared Sub New()
        Dim NativeEngineList As New List(Of Object)
        For Each CategoryId In {
            "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices",
            "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech Server\v11.0\Voices",
            "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices"
        }
            Try
                Dim ObjectTokenCategory As New SpObjectTokenCategory
                ObjectTokenCategory.SetId(CategoryId, False)
                Dim ObjectTokens As ISpeechObjectTokens = ObjectTokenCategory.EnumerateTokens(String.Empty, String.Empty)
                Dim i As Int32 = 0
                Dim Count As Int32 = ObjectTokens.Count
                While i < Count
                    NativeEngineList.Add(ObjectTokens.Item(i))
                    i += 1
                End While
            Catch ex As Exception

            End Try
        Next
        AllNativeEngines = NativeEngineList.AsReadOnly()
    End Sub
    Public Shared Function GetAllNativeEngines() As ReadOnlyCollection(Of Object)
        Return AllNativeEngines
    End Function


    Public ReadOnly Property Culture As CultureInfo Implements ISpeakEngine.Culture
    Public ReadOnly Property Name As String Implements ISpeakEngine.Name
    Private ReadOnly NativeEngine As SpObjectToken
    Public Sub New(NativeEngineObject As Object)
        NativeEngine = CType(NativeEngineObject, SpObjectToken)
        Try
            Name = NativeEngine.GetAttribute("name")
            Culture = New CultureInfo(Integer.Parse(NativeEngine.GetAttribute("Language").Split(";"c)(0),
                                                     NumberStyles.HexNumber,
                                                     CultureInfo.InvariantCulture),
                                       False)
        Catch ex As Exception

        End Try
    End Sub
    Public Function Speak(Text As String, Param As SpeakParam) As PcmStreamWithInfo Implements ISpeakEngine.Speak
        Dim voice As New SpVoice
        Dim sps As New SpMemoryStream
        Dim format As New WaveFormatEx
        sps.Format.Type = SpeechAudioFormatType.SAFT44kHz16BitStereo
        Dim PcmFormat As New PcmFormatInfo(44100, 2, PcmSampleFormat.S16)
        voice.Voice = NativeEngine
        voice.AudioOutputStream = sps
        voice.Rate = Param.Rate
        voice.Speak(Text, SpeechVoiceSpeakFlags.SVSFDefault)

        Dim data As Byte() = CType(sps.GetData(), Byte())
        Dim bytes_200ms = (PcmFormat.Freq \ 5) * PcmFormat.BlockAlign

        Dim i As Integer

        i = 0
        While i < data.Length
            If Not PcmSampleFormatHelper.IsEmptyAudio(PcmFormat.SampleFormat, data, i, PcmFormat.BlockAlign) Then
                Exit While
            End If
            i += PcmFormat.BlockAlign
        End While
        If i > bytes_200ms Then
            Dim start = i - bytes_200ms
            Dim length = data.Length - start
            Dim temp(length - 1) As Byte
            Array.Copy(data, start, temp, 0, length)
            data = temp
        End If

        i = data.Length
        While i > 0
            If Not PcmSampleFormatHelper.IsEmptyAudio(PcmFormat.SampleFormat, data, i - PcmFormat.BlockAlign, PcmFormat.BlockAlign) Then
                Exit While
            End If
            i -= PcmFormat.BlockAlign
        End While
        If data.Length - i > bytes_200ms Then
            Dim length = i + bytes_200ms
            Dim temp(length - 1) As Byte
            Array.Copy(data, 0, temp, 0, length)
            data = temp
        End If

        Return New PcmStreamWithInfo(New MemoryStream(data, False), PcmFormat)
    End Function
End Class
