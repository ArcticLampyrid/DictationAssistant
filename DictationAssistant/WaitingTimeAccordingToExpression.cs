using System;
using System.Text.RegularExpressions;
using NCalc;

namespace DictationAssistant
{
    public class WaitingTimeAccordingToExpression : IWaitingTimeCalculator
    {
        private Expression ExpressionObject;
        public WaitingTimeAccordingToExpression(string expressionText)
        {
            ExpressionObject = new Expression(expressionText, EvaluateOptions.IgnoreCase);
            ExpressionObject.EvaluateFunction += (string name, FunctionArgs args) =>
            {
                switch (name.ToLower())
                {
                    case "length":
                        {
                            args.Result = args.Parameters[0].Evaluate().ToString().Length;
                            break;
                        }

                    case "wordcount":
                        {
                            args.Result = WaitingTimeHelper.GetWordCount(args.Parameters[0].Evaluate().ToString());
                            break;
                        }

                    case "regexreplace":
                        {
                            // RegexReplace(Input, Pattern, Replacement, [IgnoreCase = False])
                            args.Result = Regex.Replace(args.Parameters[0].Evaluate().ToString(), args.Parameters[1].Evaluate().ToString(), args.Parameters[2].Evaluate().ToString(), args.Parameters.Length >= 4 && Convert.ToBoolean(args.Parameters[3].Evaluate()) ? RegexOptions.IgnoreCase : RegexOptions.None
    );
                            break;
                        }

                    case "regexmatch":
                        {
                            // RegexMatch(Input, Pattern, [IgnoreCase = False])
                            args.Result = Regex.IsMatch(args.Parameters[0].Evaluate().ToString(), args.Parameters[1].Evaluate().ToString(), args.Parameters.Length >= 3 && Convert.ToBoolean(args.Parameters[2].Evaluate()) ? RegexOptions.IgnoreCase : RegexOptions.None
    );
                            break;
                        }
                }
            };
        }
        public Int32 CalculateWaitingTime(string word)
        {
            void EvaluateParameterCallback(string name, ParameterArgs args)
            {
                switch (name.ToLower())
                {
                    case "length":
                        {
                            args.Result = word.Length;
                            break;
                        }

                    case "wordcount":
                        {
                            args.Result = WaitingTimeHelper.GetWordCount(word);
                            break;
                        }

                    case "text":
                        {
                            args.Result = word;
                            break;
                        }
                }
            }

            ExpressionObject.EvaluateParameter += EvaluateParameterCallback;
            Int32 ReturnValue = Convert.ToInt32(ExpressionObject.Evaluate());
            ExpressionObject.EvaluateParameter -= EvaluateParameterCallback;
            return ReturnValue;
        }
    }
}
