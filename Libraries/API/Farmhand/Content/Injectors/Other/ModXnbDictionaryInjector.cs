namespace Farmhand.Content.Injectors.Other
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Farmhand.API;
    using Farmhand.Registries;
    using Farmhand.Registries.Containers;

    internal class ModXnbDictionaryInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool HandlesAsset(Type type, string asset)
        {
            var isRightType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
            var item = XnbRegistry.GetItem(asset, null, true);
            return isRightType
                   && (item?.Any(
                           x => x.OwningMod?.ModState != null && !x.Replace && x.OwningMod.ModState == ModState.Loaded)
                       ?? false);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var items = XnbRegistry.GetItem(assetName, null, true).ToArray();
            if (!items.Any(x => x.IsXnb))
            {
                return;
            }

            var outputDictionary = obj as IDictionary;
            if (outputDictionary == null)
            {
                throw new ArgumentException("Expected argument to implement IDictionary", nameof(obj));
            }

            foreach (var item in items)
            {
                var modItems = (IDictionary)this.LoadModXnb<T>(item);
                foreach (var key in modItems.Keys)
                {
                    outputDictionary[key] = modItems[key];
                }
            }
        }

        #endregion

        private T LoadModXnb<T>(ModXnb item)
        {
            var currentDirectory = Path.GetDirectoryName(item.AbsoluteFilePath);
            var modContentManager = (ContentManager)Content.GetContentManagerForMod(item.OwningMod);
            var relPath = modContentManager.RootDirectory + "\\";
            if (currentDirectory != null)
            {
                var relRootUri = new Uri(relPath, UriKind.Absolute);
                var fullPath = new Uri(currentDirectory, UriKind.Absolute);
                var relUri = relRootUri.MakeRelativeUri(fullPath) + "/" + item.File;
                return modContentManager.LoadDirect<T>(relUri);
            }

            return default(T);
        }
    }
}