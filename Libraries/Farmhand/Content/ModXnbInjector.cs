using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Farmhand.API.Utilities;
using Farmhand.Logging;
using Farmhand.Registries;
using Farmhand.Registries.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Farmhand.Content
{
    internal class ModXnbInjector : IContentInjector
    {
        public bool IsLoader => true;
        public bool IsInjector => false;

        private static List<Microsoft.Xna.Framework.Content.ContentManager> _modManagers;
        private readonly Dictionary<string, Texture2D> _cachedAlteredTextures = new Dictionary<string, Texture2D>();

        public bool HandlesAsset(Type type, string assetName)
        {
            var item = XnbRegistry.GetItem(assetName, null, true);
            return item?.Any(x => x.OwningMod?.ModState != null && x.OwningMod.ModState == ModState.Loaded) ?? false;
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var output = default(T);

            var items = XnbRegistry.GetItem(assetName, null, true);
            try
            {
                if (items.Any(x => x.IsXnb))
                {
                    var item = items.First(x => x.IsXnb);
                    var currentDirectory = Path.GetDirectoryName(item.AbsoluteFilePath);
                    var modContentManager = GetContentManagerForMod(contentManager, item);
                    var relPath = modContentManager.RootDirectory + "\\";
                    if (currentDirectory != null)
                    {
                        var relRootUri = new Uri(relPath, UriKind.Absolute);
                        var fullPath = new Uri(currentDirectory, UriKind.Absolute);
                        var relUri = relRootUri.MakeRelativeUri(fullPath) + "/" + item.File;

                        Log.Verbose($"Using own asset replacement: {assetName} = {relUri}");
                        output = modContentManager.Load<T>(relUri);
                    }
                }
                else if (items.Any(i => i.IsTexture) && typeof(T) == typeof(Texture2D))
                {
                    output = (T)(LoadTexture(contentManager, assetName, items));
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Error reading own file", ex);
            }
            return output;
        }

        private void LoadModManagers(ContentManager contentManager)
        {
            if (_modManagers != null) return;

            _modManagers = new List<Microsoft.Xna.Framework.Content.ContentManager>();
            foreach (var modPath in ModLoader.ModPaths)
            {
                _modManagers.Add(new Microsoft.Xna.Framework.Content.ContentManager(contentManager.ServiceProvider, modPath));
            }
        }

        private Microsoft.Xna.Framework.Content.ContentManager GetContentManagerForMod(ContentManager contentManager, ModXnb mod)
        {
            LoadModManagers(contentManager);
            return _modManagers.FirstOrDefault(n => mod.OwningMod.ModDirectory.Contains(n.RootDirectory));
        }

        private object LoadTexture(ContentManager contentManager, string assetName, IEnumerable<ModXnb> items)
        {
            var originalTexture = contentManager.LoadDirect<Texture2D>(assetName);
            var obj = originalTexture;

            string assetKey = $"{assetName}-\u2764-modified";
            if (_cachedAlteredTextures.ContainsKey(assetKey))
            {
                obj = _cachedAlteredTextures[assetKey];
            }
            else
            {
                foreach (var item in items)
                {
                    var modItem = TextureRegistry.GetItem(item.Texture, item.OwningMod);
                    var modTexture = modItem?.Texture;
                    if (item.Destination != null)
                    {
                        var texture = TextureUtility.PatchTexture(obj, modTexture, item.Source ?? new Rectangle(0, 0, modTexture.Width, modTexture.Height), item.Destination);
                        obj = texture;
                    }
                }
                _cachedAlteredTextures[assetKey] = obj;
            }
            var outputMessage = items.Select(n => n.OwningMod.Name + " (" + n.Texture + ")").Aggregate((a, b) => a + ", " + b);
            Log.Verbose($"Using own asset replacement: {assetName} = " + outputMessage);
            return obj;
        }

        public void Inject<T>(T obj, string assetName)
        {
            throw new NotImplementedException();
        }
    }
}
