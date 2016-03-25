using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Revolution.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Revolution.Registries.Containers
{
    public enum ModState
    {
        Unloaded,
        Loaded,
        Deactivated,
        MissingDependency,
        Errored,
        ForciblyUnloaded,
        InvalidManifest
    }

    public class ModInfo
    {
        public ModInfo()
        {
            ModState = ModState.Unloaded;
        }

        public string UniqueId { get; set; }
        public string ModDll { get; set; }
        public string Name { get; set; }        
        public string Author { get; set; }
        public Version Version { get; set; }
        public string Description { get; set; }
        public string ConfigurationFile { get; set; }
        public List<ModDependency> Dependencies { get; set; }
        public ModContent Content { get; set; }

        [JsonIgnore]
        public ModState ModState { get; set; }
        [JsonIgnore]
        public Exception LastException { get; set; }
        [JsonIgnore]
        public bool HasDll => !string.IsNullOrWhiteSpace(ModDll);
        [JsonIgnore]
        public bool HasConfig => HasDll && !string.IsNullOrWhiteSpace(ConfigurationFile);
        [JsonIgnore]
        public bool HasContent => Content != null && Content.HasContent;
        [JsonIgnore]
        public Assembly ModAssembly { get; set; }
        [JsonIgnore]
        public Mod Instance { get; set; }
        [JsonIgnore]
        public string ModRoot { get; set; }
        [JsonIgnore]
        private object ConfigurationSettings { get; set; }
        
        public bool LoadModDll()
        {
            if (Instance != null)
            {
                throw new Exception("Error! Mod has already been loaded!");
            }
            
            var modDllPath = ModRoot + "\\" + ModDll;

            if (!modDllPath.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                modDllPath += ".dll";
            }

            try
            {
                ModAssembly = Assembly.LoadFrom(modDllPath);
                if (ModAssembly.GetTypes().Count(x => x.BaseType == typeof(Mod)) > 0)
                {
                    var tar = ModAssembly.GetTypes().First(x => x.BaseType == typeof(Mod));
                    Instance = (Mod)ModAssembly.CreateInstance(tar.ToString());
                    if (Instance != null)
                    {
                        Instance.ModSettings = this;
                        Instance.Entry();
                    }
                    Log.Verbose ($"Loaded mod dll: {Name}");
                }
                else
                {
                    throw new Exception("Invalid Mod DLL");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Failed to load mod '{modDllPath}'\n\t-{ex.Message}\n\t\t-{ex.StackTrace}"), ex);
            }

            return Instance != null;
        }

        public void LoadContent()
        {
            Content?.LoadContent(this);
        }

        internal void LoadConfig()
        {
            if (Instance == null)
            {
                throw new Exception("Error! Configurations can only be loaded into an already loaded mod!");
            }

            var configPath = ModRoot + "\\" + ConfigurationFile;
            if(File.Exists(configPath))
            {
                Instance.LoadConfigurationSettings(configPath);
            }
        }
        
        public Texture2D GetModTexture(string id)
        {
            return TextureRegistry.GetItem(this, id).Texture;
        }
    }
}
