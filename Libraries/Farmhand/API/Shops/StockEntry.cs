using Farmhand.API.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Shops
{
    // A data class containing all the information needed to add an item to a shop stock
    public class StockEntry
    {
        // The Item Id of this shop stock entry, if it is not -1 it will be used
        public int ItemId { get; set; } = -1;

        // The Big Craftable Id of this shop stock entry, if it is not -1 it will be used
        public int BigCraftableId { get; set; } = -1;

        // The ItemInformation of this shop stock entry, if it is an item it will not be null
        public ItemInformation ItemInformation { get; set; } = null;

        // The BigCraftableInformation of this shop stock entry, if it is a big craftable it will not be null
        public BigCraftableInformation BigCraftableInformation { get; set; } = null;

        // The price of the item, if applicable
        public int Price { get; set; } = 0;

        // The amount in stock, if applicable
        public int Stock { get; set; } = ShopUtilities.Infinite;

        // The delegate object used to check it this item should be added
        public ShopUtilities.CheckIfAddShopStock CheckDelegate { get; set; } = new ShopUtilities.CheckIfAddShopStock(DummyAdd);

        public StockEntry(ItemInformation itemInformation, int price = 0, int stock = ShopUtilities.Infinite)
        {
            ItemInformation = itemInformation;
            Price = price;
            Stock = stock;
        }

        public StockEntry(ItemInformation itemInformation, ShopUtilities.CheckIfAddShopStock checkDelegate, int price = 0, int stock = ShopUtilities.Infinite)
        {
            ItemInformation = itemInformation;
            CheckDelegate = checkDelegate;
            Price = price;
            Stock = stock;
        }

        public StockEntry(BigCraftableInformation bigCraftableInformation, int price = 0, int stock = ShopUtilities.Infinite)
        {
            BigCraftableInformation = bigCraftableInformation;
            Price = price;
            Stock = stock;
        }

        public StockEntry(BigCraftableInformation bigCraftableInformation, ShopUtilities.CheckIfAddShopStock checkDelegate, int price = 0, int stock = ShopUtilities.Infinite)
        {
            BigCraftableInformation = bigCraftableInformation;
            CheckDelegate = checkDelegate;
            Price = price;
            Stock = stock;
        }

        public StockEntry(StockType stockType, int Id, int price = 0, int stock = ShopUtilities.Infinite)
        {
            if (stockType == StockType.Item)
            {
                ItemId = Id;
            }
            else if (stockType == StockType.BigCraftable)
            {
                BigCraftableId = Id;
            }
            else
            {
                ItemId = Id;
            }

            Price = price;
            Stock = stock;
        }

        public StockEntry(StockType stockType, int Id, ShopUtilities.CheckIfAddShopStock checkDelegate, int price = 0, int stock = ShopUtilities.Infinite)
        {
            if(stockType == StockType.Item)
            {
                ItemId = Id;
            }
            else if(stockType == StockType.BigCraftable)
            {
                BigCraftableId = Id;
            }
            else
            {
                ItemId = Id;
            }

            CheckDelegate = checkDelegate;
            Price = price;
            Stock = stock;
        }

        // Check if this item should be added to the shop stock
        public bool DoAdd()
        {
            return CheckDelegate();
        }

        // Check if this is using ItemId
        public bool UsesItemId()
        {
            return (ItemId != -1);
        }

        public bool UsesBigCraftableId()
        {
            return (BigCraftableId != -1);
        }

        // Check if this is using ItemInformation
        public bool UsesItemInformation()
        {
            return (ItemInformation != null);
        }

        // Check if this is using BigCraftableInformation
        public bool UsesBigCraftableInformation()
        {
            return (BigCraftableInformation != null);
        }

        // A dummy method which will always return true. A default value used when no delegate is given to check for special conditions
        static bool DummyAdd()
        {
            return true;
        }
    }
}
