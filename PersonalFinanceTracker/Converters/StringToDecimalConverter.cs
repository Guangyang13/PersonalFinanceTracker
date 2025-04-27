using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PersonalFinanceTracker.Converters
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            if (string.IsNullOrEmpty(value.ToString())) return 0;
            if (decimal.TryParse(value.ToString(), out decimal amount)) return amount;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            if (string.IsNullOrEmpty(value.ToString())) return 0;
            if (decimal.TryParse(value.ToString(), out decimal amount)) return amount.ToString();
            return 0;
        }
    }
}
