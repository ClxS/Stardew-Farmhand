namespace Farmhand
{
    using Farmhand.Registries.Containers;

    /// <summary>
    ///     Base type for Farmhand mods.
    /// </summary>
    public abstract class Mod
    {
        /// <summary>
        ///     Gets or sets the information loaded from a mods Manifest.JSON. It is populated by the API prior to Entry being
        ///     called
        /// </summary>
        public ModManifest ModSettings { get; set; }

        /// <summary>
        ///     Gets or sets the mod configuration settings.
        /// </summary>
        public virtual ModSettings ConfigurationSettings { get; set; }

        /// <summary>
        ///     The Entry method for a mod. This method will be called during the loading process and is intended to let the
        ///     developer setup
        /// </summary>
        public abstract void Entry();
    }
}