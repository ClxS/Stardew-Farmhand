using Revolution.Attributes;
using Revolution.Events.Arguments;
using System;
using Revolution.Events.Arguments.PlayerEvents;
using StardewValley;

namespace Revolution.Events
{
    public class PlayerEvents
    {
        public static event EventHandler<EventArgsOnBeforePlayerTakesDamage> OnBeforePlayerTakesDamage = delegate { };
        public static event EventHandler<EventArgsOnAfterPlayerTakesDamage> OnAfterPlayerTakesDamage = delegate { };
        public static event EventHandler<EventArgsOnPlayerDoneEating> OnPlayerDoneEating = delegate { };
        public static event EventHandler<EventArgsOnItemAddedToInventory> OnItemAddedToInventory = delegate { };
        public static event EventHandler OnFarmerChanged = delegate { };
        public static event EventHandler OnLevelUp = delegate { };
        
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
        
        [Hook(HookType.Exit, "StardewValley.Farmer", ".ctor")]
        internal static void InvokeFarmerChanged()
        {
            EventCommon.SafeInvoke(OnFarmerChanged, null);
        }
        
        [Hook(HookType.Exit, "StardewValley.Farmer", "addItemToInventory")]
        internal static bool InvokeItemAddedToInventory(
            [ThisBind] object @this,
            [InputBind(typeof(Item), "item")] Item item)
        {
            return EventCommon.SafeCancellableInvoke(OnItemAddedToInventory, @this, new EventArgsOnItemAddedToInventory(item));
        }
        
        [PendingHook]
        internal static void InvokeLevelUp()
        {
            EventCommon.SafeInvoke(OnLevelUp, null);
        }  

    }
}
