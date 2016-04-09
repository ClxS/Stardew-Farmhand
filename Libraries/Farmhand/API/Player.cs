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
        /// Adds a recipe to the provided player
        /// </summary>
        /// <param name="name">The name of the recipe to enable</param>
        /// <param name="player">The player. Defaults to null. If null, will use Game1.player</param>
        public static void AddRecipe(string name, Farmer player = null)
        {
            if (player == null)
            {
                player = Game1.player;
            }

            player.craftingRecipes.Add(name, 1);
        }
    }
}
