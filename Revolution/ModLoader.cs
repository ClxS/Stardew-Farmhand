using Revolution.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Revolution
{
    public static class ModLoader
    {
        public static List<Mod> InstalledMods = new List<Mod>();

        internal static List<string> ModPaths = new List<string>() {
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Mods"
        };
        
        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void LoadMods()
        {            
            foreach (string ModPath in ModPaths)
            {
                try
                {                
                    foreach (String s in Directory.GetFiles(ModPath, "*.dll"))
                    {
                        try
                        {
                            Assembly mod = Assembly.LoadFrom(s); //to combat internet-downloaded DLLs

                            if (mod.GetTypes().Count(x => x.BaseType == typeof(Mod)) > 0)
                            {                           
                                Type tar = mod.GetTypes().First(x => x.BaseType == typeof(Mod));
                                Mod m = (Mod)mod.CreateInstance(tar.ToString());                            
                                m.Entry();
                                InstalledMods.Add(m);
                                Console.WriteLine("Loaded mod: {0}", m.Name);
                            }
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error loading mod: {0}", ex.Message);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("Error reading path: {0}", ex.Message);
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
