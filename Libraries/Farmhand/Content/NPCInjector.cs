using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand.API.NPCs;

namespace Farmhand.Content
{
    public class NPCGiftTastesInjector : IContentInjector
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

            foreach (var npc in NPC.NPCs)
            {
                giftTastes[npc.Value.Name] = npc.Value.GiftTastes.ToString();
            }
        }
    }

    public class NPCDispositionsInjector : IContentInjector
    {
        public bool IsLoader => false;
        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\NPCDispositions";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var dispositions = obj as Dictionary<string, string>;
            if (dispositions == null)
                throw new Exception($"Unexpected type for {assetName}");
            
            foreach (var npc in NPC.NPCs)
                dispositions[npc.Value.Name] = npc.Value.DispositionString;
        }
    }

    public class NPCRainyDialogueInjector : IContentInjector
    {
        public bool IsLoader => false;
        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Characters\\Dialogue\\rainy";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var rainyDialogue = obj as Dictionary<string, string>;
            if (rainyDialogue == null)
                throw new Exception($"Unexpected type for {assetName}");

            foreach (var npc in NPC.NPCs)
            {
                rainyDialogue[npc.Value.Name] = npc.Value.Dialogues.GetRainyDialogue;
            }
        }
    }
}