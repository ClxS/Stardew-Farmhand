namespace Farmhand.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Farmhand.API.Generic;

    /// <summary>
    ///     Contains useful extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        ///     Converts a <see cref="List{ItemQuantityPair}" /> into a ID-Count string.
        /// </summary>
        /// <param name="items">
        ///     The items to convert.
        /// </param>
        /// <returns>
        ///     The string equivalent.
        /// </returns>
        public static string ToItemSetString(this List<ItemQuantityPair> items)
        {
            return string.Join(" ", items.Select(x => $"{x.ItemId} {x.Count}"));
        }

        /// <summary>
        ///     Converts a <see cref="List{T}" /> into a space separated string.
        /// </summary>
        /// <param name="list">
        ///     The list to convert.
        /// </param>
        /// <typeparam name="T">
        ///     The type of items in the list.
        /// </typeparam>
        /// <returns>
        ///     The converted string.
        /// </returns>
        public static string ToSpaceSeparatedString<T>(this List<T> list)
        {
            return string.Join(" ", list);
        }

        /// <summary>
        ///     Gets an enum name as a string
        /// </summary>
        /// <param name="value">
        ///     The enum value.
        /// </param>
        /// <returns>
        ///     The name of the enum as a <see cref="string" />.
        /// </returns>
        public static string GetEnumName(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}