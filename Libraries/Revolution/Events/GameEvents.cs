using Revolution.Attributes;
using Revolution.Events.Arguments;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Revolution.Events
{
    public static class GameEvents
    {
        public static EventHandler<EventArgsOnGameInitialise> OnBeforeGameInitialised = delegate { };
        public static EventHandler<EventArgsOnGameInitialised> OnAfterGameInitialised = delegate { };
        public static event EventHandler OnBeforeLoadContent = delegate { };
        public static event EventHandler OnAfterLoadedContent = delegate { };
        public static event EventHandler OnBeforeUnoadContent = delegate { };
        public static event EventHandler OnAfterUnloadedContent = delegate { };
        public static event EventHandler<CancelEventArgs> OnBeforeUpdateTick = delegate { };
        public static event EventHandler OnAfterUpdateTick = delegate { };
        
        [Hook(HookType.Entry, "StardewValley.Game1", "Initialize")]
        internal static void InvokeBeforeGameInitialise()
        {
            EventCommon.SafeInvoke(OnBeforeGameInitialised, null, new EventArgsOnGameInitialise());
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "Initialize")]
        internal static void InvokeAfterGameInitialise()
        {
            EventCommon.SafeInvoke(OnAfterGameInitialised, null, new EventArgsOnGameInitialised());
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "LoadContent")]
        internal static void InvokeBeforeLoadContent()
        {
            EventCommon.SafeInvoke(OnBeforeLoadContent, null);
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "LoadContent")]
        internal static void InvokeAfterLoadedContent()
        {
            EventCommon.SafeInvoke(OnAfterLoadedContent, null);
        }
                
        [Hook(HookType.Entry, "StardewValley.Game1", "UnloadContent")]
        internal static void InvokeBeforeUnloadContent()
        {
            EventCommon.SafeInvoke(OnBeforeUnoadContent, null);
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "UnloadContent")]
        internal static void InvokeAfterUnloadedContent()
        {
            EventCommon.SafeInvoke(OnAfterUnloadedContent, null);
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "Update")]
        internal static bool InvokeBeforeUpdate()
        {
            return EventCommon.SafeCancellableInvoke(OnBeforeUpdateTick, null, new CancelEventArgs());
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "Update")]
        internal static void InvokeAfterUpdate()
        {
            EventCommon.SafeInvoke(OnAfterUpdateTick, null);
        }
    }
}
