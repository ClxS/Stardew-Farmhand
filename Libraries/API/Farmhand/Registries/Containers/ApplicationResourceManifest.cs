namespace Farmhand.Registries.Containers
{
    using System;
    using System.Collections.Generic;

    using Farmhand.Helpers;
    using Farmhand.Logging;

    internal class ApplicationResourceManifest
    {
        public static List<ApplicationResourceManifest> LoadedManifests { get; set; } =
            new List<ApplicationResourceManifest>();

        public ManifestContent Content { get; set; }

        public void LoadContent()
        {
            if (this.Content == null)
            {
                return;
            }

            Log.Verbose("Loading Content");
            if (this.Content.Textures != null)
            {
                foreach (var texture in this.Content.Textures)
                {
                    texture.Texture = ApplicationResourcesUtility.LoadTexture(texture.File);

                    if (texture.Texture == null)
                    {
                        throw new Exception($"Missing API Texture: {texture.File}");
                    }

                    Log.Verbose($"Registering new API texture: {texture.Id}");
                    TextureRegistry.RegisterItem(texture.Id, texture);
                }
            }

            if (this.Content.Maps != null)
            {
                foreach (var map in this.Content.Maps)
                {
                    map.Map = ApplicationResourcesUtility.LoadMap(map.File);

                    if (map.Map == null)
                    {
                        throw new Exception($"Missing API map: {map.AbsoluteFilePath}");
                    }

                    Log.Verbose($"Registering new API map: {map.Id}");
                    MapRegistry.RegisterItem(map.Id, map);
                }
            }

            if (this.Content.Xnb == null)
            {
                return;
            }

            foreach (var file in this.Content.Xnb)
            {
                if (file.IsXnb)
                {
                    throw new NotImplementedException();
                }

                file.OwningMod = null;

                if (!file.Exists(null))
                {
                    if (file.IsXnb)
                    {
                        throw new Exception($"Replacement File: {file.AbsoluteFilePath}");
                    }

                    if (file.IsTexture)
                    {
                        throw new Exception($"Replacement Texture: {file.Texture}");
                    }
                }

                Log.Verbose("Registering new API texture XNB override");
                XnbRegistry.RegisterItem(file.Original, file);
            }
        }
    }
}