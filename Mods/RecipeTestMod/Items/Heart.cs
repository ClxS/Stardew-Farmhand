using Farmhand.API.Items;
using Farmhand.Registries;

namespace RecipeTestMod.Items
{
    class Heart : StardewValley.Object
    {
        private static ItemInformation _information;
        public static ItemInformation Information => _information ?? (_information = new Farmhand.API.Items.ItemInformation
        {
            Name = "Heart",
            Category = ItemCategory.Basic,
            Description = "Extracted from a particularly naughty child",
            Price = 3000,
            Texture = TextureRegistry.GetModSpecificId(RecipeTestMod.Instance.ModSettings, "icon_Heart"),
            Type = ItemType.Other
        });
    }
}
