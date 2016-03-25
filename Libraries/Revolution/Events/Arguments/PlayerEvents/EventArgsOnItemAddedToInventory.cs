using System.ComponentModel;
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
