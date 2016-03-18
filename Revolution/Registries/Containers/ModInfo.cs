using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Revolution.Registries.Containers
{
    public enum ModState
    {
        Unloaded,
        Loaded,
        MissingDependency,
        Errored,
        ForciblyUnloaded
    }

    public class ModInfo
    {
        public ModInfo()
        {
            ModState = ModState.Unloaded;
        }

        public string UniqueModId { get; set; }
        public string ModDLL { get; set; }
        public string Name { get; set; }        
        public string Author { get; set; }
        public Version Version { get; set; }
        public string Description { get; set; }
        public string ConfigurationFile { get; set; }
        public List<ModDependency> Dependencies { get; set; }

        [JsonIgnore]
        public ModState ModState { get; set; }

        [JsonIgnore]
        public bool HasDLL { get { return !string.IsNullOrWhiteSpace(ModDLL); } }
        [JsonIgnore]
        public bool HasConfig { get { return HasDLL && !string.IsNullOrWhiteSpace(ConfigurationFile); } }

        [JsonIgnore]
        public bool HasContent { get { return false; } }

        [JsonIgnore]
        public Mod Instance { get; set; }

        [JsonIgnore]
        public string ModRoot { get; set; }

        [JsonIgnore]
        private object ConfigurationSettings { get; set; }

        public bool LoadModDLL()
        {
            if (Instance != null)
            {
                throw new Exception("Error! Mod has already been loaded!");
            }
            
            var modDllPath = ModRoot + "\\" + ModDLL;

            if (!modDllPath.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                modDllPath += ".dll";
            }

            try
            {
                Assembly mod = Assembly.LoadFrom(modDllPath);
                if (mod.GetTypes().Count(x => x.BaseType == typeof(Mod)) > 0)
                {
                    Type tar = mod.GetTypes().First(x => x.BaseType == typeof(Mod));
                    Instance = (Mod)mod.CreateInstance(tar.ToString());
                    Instance.ModSettings = this;
                    Instance.Entry();
                    Console.WriteLine("Loaded mod: {0}", Name);
                }
                else
                {
                    throw new Exception("Invalid Mod DLL");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to load mod '{0}'\n", modDllPath), ex);
            }

            return Instance != null;
        }
    }
}
