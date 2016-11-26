using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Farmhand.API.Generic;

namespace Farmhand.Helpers
{
    public static class ExtensionMethods
    {
        public static string ToItemSetString(this List<ItemQuantityPair> items)
        {
            return string.Join(" ", items.Select(x => $"{x.ItemId} {x.Count}"));
        }

        public static string ToSpaceSeparatedString<T>(this List<T> list)
        {
            return string.Join(" ", list);
        }

        public static string GetEnumName(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
