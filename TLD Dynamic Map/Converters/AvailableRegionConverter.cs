using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using TLD_Dynamic_Map.Helpers;

namespace TLD_Dynamic_Map.Converters
{
    public class AvailableRegionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = new HashSet<string>();
            foreach (object o in values)
            {
                if (o.GetType() == typeof(List<string>))
                {
                    var l = (List<string>)o;
                    result.UnionWith(l);
                }
                else if (o.GetType() == typeof(string))
                {
                    result.Add((string)o);
                }
            }
            return result.Select(item => new EnumerationMember { Value = item, Description = MapDictionary.GetInGameName(item) });
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
