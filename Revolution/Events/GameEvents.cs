using Revolution.Events.Arguments;
using System;

namespace Revolution.Events
{
    public static class GameEvents
    {
        public static EventHandler<EventArgsOnGameInitialise> OnBeforeGameInitialised = delegate { };
        public static EventHandler<EventArgsOnGameInitialised> OnAfterGameInitialised = delegate { };
        public static event EventHandler OnBeforeLoadContent = delegate { };
        public static event EventHandler OnAfterLoadedContent = delegate { };
        public static event EventHandler OnBeforeUpdateTick = delegate { };
        public static event EventHandler OnAfterUpdateTick = delegate { };

        public static void InvokeBeforeGameInitialise()
        {
            OnBeforeGameInitialised.Invoke(null, new EventArgsOnGameInitialise());
        }
        public static void InvokeAfterGameInitialise()
        {
            OnAfterGameInitialised.Invoke(null, new EventArgsOnGameInitialised());
        }
        public static void InvokeBeforeLoadContent()
        {
            OnBeforeLoadContent.Invoke(null, EventArgs.Empty);
        }
        public static void InvokeAfterLoadedContent()
        {
            OnAfterLoadedContent.Invoke(null, EventArgs.Empty);
        }
        public static void InvokeBeforeUpdate()
        {
            OnBeforeUpdateTick.Invoke(null, EventArgs.Empty);
        }
        public static void InvokeAfterUpdate()
        {
            OnAfterUpdateTick.Invoke(null, EventArgs.Empty);
        }
    }
}
