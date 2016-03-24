using Revolution.Attributes;
using Revolution.Events.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    public class PlayerEvents
    {
        public static EventHandler<EventArgsOnBeforePlayerTakesDamage> OnBeforePlayerTakesDamage = delegate { };
        public static EventHandler<EventArgsOnAfterPlayerTakesDamage> OnAfterPlayerTakesDamage = delegate { };
        public static EventHandler<EventArgsOnPlayerDoneEating> OnPlayerDoneEating = delegate { };
        public static event EventHandler OnFarmerChanged = delegate { };
        public static event EventHandler OnInventoryChanged = delegate { };
        public static event EventHandler OnLevelUp = delegate { };
        
        [Hook(HookType.Entry, "StardewValley.Game1", "farmerTakeDamage")]
        internal static void InvokeBeforePlayerTakesDamage()
        {
            EventCommon.SafeCancellableInvoke(OnBeforePlayerTakesDamage, null, new EventArgsOnBeforePlayerTakesDamage());
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
        
        [PendingHook]
        internal static void InvokeFarmerChanged()
        {
            EventCommon.SafeInvoke(OnFarmerChanged, null);
        }
        
        [Hook(HookType.Exit, "StardewValley.Game1", "addItemToInventory")]
        internal static void InvokeInventoryChanged()
        {
            EventCommon.SafeInvoke(OnInventoryChanged, null);
        }
        
        [PendingHook]
        internal static void InvokeLevelUp()
        {
            EventCommon.SafeInvoke(OnLevelUp, null);
        }  

    }
}
