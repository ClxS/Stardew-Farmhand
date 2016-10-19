using System;
using System.Collections.Generic;

namespace Farmhand.Content
{
    class BigCraftableInjector : IContentInjector
    {
        public bool IsLoader => false;
        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\BigCraftablesInformation";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var bigCraftables = obj as Dictionary<int, string>;
            if (bigCraftables == null)
                throw new Exception($"Unexpected type for {assetName}");

            foreach (var bigCraftable in Farmhand.API.Items.BigCraftable.BigCraftables)
            {
                bigCraftables[bigCraftable.Id] = bigCraftable.ToString();
            }
        }
    }
}
