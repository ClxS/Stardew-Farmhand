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
            Category = ItemCategory.None,
            Description = "Extracted from a particularly naughty child",
            Price = 3000,
            Texture = TextureRegistry.GetModSpecificId(RecipeTestMod.Instance.ModSettings, "icon_Heart"),
            Type = ItemType.Basic
        });

        // A default constructor that calls the base default constructor is required for proper ID fixing.
        // Using the default constructor to create an object intended to be used is not recommended
        protected Heart()
            : base()
        {
            Farmhand.Logging.Log.Success("Deserializing Heart Class");
        }

        public Heart(ItemInformation information)
            : base(Vector2.Zero, information.Id, information.Name, true, true, false, false)
        {
            Farmhand.Logging.Log.Success("Using Heart Class");
        }
    }
}
