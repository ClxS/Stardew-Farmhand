using Revolution.Events.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    class PlayerEvents
    {
        public static EventHandler<EventArgsOnPlayerTakesDamage> OnPlayerTakesDamage = delegate { };
        public static EventHandler<EventArgsOnPlayerDoneEating> OnPlayerDoneEating = delegate { };

        public static void InvokeOnPlayerTakesDamage()
        {
            OnPlayerTakesDamage.Invoke(null, new EventArgsOnPlayerTakesDamage());            
        }

        public static void InvokeOnPlayerDoneEating()
        {
            OnPlayerDoneEating.Invoke(null, new EventArgsOnPlayerDoneEating());
        }
    }
}
