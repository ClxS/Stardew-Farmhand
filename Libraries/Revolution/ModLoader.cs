using Newtonsoft.Json;
using Revolution.Attributes;
using Revolution.Registries;
using Revolution.Registries.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Revolution.Events;
using Revolution.Logging;

namespace Revolution
{
    public static class ModLoader
    {
        internal static List<string> ModPaths = new List<string>
        {
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Mods"
        };

        internal static EventManager ModEventManager = new EventManager();
        
        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void LoadMods()
        {
            ApiEvents.OnModError += ApiEvents_OnModError;

            Log.Info("Loading Mods...");
            try
            {
                Log.Verbose("Loading Mod Manifests");
                LoadModManifests();
                Log.Verbose("Validating Mod Manifests");
                ValidateModManifests();
                Log.Verbose("Resolving Mod Dependencies");
                ResolveDependencies();
                Log.Verbose("Importing Mod DLLs, Settings, and Content");
                LoadFinalMods();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
            var numModsLoaded = ModRegistry.GetRegisteredItems().Count(n => n.ModState == ModState.Loaded);
            Log.Info($"{numModsLoaded} Mods Loaded!");
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
                Log.Error($"Exception thrown by unknown mod with assembly: {e.Assembly.FullName}\n\t{e.Exception.Message}");
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
                    if(mod.UniqueId.Contains("\\") || mod.HasContent && mod.Content.Textures != null && mod.Content.Textures.Any(n => n.Id.Contains("\\")))
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
                    if (mod.HasDll && mod.HasConfig)
                    {
                        mod.LoadConfig();
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
            var modInfos = registeredMods as ModInfo[] ?? registeredMods.ToArray();
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
            foreach (string modPath in ModPaths)
            {
                foreach (String perModPath in Directory.GetDirectories(modPath))
                {
                    var modJsonFiles = Directory.GetFiles(perModPath, "manifest.json");
                    foreach (var file in modJsonFiles)
                    {
                        using (StreamReader r = new StreamReader(file))
                        {
                            string json = r.ReadToEnd();
                            ModInfo modInfo = JsonConvert.DeserializeObject<ModInfo>(json);
                            
                            modInfo.ModRoot = perModPath;
                            ModRegistry.RegisterItem(modInfo.UniqueId ?? Guid.NewGuid().ToString(), modInfo);
                        }
                    }
                }
            }
        }
        
        public static void DeactivateMod(Mod mod, ModState state = ModState.Deactivated, Exception error = null)
        {
            DeactivateMod(mod.ModSettings);
        }

        public static void DeactivateMod(ModInfo mod, ModState state = ModState.Deactivated, Exception error = null)
        {
            DetachAssemblyDelegates(mod.ModAssembly);
            mod.ModState = state;
            mod.LastException = error;
        }

        public static void DetachAssemblyDelegates(Assembly assembly)
        {
            if (assembly != null)
            {
                ModEventManager.DetachDelegates(assembly);
            }
        }

        public static void ReactivateMod(Mod mod)
        {
            ReactivateMod(mod.ModSettings);
        }

        public static void ReactivateMod(ModInfo mod)
        {
            if (mod.ModAssembly != null)
            {
                ModEventManager.ReattachDelegates(mod.ModAssembly);
            }
        }
    }
}
