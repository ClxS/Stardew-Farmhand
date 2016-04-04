using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Events.Arguments.PlayerEvents
{
    public class EventArgsOnLevelUp : EventArgs
    {
        public EventArgsOnLevelUp(int which, int newLevel, int oldLevel)
        {
            Which = which;
            NewLevel = newLevel;
            OldLevel = oldLevel;
        }

        public int Which { get; set; }
        public int NewLevel { get; set; }
        public int OldLevel { get; set; }
    }
}
