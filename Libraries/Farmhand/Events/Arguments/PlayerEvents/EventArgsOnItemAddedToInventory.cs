namespace Farmhand.Events.Arguments.PlayerEvents
{
    using StardewValley;

    public class EventArgsOnItemAddedToInventory : ReturnableEventArgs
    {
        private Item item;

        public EventArgsOnItemAddedToInventory(Item item)
        {
            this.item = item;
        }

        public Item Item
        {
            get
            {
                return this.item;
            }

            set
            {
                this.item = value;
                this.IsHandled = true;
            }
        }
    }
}