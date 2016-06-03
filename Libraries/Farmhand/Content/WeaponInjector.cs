using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Content
{
    class WeaponInjector : IContentInjector
    {
        public bool IsLoader => false;
        public bool IsInjector => true;

        public bool HandlesAsset(Type type, string asset)
        {
            return asset == "Data\\weapons";
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            Logging.Log.Error("You shouldn't be here!");
            return default(T);
        }

        public void Inject<T>(T obj, string assetName)
        {
            var weapons = obj as Dictionary<int, string>;
            if (weapons == null)
                throw new Exception($"Unexpected type for {assetName}");

            foreach (var weapon in Farmhand.API.Tools.Weapon.Weapons)
            {
                weapons[weapon.Id] = weapon.ToString();
            }
        }
    }
}
