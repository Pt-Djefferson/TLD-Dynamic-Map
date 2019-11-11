using System;
using System.Globalization;
using System.Windows.Data;
using TLD_Dynamic_Map.Helpers;

namespace TLD_Dynamic_Map.Converters
{
    class PrefabNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gearName = value as string;
            if (gearName == null)
                return "";

            return ItemDictionary.GetInGameName(gearName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
