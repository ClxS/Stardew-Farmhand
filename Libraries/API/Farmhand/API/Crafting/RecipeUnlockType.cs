namespace Farmhand.API.Crafting
{
    /// <summary>
    ///     An enumeration of recipe unlock types.
    /// </summary>
    public enum RecipeUnlockType
    {
        /// <summary>
        ///     The recipe is automatically unlocked on reaching a certain skill level.
        ///     (TODO: Not yet implemented)
        /// </summary>
        SkillBased,

        /// <summary>
        ///     The recipe is manually added to the player by a mod.
        /// </summary>
        Manual
    }
}