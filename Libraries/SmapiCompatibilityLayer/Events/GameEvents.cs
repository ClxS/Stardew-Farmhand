using System;
using Revolution.Events.Arguments.GameEvents;

// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Events
{
    public static class GameEvents
    {
        public static event EventHandler GameLoaded = delegate { };
        public static event EventHandler Initialize = delegate { };
        public static event EventHandler LoadContent = delegate { };
        public static event EventHandler UpdateTick = delegate { };

        public static void InvokeGameLoaded(object sender, EventArgsOnGameInitialise eventArgsOnGameInitialise)
        {
            GameLoaded.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeInitialize(object sender, EventArgsOnGameInitialised eventArgsOnGameInitialised)
        {
            try
            {
                Initialize.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Revolution.Logging.Log.Error("An exception occured in XNA Initialize: " + ex);
            }
        }

        public static void InvokeLoadContent(object sender, EventArgs eventArgs)
        {
            try
            {
                LoadContent.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Revolution.Logging.Log.Error("An exception occured in XNA LoadContent: " + ex);
            }
        }

        public static void InvokeUpdateTick(object sender, EventArgs eventArgs)
        {
            try
            {
                UpdateTick.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Revolution.Logging.Log.Error("An exception occured in XNA UpdateTick: " + ex);
            }
        }
    }
}
