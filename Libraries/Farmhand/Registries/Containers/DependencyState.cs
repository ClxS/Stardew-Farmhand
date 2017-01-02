namespace Farmhand.Registries.Containers
{
    /// <summary>
    ///     Dependency resolution states.
    /// </summary>
    public enum DependencyState
    {
        /// <summary>
        ///     Dependency found and is a suitable version.
        /// </summary>
        Ok,

        /// <summary>
        ///     Dependency is missing.
        /// </summary>
        Missing,

        /// <summary>
        ///     A dependency's dependency is missing.
        /// </summary>
        ParentMissing,

        /// <summary>
        ///     The dependency was located, but it is too outdated.
        /// </summary>
        TooLowVersion,

        /// <summary>
        ///     The dependency was located, but it is too recent.
        /// </summary>
        TooHighVersion
    }
}