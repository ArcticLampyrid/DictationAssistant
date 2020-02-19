using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DictationAssistant
{
    public class LanguageSpecificStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var strings = (IDictionary<XmlLanguage, string>)value;
            var localXmlLanguage = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
            if (!strings.TryGetValue(localXmlLanguage, out var result))
                result = strings.Values.FirstOrDefault();
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
