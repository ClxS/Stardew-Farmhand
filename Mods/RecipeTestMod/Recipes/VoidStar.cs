using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.API.Crafting;
using Farmhand.API.Generic;
using Farmhand.API.Items;
using Farmhand.Registries;

namespace RecipeTestMod.Recipes
{
    class VoidStar
    {
        private static Farmhand.API.Crafting.CraftingRecipe _recipe;
        public static Farmhand.API.Crafting.CraftingRecipe Recipe
        {
            get
            {
                if (_recipe == null)
                {
                    _recipe = new Farmhand.API.Crafting.CraftingRecipe
                    {
                        Name = "Void Stuff",
                        Category = "Home",
                        IsBigCraftable = false,
                        RecipeUnlockType = RecipeUnlockType.Manual,
                        RequiredMaterials = new List<ItemQuantityPair>()
                        {
                            new ItemQuantityPair() { ItemId = 390, Count = 10}
                        },
                        ItemsProduced = new List<ItemQuantityPair>()
                        {
                            new ItemQuantityPair() { ItemId = 768, Count = 1}
                        }
                    };
                }

                return _recipe;
            }
        }

        


    }
}
