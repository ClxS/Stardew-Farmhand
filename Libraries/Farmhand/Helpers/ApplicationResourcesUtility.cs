namespace Farmhand.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Farmhand.Attributes;
    using Farmhand.Registries.Containers;

    using Microsoft.Xna.Framework.Graphics;

    using Newtonsoft.Json;

    using StardewValley;

    using xTile;

    internal class ApplicationResourcesUtility
    {
        [Hook(HookType.Entry, "StardewValley.Game1", "Initialize")]
        public static void LoadInternalApiManifests()
        {
            var manifestRead =
                Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(n => n.EndsWith(".manifest.json"));
            foreach (var file in manifestRead)
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file))
                {
                    if (stream == null)
                    {
                        continue;
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        var manifestFile = reader.ReadToEnd();
                        var manifest = JsonConvert.DeserializeObject<ApplicationResourceManifest>(manifestFile);
                        manifest.LoadContent();

                        ApplicationResourceManifest.LoadedManifests.Add(manifest);
                    }
                }
            }
        }

        public static Texture2D LoadTexture(string textureFile)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(textureFile))
            {
                if (stream == null || Game1.graphics == null)
                {
                    return null;
                }

                var texture = Texture2D.FromStream(Game1.graphics.GraphicsDevice, stream);
                return texture;
            }
        }

        public static Map LoadMap(string mapFile)
        {
            throw new NotImplementedException();
        }
    }
}