namespace Farmhand.API.Locations
{
    using StardewValley;

    /// <summary>
    /// Extension methods to help with NPCs.
    /// </summary>
    public static class GameLocationExtensionMethods
    {
        /// <summary>
        /// Adds a character to the map.
        /// </summary>
        /// <param name="this">
        /// The location to add the character to.
        /// </param>
        /// <param name="npc">
        /// The NPC to add.
        /// </param>
        public static void AddCharacter(this GameLocation @this, NPC npc)
        {
            @this.addCharacter(npc);
        }
    }
}