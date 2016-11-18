using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Farmhand.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Farmhand.Helpers;
using xTile;

namespace Farmhand.Registries.Containers
{
    public class ModManifest
    {
        public ModManifest()
        {
            ModState = ModState.Unloaded;
        }

        public static event EventHandler BeforeLoaded;
        public static event EventHandler AfterLoaded;

        [JsonConverter(typeof(UniqueIdConverter))]
        public UniqueId<string> UniqueId { get; set; }

        public string ModDll { get; set; }

        public string Name { get; set; }    
            
        public string Author { get; set; }

        public System.Version Version { get; set; }

        public string Description { get; set; }
        public List<ModDependency> Dependencies { get; set; }

        public ManifestContent Content { get; set; }

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
                // When a SMAPI mod is still referencing the vanilla assembly,
                // it doesn't see the Farmhand game. Everything will still be 
                // in the state for when the assembly would first be loaded.
                // This will fix the references so the mods will actually work.
                Mono.Cecil.AssemblyDefinition asm = Mono.Cecil.AssemblyDefinition.ReadAssembly(modDllPath);
                bool vanillaRef = false;
                foreach (var asmRef in asm.MainModule.AssemblyReferences)
                {
                    if (asmRef.Name.Contains("Stardew Valley"))
                    {
                        asmRef.Name = asmRef.Name.Replace("Stardew Valley", "Stardew Farmhand");
                        vanillaRef = true;
                    }
                    // TODO: I heard something about the vanilla assembly being 
                    // StardewValley on Mac/Linux? Is it "StardewFarmhand" or 
                    // "Stardew Farmhand" on those platforms?
                }

                byte[] bytes = System.IO.File.ReadAllBytes(modDllPath);
                if ( vanillaRef ) // No need to rewrite the assembly if we didn't change anything
                {
                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                    {
                        asm.Write(stream);
                        bytes = stream.GetBuffer();
                    }
                }

                ModAssembly = Assembly.Load(bytes);
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
                Log.Exception("Error loading mod DLL", ex);
                //throw new Exception(string.Format($"Failed to load mod '{modDllPath}'\n\t-{ex.Message}\n\t\t-{ex.StackTrace}"), ex);
            }

            return Instance != null;
        }

        internal void LoadContent()
        {
            if (Content == null)
                return;

            Logging.Log.Verbose("Loading Content");
            if (Content.Textures != null)
            {
                foreach (var texture in Content.Textures)
                {
                    texture.AbsoluteFilePath = $"{ModDirectory}\\{Constants.ModContentDirectory}\\{texture.File}";

                    if (!texture.Exists())
                    {
                        throw new Exception($"Missing Texture: {texture.AbsoluteFilePath}");
                    }

                    Logging.Log.Verbose($"Registering new texture: {texture.Id}");
                    TextureRegistry.RegisterItem(texture.Id, texture, this);
                }
            }

            if (Content.Maps != null)
            {
                foreach (var map in Content.Maps)
                {
                    map.AbsoluteFilePath = $"{ModDirectory}\\{Constants.ModContentDirectory}\\{map.File}";

                    if (!map.Exists())
                    {
                        throw new Exception($"Missing map: {map.AbsoluteFilePath}");
                    }

                    Logging.Log.Verbose($"Registering new map: {map.Id}");
                    MapRegistry.RegisterItem(map.Id, map, this);
                }
            }

            if (Content.Xnb == null) return;

            foreach (var file in Content.Xnb)
            {
                if (file.IsXnb)
                {
                    file.AbsoluteFilePath = $"{ModDirectory}\\{Constants.ModContentDirectory}\\{file.File}";
                }
                file.OwningMod = this;
                if (!file.Exists(this))
                {
                    if (file.IsXnb)
                        throw new Exception($"Replacement File: {file.AbsoluteFilePath}");
                    if (file.IsTexture)
                        throw new Exception($"Replacement Texture: {file.Texture}");
                }
                Logging.Log.Verbose("Registering new texture XNB override");
                XnbRegistry.RegisterItem(file.Original, file, this);
            }
        }
        
        public Texture2D GetTexture(string id)
        {
            return TextureRegistry.GetItem(id, this).Texture;
        }

        public Map GetMap(string id)
        {
            return MapRegistry.GetItem(id, this).Map;
        }

        #endregion

        public void OnBeforeLoaded()
        {
            BeforeLoaded?.Invoke(this, EventArgs.Empty);
        }

        public void OnAfterLoaded()
        {
            AfterLoaded?.Invoke(this, EventArgs.Empty);
        }
    }
}
