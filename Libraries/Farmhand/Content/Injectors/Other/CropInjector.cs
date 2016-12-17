using System;
using System.Collections.Generic;

namespace Farmhand.Content.Injectors.Other
{
    class CropInjector : IContentInjector
    {
        public bool IsLoader => false;
        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\Crops";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var crops = obj as Dictionary<int, string>;
            if (crops == null)
                throw new Exception($"Unexpected type for {assetName}");

            foreach (var crop in API.Crops.Crop.Crops)
            {
                crops[crop.Value.Seed] = crop.Value.ToString();
            }
        }
    }
}
