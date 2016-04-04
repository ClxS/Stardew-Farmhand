using Farmhand.Attributes;
using Farmhand.Events.Arguments;
using System;
using System.ComponentModel;
using Farmhand.Events.Arguments.PlayerEvents;
using Farmhand.Logging;
using StardewValley;

namespace Farmhand.Events
{
    public class PlayerEvents
    {
        public static event EventHandler<EventArgsOnBeforePlayerTakesDamage> OnBeforePlayerTakesDamage = delegate { };
        public static event EventHandler<EventArgsOnAfterPlayerTakesDamage> OnAfterPlayerTakesDamage = delegate { };
        public static event EventHandler<EventArgsOnPlayerDoneEating> OnPlayerDoneEating = delegate { };
        public static event EventHandler<EventArgsOnItemAddedToInventory> OnItemAddedToInventory = delegate { };
        public static event EventHandler<CancelEventArgs> OnBeforeGainExperience = delegate { };
        public static event EventHandler OnAfterGainExperience = delegate { };
        public static event EventHandler OnFarmerChanged = delegate { };
        public static event EventHandler<EventArgsOnLevelUp> OnLevelUp = delegate { };
        
        [Hook(HookType.Entry, "StardewValley.Game1", "farmerTakeDamage")]
        internal static bool InvokeBeforePlayerTakesDamage()
        {
            return EventCommon.SafeCancellableInvoke(OnBeforePlayerTakesDamage, null, new EventArgsOnBeforePlayerTakesDamage());
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "farmerTakeDamage")]
        internal static void InvokeAfterPlayerTakesDamage()
        {
            EventCommon.SafeInvoke(OnAfterPlayerTakesDamage, null, new EventArgsOnAfterPlayerTakesDamage());
        }
        
        [Hook(HookType.Exit, "StardewValley.Game1", "doneEating")]
        internal static void InvokeOnPlayerDoneEating()
        {
            EventCommon.SafeInvoke(OnPlayerDoneEating, null, new EventArgsOnPlayerDoneEating());
        }
        
        internal static void InvokeFarmerChanged(Farmer priorFarmer, Farmer newFarmer)
        {
            EventCommon.SafeInvoke(OnFarmerChanged, newFarmer);
        }

        //[Hook(HookType.Exit, "StardewValley.Farmer", "addItemToInventory")]
        //internal static bool InvokeItemAddedToInventory(
        //    [ThisBind] object @this,
        //    [InputBind(typeof(Item), "item")] Item item)
        //{
        //    return EventCommon.SafeCancellableInvoke(OnItemAddedToInventory, @this, new EventArgsOnItemAddedToInventory(item));
        //}

        [Hook(HookType.Entry, "StardewValley.Farmer", "gainExperience")]
        internal static bool InvokeOnBeforeGainExperience([ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which,
            [InputBind(typeof(int), "howMuch")] int howMuch)
        {
            return EventCommon.SafeCancellableInvoke(OnBeforeGainExperience, @this, new CancelEventArgs());
        }

        [Hook(HookType.Exit, "StardewValley.Farmer", "gainExperience")]
        internal static void InvokeLevelUp([ThisBind] object @this,
            [InputBind(typeof(int), "which")] int which,
            [LocalBind(typeof(int), 1)] int originalLevel,
            [LocalBind(typeof(int), 0)] int newLevel
        )
        {
            EventCommon.SafeInvoke(OnLevelUp, @this, new EventArgsOnLevelUp(which, newLevel, originalLevel));
        }

    }
}
