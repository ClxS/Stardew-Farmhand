namespace Farmhand.Helpers.Randomizer
{
    using System;
    using System.Collections.Generic;

    internal static class Data
    {
        public const string Numbers = "0123456789";

        public const string CharsLower = "abcdefghijklmnopqrstuvwxyz";

        public const string HexPool = Numbers + "abcdef";

        public static readonly string CharsUpper = CharsLower.ToUpperInvariant();

        public static readonly List<Tuple<string, string, string, int>> Months =
            new List<Tuple<string, string, string, int>>
                {
                    { "January", "Jan", "01", 31 },
                    { "February", "Feb", "02", 28 },
                    { "March", "Mar", "03", 31 },
                    { "April", "Apr", "04", 30 },
                    { "May", "May", "05", 31 },
                    { "June", "Jun", "06", 30 },
                    { "July", "Jul", "07", 31 },
                    { "August", "Aug", "08", 31 },
                    { "September", "Sep", "09", 30 },
                    { "October", "Oct", "10", 31 },
                    { "November", "Nov", "11", 30 },
                    { "December", "Dec", "12", 31 }
                };
    }
}
