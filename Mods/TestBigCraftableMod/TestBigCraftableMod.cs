using Farmhand;
using Farmhand.API.Items;
using TestBigCraftableMod.BigCraftables;

namespace TestBigCraftableMod
{
    public class TestBigCraftableMod : Mod
    {
        public static TestBigCraftableMod Instance;

        public override void Entry()
        {
            Instance = this;

            Farmhand.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.PlayerEvents.OnFarmerChanged += PlayerEvents_OnFarmerChanged;

            Farmhand.API.Serializer.RegisterType<TestBigCraftable>();
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            BigCraftable.RegisterBigCraftable<TestBigCraftable>(TestBigCraftable.StaticInformation);
            Farmhand.API.Shops.ShopUtilities.AddToShopStock(Instance, Farmhand.API.Shops.Shops.Pierre, TestBigCraftable.StaticInformation);
        }

        private void PlayerEvents_OnFarmerChanged(object sender, System.EventArgs e)
        {
            Farmhand.API.Player.Player.AddObject<TestBigCraftable>();
        }
    }
}
