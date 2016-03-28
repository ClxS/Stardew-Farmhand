using System;
using StardewValley;
using StardewValley.Menus;

namespace StardewModdingAPI.Events
{
    public static class MenuEvents
    {
        public static event EventHandler<EventArgsClickableMenuChanged> MenuChanged = delegate { };

        internal static void InvokeMenuChanged(object sender, EventArgs eventArgs)
        {
            MenuChanged.Invoke(null, new EventArgsClickableMenuChanged(null, Game1.activeClickableMenu));
        }
    }
}