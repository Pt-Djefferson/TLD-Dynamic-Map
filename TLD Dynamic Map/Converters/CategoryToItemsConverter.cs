using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using TLD_Dynamic_Map.Game_data;
using TLD_Dynamic_Map.Helpers;
using TLD_Dynamic_Map.Properties;

namespace TLD_Dynamic_Map.Converters
{
    class CategoryToItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(ItemCategory))
                return null;

            var cat = (ItemCategory)value;

            var result = new List<EnumerationMember>();
            foreach (KeyValuePair<string, ItemInfo> entry in ItemDictionary.itemInfo)
            {
                if (entry.Value.category == cat && !entry.Value.hide)
                {
                    var member = new EnumerationMember() { Value = entry.Key, Description = Resources.ResourceManager.GetString(entry.Key) ?? entry.Key };
                    result.Add(member);
                }
            }
            result = result.OrderBy(item => item.Description).ToList();
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
