using Farmhand.API.Items;
using Farmhand.Registries;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public RabbitsPaw()
            : base(Vector2.Zero, Information.Id, Information.Name, true, true, false, false)
        {
            Farmhand.Logging.Log.Success("Using RabbitsPaw Class");
        }
    }
}
