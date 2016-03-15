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
        public static List<string> ModPaths = new List<string>() {
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Mods"
        };
        
        public static void LoadMods()
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
    }
}
