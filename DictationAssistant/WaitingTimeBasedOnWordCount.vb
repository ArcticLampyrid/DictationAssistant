
''' <summary>
''' 播报间隔接口
''' </summary>
''' <remarks></remarks>
''' <summary>
''' Seconds = SecondsPerWord * WordCount + BasedSecond
''' </summary>
Public Class WaitingTimeBasedOnWordCount
    Implements IWaitingTimeCalculator
    Private SecondsPerWord As Int32
    Private BasedSecond As Int32
    Public Sub New(SecondsPerWord As Integer, Optional BasedSecond As Integer = 0)
        Me.SecondsPerWord = SecondsPerWord
        Me.BasedSecond = BasedSecond
    End Sub
    Public Function CalculateWaitingTime(Word As String) As Int32 Implements IWaitingTimeCalculator.CalculateWaitingTime
        Return If(SecondsPerWord <> 0, WaitingTimeHelper.GetWordCount(Word) * SecondsPerWord, 0) + BasedSecond
    End Function
End Class
