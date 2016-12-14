using Farmhand.API.Items;
using Farmhand.Registries;
using Microsoft.Xna.Framework;

namespace RecipeTestMod.Items
{
    public class PuppyTail : StardewValley.Object
    {
        private static ItemInformation _information;
        public static ItemInformation Information => _information ?? (_information = new Farmhand.API.Items.ItemInformation
        {
            Name = "Puppy Tail",
            Category = ItemCategory.None,
            Description = "Hopefully Organic!",
            Price = 3000,
            Texture = TextureRegistry.GetModSpecificId(RecipeTestMod.Instance.ModSettings, "icon_Puppy"),
            Type = ItemType.Basic
        });

        // A default constructor that calls the base default constructor is required for proper ID fixing.
        // Using the default constructor to create an object intended to be used is not recommended
        protected PuppyTail()
            : base()
        {
            Farmhand.Logging.Log.Success("Deserializing PuppyTail Class");
        }

        public PuppyTail(ItemInformation information)
            : base(Vector2.Zero, information.Id, information.Name, true, true, false, false)
        {
            Farmhand.Logging.Log.Success("Using PuppyTail Class");
        }
    }
}
