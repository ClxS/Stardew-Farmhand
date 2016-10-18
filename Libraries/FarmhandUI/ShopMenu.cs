using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand.API.Shops;
using Farmhand.Attributes;
using StardewValley;

namespace Farmhand.UI
{
    [HookRedirectConstructorFromBase("StardewValley.Event", "answerDialogue", new Type[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.Event", "checkAction", new Type[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "adventureShop", new Type[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "openShopMenu", new Type[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "performAction", new Type[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.Locations.Forest", "checkAction", new Type[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.NPC", "checkAction", new Type[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.Objects.Furniture", "checkForAction", new Type[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "answerDialogueAction", new Type[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "answerDialogueAction", new Type[] { typeof(List<Item>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "carpenters", new Type[] { typeof(List<Item>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "openShopMenu", new Type[] { typeof(List<Item>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "saloon", new Type[] { typeof(List<Item>), typeof(int), typeof(string) })]
    public class ShopMenu : StardewValley.Menus.ShopMenu
    {
        private Dictionary<Item, int[]> injectedStock = new Dictionary<Item, int[]>();
        private Shops shopType = Shops.Unknown;

        public ShopMenu(Dictionary<Item, int[]> itemPriceAndStock, int currency = 0, string who = null) :
            base(itemPriceAndStock, currency, who)
        {
            shopType = GetShopType(itemPriceAndStock, currency, who);
            if (shopType != Shops.Unknown)
            {
                InjectStock(shopType);
            }
        }

        public ShopMenu(List<Item> itemsForSale, int currency = 0, string who = null) :
            base(itemsForSale, currency, who)
        {
            shopType = GetShopType(itemsForSale, currency, who);
            if (shopType != Shops.Unknown)
            {
                InjectStock(shopType);
            }
        }

        
        protected void InjectStock(Shops shop)
        {
            Dictionary<Item, int[]> newStock = ShopUtilities.GetNewStock(shop);
            foreach (KeyValuePair<Item, int[]> stock in newStock)
            {
                forSale.Add(stock.Key);
                itemPriceAndStock.Add(stock.Key, stock.Value);
            }
            injectedStock = newStock;
        }

        protected void InjectStockSoft()
        {
            foreach (KeyValuePair<Item, int[]> stock in injectedStock)
            {
                itemPriceAndStock.Add(stock.Key, stock.Value);
            }
        }

        /// <summary>
        /// Opens a registered custom shop
        /// </summary>
        /// <param name="owner">Instance of mod which created the shop</param>
        /// <param name="shopName">String identifier of shop to open</param>
        public static void OpenShop(Mod owner, string shopName)
        {
            string internalShopName = ShopUtilities.GetInternalShopName(owner, shopName);

            if (ShopUtilities.RegisteredShops.ContainsKey(internalShopName))
            {
                Game1.activeClickableMenu = new ShopMenu(ShopUtilities.GetStock(owner, shopName), ShopUtilities.RegisteredShops[internalShopName].CurrencyType, null);
            }
            else
            {
                Logging.Log.Warning($"Shop {internalShopName} not found! Could not open shop.");
            }
        }

        // Determine what kind of shop this is
        protected Shops GetShopType(Dictionary<Item, int[]> shop, int currency = 0, string who = null)
        {
            // String checking is TERRIBLE, but it's how stardew does it, and it's fairly easy to change if it breaks later on
            if (who != null)
            {
                if (who.Equals("Marlon"))
                {
                    return Shops.Adventure;
                }
                else if (who.Equals("Marnie"))
                {
                    return Shops.Animal;
                }
                else if (who.Equals("Clint"))
                {
                    return Shops.Blacksmith;
                }
                else if (who.Equals("Willy"))
                {
                    return Shops.Fish;
                }
                else if (who.Equals("Pierre"))
                {
                    return Shops.Pierre;
                }
                else if (who.Equals("HatMouse"))
                {
                    return Shops.Hat;
                }
                else if (who.Equals("Traveler"))
                {
                    return Shops.TravelingMerchant;
                }
                else if (who.Equals("Dwarf"))
                {
                    return Shops.Dwarf;
                }
                else if (who.Equals("Krobus"))
                {
                    return Shops.Sewer;
                }
                else if (who.Equals("Robin"))
                {
                    return Shops.Carpenter;
                }
                else if (who.Equals("Sandy"))
                {
                    return Shops.Sandy;
                }
                else if (who.Equals("Gus"))
                {
                    return Shops.Saloon;
                }
            }
            // If there wasn't a string attached, because reasons, this backup attempts to compare the shops and determine which one it is
            else
            {
                if (CompareShopStocks(shop, Utility.getHospitalStock()))
                {
                    return Shops.Hospital;
                }
                if (CompareShopStocks(shop, Utility.getQiShopStock()))
                {
                    return Shops.Qi;
                }
                if (CompareShopStocks(shop, Utility.getJojaStock()))
                {
                    return Shops.Joja;
                }
                if (CompareShopStocks(shop, Utility.getAllWallpapersAndFloorsForFree()))
                {
                    return Shops.FreeWallpapersAndFloors;
                }
                if (CompareShopStocks(shop, Utility.getAllFurnituresForFree()))
                {
                    return Shops.FreeFurnitures;
                }
                // There is no internal definition for the ice cream shop, it's made on the fly
                else if (CompareShopStocks(shop, new Dictionary<Item, int[]>
                                    {
                                        {
                                            new StardewValley.Object(233, 1, false, -1, 0),
                                            new int[]
                                            {
                                                250,
                                                2147483647
                                            }
                                        }
                                    }))
                {
                    return Shops.IceCream;
                }
            }

            return Shops.Unknown;
        }

        protected Shops GetShopType(List<Item> shop, int currency = 0, string who = null)
        {
            // String checking is TERRIBLE, but it's how stardew does it, and it's fairly easy to change if it breaks later on
            if (who != null)
            {
                if (who.Equals("Marlon"))
                {
                    return Shops.Adventure;
                }
                else if (who.Equals("Marnie"))
                {
                    return Shops.Animal;
                }
                else if (who.Equals("Clint"))
                {
                    return Shops.Blacksmith;
                }
                else if (who.Equals("Willy"))
                {
                    return Shops.Fish;
                }
                else if (who.Equals("Pierre"))
                {
                    return Shops.Pierre;
                }
                else if (who.Equals("HatMouse"))
                {
                    return Shops.Hat;
                }
                else if (who.Equals("Traveler"))
                {
                    return Shops.TravelingMerchant;
                }
                else if (who.Equals("Dwarf"))
                {
                    return Shops.Dwarf;
                }
                else if (who.Equals("Krobus"))
                {
                    return Shops.Sewer;
                }
                else if (who.Equals("Robin"))
                {
                    return Shops.Carpenter;
                }
                else if (who.Equals("Sandy"))
                {
                    return Shops.Sandy;
                }
                else if (who.Equals("Gus"))
                {
                    return Shops.Saloon;
                }
            }

            return Shops.Unknown;
        }

        // Compare two shop stocks to determine if they're equal
        protected bool CompareShopStocks(Dictionary<Item, int[]> a, Dictionary<Item, int[]> b)
        {
            if (a.Count != b.Count) return false;

            KeyValuePair<Item, int[]>[] aArray = a.ToArray();
            KeyValuePair<Item, int[]>[] bArray = b.ToArray();

            for (int i = 0; i < aArray.Length; i++)
            {
                // Compare the objects Ids
                if (aArray[i].Key.parentSheetIndex != bArray[i].Key.parentSheetIndex)
                {
                    return false;
                }

                // Compare the arrays of integers
                if (aArray[i].Value.Length != bArray[i].Value.Length)
                {
                    return false;
                }

                for (int n = 0; n < aArray[i].Value.Length; n++)
                {
                    if (aArray[i].Value[n] != bArray[i].Value[n])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Compare two shop stocks to determine if they're equal
        protected bool CompareShopStocks(List<Item> a, List<Item> b)
        {
            if (a.Count != b.Count) return false;

            for (int i = 0; i < a.Count; i++)
            {
                // Compare the object Ids
                if (a[i].parentSheetIndex != b[i].parentSheetIndex)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
