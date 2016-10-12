using Farmhand.API.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmhand.API.Crops
{
    public class Crop
    {
        public static Dictionary<int, CropInformation> Crops { get; } = new Dictionary<int, CropInformation>();

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

            crop.Id = StardewValley.Game1.content.Load<Dictionary<int, string>>("Data\\Crops").Count()+1;

            Crops.Add(crop.Seed, crop);
            TextureUtility.AddSpriteToSpritesheet(ref Game1.cropSpriteSheet, crop.Texture, crop.Id, 128, 32);
        }
    }
}
