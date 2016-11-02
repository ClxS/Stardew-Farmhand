using System;
using StardewValley;
using Farmhand.Events;
using Farmhand.Events.Arguments.MenuEvents;

namespace StardewModdingAPI.Events
{
    public static class MenuEvents
    {
        public static event EventHandler<EventArgsClickableMenuChanged> MenuChanged = delegate { };

        internal static void InvokeMenuChanged(object sender, EventArgsOnMenuChanged eventArgs)
        {
            EventCommon.SafeInvoke(MenuChanged, null, new EventArgsClickableMenuChanged(eventArgs.PriorMenu, eventArgs.NewMenu));
        }
    }
}