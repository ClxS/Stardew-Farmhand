using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Content
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

        public void Inject<T>(T obj, string assetName)
        {
            var crops = obj as Dictionary<int, string>;
            if (crops == null)
                throw new Exception($"Unexpected type for {assetName}");

            foreach (var crop in Farmhand.API.Crops.Crop.Crops)
            {
                crops[crop.Value.Seed] = crop.Value.ToString();
            }
        }
    }
}
