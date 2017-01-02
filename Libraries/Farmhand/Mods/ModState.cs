namespace Farmhand
{
    /// <summary>
    ///     Contains the variable possible states of a Mod
    /// </summary>
    public enum ModState
    {
        /// <summary>
        ///     Unloaded mods have not yet been loaded by the ModLoader
        /// </summary>
        Unloaded,

        /// <summary>
        ///     Loaded mods are activate and should be functioning
        /// </summary>
        Loaded,

        /// <summary>
        ///     Deactivated mods are ones which were manually deactivated
        /// </summary>
        Deactivated,

        /// <summary>
        ///     This mod was found to have a missing dependency by the ModLoader
        /// </summary>
        MissingDependency,

        /// <summary>
        ///     A required dependency of this mod failed on entry.
        /// </summary>
        DependencyLoadError,

        /// <summary>
        ///     This mod threw an exception and was forcibly unloaded by the ModLoader
        /// </summary>
        Errored,

        /// <summary>
        ///     This mod did not have a valid manifest. Typically this is caused by things such as invalid UniqueIDs or Content
        ///     issues.
        /// </summary>
        InvalidManifest
    }
}