namespace Farmhand.API.Tools
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Utilities;
    using Farmhand.Attributes;
    using Farmhand.Logging;

    using StardewValley;
    using StardewValley.Tools;

    /// <summary>
    ///     Weapon-related API functionality.
    /// </summary>
    public static class Weapon
    {
        private static readonly List<MeleeWeapon> DeserializedWeapons = new List<MeleeWeapon>();

        /// <summary>
        ///     Gets a <see cref="List{WeaponInformation}" /> of registered weapons.
        /// </summary>
        public static List<WeaponInformation> Weapons { get; } = new List<WeaponInformation>();

        internal static Dictionary<Type, WeaponInformation> RegisteredTypeInformation { get; } =
            new Dictionary<Type, WeaponInformation>();

        /// <summary>
        ///     Registers a new weapon
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the weapon to register.
        /// </typeparam>
        /// <param name="weapon">
        ///     Information about the weapon to register
        /// </param>
        public static void RegisterWeapon<T>(WeaponInformation weapon)
        {
            if (StardewValley.Tool.weaponsTexture == null)
            {
                throw new Exception(
                    "objectInformation is null! This likely occurs if you try to register an item before AfterContentLoaded");
            }

            weapon.Id = IdManager.AssignNewIdSequential(Game1.content.Load<Dictionary<int, string>>("Data\\weapons"));
            Weapons.Add(weapon);
            RegisteredTypeInformation[typeof(T)] = weapon;
            TextureUtility.AddSpriteToSpritesheet(
                ref StardewValley.Tool.weaponsTexture,
                weapon.Texture,
                weapon.Id,
                16,
                16);
        }

        // Uses the registered deserialized objects list to fix all the IDs
        internal static void FixupWeaponIds(object sender, EventArgs e)
        {
            foreach (var deserializedWeapon in DeserializedWeapons)
            {
                var type = deserializedWeapon.GetType();

                if (RegisteredTypeInformation.ContainsKey(type))
                {
                    var expectedId = RegisteredTypeInformation[type].Id;

                    if (deserializedWeapon.currentParentTileIndex != expectedId)
                    {
                        Log.Warning(
                            $"Correcting id mismatch - {type.Name} - {deserializedWeapon.parentSheetIndex} != {expectedId}");
                        deserializedWeapon.initialParentTileIndex = expectedId;
                        deserializedWeapon.currentParentTileIndex = expectedId;
                        deserializedWeapon.indexOfMenuItemView = expectedId;
                    }
                }
            }
        }

        // Is called from the default constructor of StardewValley.Tools.MeleeWeapon, and alerts this registry to fix its ID after loading is finished      
        [Hook(HookType.Exit, "StardewValley.Tools.MeleeWeapon", "System.Void StardewValley.Tools.MeleeWeapon::.ctor()")]
        internal static void RegisterDeserializingObject([ThisBind] object @this)
        {
            var meleeWeapon = @this as MeleeWeapon;
            if (meleeWeapon != null)
            {
                DeserializedWeapons.Add(meleeWeapon);
            }
        }
    }
}