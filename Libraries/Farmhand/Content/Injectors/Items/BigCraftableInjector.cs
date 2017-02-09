namespace Farmhand.Content.Injectors.Items
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Items;

    internal class BigCraftableInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\BigCraftablesInformation";
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var bigCraftables = obj as Dictionary<int, string>;
            if (bigCraftables == null)
            {
                throw new Exception($"Unexpected type for {assetName}");
            }

            foreach (var bigCraftable in BigCraftable.BigCraftables)
            {
                bigCraftables[bigCraftable.Id] = bigCraftable.ToString();
            }
        }

        #endregion
    }
}