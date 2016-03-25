using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StardewValley;

namespace Revolution.Events.Arguments.PlayerEvents
{
    public class EventArgsOnItemAddedToInventory : CancelEventArgs
    {
        public EventArgsOnItemAddedToInventory(Item item)
        {
            Item = item;
        }

        public Item Item { get; set; }
    }
}
