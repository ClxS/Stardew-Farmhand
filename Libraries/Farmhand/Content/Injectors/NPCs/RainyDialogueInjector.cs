using System;
using System.Collections.Generic;

namespace Farmhand.Content.Injectors.NPCs
{
    internal class RainyDialogueInjector : IContentInjector
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

            foreach (var npc in API.NPCs.Npc.Npcs)
            {
                rainyDialogue[npc.Value.Item1.Name] = npc.Value.Item1.Dialogues.GetRainyDialogue;
            }
        }
    }
}
