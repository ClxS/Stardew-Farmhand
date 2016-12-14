using Farmhand.API.Items;
using Farmhand.Registries;
using Microsoft.Xna.Framework;

namespace RecipeTestMod.Items
{
    public class RabbitsPaw : StardewValley.Object
    {
        private static ItemInformation _information;
        public static ItemInformation Information => _information ?? (_information = new Farmhand.API.Items.ItemInformation
        {
            Name = "Rabbit's Paw",
            Category = ItemCategory.None,
            Description = "Poor Thumper...",
            Price = 3000,
            Texture = TextureRegistry.GetModSpecificId(RecipeTestMod.Instance.ModSettings, "icon_Rabbit"),
            Type = ItemType.Basic
        });

        // A default constructor that calls the base default constructor is required for proper ID fixing.
        // Using the default constructor to create an object intended to be used is not recommended
        protected RabbitsPaw()
            : base()
        {
            Farmhand.Logging.Log.Success("Deserializing RabbitsPaw Class");
        }

        public RabbitsPaw(ItemInformation information)
            : base(Vector2.Zero, information.Id, information.Name, true, true, false, false)
        {
            Farmhand.Logging.Log.Success("Using RabbitsPaw Class");
        }
    }
}
