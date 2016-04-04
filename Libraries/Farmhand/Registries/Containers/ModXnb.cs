using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Farmhand.Registries.Containers
{
    public class TextureRect
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"X:{X}, Y:{Y}, H:{Height}, W{Width}";
        }

        public static implicit operator Rectangle(TextureRect rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
    public class ModXnb
    {
        public string Original { get; set; }
        public string File { get; set; }
        public string Texture { get; set; }

        public TextureRect Source { get; set; }
        public TextureRect Destination { get; set; }

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
