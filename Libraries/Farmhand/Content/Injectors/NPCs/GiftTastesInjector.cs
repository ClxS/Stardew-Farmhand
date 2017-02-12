namespace Farmhand.Content.Injectors.NPCs
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.NPCs;

    internal class GiftTastesInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\NPCGiftTastes";
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