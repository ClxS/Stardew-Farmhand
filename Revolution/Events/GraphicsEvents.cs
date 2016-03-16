using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    class GraphicsEvents
    {
        public static event EventHandler OnResize = delegate { };
        public static event EventHandler OnBeforeDraw = delegate { };
        public static event EventHandler OnAfterDraw = delegate { };

        public static void InvokeResize()
        {
            OnResize.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeBeforeDraw(object sender, EventArgs e)
        {
            OnBeforeDraw.Invoke(sender, EventArgs.Empty);
        }

        public static void InvokeAfterDraw(object sender, EventArgs e)
        {
            OnAfterDraw.Invoke(sender, EventArgs.Empty);
        }
    }
}
