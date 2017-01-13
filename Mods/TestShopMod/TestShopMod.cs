using Farmhand;
using StardewValley;
using TestShopMod.Tools;
using Farmhand.API.Shops;

namespace TestShopMod
{
    public class TestShopMod : Mod
    {
        public static TestShopMod Instance;

        public override void Entry()
        {
            Instance = this;

            Farmhand.API.Serializer.RegisterType<DimensionalSack>();

            Farmhand.Events.GameEvents.AfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.PlayerEvents.FarmerChanged += PlayerEvents_OnFarmerChanged;
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            // Register the DimensionalSack
            Farmhand.API.Tools.Tool.RegisterTool<StardewValley.Tool>(DimensionalSack.Information);

            // Register the new shop
            ShopUtilities.RegisterShop(Instance, "DimensionalSack");
            // Add stock to the new shop
            ShopUtilities.AddToShopStock(Instance, "DimensionalSack", StockType.Item, 167, 5);
            ShopUtilities.AddToShopStock(Instance, "DimensionalSack", StockType.Item, 168);
            ShopUtilities.AddToShopStock(Instance, "DimensionalSack", StockType.Item, 169);
            ShopUtilities.AddToShopStock(Instance, "DimensionalSack", StockType.Item, 170);
            ShopUtilities.AddToShopStock(Instance, "DimensionalSack", StockType.Item, 171);
            ShopUtilities.AddToShopStock(Instance, "DimensionalSack", StockType.Item, 172);
        }

        private void PlayerEvents_OnFarmerChanged(object sender, System.EventArgs e)
        {
            // Check if the player already has this tool
            bool hasTool = false;
            for (int i = 0; i < Game1.player.items.Count; i++)
            {
                if (Game1.player.items[i] is DimensionalSack)
                {
                    hasTool = true;
                }
            }

            // Give the player the tool
            if (!hasTool)
            {
                Farmhand.API.Player.Player.AddTool<DimensionalSack>();
            }
        }
    }
}
