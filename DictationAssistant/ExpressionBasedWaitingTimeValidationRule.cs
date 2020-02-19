using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace DictationAssistant
{
    class ExpressionBasedWaitingTimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
			try
			{
                new ExpressionBasedWaitingTime(value.ToString());
                return ValidationResult.ValidResult;
            }
			catch (Exception e)
			{
                return new ValidationResult(false, e);
			}
        }
    }
}
