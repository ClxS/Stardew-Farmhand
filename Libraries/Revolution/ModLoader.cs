using Newtonsoft.Json;
using Revolution.Attributes;
using Revolution.Registries;
using Revolution.Registries.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Revolution.Events;

namespace Revolution
{
    public static class ModLoader
    {
        internal static List<string> ModPaths = new List<string>() {
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Mods"
        };

        internal static EventManager ModEventManager = new EventManager();
        
        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void LoadMods()
        {
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
            catch (System.Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
            var numModsLoaded = ModRegistry.GetRegisteredItems().Count(n => n.ModState == ModState.Loaded);
            Log.Info($"{numModsLoaded} Mods Loaded!");
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
                    if (mod.HasDLL)
                    {
                        mod.LoadModDLL();                        
                    }
                    if (mod.HasDLL && mod.HasConfig)
                    {
                        mod.LoadConfig();
                    }
                    mod.ModState = ModState.Loaded;
                    Log.Success($"Loaded Mod: {mod.Name} v{mod.Version} by {mod.Author}");              
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Error loading mod {mod.Name} by {mod.Author}\n\t-{ex.Message}");
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
            do 
            {
                foreach (var mod in registeredMods)
                {
                    if (mod.ModState != ModState.MissingDependency && mod.Dependencies != null)
                    {
                        foreach (var dependency in mod.Dependencies)
                        {
                            var dependencyMatch = registeredMods.FirstOrDefault(n => n.UniqueId == dependency.UniqueId);
                            dependency.DependencyState = DependencyState.OK;
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
                                Version dependencyVersion = dependencyMatch.Version;
                                if (dependencyVersion != null)
                                {
                                    if (dependency.MinimumVersion != null && dependency.MinimumVersion > dependencyVersion)
                                    {
                                        mod.ModState = ModState.MissingDependency;
                                        dependency.DependencyState = DependencyState.TooLowVersion;
                                        stateChange = true;
                                        Log.Error($"Failed to load {mod.Name} due to minimum version incompatibility with {dependency.UniqueId}: " +
                                            $"v.{dependencyMatch.Version.ToString()} < v.{dependency.MinimumVersion.ToString()}");
                                    }
                                    else if (dependency.MaximumVersion != null && dependency.MaximumVersion < dependencyVersion)
                                    {
                                        mod.ModState = ModState.MissingDependency;
                                        dependency.DependencyState = DependencyState.TooHighVersion;
                                        stateChange = true;
                                        Log.Error($"Failed to load {mod.Name} due to maximum version incompatibility with {dependency.UniqueId}: " +
                                            $"v.{dependencyMatch.Version.ToString()} > v.{dependency.MinimumVersion.ToString()}");
                                    }
                                }
                            }
                        }
                    }
                }
            } while (stateChange);
        }

        private static void LoadModManifests()
        {
            foreach (string ModPath in ModPaths)
            {
                foreach (String modPath in Directory.GetDirectories(ModPath))
                {
                    var modJsonFiles = Directory.GetFiles(modPath, "manifest.json");
                    foreach (var file in modJsonFiles)
                    {
                        using (StreamReader r = new StreamReader(file))
                        {
                            string json = r.ReadToEnd();
                            ModInfo modInfo = JsonConvert.DeserializeObject<ModInfo>(json);
                            
                            modInfo.ModRoot = modPath;
                            ModRegistry.RegisterItem(modInfo.UniqueId ?? Guid.NewGuid().ToString(), modInfo);
                        }
                    }
                }
            }
        }

        private static void ValidateModInfo(ModInfo modInfo)
        {
            throw new NotImplementedException();
        }

        public static void DeactivateMod(Mod mod)
        {
            DeactivateMod(mod.ModSettings);
        }

        public static void DeactivateMod(ModInfo mod)
        {
            if(mod.ModAssembly != null)
            {
                ModEventManager.DetachDelegates(mod.ModAssembly);
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
