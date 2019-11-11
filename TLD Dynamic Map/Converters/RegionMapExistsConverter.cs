using System;
using System.Globalization;
using System.Windows.Data;
using TLD_Dynamic_Map.Helpers;

namespace TLD_Dynamic_Map.Converters
{
    class RegionMapExistsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string region = value as string;
            if (region == null)
                return false;
            return MapDictionary.MapExists(region);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
