namespace Farmhand.API.NPCs
{
    using System;

    using Farmhand.API.NPCs.Characteristics;

    /// <summary>
    ///     Utility to aid the creation of Non-Playable Characters.
    /// </summary>
    public static class NpcUtility
    {
        /// <summary>
        ///     Gets the birthday season as a string.
        /// </summary>
        /// <param name="season">
        ///     The season to get.
        /// </param>
        /// <returns>
        ///     The birthday season as a string.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if season is not a valid value.
        /// </exception>
        public static string GetBirthdaySeasonValue(BirthdaySeason season)
        {
            switch (season)
            {
                case BirthdaySeason.Spring:
                    return "spring";
                case BirthdaySeason.Summer:
                    return "summer";
                case BirthdaySeason.Fall:
                    return "fall";
                case BirthdaySeason.Winter:
                    return "winter";
                default:
                    throw new ArgumentOutOfRangeException(nameof(season), season, null);
            }
        }
    }
}