using Microsoft.Xna.Framework.Graphics;

namespace Farmhand.API.Items
{
    /// <summary>
    /// Contains general big craftables information
    /// </summary>
    public class BigCraftableInformation
    {
        // Big Craftable ID
        public int Id { get; set; }

        // Big Craftable Texture
        public Texture2D Texture { get; set; }

        // Big Craftable name
        public string Name { get; set; }

        // Big Craftable price
        public int Price { get; set; }

        // Big Craftable edibility
        public int Edibility { get; set; } = -300;

        // Big Craftable type
        public ItemType Type { get; set; }

        // Big Craftable category
        public ItemCategory Category { get; set; } = ItemCategory.None;

        // Description
        public string Description { get; set; } = "";

        // Can be set outdoors?
        public bool SetOutdoors { get; set; }

        // Can be set indoors?
        public bool SetIndoors { get; set; }

        // Fragility
        public int Fragility { get; set; } = 0;

        // Is it a lamp?
        public bool IsLamp { get; set; } = false;

        public override string ToString()
        {
            // combine the Type and Category, if there is a category
            string typeAndCategoryString = $"{Type}";
            if (Category != ItemCategory.None)
            {
                typeAndCategoryString += $" {(int)Category}";
            }

            return $"{Name}/{Price}/{Edibility}/{typeAndCategoryString}/{Description}/{SetOutdoors}/{SetIndoors}/{Fragility}/{IsLamp}";
        }
    }
}
