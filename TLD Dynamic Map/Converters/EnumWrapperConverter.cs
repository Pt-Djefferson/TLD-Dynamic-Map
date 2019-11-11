using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TLD_Dynamic_Map.Helpers;
using System.Linq;

namespace TLD_Dynamic_Map.Converters
{
    public class EnumWrapperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumWrapper = (EnumWrapper) value;
            var enumName = enumWrapper.GetType().GetGenericArguments()[0].Name;

            return enumWrapper.Values.Select(s => new EnumerationMember
            {
                Value = s,
                Description = Properties.Resources.ResourceManager.GetString(enumName + "_" + s) ?? s
            }).ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
