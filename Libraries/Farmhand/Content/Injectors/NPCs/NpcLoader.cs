using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StardewValley;

namespace Farmhand.Content.Injectors.NPCs
{
    public class NpcLoader : IContentInjector
    {
        public bool IsInjector => false;
        public bool IsLoader => true;

        public List<string> NpcExceptions
            => Directory.GetFiles($"{Game1.content.RootDirectory}\\Characters")
                .Select(file => file?.Replace("Content\\", "").Replace(".xnb", ""))
                .ToList();

        public bool HandlesAsset(Type type, string assetName)
        {
            var baseName = assetName.Replace("Characters\\", "");
            return API.NPCs.Npc.Npcs.ContainsKey(baseName);
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var baseName = assetName.Replace("Characters\\", "");
            var sprite = API.NPCs.Npc.Npcs[baseName].Item1.Texture;

            return (T)Convert.ChangeType(sprite, typeof(T));
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            Logging.Log.Error("You shouldn't be here!");
        }
    }
}
