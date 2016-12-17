using System;
using System.Collections.Generic;

namespace Farmhand.Content.Injectors.NPCs
{
    internal class NpcDispositionsInjector : IContentInjector
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

            foreach (var npc in API.NPCs.Npc.Npcs)
            {
                dispositions[npc.Value.Item1.Name] = npc.Value.Item1.DispositionString;
            }
        }
    }
}
