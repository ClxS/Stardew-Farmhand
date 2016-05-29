using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Content
{
    class MonsterLoader : IContentInjector
    {
        public bool IsLoader => true;
        public bool IsInjector => false;

        // This exceptions list is used to prevent this loader from trying to load vanilla game assets
        // TODO this feels more like a hotfix, I'm sure there's a more elegant solution
        public string[] MonsterExceptions = new string[] 
        {
            "Characters\\Monsters\\Armored Bug",
            "Characters\\Monsters\\Bat",
            "Characters\\Monsters\\Big Slime",
            "Characters\\Monsters\\Bug",
            "Characters\\Monsters\\Cat",
            "Characters\\Monsters\\Crow",
            "Characters\\Monsters\\Duggy",
            "Characters\\Monsters\\Dust Spirit",
            "Characters\\Monsters\\Fireball",
            "Characters\\Monsters\\Fly",
            "Characters\\Monsters\\Frog",
            "Characters\\Monsters\\Frost Bat",
            "Characters\\Monsters\\Frost Jelly",
            "Characters\\Monsters\\Ghost",
            "Characters\\Monsters\\Goblin Peasant",
            "Characters\\Monsters\\Green Slime",
            "Characters\\Monsters\\Grub",
            "Characters\\Monsters\\Lava Bat",
            "Characters\\Monsters\\Lava Crab",
            "Characters\\Monsters\\Metal Head",
            "Characters\\Monsters\\Mummy",
            "Characters\\Monsters\\Rock Crab",
            "Characters\\Monsters\\Serpent",
            "Characters\\Monsters\\Shadow Brute",
            "Characters\\Monsters\\Shadow Girl",
            "Characters\\Monsters\\Shadow Guy",
            "Characters\\Monsters\\Shadow King",
            "Characters\\Monsters\\Shadow Shaman",
            "Characters\\Monsters\\Skeleton Mage",
            "Characters\\Monsters\\Skeleton Warrior",
            "Characters\\Monsters\\Skeleton",
            "Characters\\Monsters\\Sludge",
            "Characters\\Monsters\\Spiker",
            "Characters\\Monsters\\Squid Kid",
            "Characters\\Monsters\\Stone Golem",
            "Characters\\Monsters\\Turtle",
            "Characters\\Monsters\\Yeti",
        };

        public bool HandlesAsset(Type type, string asset)
        {
            // Check the exceptions list for vanilla assets
            for(int i=0; i<MonsterExceptions.Length; i++)
            {
                if(asset == MonsterExceptions[i])
                {
                    return false;
                }
            }

            // Then if this is in the Monsters folder, and not vanilla, it is handled by this loader.
            return asset.StartsWith("Characters\\Monsters\\");
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            string baseName = assetName.Replace("Characters\\Monsters\\", "");

            object Sprite = Farmhand.API.Monsters.Monster.Monsters[baseName].Texture;

            return (T)Convert.ChangeType(Sprite, typeof(T));
        }

        public void Inject<T>(T obj, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
        }
    }
}
