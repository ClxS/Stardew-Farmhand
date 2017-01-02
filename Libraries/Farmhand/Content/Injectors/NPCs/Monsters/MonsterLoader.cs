namespace Farmhand.Content.Injectors.NPCs.Monsters
{
    using System;

    using Farmhand.API.Monsters;
    using Farmhand.Logging;

    internal class MonsterLoader : IContentInjector
    {
        // This exceptions list is used to prevent this loader from trying to load vanilla game assets
        // TODO this feels more like a hotfix, I'm sure there's a more elegant solution
        public string[] MonsterExceptions =
            {
                "Characters\\Monsters\\Armored Bug", "Characters\\Monsters\\Bat",
                "Characters\\Monsters\\Big Slime", "Characters\\Monsters\\Bug",
                "Characters\\Monsters\\Cat", "Characters\\Monsters\\Crow",
                "Characters\\Monsters\\Duggy", "Characters\\Monsters\\Dust Spirit",
                "Characters\\Monsters\\Fireball", "Characters\\Monsters\\Fly",
                "Characters\\Monsters\\Frog", "Characters\\Monsters\\Frost Bat",
                "Characters\\Monsters\\Frost Jelly", "Characters\\Monsters\\Ghost",
                "Characters\\Monsters\\Goblin Peasant",
                "Characters\\Monsters\\Green Slime", "Characters\\Monsters\\Grub",
                "Characters\\Monsters\\Lava Bat", "Characters\\Monsters\\Lava Crab",
                "Characters\\Monsters\\Metal Head", "Characters\\Monsters\\Mummy",
                "Characters\\Monsters\\Rock Crab", "Characters\\Monsters\\Serpent",
                "Characters\\Monsters\\Shadow Brute",
                "Characters\\Monsters\\Shadow Girl", "Characters\\Monsters\\Shadow Guy",
                "Characters\\Monsters\\Shadow King",
                "Characters\\Monsters\\Shadow Shaman",
                "Characters\\Monsters\\Skeleton Mage",
                "Characters\\Monsters\\Skeleton Warrior",
                "Characters\\Monsters\\Skeleton", "Characters\\Monsters\\Sludge",
                "Characters\\Monsters\\Spiker", "Characters\\Monsters\\Squid Kid",
                "Characters\\Monsters\\Stone Golem", "Characters\\Monsters\\Turtle",
                "Characters\\Monsters\\Yeti"
            };

        #region IContentInjector Members

        public bool IsLoader => true;

        public bool IsInjector => false;

        public bool HandlesAsset(Type type, string asset)
        {
            // Check the exceptions list for vanilla assets
            for (var i = 0; i < this.MonsterExceptions.Length; i++)
            {
                if (asset == this.MonsterExceptions[i])
                {
                    return false;
                }
            }

            // Then if this is in the Monsters folder, and not vanilla, it is handled by this loader.
            return asset.StartsWith("Characters\\Monsters\\");
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var baseName = assetName.Replace("Characters\\Monsters\\", string.Empty);

            object sprite = Monster.Monsters[baseName].Texture;

            return (T)Convert.ChangeType(sprite, typeof(T));
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            Log.Error("You shouldn't be here!");
        }

        #endregion
    }
}