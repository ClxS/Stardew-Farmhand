using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Farmhand.Logging;

namespace Farmhand
{
    /// <summary>
    /// Contains useful extension methods used by the ModConfiguration class
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Writes a config to a json blob on the disk specified in the config's properties.
        /// </summary>
        public static void Save<T>(this T baseConfig) where T : ModConfiguration
        {
            if (string.IsNullOrEmpty(baseConfig?.ConfigLocation) || string.IsNullOrEmpty(baseConfig.ConfigDir))
            {
                Log.Error("A config attempted to save when it itself or it's location were null.");
                return;
            }

            var s = JsonConvert.SerializeObject(baseConfig, typeof(T), Formatting.Indented, new JsonSerializerSettings());

            if (!Directory.Exists(baseConfig.ConfigDir))
                Directory.CreateDirectory(baseConfig.ConfigDir);

            if (!File.Exists(baseConfig.ConfigLocation) || !File.ReadAllText(baseConfig.ConfigLocation).SequenceEqual(s))
                File.WriteAllText(baseConfig.ConfigLocation, s);
        }
    }
}