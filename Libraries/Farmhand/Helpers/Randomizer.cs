using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Farmhand.Helpers.Data;

namespace Farmhand.Helpers
{
    internal static class Extensions
    {
        public static string Capitalize(this string @this) => @this[0].ToString().ToUpperInvariant() + @this.Substring(1);

        public static void Add<T1, T2, T3, T4>(this List<Tuple<T1, T2, T3, T4>> @this, T1 item1, T2 item2, T3 item3, T4 item4) => @this.Add(new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4));
        public static void Add<T1, T2>(this List<Tuple<T1, T2>> @this, T1 item1, T2 item2) => @this.Add(new Tuple<T1, T2>(item1, item2));
        public static void Add<T1, T2>(this List<KeyValuePair<T1, T2>> @this, T1 item1, T2 item2) => @this.Add(new KeyValuePair<T1, T2>(item1, item2));
    }

    internal static class RUtils
    {
        public static int LuhnCalcualte(long num)
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

            return checkDigit == LuhnCalcualte(long.Parse(str.Substring(0, str.Length - 1)));
        }

        public static string NumberPadding(int num, int width = 2, char pad = '0')
            => num.ToString().Length >= width
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

    internal static class Data
    {
        public const string Numbers = "0123456789";
        public const string CharsLower = "abcdefghijklmnopqrstuvwxyz";
        public static readonly string CharsUpper = CharsLower.ToUpperInvariant();
        public const string HexPool = Numbers + "abcdef";

        public static readonly List<Tuple<string, string, string, int>> Months = new List
            <Tuple<string, string, string, int>>
        {
                {"January", "Jan", "01", 31},
                {"February", "Feb", "02", 28},
                {"March", "Mar", "03", 31},
                {"April", "Apr", "04", 30},
                {"May", "May", "05", 31},
                {"June", "Jun", "06", 30},
                {"July", "Jul", "07", 31},
                {"August", "Aug", "08", 31},
                {"September", "Sep", "09", 30},
                {"October", "Oct", "10", 31},
                {"November", "Nov", "11", 30},
                {"December", "Dec", "12", 31}
            };
    }

    public enum CasingRules
    {
        LowerCase,
        UpperCase,
        MixedCase
    }

    public enum AgeRanges
    {
        Child,
        Teen,
        Adult,
        Senior,
        All
    }

    public enum ColorFormats
    {
        Hex,
        ShortHex,
        RGB,
        Ox,
        RGBA,
        Named
    }

    public class Randomizer
    {
        private readonly Func<double> _random;

        #region Constructors

        private Randomizer(uint? seed, bool test)
        {
            var mt = seed.HasValue && !test ? new MersenneTwister(seed.Value) : new MersenneTwister();
            _random = mt.GenRandReal2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Randomizer"/> class, using a time-dependent default seed value and default random generator function.
        /// </summary>
        public Randomizer() : this(null, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Randomizer"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value for the pseudo-random number sequence.</param>
        public Randomizer(uint seed) : this(seed, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Randomizer"/> class, using the specified random generator function.
        /// </summary>
        /// <param name="randomGenerator">A function used to generate random <see cref="double"/> values.</param>
        /// <exception cref="ArgumentNullException">Thrown when given <c>randomGenerator</c> is null.</exception>
        public Randomizer(Func<double> randomGenerator)
        {
            if (randomGenerator == null)
            {
                throw new ArgumentNullException(nameof(randomGenerator));
            }

            _random = randomGenerator;
        }

        #endregion

        #region Methods

        #region Basics

        /// <summary>
        /// Returns a random bool, either true or false.
        /// </summary>
        /// <param name="likelihood">The default likelihood of success (returning true) is 50%.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>likelihood</c> is less than 0 or greater than 100.</exception>
        /// <returns>Returns a random bool, either true or false.</returns>
        public bool NextBool(int likelihood = 50)
        {
            if (likelihood < 0 || likelihood > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(likelihood), "Likelihood accepts values from 0 to 100.");
            }

            return _random() * 100 < likelihood;
        }

        /// <summary>
        /// Returns a random integer.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns a random integer.</returns>
        public int NextInt(int min = int.MinValue + 1, int max = int.MaxValue - 1)
        {
            if (min > max)
            {
                throw new ArgumentException("Min cannot be greater than Max.", nameof(min));
            }

            return (int)Math.Floor(_random() * (max - min + 1) + min);
        }

        /// <summary>
        /// Returns a random long.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns a random long.</returns>
        public long NextLong(long min = long.MinValue + 1, long max = long.MaxValue - 1)
        {
            if (min > max)
            {
                throw new ArgumentException("Min cannot be greater than Max.", nameof(min));
            }

            return (long)Math.Floor(_random() * (max - min + 1) + min);
        }

        /// <summary>
        /// Returns a random natural integer.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentException">Thrown when <c>min</c> is less than 0.</exception>
        /// <returns>Returns a random natural integer.</returns>
        public int NextNatural(int min = 0, int max = int.MaxValue - 1)
        {
            if (min < 0)
            {
                throw new ArgumentException("Min cannot be less than zero.", nameof(min));
            }

            return NextInt(min, max);
        }

        /// <summary>
        /// Returns a random character.
        /// </summary>
        /// <param name="pool">Characters pool</param>
        /// <param name="alpha">Set to True to use only an alphanumeric character.</param>
        /// <param name="symbols">Set to true to return only symbols.</param>
        /// <param name="casing">Default casing.</param>
        /// <returns>Returns a random character.</returns>
        public char NextChar(string pool = null, bool alpha = false, bool symbols = false, CasingRules casing = CasingRules.MixedCase)
        {
            const string s = "!@#$%^&*()[]";
            string letters, p;

            if (alpha && symbols)
            {
                throw new ArgumentException("Cannot specify both alpha and symbols.");
            }

            if (casing == CasingRules.LowerCase)
            {
                letters = CharsLower;
            }
            else if (casing == CasingRules.UpperCase)
            {
                letters = CharsUpper;
            }
            else
            {
                letters = CharsLower + CharsUpper;
            }

            if (!string.IsNullOrEmpty(pool))
            {
                p = pool;
            }
            else if (alpha)
            {
                p = letters;
            }
            else if (symbols)
            {
                p = s;
            }
            else
            {
                p = letters + Numbers + s;
            }

            return p[NextNatural(max: p.Length - 1)];
        }

        /// <summary>
        /// Returns a random double.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <param name="decimals">Decimals count.</param>
        /// <returns>Returns random generated double.</returns>
        public double NextDouble(double min = double.MinValue + 1.0, double max = double.MaxValue - 1.0, uint decimals = 4)
        {
            var _fixed = Math.Pow(10, decimals);
            var num = NextLong((int)(min * _fixed), (int)(max * _fixed));
            var numFixed = (num / _fixed).ToString("N" + decimals);

            return double.Parse(numFixed);
        }

        /// <summary>
        /// Returns a random string.
        /// </summary>
        /// <param name="length">The length of the string.</param>
        /// <param name="pool">Characters pool</param>
        /// <param name="alpha">Set to True to use only an alphanumeric character.</param>
        /// <param name="symbols">Set to true to return only symbols.</param>
        /// <param name="casing">Default casing.</param>
        /// <exception cref="ArgumentException">Thrown when <c>length</c> is less than 0.</exception>
        /// <returns>Returns a random string.</returns>
        public string NextString(int length = 10, string pool = null, bool alpha = false, bool symbols = false,
            CasingRules casing = CasingRules.MixedCase)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length cannot be less than zero.", nameof(length));
            }

            return string.Join("",
                NextList<char>(new Func<string, bool, bool, CasingRules, char>(NextChar), length, pool, alpha, symbols,
                    casing));
        }

        /// <summary>
        /// Returns a list of n random terms.
        /// </summary>
        /// <param name="generator">Generator function to produce items.</param>
        /// <param name="count">The count of produced items.</param>
        /// <param name="args">The arguments list that will be passed to the generator function.</param>
        /// <exception cref="ArgumentNullException">Thrown when <c>generator</c> is null.</exception>
        /// <returns>Returns a list of n random terms.</returns>
        public List<T> NextList<T>(Delegate generator, int count = 1, params object[] args)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            var i = count > 0 ? count : 1;
            var arr = new List<T>();

            for (; i-- != 0;)
            {
                arr.Add((T)(args.Any() ? generator.DynamicInvoke(args) : generator.DynamicInvoke()));
            }

            return arr;
        }

        /// <summary>
        /// Given a function that generates something random and a number of items to generate.
        /// </summary>
        /// <typeparam name="T">The type of the returned list.</typeparam>
        /// <param name="generator">The function that generates something random.</param>
        /// <param name="count">Number of terms to generate.</param>
        /// <param name="comparator">Comparator function.</param>
        /// <param name="args">The arguments list that will be passed to the generator function.</param>
        /// <exception cref="ArgumentNullException">Thrown when <c>generator</c> is null.</exception>
        /// <returns>Returns an array of items where none repeat.</returns>
        public List<T> GenerateUniqueList<T>(Delegate generator, int count = 1, Func<List<T>, T, bool> comparator = null,
            params object[] args)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            var cmprt = comparator ?? ((arr, item) => arr.IndexOf(item) != -1);

            var list = new List<T>();
            int cnt = 0, maxDuplicates = count * 50;

            while (list.Count < count)
            {
                var res = (T)generator.DynamicInvoke(args);

                if (!cmprt(list, res))
                {
                    list.Add(res);

                    cnt = 0;
                }

                if (++cnt > maxDuplicates)
                {
                    throw new RankException("count is likely too large for sample set");
                }
            }

            return list;
        }

        /// <summary>
        /// Given a list, scramble the order and return it.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">A list to shuffle.</param>
        /// <returns>Returns shuffled list.</returns>
        public List<T> ShuffleList<T>(List<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var oldList = list.ToList();
            var newList = new List<T>();
            var len = oldList.Count;

            for (var i = 0; i < len; i++)
            {
                var j = NextNatural(max: oldList.Count - 1);

                newList.Add(oldList[j]);
                oldList.RemoveAt(j);
            }

            return newList;
        }

        /// <summary>
        /// Given a list, returns a single random element.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The list to pick from.</param>
        /// <exception cref="ArgumentNullException">Thrown when given list is null.</exception>
        /// <exception cref="ArgumentException">Thrown when given list is empty.</exception>
        /// <returns>Returns random item from the given list.</returns>
        public T PickRandomItem<T>(List<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (!list.Any())
            {
                throw new ArgumentException("Cannot pick from an empty list.", nameof(list));
            }

            return list[NextNatural(max: list.Count - 1)];
        }

        /// <summary>
        /// Given a list, returns a random set with <c>count</c> elements.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">THe list to pick from.</param>
        /// <param name="count">Number of items to pick.</param>
        /// <exception cref="ArgumentNullException">Thrown when given list is null.</exception>
        /// <exception cref="ArgumentException">Thrown when given list is empty.</exception>
        /// <exception cref="ArgumentException">Thrown when <c>count</c> is less than 0.</exception>
        /// <returns>Returns the set of picked items.</returns>
        public List<T> PickRandomSet<T>(List<T> list, int count = 2)
        {
            if (count == 0)
            {
                return new List<T>();
            }

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (!list.Any())
            {
                throw new ArgumentException("Cannot pick from an empty list.", nameof(list));
            }

            if (count < 0)
            {
                throw new ArgumentException("count must be positive number.", nameof(count));
            }

            return count == 1 ? new List<T>(new[] { PickRandomItem(list) }) : ShuffleList(list).Take(count).ToList();
        }

        #endregion

        #region Text

        /// <summary>
        /// Return a random paragraph generated from sentences populated by semi-pronounceable random (nonsense) words.
        /// </summary>
        /// <param name="sentencesCount">Number of sentences.</param>
        /// <returns>Returns random generated paragraph.</returns>
        public string NextParagraph(int sentencesCount = 5)
            => string.Join(" ", NextList<string>(new Func<int, bool, string>(NextSentence), sentencesCount, 15, true));

        /// <summary>
        /// Return a random sentence populated by semi-pronounceable random (nonsense) words.
        /// </summary>
        /// <param name="wordsCount">Number of words.</param>
        /// <param name="punctuation">True to use punctuation.</param>
        /// <returns>Returns random generated sentence.</returns>
        public string NextSentence(int wordsCount = 15, bool punctuation = true)
        {
            var punct = "";
            var text = string.Join(" ",
                NextList<string>(new Func<bool, int?, int?, string>(NextWord), wordsCount, false, 2, null)).Capitalize();

            if (punctuation && !new Regex(@"^[\.\?;!:]$", RegexOptions.IgnoreCase).IsMatch(text))
            {
                punct = ".";
            }

            if (punctuation)
            {
                text += punct;
            }

            return text;
        }

        /// <summary>
        /// Return a semi-pronounceable random (nonsense) word.
        /// </summary>
        /// <param name="capitalize">True to capitalize a word.</param>
        /// <param name="syllablesCount">Number of syllables which the word will have.</param>
        /// <param name="length">Length of a word.</param>
        /// <returns>Returns random generated word.</returns>
        public string NextWord(bool capitalize = false, int? syllablesCount = 2, int? length = null)
        {
            if (syllablesCount != null && length != null)
            {
                throw new ArgumentException("Cannot specify both syllablesCount AND length.");
            }

            var text = "";

            if (length.HasValue)
            {
                do
                {
                    text += NextSyllable();
                } while (text.Length < length.Value);

                text = text.Substring(0, length.Value);
            }
            else if (syllablesCount.HasValue)
            {
                for (var i = 0; i < syllablesCount.Value; i++)
                {
                    text += NextSyllable();
                }
            }

            if (capitalize)
            {
                text = text.Capitalize();
            }

            return text;
        }

        /// <summary>
        /// Return a semi-speakable syllable, 2 or 3 letters.
        /// </summary>
        /// <param name="length">Length of a syllable.</param>
        /// <param name="capitalize">True to capitalize a syllable.</param>
        /// <returns>Returns random generated syllable.</returns>
        public string NextSyllable(int length = 2, bool capitalize = false)
        {
            const string consonats = "bcdfghjklmnprstvwz";
            const string vowels = "aeiou";
            const string all = consonats + vowels;
            var text = "";
            var chr = -1;

            for (var i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    chr = NextChar(all);
                }
                else if (consonats.IndexOf((char)chr) == -1) //(consonats[chr] == -1)
                {
                    chr = NextChar(consonats);
                }
                else
                {
                    chr = NextChar(vowels);
                }

                text += (char)chr;
            }

            if (capitalize)
            {
                text = text.Capitalize();
            }

            return text;
        }

        #endregion

        #region Person

        /// <summary>
        /// Generates a random age
        /// </summary>
        /// <param name="types">Age range.</param>
        /// <returns>Returns random generated age.</returns>
        public int NextAge(AgeRanges types = AgeRanges.Adult)
        {
            int[] range;

            switch (types)
            {
                case AgeRanges.Child:
                    range = new[] { 1, 12 };
                    break;

                case AgeRanges.Teen:
                    range = new[] { 13, 19 };
                    break;

                case AgeRanges.Senior:
                    range = new[] { 65, 100 };
                    break;

                case AgeRanges.All:
                    range = new[] { 1, 100 };
                    break;

                default:
                    range = new[] { 18, 65 };
                    break;
            }

            return NextNatural(range[0], range[1]);
        }

        /// <summary>
        /// Generates a random Brazilian tax Id.
        /// </summary>
        /// <returns>Returns random generated Brazilian tax Id.</returns>
        public string NextCPF()
        {
            var n = NextList<int>(new Func<int, int, int>(NextNatural), 9, 0, 9).ToArray();
            var d1 = n[8] * 2 + n[7] * 3 + n[6] * 4 + n[5] * 5 + n[4] * 6 + n[3] * 7 + n[2] * 8 + n[1] * 9 + n[0] * 10;

            d1 = 11 - d1 % 11;

            if (d1 >= 10)
            {
                d1 = 0;
            }

            var d2 = d1 * 2 + n[8] * 3 + n[7] * 4 + n[6] * 5 + n[5] * 6 + n[4] * 7 + n[3] * 8 + n[2] * 9 + n[1] * 10 + n[0] * 11;

            d2 = 11 - d2 % 11;

            if (d2 >= 10)
            {
                d2 = 0;
            }

            return "" + n[0] + n[1] + n[2] + '.' + n[3] + n[4] + n[5] + '.' + n[6] + n[7] + n[8] + '-' + d1 + d2;
        }

        /// <summary>
        /// Generates a random social security number.
        /// </summary>
        /// <param name="ssnFour">True to generate last 4 digits.</param>
        /// <param name="dashes">False to remove dashes.</param>
        /// <returns>Returns random generated social security number.</returns>
        public string NextSSN(bool ssnFour = false, bool dashes = true)
        {
            const string ssnPool = "1234567890";
            string ssn, dash = dashes ? "-" : "";

            if (!ssnFour)
            {
                ssn = NextString(3, ssnPool) + dash + NextString(2, ssnPool) +
                      dash + NextString(4, ssnPool);
            }
            else
            {
                ssn = NextString(4, ssnPool);
            }

            return ssn;
        }

        #endregion

        #region Date\Time

        /// <summary>
        /// Generates a random year.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <returns>Returns random generated year.</returns>
        public int NextYear(int min = 2016, int max = 2116) => NextNatural(min, max);

        /// <summary>
        /// Generates a random month.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 12.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns random generated month.</returns>
        public string NextMonth(int min = 1, int max = 12)
        {
            if (min < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be less than 1.");
            }

            if (max > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 12.");
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
            }

            return PickRandomItem(Months.Skip(min - 1).Take(max - min - 1).ToList()).Item1;
        }

        /// <summary>
        /// Generates a random second.
        /// </summary>
        /// <returns>Returns random generated second.</returns>
        public int NextSecond() => NextNatural(max: 59);

        /// <summary>
        /// Generates a random minute.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 59.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns random generated minute.</returns>
        public int NextMinute(int min = 0, int max = 59)
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be less than 0.");
            }

            if (max > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 59.");
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
            }

            return NextNatural(min, max);
        }

        /// <summary>
        /// Generates a random hour.
        /// </summary>
        /// <param name="twentyfourHours">True to use 24-hours format.</param>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 23.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 12 in 12-hours format.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns random generated hour.</returns>
        public int NextHour(bool twentyfourHours = true, int? min = null, int? max = null)
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be less than 0.");
            }

            if (twentyfourHours && max > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 23 for twentyfourHours option.");
            }

            if (!twentyfourHours && max > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 12.");
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
            }

            min = min ?? (twentyfourHours ? 0 : 1);
            max = max ?? (twentyfourHours ? 23 : 12);

            return NextNatural(min.Value, max.Value);
        }

        /// <summary>
        /// Generates a random millisecond.
        /// </summary>
        /// <returns>Returns random generated millisecond.</returns>
        public int NextMillisecond() => NextNatural(max: 999);

        /// <summary>
        /// Generates a random date.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <returns>Returns random generated date.</returns>
        public DateTime NextDate(DateTime? min = null, DateTime? max = null)
        {
            if (min.HasValue && max.HasValue)
            {
                return RUtils.UnixTimestampToDateTime(NextLong((long)RUtils.DateTimeToUnixTimestamp(min.Value),
                    (long)RUtils.DateTimeToUnixTimestamp(max.Value)));
            }

            var m = NextNatural(1, 12);

            return new DateTime(NextYear(), m, NextNatural(1, Months[m - 1].Item4), NextHour(), NextMinute(),
                NextSecond(), NextMillisecond());
        }

        #endregion

        #endregion
    }
}
