using Newtonsoft.Json;
using Farmhand.Attributes;
using Farmhand.Registries;
using Farmhand.Registries.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Farmhand.Events;
using Farmhand.Helpers;
using Farmhand.Logging;
using StardewValley;

namespace Farmhand
{
    /// <summary>
    /// Handles loading mods
    /// </summary>
    public static class ModLoader
    {
        /// <summary>
        /// This value stores all the valid mod search directories
        /// </summary>
        public static List<string> ModPaths = new List<string>
        {
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Mods"
        };

        internal static List<ICompatibilityLayer> CompatibilityLayers { get; set; } = new List<ICompatibilityLayer>();

        internal static EventManager ModEventManager = new EventManager();

        /// <summary>
        /// States whether or not we're using SMAPI mods, so that certain things can be disabled if not. Defaults to false and is automatically set by
        /// the ModLoader when encountering a SMAPI mod
        /// </summary>
        public static bool UsingSmapiMods = false;
        
        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void LoadMods()
        {
            ApiEvents.OnModError += ApiEvents_OnModError;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
            
            Log.Info("Loading Mods...");
            try
            {
                TryLoadModCompatiblityLayers();
                Log.Verbose("Loading Mod Manifests");
                LoadModManifests();
                Log.Verbose("Validating Mod Manifests");
                ValidateModManifests();
                Log.Verbose("Resolving Mod Dependencies");
                ResolveDependencies();
                Log.Verbose("Importing Mod DLLs, Settings, and Content");
                LoadFinalMods();

                if (UsingSmapiMods)
                {
                    Log.Verbose("Using SMAPI - Attaching SMAPI events");
                    ModEventManager.AttachSmapiEvents();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
            var numModsLoaded = ModRegistry.GetRegisteredItems().Count(n => n.ModState == ModState.Loaded);
            Log.Info($"Stardew Farmhand v{Constants.Version}: {numModsLoaded} Mods Loaded!");

            Game1.version += $"Stardew Farmhand v{Constants.Version}: {numModsLoaded} mods loaded";

        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Stardew Valley"))
            {
                return Assembly.GetExecutingAssembly();
            }
            else if (args.Name.StartsWith("StardewModdingAPI"))
            {
                return Assembly.GetExecutingAssembly();
            }

            return null;
        }

        private static void TryLoadModCompatiblityLayers()
        {
            string[] layers = {"StardewModdingAPI.dll"};

            foreach (var layer in layers)
            {
                try
                {
                    if (!File.Exists(layer)) continue;

                    var assm = Assembly.LoadFrom(layer);
                    if (assm.GetTypes().Count(x => x.BaseType == typeof (ICompatibilityLayer)) <= 0) continue;
                    
                    var type = assm.GetTypes().First(x => x.BaseType == typeof(ICompatibilityLayer));
                    var inst = (ICompatibilityLayer)assm.CreateInstance(type.ToString());
                    if (inst == null) continue;

                    inst.OwnAssembly = assm;

                    CompatibilityLayers.Add(inst);
                    inst.AttachEvents(Game1.game1);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    var test = ex.LoaderExceptions;
                    Log.Exception("Failed to load compatibility layer" + test[0].Message, ex);
                }
            }
        }

        private static void ApiEvents_OnModError(object sender, Events.Arguments.EventArgsOnModError e)
        {
            var mod = ModRegistry.GetRegisteredItems().FirstOrDefault(n => n.ModAssembly == e.Assembly);
            if(mod != null)
            {
                Log.Exception($"Exception thrown by mod: {mod.Name} - {mod.Author}", e.Exception);
                DeactivateMod(mod, ModState.Errored, e.Exception);
            }
            else
            {
                Log.Exception($"Exception thrown by unknown mod with assembly: {e.Assembly.FullName}", e.Exception);
                DetachAssemblyDelegates(e.Assembly);
            }

        }

        private static void ValidateModManifests()
        {
            var registeredMods = ModRegistry.GetRegisteredItems();
            foreach (var mod in registeredMods.Where(n => n.ModState == ModState.Unloaded))
            {
                try
                {
                    mod.UniqueId = mod.UniqueId ?? Guid.NewGuid().ToString();
                    if (mod.UniqueId.Contains("\\") || mod.HasContent && mod.Content.Textures != null && mod.Content.Textures.Any(n => n.Id.Contains("\\")))
                    {
                        Log.Error($"Error - {mod.Name} by {mod.Author} manifest is invalid. UniqueIDs cannot contain \"\\\"");
                        mod.ModState = ModState.InvalidManifest;
                    }
                }
                catch(Exception ex)
                {
                    Log.Error($"Error validating mod {mod.Name} by {mod.Author}\n\t-{ex.Message}");
                }
            }
        }

        private static void LoadFinalMods()
        {
            var registeredMods = ModRegistry.GetRegisteredItems();
            foreach(var mod in registeredMods.Where(n => n.ModState == ModState.Unloaded))
            {
                Log.Verbose($"Loading mod: {mod.Name} by {mod.Author}");      
                try
                {
                    if (mod.HasContent)
                    {
                        mod.LoadContent();
                    }
                    if (mod.HasDll)
                    {
                        mod.LoadModDll();
                    }
                    mod.ModState = ModState.Loaded;
                    Log.Success($"Loaded Mod: {mod.Name} v{mod.Version} by {mod.Author}");              
                }
                catch (Exception ex)
                {
                    Log.Exception($"Error loading mod {mod.Name} by {mod.Author}", ex);
                    mod.ModState = ModState.Errored;
                    //TODO, well something broke. Do summut' 'bout it!
                }
            }
        }

        private static void ResolveDependencies()
        {
            var registeredMods = ModRegistry.GetRegisteredItems();

            //Loop to verify every dependent mod is available. 
            bool stateChange = false;
            var modInfos = registeredMods as ModManifest[] ?? registeredMods.ToArray();
            do 
            {
                foreach (var mod in modInfos)
                {
                    if (mod.ModState == ModState.MissingDependency || mod.Dependencies == null) continue;

                    foreach (var dependency in mod.Dependencies)
                    {
                        var dependencyMatch = modInfos.FirstOrDefault(n => n.UniqueId == dependency.UniqueId);
                        dependency.DependencyState = DependencyState.Ok;
                        if (dependencyMatch == null)
                        {
                            mod.ModState = ModState.MissingDependency;
                            dependency.DependencyState = DependencyState.Missing;
                            stateChange = true;
                            Log.Error($"Failed to load {mod.Name} due to missing dependency: {dependency.UniqueId}");
                        }
                        else if (dependencyMatch.ModState == ModState.MissingDependency)
                        {
                            mod.ModState = ModState.MissingDependency;
                            dependency.DependencyState = DependencyState.ParentMissing;
                            stateChange = true;
                            Log.Error($"Failed to load {mod.Name} due to missing dependency missing dependency: {dependency.UniqueId}");
                        }
                        else
                        {
                            var dependencyVersion = dependencyMatch.Version;
                            if (dependencyVersion == null) continue;

                            if (dependency.MinimumVersion != null && dependency.MinimumVersion > dependencyVersion)
                            {
                                mod.ModState = ModState.MissingDependency;
                                dependency.DependencyState = DependencyState.TooLowVersion;
                                stateChange = true;
                                Log.Error($"Failed to load {mod.Name} due to minimum version incompatibility with {dependency.UniqueId}: " +
                                          $"v.{dependencyMatch.Version} < v.{dependency.MinimumVersion}");
                            }
                            else if (dependency.MaximumVersion != null && dependency.MaximumVersion < dependencyVersion)
                            {
                                mod.ModState = ModState.MissingDependency;
                                dependency.DependencyState = DependencyState.TooHighVersion;
                                stateChange = true;
                                Log.Error($"Failed to load {mod.Name} due to maximum version incompatibility with {dependency.UniqueId}: " +
                                          $"v.{dependencyMatch.Version} > v.{dependency.MaximumVersion}");
                            }
                        }
                    }
                }
            } while (stateChange);
        }

        private static void LoadModManifests()
        {
            foreach (var modPath in ModPaths)
            {
                foreach (var perModPath in Directory.GetDirectories(modPath))
                {
                    var modJsonFiles = Directory.GetFiles(perModPath, "manifest.json");
                    foreach (var file in modJsonFiles)
                    {
                        using (var r = new StreamReader(file))
                        {
                            var json = r.ReadToEnd();
                            var modInfo = JsonConvert.DeserializeObject<ModManifest>(json, new Farmhand.Helpers.VersionConverter());
                            
                            modInfo.ModDirectory = perModPath;
                            ModRegistry.RegisterItem(modInfo.UniqueId ?? Guid.NewGuid().ToString(), modInfo);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Forcibly deactivates a mod by detaching it's event listeners.
        /// </summary>
        /// <param name="mod">The mod to deactive</param>
        /// <param name="state">The new state of this mod. Defaults to ModState.Deactivated</param>
        /// <param name="error">The exception encountered causing the mod to be unloaded. Defaults to null</param>
        public static void DeactivateMod(Mod mod, ModState state = ModState.Deactivated, Exception error = null)
        {
            DeactivateMod(mod.ModSettings);
        }

        /// <summary>
        /// Forcibly deactivates a mod by detaching it's event listeners.
        /// </summary>
        /// <param name="mod">The manifest of the mod to deactive</param>
        /// <param name="state">The new state of this mod. Defaults to ModState.Deactivated</param>
        /// <param name="error">The exception encountered causing the mod to be unloaded. Defaults to null</param>
        public static void DeactivateMod(ModManifest mod, ModState state = ModState.Deactivated, Exception error = null)
        {
            try
            {
                DetachAssemblyDelegates(mod.ModAssembly);
                mod.ModState = state;
                mod.LastException = error;
                Log.Success($"Successfully unloaded mod {mod.Name} by {mod.Author}");
            }
            catch (Exception ex)
            { 
                // Ignored
            }
        }

        /// <summary>
        /// Forcibly detaches event delegates associated with a particular assembly
        /// </summary>
        /// <param name="assembly">The assembly the detach</param>
        public static void DetachAssemblyDelegates(Assembly assembly)
        {
            if (assembly != null)
            {
                ModEventManager.DetachDelegates(assembly);
            }
        }

        /// <summary>
        /// Reattaches disabled delegates for previously disabled mods
        /// </summary>
        /// <param name="mod">The mod to reactivate</param>
        public static void ReactivateMod(Mod mod)
        {
            ReactivateMod(mod.ModSettings);
        }

        /// <summary>
        /// Reattaches disabled delegates for previously disabled mods
        /// </summary>
        /// <param name="mod">The manifest of the mod to reactivate</param>
        public static void ReactivateMod(ModManifest mod)
        {
            if (mod.ModAssembly != null)
            {
                ModEventManager.ReattachDelegates(mod.ModAssembly);
            }
        }
    }
}
