using System.IO;
using Newtonsoft.Json;

namespace Farmhand
{
    public class Program
    {
        public static FarmhandConfig Config { get; set; }
        
        public static void ReadConfig()
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
