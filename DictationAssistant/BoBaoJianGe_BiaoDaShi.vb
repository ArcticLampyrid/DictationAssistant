Imports System.Text.RegularExpressions
Imports NCalc

Public Class BoBaoJianGe_BiaoDaShi
    Implements BoBaoJianGe
    Private ExpressionObject As Expression
    Public Sub New(BiaoDaShi As String)
        ExpressionObject = New Expression(BiaoDaShi, EvaluateOptions.IgnoreCase)

        AddHandler ExpressionObject.EvaluateFunction,
            Sub(name As String, args As FunctionArgs)
                Select Case name.ToLower()
                    Case "Length".ToLower()
                        args.Result = args.Parameters(0).Evaluate().ToString().Length
                    Case "WordCount".ToLower()
                        args.Result = BoBaoJianGeHelper.GetWordCount(args.Parameters(0).Evaluate().ToString())
                    Case "RegexReplace".ToLower()
                        args.Result = Regex.Replace(args.Parameters(0).Evaluate().ToString(),
                                                    args.Parameters(1).Evaluate().ToString(),
                                                    args.Parameters(2).Evaluate().ToString(),
                                                    If(args.Parameters.Length >= 4 AndAlso
                                                    Convert.ToBoolean(args.Parameters(3).Evaluate()),
                                                    RegexOptions.IgnoreCase,
                                                    RegexOptions.None)
                                                    )
                End Select
            End Sub
    End Sub
    Public Function GetBoBoJianGe(CiYu As ICiYuXinXi) As Int32 Implements BoBaoJianGe.GetBoBoJianGe
        Dim EvaluateParameterCallback As EvaluateParameterHandler =
            Sub(name As String, args As ParameterArgs)
                Select Case name.ToLower()
                    Case "Length".ToLower()
                        args.Result = CiYu.GetCiYu().Length
                    Case "WordCount".ToLower()
                        args.Result = BoBaoJianGeHelper.GetWordCount(CiYu)
                    Case "Text".ToLower()
                        args.Result = CiYu.GetCiYu()
                End Select
            End Sub

        AddHandler ExpressionObject.EvaluateParameter, EvaluateParameterCallback
        Dim ReturnValue As Int32 = Convert.ToInt32(ExpressionObject.Evaluate())
        RemoveHandler ExpressionObject.EvaluateParameter, EvaluateParameterCallback
        Return ReturnValue
    End Function
End Class
