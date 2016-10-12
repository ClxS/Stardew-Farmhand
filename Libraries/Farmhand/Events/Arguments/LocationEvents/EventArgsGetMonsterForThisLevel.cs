using System;

namespace Farmhand.Events.Arguments.LocationEvents
{
    public class EventArgsGetMonsterForThisLevel : EventArgs
    {
        public EventArgsGetMonsterForThisLevel(int level, int xTile, int yTile)
        {
            Level = level;
            XTile = xTile;
            YTile = yTile;
        }

        public int Level { get; set; }
        public int XTile { get; set; }
        public int YTile { get; set; }
    }
}
