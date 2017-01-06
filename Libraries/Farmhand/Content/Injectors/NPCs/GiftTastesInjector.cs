namespace Farmhand.Content.Injectors.NPCs
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.NPCs;
    using Farmhand.Logging;

    internal class GiftTastesInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool IsLoader => false;

        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\NPCGiftTastes";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var giftTastes = obj as Dictionary<string, string>;
            if (giftTastes == null)
            {
                throw new Exception($"Unexpected type for {assetName}");
            }

            foreach (var npc in Npc.Npcs)
            {
                giftTastes[npc.Value.Item1.Name] = npc.Value.Item1.GiftTastes.BuildTasteInformation();
            }
        }

        #endregion
    }
}