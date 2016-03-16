using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    class MenuEvents
    {
        public static event EventHandler MenuChanged = delegate { };

        public static void InvokeMenuChanged(IClickableMenu priorMenu, IClickableMenu newMenu)
        {
            MenuChanged.Invoke(null, EventArgs.Empty);
        }
    }
}
