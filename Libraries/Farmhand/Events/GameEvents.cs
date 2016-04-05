using Farmhand.Attributes;
using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Farmhand.Events.Arguments.GameEvents;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events relating to the main game state
    /// </summary>
    public static class GameEvents
    {
        public static EventHandler<EventArgsOnGameInitialise> OnBeforeGameInitialised = delegate { };
        public static EventHandler<EventArgsOnGameInitialised> OnAfterGameInitialised = delegate { };
        public static event EventHandler OnBeforeLoadContent = delegate { };
        public static event EventHandler OnAfterLoadedContent = delegate { };
        public static event EventHandler OnBeforeUnoadContent = delegate { };
        public static event EventHandler OnAfterUnloadedContent = delegate { };
        public static event EventHandler<EventArgsOnBeforeGameUpdate> OnBeforeUpdateTick = delegate { };
        public static event EventHandler OnAfterUpdateTick = delegate { };
        
        [Hook(HookType.Entry, "StardewValley.Game1", "Initialize")]
        internal static void InvokeBeforeGameInitialise([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnBeforeGameInitialised, @this, new EventArgsOnGameInitialise());
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "Initialize")]
        internal static void InvokeAfterGameInitialise([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnAfterGameInitialised, @this, new EventArgsOnGameInitialised());
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "LoadContent")]
        internal static void InvokeBeforeLoadContent([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnBeforeLoadContent, @this);
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "LoadContent")]
        internal static void InvokeAfterLoadedContent([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnAfterLoadedContent, @this);
        }
                
        [Hook(HookType.Entry, "StardewValley.Game1", "UnloadContent")]
        internal static void InvokeBeforeUnloadContent([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnBeforeUnoadContent, @this);
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "UnloadContent")]
        internal static void InvokeAfterUnloadedContent([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnAfterUnloadedContent, @this);
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "Update")]
        internal static bool InvokeBeforeUpdate(
            [ThisBind] object @this, 
            [InputBind(typeof(GameTime), "gameTime")] GameTime gt)
        {
            return EventCommon.SafeCancellableInvoke(OnBeforeUpdateTick, @this, new EventArgsOnBeforeGameUpdate(gt));
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "Update")]
        internal static void InvokeAfterUpdate([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnAfterUpdateTick, @this);
        }
    }
}
