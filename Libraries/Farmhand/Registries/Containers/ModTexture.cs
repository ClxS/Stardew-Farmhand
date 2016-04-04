using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;
using System.IO;

namespace Farmhand.Registries.Containers
{
    public class ModTexture
    {
        public string File { get; set; }
        public string Id { get; set; }

        [JsonIgnore]
        public string AbsoluteFilePath { get; set; }

        [JsonIgnore]
        private Texture2D _texture;

        [JsonIgnore]
        public Texture2D Texture {
            get
            {
                if(_texture == null && Exists())
                {
                    _texture = Texture2D.FromStream(Game1.graphics.GraphicsDevice, new FileStream(AbsoluteFilePath, FileMode.Open));
                }
                return _texture;
            }
        }

        public bool Exists()
        {
            return !string.IsNullOrEmpty(AbsoluteFilePath) && System.IO.File.Exists(AbsoluteFilePath);
        }
    }
}
