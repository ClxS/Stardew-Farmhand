namespace Farmhand.Content.Injectors.NPCs.Monsters
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Monsters;
    using Farmhand.Logging;

    internal class MonsterInjector : IContentInjector
    {
        #region IContentInjector Members

        public bool IsLoader => false;

        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\Monsters";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            var monsters = obj as Dictionary<string, string>;
            if (monsters == null)
            {
                throw new Exception($"Unexpected type for {assetName}");
            }

            foreach (var monster in Monster.Monsters)
            {
                monsters[monster.Value.Name] = monster.Value.ToString();
            }
        }

        #endregion
    }
}