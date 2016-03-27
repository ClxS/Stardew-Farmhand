using Revolution.Attributes;
using System;

namespace Revolution.Events
{
    public class GraphicsEvents
    {
        public static event EventHandler OnResize = delegate { };
        public static event EventHandler OnBeforeDraw = delegate { };
        public static event EventHandler OnAfterDraw = delegate { };
        
        [PendingHook]
        internal static void InvokeResize()
        {
            EventCommon.SafeInvoke(OnResize, null);
        }
        
        [Hook(HookType.Entry, "StardewValley.Game1", "draw")]
        internal static void InvokeBeforeDraw([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnBeforeDraw, @this);
        }
        
        [Hook(HookType.Exit, "StardewValley.Game1", "draw")]
        internal static void InvokeAfterDraw([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnAfterDraw, @this);
        }
    }
}
