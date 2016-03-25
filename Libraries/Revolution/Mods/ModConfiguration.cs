using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Revolution.Logging;

namespace Revolution
{
    public class ModConfiguration
    {
        [JsonIgnore]
        public virtual string ConfigLocation { get; protected internal set; }

        [JsonIgnore]
        public virtual string ConfigDir => Path.GetDirectoryName(ConfigLocation);

        public virtual ModConfiguration Instance<T>() where T : ModConfiguration => Activator.CreateInstance<T>();
        
        /// <summary>
        /// Loads the config from the json blob on disk, updating and re-writing to the disk if needed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadConfig<T>(string configLocation) where T : ModConfiguration, new()
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

                    //update the config with default values if needed
                    ret = c.UpdateConfig<T>();
                }
                catch (Exception ex)
                {
                    Log.Exception($"Invalid JSON ({typeof(T).Name}): {configLocation}", ex);

                    var c = new T { ConfigLocation = configLocation };
                    c.GenerateDefaultConfig<T>();
                    ret = c;
                }
            }

            ret.WriteConfig();
            return ret;
        }

        /// <summary>
        /// MUST be implemented in inheriting class!
        /// </summary>
        public virtual T GenerateDefaultConfig<T>() where T : ModConfiguration
        {
            return null;
        }

        /// <summary>
        /// Merges a default-value config with the user-config on disk.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T UpdateConfig<T>() where T : ModConfiguration
        {
            try
            {
                //default config
                var b = JObject.FromObject(Instance<T>().GenerateDefaultConfig<T>());

                //user config
                var u = JObject.FromObject(this);

                //overwrite default values with user values
                b.Merge(u, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace });

                //cast json object to config
                var c = b.ToObject<T>();

                //re-write the location on disk to the object
                c.ConfigLocation = ConfigLocation;

                return c;
            }
            catch (Exception ex)
            {
                Log.Error("An error occured when updating a config: " + ex);
                return this as T;
            }
        }
    }
}
