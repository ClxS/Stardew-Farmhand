namespace Farmhand.Events.Arguments.ApiEvents
{
    using System;

    using Farmhand.Registries.Containers;

    /// <summary>
    ///     Arguments for ModLoad.
    /// </summary>
    public class ModLoadEventEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ModLoadEventEventArgs" /> class.
        /// </summary>
        /// <param name="mod">
        ///     The mod loaded.
        /// </param>
        public ModLoadEventEventArgs(ModManifest mod)
        {
            this.Mod = mod;
        }

        /// <summary>
        ///     Gets the mod loaded.
        /// </summary>
        public ModManifest Mod { get; }
    }
}