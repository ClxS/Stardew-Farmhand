using Farmhand.API.Utilities;
using Farmhand.Attributes;
using Farmhand.Logging;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;

namespace Farmhand.API.Items
{
    /// <summary>
    /// A class which can be extended from to create big craftable items easier, and provides functionality related to big craftables
    /// </summary>
    public static class BigCraftable
    {
        private static List<StardewValley.Object> deserializedObjects = new List<StardewValley.Object>();

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
            StardewValley.Game1.bigCraftablesInformation = Farmhand.API.Content.ContentManager.Load<Dictionary<int, string>>("Data\\BigCraftablesInformation");
        }

        // Uses the registered deserialized objects list to fix all the IDs
        internal static void FixupBigCraftableIds(object sender, System.EventArgs e)
        {
            foreach (StardewValley.Object deserializedObject in deserializedObjects)
            {
                var type = deserializedObject.GetType();
                int expectedId = -1;

                if (RegisteredTypeInformation.ContainsKey(type))
                {
                    expectedId = RegisteredTypeInformation[type].Id;

                    if (deserializedObject.parentSheetIndex != expectedId)
                    {
                        Log.Warning($"Correcting id mismatch - {type.Name} - {deserializedObject.parentSheetIndex} != {expectedId}");
                        deserializedObject.parentSheetIndex = expectedId;
                    }
                }
            }
        }

        // Is called from the default constructor of StardewValley.Object, and alerts this registry to fix its ID after loading is finished
        [Hook(HookType.Exit, "StardewValley.Object", "System.Void StardewValley.Object::.ctor()")]
        public static void RegisterDeserializingObject([ThisBind] object @this)
        {
            if (@this is StardewValley.Object)
            {
                deserializedObjects.Add(@this as StardewValley.Object);
            }
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
                        return (StardewValley.Object)Activator.CreateInstance(bigCraftableType, bigCraftable, vector, isRecipe);
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
