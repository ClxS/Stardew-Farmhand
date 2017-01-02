namespace Farmhand.Registries.Containers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;

    using Farmhand.Helpers;
    using Farmhand.Logging;

    using Microsoft.Xna.Framework.Graphics;

    using Mono.Cecil;

    using Newtonsoft.Json;

    using xTile;

    /// <summary>
    ///     Contains registration information for a Farmhand mod.
    /// </summary>
    public class ModManifest : IModManifest
    {
        private static readonly SHA1CryptoServiceProvider Sha1 = new SHA1CryptoServiceProvider();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModManifest" /> class.
        /// </summary>
        public ModManifest()
        {
            this.ModState = ModState.Unloaded;
        }

        /// <summary>
        ///     Gets the mod DLL.
        /// </summary>
        [JsonProperty]
        public string ModDll { get; internal set; }

        /// <summary>
        ///     Gets the mod dependencies.
        /// </summary>
        [JsonProperty]
        public List<ModDependency> Dependencies { get; internal set; }

        /// <summary>
        ///     Gets the content for this mod.
        /// </summary>
        [JsonProperty]
        public ManifestContent Content { get; internal set; }

        #region IModManifest Members

        /// <summary>
        ///     Gets the unique ID for this mod.
        /// </summary>
        [JsonConverter(typeof(UniqueIdConverter))]
        [JsonProperty]
        public UniqueId<string> UniqueId { get; internal set; }

        /// <summary>
        ///     Gets whether this is a Farmhand mod.
        /// </summary>
        public bool IsFarmhandMod => true;

        /// <summary>
        ///     Gets the name of this mod.
        /// </summary>
        [JsonProperty]
        public string Name { get; internal set; }

        /// <summary>
        ///     Gets the author of this mod.
        /// </summary>
        [JsonProperty]
        public string Author { get; internal set; }

        /// <summary>
        ///     Gets the version of this mod.
        /// </summary>
        [JsonProperty]
        public Version Version { get; internal set; }

        /// <summary>
        ///     Gets the description of this mod.
        /// </summary>
        [JsonProperty]
        public string Description { get; internal set; }

        #endregion

        /// <summary>
        ///     Fires just prior to loading the mod.
        /// </summary>
        public static event EventHandler BeforeLoaded;

        /// <summary>
        ///     Fires just after loading the mod.
        /// </summary>
        public static event EventHandler AfterLoaded;

        internal void OnBeforeLoaded()
        {
            BeforeLoaded?.Invoke(this, EventArgs.Empty);
        }

        internal void OnAfterLoaded()
        {
            AfterLoaded?.Invoke(this, EventArgs.Empty);
        }

        #region Manifest Instance Data

        /// <summary>
        ///     Gets the configuration path for this mod.
        /// </summary>
        [JsonIgnore]
        public string ConfigurationPath => $"{this.ModDirectory}\\Config";

        /// <summary>
        ///     Gets the mod load state.
        /// </summary>
        [JsonIgnore]
        public ModState ModState { get; internal set; }

        /// <summary>
        ///     Gets or sets the last exception thrown by this mod.
        /// </summary>
        [JsonIgnore]
        public Exception LastException { get; set; }

        /// <summary>
        ///     Gets whether this mod has a DLL.
        /// </summary>
        [JsonIgnore]
        public bool HasDll => !string.IsNullOrWhiteSpace(this.ModDll);

        /// <summary>
        ///     Gets whether this mod has a config file.
        /// </summary>
        [JsonIgnore]
        public bool HasConfig => this.HasDll && !string.IsNullOrWhiteSpace(this.ConfigurationPath);

        /// <summary>
        ///     Gets whether this mod has defined content.
        /// </summary>
        [JsonIgnore]
        public bool HasContent => this.Content != null && this.Content.HasContent;

        /// <summary>
        ///     Gets the mod assembly.
        /// </summary>
        [JsonIgnore]
        public Assembly ModAssembly { get; internal set; }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        [JsonIgnore]
        public Mod Instance { get; internal set; }

        /// <summary>
        ///     Gets the mod directory.
        /// </summary>
        [JsonIgnore]
        public string ModDirectory { get; internal set; }

        internal bool LoadModDll()
        {
            if (this.Instance != null)
            {
                throw new Exception("Error! Mod has already been loaded!");
            }

            try
            {
                this.ModAssembly = Assembly.Load(this.GetDllBytes());
                if (this.ModAssembly.GetTypes().Count(x => x.BaseType == typeof(Mod)) > 0)
                {
                    var type = this.ModAssembly.GetTypes().First(x => x.BaseType == typeof(Mod));
                    var instance = (Mod)this.ModAssembly.CreateInstance(type.ToString());
                    if (instance != null)
                    {
                        instance.ModSettings = this;
                        if (instance.ConfigurationSettings != null)
                        {
                            var config = instance.ConfigurationSettings;
                            config.Manifest = this;
                            if (config.DoesConfigurationFileExist())
                            {
                                config.Load();
                            }
                            else
                            {
                                // If there isn't an already saved config file to load, we should create one.
                                config.Save();
                            }
                        }

                        instance.Entry();
                    }

                    this.Instance = instance;
                    Log.Verbose($"Loaded mod dll: {this.Name}");
                }

                if (this.Instance == null)
                {
                    throw new Exception("Invalid Mod DLL");
                }
            }
            catch (Exception ex)
            {
                var exception = ex as ReflectionTypeLoadException;
                if (exception != null)
                {
                    foreach (var e in exception.LoaderExceptions)
                    {
                        Log.Exception(
                            "LoaderExceptions entry: "
                            + $"{e.Message} ${e.Source} ${e.TargetSite} ${e.StackTrace} ${e.Data}",
                            e);
                    }
                }

                Log.Exception("Error loading mod DLL", ex);
            }

            return this.Instance != null;
        }

        internal byte[] GetDllBytes()
        {
            var modDllPath = $"{this.ModDirectory}\\{this.ModDll}";

            if (!modDllPath.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                modDllPath += ".dll";
            }

            // When a SMAPI mod is still referencing the vanilla assembly,
            // it doesn't see the Farmhand game. Everything will still be 
            // in the state for when the assembly would first be loaded.
            // This will fix the references so the mods will actually work.
            // For loading mods from other platforms:
            // There are also problems with XNA <=> Mono. Simply replacing
            // the assembly name isn't really possible in this case, since
            // all three XNA assemblies map to one Mono framework.
            // (Trust me, I tried. Suddenly it looks for the definition of
            // Vector2 inside the Farmhand assembly, or somewhere else
            // bizarre.)
            var bytes = File.ReadAllBytes(modDllPath);

            var asm = AssemblyDefinition.ReadAssembly(new MemoryStream(bytes, false));

            var shouldFix = false;
            var toRemove = new List<AssemblyNameReference>();
            foreach (var asmRef in asm.MainModule.AssemblyReferences)
            {
                // We only want to go to the effort of fixing everything if 
                // there is a reference that even needs fixing.
                // Also, the bad references will need to be removed.
                if (ReferenceHelper.MatchesPlatform(asmRef))
                {
                    continue;
                }

                shouldFix = true;
                toRemove.Add(asmRef);
            }

            foreach (var @ref in toRemove)
            {
                // However, we can't just simply remove the old references.
                // The indices mess up and really weird stuff happens (see
                // two comment blocks ago). Placing a dummy re-reference of
                // ourself SEEMS to fix that.
                var index = asm.MainModule.AssemblyReferences.IndexOf(@ref);
                asm.MainModule.AssemblyReferences.Remove(@ref);
                asm.MainModule.AssemblyReferences.Insert(index, ReferenceHelper.ThisRef);
            }

            if (shouldFix)
            {
                if (Program.Config.CachePorts)
                {
                    // We want to cache any 'fixed' assemblies so that we don't have to
                    // go through and fix everything again. However if the mod is updated
                    // or something, it will need to be fixed again.
                    var check = Convert.ToBase64String(Sha1.ComputeHash(bytes));
                    check = check.Replace("=", string.Empty).Replace('+', '-').Replace('/', '_');

                    // Fix for valid file name. = is just padding
                    var checkPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "StardewValley",
                        "cache",
                        this.ModDll + "-" + check + ".dll");
                    if (File.Exists(checkPath))
                    {
                        try
                        {
                            bytes = File.ReadAllBytes(checkPath);
                        }
                        catch (Exception ex)
                        {
                            Log.Exception($"Exception reading cached DLL {checkPath}", ex);
                            bytes = FixDll(asm, checkPath);
                        }
                    }
                    else
                    {
                        bytes = FixDll(asm, checkPath);
                    }
                }
                else
                {
                    bytes = FixDll(asm, null);
                }
            }

            return bytes;
        }

        private static byte[] FixDll(AssemblyDefinition asm, string cachePath)
        {
            ReferenceHelper.ReferenceResolver.DefinitionResolver.Fix(asm);

            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                asm.Write(stream);
                bytes = stream.GetBuffer();
            }

            if (cachePath != null)
            {
                try
                {
                    var dir = Path.GetDirectoryName(cachePath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    File.WriteAllBytes(cachePath, bytes);
                }
                catch (Exception ex)
                {
                    Log.Exception($"Exception caching fixed DLL {cachePath}", ex);
                }
            }

            return bytes;
        }

        internal void LoadContent()
        {
            if (this.Content == null)
            {
                return;
            }

            Log.Verbose("Loading Content");
            if (this.Content.Textures != null)
            {
                foreach (var texture in this.Content.Textures)
                {
                    texture.AbsoluteFilePath = $"{this.ModDirectory}\\{Constants.ModContentDirectory}\\{texture.File}";

                    if (!texture.Exists())
                    {
                        throw new Exception($"Missing Texture: {texture.AbsoluteFilePath}");
                    }

                    Log.Verbose($"Registering new texture: {texture.Id}");
                    TextureRegistry.RegisterItem(texture.Id, texture, this);
                }
            }

            if (this.Content.Maps != null)
            {
                foreach (var map in this.Content.Maps)
                {
                    map.AbsoluteFilePath = $"{this.ModDirectory}\\{Constants.ModContentDirectory}\\{map.File}";

                    if (!map.Exists())
                    {
                        throw new Exception($"Missing map: {map.AbsoluteFilePath}");
                    }

                    Log.Verbose($"Registering new map: {map.Id}");
                    MapRegistry.RegisterItem(map.Id, map, this);
                }
            }

            if (this.Content.Xnb == null)
            {
                return;
            }

            foreach (var file in this.Content.Xnb)
            {
                if (file.IsXnb)
                {
                    file.AbsoluteFilePath = $"{this.ModDirectory}\\{Constants.ModContentDirectory}\\{file.File}";
                }

                file.OwningMod = this;
                if (!file.Exists(this))
                {
                    if (file.IsXnb)
                    {
                        throw new Exception($"Replacement File: {file.AbsoluteFilePath}");
                    }

                    if (file.IsTexture)
                    {
                        throw new Exception($"Replacement Texture: {file.Texture}");
                    }
                }

                Log.Verbose("Registering new texture XNB override");
                XnbRegistry.RegisterItem(file.Original, file, this);
            }
        }

        /// <summary>
        ///     Gets a texture registered by this mod via it's manifest file
        /// </summary>
        /// <param name="id">
        ///     The id of the texture.
        /// </param>
        /// <returns>
        ///     The registered <see cref="Texture2D" />.
        /// </returns>
        public Texture2D GetTexture(string id)
        {
            return TextureRegistry.GetItem(id, this).Texture;
        }

        /// <summary>
        ///     Gets a map registered by this mod via it's manifest file
        /// </summary>
        /// <param name="id">
        ///     The id of the map.
        /// </param>
        /// <returns>
        ///     The registered <see cref="Map" />.
        /// </returns>
        public Map GetMap(string id)
        {
            return MapRegistry.GetItem(id, this).Map;
        }

        #endregion
    }
}