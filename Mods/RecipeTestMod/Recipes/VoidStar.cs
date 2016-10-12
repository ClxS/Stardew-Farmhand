using System.Collections.Generic;
using Farmhand.API.Crafting;
using Farmhand.API.Generic;

namespace RecipeTestMod.Recipes
{
    class VoidStar
    {
        private static CraftingRecipe _recipe;
        public static CraftingRecipe Recipe
        {
            get
            {
                return _recipe ?? (_recipe = new Farmhand.API.Crafting.CraftingRecipe
                       {
                           Name = "Void Stuff",
                           Category = "Home",
                           IsBigCraftable = false,
                           RecipeUnlockType = RecipeUnlockType.Manual,
                           MaterialsRequired = new List<ItemQuantityPair>
                           {
                               new ItemQuantityPair {ItemId = 390, Count = 10}
                           },
                           ItemsProduced = new List<ItemQuantityPair>
                           {
                               new ItemQuantityPair {ItemId = 768, Count = 1}
                           }
                       });
            }
        }

        


    }
}
