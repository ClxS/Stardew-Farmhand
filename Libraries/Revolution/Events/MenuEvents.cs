using Revolution.Attributes;
using StardewValley.Menus;
using System;

namespace Revolution.Events
{
    public class MenuEvents
    {
        public static event EventHandler OnMenuChanged = delegate { };
        
        [PendingHook]
        internal static void InvokeMenuChanged(IClickableMenu priorMenu, IClickableMenu newMenu)
        {
            EventCommon.SafeInvoke(OnMenuChanged, null);
        }
    }
}
