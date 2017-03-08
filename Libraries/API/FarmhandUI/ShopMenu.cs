namespace Farmhand.UI
{
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.API;
    using Farmhand.API.Shops;
    using Farmhand.Attributes;
    using Farmhand.Logging;

    using StardewValley;

    /// <summary>
    ///     A shop menu override.
    /// </summary>
    [HookRedirectConstructorFromBase("StardewValley.Event", "answerDialogue",
        new[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.Event", "checkAction",
        new[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "adventureShop",
        new[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "openShopMenu",
        new[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "performAction",
        new[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.Locations.Forest", "checkAction",
        new[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.NPC", "checkAction",
        new[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.Objects.Furniture", "checkForAction",
        new[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "answerDialogueAction",
        new[] { typeof(Dictionary<Item, int[]>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "answerDialogueAction",
        new[] { typeof(List<Item>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "carpenters",
        new[] { typeof(List<Item>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "openShopMenu",
        new[] { typeof(List<Item>), typeof(int), typeof(string) })]
    [HookRedirectConstructorFromBase("StardewValley.GameLocation", "saloon",
        new[] { typeof(List<Item>), typeof(int), typeof(string) })]
    public class ShopMenu : StardewValley.Menus.ShopMenu
    {
        private Dictionary<Item, int[]> injectedStock = new Dictionary<Item, int[]>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShopMenu" /> class.
        /// </summary>
        /// <param name="itemPriceAndStock">
        ///     A dictionary of item prices and stock.
        /// </param>
        /// <param name="currency">
        ///     The currency.
        /// </param>
        /// <param name="who">
        ///     Who opened this shop menu.
        /// </param>
        public ShopMenu(Dictionary<Item, int[]> itemPriceAndStock, int currency = 0, string who = null)
            : base(itemPriceAndStock, currency, who)
        {
            var shopType = this.GetShopType(itemPriceAndStock, currency, who);
            if (shopType != Shops.Unknown)
            {
                this.InjectStock(shopType);
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShopMenu" /> class.
        /// </summary>
        /// <param name="itemsForSale">
        ///     A list of items for sale.
        /// </param>
        /// <param name="currency">
        ///     The currency.
        /// </param>
        /// <param name="who">
        ///     Who opened this shop menu.
        /// </param>
        public ShopMenu(List<Item> itemsForSale, int currency = 0, string who = null)
            : base(itemsForSale, currency, who)
        {
            var shopType = this.GetShopType(itemsForSale, currency, who);
            if (shopType != Shops.Unknown)
            {
                this.InjectStock(shopType);
            }
        }

        /// <summary>
        ///     Injects stock into the shop.
        /// </summary>
        /// <param name="shop">
        ///     The shop to inject into.
        /// </param>
        protected void InjectStock(Shops shop)
        {
            var newStock = ShopUtilities.GetNewStock(shop);
            foreach (var stock in newStock)
            {
                this.forSale.Add(stock.Key);
                this.itemPriceAndStock.Add(stock.Key, stock.Value);
            }

            this.injectedStock = newStock;
        }

        /// <summary>
        ///     Injects stock into the shop
        /// </summary>
        protected void InjectStockSoft()
        {
            foreach (var stock in this.injectedStock)
            {
                this.itemPriceAndStock.Add(stock.Key, stock.Value);
            }
        }

        /// <summary>
        ///     Opens a registered custom shop
        /// </summary>
        /// <param name="owner">Instance of mod which created the shop</param>
        /// <param name="shopName">String identifier of shop to open</param>
        public static void OpenShop(Mod owner, string shopName)
        {
            var internalShopName = ShopUtilities.GetInternalShopName(owner, shopName);

            if (ShopUtilities.RegisteredShops.ContainsKey(internalShopName))
            {
                Game.ActiveClickableMenu = new ShopMenu(
                    ShopUtilities.GetStock(owner, shopName),
                    ShopUtilities.RegisteredShops[internalShopName].CurrencyType);
            }
            else
            {
                Log.Warning($"Shop {internalShopName} not found! Could not open shop.");
            }
        }

        /// <summary>
        ///     Determine what kind of shop this is.
        /// </summary>
        /// <param name="shop">
        ///     The stock of shop to check.
        /// </param>
        /// <param name="shopCurrency">
        ///     The shop currency.
        /// </param>
        /// <param name="who">
        ///     Which NPC is running this shop.
        /// </param>
        /// <returns>
        ///     An enum of type <see cref="Shops" />.
        /// </returns>
        protected Shops GetShopType(Dictionary<Item, int[]> shop, int shopCurrency = 0, string who = null)
        {
            // String checking is TERRIBLE, but it's how stardew does it, and it's fairly easy to change if it breaks later on
            if (who != null)
            {
                if (who.Equals("Marlon"))
                {
                    return Shops.Adventure;
                }

                if (who.Equals("Marnie"))
                {
                    return Shops.Animal;
                }

                if (who.Equals("Clint"))
                {
                    return Shops.Blacksmith;
                }

                if (who.Equals("Willy"))
                {
                    return Shops.Fish;
                }

                if (who.Equals("Pierre"))
                {
                    return Shops.Pierre;
                }

                if (who.Equals("HatMouse"))
                {
                    return Shops.Hat;
                }

                if (who.Equals("Traveler"))
                {
                    return Shops.TravelingMerchant;
                }

                if (who.Equals("Dwarf"))
                {
                    return Shops.Dwarf;
                }

                if (who.Equals("Krobus"))
                {
                    return Shops.Sewer;
                }

                if (who.Equals("Robin"))
                {
                    return Shops.Carpenter;
                }

                if (who.Equals("Sandy"))
                {
                    return Shops.Sandy;
                }

                if (who.Equals("Gus"))
                {
                    return Shops.Saloon;
                }
            }
            else
            {
                // If there wasn't a string attached, because reasons, this backup attempts to compare the shops and determine which one it is
                if (this.CompareShopStocks(shop, Utility.getHospitalStock()))
                {
                    return Shops.Hospital;
                }

                if (this.CompareShopStocks(shop, Utility.getQiShopStock()))
                {
                    return Shops.Qi;
                }

                if (this.CompareShopStocks(shop, Utility.getJojaStock()))
                {
                    return Shops.Joja;
                }

                if (this.CompareShopStocks(shop, Utility.getAllWallpapersAndFloorsForFree()))
                {
                    return Shops.FreeWallpapersAndFloors;
                }

                if (this.CompareShopStocks(shop, Utility.getAllFurnituresForFree()))
                {
                    return Shops.FreeFurnitures;
                }

                // There is no internal definition for the ice cream shop, it's made on the fly
                // ReSharper disable once RedundantNameQualifier
                if (this.CompareShopStocks(
                    shop,
                    new Dictionary<Item, int[]> { { new StardewValley.Object(233, 1), new[] { 250, 2147483647 } } }))
                {
                    return Shops.IceCream;
                }
            }

            return Shops.Unknown;
        }

        /// <summary>
        ///     Determine what kind of shop this is.
        /// </summary>
        /// <param name="shop">
        ///     The stock of the shop to check.
        /// </param>
        /// <param name="shopCurrency">
        ///     The shop currency.
        /// </param>
        /// <param name="who">
        ///     Which NPC is running this shop.
        /// </param>
        /// <returns>
        ///     The <see cref="Shops" />.
        /// </returns>
        protected Shops GetShopType(List<Item> shop, int shopCurrency = 0, string who = null)
        {
            // String checking is TERRIBLE, but it's how stardew does it, and it's fairly easy to change if it breaks later on
            if (who != null)
            {
                if (who.Equals("Marlon"))
                {
                    return Shops.Adventure;
                }

                if (who.Equals("Marnie"))
                {
                    return Shops.Animal;
                }

                if (who.Equals("Clint"))
                {
                    return Shops.Blacksmith;
                }

                if (who.Equals("Willy"))
                {
                    return Shops.Fish;
                }

                if (who.Equals("Pierre"))
                {
                    return Shops.Pierre;
                }

                if (who.Equals("HatMouse"))
                {
                    return Shops.Hat;
                }

                if (who.Equals("Traveler"))
                {
                    return Shops.TravelingMerchant;
                }

                if (who.Equals("Dwarf"))
                {
                    return Shops.Dwarf;
                }

                if (who.Equals("Krobus"))
                {
                    return Shops.Sewer;
                }

                if (who.Equals("Robin"))
                {
                    return Shops.Carpenter;
                }

                if (who.Equals("Sandy"))
                {
                    return Shops.Sandy;
                }

                if (who.Equals("Gus"))
                {
                    return Shops.Saloon;
                }
            }

            return Shops.Unknown;
        }

        /// <summary>
        ///     Compare two shop stocks to determine if they're equal.
        /// </summary>
        /// <param name="a">
        ///     Stock from shop a.
        /// </param>
        /// <param name="b">
        ///     Stock from shop b.
        /// </param>
        /// <returns>
        ///     Whether the two stocks are equal.
        /// </returns>
        protected bool CompareShopStocks(Dictionary<Item, int[]> a, Dictionary<Item, int[]> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            var shopAArray = a.ToArray();
            var shopBArray = b.ToArray();

            for (var i = 0; i < shopAArray.Length; i++)
            {
                // Compare the objects Ids
                if (shopAArray[i].Key.parentSheetIndex != shopBArray[i].Key.parentSheetIndex)
                {
                    return false;
                }

                // Compare the arrays of integers
                if (shopAArray[i].Value.Length != shopBArray[i].Value.Length)
                {
                    return false;
                }

                for (var n = 0; n < shopAArray[i].Value.Length; n++)
                {
                    if (shopAArray[i].Value[n] != shopBArray[i].Value[n])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     Compare two shop stocks to determine if they're equal.
        /// </summary>
        /// <param name="a">
        ///     Stock from shop a.
        /// </param>
        /// <param name="b">
        ///     Stock from shop b.
        /// </param>
        /// <returns>
        ///     Whether the two stocks are equal.
        /// </returns>
        protected bool CompareShopStocks(List<Item> a, List<Item> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            for (var i = 0; i < a.Count; i++)
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