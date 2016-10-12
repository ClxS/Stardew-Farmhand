using Farmhand.API.Utilities;
using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;

namespace Farmhand.API.Items
{
    /// <summary>
    /// A class which can be extended from to create big craftable items easier, and provides functionality related to big craftables
    /// </summary>
    public class BigCraftable : StardewValley.Object
    {
        // A static list containing all registered big craftable information
        public static List<BigCraftableInformation> BigCraftables { get; } = new List<BigCraftableInformation>();
        public static Dictionary<Type, BigCraftableInformation> RegisteredTypeInformation { get; } = new Dictionary<Type, BigCraftableInformation>();

        /// <summary>
        /// Registers a new big craftable
        /// </summary>
        /// <param name="bigCraftable">Information of big craftable to register</param>
        public static void RegisterBigCraftable<T>(BigCraftableInformation bigCraftable)
        {
            if (Game1.bigCraftableSpriteSheet == null)
            {
                throw new Exception("objectInformation is null! This likely occurs if you try to register a big craftable before AfterContentLoaded");
            }

            bigCraftable.Id = IdManager.AssignNewIdSequential(Game1.bigCraftablesInformation);

            BigCraftables.Add(bigCraftable);
            RegisteredTypeInformation[typeof(T)] = bigCraftable;
            TextureUtility.AddSpriteToSpritesheet(ref Game1.bigCraftableSpriteSheet, bigCraftable.Texture, bigCraftable.Id, 16, 32);
            // Reload big craftable information with the newly injected information
            StardewValley.Game1.bigCraftablesInformation = StardewValley.Game1.content.Load<Dictionary<int, string>>("Data\\BigCraftablesInformation");
        }


        // Big Craftable Information
        public BigCraftableInformation Information { get; set; }

        public BigCraftable(BigCraftableInformation information) :
            base(Vector2.Zero, information.Id, false)
        {
            Information = information;
        }

        [HookRedirectConstructorToMethod("StardewValley.Utility","tryToAddObjectToHome")]
        public static StardewValley.Object Create(Vector2 vector, int Id, string name, bool canBeSetDown, bool canBeGrabbed, bool isHoeDirt, bool isSpawnedObject)
        {
            return new StardewValley.Object(vector, Id, name, canBeSetDown, canBeGrabbed, isHoeDirt, isSpawnedObject);
        }
    }
}
