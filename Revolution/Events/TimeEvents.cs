using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    class TimeEvents
    {
        public static event EventHandler OnTimeOfDayChanged = delegate { };
        public static event EventHandler OnDayOfMonthChanged = delegate { };
        public static event EventHandler OnYearOfGameChanged = delegate { };
        public static event EventHandler OnSeasonOfYearChanged = delegate { };

        public static void InvokeTimeOfDayChanged(Int32 priorInt, Int32 newInt)
        {
            OnTimeOfDayChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeDayOfMonthChanged(Int32 priorInt, Int32 newInt)
        {
            OnDayOfMonthChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeYearOfGameChanged(Int32 priorInt, Int32 newInt)
        {
            OnYearOfGameChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeSeasonOfYearChanged(String priorString, String newString)
        {
            OnSeasonOfYearChanged.Invoke(null, EventArgs.Empty);
        }
    }
}
