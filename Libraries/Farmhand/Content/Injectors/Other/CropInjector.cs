namespace Farmhand.Content.Injectors.Other
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Crops;

    internal class CropInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\Crops";
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var crops = obj as Dictionary<int, string>;
            if (crops == null)
            {
                throw new Exception($"Unexpected type for {assetName}");
            }

            foreach (var crop in Crop.Crops)
            {
                crops[crop.Value.Seed] = crop.Value.ToString();
            }
        }

        #endregion
    }
}