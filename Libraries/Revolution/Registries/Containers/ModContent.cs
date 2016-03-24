using Revolution.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
                return (Textures != null && Textures.Any()) ||
                    (XNB != null && XNB.Any());
            }
        }


        public List<ModTexture> Textures { get; set; }
        public List<ModXnb> XNB { get; set; }

        public void LoadContent(ModInfo mod)
        {
            if (Textures != null)
            {
                foreach (var texture in Textures)
                {
                    texture.AbsoluteFilePath = $"{mod.ModRoot}\\{Constants.ModContentDirectory}\\{texture.File}";

                    if (!texture.Exists())
                    {
                        throw new Exception($"Missing Texture: {texture.AbsoluteFilePath}");
                    }

                    TextureRegistry.RegisterItem(mod, texture.Id, texture);
                }
            }
            
            if (XNB != null)
            {
                foreach (var file in XNB)
                {
                    file.AbsoluteFilePath = $"{mod.ModRoot}\\{Constants.ModContentDirectory}\\{file.File}";
                    file.OwningMod = mod;
                    if (!file.Exists())
                    {
                        throw new Exception($"Missing XNB: {file.AbsoluteFilePath}");
                    }
                    XnbRegistry.RegisterItem(file.File, file);
                }
            }
        }        
    }
}
