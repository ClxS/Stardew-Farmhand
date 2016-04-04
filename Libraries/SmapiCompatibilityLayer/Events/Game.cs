using System;
using Farmhand.Events;
using Farmhand.Events.Arguments.GameEvents;

namespace StardewModdingAPI.Events
{
    public static class GameEvents
    {
        public static event EventHandler GameLoaded = delegate { };
        public static event EventHandler Initialize = delegate { };
        public static event EventHandler LoadContent = delegate { };
        public static event EventHandler FirstUpdateTick = delegate { };

        /// <summary>
        ///     Fires every update (1/60 of a second)
        /// </summary>
        public static event EventHandler UpdateTick = delegate { };

        /// <summary>
        ///     Fires every other update (1/30 of a second)
        /// </summary>
        public static event EventHandler SecondUpdateTick = delegate { };

        /// <summary>
        ///     Fires every fourth update (1/15 of a second)
        /// </summary>
        public static event EventHandler FourthUpdateTick = delegate { };

        /// <summary>
        ///     Fires every eighth update (roughly 1/8 of a second)
        /// </summary>
        public static event EventHandler EighthUpdateTick = delegate { };

        /// <summary>
        ///     Fires every fifthteenth update (1/4 of a second)
        /// </summary>
        public static event EventHandler QuarterSecondTick = delegate { };

        /// <summary>
        ///     Fires every thirtieth update (1/2 of a second)
        /// </summary>
        public static event EventHandler HalfSecondTick = delegate { };

        /// <summary>
        ///     Fires every sixtieth update (a second)
        /// </summary>
        public static event EventHandler OneSecondTick = delegate { };

        private static bool FirstUpdateFired { get; set; } = false;

        internal static void InvokeGameLoaded(object sender, EventArgsOnGameInitialise eventArgsOnGameInitialise)
        {
            EventCommon.SafeInvoke(GameLoaded, sender);
        }

        internal static void InvokeInitialize(object sender, EventArgsOnGameInitialised eventArgsOnGameInitialised)
        {
            try
            {
                EventCommon.SafeInvoke(Initialize, sender);
            }
            catch (Exception ex)
            {
                Log.AsyncR("An exception occured in XNA Initialize: " + ex);
            }
        }

        internal static void InvokeLoadContent(object sender, EventArgs eventArgs)
        {
            try
            {
                if (!FirstUpdateFired)
                {
                    FirstUpdateFired = true;
                    EventCommon.SafeInvoke(FirstUpdateTick, sender);
                }
                EventCommon.SafeInvoke(LoadContent, sender);
            }
            catch (Exception ex)
            {
                Log.AsyncR("An exception occured in XNA LoadContent: " + ex);
            }
        }

        internal static void InvokeUpdateTick(object sender, EventArgs eventArgs)
        {
            try
            {
                EventCommon.SafeInvoke(UpdateTick, sender);
            }
            catch (Exception ex)
            {
                Log.AsyncR("An exception occured in XNA UpdateTick: " + ex);
            }
        }

        internal static void InvokeSecondUpdateTick()
        {
            //DODO Hook this
            EventCommon.SafeInvoke(SecondUpdateTick, null);
        }

        internal static void InvokeFourthUpdateTick()
        {
            //DODO Hook this
            EventCommon.SafeInvoke(FourthUpdateTick, null);
        }

        internal static void InvokeEighthUpdateTick()
        {
            //DODO Hook this
            EventCommon.SafeInvoke(EighthUpdateTick, null);
        }

        internal static void InvokeQuarterSecondTick()
        {
            //DODO Hook this
            EventCommon.SafeInvoke(QuarterSecondTick, null);
        }

        internal static void InvokeHalfSecondTick()
        {
            //DODO Hook this
            EventCommon.SafeInvoke(HalfSecondTick, null);
        }

        internal static void InvokeOneSecondTick()
        {
            //DODO Hook this
            EventCommon.SafeInvoke(OneSecondTick, null);
        }

        internal static void InvokeFirstUpdateTick()
        {
            EventCommon.SafeInvoke(FirstUpdateTick, null);
        }
    }
}