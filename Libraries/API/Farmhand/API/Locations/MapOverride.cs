namespace Farmhand.API.Locations
{
    /// <summary>
    ///     Enumeration of valid map override types.
    /// </summary>
    public enum MapOverride
    {
        /// <summary>
        ///     When merging, allow other maps to override this maps changes without conflict.
        /// </summary>
        SoftMerge,

        /// <summary>
        ///     When merging, use regular merging rules.
        /// </summary>
        NormalMerge,

        /// <summary>
        ///     When merging, use ONLY this map, ignore other maps.
        /// </summary>
        FullOverride
    }
}