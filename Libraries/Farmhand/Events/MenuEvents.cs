using Farmhand.Attributes;
using StardewValley.Menus;
using System;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events relating to menus
    /// </summary>
    public static class MenuEvents
    {
        public static event EventHandler OnMenuChanged = delegate { };
        
        [PendingHook]
        internal static void InvokeMenuChanged(IClickableMenu priorMenu, IClickableMenu newMenu)
        {
            EventCommon.SafeInvoke(OnMenuChanged, null);
        }
    }
}
