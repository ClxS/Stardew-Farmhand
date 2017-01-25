namespace Farmhand.Events.Arguments.Menus.InventoryMenuEvents
{
    using Farmhand.Events.Arguments.Common;

    using StardewValley;

    /// <summary>
    ///     The before item hover event args.
    /// </summary>
    public class BeforeItemHoverEventArgs : ReturnableEventArgs
    {
        private Item item;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BeforeItemHoverEventArgs" /> class.
        /// </summary>
        /// <param name="x">
        ///     The X hover coordinate.
        /// </param>
        /// <param name="y">
        ///     The Y hover coordinate.
        /// </param>
        /// <param name="heldItem">
        ///     The currently held item.
        /// </param>
        public BeforeItemHoverEventArgs(int x, int y, Item heldItem)
        {
            this.item = null;
            this.X = x;
            this.Y = y;
            this.HeldItem = heldItem;
        }

        /// <summary>
        ///     Gets the X hover coordinate.
        /// </summary>
        public int X { get; }

        /// <summary>
        ///     Gets the Y hover coordinate.
        /// </summary>
        public int Y { get; }

        /// <summary>
        ///     Gets the currently held item.
        /// </summary>
        public Item HeldItem { get; }

        /// <summary>
        ///     Gets or sets the hovered item.
        /// </summary>
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