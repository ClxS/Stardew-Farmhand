using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Revolution.Registries.Containers
{
    public enum ModState
    {
        Inactive,
        Unloaded,
        Loaded,
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
        public string ModDLL { get; set; }
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
        public bool HasDLL { get { return !string.IsNullOrWhiteSpace(ModDLL); } }
        [JsonIgnore]
        public bool HasConfig { get { return HasDLL && !string.IsNullOrWhiteSpace(ConfigurationFile); } }

        [JsonIgnore]
        public bool HasContent { get { return Content != null && Content.HasContent; } }

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
            if (Content != null)
            {
                Content.LoadContent(this);
            }
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
