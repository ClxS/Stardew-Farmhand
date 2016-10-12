using Farmhand.API.Items;

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
