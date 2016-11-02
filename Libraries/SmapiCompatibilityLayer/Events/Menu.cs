using System;
using StardewValley;
using Farmhand.Events;
using Farmhand.Events.Arguments.MenuEvents;

namespace StardewModdingAPI.Events
{
    public static class MenuEvents
    {
        public static event EventHandler<EventArgsClickableMenuChanged> MenuChanged = delegate { };
        public static event EventHandler<EventArgsClickableMenuClosed> MenuClosed = delegate { };

        internal static void InvokeMenuChanged(object sender, EventArgsOnMenuChanged eventArgs)
        {
            if ( eventArgs.PriorMenu != null && eventArgs.NewMenu == null)
                EventCommon.SafeInvoke(MenuClosed, null, new EventArgsClickableMenuClosed(eventArgs.PriorMenu));
            else
                EventCommon.SafeInvoke(MenuChanged, null, new EventArgsClickableMenuChanged(eventArgs.PriorMenu, eventArgs.NewMenu));
        }
    }
}