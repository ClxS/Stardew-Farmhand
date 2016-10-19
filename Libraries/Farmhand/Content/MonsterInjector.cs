using System;
using System.Collections.Generic;

namespace Farmhand.Content
{
    class MonsterInjector : IContentInjector
    {
        public bool IsLoader => false;
        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\Monsters";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var monsters = obj as Dictionary<string, string>;
            if (monsters == null)
                throw new Exception($"Unexpected type for {assetName}");

            foreach (var monster in Farmhand.API.Monsters.Monster.Monsters)
            {
                monsters[monster.Value.Name] = monster.Value.ToString();
            }
        }
    }
}
