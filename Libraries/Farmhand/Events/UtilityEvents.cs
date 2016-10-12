using Farmhand.Attributes;
using Farmhand.Events.Arguments;
using System;

namespace Farmhand.Events
{
    public class UtilityEvents
    {
        public static event EventHandler<EventArgsOnGetDwarfShopStock> OnPostGetDwarfShopStock = delegate { };

        [Hook(HookType.Exit, "StardewValley.Utility", "getDwarfShopStock")]
        internal static void InvokePostGetDwarfShopStock()
        {
            EventCommon.SafeInvoke(OnPostGetDwarfShopStock, null, new EventArgsOnGetDwarfShopStock());
        }
    }
}
