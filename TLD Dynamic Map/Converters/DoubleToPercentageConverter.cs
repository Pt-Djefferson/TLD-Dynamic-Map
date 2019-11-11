using System;
using System.Globalization;
using System.Windows.Data;

namespace TLD_Dynamic_Map.Converters
{
    class DoubleToPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;

            val = Math.Round(val * 100);
            return val + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
