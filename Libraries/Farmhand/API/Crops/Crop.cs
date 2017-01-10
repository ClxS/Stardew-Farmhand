namespace Farmhand.API.Crops
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Utilities;
    using Farmhand.Attributes;
    using Farmhand.Logging;

    using StardewValley;

    /// <summary>
    ///     Crops-related API functionality.
    /// </summary>
    public static class Crop
    {
        private static readonly List<StardewValley.Crop> DeserializedCrops = new List<StardewValley.Crop>();

        /// <summary>
        ///     Gets the registered crops.
        /// </summary>
        public static Dictionary<int, CropInformation> Crops { get; } = new Dictionary<int, CropInformation>();

        /// <summary>
        ///     Gets the registered crop type information.
        /// </summary>
        public static Dictionary<Type, CropInformation> RegisteredTypeInformation { get; } =
            new Dictionary<Type, CropInformation>();

        /// <summary>
        ///     Registers a new crop
        /// </summary>
        /// <typeparam name="T">
        ///     The <see cref="Type" /> for this crop instances.
        /// </typeparam>
        /// <param name="crop">
        ///     Information of crop to register
        /// </param>
        public static void RegisterCrop<T>(CropInformation crop)
        {
            if (Game1.cropSpriteSheet == null)
            {
                throw new Exception(
                    "objectInformation is null! This likely occurs if you try to register a crop before AfterContentLoaded");
            }

            crop.Id = Content.ContentManager.Load<Dictionary<int, string>>("Data\\Crops").Count + 1;

            Crops.Add(crop.Seed, crop);
            TextureUtility.AddSpriteToSpritesheet(ref Game1.cropSpriteSheet, crop.Texture, crop.Id, 128, 32);
            RegisteredTypeInformation[typeof(T)] = crop;
        }

        /// <summary>
        ///     Registers a crop for de-serialization purposes.
        /// </summary>
        /// <param name="this">
        ///     The this.
        /// </param>
        /// <remarks>
        ///     Is called from the default constructor of StardewValley.Crop,
        ///     and alerts this registry to fix its ID after loading is finished
        /// </remarks>
        [Hook(HookType.Exit, "StardewValley.Crop", "System.Void StardewValley.Crop::.ctor()")]
        public static void RegisterDeserializingCrop([ThisBind] object @this)
        {
            var crop = @this as StardewValley.Crop;
            if (crop != null)
            {
                DeserializedCrops.Add(crop);
            }
        }

        // Uses the registered deserialized objects list to fix all the IDs
        internal static void FixupCropIds(object sender, EventArgs e)
        {
            foreach (var deserializedCrop in DeserializedCrops)
            {
                var type = deserializedCrop.GetType();

                if (RegisteredTypeInformation.ContainsKey(type))
                {
                    var expectedIndexOfHarvest = RegisteredTypeInformation[type].Yield;
                    var expectedRowInSpriteSheet = RegisteredTypeInformation[type].Id;

                    if (deserializedCrop.indexOfHarvest != expectedIndexOfHarvest)
                    {
                        Log.Warning(
                            $"Correcting harvest index mismatch - {type.Name} - {deserializedCrop.indexOfHarvest} != {expectedIndexOfHarvest}");
                        deserializedCrop.indexOfHarvest = expectedIndexOfHarvest;
                    }

                    if (deserializedCrop.rowInSpriteSheet != expectedRowInSpriteSheet)
                    {
                        Log.Warning(
                            $"Correcting id mismatch - {type.Name} - {deserializedCrop.rowInSpriteSheet} != {expectedRowInSpriteSheet}");
                        deserializedCrop.rowInSpriteSheet = expectedRowInSpriteSheet;
                    }
                }
            }
        }
    }
}