using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Shops
{
    public class ShopInformation
    {
        // The name of this shop
        public string Name { get; set; }

        // The owning mod of this shop
        public Mod Owner { get; set; }

        // The stock available in this shop
        public List<StockEntry> Stock { get; } = new List<StockEntry>();

        // Type of currency used in this store
        public int CurrencyType { get; set; } = 0;

        public ShopInformation(Mod owner, string name, int currency = 0)
        {
            Owner = owner;
            Name = name;
            CurrencyType = currency;
        }

        public string UniqueModId()
        {
            return ShopUtilities.GetInternalShopName(Owner, Name);
        }
    }
}
