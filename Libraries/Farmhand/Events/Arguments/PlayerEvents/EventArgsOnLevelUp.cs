using System;

namespace Farmhand.Events.Arguments.PlayerEvents
{
    public class EventArgsOnLevelUp : EventArgs
    {
        public enum LevelType
        {
            Farming = 0,
            Fishing = 1,
            Foraging = 2,
            Mining = 3,
            Combat = 4,
            Luck = 5
        }

        public EventArgsOnLevelUp(int which, int newLevel, int oldLevel)
        {
            Which = (LevelType)which;
            NewLevel = newLevel;
            OldLevel = oldLevel;
        }

        public LevelType Which { get; set; }
        public int NewLevel { get; set; }
        public int OldLevel { get; set; }
    }
}
