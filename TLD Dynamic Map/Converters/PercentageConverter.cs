﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace TLD_Dynamic_Map.Converters
{
    class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;
            var target = (double)parameter;

            var percentage = Math.Round((val / target) * 100);

            return percentage + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
