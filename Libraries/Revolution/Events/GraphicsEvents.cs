using Revolution.Attributes;
using System;
using Revolution.Events.Arguments.GraphicsEvents;
using StardewValley;

namespace Revolution.Events
{
    public class GraphicsEvents
    {
        public static event EventHandler<EventArgsClientSizeChanged> OnResize = delegate { };
        public static event EventHandler OnBeforeDraw = delegate { };
        public static event EventHandler OnAfterDraw = delegate { };
        
        [PendingHook]
        [Hook(HookType.Entry, "StardewValley.Game1", "Window_ClientSizeChanged")]
        internal static void InvokeResize([ThisBind] Game1 @this)
        {
            EventCommon.SafeInvoke(OnResize, @this, new EventArgsClientSizeChanged(@this.Window.ClientBounds.Width, @this.Window.ClientBounds.Height));
        }
        
        [Hook(HookType.Entry, "StardewValley.Game1", "Draw")]
        internal static void InvokeBeforeDraw([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnBeforeDraw, @this);
        }
        
        [Hook(HookType.Exit, "StardewValley.Game1", "Draw")]
        internal static void InvokeAfterDraw([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnAfterDraw, @this);
        }
    }
}
