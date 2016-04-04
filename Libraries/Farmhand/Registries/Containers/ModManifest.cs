using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Farmhand.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Farmhand.Registries.Containers
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

        public System.Version Version { get; set; }

        public string Description { get; set; }
        public List<ModDependency> Dependencies { get; set; }

        public ModContent Content { get; set; }

        private string _configurationFile;
        public string ConfigurationFile
        {
            get { return $"{ModDirectory}\\{_configurationFile}"; }
            set { _configurationFile = value; }
        }

        #region SMAPI Compatibility

        public bool IsSmapiMod { get; set; }

        public string Authour
        {
            set { Author = value; }
        }

        public string EntryDll
        {
            set
            {
                var index = value.LastIndexOf(".dll", StringComparison.Ordinal);
                ModDll = index != -1 ? value.Remove(index) : value;
            }
        }
        public bool PerSaveConfigs { get; set; }

        #endregion

        #region Manifest Instance Data

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
        public object Instance { get; set; }
        //[JsonIgnore]
        //public StardewModdingAPI.Mod SmapiInstance { get; set; }
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
                if (ModAssembly.GetTypes().Count(x => x.BaseType == typeof(Farmhand.Mod)) > 0)
                {
                    var type = ModAssembly.GetTypes().First(x => x.BaseType == typeof(Farmhand.Mod));
                    Mod instance = (Mod)ModAssembly.CreateInstance(type.ToString());
                    if (instance != null)
                    {
                        instance.ModSettings = this;
                        instance.Entry();
                    }
                    Instance = instance;
                    Log.Verbose ($"Loaded mod dll: {Name}");
                }
                else
                {
                    var types = ModAssembly.GetTypes();
                    foreach (var layer in ModLoader.CompatibilityLayers)
                    {
                        if (!layer.ContainsOurModType(types)) continue;

                        Instance = layer.LoadMod(ModAssembly, types, this);
                        Log.Verbose($"Loaded mod dll: {Name}");
                        break;
                    }
                }
               
                if(Instance == null)
                {
                    throw new Exception("Invalid Mod DLL");
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Test", ex);
                //throw new Exception(string.Format($"Failed to load mod '{modDllPath}'\n\t-{ex.Message}\n\t\t-{ex.StackTrace}"), ex);
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

#endregion
    }
}
