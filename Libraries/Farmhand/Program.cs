using Farmhand.Attributes;


namespace Farmhand
{
    using System.IO;

    using Farmhand.Attributes;

    using Newtonsoft.Json;

    /// <summary>
    ///     Holds Farmhand configuration details.
    /// </summary>
    [HookExposeInternal("FarmhandPatcherSecondPass")]
    [HookExposeInternal("FarmhandUI")]
    [HookExposeInternal("FarmhandCharacter")]
    [HookExposeInternal("FarmhandGame")]
    public class Program
    {
        /// <summary>
        ///     Gets or sets the Farmhand configuration.
        /// </summary>
        public static FarmhandConfig Config { get; set; }

        internal static void ReadConfig()
        {
            try
            {
                var cfg = File.ReadAllText("FarmhandConfig.json");
                Config = JsonConvert.DeserializeObject<FarmhandConfig>(cfg);
            }
            catch (FileNotFoundException)
            {
                Config = new FarmhandConfig();
                SaveConfig();
            }
        }

        /// <summary>
        /// The save config.
        /// </summary>
        internal static void SaveConfig()
        {
            var cfg = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText("FarmhandConfig.json", cfg);
        }
    }
}