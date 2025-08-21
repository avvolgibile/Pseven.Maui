using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseven.Converters
{
    public class AggiungiIVA22Converter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is not null && !string.IsNullOrEmpty((string)value)
                ? Math.Round(double.Parse((string)value) * 1.22, 0).ToString().Replace(".", ",")
                : value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
