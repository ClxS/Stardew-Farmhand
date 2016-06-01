using Farmhand;
using StardewValley;
using Farmhand.Events;
using Farmhand.Events.Arguments.GlobalRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Farmhand.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.PlayerEvents.OnFarmerChanged += PlayerEvents_OnFarmerChanged;

            Farmhand.API.Serializer.RegisterType<Bluemelon>();
            Farmhand.API.Serializer.RegisterType<BluemelonSeeds>();

            // TODO once returnable events are working, this can be replaced with a proper event
            Farmhand.Events.GlobalRouteManager.Listen(ListenerType.Post, "StardewValley.Locations.SeedShop", "shopStock", SeedShopEvents_PostGetShopStock);
            Farmhand.Events.GlobalRouteManager.Listen(ListenerType.Post, "StardewValley.Utility", "getHospitalStock", UtilityEvents_PostGetHospitalStock);
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            Farmhand.API.Items.Item.RegisterItem<Bluemelon>(Bluemelon.Information);
            Farmhand.API.Items.Item.RegisterItem<BluemelonSeeds>(BluemelonSeeds.Information);
            Farmhand.API.Crops.Crop.RegisterCrop<TestCrop>(TestCrop.Information);
        }

        private void PlayerEvents_OnFarmerChanged(object sender, System.EventArgs e)
        {
            Farmhand.API.Player.Player.AddObject(Bluemelon.Information.Id);
            Farmhand.API.Player.Player.AddObject(BluemelonSeeds.Information.Id);
        }

        // This adds bluemelon seeds from this mod to Pierre's stock. They will be at the bottom since no list sorting is done
        private void SeedShopEvents_PostGetShopStock(EventArgsGlobalRoute e)
        {
            Farmhand.Logging.Log.Success("Altering Pierre shop stock");

            var args = e as EventArgsGlobalRouteReturnable;

            List<Item> normalStock = (List<Item>)args.Output;

            normalStock.Add(new StardewValley.Object(Vector2.Zero, BluemelonSeeds.Information.Id, 2147483647));

            args.Output = normalStock;
        }

        // This adds Life Elixer to the hospital shop stock, as an example of another shop because pierre's shop stock is weird. Most shops are from Utility
        private void UtilityEvents_PostGetHospitalStock(EventArgsGlobalRoute e)
        {
            var args = e as EventArgsGlobalRouteReturnable;

            Dictionary<Item, int[]> normalStock = (Dictionary<Item, int[]>)args.Output;

            normalStock.Add(new StardewValley.Object(773, 1, false, -1, 0), new int[]
            {
            3000,
            2147483647
            });

            args.Output = normalStock;
        }
    }
}
