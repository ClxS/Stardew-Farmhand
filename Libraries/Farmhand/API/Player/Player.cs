namespace Farmhand.API.Player
{
    using Microsoft.Xna.Framework;

    using StardewValley;

    /// <summary>
    ///     API layer to interact with the player
    /// </summary>
    public static class Player
    {
        /// <summary>
        ///     Adds a recipe to the provided player
        /// </summary>
        /// <param name="name">The name of the recipe to enable</param>
        /// <param name="player">The player. Defaults to null. If null, will use Game1.player</param>
        public static void AddRecipe(string name, Farmer player = null)
        {
            if (player == null)
            {
                player = Game1.player;
            }

            if (player.craftingRecipes.ContainsKey(name))
            {
                player.craftingRecipes[name] = 1;
            }
            else
            {
                player.craftingRecipes.Add(name, 1);
            }
        }

        /// <summary>
        ///     Adds an object to the player's inventory.
        /// </summary>
        /// <param name="id">
        ///     The item's ID.
        /// </param>
        /// <param name="player">
        ///     The player to give the item.
        /// </param>
        public static void AddObject(int id, Farmer player = null)
        {
            if (player == null)
            {
                player = Game1.player;
            }

            // TODO: Make this use the Object factory
            player.addItemToInventory(new Object(Vector2.Zero, id, string.Empty, true, true, false, false));
        }

        /// <summary>
        ///     Adds an object to the player's inventory.
        /// </summary>
        /// <param name="player">
        ///     The player to give the item.
        /// </param>
        /// <typeparam name="T">
        ///     The type of item to give the player.
        /// </typeparam>
        public static void AddObject<T>(Farmer player = null) where T : Object, new()
        {
            if (player == null)
            {
                player = Game1.player;
            }

            player.addItemToInventory(new T());
        }

        /// <summary>
        ///     Adds an object to the player's inventory.
        /// </summary>
        /// <param name="object">
        ///     The object to give the player.
        /// </param>
        /// <param name="player">
        ///     The player to give the item.
        /// </param>
        public static void AddObject(Object @object, Farmer player = null)
        {
            if (player == null)
            {
                player = Game1.player;
            }

            player.addItemToInventory(@object);
        }

        /// <summary>
        ///     Adds a tool to the player's inventory.
        /// </summary>
        /// <param name="player">
        ///     The player to give the tool.
        /// </param>
        /// <typeparam name="T">
        ///     The tool to give the player.
        /// </typeparam>
        public static void AddTool<T>(Farmer player = null) where T : Tool, new()
        {
            if (player == null)
            {
                player = Game1.player;
            }

            player.addItemToInventory(new T());
        }

        /// <summary>
        ///     Adds a tool to the player's inventory.
        /// </summary>
        /// <param name="tool">
        ///     The tool to give the player.
        /// </param>
        /// <param name="player">
        ///     The player to give the tool.
        /// </param>
        public static void AddTool(Tool tool, Farmer player = null)
        {
            if (player == null)
            {
                player = Game1.player;
            }

            player.addItemToInventory(tool);
        }
    }
}