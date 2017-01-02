namespace Farmhand.Content.Injectors.NPCs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Farmhand.API.NPCs;
    using Farmhand.Logging;

    using StardewValley;

    internal class NpcLoader : IContentInjector
    {
        public List<string> NpcExceptions
            =>
                Directory.GetFiles($"{Game1.content.RootDirectory}\\Characters")
                    .Select(file => file?.Replace("Content\\", string.Empty).Replace(".xnb", string.Empty))
                    .ToList();

        #region IContentInjector Members

        public bool IsInjector => false;

        public bool IsLoader => true;

        public bool HandlesAsset(Type type, string assetName)
        {
            var baseName = assetName.Replace("Characters\\", string.Empty);
            return Npc.Npcs.ContainsKey(baseName);
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var baseName = assetName.Replace("Characters\\", string.Empty);
            var sprite = Npc.Npcs[baseName].Item1.Texture;

            return (T)Convert.ChangeType(sprite, typeof(T));
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            Log.Error("You shouldn't be here!");
        }

        #endregion
    }
}