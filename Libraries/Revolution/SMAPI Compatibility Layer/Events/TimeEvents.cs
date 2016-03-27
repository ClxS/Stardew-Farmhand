using System;

// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Events
{
    public static class TimeEvents
    {
        public static event EventHandler<EventArgsIntChanged> TimeOfDayChanged = delegate { };
        public static event EventHandler<EventArgsIntChanged> DayOfMonthChanged = delegate { };
        public static event EventHandler<EventArgsIntChanged> YearOfGameChanged = delegate { };
        public static event EventHandler<EventArgsStringChanged> SeasonOfYearChanged = delegate { };

        public static void InvokeTimeOfDayChanged(object sender, EventArgs eventArgs)
        {
            TimeOfDayChanged.Invoke(null, new EventArgsIntChanged(0, 0));
        }

        public static void InvokeDayOfMonthChanged(object sender, EventArgs eventArgs)
        {
            DayOfMonthChanged.Invoke(null, new EventArgsIntChanged(0, 0));
        }

        public static void InvokeYearOfGameChanged(object sender, EventArgs eventArgs)
        {
            YearOfGameChanged.Invoke(null, new EventArgsIntChanged(0, 0));
        }

        public static void InvokeSeasonOfYearChanged(object sender, EventArgs eventArgs)
        {
            SeasonOfYearChanged.Invoke(null, new EventArgsStringChanged("", ""));
        }
    }
}
