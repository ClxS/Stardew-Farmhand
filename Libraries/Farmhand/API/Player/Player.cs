﻿using Microsoft.Xna.Framework;
using StardewValley;

namespace Farmhand.API.Player
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

            if(player.craftingRecipes.ContainsKey(name))
            {
                player.craftingRecipes[name] = 1;
            }
            else
            {
                player.craftingRecipes.Add(name, 1);
            }
        }

        public static void AddObject(int id, Farmer player = null)
        {
            if (player == null)
            {
                player = Game1.player;
            }
            
            player.addItemToInventory(new Object(Vector2.Zero, id, "", true, true, false, false));
        }

        public static void AddObject<T>(Farmer player = null) where T : StardewValley.Object, new()
        {
            if (player == null)
            {
                player = Game1.player;
            }

            player.addItemToInventory(new T());
        }

        public static void AddObject(StardewValley.Object @object, Farmer player = null)
        {
            if (player == null)
            {
                player = Game1.player;
            }

            player.addItemToInventory(@object);
        }

        public static void AddTool<T>(Farmer player = null) where T : StardewValley.Tool, new()
        {
            if (player == null)
            {
                player = Game1.player;
            }

            player.addItemToInventory(new T());
        }

        public static void AddTool(StardewValley.Tool tool, Farmer player = null)
        {
            if (player == null)
            {
                player = Game1.player;
            }

            player.addItemToInventory(tool);
        }
    }
}
