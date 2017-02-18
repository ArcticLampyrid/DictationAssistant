Public Class BASS
    Private Declare Unicode Function BASS_Init Lib "bass.dll" (ByVal device As Integer, ByVal freq As Integer, ByVal flags As Integer, ByVal win As IntPtr, ByVal clsid As IntPtr) As Integer

    Private Declare Unicode Function BASS_Free Lib "bass.dll" () As Integer

    Private Const BASS_UNICODE As Integer = &H80000000

    Private Declare Unicode Function BASS_PluginLoad Lib "bass.dll" (ByVal file As String, ByVal flags As Integer) As Integer

    Private Declare Unicode Function BASS_PluginFree Lib "bass.dll" (ByVal handle As Integer) As Integer

    Public Shared Function Init(ByVal device As Integer, ByVal freq As Integer, ByVal flags As Integer, ByVal win As IntPtr) As Boolean
        Try
            Return BASS_Init(device, freq, flags, win, Nothing) <> 0
        Catch ex As Exception

        End Try
        Return False
    End Function
    Public Shared Function LoadPlugin(File As String) As Integer
        Try
            Return BASS_PluginLoad(File, BASS_UNICODE)
        Catch ex As Exception

        End Try
        Return 0
    End Function
    Public Shared Function FreePlugin(ByVal handle As Integer) As Boolean
        Try
            Return BASS_PluginFree(handle) <> 0
        Catch ex As Exception

        End Try
        Return False
    End Function
    Public Shared Function FreeAllPlugin() As Boolean
        Return FreePlugin(0)
    End Function
    Public Shared Function Free() As Boolean
        Try
            Return BASS_Free() <> 0
        Catch ex As Exception

        End Try
        Return False
    End Function
End Class
