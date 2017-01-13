using Farmhand;
using Farmhand.API.Shops;
using StardewValley;
using Farmhand.Events;
using Farmhand.Events.Arguments.GlobalRoute;
using System.Collections.Generic;
using TestCropMod.Crops;
using TestCropMod.Items;
using Microsoft.Xna.Framework;

namespace TestCropMod
{
    public class TestCropMod : Mod
    {
        public static TestCropMod Instance;

        public override void Entry()
        {
            Instance = this;

            Farmhand.Events.GameEvents.AfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.PlayerEvents.FarmerChanged += PlayerEvents_OnFarmerChanged;

            Farmhand.API.Serializer.RegisterType<Bluemelon>();
            Farmhand.API.Serializer.RegisterType<BluemelonSeeds>();
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            Farmhand.API.Items.Item.RegisterItem<Bluemelon>(Bluemelon.Information);
            Farmhand.API.Items.Item.RegisterItem<BluemelonSeeds>(BluemelonSeeds.Information);
            Farmhand.API.Crops.Crop.RegisterCrop<TestCrop>(TestCrop.Information);

            // Adds a new item to piere's shop, with a delegate to check whether or not they're on sale
            ShopUtilities.AddToShopStock(Instance, Shops.Pierre, BluemelonSeeds.Information, new ShopUtilities.CheckIfAddShopStock(CheckIfBluemelonSeedsAreOnSale), 123);
            // Adds a new item to joja's shop, without a delegate to check whether or not they're no sale, so they always will be
            ShopUtilities.AddToShopStock(Instance, Shops.Joja, BluemelonSeeds.Information, 123);
        }

        private void PlayerEvents_OnFarmerChanged(object sender, System.EventArgs e)
        {
            Farmhand.API.Player.Player.AddObject(Bluemelon.Information.Id);
            Farmhand.API.Player.Player.AddObject(BluemelonSeeds.Information.Id);
        }

        public static bool CheckIfBluemelonSeedsAreOnSale()
        {
            return !StardewValley.Game1.currentSeason.Equals("winter");
        }
    }
}
