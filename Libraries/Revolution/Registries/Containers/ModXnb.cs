using Newtonsoft.Json;

namespace Revolution.Registries.Containers
{
    public class ModXnb
    {
        public string Original { get; set; }
        public string File { get; set; }
        public string Texture { get; set; }

        [JsonIgnore]
        public ModManifest OwningMod { get; set; }

        [JsonIgnore]
        public string AbsoluteFilePath { get; set; }

        [JsonIgnore]
        public bool IsTexture => !string.IsNullOrEmpty(Texture);

        [JsonIgnore]
        public bool IsXnb => !string.IsNullOrEmpty(File);
        

        public bool Exists(ModManifest mod)
        {
            if (IsXnb)
            {
                return System.IO.File.Exists(AbsoluteFilePath + ".xnb");
            }
            if(IsTexture)
            {
                return TextureRegistry.GetItem(mod, Texture) != null;
            }
            return false;
        }
    }
}
