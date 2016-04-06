using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StardewValley;

namespace Farmhand.API
{
    /// <summary>
    /// API layer to interact with the player
    /// </summary>
    public static class Player
    {
        /// <summary>
        /// Contains all custom crafting recipes added to the API
        /// </summary>
        public static List<CraftingRecipe> CraftingRecipes = new List<CraftingRecipe>();
         
        /// <summary>
        /// Adds custom save-safe crafting recipes
        /// </summary>
        /// <param name="recipe">The recipe to add</param>
        /// <param name="farmer">The farmer to add. Will default to Game1.player when null</param>
        /// <returns>The internal id assigned to this recipe</returns>
        public static int AddRecipe(CraftingRecipe recipe, Farmer farmer = null)
        {
            if (farmer == null)
            {
                farmer = Game1.player;
            }
            
            return 0;
        }
    }
}
