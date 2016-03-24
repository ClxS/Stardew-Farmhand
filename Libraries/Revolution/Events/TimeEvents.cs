using Revolution.Attributes;
using Revolution.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    public class TimeEvents
    {
        public static event EventHandler OnBeforeTimeChanged = delegate { };
        public static event EventHandler OnAfterTimeChanged = delegate { };
        public static event EventHandler OnBeforeDayChanged = delegate { };
        public static event EventHandler OnAfterDayChanged = delegate { };
        public static event EventHandler OnBeforeSeasonChanged = delegate { };
        public static event EventHandler OnAfterSeasonChanged = delegate { };

        public static event EventHandler OnBeforeYearChanged = delegate { };
        public static event EventHandler OnAfterYearChanged = delegate { };

        [Hook(HookType.Entry, "StardewValley.Game1", "performTenMinuteClockUpdate")]
        internal static void InvokeBeforeTimeChanged()
        {
            try
            {
                EventCommon.SafeInvoke(OnBeforeTimeChanged, null);
            }
            catch (System.Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        /*[Hook(HookType.Exit, "StardewValley.Game1", "performTenMinuteClockUpdate")]
        internal static void InvokeAfterTimeChanged()
        {
            EventCommon.SafeInvoke(OnAfterTimeChanged, null);
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "newDayAfterFade")]
        internal static void InvokeBeforeDayChanged()
        {
            EventCommon.SafeInvoke(OnBeforeDayChanged, null);
        }
        [Hook(HookType.Exit, "StardewValley.Game1", "newDayAfterFade")]
        internal static void InvokeAfterDayChanged()
        {
            EventCommon.SafeInvoke(OnAfterDayChanged, null);
        }*/

        [Hook(HookType.Entry, "StardewValley.Game1", "newSeason")]
        internal static void InvokeBeforeSeasonChanged()
        {
            EventCommon.SafeInvoke(OnBeforeSeasonChanged, null);
        }
        [Hook(HookType.Exit, "StardewValley.Game1", "newSeason")]
        internal static void InvokeAfterSeasonChanged()
        {
            EventCommon.SafeInvoke(OnAfterSeasonChanged, null);
        }
        
        [PendingHook]
        internal static void InvokeBeforeYearChanged() 
        {
            throw new NotImplementedException();
        }
        
        [PendingHook]
        internal static void InvokeAfterYearChanged()
        {
            throw new NotImplementedException();
        }
    }
}
