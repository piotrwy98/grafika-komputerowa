using System;
using System.Globalization;
using System.Windows.Data;

namespace GrafikaKomputerowa.Converters
{
    public class CompressionLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return long.Parse((string) parameter) == (long) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return long.Parse((string) parameter);
        }
    }
}
