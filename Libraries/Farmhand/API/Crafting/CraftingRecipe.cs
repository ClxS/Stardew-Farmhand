using System.Collections.Generic;
using Farmhand.API.Generic;
using Farmhand.API.Utilities;
using Farmhand.Helpers;

namespace Farmhand.API.Crafting
{
    public class CraftingRecipe
    {
        /// <summary>
        /// Contains all custom crafting recipes added to the API
        /// </summary>
        public static List<CraftingRecipe> CraftingRecipes = new List<CraftingRecipe>();

        /// <summary>
        /// Adds custom save-safe crafting recipes
        /// </summary>
        /// <param name="recipe">The recipe to add</param>
        /// <returns>The internal id assigned to this recipe</returns>
        public static string RegisterRecipe(CraftingRecipe recipe)
        {
            CraftingRecipes.Add(recipe);
            recipe.PrivateName = $"{recipe.Name}:{IdManager.AssignNewId(200)}";
            StardewValley.CraftingRecipe.craftingRecipes.Add(recipe.PrivateName, recipe.RecipeString);
            return recipe.PrivateName;
        }

        private string DefaultSkillRequirement => IsBigCraftable ? "null" : "l 0";
        private string SkillString => RequiredSkill?.ToString() ?? DefaultSkillRequirement;
        public string RecipeString => $"{RequiredMaterials.ToItemSetString()}/{Category}/{ItemsProduced.ToItemSetString()}/{IsBigCraftable}/{SkillString}";

        public string Name { get; set; }
        public string PrivateName { get; internal set; }
        public string Category { get; set; } = "Home";
        public bool IsAdded => string.IsNullOrEmpty(PrivateName);
        
        public RequiredSkill RequiredSkill;
        public RecipeUnlockType RecipeUnlockType;
        public List<ItemQuantityPair> ItemsProduced;
        public List<ItemQuantityPair> RequiredMaterials;
        public bool IsBigCraftable;
    }
}
