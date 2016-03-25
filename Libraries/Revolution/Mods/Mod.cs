using Revolution.Registries.Containers;

namespace Revolution
{
    public abstract class Mod
    {
        public ModManifest ModSettings { get; set; }

        public abstract void Entry();
        public virtual void LoadConfigurationSettings(string configPath) { }
    }
}
