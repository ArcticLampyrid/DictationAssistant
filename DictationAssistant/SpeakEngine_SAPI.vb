Imports 自动默写
Imports SpeechLib
Imports System.Collections.ObjectModel
Imports System.Globalization

Public Class SpeakEngine_SAPI
    Implements ISpeakEngine

    Private Shared AllNativeEngines As ReadOnlyCollection(Of Object)
    Shared Sub New()
        Dim CategoryIds As String() = {"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices", "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech Server\v11.0\Voices", "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices"}
        Dim NativeEngineList As New List(Of Object)
        For Each CategoryId In CategoryIds
            Try
                Dim ObjectTokenCategory As New SpObjectTokenCategory
                ObjectTokenCategory.SetId(CategoryId)
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
        AllNativeEngines = New ReadOnlyCollection(Of Object)(NativeEngineList)
    End Sub
    Public Shared Function GetAllNativeEngines() As ReadOnlyCollection(Of Object)
        Return AllNativeEngines
    End Function



    Private WithEvents SpVoiceObject As SpVoice
    Private CurrentSpeakStateControler As SpeakStateControler_SAPI
    Public Sub New(NativeEngine As Object)
        SpVoiceObject = New SpVoice
        SpVoiceObject.Voice = CType(NativeEngine, SpObjectToken)
        Try
            Culture = New CultureInfo(Integer.Parse(SpVoiceObject.Voice.GetAttribute("Language").Split(";"c)(0),
                                                     NumberStyles.HexNumber,
                                                     CultureInfo.InvariantCulture),
                                       False)
        Catch ex As Exception

        End Try
    End Sub

    Public Function Speak(Text As String) As ISpeakStateControler Implements ISpeakEngine.Speak
        If CurrentSpeakStateControler IsNot Nothing Then
            Dim LocalSpeakStateControler As SpeakStateControler_SAPI = CurrentSpeakStateControler
            CurrentSpeakStateControler = Nothing
            LocalSpeakStateControler.StopSpeak()
            LocalSpeakStateControler.OnEndSpeak()
        End If
        SpVoiceObject.Speak(Text, SpeechVoiceSpeakFlags.SVSFlagsAsync)
        CurrentSpeakStateControler = New SpeakStateControler_SAPI(Me)
        Return CurrentSpeakStateControler
    End Function

    Private Sub SpVoiceObject_EndStream(StreamNumber As Integer, StreamPosition As Object) Handles SpVoiceObject.EndStream
        If CurrentSpeakStateControler IsNot Nothing Then
            Dim LocalSpeakStateControler As SpeakStateControler_SAPI = CurrentSpeakStateControler
            CurrentSpeakStateControler = Nothing
            LocalSpeakStateControler.OnEndSpeak()
            LocalSpeakStateControler = Nothing
        End If
    End Sub

    Public ReadOnly Property Name As String Implements ISpeakEngine.Name
        Get
            Try
                Return SpVoiceObject.Voice.GetAttribute("name") '用GetDescription在部分罕见的引擎会出错
            Catch ex As Exception
                Throw New NotSupportedException
            End Try
        End Get
    End Property

    Public Property Volume As Byte Implements ISpeakEngine.Volume
        Get
            Return Convert.ToByte(SpVoiceObject.Volume)
        End Get
        Set(value As Byte)
            SpVoiceObject.Volume = value
        End Set
    End Property

    Public Property Rate As SByte Implements ISpeakEngine.Rate
        Get
            Return Convert.ToSByte(SpVoiceObject.Rate)
        End Get
        Set(value As SByte)
            SpVoiceObject.Rate = value
        End Set
    End Property

    Public ReadOnly Property Culture As CultureInfo Implements ISpeakEngine.Culture

    Private Class SpeakStateControler_SAPI
        Implements ISpeakStateControler

        Private SpeakEngineObject As SpeakEngine_SAPI

        Public Sub New(SpeakEngineObject As SpeakEngine_SAPI)
            Me.SpeakEngineObject = SpeakEngineObject
        End Sub

        Public Event EndSpeak() Implements ISpeakStateControler.EndSpeak

        Public Sub OnEndSpeak()
            SpeakEngineObject = Nothing
            RaiseEvent EndSpeak()
        End Sub

        Public Sub StopSpeak() Implements ISpeakStateControler.StopSpeak
            If Me.SpeakEngineObject Is Nothing Then
                Return
            End If
            If SpeakEngineObject.SpVoiceObject.Status.RunningState = SpeechRunState.SRSEDone Then
                SpeakEngineObject.CurrentSpeakStateControler = Nothing
                SpeakEngineObject = Nothing
                Return
            End If

            SpeakEngineObject.CurrentSpeakStateControler = Nothing
            Try
                Dim OldEventInterests As SpeechVoiceEvents
                OldEventInterests = SpeakEngineObject.SpVoiceObject.EventInterests
                SpeakEngineObject.SpVoiceObject.EventInterests = 0
                SpeakEngineObject.SpVoiceObject.Speak(String.Empty, SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak) '部分引擎似乎会出错，所以加个Try
                SpeakEngineObject.SpVoiceObject.EventInterests = OldEventInterests
            Catch ex As Exception

            End Try
            SpeakEngineObject = Nothing
        End Sub
    End Class
End Class
