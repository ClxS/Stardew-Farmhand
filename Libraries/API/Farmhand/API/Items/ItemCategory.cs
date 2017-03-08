namespace Farmhand.API.Items
{
    using StardewValley;

    // ReSharper disable StyleCop.SA1602

    /// <summary>
    ///     Category of the item
    /// </summary>
    public enum ItemCategory
    {
        None = 0,

        Greens = Object.GreensCategory,

        Gem = Object.GemCategory,

        Vegetable = Object.VegetableCategory,

        Fish = Object.FishCategory,

        Egg = Object.EggCategory,

        Milk = Object.MilkCategory,

        Cooking = Object.CookingCategory,

        Crafting = Object.CraftingCategory,

        BigCraftable = Object.BigCraftableCategory,

        Fruit = Object.FruitsCategory,

        Seeds = Object.SeedsCategory,

        Minerals = Object.mineralsCategory,

        Flowers = Object.flowersCategory,

        Meat = Object.meatCategory,

        Metal = Object.metalResources,

        Building = Object.buildingResources,

        SellAtPierres = Object.sellAtPierres,

        SellAtPierresAndMarnies = Object.sellAtPierresAndMarnies,

        Fertilizer = Object.fertilizerCategory,

        Junk = Object.junkCategory,

        Bait = Object.baitCategory,

        Tackle = Object.tackleCategory,

        SellAtFishShop = Object.sellAtFishShopCategory,

        Furniture = Object.furnitureCategory,

        Ingredient = Object.ingredientsCategory,

        ArtisanGoods = Object.artisanGoodsCategory,

        Syrup = Object.syrupCategory,

        MonsterLoot = Object.monsterLootCategory,

        Equipment = Object.equipmentCategory,

        Hat = Object.hatCategory,

        Ring = Object.ringCategory,

        Weapon = Object.weaponCategory,

        Boots = Object.bootsCategory,

        Tool = Object.toolCategory
    }
}