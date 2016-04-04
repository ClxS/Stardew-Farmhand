using Farmhand.Events;
using System;

namespace StardewModdingAPI.Events
{
    public static class TimeEvents
    {
        public static event EventHandler<EventArgsIntChanged> TimeOfDayChanged = delegate { };
        public static event EventHandler<EventArgsIntChanged> DayOfMonthChanged = delegate { };
        public static event EventHandler<EventArgsIntChanged> YearOfGameChanged = delegate { };
        public static event EventHandler<EventArgsStringChanged> SeasonOfYearChanged = delegate { };

        /// <summary>
        /// Occurs when Game1.newDay changes. True directly before saving, and False directly after.
        /// </summary>
        public static event EventHandler<EventArgsNewDay> OnNewDay = delegate { };

        internal static void InvokeTimeOfDayChanged(object sender, EventArgs eventArgs)
        {
            EventCommon.SafeInvoke(TimeOfDayChanged, null, new EventArgsIntChanged(0, 0));
        }

        internal static void InvokeDayOfMonthChanged(object sender, EventArgs eventArgs)
        {
            EventCommon.SafeInvoke(DayOfMonthChanged, null, new EventArgsIntChanged(0, 0));
        }

        internal static void InvokeYearOfGameChanged(object sender, EventArgs eventArgs)
        {
            EventCommon.SafeInvoke(YearOfGameChanged, null, new EventArgsIntChanged(0, 0));
        }

        internal static void InvokeSeasonOfYearChanged(object sender, EventArgs eventArgs)
        {
            EventCommon.SafeInvoke(SeasonOfYearChanged, null, new EventArgsStringChanged("", ""));
        }

        internal static void InvokeOnNewDay(int priorInt, int newInt, bool newDay)
        {
            //TODO Hook this up
            EventCommon.SafeInvoke(OnNewDay, null, new EventArgsNewDay(priorInt, newInt, newDay));
        }
    }
}