using StardewValley.Menus;
using System;

// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Events
{
    public static class MenuEvents
    {
        public static event EventHandler<EventArgsClickableMenuChanged> MenuChanged = delegate { };

        public static void InvokeMenuChanged(IClickableMenu priorMenu, IClickableMenu newMenu)
        {
            MenuChanged.Invoke(null, new EventArgsClickableMenuChanged(priorMenu, newMenu));
        }
    }
}
