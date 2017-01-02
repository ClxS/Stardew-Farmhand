namespace Farmhand.API.Shops
{
    using Farmhand.API.Items;

    /// <summary>
    ///     A data class containing all the information needed to add an item to a shop stock
    /// </summary>
    public class StockEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StockEntry" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod that has requested this stock entry.
        /// </param>
        /// <param name="itemInformation">
        ///     The item information.
        /// </param>
        /// <param name="price">
        ///     The price.
        /// </param>
        /// <param name="stock">
        ///     The stock.
        /// </param>
        public StockEntry(Mod owner, ItemInformation itemInformation, int price = 0, int stock = ShopUtilities.Infinite)
        {
            this.Owner = owner;
            this.ItemInformation = itemInformation;
            this.Price = price;
            this.Stock = stock;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockEntry" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod that has requested this stock entry.
        /// </param>
        /// <param name="itemInformation">
        ///     The item information.
        /// </param>
        /// <param name="checkDelegate">
        ///     The check delegate.
        /// </param>
        /// <param name="price">
        ///     The price.
        /// </param>
        /// <param name="stock">
        ///     The stock.
        /// </param>
        public StockEntry(
            Mod owner,
            ItemInformation itemInformation,
            ShopUtilities.CheckIfAddShopStock checkDelegate,
            int price = 0,
            int stock = ShopUtilities.Infinite)
        {
            this.Owner = owner;
            this.ItemInformation = itemInformation;
            this.CheckDelegate = checkDelegate;
            this.Price = price;
            this.Stock = stock;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockEntry" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod that has requested this stock entry.
        /// </param>
        /// <param name="bigCraftableInformation">
        ///     The big craftable information.
        /// </param>
        /// <param name="price">
        ///     The price.
        /// </param>
        /// <param name="stock">
        ///     The stock.
        /// </param>
        public StockEntry(
            Mod owner,
            BigCraftableInformation bigCraftableInformation,
            int price = 0,
            int stock = ShopUtilities.Infinite)
        {
            this.Owner = owner;
            this.BigCraftableInformation = bigCraftableInformation;
            this.Price = price;
            this.Stock = stock;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockEntry" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod that has requested this stock entry.
        /// </param>
        /// <param name="bigCraftableInformation">
        ///     The big craftable information.
        /// </param>
        /// <param name="checkDelegate">
        ///     The check delegate.
        /// </param>
        /// <param name="price">
        ///     The price.
        /// </param>
        /// <param name="stock">
        ///     The stock.
        /// </param>
        public StockEntry(
            Mod owner,
            BigCraftableInformation bigCraftableInformation,
            ShopUtilities.CheckIfAddShopStock checkDelegate,
            int price = 0,
            int stock = ShopUtilities.Infinite)
        {
            this.Owner = owner;
            this.BigCraftableInformation = bigCraftableInformation;
            this.CheckDelegate = checkDelegate;
            this.Price = price;
            this.Stock = stock;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockEntry" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod that has requested this stock entry.
        /// </param>
        /// <param name="stockType">
        ///     The stock type.
        /// </param>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <param name="price">
        ///     The price.
        /// </param>
        /// <param name="stock">
        ///     The stock.
        /// </param>
        public StockEntry(Mod owner, StockType stockType, int id, int price = 0, int stock = ShopUtilities.Infinite)
        {
            this.Owner = owner;

            if (stockType == StockType.Item)
            {
                this.ItemId = id;
            }
            else if (stockType == StockType.BigCraftable)
            {
                this.BigCraftableId = id;
            }
            else
            {
                this.ItemId = id;
            }

            this.Price = price;
            this.Stock = stock;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockEntry" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The mod that has requested this stock entry.
        /// </param>
        /// <param name="stockType">
        ///     The stock type.
        /// </param>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <param name="checkDelegate">
        ///     The check delegate.
        /// </param>
        /// <param name="price">
        ///     The price.
        /// </param>
        /// <param name="stock">
        ///     The stock.
        /// </param>
        public StockEntry(
            Mod owner,
            StockType stockType,
            int id,
            ShopUtilities.CheckIfAddShopStock checkDelegate,
            int price = 0,
            int stock = ShopUtilities.Infinite)
        {
            this.Owner = owner;

            if (stockType == StockType.Item)
            {
                this.ItemId = id;
            }
            else if (stockType == StockType.BigCraftable)
            {
                this.BigCraftableId = id;
            }
            else
            {
                this.ItemId = id;
            }

            this.CheckDelegate = checkDelegate;
            this.Price = price;
            this.Stock = stock;
        }

        /// <summary>
        ///     Gets or sets the mod that has requested this stock entry.
        /// </summary>
        public Mod Owner { get; set; }

        /// <summary>
        ///     Gets or sets the Item Id of this shop stock entry, if it is not -1 it will be used
        /// </summary>
        public int ItemId { get; set; } = -1;

        /// <summary>
        ///     Gets or sets the Big Craftable Id of this shop stock entry, if it is not -1 it will be used
        /// </summary>
        public int BigCraftableId { get; set; } = -1;

        /// <summary>
        ///     Gets or sets the <see cref="ItemInformation" /> of this shop stock entry, if it is an item it will not be null
        /// </summary>
        public ItemInformation ItemInformation { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="BigCraftableInformation" /> of this shop stock entry, if it is a big craftable it will
        ///     not be null
        /// </summary>
        public BigCraftableInformation BigCraftableInformation { get; set; }

        /// <summary>
        ///     Gets or sets the price of the item, if applicable
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        ///     Gets or sets the amount in stock, if applicable
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        ///     Gets or sets the delegate object used to check it this item should be added
        /// </summary>
        public ShopUtilities.CheckIfAddShopStock CheckDelegate { get; set; } = DummyAdd;

        /// <summary>
        ///     Check if this item should be added to the shop stock
        /// </summary>
        /// <returns>
        ///     Whether this item should be added to the shop stock.
        /// </returns>
        public bool DoAdd()
        {
            return this.CheckDelegate();
        }

        /// <summary>
        ///     Check if this is using ItemId
        /// </summary>
        /// <returns>
        ///     Whether ItemId is in use.
        /// </returns>
        public bool UsesItemId()
        {
            return this.ItemId != -1;
        }

        /// <summary>
        ///     Check if this is using BigCraftableId
        /// </summary>
        /// <returns>
        ///     Whether BigCraftableId is in use.
        /// </returns>
        public bool UsesBigCraftableId()
        {
            return this.BigCraftableId != -1;
        }

        /// <summary>
        ///     Check if this is using ItemInformation
        /// </summary>
        /// <returns>
        ///     Whether ItemInformation is in use.
        /// </returns>
        public bool UsesItemInformation()
        {
            return this.ItemInformation != null;
        }

        /// <summary>
        ///     Check if this is using BigCraftableInformation
        /// </summary>
        /// <returns>
        ///     Whether ItemInformation is in use.
        /// </returns>
        public bool UsesBigCraftableInformation()
        {
            return this.BigCraftableInformation != null;
        }

        /// <summary>
        ///     A dummy method which will always return true. A default value used when no delegate is given to check for special
        ///     conditions
        /// </summary>
        /// <returns>Always true</returns>
        private static bool DummyAdd()
        {
            return true;
        }
    }
}