using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Revolution.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Revolution.Registries.Containers
{
    public class ModManifest
    {
        public ModManifest()
        {
            ModState = ModState.Unloaded;
        }

        public string UniqueId { get; set; }
        public string ModDll { get; set; }
        public string Name { get; set; }        
        public string Author { get; set; }
        public Version Version { get; set; }
        public string Description { get; set; }
        public List<ModDependency> Dependencies { get; set; }
        public ModContent Content { get; set; }
        private string _configurationFile;
        public string ConfigurationFile
        {
            get { return $"{ModDirectory}\\{_configurationFile}"; }
            set { _configurationFile = value; }
        }

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
        public string ModDirectory { get; set; }

        internal bool LoadModDll()
        {
            if (Instance != null)
            {
                throw new Exception("Error! Mod has already been loaded!");
            }

            var modDllPath = $"{ModDirectory}\\{ModDll}";

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

        internal void LoadContent()
        {
            Content?.LoadContent(this);
        }
        
        public Texture2D GetModTexture(string id)
        {
            return TextureRegistry.GetItem(this, id).Texture;
        }
    }
}
