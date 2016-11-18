using System;

namespace Farmhand.Events.Arguments.TimeEvents
{
    public class EventArgsShouldTimePassCheck : EventArgs
    {
        public EventArgsShouldTimePassCheck( bool timeShouldPass )
        {
            TimeShouldPass = timeShouldPass;
        }

        public bool TimeShouldPass { get; set; }
    }
}
