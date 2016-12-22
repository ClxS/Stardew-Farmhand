using Farmhand.Attributes;
using Farmhand.Logging;
using Farmhand.Events.Arguments.TimeEvents;
using System;
using System.Diagnostics.CodeAnalysis;
using Farmhand.Events.Arguments.Common;
using StardewValley;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events relating to time
    /// </summary>
    public static class TimeEvents
    {
#pragma warning disable 67
        public static event EventHandler<EventArgsIntChanged> OnBeforeTimeChanged = delegate { };
        public static event EventHandler<EventArgsIntChanged> OnAfterTimeChanged = delegate { };
        public static event EventHandler OnBeforeDayChanged = delegate { };
        public static event EventHandler OnAfterDayChanged = delegate { };
        public static event EventHandler OnBeforeSeasonChanged = delegate { };
        public static event EventHandler OnAfterSeasonChanged = delegate { };

        public static event EventHandler OnBeforeYearChanged = delegate { };
        public static event EventHandler OnAfterYearChanged = delegate { };

        public static event EventHandler<EventArgsShouldTimePassCheck> ShouldTimePassCheck = delegate { };
#pragma warning restore 67

        [Hook(HookType.Entry, "StardewValley.Game1", "performTenMinuteClockUpdate")]
        internal static void InvokeBeforeTimeChanged()
        {
            var newTime = Game1.timeOfDay + 10;
            if (newTime % 100 >= 60)
                newTime = newTime - (newTime % 100) + 100;
            
            EventCommon.SafeInvoke(OnBeforeTimeChanged, null, new EventArgsIntChanged(Game1.timeOfDay, newTime));
        }

        [Hook(HookType.Exit, "StardewValley.Game1", "performTenMinuteClockUpdate")]
        internal static void InvokeAfterTimeChanged()
        {
            var oldTime = Game1.timeOfDay - 10;
            EventCommon.SafeInvoke(OnAfterTimeChanged, null, new EventArgsIntChanged(oldTime, Game1.timeOfDay));
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
        }

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

        //[PendingHook]
        //internal static void InvokeBeforeYearChanged() 
        //{
        //    throw new NotImplementedException();
        //}

        internal static bool DidShouldTimePassCheckThisFrame;
        internal static bool PrevTimePassResult;
        [HookReturnable(HookType.Exit, "StardewValley.Game1", "shouldTimePass")]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        internal static bool ShouldTimePass(
            [UseOutputBind] ref bool useOutput,
            [MethodOutputBind] bool shouldPass )
        {
            if ( !DidShouldTimePassCheckThisFrame )
            {
                var ev = new EventArgsShouldTimePassCheck(shouldPass);
                EventCommon.SafeInvoke(ShouldTimePassCheck, null, ev);
                PrevTimePassResult = ev.TimeShouldPass;
                DidShouldTimePassCheckThisFrame = true;
            }

            useOutput = true;
            return PrevTimePassResult;
        }
    }
}
