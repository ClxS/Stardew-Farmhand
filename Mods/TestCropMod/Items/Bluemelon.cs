using Farmhand.API.Items;
using Farmhand.Registries;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Type = ItemType.Food,
            Editibility = 15
        });

        public Bluemelon()
            : base(Vector2.Zero, Information.Id, Information.Name, true, true, false, false)
        {
            
        }
    }
}
