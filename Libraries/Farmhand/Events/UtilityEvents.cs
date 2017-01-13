namespace Farmhand.Events
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.UtilityEvents;

    /// <summary>
    ///     Events which are used for utility purposes.
    /// </summary>
    public class UtilityEvents
    {
        /// <summary>
        ///     Fires when the game tries to get the Dwarfs shop stock.
        /// </summary>
        public static event EventHandler<GetDwarfShopStockEventArgs> PostGetDwarfShopStock = delegate { };

        [Hook(HookType.Exit, "StardewValley.Utility", "getDwarfShopStock")]
        internal static void OnPostGetDwarfShopStock()
        {
            EventCommon.SafeInvoke(PostGetDwarfShopStock, null, new GetDwarfShopStockEventArgs());
        }
    }
}