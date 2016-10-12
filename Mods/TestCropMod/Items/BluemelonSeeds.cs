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

        public BluemelonSeeds()
            : base(Vector2.Zero, Information.Id, Information.Name, true, true, false, false)
        {

        }
    }
}
