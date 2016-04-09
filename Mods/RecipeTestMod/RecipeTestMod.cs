using System.Collections.Generic;
using Farmhand;
using Farmhand.API;
using Farmhand.Logging;
using StardewValley;
using CraftingRecipe = Farmhand.API.CraftingRecipe;

namespace RecipeTestMod
{
    internal class RecipeTestMod : Mod
    {
        private CraftingRecipe _voidRecipe;

        public override void Entry()
        {
            _voidRecipe = new CraftingRecipe
            {
                Name = "Void Stuff",
                Category = "Home",
                IsBigCraftable = false,
                RecipeUnlockType = RecipeUnlockType.Manual,
                RequiredMaterials = new List<ItemQuantityPair>()
                {
                    new ItemQuantityPair() {ItemId = 390, Count = 10}
                },
                ItemsProduced = new List<ItemQuantityPair>()
                {
                    new ItemQuantityPair() {ItemId = 768, Count = 1}
                }
            };

            Farmhand.Events.GameEvents.OnAfterLoadedContent += GameEvents_OnAfterLoadedContent;
            Farmhand.Events.PlayerEvents.OnFarmerChanged += PlayerEvents_OnFarmerChanged;
        }

        private void GameEvents_OnAfterLoadedContent(object sender, System.EventArgs e)
        {
            CraftingRecipe.RegisterRecipe(_voidRecipe);
        }

        private void PlayerEvents_OnFarmerChanged(object sender, System.EventArgs e)
        {
            var player = sender as Farmer;
            Farmhand.API.Player.AddRecipe(_voidRecipe.PrivateName);
        }
        
    }
}
