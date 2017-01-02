namespace Farmhand.Content.Injectors.Other
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Farmhand.API.Utilities;
    using Farmhand.Logging;
    using Farmhand.Registries;
    using Farmhand.Registries.Containers;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    internal class ModXnbInjector : IContentInjector
    {
        private static List<LocalizedContentManager> modManagers;

        private readonly Dictionary<string, Texture2D> cachedAlteredTextures = new Dictionary<string, Texture2D>();

        #region IContentInjector Members

        public bool IsLoader => true;

        public bool IsInjector => false;

        public bool HandlesAsset(Type type, string assetName)
        {
            var item = XnbRegistry.GetItem(assetName, null, true);
            return item?.Any(x => x.OwningMod?.ModState != null && x.OwningMod.ModState == ModState.Loaded) ?? false;
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var output = default(T);

            var items = XnbRegistry.GetItem(assetName, null, true);
            var isDirty = XnbRegistry.IsDirty(assetName, null, true);
            try
            {
                var modXnbs = items as ModXnb[] ?? items.ToArray();
                if (modXnbs.Any(x => x.IsXnb))
                {
                    var item = modXnbs.First(x => x.IsXnb);

                    if (modXnbs.Length > 1)
                    {
                        var outputMessage =
                            modXnbs.Skip(1)
                                .Select(n => n.OwningMod.Name + " (" + n.Texture + ")")
                                .Aggregate((a, b) => a + ", " + b);
                        Log.Warning(
                            $"XNB Conflict on asset {assetName}. Using {item.OwningMod} ({item.File}) and ignoring: {outputMessage}");
                    }

                    var currentDirectory = Path.GetDirectoryName(item.AbsoluteFilePath);
                    var modContentManager = this.GetContentManagerForMod(contentManager, item);
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
                else if (modXnbs.Any(i => i.IsTexture) && typeof(T) == typeof(Texture2D))
                {
                    output = (T)this.LoadTexture(contentManager, assetName, modXnbs, isDirty);
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Error reading own file", ex);
            }

            XnbRegistry.ClearDirtyFlag(assetName, null, true);
            return output;
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void LoadModManagers(ContentManager contentManager)
        {
            if (modManagers != null)
            {
                return;
            }

            modManagers = new List<LocalizedContentManager>();
            foreach (var modPath in ModLoader.ModPaths)
            {
                modManagers.Add(contentManager.CreateContentManager(modPath));
            }
        }

        private LocalizedContentManager GetContentManagerForMod(ContentManager contentManager, ModXnb mod)
        {
            this.LoadModManagers(contentManager);
            return modManagers.FirstOrDefault(n => mod.OwningMod.ModDirectory.Contains(n.RootDirectory));
        }

        private object LoadTexture(
            ContentManager contentManager,
            string assetName,
            IEnumerable<ModXnb> items,
            bool isDirty)
        {
            var originalTexture = contentManager.LoadDirect<Texture2D>(assetName);
            var obj = originalTexture;

            string assetKey = $"{assetName}-\u2764-modified";
            var modXnbs = items as ModXnb[] ?? items.ToArray();
            if (this.cachedAlteredTextures.ContainsKey(assetKey) && !isDirty)
            {
                obj = this.cachedAlteredTextures[assetKey];
            }
            else
            {
                foreach (var item in modXnbs)
                {
                    var modItem = TextureRegistry.GetItem(item.Texture, item.OwningMod);
                    var modTexture = modItem?.Texture;
                    if (item.Destination != null)
                    {
                        if (modTexture != null)
                        {
                            var texture = TextureUtility.PatchTexture(
                                obj,
                                modTexture,
                                item.Source ?? new Rectangle(0, 0, modTexture.Width, modTexture.Height),
                                item.Destination);
                            obj = texture;
                        }
                    }
                }

                this.cachedAlteredTextures[assetKey] = obj;
            }

            var outputMessage =
                modXnbs.Select(n => n.OwningMod.Name + " (" + n.Texture + ")").Aggregate((a, b) => a + ", " + b);
            Log.Verbose($"Using own asset replacement: {assetName} = " + outputMessage);
            return obj;
        }
    }
}