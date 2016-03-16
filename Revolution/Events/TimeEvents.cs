using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    public class TimeEvents
    {
        public static event EventHandler OnBeforeTimeOfDayChanged = delegate { };
        public static event EventHandler OnAfterTimeOfDayChanged = delegate { };
        public static event EventHandler OnDayOfMonthChanged = delegate { };
        public static event EventHandler OnYearOfGameChanged = delegate { };
        public static event EventHandler OnSeasonOfYearChanged = delegate { };

        public static void InvokeBeforeTimeOfDayChanged(Int32 priorInt, Int32 newInt)
        {
            OnBeforeTimeOfDayChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeAfterTimeOfDayChanged(Int32 priorInt, Int32 newInt)
        {
            OnAfterTimeOfDayChanged.Invoke(null, EventArgs.Empty);
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
