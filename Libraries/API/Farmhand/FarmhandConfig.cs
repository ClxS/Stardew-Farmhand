namespace Farmhand
{
    /// <summary>
    ///     Contains configuration details for Farmhand.
    /// </summary>
    public class FarmhandConfig
    {
        /// <summary>
        ///     Gets or sets a value indicating whether debug mode is enabled.
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether we want to cache assembly ports.
        /// </summary>
        public bool CachePorts { get; set; } = true;
    }
}