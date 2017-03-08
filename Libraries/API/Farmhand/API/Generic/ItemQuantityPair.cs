namespace Farmhand.API.Generic
{
    /// <summary>
    ///     Used to define an item-quantity pairing.
    /// </summary>
    public class ItemQuantityPair
    {
        /// <summary>
        ///     Gets or sets the item ID.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        ///     Gets or sets the quantity of items.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///     Converts the pair into a game compatible string.
        /// </summary>
        /// <returns>
        ///     The requirement as a string.
        /// </returns>
        public override string ToString()
        {
            return $"{this.ItemId} {this.Count}";
        }
    }
}