using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StardewModdingAPI.Events
{
    public static class TimeEvents
    {
        public static event EventHandler<EventArgsIntChanged> TimeOfDayChanged = delegate { };
        public static event EventHandler<EventArgsIntChanged> DayOfMonthChanged = delegate { };
        public static event EventHandler<EventArgsIntChanged> YearOfGameChanged = delegate { };
        public static event EventHandler<EventArgsStringChanged> SeasonOfYearChanged = delegate { };

        public static void InvokeTimeOfDayChanged(Int32 priorInt, Int32 newInt)
        {
            TimeOfDayChanged.Invoke(null, new EventArgsIntChanged(priorInt, newInt));
        }

        public static void InvokeDayOfMonthChanged(Int32 priorInt, Int32 newInt)
        {
            DayOfMonthChanged.Invoke(null, new EventArgsIntChanged(priorInt, newInt));
        }

        public static void InvokeYearOfGameChanged(Int32 priorInt, Int32 newInt)
        {
            YearOfGameChanged.Invoke(null, new EventArgsIntChanged(priorInt, newInt));
        }

        public static void InvokeSeasonOfYearChanged(String priorString, String newString)
        {
            SeasonOfYearChanged.Invoke(null, new EventArgsStringChanged(priorString, newString));
        }
    }
}
