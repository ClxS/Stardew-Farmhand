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
        ///     LINQ-extension which providers Distinct by a provided property.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>
        ///     The distinct items.
        /// </returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        ///     Returns the maximal element of the given sequence, based on
        ///     the given projection.
        /// </summary>
        /// <remarks>
        ///     If more than one element has the maximal projected value, the first
        ///     one encountered will be returned. This overload uses the default comparer
        ///     for the projected type. This operator uses immediate execution, but
        ///     only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="selector" /> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source" /> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, null);
        }

        /// <summary>
        ///     Returns the maximal element of the given sequence, based on
        ///     the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        ///     (From MoreLinq)
        ///     If more than one element has the maximal projected value, the first
        ///     one encountered will be returned. This operator uses immediate execution, but
        ///     only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" />, <paramref name="selector" />
        ///     or <paramref name="comparer" /> is null
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source" /> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }

                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }

                return max;
            }
        }

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