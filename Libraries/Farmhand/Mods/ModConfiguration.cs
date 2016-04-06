using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Farmhand.Logging;

namespace Farmhand
{
    /// <summary>
    /// This class provides an editable configuration file which mods can save data to. It is useful for things such as saving a Mod's options.
    /// </summary>
    public class ModConfiguration
    {
        /// <summary>
        /// The location of the mod file
        /// </summary>
        [JsonIgnore]
        public virtual string ConfigLocation { get; protected internal set; }

        /// <summary>
        /// The directory of the mod file
        /// </summary>
        [JsonIgnore]
        public virtual string ConfigDir => Path.GetDirectoryName(ConfigLocation);

        /// <summary>
        /// Creates an instance of a ModConfiguration when called.
        /// </summary>
        /// <typeparam name="T">The mod configuration type. This must derive from ModConfiguration</typeparam>
        /// <returns></returns>
        public virtual ModConfiguration Instance<T>() where T : ModConfiguration => Activator.CreateInstance<T>();
        
        /// <summary>
        /// Loads the config from the json blob on disk, updating and re-writing to the disk if needed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string configLocation) where T : ModConfiguration, new()
        {
            if (string.IsNullOrEmpty(configLocation))
            {
                Log.Error("A config tried to load without specifying a location on the disk.");
                return null;
            }

            T ret;

            if (!File.Exists(configLocation))
            {
                //no config exists, generate default values
                var c = new T {ConfigLocation = configLocation};
                c.GenerateDefaultConfig<T>();
                ret = c;
            }
            else
            {
                try
                {
                    //try to load the config from a json blob on disk
                    T c = JsonConvert.DeserializeObject<T>(File.ReadAllText(configLocation));
                    c.ConfigLocation = configLocation;
                    ret = c;
                }
                catch (Exception ex)
                {
                    Log.Exception($"Invalid JSON ({typeof(T).Name}): {configLocation}", ex);

                    var c = new T { ConfigLocation = configLocation };
                    c.GenerateDefaultConfig<T>();
                    ret = c;
                }
            }
            
            return ret;
        }
        
        /// <summary>
        /// This is intended to allow developers to populate their Mod Configurations with default data when creating a new one.
        /// </summary>
        public virtual T GenerateDefaultConfig<T>() where T : ModConfiguration
        {
            return null;
        }
    }
}
