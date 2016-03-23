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

namespace Revolution
{
    public static class ModLoader
    {
        internal static List<string> ModPaths = new List<string>() {
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Mods"
        };
        
        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void LoadMods()
        {
            try
            {
                Console.WriteLine("Loading Mod Manifests");
                LoadModManifests();
                Console.WriteLine("Validating Mod Manifests");
                ValidateModManifests();
                Console.WriteLine("Resolving Mod Dependencies");
                ResolveDependencies();
                Console.WriteLine("Importing Mod DLLs, Settings, and Content");
                LoadFinalMods();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
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
                        Console.WriteLine($"Error - {mod.Name} by {mod.Author} manifest is invalid. Mod UniqueID cannot contain \"\\\"");
                        mod.ModState = ModState.InvalidManifest;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error validating mod {mod.Name} by {mod.Author}\n\t-{ex.Message}");
                }
            }
        }

        private static void LoadFinalMods()
        {
            var registeredMods = ModRegistry.GetRegisteredItems();
            Console.WriteLine("Mod Count: " + registeredMods.Count());
            foreach(var mod in registeredMods.Where(n => n.ModState == ModState.Unloaded))
            {
                Console.WriteLine($"Loading mod: {mod.Name} by {mod.Author}");      
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
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"Error loading mod {mod.Name} by {mod.Author}\n\t-{ex.Message}");
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
                                Console.WriteLine($"Failed to load {mod.Name} due to missing dependency: {dependency.UniqueId}");
                            }
                            else if (dependencyMatch.ModState == ModState.MissingDependency)
                            {
                                mod.ModState = ModState.MissingDependency;
                                dependency.DependencyState = DependencyState.ParentMissing;
                                stateChange = true;
                                Console.WriteLine($"Failed to load {mod.Name} due to missing dependency missing dependency: {dependency.UniqueId}");
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
                                        Console.WriteLine($"Failed to load {mod.Name} due to minimum version incompatibility with {dependency.UniqueId}: " +
                                            $"v.{dependencyMatch.Version.ToString()} < v.{dependency.MinimumVersion.ToString()}");
                                    }
                                    else if (dependency.MaximumVersion != null && dependency.MaximumVersion < dependencyVersion)
                                    {
                                        mod.ModState = ModState.MissingDependency;
                                        dependency.DependencyState = DependencyState.TooHighVersion;
                                        stateChange = true;
                                        Console.WriteLine($"Failed to load {mod.Name} due to maximum version incompatibility with {dependency.UniqueId}: " +
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
            //TODO Deactivate all events associated with mod
            //TODO Unload mod's domain
        }
    }
}
