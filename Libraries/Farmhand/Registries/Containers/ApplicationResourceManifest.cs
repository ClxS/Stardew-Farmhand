using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.Helpers;

namespace Farmhand.Registries.Containers
{
    public class ApplicationResourceManifest
    {
        public static List<ApplicationResourceManifest> LoadedManifests { get; set; } = new List<ApplicationResourceManifest>();

        public ManifestContent Content { get; set; }

        public void LoadContent()
        {
            if (Content == null)
                return;

            Logging.Log.Verbose("Loading Content");
            if (Content.Textures != null)
            {
                foreach (var texture in Content.Textures)
                {
                    texture.Texture = ApplicationResourcesUtility.LoadTexture(texture.File);

                    if (texture.Texture == null)
                    {
                        throw new Exception($"Missing API Texture: {texture.File}");
                    }

                    Logging.Log.Verbose($"Registering new API texture: {texture.Id}");
                    TextureRegistry.RegisterItem(texture.Id, texture);
                }
            }

            if (Content.Maps != null)
            {
                foreach (var map in Content.Maps)
                {
                    map.Map = ApplicationResourcesUtility.LoadMap(map.File);

                    if (map.Map == null)
                    {
                        throw new Exception($"Missing API map: {map.AbsoluteFilePath}");
                    }

                    Logging.Log.Verbose($"Registering new API map: {map.Id}");
                    MapRegistry.RegisterItem(map.Id, map);
                }
            }

            if (Content.Xnb == null) return;

            foreach (var file in Content.Xnb)
            {
                if (file.IsXnb)
                {
                    throw new NotImplementedException();
                }
                file.OwningMod = null;

                if (!file.Exists(null))
                {
                    if (file.IsXnb)
                        throw new Exception($"Replacement File: {file.AbsoluteFilePath}");
                    if (file.IsTexture)
                        throw new Exception($"Replacement Texture: {file.Texture}");
                }
                Logging.Log.Verbose("Registering new API texture XNB override");
                XnbRegistry.RegisterItem(file.Original, file);
            }
        }
    }
}
