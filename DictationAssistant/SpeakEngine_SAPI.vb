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
    Private NotRaiseEndSpeakEventOne As Boolean
    Private _Culture As CultureInfo
    Public Event EndSpeak() Implements ISpeakEngine.EndSpeak
    Public Sub New(NativeEngine As Object)
        SpVoiceObject = New SpVoice
        SpVoiceObject.Voice = CType(NativeEngine, SpObjectToken)
        Try
            _Culture = New CultureInfo(Integer.Parse(SpVoiceObject.Voice.GetAttribute("Language").Split(";"c)(0),
                                                     NumberStyles.HexNumber,
                                                     CultureInfo.InvariantCulture),
                                       False)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub Speak(Text As String) Implements ISpeakEngine.Speak
        SpVoiceObject.Speak(Text, SpeechVoiceSpeakFlags.SVSFlagsAsync)
    End Sub

    Public Sub StopSpeak() Implements ISpeakEngine.StopSpeak
        If SpVoiceObject.Status.RunningState = SpeechRunState.SRSEDone Then
            Return
        End If

        NotRaiseEndSpeakEventOne = True
        Try
            SpVoiceObject.Speak(String.Empty, SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak) '部分引擎似乎会出错，所以加个Try
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SpVoiceObject_EndStream(StreamNumber As Integer, StreamPosition As Object) Handles SpVoiceObject.EndStream
        If NotRaiseEndSpeakEventOne Then
            NotRaiseEndSpeakEventOne = False
            Return
        End If
        RaiseEvent EndSpeak()
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
        Get
            Return _Culture
        End Get
    End Property

    Private Function IsSupportedLanguage(LanguageID As Integer) As Boolean
        Try
            Dim Languages() As String = SpVoiceObject.Voice.GetAttribute("Language").Split(";"c)
            For Each Language As String In Languages
                If Integer.Parse(Language, Globalization.NumberStyles.HexNumber) = LanguageID Then
                    Return True
                End If
            Next
        Catch ex As Exception

        End Try
        Return False
    End Function
End Class
