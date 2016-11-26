using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StardewValley;

namespace Farmhand.Content.Injectors.NPCs
{
    class DialogueLoader : IContentInjector
    {
        public bool IsLoader => true;
        public bool IsInjector => true;

        public List<string> DialoguesExceptions
            => Directory.GetFiles($"{Game1.content.RootDirectory}\\Characters\\Dialogue")
                .Select(file => file?.Replace("Content\\", "").Replace(".xnb", ""))
                .ToList();

        public bool HandlesAsset(Type type, string assetName)
        {
            var baseName = assetName.Replace("Characters\\Dialogue\\", "");
            return API.NPCs.Npc.Npcs.ContainsKey(baseName);
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var baseName = assetName.Replace("Characters\\Dialogue\\", "");
            var dialogues = API.NPCs.Npc.Npcs[baseName].Item1.Dialogues.BuildBaseDialogues();

            return (T)Convert.ChangeType(dialogues, typeof(T));
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            Logging.Log.Error("You shouldn't be here!");
        }
    }
}
