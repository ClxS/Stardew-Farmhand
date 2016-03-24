using Revolution.Attributes;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
