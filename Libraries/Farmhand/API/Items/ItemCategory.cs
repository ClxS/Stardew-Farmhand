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

        Greens = object.GreensCategory,

        Gem = object.GemCategory,

        Vegetable = object.VegetableCategory,

        Fish = object.FishCategory,

        Egg = object.EggCategory,

        Milk = object.MilkCategory,

        Cooking = object.CookingCategory,

        Crafting = object.CraftingCategory,

        BigCraftable = object.BigCraftableCategory,

        Fruit = object.FruitsCategory,

        Seeds = object.SeedsCategory,

        Minerals = object.mineralsCategory,

        Flowers = object.flowersCategory,

        Meat = object.meatCategory,

        Metal = object.metalResources,

        Building = object.buildingResources,

        SellAtPierres = object.sellAtPierres,

        SellAtPierresAndMarnies = object.sellAtPierresAndMarnies,

        Fertilizer = object.fertilizerCategory,

        Junk = object.junkCategory,

        Bait = object.baitCategory,

        Tackle = object.tackleCategory,

        SellAtFishShop = object.sellAtFishShopCategory,

        Furniture = object.furnitureCategory,

        Ingredient = object.ingredientsCategory,

        ArtisanGoods = object.artisanGoodsCategory,

        Syrup = object.syrupCategory,

        MonsterLoot = object.monsterLootCategory,

        Equipment = object.equipmentCategory,

        Hat = object.hatCategory,

        Ring = object.ringCategory,

        Weapon = object.weaponCategory,

        Boots = object.bootsCategory,

        Tool = object.toolCategory
    }
}