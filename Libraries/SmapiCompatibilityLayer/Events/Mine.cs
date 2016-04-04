using Farmhand.Events;
using System;

namespace StardewModdingAPI.Events
{
    public static class MineEvents
    {
        public static event EventHandler<EventArgsMineLevelChanged> MineLevelChanged = delegate { };

        internal static void InvokeMineLevelChanged(int previousMinelevel, int currentMineLevel)
        {
            //TODO Hook this up
            EventCommon.SafeInvoke(MineLevelChanged, null, new EventArgsMineLevelChanged(previousMinelevel, currentMineLevel));
        }
    }
}