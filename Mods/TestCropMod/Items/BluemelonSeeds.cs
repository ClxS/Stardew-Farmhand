using Farmhand.API.Items;
using Farmhand.Registries;
using Microsoft.Xna.Framework;

namespace TestCropMod.Items
{
    public class BluemelonSeeds : StardewValley.Object
    {
        private static ItemInformation _information;
        public static ItemInformation Information => _information ?? (_information = new Farmhand.API.Items.ItemInformation
        {
            Name = "Bluemelon Seeds",
            Category = ItemCategory.Seeds,
            Description = "Plant these in the spring or summer or fall. Takes 4 days to mature.",
            Price = 10,
            Texture = TextureRegistry.GetModSpecificId(TestCropMod.Instance.ModSettings, "icon_BluemelonSeeds"),
            Type = ItemType.Basic
        });

        // A default constructor that calls the base default constructor is required for proper ID fixing.
        // Using the default constructor to create an object intended to be used is not recommended
        protected BluemelonSeeds()
            : base()
        {

        }

        public BluemelonSeeds(ItemInformation information)
            : base(Vector2.Zero, information.Id, information.Name, true, true, false, false)
        {

        }
    }
}
