using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using TLD_Dynamic_Map.Game_data;

namespace TLD_Dynamic_Map.Converters
{
    public class InjuriesToOpacityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int param = (int)parameter;
            var afflictions = (Collection<Affliction>)value;
            return afflictions.Any(item => item.Location == param);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
