using System;

namespace StardewModdingAPI.Events
{
    public static class MineEvents
    {
        public static event EventHandler<EventArgsMineLevelChanged> MineLevelChanged = delegate { };

        internal static void InvokeMineLevelChanged(int previousMinelevel, int currentMineLevel)
        {
            MineLevelChanged.Invoke(null, new EventArgsMineLevelChanged(previousMinelevel, currentMineLevel));
        }
    }
}