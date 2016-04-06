using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Farmhand.Attributes;
using Farmhand.Events.Arguments.SaveEvents;
using Farmhand.Logging;

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
        /// Triggered after saving
        /// </summary>
        public static event EventHandler OnAfterSave = delegate { };

        /// <summary>
        /// Triggered prior to loading
        /// </summary>
        public static event EventHandler<EventArgsOnBeforeLoad> OnBeforeLoad = delegate { };

        /// <summary>
        /// Triggered afer loading
        /// </summary>
        public static event EventHandler<EventArgsOnAfterLoad> OnAfterLoad = delegate { };

        [Hook(HookType.Entry, "StardewValley.SaveGame", "Save")]
        internal static void InvokeOnBeforeSave()
        {
            EventCommon.SafeInvoke(OnBeforeSave, null);
        }

        [Hook(HookType.Exit, "StardewValley.SaveGame", "Save")]
        internal static void InvokeOnAfterSave()
        {
            EventCommon.SafeInvoke(OnAfterSave, null);
        }

        [Hook(HookType.Entry, "StardewValley.SaveGame", "Load")]
        internal static bool InvokeOnBeforeLoad([InputBind(typeof(string), "filename")] string filename)
        {
            return EventCommon.SafeCancellableInvoke(OnBeforeLoad, null, new EventArgsOnBeforeLoad(filename));
        }

        [Hook(HookType.Exit, "StardewValley.SaveGame", "Load")]
        internal static void InvokeOnAfterLoad([InputBind(typeof(string), "filename")] string filename)
        {
            EventCommon.SafeInvoke(OnAfterLoad, null, new EventArgsOnAfterLoad(filename));
        }
    }
}
