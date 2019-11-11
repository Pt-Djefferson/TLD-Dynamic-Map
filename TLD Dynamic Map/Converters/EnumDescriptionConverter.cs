using System;
using System.Globalization;
using System.Windows.Data;
using TLD_Dynamic_Map.Helpers;

namespace TLD_Dynamic_Map.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumerationExtension.GetDescription(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
