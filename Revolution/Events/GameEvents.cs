using Revolution.Events.Arguments;
using System;

namespace Revolution.Events
{
    public static class GameEvents
    {
        public static EventHandler<EventArgsOnGameInitialise> OnBeforeGameInitialised = delegate { };
        public static EventHandler<EventArgsOnGameInitialised> OnAfterGameInitialised = delegate { };

        public static void InvokeBeforeGameInitialise()
        {
            OnBeforeGameInitialised.Invoke(null, new EventArgsOnGameInitialise());
        }
        public static void InvokeAfterGameInitialise()
        {
            OnAfterGameInitialised.Invoke(null, new EventArgsOnGameInitialised());
        }
    }
}
