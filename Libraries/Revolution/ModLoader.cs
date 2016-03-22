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
           // try
            {
                Console.WriteLine("Loading Mod Manifests");
                LoadModManifests();
                Console.WriteLine("Resolving Mod Dependencies");
                ResolveDependencies();
                Console.WriteLine("Importing Mod DLLs, Settings, and Content");
                LoadFinalMods();
            }
            //catch (System.Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Console.WriteLine(ex.StackTrace);
            //}            
        }

        //[HookRedirect("StardewValley.TitleScreenMenu", ".ctor")]
        [Hook(HookType.Entry, "StardewValley.Menus.TitleMenu", ".ctor")]
        internal static void RegisterModUI()
        {
            Console.WriteLine("In Title Screen");
            //TitleScreenMenu @this = inst as TitleScreenMenu;
            //if (@this != null)
            {
                //@this.buttons = new List<ClickableComponent>();
                //@this.texture = Game1.temporaryContent.Load<Texture2D>(@"LooseSprites\TitleButtons");
                //@this.buttons.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + 4, yPositionOnScreen, 0x100, 0x9d), "by ConcernedApe"));
                //@this.buttons.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + 4, yPositionOnScreen + 0xb8, 0x100, 0x80), "New Game"));
                //@this.buttons.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + 4, yPositionOnScreen + 0x13d, 0x100, 0x80), "Load Game"));
                //@this.buttons.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + 4, yPositionOnScreen + 0x1c0, 0x100, 0x80), "Co-op"));
                //@this.buttons.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + 4, yPositionOnScreen + 0x243, 0x100, 0x80), "Leave Stardew Valley"));
            }
        }

        private static void LoadFinalMods()
        {
            var registeredMods = ModRegistry.GetRegisteredItems();
            Console.WriteLine("Mod Count: " + registeredMods.Count());
            foreach(var mod in registeredMods.Where(n => n.ModState == ModState.Unloaded))
            {
                Console.WriteLine("Loading mod {0} by {1}", mod.Name, mod.Author);      
                try
                {
                    if (mod.HasContent)
                    {
                        //TODO
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
                    Console.WriteLine("Error loading mod {0} by {1}\n\t-{2}", mod.Name, mod.Author, ex.Message);
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
                            var dependencyMatch = registeredMods.FirstOrDefault(n => n.UniqueId == dependency.UniqueModId);
                            if (dependencyMatch == null)
                            {
                                mod.ModState = ModState.MissingDependency;
                                stateChange = true;
                                Console.WriteLine("Failed to load {0} due to missing dependency: {1}}", mod.Name, dependency.UniqueModId);
                            }
                            else if (dependencyMatch.ModState == ModState.MissingDependency)
                            {
                                mod.ModState = ModState.MissingDependency;
                                stateChange = true;
                                Console.WriteLine("Failed to load {0} due to missing dependency missing dependency: {1}", mod.Name, dependency.UniqueModId);
                            }
                            else
                            {
                                Version dependencyVersion = dependencyMatch.Version;
                                if (dependencyVersion != null)
                                {
                                    if (dependency.MinimumVersion != null && dependency.MinimumVersion > dependencyVersion)
                                    {
                                        stateChange = true;
                                        Console.WriteLine("Failed to load {0} due to minimum version incompatibility with {1}: v.{2} < v.{3}",
                                            mod.Name, dependency.UniqueModId, dependencyMatch.Version.ToString(), dependency.MinimumVersion.ToString());
                                    }
                                    else if (dependency.MaximumVersion != null && dependency.MaximumVersion < dependencyVersion)
                                    {
                                        stateChange = true;
                                        Console.WriteLine("Failed to load {0} due to maximum version incompatibility with {1}: v.{2} > v.{3}",
                                            mod.Name, dependency.UniqueModId, dependencyMatch.Version.ToString(), dependency.MinimumVersion.ToString());
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

        public static void DeactivateMod(Mod mod)
        {
            //TODO Deactivate all events associated with mod
            //TODO Unload mod's domain
        }
    }
}
