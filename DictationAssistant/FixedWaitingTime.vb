
''' <summary>
''' 播报间隔接口
''' </summary>
''' <remarks></remarks>
Public Class FixedWaitingTime
    Implements IWaitingTimeCalculator
    Private FixedValue As Integer
    Public Sub New(ShiChang As Integer)
        Me.FixedValue = ShiChang
    End Sub
    Public Function CalculateWaitingTime(Word As String) As Int32 Implements IWaitingTimeCalculator.CalculateWaitingTime
        Return FixedValue
    End Function
End Class
