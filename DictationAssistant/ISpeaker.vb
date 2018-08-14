Public Interface ISpeaker
    Function Speak(Text As String) As ISpeakStateControler
End Interface
Public Interface ISpeakStateControler
    Sub StopSpeak()
    Event PlayCompleted As EventHandler
End Interface