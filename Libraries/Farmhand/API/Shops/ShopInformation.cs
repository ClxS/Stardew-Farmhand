namespace Farmhand.API.Shops
{
    using System.Collections.Generic;

    /// <summary>
    ///     Contains information related to a shop.
    /// </summary>
    public class ShopInformation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShopInformation" /> class.
        /// </summary>
        /// <param name="owner">
        ///     The owning mod.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="currency">
        ///     The currency.
        /// </param>
        public ShopInformation(Mod owner, string name, int currency = 0)
        {
            this.Owner = owner;
            this.Name = name;
            this.CurrencyType = currency;
        }

        /// <summary>
        ///     Gets or sets the name of this shop
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the owning mod of this shop
        /// </summary>
        public Mod Owner { get; set; }

        /// <summary>
        ///     Gets the stock available in this shop
        /// </summary>
        public List<StockEntry> Stock { get; } = new List<StockEntry>();

        /// <summary>
        ///     Gets or sets the type of currency used in this store
        /// </summary>
        public int CurrencyType { get; set; }

        /// <summary>
        ///     Gets a unique ID for this shop, based on the owning mod.
        /// </summary>
        /// <returns>
        ///     The unique ID for this shop.
        /// </returns>
        public string UniqueModId()
        {
            return ShopUtilities.GetInternalShopName(this.Owner, this.Name);
        }
    }
}