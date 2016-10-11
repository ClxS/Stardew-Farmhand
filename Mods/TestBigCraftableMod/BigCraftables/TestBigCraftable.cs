using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmhand.API.Items;
using Microsoft.Xna.Framework;

namespace TestBigCraftableMod.BigCraftables
{
    public class TestBigCraftable : BigCraftable
    {
        private static BigCraftableInformation _information;
        public static BigCraftableInformation StaticInformation => _information ?? (_information = new Farmhand.API.Items.BigCraftableInformation
        {
            Texture = TestBigCraftableMod.Instance.ModSettings.GetTexture("sprite_TestBigCraftable"),
            Name = "Test Big Craftable",
            Price = 1,
            Edibility = -300,
            Type = ItemType.Crafting,
            Category = ItemCategory.BigCraftable,
            Description = "This machine seems a little odd.",
            SetOutdoors = true,
            SetIndoors = true,
            Fragility = 0,
            IsLamp = false
        });

        public TestBigCraftable() :
            base(StaticInformation)
        {

        }
    }
}
