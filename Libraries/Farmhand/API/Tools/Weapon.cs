using Farmhand.API.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;

namespace Farmhand.API.Tools
{
    public class Weapon
    {
        public static List<WeaponInformation> Weapons { get; } = new List<WeaponInformation>();

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
            TextureUtility.AddSpriteToSpritesheet(ref StardewValley.Tool.weaponsTexture, weapon.Texture, weapon.Id, 16, 16);
        }
    }
}
