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
        internal static bool InvokeBeforePlayerTakesDamage()
        {
            var args = new EventArgsOnBeforePlayerTakesDamage();
            OnBeforePlayerTakesDamage.Invoke(null, args);
            return args.Cancel;
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "farmerTakeDamage")]
        internal static void InvokeAfterPlayerTakesDamage()
        {
            OnAfterPlayerTakesDamage.Invoke(null, new EventArgsOnAfterPlayerTakesDamage());
        }
        
        [Hook(HookType.Exit, "StardewValley.Game1", "doneEating")]
        internal static void InvokeOnPlayerDoneEating()
        {
            OnPlayerDoneEating.Invoke(null, new EventArgsOnPlayerDoneEating());
        }
        
        [PendingHook]
        internal static void InvokeFarmerChanged()
        {
            OnFarmerChanged.Invoke(null, EventArgs.Empty);
        }
        
        [Hook(HookType.Exit, "StardewValley.Game1", "addItemToInventory")]
        internal static void InvokeInventoryChanged()
        {
            OnInventoryChanged.Invoke(null, EventArgs.Empty);
        }
        
        [PendingHook]
        internal static void InvokeLevelUp()
        {
            OnLevelUp.Invoke(null, EventArgs.Empty);
        }  

    }
}
