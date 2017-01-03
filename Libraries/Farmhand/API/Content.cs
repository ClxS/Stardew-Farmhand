namespace Farmhand.API
{
    using Farmhand.Content;

    using StardewValley;

    /// <summary>
    ///     Content-related API functionality.
    /// </summary>
    public static class Content
    {
        /// <summary>
        ///     Gets the game's <see cref="ContentManager" />.
        /// </summary>
        public static ContentManager ContentManager => Game1.content as ContentManager;
    }
}