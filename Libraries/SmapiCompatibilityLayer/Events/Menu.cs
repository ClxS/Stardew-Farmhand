using System;
using StardewValley;
using StardewValley.Menus;
using Farmhand.Events;

namespace StardewModdingAPI.Events
{
    public static class MenuEvents
    {
        public static event EventHandler<EventArgsClickableMenuChanged> MenuChanged = delegate { };

        internal static void InvokeMenuChanged(object sender, EventArgs eventArgs)
        {
            EventCommon.SafeInvoke(MenuChanged, null, new EventArgsClickableMenuChanged(null, Game1.activeClickableMenu));
        }
    }
}