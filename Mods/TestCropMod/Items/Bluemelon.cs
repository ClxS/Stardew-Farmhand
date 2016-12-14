using Farmhand.API.Items;
using Farmhand.Registries;
using Microsoft.Xna.Framework;

namespace TestCropMod.Items
{
    public class Bluemelon : StardewValley.Object
    {
        private static ItemInformation _information;
        public static ItemInformation Information => _information ?? (_information = new Farmhand.API.Items.ItemInformation
        {
            Name = "Bluemelon",
            Category = ItemCategory.Fruit,
            Description = "A shiny, tasty, simple melon. It's also blue.",
            Price = 30,
            Texture = TextureRegistry.GetModSpecificId(TestCropMod.Instance.ModSettings, "icon_Bluemelon"),
            Type = ItemType.Basic,
            Edibility = 15
        });

        // A default constructor that calls the base default constructor is required for proper ID fixing.
        // Using the default constructor to create an object intended to be used is not recommended
        protected Bluemelon()
            : base()
        {

        }

        public Bluemelon(ItemInformation information)
            : base(Vector2.Zero, information.Id, information.Name, true, true, false, false)
        {
            
        }
    }
}
