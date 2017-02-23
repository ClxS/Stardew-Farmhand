namespace Farmhand.Events
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.MenuEvents;

    using StardewValley.Menus;

    /// <summary>
    ///     Contains events relating to menus
    /// </summary>
    public static class MenuEvents
    {
        /// <summary>
        ///     Fires just after the menu changes.
        /// </summary>
        /// <remarks>
        ///     TODO: Not yet implemented
        /// </remarks>
        public static event EventHandler<MenuChangedEventArgs> MenuChanged = delegate { };

        /// <summary>
        ///     Fires just after showing end of night menu.
        /// </summary>
        public static event EventHandler ShowEndOfNightMenus = delegate { };

        [PendingHook]
        internal static void OnMenuChanged(IClickableMenu priorMenu, IClickableMenu newMenu)
        {
            EventCommon.SafeInvoke(MenuChanged, null, new MenuChangedEventArgs(priorMenu, newMenu));
        }


        [Hook(HookType.Exit, "StardewValley.Game1", "showEndOfNightStuff")]
        internal static void OnShowEndOfNightMenus()
        {
            EventCommon.SafeInvoke(ShowEndOfNightMenus, null);
        }
    }
}