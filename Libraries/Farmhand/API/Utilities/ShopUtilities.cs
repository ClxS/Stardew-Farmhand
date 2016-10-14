using Farmhand.API.Items;
using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Utilities
{
    // Shop constants, for identifying which shop is desired
    public enum Shops
    {
        Unknown,
        Pierre,
        TravelingMerchant,
        Dwarf,
        Hospital,
        Qi,
        Joja,
        Hat,
        Fish,
        Animal,
        Sandy,
        Saloon,
        Adventure,
        Carpenter,
        Blacksmith,
        IceCream,
        Sewer,
        FreeWallpapersAndFloors,
        FreeFurnitures
    }

    public class ShopUtilities
    {
        // The delegate type which will control whether or not a given item will get added to the shop stock
        public delegate bool CheckIfAddShopStock();

        // ShopMenu's concept of infinite
        public const int Infinite = StardewValley.Menus.ShopMenu.infiniteStock;

        // Shop stock entry lists, one for each shop
        private static List<StockEntry> PierreShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> TravelingMerchantShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> DwarfShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> HospitalShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> QiShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> JojaShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> HatShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> FishShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> AnimalShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> SandyShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> SaloonShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> AdventureShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> CarpenterShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> BlacksmithShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> IceCreamShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> SewerShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> FreeWallpapersAndFloorsShopStock { get; } = new List<StockEntry>();
        private static List<StockEntry> FreeFurnituresShopStock { get; } = new List<StockEntry>();


        public static void AddToShopStock(Shops shop, ItemInformation item, int price = 0, int stock = Infinite)
        {
            AddToShopStock(shop, new StockEntry(item, price, stock));
        }

        public static void AddToShopStock(Shops shop, ItemInformation item, CheckIfAddShopStock checkDelegate, int price = 0, int stock = Infinite)
        {
            AddToShopStock(shop, new StockEntry(item, checkDelegate, price, stock));
        }

        public static void AddToShopStock(Shops shop, BigCraftableInformation item, int price = 0, int stock = Infinite)
        {
            AddToShopStock(shop, new StockEntry(item, price, stock));
        }

        public static void AddToShopStock(Shops shop, BigCraftableInformation item, CheckIfAddShopStock checkDelegate, int price = 0, int stock = Infinite)
        {
            AddToShopStock(shop, new StockEntry(item, checkDelegate, price, stock));
        }

        private static void AddToShopStock(Shops shop, StockEntry newEntry)
        {
            switch (shop)
            {
                case Shops.Pierre: { PierreShopStock.Add(newEntry); break; }
                case Shops.TravelingMerchant: { TravelingMerchantShopStock.Add(newEntry); break; }
                case Shops.Dwarf: { DwarfShopStock.Add(newEntry); break; }
                case Shops.Hospital: { HospitalShopStock.Add(newEntry); break; }
                case Shops.Qi: { QiShopStock.Add(newEntry); break; }
                case Shops.Joja: { JojaShopStock.Add(newEntry); break; }
                case Shops.Hat: { HatShopStock.Add(newEntry); break; }
                case Shops.Fish: { FishShopStock.Add(newEntry); break; }
                case Shops.Animal: { AnimalShopStock.Add(newEntry); break; }
                case Shops.Sandy: { SandyShopStock.Add(newEntry); break; }
                case Shops.Saloon: { SaloonShopStock.Add(newEntry); break; }
                case Shops.Adventure: { AdventureShopStock.Add(newEntry); break; }
                case Shops.Carpenter: { CarpenterShopStock.Add(newEntry); break; }
                case Shops.Blacksmith: { BlacksmithShopStock.Add(newEntry); break; }
                case Shops.IceCream: { IceCreamShopStock.Add(newEntry); break; }
                case Shops.Sewer: { SewerShopStock.Add(newEntry); break; }
                case Shops.FreeWallpapersAndFloors: { FreeWallpapersAndFloorsShopStock.Add(newEntry); break; }
                case Shops.FreeFurnitures: { FreeFurnituresShopStock.Add(newEntry); break; }
            }
        }

        public static Dictionary<StardewValley.Item, int[]> GetNewStock(Shops shop)
        {
            List<StockEntry> stockItems = null;
            Dictionary<StardewValley.Item, int[]> dictionaryToInject = new Dictionary<StardewValley.Item, int[]>(); ;

            switch (shop)
            {
                case Shops.Pierre: { stockItems =  PierreShopStock; break; }
                case Shops.TravelingMerchant: { stockItems = TravelingMerchantShopStock; break; }
                case Shops.Dwarf: { stockItems = DwarfShopStock; break; }
                case Shops.Hospital: { stockItems = HospitalShopStock; break; }
                case Shops.Qi: { stockItems = QiShopStock; break; }
                case Shops.Joja: { stockItems = JojaShopStock; break; }
                case Shops.Hat: { stockItems = HatShopStock; break; }
                case Shops.Fish: { stockItems = FishShopStock; break; }
                case Shops.Animal: { stockItems = AnimalShopStock; break; }
                case Shops.Sandy: { stockItems = SandyShopStock; break; }
                case Shops.Saloon: { stockItems = SaloonShopStock; break; }
                case Shops.Adventure: { stockItems = AdventureShopStock; break; }
                case Shops.Carpenter: { stockItems = CarpenterShopStock; break; }
                case Shops.Blacksmith: { stockItems = BlacksmithShopStock; break; }
                case Shops.IceCream: { stockItems = IceCreamShopStock; break; }
                case Shops.Sewer: { stockItems = SewerShopStock; break; }
                case Shops.FreeWallpapersAndFloors: { stockItems = FreeWallpapersAndFloorsShopStock; break; }
                case Shops.FreeFurnitures: { stockItems = FreeFurnituresShopStock; break; }
            }

            if (stockItems == null) { return dictionaryToInject; }

            foreach (StockEntry stockItem in stockItems)
            {
                if (stockItem.DoAdd())
                {
                    if (stockItem.UsesItemInformation())
                    {
                        // TODO when there is a factory for items, use that instead
                        dictionaryToInject.Add(new StardewValley.Object(Vector2.Zero, stockItem.ItemInformation.Id, stockItem.Stock), new int[] { stockItem.Price, stockItem.Stock });
                    }
                    else if (stockItem.UsesBigCraftableInformation())
                    {
                        dictionaryToInject.Add(BigCraftable.Create(Vector2.Zero, stockItem.BigCraftableInformation.Id, false), new int[] { stockItem.Price, stockItem.Stock });
                    }
                }
            }

            return dictionaryToInject;
        }

        // A data class containing all the information needed to add an item to a shop stock
        private class StockEntry
        {
            // The ItemInformation of this shop stock entry, if it is an item it will not be null
            public ItemInformation ItemInformation { get; set; } = null;

            // The BigCraftableInformation of this shop stock entry, if it is a big craftable it will not be null
            public BigCraftableInformation BigCraftableInformation { get; set; } = null;

            // The price of the item, if applicable
            public int Price { get; set; } = 0;

            // The amount in stock, if applicable
            public int Stock { get; set; } = 2147483647;

            // The delegate object used to check it this item should be added
            public CheckIfAddShopStock CheckDelegate { get; set; } = new CheckIfAddShopStock(DummyAdd);

            public StockEntry(ItemInformation itemInformation, int price = 0, int stock = Infinite)
            {
                ItemInformation = itemInformation;
                Price = price;
                Stock = stock;
            }

            public StockEntry(ItemInformation itemInformation, CheckIfAddShopStock checkDelegate, int price = 0, int stock = Infinite)
            {
                ItemInformation = itemInformation;
                CheckDelegate = checkDelegate;
                Price = price;
                Stock = stock;
            }

            public StockEntry(BigCraftableInformation bigCraftableInformation, int price = 0, int stock = Infinite)
            {
                BigCraftableInformation = bigCraftableInformation;
                Price = price;
                Stock = stock;
            }

            public StockEntry(BigCraftableInformation bigCraftableInformation, CheckIfAddShopStock checkDelegate, int price = 0, int stock = Infinite)
            {
                BigCraftableInformation = bigCraftableInformation;
                CheckDelegate = checkDelegate;
                Price = price;
                Stock = stock;
            }

            // Check if this item should be added to the shop stock
            public bool DoAdd()
            {
                return CheckDelegate();
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
}
