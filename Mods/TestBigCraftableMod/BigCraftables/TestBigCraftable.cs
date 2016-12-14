using Farmhand.API.Items;
using Microsoft.Xna.Framework;
using StardewValley;

namespace TestBigCraftableMod.BigCraftables
{
    public class TestBigCraftable : Farmhand.Overrides.Game.Item.BigCraftable
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

        // A default constructor that calls the base default constructor is required for proper ID fixing.
        // Using the default constructor to create an object intended to be used is not recommended
        protected TestBigCraftable() :
            base()
        {

        }

        public TestBigCraftable(BigCraftableInformation information, Vector2 vector, bool isRecipe = false) :
            base(information, vector, isRecipe)
        {
            
        }

        public override bool clicked(Farmer who)
        {
            Farmhand.Logging.Log.Success("Test Big Craftable Overriding Clicked Event!");
            return base.clicked(who);
        }
    }
}
