using Revolution.Registries.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution
{
    public abstract class Mod
    {
        public ModInfo ModSettings { get; set; }

        public abstract void Entry();
        public virtual void LoadConfigurationSettings(string configPath) { }
    }
}
