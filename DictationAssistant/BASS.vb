Public Class BASS
    Private Declare Unicode Function BASS_Init Lib "bass.dll" (ByVal device As Int32, ByVal freq As Int32, ByVal flags As Int32, ByVal win As IntPtr, ByVal clsid As IntPtr) As Int32

    Private Declare Unicode Function BASS_Free Lib "bass.dll" () As Integer

    Private Declare Unicode Function BASS_SetVolume Lib "bass.dll" (volume As Single) As Integer

    Private Const BASS_UNICODE As Int32 = &H80000000

    Private Declare Unicode Function BASS_PluginLoad Lib "bass.dll" (ByVal file As String, ByVal flags As Int32) As Int32

    Private Declare Unicode Function BASS_PluginFree Lib "bass.dll" (ByVal handle As Int32) As Int32

    Public Shared Function Init(ByVal device As Integer, ByVal freq As Integer, ByVal flags As Integer, ByVal win As IntPtr) As Boolean
        Return BASS_Init(device, freq, flags, win, Nothing) <> 0
    End Function

    Public Shared Function LoadPlugin(File As String) As Int32
        Return BASS_PluginLoad(File, BASS_UNICODE)
    End Function
    Public Shared Function FreePlugin(ByVal handle As Int32) As Boolean
        Return BASS_PluginFree(handle) <> 0
    End Function
    Public Shared Function FreeAllPlugin() As Boolean
        Return FreePlugin(0)
    End Function

    Public Shared Function Free() As Boolean
        Return BASS_Free() <> 0
    End Function

    ''' <summary>
    ''' 设置音量
    ''' </summary>
    ''' <param name="volume">音量等级，0（无声）~1（最大）</param>
    ''' <returns>是否成功</returns>
    ''' <remarks></remarks>
    Public Shared Function SetVolume(volume As Single) As Boolean
        Return BASS_SetVolume(volume) <> 0
    End Function
End Class
