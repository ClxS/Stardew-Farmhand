using Farmhand.API.Utilities;
using Farmhand.Attributes;
using Farmhand.Logging;
using StardewValley;
using System;
using System.Collections.Generic;

namespace Farmhand.API.Tools
{
    public class Weapon
    {
        private static List<StardewValley.Tools.MeleeWeapon> deserializedWeapons = new List<StardewValley.Tools.MeleeWeapon>();

        public static List<WeaponInformation> Weapons { get; } = new List<WeaponInformation>();
        public static Dictionary<Type, WeaponInformation> RegisteredTypeInformation { get; } = new Dictionary<Type, WeaponInformation>();

        /// <summary>
        /// Registers a new weapon
        /// </summary>
        /// <param name="weapon">Information of weapon to register</param>
        public static void RegisterWeapon<T>(WeaponInformation weapon)
        {
            if (StardewValley.Tool.weaponsTexture == null)
            {
                throw new Exception("objectInformation is null! This likely occurs if you try to register an item before AfterContentLoaded");
            }

            weapon.Id = IdManager.AssignNewIdSequential(Game1.content.Load<Dictionary<int, string>>("Data\\weapons"));
            Weapons.Add(weapon);
            RegisteredTypeInformation[typeof(T)] = weapon;
            TextureUtility.AddSpriteToSpritesheet(ref StardewValley.Tool.weaponsTexture, weapon.Texture, weapon.Id, 16, 16);
        }

        // Uses the registered deserialized objects list to fix all the IDs
        internal static void FixupWeaponIds(object sender, System.EventArgs e)
        {
            foreach (StardewValley.Tools.MeleeWeapon deserializedWeapon in deserializedWeapons)
            {
                var type = deserializedWeapon.GetType();
                int expectedId = -1;

                if (RegisteredTypeInformation.ContainsKey(type))
                {
                    expectedId = RegisteredTypeInformation[type].Id;

                    if (deserializedWeapon.currentParentTileIndex != expectedId)
                    {
                        Log.Warning($"Correcting id mismatch - {type.Name} - {deserializedWeapon.parentSheetIndex} != {expectedId}");
                        deserializedWeapon.initialParentTileIndex = expectedId;
                        deserializedWeapon.currentParentTileIndex = expectedId;
                        deserializedWeapon.indexOfMenuItemView = expectedId;
                    }
                }
            }
        }

        // Is called from the default constructor of StardewValley.Tools.MeleeWeapon, and alerts this registry to fix its ID after loading is finished
        [Hook(HookType.Exit, "StardewValley.Tools.MeleeWeapon", "System.Void StardewValley.Tools.MeleeWeapon::.ctor()")]
        public static void RegisterDeserializingObject([ThisBind] object @this)
        {
            if (@this is StardewValley.Tools.MeleeWeapon)
            {
                deserializedWeapons.Add(@this as StardewValley.Tools.MeleeWeapon);
            }
        }
    }
}
