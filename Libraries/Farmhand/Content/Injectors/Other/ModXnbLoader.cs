namespace Farmhand.Content.Injectors.Other
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Farmhand.API;
    using Farmhand.API.Utilities;
    using Farmhand.Logging;
    using Farmhand.Registries;
    using Farmhand.Registries.Containers;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    internal class ModXnbLoader : IContentLoader
    {
        private readonly Dictionary<string, Texture2D> cachedAlteredTextures = new Dictionary<string, Texture2D>();

        #region IContentLoader Members

        public bool HandlesAsset(Type type, string assetName)
        {
            var item = XnbRegistry.GetItem(assetName, null, true);
            return item?.Any(x => x.OwningMod?.ModState != null && x.IsXnb && x.Replace && x.OwningMod.ModState == ModState.Loaded) ?? false;
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var output = default(T);

            var items = XnbRegistry.GetItem(assetName, null, true);
            try
            {
                var modXnbs = items as ModXnb[] ?? items.ToArray();
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
                var modContentManager = this.GetContentManagerForXnb(item);
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
            catch (Exception ex)
            {
                Log.Exception("Error reading own file", ex);
            }

            XnbRegistry.ClearDirtyFlag(assetName, null, true);
            return output;
        }

        #endregion

        private LocalizedContentManager GetContentManagerForXnb(ModXnb mod)
        {
            return Content.GetContentManagerForMod(mod.OwningMod);
        }
    }
}