namespace Farmhand.Events.Arguments.PlayerEvents
{
    using Farmhand.Events.Arguments.Common;

    using StardewValley;

    /// <summary>
    ///     Arguments for ItemAddedToInventory.
    /// </summary>
    public class ItemAddedToInventoryEventArgs : ReturnableEventArgs
    {
        private Item item;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemAddedToInventoryEventArgs" /> class.
        /// </summary>
        /// <param name="item">
        ///     The item added to inventory.
        /// </param>
        public ItemAddedToInventoryEventArgs(Item item)
        {
            this.item = item;
        }

        /// <summary>
        ///     Gets or sets the item to add to inventory.
        /// </summary>
        /// <remarks>
        ///     Setting this item results in a different item being added to the player's inventory.
        /// </remarks>
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