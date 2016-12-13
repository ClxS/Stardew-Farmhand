using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StardewValley;

namespace Farmhand.Content
{
    class MonsterLoader : IContentInjector
    {
        public bool IsLoader => true;
        public bool IsInjector => false;

        // This exceptions list is used to prevent this loader from trying to load vanilla game assets
        // TODO this feels more like a hotfix, I'm sure there's a more elegant solution
        public List<string> MonsterExceptions
            => Directory.GetFiles($"{Game1.content.RootDirectory}\\Characters\\Monsters")
                .Select(file => file?.Replace("Content\\", "").Replace(".xnb", ""))
                .ToList();

        public bool HandlesAsset(Type type, string asset)
        {

            // Then if this is in the Monsters folder, and not vanilla, it is handled by this loader.
            return !MonsterExceptions.Any(_ => _.Equals(asset)) && asset.StartsWith("Characters\\Monsters\\");
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            string baseName = assetName.Replace("Characters\\Monsters\\", "");

            object Sprite = Farmhand.API.Monsters.Monster.Monsters[baseName].Texture;

            return (T)Convert.ChangeType(Sprite, typeof(T));
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            Logging.Log.Error("You shouldn't be here!");
        }
    }
}
