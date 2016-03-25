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
        
        [PendingHook]
        internal static void InvokeBeforeDraw(object sender, EventArgs e)
        {
            EventCommon.SafeInvoke(OnBeforeDraw, null);
        }
        
        [PendingHook]
        internal static void InvokeAfterDraw(object sender, EventArgs e)
        {
            EventCommon.SafeInvoke(OnAfterDraw, null);
        }
    }
}
