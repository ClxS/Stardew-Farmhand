namespace Farmhand.Content.Injectors.Items
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Items;
    using Farmhand.Logging;

    internal class BigCraftableInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool IsLoader => false;

        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\BigCraftablesInformation";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Log.Error("You shouldn't be here!");
            return default(T);
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