using Revolution.Events.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    public class PlayerEvents
    {
        public static EventHandler<EventArgsOnPlayerTakesDamage> OnPlayerTakesDamage = delegate { };
        public static EventHandler<EventArgsOnPlayerDoneEating> OnPlayerDoneEating = delegate { };
        public static event EventHandler OnFarmerChanged = delegate { };
        public static event EventHandler OnInventoryChanged = delegate { };
        public static event EventHandler OnLeveledUp = delegate { };

        public static void InvokeOnPlayerTakesDamage()
        {
            OnPlayerTakesDamage.Invoke(null, new EventArgsOnPlayerTakesDamage());            
        }

        public static void InvokeOnPlayerDoneEating()
        {
            OnPlayerDoneEating.Invoke(null, new EventArgsOnPlayerDoneEating());
        }

        public static void InvokeFarmerChanged()
        {
            OnFarmerChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeInventoryChanged()
        {
            OnInventoryChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeLeveledUp()
        {
            OnLeveledUp.Invoke(null, EventArgs.Empty);
        }
    }
}
