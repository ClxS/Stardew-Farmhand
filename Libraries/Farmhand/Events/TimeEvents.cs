namespace Farmhand.Events
{
    using System;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.Common;
    using Farmhand.Events.Arguments.TimeEvents;

    using StardewValley;

    /// <summary>
    ///     Contains events relating to time.
    /// </summary>
    public static class TimeEvents
    {
        internal static bool DidShouldTimePassCheckThisFrame { get; set; }

        internal static bool PreviousTimePassResult { get; set; }

        /// <summary>
        ///     Fired prior to time changes.
        /// </summary>
        public static event EventHandler<EventArgsIntChanged> BeforeTimeChanged = delegate { };

        /// <summary>
        ///     Fired after time changes.
        /// </summary>
        public static event EventHandler<EventArgsIntChanged> AfterTimeChanged = delegate { };

        /// <summary>
        ///     Fired before day changes.
        /// </summary>
        public static event EventHandler BeforeDayChanged = delegate { };

        /// <summary>
        ///     Fired after day changes.
        /// </summary>
        public static event EventHandler AfterDayChanged = delegate { };

        /// <summary>
        ///     Fired just before season changes.
        /// </summary>
        public static event EventHandler BeforeSeasonChanged = delegate { };

        /// <summary>
        ///     Fired just after season changes.
        /// </summary>
        public static event EventHandler AfterSeasonChanged = delegate { };

        /// <summary>
        ///     Fired just before year changes.
        /// </summary>
        /// <remarks>
        ///     TODO: Not yet implemented
        /// </remarks>
        public static event EventHandler BeforeYearChanged = delegate { };

        /// <summary>
        ///     Fired just after year changes.
        /// </summary>
        /// <remarks>
        ///     TODO: Not yet implemented
        /// </remarks>
        public static event EventHandler AfterYearChanged = delegate { };

        /// <summary>
        ///     Fires when the game checks whether time should pass.
        /// </summary>
        /// <remarks>
        ///     You can use this event to prevent the game's clock from progressing.
        /// </remarks>
        public static event EventHandler<EventArgsShouldTimePassCheck> ShouldTimePassCheck = delegate { };

        [Hook(HookType.Entry, "StardewValley.Game1", "performTenMinuteClockUpdate")]
        internal static void OnBeforeTimeChanged()
        {
            var newTime = Game1.timeOfDay + 10;
            if (newTime % 100 >= 60)
            {
                newTime = newTime - newTime % 100 + 100;
            }

            EventCommon.SafeInvoke(BeforeTimeChanged, null, new EventArgsIntChanged(Game1.timeOfDay, newTime));
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "performTenMinuteClockUpdate")]
        internal static void OnAfterTimeChanged()
        {
            var oldTime = Game1.timeOfDay - 10;
            EventCommon.SafeInvoke(AfterTimeChanged, null, new EventArgsIntChanged(oldTime, Game1.timeOfDay));
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "newDayAfterFade")]
        internal static void OnBeforeDayChanged()
        {
            EventCommon.SafeInvoke(BeforeDayChanged, null);
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "newDayAfterFade")]
        internal static void OnAfterDayChanged()
        {
            EventCommon.SafeInvoke(AfterDayChanged, null);
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "newSeason")]
        internal static void OnBeforeSeasonChanged()
        {
            EventCommon.SafeInvoke(BeforeSeasonChanged, null);
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "newSeason")]
        internal static void OnAfterSeasonChanged()
        {
            EventCommon.SafeInvoke(AfterSeasonChanged, null);
        }

        [HookReturnable(HookType.Exit, "StardewValley.Game1", "shouldTimePass")]
        internal static bool OnShouldTimePass([UseOutputBind] out bool useOutput, [MethodOutputBind] bool shouldPass)
        {
            if (!DidShouldTimePassCheckThisFrame)
            {
                var ev = new EventArgsShouldTimePassCheck(shouldPass);
                EventCommon.SafeInvoke(ShouldTimePassCheck, null, ev);
                PreviousTimePassResult = ev.TimeShouldPass;
                DidShouldTimePassCheckThisFrame = true;
            }

            useOutput = true;
            return PreviousTimePassResult;
        }

        [PendingHook]
        internal static void OnBeforeYearChanged()
        {
            BeforeYearChanged(null, EventArgs.Empty);
        }

        [PendingHook]
        internal static void OnAfterYearChanged()
        {
            AfterYearChanged(null, EventArgs.Empty);
        }
    }
}