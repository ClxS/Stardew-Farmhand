namespace Farmhand.Content.Injectors.NPCs
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.NPCs;

    internal class NpcDispositionsInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\NPCDispositions";
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var dispositions = obj as Dictionary<string, string>;
            if (dispositions == null)
            {
                throw new Exception($"Unexpected type for {assetName}");
            }

            foreach (var npc in Npc.Npcs)
            {
                dispositions[npc.Value.Item1.Name] = npc.Value.Item1.DispositionString;
            }
        }

        #endregion
    }
}