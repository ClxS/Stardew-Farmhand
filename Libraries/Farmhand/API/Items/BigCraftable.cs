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
    public class BigCraftable
    {
        // A static list containing all registered big craftable information
        public static List<BigCraftableInformation> BigCraftables { get; } = new List<BigCraftableInformation>();
        public static Dictionary<Type, BigCraftableInformation> RegisteredTypeInformation { get; } = new Dictionary<Type, BigCraftableInformation>();
        public static Dictionary<int, Type> RegisteredIdType { get; } = new Dictionary<int, Type>();

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
            RegisteredIdType[bigCraftable.Id] = typeof(T);
            TextureUtility.AddSpriteToSpritesheet(ref Game1.bigCraftableSpriteSheet, bigCraftable.Texture, bigCraftable.Id, 16, 32);
            // Reload big craftable information with the newly injected information
            StardewValley.Game1.bigCraftablesInformation = StardewValley.Game1.content.Load<Dictionary<int, string>>("Data\\BigCraftablesInformation");
        }

        [HookRedirectConstructorToMethod("StardewValley.Buildings.Barn", "performActionOnConstruction")]
        [HookRedirectConstructorToMethod("StardewValley.Buildings.Barn", "performActionOnUpgrade")]
        [HookRedirectConstructorToMethod("StardewValley.Buildings.Building", "dayUpdate")]
        [HookRedirectConstructorToMethod("StardewValley.Buildings.Coop", "performActionOnConstruction")]
        [HookRedirectConstructorToMethod("StardewValley.Buildings.Coop", "performActionOnUpgrade")]
        [HookRedirectConstructorToMethod("StardewValley.Buildings.Coop", "upgrade")]
        [HookRedirectConstructorToMethod("StardewValley.CraftingRecipe", "createItem")]
        [HookRedirectConstructorToMethod("StardewValley.Debris", "updateChunks")]
        [HookRedirectConstructorToMethod("StardewValley.Event", "checkAction")]
        [HookRedirectConstructorToMethod("StardewValley.Event", "checkForNextCommand")]
        [HookRedirectConstructorToMethod("StardewValley.Events.SoundInTheNightEvent", "makeChangesToLocation")]
        [HookRedirectConstructorToMethod("StardewValley.Farm", "checkAction")]
        [HookRedirectConstructorToMethod("StardewValley.Farmer", "tryToCraftItem")]
        [HookRedirectConstructorToMethod("StardewValley.Game1", "loadForNewGame")]
        [HookRedirectConstructorToMethod("StardewValley.Game1", "parseDebugInput")]
        [HookRedirectConstructorToMethod("StardewValley.GameLocation", "answerDialogueAction")]
        [HookRedirectConstructorToMethod("StardewValley.GameLocation", "doStarpoint")]
        [HookRedirectConstructorToMethod("StardewValley.GameLocation", "performAction")]
        [HookRedirectConstructorToMethod("StardewValley.LevelBuilder", "tryToAddObject")]
        [HookRedirectConstructorToMethod("StardewValley.Locations.FarmCave", "setUpMushroomHouse")]
        [HookRedirectConstructorToMethod("StardewValley.Locations.LibraryMuseum", "getRewardsForPlayer")]
        [HookRedirectConstructorToMethod("StardewValley.Locations.Sewer", "populateShopStock")]
        [HookRedirectConstructorToMethod("StardewValley.Object", "getOne")]
        [HookRedirectConstructorToMethod("StardewValley.Object", "minutesElapsed")]
        [HookRedirectConstructorToMethod("StardewValley.Objects.ObjectFactory", "getItemFromDescription")]
        [HookRedirectConstructorToMethod("StardewValley.SlimeHutch", "DayUpdate")]
        [HookRedirectConstructorToMethod("StardewValley.Utility", "generateNewFarm")]
        [HookRedirectConstructorToMethod("StardewValley.Utility", "getAnimalShopStock")]
        [HookRedirectConstructorToMethod("StardewValley.Utility", "getCarpenterStock")]
        [HookRedirectConstructorToMethod("StardewValley.Utility", "getDwarfShopStock")]
        [HookRedirectConstructorToMethod("StardewValley.Utility", "getItemFromStandardTextDescription")]
        [HookRedirectConstructorToMethod("StardewValley.Utility", "getQiShopStock")]
        [HookRedirectConstructorToMethod("StardewValley.Utility", "getTravelingMerchantStock")]
        public static StardewValley.Object Create(Vector2 vector, int Id, bool isRecipe = false)
        {
            // Search for a big craftable with a custom type
            foreach (BigCraftableInformation bigCraftable in BigCraftables)
            {
                if(bigCraftable.Id == Id)
                {
                    try
                    {
                        Type bigCraftableType = RegisteredIdType[Id];
                        return (StardewValley.Object)Activator.CreateInstance(bigCraftableType, bigCraftable, vector, Id, isRecipe);
                    }
                    catch(Exception e)
                    {
                        Logging.Log.Error($"{e.Message}");
                    }
                }
            }
            // If there are none, just use a default object
            return new StardewValley.Object(vector, Id, isRecipe);
        }
    }
}
