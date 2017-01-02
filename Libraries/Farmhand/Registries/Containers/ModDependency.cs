namespace Farmhand.Registries.Containers
{
    using System;

    /// <summary>
    ///     A dependency between mods
    /// </summary>
    public class ModDependency
    {
        /// <summary>
        ///     Gets or sets the unique id of our dependency.
        /// </summary>
        public string UniqueId { get; set; }

        /// <summary>
        ///     Gets or sets the minimum version.
        /// </summary>
        public Version MinimumVersion { get; set; }

        /// <summary>
        ///     Gets or sets the maximum version.
        /// </summary>
        public Version MaximumVersion { get; set; }

        /// <summary>
        ///     Gets or sets the dependency state.
        /// </summary>
        public DependencyState DependencyState { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether is required. If it is not required, load order will still be
        ///     enforced, but a missing/error loading dependency will not prevent this mod loading
        /// </summary>
        public bool IsRequired { get; set; } = true;
    }
}