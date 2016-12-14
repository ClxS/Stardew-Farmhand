using Farmhand.API.Utilities;
using Farmhand.Attributes;
using Farmhand.Logging;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmhand.API.Crops
{
    public class Crop
    {
        private static List<StardewValley.Crop> deserializedCrops = new List<StardewValley.Crop>();

        public static Dictionary<int, CropInformation> Crops { get; } = new Dictionary<int, CropInformation>();
        public static Dictionary<Type, CropInformation> RegisteredTypeInformation { get; } = new Dictionary<Type, CropInformation>();

        /// <summary>
        /// Registers a new crop
        /// </summary>
        /// <param name="crop">Information of crop to register</param>
        public static void RegisterCrop<T>(CropInformation crop)
        {
            if (Game1.cropSpriteSheet == null)
            {
                throw new Exception("objectInformation is null! This likely occurs if you try to register a crop before AfterContentLoaded");
            }

            crop.Id = Farmhand.API.Content.ContentManager.Load<Dictionary<int, string>>("Data\\Crops").Count()+1;

            Crops.Add(crop.Seed, crop);
            TextureUtility.AddSpriteToSpritesheet(ref Game1.cropSpriteSheet, crop.Texture, crop.Id, 128, 32);
            RegisteredTypeInformation[typeof(T)] = crop;
        }

        // Uses the registered deserialized objects list to fix all the IDs
        internal static void FixupCropIds(object sender, System.EventArgs e)
        {
            foreach (StardewValley.Crop deserializedCrop in deserializedCrops)
            {
                var type = deserializedCrop.GetType();

                if (RegisteredTypeInformation.ContainsKey(type))
                {
                    int expectedIndexOfHarvest = RegisteredTypeInformation[type].Yield;
                    int expectedRowInSpriteSheet = RegisteredTypeInformation[type].Id;

                    if (deserializedCrop.indexOfHarvest != expectedIndexOfHarvest)
                    {
                        Log.Warning($"Correcting harvest index mismatch - {type.Name} - {deserializedCrop.indexOfHarvest} != {expectedIndexOfHarvest}");
                        deserializedCrop.indexOfHarvest = expectedIndexOfHarvest;
                    }

                    if (deserializedCrop.rowInSpriteSheet != expectedRowInSpriteSheet)
                    {
                        Log.Warning($"Correcting id mismatch - {type.Name} - {deserializedCrop.rowInSpriteSheet} != {expectedRowInSpriteSheet}");
                        deserializedCrop.rowInSpriteSheet = expectedRowInSpriteSheet;
                    }
                }
            }
        }

        // Is called from the default constructor of StardewValley.Crop, and alerts this registry to fix its ID after loading is finished
        [Hook(HookType.Exit, "StardewValley.Crop", "System.Void StardewValley.Crop::.ctor()")]
        public static void RegisterDeserializingCrop([ThisBind] object @this)
        {
            if (@this is StardewValley.Crop)
            {
                deserializedCrops.Add(@this as StardewValley.Crop);
            }
        }
    }
}
