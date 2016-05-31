using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.API.Items;
using Farmhand.Registries;

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
            Texture = TextureRegistry.GetModSpecificId(RecipeTestMod.Instance.ModSettings, "icon_Heart"),
            Type = ItemType.Basic
        });
    }
}
