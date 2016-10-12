namespace Farmhand.API.Items
{
    /// <summary>
    /// Contains general item information
    /// </summary>
    public class ItemInformation
    {
        public int Id { get; internal set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public ItemCategory Category { get; set; } = ItemCategory.None;
        public int Price { get; set; }
        public string Texture { get; set; }
        public int Edibility { get; set; } = -300;

        public override string ToString()
        {
            // combine the Type and Category, if there is a category
            string TypeAndCategoryString = $"{Type}";
            if(Category != ItemCategory.None)
            {
                TypeAndCategoryString += $" {(int)Category}";
            }

            return $"{Name}/{Price}/{Edibility}/{TypeAndCategoryString}/{Description}";
        }
    }
}
