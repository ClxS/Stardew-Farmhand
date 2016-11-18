using Farmhand.Attributes;
using Farmhand.Events.Arguments.LocationEvents;
using StardewValley;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events relating to (in-game) events
    /// </summary>
    public static class EventEvents
    {
        public static event EventHandler OnEventFinished = delegate { };
        
        [Hook(HookType.Entry, "StardewValley.Event", "exitEvent")]
        internal static void InvokeOnBeforeLocationLoadObjects([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnEventFinished, @this);
        }
    }
}
