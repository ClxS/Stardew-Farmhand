using Farmhand.Attributes;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Overrides
{
    [HookForceVirtualBase]
    [HookAlterBaseFieldProtection(LowestProtection.Protected)]
    class ShopMenu : StardewValley.Menus.ShopMenu
    {
        public ShopMenu(Dictionary<Item, int[]> itemPriceAndStock, int currency = 0, string who = null) : base(itemPriceAndStock, currency, who)
        {
        }
    }
}
