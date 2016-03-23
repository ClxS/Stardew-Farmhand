using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries.Containers
{
    public class ModContent
    {
        public bool HasContent
        {
            get
            {
                return Textures.Any();
            }
        }

        public List<ModTexture> Textures { get; set; }

        public void LoadContent(ModInfo mod)
        {
            foreach(var texture in Textures)
            {
                texture.AbsoluteFilePath = $"{mod.ModRoot}\\{Constants.ModContentDirectory}\\{texture.File}";

                if (!texture.Exists())
                {
                    throw new Exception($"Missing Texture: {texture.AbsoluteFilePath}");
                }

                TextureRegistry.RegisterItem(mod, texture.Id, texture);
            }           
        }
        
    }
}
