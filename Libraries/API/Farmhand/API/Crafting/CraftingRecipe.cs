namespace Farmhand.API.Crafting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.API.Generic;
    using Farmhand.API.Utilities;
    using Farmhand.Helpers;

    /// <summary>
    ///     A custom crafting recipe.
    /// </summary>
    public class CraftingRecipe
    {
        /// <summary>
        ///     Gets all custom crafting recipes added to the API
        /// </summary>
        public static List<CraftingRecipe> CraftingRecipes { get; } = new List<CraftingRecipe>();

        /// <summary>
        ///     Gets or sets the category.
        /// </summary>
        public string Category { get; set; } = "Home";

        /// <summary>
        ///     Gets or sets a value indicating whether the recipe is for a big craftable.
        /// </summary>
        public bool IsBigCraftable { get; set; }

        /// <summary>
        ///     Gets whether this recipe has been added to the API.
        /// </summary>
        public bool IsRegistered => string.IsNullOrEmpty(this.PrivateName);

        /// <summary>
        ///     Gets or sets the items produced.
        /// </summary>
        public List<ItemQuantityPair> ItemsProduced { get; set; }

        /// <summary>
        ///     Gets or sets the materials required.
        /// </summary>
        public List<ItemQuantityPair> MaterialsRequired { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets the private name for this recipe.
        /// </summary>
        public string PrivateName { get; internal set; }

        /// <summary>
        ///     Gets the recipe as a game compatible string.
        /// </summary>
        public string RecipeString
            =>
                $"{this.MaterialsRequired.ToItemSetString()}/{this.Category}/{this.ItemsProduced.ToItemSetString()}/{this.IsBigCraftable}/{this.SkillString}"
        ;

        /// <summary>
        ///     Gets or sets the <see cref="RecipeUnlockType" />.
        /// </summary>
        public RecipeUnlockType RecipeUnlockType { get; set; }

        /// <summary>
        ///     Gets or sets the required skill.
        /// </summary>
        public RequiredSkill RequiredSkill { get; set; }

        private string DefaultSkillRequirement => this.IsBigCraftable ? "null" : "l 0";

        private string SkillString => this.RequiredSkill?.ToString() ?? this.DefaultSkillRequirement;

        /// <summary>
        ///     Adds custom save-safe crafting recipes
        /// </summary>
        /// <param name="recipe">The recipe to add</param>
        /// <returns>The internal id assigned to this recipe</returns>
        public static string RegisterRecipe(CraftingRecipe recipe)
        {
            if (StardewValley.CraftingRecipe.craftingRecipes == null
                || !StardewValley.CraftingRecipe.craftingRecipes.Any())
            {
                throw new Exception(
                    "craftingRecipes is empty! This likely occurs if you try to register an item before AfterContentLoaded");
            }

            CraftingRecipes.Add(recipe);
            recipe.PrivateName = $"{recipe.Name}:{IdManager.AssignUniqueId(200)}";
            StardewValley.CraftingRecipe.craftingRecipes.Add(recipe.PrivateName, recipe.RecipeString);
            return recipe.PrivateName;
        }
    }
}