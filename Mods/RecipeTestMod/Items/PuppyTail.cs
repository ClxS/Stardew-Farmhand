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

        public PuppyTail()
            : base(Vector2.Zero, Information.Id, Information.Name, true, true, false, false)
        {
            Farmhand.Logging.Log.Success("Using PuppyTail Class");
        }
    }
}
