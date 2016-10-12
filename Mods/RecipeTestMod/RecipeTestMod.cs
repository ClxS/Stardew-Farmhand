using Farmhand;
using Farmhand.API.Generic;
using RecipeTestMod.Items;
using RecipeTestMod.Recipes;
using Farmhand.Events.Arguments.PlayerEvents;

namespace RecipeTestMod
{
    internal class RecipeTestMod : Mod
    {
        public static RecipeTestMod Instance;
        
        public override void Entry()
        {
            Instance = this;
            
            Farmhand.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.PlayerEvents.OnFarmerChanged += PlayerEvents_OnFarmerChanged;

            Farmhand.API.Serializer.RegisterType<Heart>();
            Farmhand.API.Serializer.RegisterType<PuppyTail>();
            Farmhand.API.Serializer.RegisterType<RabbitsPaw>();
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            Farmhand.API.Items.Item.RegisterItem<Heart>(Heart.Information);
            Farmhand.API.Items.Item.RegisterItem<PuppyTail>(PuppyTail.Information);
            Farmhand.API.Items.Item.RegisterItem<RabbitsPaw>(RabbitsPaw.Information);
            VoidStar.Recipe.MaterialsRequired.Add(new ItemQuantityPair { Count = 10, ItemId = Heart.Information.Id });
            VoidStar.Recipe.MaterialsRequired.Add(new ItemQuantityPair { Count = 2, ItemId = PuppyTail.Information.Id });
            Farmhand.API.Crafting.CraftingRecipe.RegisterRecipe(VoidStar.Recipe);
        }
        
        private void PlayerEvents_OnFarmerChanged(object sender, EventArgsOnFarmerChanged e)
        {
            Farmhand.API.Player.Player.AddRecipe(VoidStar.Recipe.PrivateName);
            Farmhand.API.Player.Player.AddObject<Heart>();
            Farmhand.API.Player.Player.AddObject<PuppyTail>();
            Farmhand.API.Player.Player.AddObject<RabbitsPaw>();
        }

    }
}
