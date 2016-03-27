using System;
using StardewValley.Menus;

namespace StardewModdingAPI.Events
{
    public class EventArgsClickableMenuChanged : EventArgs
    {
        public EventArgsClickableMenuChanged(IClickableMenu priorMenu, IClickableMenu newMenu)
        {
            NewMenu = newMenu;
            PriorMenu = priorMenu;
        }
        public IClickableMenu NewMenu { get; private set; }
        public IClickableMenu PriorMenu { get; private set; }
    }
}