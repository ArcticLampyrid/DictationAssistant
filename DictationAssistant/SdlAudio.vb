Imports System.Runtime.InteropServices

Public Class SdlAudio

    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function SDL_Init(ByVal flags As UInteger) As Integer
    End Function

    <DllImport("SDL2.dll", ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Private Shared Function SDL_SetHintWithPriority(ByVal name As String, ByVal value As String, ByVal priority As Integer) As Integer
    End Function

    Private Const SDL_INIT_AUDIO As UInteger = &H10UI
    Shared Sub New()
        SDL_SetHintWithPriority("SDL_WINDOWS_DISABLE_THREAD_NAMING", "1", 2)
        SDL_Init(SDL_INIT_AUDIO)
    End Sub
    Public Shared Sub Use()
        Return
    End Sub
End Class
