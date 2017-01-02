namespace Farmhand
{
    using System.IO;

    using Newtonsoft.Json;

    /// <summary>
    ///     Holds Farmhand configuration details.
    /// </summary>
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
                var cfg = JsonConvert.SerializeObject(Config, Formatting.Indented);
                File.WriteAllText("FarmhandConfig.json", cfg);
            }
        }
    }
}