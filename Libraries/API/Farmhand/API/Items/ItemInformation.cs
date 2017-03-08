namespace Farmhand.API.Items
{
    /// <summary>
    ///     Contains general item information
    /// </summary>
    public class ItemInformation
    {
        /// <summary>
        ///     Gets or sets the internal ID for this item.
        ///     <remarks>
        ///         This is assigned by the API so you should not modify this value.
        ///     </remarks>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public ItemType Type { get; set; }

        /// <summary>
        ///     Gets or sets the category.
        /// </summary>
        public ItemCategory Category { get; set; } = ItemCategory.None;

        /// <summary>
        ///     Gets or sets the price.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        ///     Gets or sets the texture.
        /// </summary>
        public string Texture { get; set; }

        /// <summary>
        ///     Gets or sets the edibility.
        /// </summary>
        public int Edibility { get; set; } = -300;

        /// <summary>
        ///     Returns a game compatible <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            // combine the Type and Category, if there is a category
            string typeAndCategoryString = $"{this.Type}";
            if (this.Category != ItemCategory.None)
            {
                typeAndCategoryString += $" {(int)this.Category}";
            }

            return $"{this.Name}/{this.Price}/{this.Edibility}/{typeAndCategoryString}/{this.Description}";
        }
    }
}