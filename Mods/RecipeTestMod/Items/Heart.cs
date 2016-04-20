using Farmhand.API.Items;
using Farmhand.Registries;
using Microsoft.Xna.Framework;

namespace RecipeTestMod.Items
{
    public class Heart : StardewValley.Object
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

        public Heart()
            : base(Vector2.Zero, Information.Id, Information.Name, true, true, false, false)
        {
            Farmhand.Logging.Log.Success("Using Heart Class");
        }
    }
}
