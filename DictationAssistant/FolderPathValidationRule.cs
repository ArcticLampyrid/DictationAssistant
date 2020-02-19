using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace DictationAssistant
{
    class FolderValidationRule : ValidationRule
    {
        public bool Emptiable { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var path = value?.ToString();
            if (string.IsNullOrEmpty(path))
                return Emptiable ? ValidationResult.ValidResult : new ValidationResult(false, "Can't be empty");
            if (!Directory.Exists(path))
            {
                return new ValidationResult(false, "Folder doesn't exist");
            }
            return ValidationResult.ValidResult;
        }
    }
}
