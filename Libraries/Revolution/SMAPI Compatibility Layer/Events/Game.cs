using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StardewModdingAPI.Events
{
    public static class GameEvents
    {
        public static event EventHandler GameLoaded = delegate { };
        public static event EventHandler Initialize = delegate { };
        public static event EventHandler LoadContent = delegate { };
        public static event EventHandler UpdateTick = delegate { };

        public static void InvokeGameLoaded()
        {
            GameLoaded.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeInitialize()
        {
            try
            {
                Initialize.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("An exception occured in XNA Initialize: " + ex.ToString());
            }
        }

        public static void InvokeLoadContent()
        {
            try
            {
                LoadContent.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("An exception occured in XNA LoadContent: " + ex.ToString());
            }
        }

        public static void InvokeUpdateTick()
        {
            try
            {
                UpdateTick.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.Error("An exception occured in XNA UpdateTick: " + ex.ToString());
            }
        }
    }
}
