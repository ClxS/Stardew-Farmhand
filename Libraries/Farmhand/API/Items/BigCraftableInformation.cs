namespace Farmhand.API.Items
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///     Contains general big craftables information
    /// </summary>
    public class BigCraftableInformation
    {
        /// <summary>
        ///     Gets or sets the category.
        /// </summary>
        public ItemCategory Category { get; set; } = ItemCategory.None;

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the edibility.
        /// </summary>
        public int Edibility { get; set; } = -300;

        /// <summary>
        ///     Gets or sets the fragility.
        /// </summary>
        public int Fragility { get; set; } = 0;

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <remarks>
        ///     You should not assign to this. It is only settable for the game's XmlSerializer compatibility. It is assigned by
        ///     the API after registering the item.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the item is a lamp.
        /// </summary>
        public bool IsLamp { get; set; } = false;

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the price.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether can be placed indoors.
        /// </summary>
        public bool SetIndoors { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether can be placed outdoors.
        /// </summary>
        public bool SetOutdoors { get; set; }

        /// <summary>
        ///     Gets or sets the texture.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        ///     Gets or sets the item type.
        /// </summary>
        public ItemType Type { get; set; }

        /// <summary>
        ///     Converts the information into a game compatible string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            // combine the Type and Category, if there is a category
            string typeAndCategoryString = $"{this.Type}";
            if (this.Category != ItemCategory.None)
            {
                typeAndCategoryString += $" {(int)this.Category}";
            }

            return
                $"{this.Name}/{this.Price}/{this.Edibility}/{typeAndCategoryString}/{this.Description}/{this.SetOutdoors}/{this.SetIndoors}/{this.Fragility}/{this.IsLamp}";
        }
    }
}