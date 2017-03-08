namespace Farmhand.Events.Arguments.MenuEvents
{
    using System;

    using StardewValley.Menus;

    /// <summary>
    ///     Arguments for MenuChanged.
    /// </summary>
    public class MenuChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MenuChangedEventArgs" /> class.
        /// </summary>
        /// <param name="priorMenu">
        ///     The prior menu.
        /// </param>
        /// <param name="newMenu">
        ///     The new menu.
        /// </param>
        public MenuChangedEventArgs(IClickableMenu priorMenu, IClickableMenu newMenu)
        {
            this.PriorMenu = priorMenu;
            this.NewMenu = newMenu;
        }

        /// <summary>
        ///     Gets the prior menu.
        /// </summary>
        public IClickableMenu PriorMenu { get; }

        /// <summary>
        ///     Gets the new menu.
        /// </summary>
        public IClickableMenu NewMenu { get; }
    }
}