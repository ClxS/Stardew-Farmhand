using Farmhand.API.Items;
using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Shops
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

    // The different types of stock, these use different Id systems
    public enum StockType
    {
        Item,
        BigCraftable
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

        // A registry which contains mod shop information
        public static Dictionary<string, ShopInformation> RegisteredShops { get; } = new Dictionary<string, ShopInformation>();

        /// <summary>
        /// Register a new shop that can be used
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shopName">String identifier of shop that will be used to refer to it</param>
        /// <param name="currency">Type of currency accepted at this shop</param>
        public static void RegisterShop(Mod owner, string shopName, int currency = 0)
        {
            string internalShopName = GetInternalShopName(owner, shopName);

            ShopInformation shop = new ShopInformation(owner, internalShopName, currency);

            if (!RegisteredShops.ContainsKey(internalShopName))
            {
                RegisteredShops.Add(shop.Name, shop);
            }
            else
            {
                Farmhand.Logging.Log.Warning($"Shop {internalShopName} already exists! Using originally registered shop.");
            }
        }

        /// <summary>
        /// Add an item to a shop
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shop">Shop to add the item to</param>
        /// <param name="item">ItemInformation of item to add</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, Shops shop, ItemInformation item, int price = 0, int stock = Infinite)
        {
            AddToShopStock(shop, new StockEntry(owner, item, price, stock));
        }

        /// <summary>
        /// Add an item to a shop
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shop">Shop to add the item to</param>
        /// <param name="item">ItemInformation of item to add</param>
        /// <param name="checkDelegate">Delegate method which checks if this item should be in stock</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, Shops shop, ItemInformation item, CheckIfAddShopStock checkDelegate, int price = 0, int stock = Infinite)
        {
            AddToShopStock(shop, new StockEntry(owner, item, checkDelegate, price, stock));
        }

        /// <summary>
        /// Add a big craftable to a shop
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shop">Shop to add the item to</param>
        /// <param name="item">BigCraftableInformation of item to add</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, Shops shop, BigCraftableInformation item, int price = 0, int stock = Infinite)
        {
            AddToShopStock(shop, new StockEntry(owner, item, price, stock));
        }

        /// <summary>
        /// Add a big craftable to a shop
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shop">Shop to add the item to</param>
        /// <param name="item">BigCraftableInformation of item to add</param>
        /// <param name="checkDelegate">Delegate method which checks if this item should be in stock</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, Shops shop, BigCraftableInformation item, CheckIfAddShopStock checkDelegate, int price = 0, int stock = Infinite)
        {
            AddToShopStock(shop, new StockEntry(owner, item, checkDelegate, price, stock));
        }

        /// <summary>
        /// Add an item to a registered custom shop. This method will NOT add stock to vanilla shops!
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shopName">String identifier of the shop to add stock to</param>
        /// <param name="item">ItemInformation of item to add</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, string shopName, ItemInformation item, int price = 0, int stock = Infinite)
        {
            AddToShopStock(owner, shopName, new StockEntry(owner, item, price, stock));
        }

        /// <summary>
        /// Add an item to a registered custom shop. This method will NOT add stock to vanilla shops!
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shopName">String identifier of the shop to add stock to</param>
        /// <param name="item">ItemInformation of item to add</param>
        /// <param name="checkDelegate">Delegate method which checks if this item should be in stock</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, string shopName, ItemInformation item, CheckIfAddShopStock checkDelegate, int price = 0, int stock = Infinite)
        {
            AddToShopStock(owner, shopName, new StockEntry(owner, item, checkDelegate, price, stock));
        }

        /// <summary>
        /// Add a big craftable to a registered custom shop. This method will NOT add stock to vanilla shops!
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shopName">String identifier of the shop to add stock to</param>
        /// <param name="item">BigCraftableInformation of item to add</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, string shopName, BigCraftableInformation item, int price = 0, int stock = Infinite)
        {
            AddToShopStock(owner, shopName, new StockEntry(owner, item, price, stock));
        }

        /// <summary>
        /// Add a big craftable to a registered custom shop. This method will NOT add stock to vanilla shops!
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shopName">String identifier of the shop to add stock to</param>
        /// <param name="item">BigCraftableInformation of item to add</param>
        /// <param name="checkDelegate">Delegate method which checks if this item should be in stock</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, string shopName, BigCraftableInformation item, CheckIfAddShopStock checkDelegate, int price = 0, int stock = Infinite)
        {
            AddToShopStock(owner, shopName, new StockEntry(owner, item, checkDelegate, price, stock));
        }

        /// <summary>
        /// Add an item to a shop
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shop">Shop to add the item to</param>
        /// <param name="stockType">The type of Id the item being added is</param>
        /// <param name="Id">Id of the item being added</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, Shops shop, StockType stockType, int Id, int price = 0, int stock = Infinite)
        {
            AddToShopStock(shop, new StockEntry(owner, stockType, Id, price, stock));
        }

        /// <summary>
        /// Add an item to a shop
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shop">Shop to add the item to</param>
        /// <param name="stockType">The type of Id the item being added is</param>
        /// <param name="Id">Id of the item being added</param>
        /// <param name="checkDelegate">Delegate method which checks if this item should be in stock</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, Shops shop, StockType stockType, int Id, CheckIfAddShopStock checkDelegate, int price = 0, int stock = Infinite)
        {
            AddToShopStock(shop, new StockEntry(owner, stockType, Id, checkDelegate, price, stock));
        }

        /// <summary>
        /// Add an item to a registered custom shop. This method will NOT add stock to vanilla shops!
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shopName">String identifier of the shop to add stock to</param>
        /// <param name="stockType">The type of Id the item being added is</param>
        /// <param name="Id">Id of the item being added</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, string shopName, StockType stockType, int Id, int price = 0, int stock = Infinite)
        {
            AddToShopStock(owner, shopName, new StockEntry(owner, stockType, Id, price, stock));
        }

        /// <summary>
        /// Add an item to a registered custom shop. This method will NOT add stock to vanilla shops!
        /// </summary>
        /// <param name="owner">Instance of the mod which is submitting this request</param>
        /// <param name="shopName">String identifier of the shop to add stock to</param>
        /// <param name="stockType">The type of Id the item being added is</param>
        /// <param name="Id">Id of the item being added</param>
        /// <param name="checkDelegate">Delegate method which checks if this item should be in stock</param>
        /// <param name="price">optional, price of item</param>
        /// <param name="stock">optional, amount of item in stock</param>
        public static void AddToShopStock(Mod owner, string shopName, StockType stockType, int Id, CheckIfAddShopStock checkDelegate, int price = 0, int stock = Infinite)
        {
            AddToShopStock(owner, shopName, new StockEntry(owner, stockType, Id, checkDelegate, price, stock));
        }

        // Adds stock to vanilla shops
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

        // Adds stock to custom registered shops
        private static void AddToShopStock(Mod owner, string shopName, StockEntry newEntry)
        {
            string internalShopName = GetInternalShopName(owner, shopName);

            if (RegisteredShops.ContainsKey(internalShopName))
            {
                RegisteredShops[internalShopName].Stock.Add(newEntry);
            }
            else
            {
                Farmhand.Logging.Log.Warning($"Shop {internalShopName} not found! Could not add stock to this shop.");
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
                if (stockItem.Owner.ModSettings.ModState == ModState.Loaded && stockItem.DoAdd())
                {
                    if (stockItem.UsesItemId())
                    {
                        // TODO when there is a factory for items, use that instead
                        dictionaryToInject.Add(new StardewValley.Object(Vector2.Zero, stockItem.ItemId, stockItem.Stock), new int[] { stockItem.Price, stockItem.Stock });
                    }
                    else if(stockItem.UsesBigCraftableId())
                    {
                        dictionaryToInject.Add(BigCraftable.Create(Vector2.Zero, stockItem.BigCraftableId, false), new int[] { stockItem.Price, stockItem.Stock });
                    }
                    else if (stockItem.UsesItemInformation())
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

        public static Dictionary<StardewValley.Item, int[]> GetStock(Mod owner, string shopName)
        {
            string internalShopName = GetInternalShopName(owner, shopName);

            if (RegisteredShops.ContainsKey(internalShopName))
            {
                List<StockEntry> stockItems = RegisteredShops[internalShopName].Stock;
                Dictionary<StardewValley.Item, int[]> finalShopStock = new Dictionary<StardewValley.Item, int[]>(); ;

                if (stockItems == null) { return finalShopStock; }

                foreach (StockEntry stockItem in stockItems)
                {
                    if (stockItem.Owner.ModSettings.ModState == ModState.Loaded && stockItem.DoAdd())
                    {
                        if (stockItem.UsesItemId())
                        {
                            // TODO when there is a factory for items, use that instead
                            finalShopStock.Add(new StardewValley.Object(Vector2.Zero, stockItem.ItemId, stockItem.Stock), new int[] { stockItem.Price, stockItem.Stock });
                        }
                        else if (stockItem.UsesBigCraftableId())
                        {
                            finalShopStock.Add(BigCraftable.Create(Vector2.Zero, stockItem.BigCraftableId, false), new int[] { stockItem.Price, stockItem.Stock });
                        }
                        else if (stockItem.UsesItemInformation())
                        {
                            // TODO when there is a factory for items, use that instead
                            finalShopStock.Add(new StardewValley.Object(Vector2.Zero, stockItem.ItemInformation.Id, stockItem.Stock), new int[] { stockItem.Price, stockItem.Stock });
                        }
                        else if (stockItem.UsesBigCraftableInformation())
                        {
                            finalShopStock.Add(BigCraftable.Create(Vector2.Zero, stockItem.BigCraftableInformation.Id, false), new int[] { stockItem.Price, stockItem.Stock });
                        }
                    }
                }

                return finalShopStock;
            }
            else
            {
                Farmhand.Logging.Log.Warning($"Shop {internalShopName} not registered! Could not get shop stock.");
                return new Dictionary<StardewValley.Item, int[]>();
            }
        }

        /// <summary>
        /// Returns the internally used shop string identifier for a given shop
        /// </summary>
        /// <param name="owner">Instance of the mod which owns the shop</param>
        /// <param name="shopName">String identifier for the shop provided by the mod</param>
        /// <returns></returns>
        public static string GetInternalShopName(Mod owner, string shopName)
        {
            return $"{owner.ModSettings.UniqueId}:{shopName}";
        }
    }
}
