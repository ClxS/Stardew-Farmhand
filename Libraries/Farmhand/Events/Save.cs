using System;
using Farmhand.Attributes;
using Farmhand.Events.Arguments.SaveEvents;
using System.Collections.Generic;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events related to saving/loading
    /// </summary>
    public static class SaveEvents
    {
        /// <summary>
        /// Triggered prior to saving
        /// </summary>
        public static event EventHandler OnBeforeSave = delegate { };

        /// <summary>
        /// Triggered after progress towards saving is made. 100 is complete
        /// </summary>
        public static event EventHandler<EventArgsOnAfterSaveProgress> OnAfterSaveProgress = delegate { };

        /// <summary>
        /// Triggered prior to loading
        /// </summary>
        public static event EventHandler<EventArgsOnBeforeLoad> OnBeforeLoad = delegate { };

        /// <summary>
        /// Triggered after progress towards loading is made. 100 is complete
        /// </summary>
        public static event EventHandler<EventArgsOnAfterLoadProgress> OnAfterLoadProgress = delegate { };

        [Hook(HookType.Entry, "StardewValley.SaveGame", "Save")]
        internal static void InvokeOnBeforeSave()
        {
            EventCommon.SafeInvoke(OnBeforeSave, null);
        }

        [Hook(HookType.Exit, "StardewValley.SaveGame/<getSaveEnumerator>d__46", "MoveNext")]
        internal static void InvokeOnAfterSaveProgress([ThisBind] IEnumerator<int> @this)
        {
            EventCommon.SafeInvoke(OnAfterSaveProgress, null, new EventArgsOnAfterSaveProgress(@this.Current));
        }

        [Hook(HookType.Entry, "StardewValley.SaveGame", "Load")]
        internal static bool InvokeOnBeforeLoad([InputBind(typeof(string), "filename")] string filename)
        {
            return EventCommon.SafeCancellableInvoke(OnBeforeLoad, null, new EventArgsOnBeforeLoad(filename));
        }

        [Hook(HookType.Exit, "StardewValley.SaveGame/<getLoadEnumerator>d__51", "MoveNext")]
        internal static void InvokeOnAfterLoadProgress([ThisBind] IEnumerator<int> @this)
        {
            try
            {
                string filename = (string)@this.GetType().GetField("file").GetValue(@this);
                EventCommon.SafeInvoke(OnAfterLoadProgress, null, new EventArgsOnAfterLoadProgress(filename, @this.Current));
            }
            catch(Exception e) { Logging.Log.Exception("EXCEPTION b:", e); }
        }
    }
}
