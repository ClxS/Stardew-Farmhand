namespace Farmhand.Content.Injectors.Effects
{
    using System;
    using System.IO;

    using Farmhand.Logging;

    using Microsoft.Xna.Framework.Graphics;

    internal class EffectLoader : IContentInjector
    {
        #region IContentInjector Members

        public bool IsLoader => true;

        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string assetName)
        {
            return type == typeof(Effect);
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
#if WINDOWS
            var platformExtension = ".Xna";
            var otherExtension = ".Mono";
#else
            var platformExtension = ".Mono";
            var otherExtension = ".Xna";
#endif
            var fullPath = Path.Combine(contentManager.RootDirectory, assetName);
            var platformFullPath = fullPath + platformExtension + ".xnb";
            var otherPlatformFullPath = fullPath + otherExtension + ".xnb";
            fullPath += ".xnb";
            var platformAssetName = assetName + platformExtension;
            var otherPlatformAssetName = assetName + otherExtension;

            if (!File.Exists(platformFullPath))
            {
                throw new Exception(
                    $"Located asset ({assetName}) but could not locate the "
                    + $"platform specific version ({platformAssetName}). You must provide both "
                    + $"{platformAssetName} and {otherPlatformAssetName} instead of {assetName}.");
            }

            if (!File.Exists(otherPlatformFullPath))
            {
                throw new Exception(
                    $"Located asset ({assetName}) but could not locate the "
                    + $"platform specific version ({otherPlatformAssetName}). You must provide both "
                    + $"{platformAssetName} and {otherPlatformAssetName} instead of {assetName}.");
            }

            return contentManager.LoadDirect<T>(platformAssetName);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            Log.Error("You shouldn't be here!");
        }

        #endregion
    }
}