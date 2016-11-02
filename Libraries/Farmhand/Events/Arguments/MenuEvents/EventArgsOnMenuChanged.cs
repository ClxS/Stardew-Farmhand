using System;
using StardewValley.Menus;

namespace Farmhand.Events.Arguments.MenuEvents
{
    public class EventArgsOnMenuChanged : EventArgs
    {
        public EventArgsOnMenuChanged(IClickableMenu priorMenu, IClickableMenu newMenu)
        {
            PriorMenu = priorMenu;
            NewMenu = newMenu;
        }

        public IClickableMenu PriorMenu { get; set; }
        public IClickableMenu NewMenu { get; set; }
    }
}
