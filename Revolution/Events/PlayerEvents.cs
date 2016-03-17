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
        public static void InvokeBeforePlayerTakesDamage()
        {
            OnBeforePlayerTakesDamage.Invoke(null, new EventArgsOnBeforePlayerTakesDamage());            
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "farmerTakeDamage")]
        public static void InvokeAfterPlayerTakesDamage()
        {
            OnAfterPlayerTakesDamage.Invoke(null, new EventArgsOnAfterPlayerTakesDamage());
        }

        [PendingHook]
        public static void InvokeOnPlayerDoneEating()
        {
            OnPlayerDoneEating.Invoke(null, new EventArgsOnPlayerDoneEating());
        }

        [PendingHook]
        public static void InvokeFarmerChanged()
        {
            OnFarmerChanged.Invoke(null, EventArgs.Empty);
        }

        [PendingHook]
        public static void InvokeInventoryChanged()
        {
            OnInventoryChanged.Invoke(null, EventArgs.Empty);
        }
        
        [PendingHook]
        public static void InvokeLevelUp()
        {
            OnLevelUp.Invoke(null, EventArgs.Empty);
        }  

    }
}
