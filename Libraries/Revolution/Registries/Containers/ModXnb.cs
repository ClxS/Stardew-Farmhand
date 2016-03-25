using Newtonsoft.Json;

namespace Revolution.Registries.Containers
{
    public class ModXnb
    {
        public string File { get; set; }

        [JsonIgnore]
        public ModInfo OwningMod { get; set; }

        [JsonIgnore]
        public string AbsoluteFilePath { get; set; }

        public bool Exists()
        {
            return !string.IsNullOrEmpty(AbsoluteFilePath) && System.IO.File.Exists(AbsoluteFilePath + ".xnb");
        }
    }
}
