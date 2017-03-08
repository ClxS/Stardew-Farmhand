namespace Farmhand.API.Generic
{
    /// <summary>
    ///     Used to define an item-chance pairing.
    /// </summary>
    public class ItemChancePair
    {
        /// <summary>
        ///     Gets or sets the probability value.
        /// </summary>
        public double Chance { get; set; }

        /// <summary>
        ///     Gets or sets the item ID.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        ///     Converts the pair into a game compatible string.
        /// </summary>
        /// <returns>
        ///     The requirement as a string.
        /// </returns>
        public override string ToString()
        {
            return $"{this.ItemId} {this.Chance}";
        }
    }
}