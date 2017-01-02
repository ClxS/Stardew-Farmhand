namespace Farmhand.Overrides
{
    using System.Collections.Generic;

    using Farmhand.Attributes;

    using StardewValley;

    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    internal class ShopMenu : StardewValley.Menus.ShopMenu
    {
        public ShopMenu(Dictionary<Item, int[]> itemPriceAndStock, int currency = 0, string who = null)
            : base(itemPriceAndStock, currency, who)
        {
        }
    }
}