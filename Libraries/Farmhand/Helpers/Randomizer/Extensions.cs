namespace Farmhand.Helpers.Randomizer
{
    using System;
    using System.Collections.Generic;

    internal static class Extensions
    {
        public static string Capitalize(this string @this)
            => @this[0].ToString().ToUpperInvariant() + @this.Substring(1);

        public static void Add<T1, T2, T3, T4>(
            this List<Tuple<T1, T2, T3, T4>> @this,
            T1 item1,
            T2 item2,
            T3 item3,
            T4 item4) => @this.Add(new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4));

        public static void Add<T1, T2>(this List<Tuple<T1, T2>> @this, T1 item1, T2 item2)
            => @this.Add(new Tuple<T1, T2>(item1, item2));

        public static void Add<T1, T2>(this List<KeyValuePair<T1, T2>> @this, T1 item1, T2 item2)
            => @this.Add(new KeyValuePair<T1, T2>(item1, item2));
    }
}
