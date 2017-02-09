namespace Farmhand.Content.Injectors.Other
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.API.Utilities;
    using Farmhand.Logging;
    using Farmhand.Registries;
    using Farmhand.Registries.Containers;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal class ModXnbTextureInjector : IContentInjector
    {
        private readonly Dictionary<string, Texture2D> cachedAlteredTextures = new Dictionary<string, Texture2D>();

        #region IContentInjector Members

        public bool HandlesAsset(Type type, string assetName)
        {
            var item = XnbRegistry.GetItem(assetName, null, true);
            return type == typeof(Texture2D)
                   && (item?.Any(
                           x => x.OwningMod?.ModState != null && x.IsTexture && x.OwningMod.ModState == ModState.Loaded)
                       ?? false);
        }

        public void Inject<T>(T originalTexture, string assetName, ref object output)
        {
            var items = XnbRegistry.GetItem(assetName, null, true).Where(x => x.IsTexture);
            var isDirty = XnbRegistry.IsDirty(assetName, null, true);
            var modXnbs = items as ModXnb[] ?? items.ToArray();
            var obj = originalTexture;

            string assetKey = $"{assetName}-\u2764-modified";
            if (this.cachedAlteredTextures.ContainsKey(assetKey) && !isDirty)
            {
                output = this.cachedAlteredTextures[assetKey];
            }
            else
            {
                foreach (var item in modXnbs)
                {
                    var modItem = TextureRegistry.GetItem(item.Texture, item.OwningMod);
                    var modTexture = modItem?.Texture;
                    if (item.Destination == null || modTexture == null)
                    {
                        continue;
                    }

                    var texture = TextureUtility.PatchTexture(
                        obj as Texture2D,
                        modTexture,
                        item.Source ?? new Rectangle(0, 0, modTexture.Width, modTexture.Height),
                        item.Destination);
                    output = texture;
                }

                this.cachedAlteredTextures[assetKey] = (Texture2D)output;
            }

            var outputMessage =
                modXnbs.Select(n => n.OwningMod.Name + " (" + n.Texture + ")").Aggregate((a, b) => a + ", " + b);

            Log.Verbose($"Using own asset replacement: {assetName} = " + outputMessage);
        }

        #endregion
    }
}