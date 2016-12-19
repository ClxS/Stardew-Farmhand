using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Farmhand.Logging;
using StardewValley;

namespace Farmhand.Extensibility
{
    public class ExtensibilityManager
    {
        internal static List<FarmhandExtension> Extensions { get; set; } = new List<FarmhandExtension>();

        internal static void TryLoadExtensions()
        {
            Log.Verbose("Loading Compatibility Layers");
            var appRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (appRoot == null)
                throw new NullReferenceException("appRoot was null");

            var extensions = Directory.GetFiles(Path.Combine(appRoot, Constants.ExtensionsDirectory), "*.dll");

            foreach (var extension in extensions)
            {
                try
                {
                    Log.Verbose($"Trying to load {extension}");
                    if (!File.Exists(extension))
                    {
                        Log.Error($"{extension} not present");
                        continue;
                    }

                    var assm = Assembly.LoadFrom(extension);
                    if (assm.GetTypes().Count(x => x.BaseType == typeof(FarmhandExtension)) <= 0) continue;

                    var type = assm.GetTypes().First(x => x.BaseType == typeof(FarmhandExtension));
                    var inst = (FarmhandExtension)assm.CreateInstance(type.ToString());
                    if (inst == null) continue;

                    inst.OwnAssembly = assm;


                    Extensions.Add(inst);
                    inst.RootDirectory = appRoot;
                    inst.Initialise();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    var test = ex.LoaderExceptions;
                    Log.Exception("Failed to load compatibility layer" + test[0].Message, ex);
                }
            }
        }

        public static void LoadMods(string modsPath)
        {
            foreach (var ext in Extensions)
            {
                ext.LoadMods(modsPath);
            }
        }

        public static void SetGameInstance(Game1 game1)
        {
            foreach (var extensions in Extensions)
            {
                extensions.GameInstance = game1;
            }
        }
    }
}
