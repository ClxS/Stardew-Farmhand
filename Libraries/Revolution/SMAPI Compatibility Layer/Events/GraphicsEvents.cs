using System;

// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Events
{
    public static class GraphicsEvents
    {
        public static event EventHandler Resize = delegate { };
        public static event EventHandler DrawTick = delegate { };

        public static void InvokeDrawTick()
        {
            try
            {
                DrawTick.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("An exception occured in XNA DrawTick: " + ex);
            }
        }

        public static void InvokeResize(object sender, EventArgs e)
        {
            Resize.Invoke(sender, e);
        }
    }
}
