using System;
using System.Collections.Generic;
using System.Linq;

namespace Revolution.Registries.Containers
{
    public class ModContent
    {
        public bool HasContent => (Textures != null && Textures.Any()) ||
                                  (Xnb != null && Xnb.Any());


        public List<ModTexture> Textures { get; set; }
        public List<ModXnb> Xnb { get; set; }

        public void LoadContent(ModManifest mod)
        {
            if (Textures != null)
            {
                foreach (var texture in Textures)
                {
                    texture.AbsoluteFilePath = $"{mod.ModDirectory}\\{Constants.ModContentDirectory}\\{texture.File}";

                    if (!texture.Exists())
                    {
                        throw new Exception($"Missing Texture: {texture.AbsoluteFilePath}");
                    }

                    TextureRegistry.RegisterItem(mod, texture.Id, texture);
                }
            }

            if (Xnb == null) return;

            foreach (var file in Xnb)
            {
                file.AbsoluteFilePath = $"{mod.ModDirectory}\\{Constants.ModContentDirectory}\\{file.File}";
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
