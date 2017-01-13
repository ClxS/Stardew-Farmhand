namespace Farmhand.Events
{
    using System;

    using Farmhand.Attributes;

    /// <summary>
    ///     Contains events relating to (in-game) events
    /// </summary>
    public static class EventEvents
    {
        /// <summary>
        ///     Fires upon exiting an in-game event.
        /// </summary>
        public static event EventHandler EventFinished = delegate { };

        [Hook(HookType.Entry, "StardewValley.Event", "exitEvent")]
        internal static void OnBeforeLocationLoadObjects([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(EventFinished, @this);
        }
    }
}