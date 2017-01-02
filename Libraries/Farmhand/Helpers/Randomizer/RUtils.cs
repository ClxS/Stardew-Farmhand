namespace Farmhand.Helpers.Randomizer
{
    using System;
    using System.Linq;

    internal static class RUtils
    {
        public static int LuhnCalculate(long num)
        {
            var digits = num.ToString().ToCharArray().Reverse().ToList();
            var sum = 0;

            for (int i = 0, l = digits.Count; l > i; ++i)
            {
                var digit = int.Parse(digits[i].ToString());

                if (i % 2 == 0)
                {
                    digit *= 2;

                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
            }

            return sum * 9 % 10;
        }

        public static bool LuhnCheck(long num)
        {
            var str = num.ToString();
            var checkDigit = int.Parse(str[str.Length - 1].ToString());

            return checkDigit == LuhnCalculate(long.Parse(str.Substring(0, str.Length - 1)));
        }

        public static string NumberPadding(int num, int width = 2, char pad = '0')
            =>
                num.ToString().Length >= width
                    ? $"{num}"
                    : string.Join(pad.ToString(), new string[width - $"{num}".Length + 1]) + num;

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
            return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }

        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);
        }
    }
}
