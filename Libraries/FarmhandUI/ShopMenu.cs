using Farmhand.Attributes;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Overrides.UI
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
        private Farmhand.API.Utilities.Shops shopType = Farmhand.API.Utilities.Shops.Unknown;

        public ShopMenu(Dictionary<Item, int[]> itemPriceAndStock, int currency = 0, string who = null) :
            base(itemPriceAndStock, currency, who)
        {
            shopType = GetShopType(itemPriceAndStock, currency, who);
            if (shopType != Farmhand.API.Utilities.Shops.Unknown)
            {
                InjectStock(shopType);
            }
        }

        public ShopMenu(List<Item> itemsForSale, int currency = 0, string who = null) :
            base(itemsForSale, currency, who)
        {
            shopType = GetShopType(itemsForSale, currency, who);
            if (shopType != Farmhand.API.Utilities.Shops.Unknown)
            {
                InjectStock(shopType);
            }
        }

        
        protected void InjectStock(Farmhand.API.Utilities.Shops shop)
        {
            Dictionary<Item, int[]> newStock = Farmhand.API.Utilities.ShopUtilities.GetNewStock(shop);
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

        // Determine what kind of shop this is
        protected Farmhand.API.Utilities.Shops GetShopType(Dictionary<Item, int[]> shop, int currency = 0, string who = null)
        {
            // String checking is TERRIBLE, but it's how stardew does it, and it's fairly easy to change if it breaks later on
            if (who != null)
            {
                if (who.Equals("Marlon"))
                {
                    return Farmhand.API.Utilities.Shops.Adventure;
                }
                else if (who.Equals("Marnie"))
                {
                    return Farmhand.API.Utilities.Shops.Animal;
                }
                else if (who.Equals("Clint"))
                {
                    return Farmhand.API.Utilities.Shops.Blacksmith;
                }
                else if (who.Equals("Willy"))
                {
                    return Farmhand.API.Utilities.Shops.Fish;
                }
                else if (who.Equals("Pierre"))
                {
                    return Farmhand.API.Utilities.Shops.Pierre;
                }
                else if (who.Equals("HatMouse"))
                {
                    return Farmhand.API.Utilities.Shops.Hat;
                }
                else if (who.Equals("Traveler"))
                {
                    return Farmhand.API.Utilities.Shops.TravelingMerchant;
                }
                else if (who.Equals("Dwarf"))
                {
                    return Farmhand.API.Utilities.Shops.Dwarf;
                }
                else if (who.Equals("Krobus"))
                {
                    return Farmhand.API.Utilities.Shops.Sewer;
                }
                else if (who.Equals("Robin"))
                {
                    return Farmhand.API.Utilities.Shops.Carpenter;
                }
                else if (who.Equals("Sandy"))
                {
                    return Farmhand.API.Utilities.Shops.Sandy;
                }
                else if (who.Equals("Gus"))
                {
                    return Farmhand.API.Utilities.Shops.Saloon;
                }
            }
            // If there wasn't a string attached, because reasons, this backup attempts to compare the shops and determine which one it is
            else
            {
                if (CompareShopStocks(shop, StardewValley.Utility.getHospitalStock()))
                {
                    return Farmhand.API.Utilities.Shops.Hospital;
                }
                if (CompareShopStocks(shop, StardewValley.Utility.getQiShopStock()))
                {
                    return Farmhand.API.Utilities.Shops.Qi;
                }
                if (CompareShopStocks(shop, StardewValley.Utility.getJojaStock()))
                {
                    return Farmhand.API.Utilities.Shops.Joja;
                }
                if (CompareShopStocks(shop, StardewValley.Utility.getAllWallpapersAndFloorsForFree()))
                {
                    return Farmhand.API.Utilities.Shops.FreeWallpapersAndFloors;
                }
                if (CompareShopStocks(shop, StardewValley.Utility.getAllFurnituresForFree()))
                {
                    return Farmhand.API.Utilities.Shops.FreeFurnitures;
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
                    return Farmhand.API.Utilities.Shops.IceCream;
                }
            }

            return Farmhand.API.Utilities.Shops.Unknown;
        }

        protected Farmhand.API.Utilities.Shops GetShopType(List<Item> shop, int currency = 0, string who = null)
        {
            // String checking is TERRIBLE, but it's how stardew does it, and it's fairly easy to change if it breaks later on
            if (who != null)
            {
                if (who.Equals("Marlon"))
                {
                    return Farmhand.API.Utilities.Shops.Adventure;
                }
                else if (who.Equals("Marnie"))
                {
                    return Farmhand.API.Utilities.Shops.Animal;
                }
                else if (who.Equals("Clint"))
                {
                    return Farmhand.API.Utilities.Shops.Blacksmith;
                }
                else if (who.Equals("Willy"))
                {
                    return Farmhand.API.Utilities.Shops.Fish;
                }
                else if (who.Equals("Pierre"))
                {
                    return Farmhand.API.Utilities.Shops.Pierre;
                }
                else if (who.Equals("HatMouse"))
                {
                    return Farmhand.API.Utilities.Shops.Hat;
                }
                else if (who.Equals("Traveler"))
                {
                    return Farmhand.API.Utilities.Shops.TravelingMerchant;
                }
                else if (who.Equals("Dwarf"))
                {
                    return Farmhand.API.Utilities.Shops.Dwarf;
                }
                else if (who.Equals("Krobus"))
                {
                    return Farmhand.API.Utilities.Shops.Sewer;
                }
                else if (who.Equals("Robin"))
                {
                    return Farmhand.API.Utilities.Shops.Carpenter;
                }
                else if (who.Equals("Sandy"))
                {
                    return Farmhand.API.Utilities.Shops.Sandy;
                }
                else if (who.Equals("Gus"))
                {
                    return Farmhand.API.Utilities.Shops.Saloon;
                }
            }

            return Farmhand.API.Utilities.Shops.Unknown;
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
