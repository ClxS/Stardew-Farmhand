using System;
using System.Collections.Generic;

namespace Farmhand.Content.Injectors.NPCs
{
    internal class GiftTastesInjector : IContentInjector
    {
        public bool IsLoader => false;
        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\NPCGiftTastes";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var giftTastes = obj as Dictionary<string, string>;
            if (giftTastes == null)
                throw new Exception($"Unexpected type for {assetName}");

            foreach (var npc in API.NPCs.Npc.Npcs)
            {
                giftTastes[npc.Value.Item1.Name] = npc.Value.Item1.GiftTastes.ToString();
            }
        }
    }
}
