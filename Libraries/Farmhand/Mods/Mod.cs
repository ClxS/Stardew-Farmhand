using Farmhand.Registries.Containers;

namespace Farmhand
{
    /// <summary>
    /// Base type for Farmhand mods.
    /// </summary>
    public abstract class Mod
    {
        /// <summary>
        /// Contains information loaded from a mod's Manifest.json. It is populated by the API prior to Entry being called
        /// </summary>
        public ModManifest ModSettings { get; set; }

        /// <summary>
        /// The Entry method for a mod. This method will be called during the loading process and is intended to let the developer setup 
        /// </summary>
        public abstract void Entry();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configPath"></param>
        public virtual void LoadConfigurationSettings(string configPath) { }
    }
}
