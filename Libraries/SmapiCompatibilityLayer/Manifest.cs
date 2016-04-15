using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StardewModdingAPI
{
    public class Manifest : Config
    {
        /// <summary>
        ///     The name of your mod.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     The name of the mod's authour.
        /// </summary>
        public virtual string Authour { get; set; }

        /// <summary>
        ///     The version of the mod.
        /// </summary>
        public virtual Version Version { get; set; }

        /// <summary>
        ///     A description of the mod.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        ///     The unique ID of the mod. It doesn't *need* to be anything.
        /// </summary>
        public virtual string UniqueID { get; set; }

        /// <summary>
        ///     Whether or not the mod uses per-save-config files.
        /// </summary>
        public virtual bool PerSaveConfigs { get; set; }

        /// <summary>
        ///     The name of the DLL in the directory that has the Entry() method.
        /// </summary>
        public virtual string EntryDll { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            Name = "";
            Authour = "";
            Version = new Version(0, 0, 0, "");
            Description = "";
            UniqueID = Guid.NewGuid().ToString();
            PerSaveConfigs = false;
            EntryDll = "";
            return this as T;
        }

        public override T LoadConfig<T>()
        {
            if (File.Exists(ConfigLocation))
            {
                try
                {
                    Manifest m = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(ConfigLocation));
                }
                catch
                {
                    //Invalid json blob. Try to remove version?
                    try
                    {
                        JObject j = JObject.Parse(File.ReadAllText(ConfigLocation));
                        if (!j.GetValue("Version").Contains("{"))
                        {
                            Log.AsyncC("INVALID JSON VERSION. TRYING TO REMOVE SO A NEW CAN BE AUTO-GENERATED");
                            j.Remove("Version");
                            File.WriteAllText(ConfigLocation, j.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }

            return base.LoadConfig<T>();
        }
    }
}