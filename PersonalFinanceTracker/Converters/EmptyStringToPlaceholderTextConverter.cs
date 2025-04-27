using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PersonalFinanceTracker.Converters
{
    public class EmptyStringToPlaceholderTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return parameter; // return the placeholder text
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value == null || string.IsNullOrEmpty(value.ToString()))
            //{
            //    return parameter; // return the placeholder text
            //}
            return value;

        }
    }
}
