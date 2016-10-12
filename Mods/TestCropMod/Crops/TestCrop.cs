using System.Collections.Generic;
using Farmhand.API.Crops;
using TestCropMod.Items;

namespace TestCropMod.Crops
{
    public class TestCrop : StardewValley.Crop
    {
        private static CropInformation _information;
        public static CropInformation Information => _information ?? (_information = new Farmhand.API.Crops.CropInformation
        {
            Name = "TestCrop",
            Texture = TestCropMod.Instance.ModSettings.GetTexture("sprite_TestCrop"),
            PhaseDays = new List<int> { 1, 1, 1, 1 },
            Seasons = new List<string> { "spring", "summer", "fall" },
            Seed = BluemelonSeeds.Information.Id,
            Yield = Bluemelon.Information.Id,
            Regrow = -1,
            HarvestMethod = 0,
            RaisedSeeds = false
    });
    }
}
