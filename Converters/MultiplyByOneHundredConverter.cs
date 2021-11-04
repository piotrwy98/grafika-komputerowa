using System;
using System.Globalization;
using System.Windows.Data;

namespace GrafikaKomputerowa.Converters
{
    public class MultiplyByOneHundredConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Round((float) value * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double) value / 100;
        }
    }
}
