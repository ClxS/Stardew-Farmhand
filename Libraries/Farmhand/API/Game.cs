namespace Farmhand.API
{
    using System;
    using System.Linq;

    using Farmhand.Extensibility;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    /// Game-related API Functionality
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// Gets or sets the current active clickable menu.
        /// </summary>
        public static IClickableMenu ActiveClickableMenu
        {
            get
            {
                return Game1.activeClickableMenu;
            }
            set
            {
                Game1.activeClickableMenu = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether has loaded game.
        /// </summary>
        public static bool HasLoadedGame
        {
            get
            {
                return Game1.hasLoadedGame;
            }
        }

        /// <summary>
        /// Gets the current event.
        /// </summary>
        public static Event CurrentEvent
        {
            get
            {
                return Game1.CurrentEvent;
            }
        }

        internal static Game1 CreateGameInstance()
        {
            var overridingExtension = ExtensibilityManager.Extensions.FirstOrDefault(e => e.GameOverrideClass != null);
            if (overridingExtension != null)
            {
                return (Game1)Activator.CreateInstance(overridingExtension.GameOverrideClass);
            }

            return new Game1();
        }
    }
}
