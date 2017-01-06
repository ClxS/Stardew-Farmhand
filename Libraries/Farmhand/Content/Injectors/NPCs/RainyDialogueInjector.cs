namespace Farmhand.Content.Injectors.NPCs
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.NPCs;
    using Farmhand.Logging;

    internal class RainyDialogueInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool IsLoader => false;

        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Characters\\Dialogue\\rainy";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var rainyDialogue = obj as Dictionary<string, string>;
            if (rainyDialogue == null)
            {
                throw new Exception($"Unexpected type for {assetName}");
            }

            foreach (var npc in Npc.Npcs)
            {
                rainyDialogue[npc.Value.Item1.Name] = npc.Value.Item1.Dialogues.RainyDialogue.ToString();
            }
        }

        #endregion
    }
}