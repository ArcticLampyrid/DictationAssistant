Imports System.Text.RegularExpressions
Imports NCalc

Public Class WaitingTimeAccordingToExpression
    Implements IWaitingTimeCalculator
    Private ExpressionObject As Expression
    Public Sub New(ExpressionText As String)
        ExpressionObject = New Expression(ExpressionText, EvaluateOptions.IgnoreCase)
        AddHandler ExpressionObject.EvaluateFunction,
            Sub(name As String, args As FunctionArgs)
                Select Case name.ToLower()
                    Case "Length".ToLower()
                        args.Result = args.Parameters(0).Evaluate().ToString().Length
                    Case "WordCount".ToLower()
                        args.Result = WaitingTimeHelper.GetWordCount(args.Parameters(0).Evaluate().ToString())
                    Case "RegexReplace".ToLower()
                        'RegexReplace(Input, Pattern, Replacement, [IgnoreCase = False])
                        args.Result = Regex.Replace(args.Parameters(0).Evaluate().ToString(),
                                                    args.Parameters(1).Evaluate().ToString(),
                                                    args.Parameters(2).Evaluate().ToString(),
                                                    If(args.Parameters.Length >= 4 AndAlso Convert.ToBoolean(args.Parameters(3).Evaluate()), RegexOptions.IgnoreCase, RegexOptions.None)
                                                    )

                    Case "RegexMatch".ToLower()
                        'RegexMatch(Input, Pattern, [IgnoreCase = False])
                        args.Result = Regex.IsMatch(args.Parameters(0).Evaluate().ToString(),
                                                    args.Parameters(1).Evaluate().ToString(),
                                                    If(args.Parameters.Length >= 3 AndAlso Convert.ToBoolean(args.Parameters(2).Evaluate()), RegexOptions.IgnoreCase, RegexOptions.None)
                                                    )
                End Select
            End Sub
    End Sub
    Public Function CalculateWaitingTime(Word As String) As Int32 Implements IWaitingTimeCalculator.CalculateWaitingTime
        Dim EvaluateParameterCallback As EvaluateParameterHandler =
            Sub(name As String, args As ParameterArgs)
                Select Case name.ToLower()
                    Case "Length".ToLower()
                        args.Result = Word.Length
                    Case "WordCount".ToLower()
                        args.Result = WaitingTimeHelper.GetWordCount(Word)
                    Case "Text".ToLower()
                        args.Result = Word
                End Select
            End Sub

        AddHandler ExpressionObject.EvaluateParameter, EvaluateParameterCallback
        Dim ReturnValue As Int32 = Convert.ToInt32(ExpressionObject.Evaluate())
        RemoveHandler ExpressionObject.EvaluateParameter, EvaluateParameterCallback
        Return ReturnValue
    End Function
End Class
