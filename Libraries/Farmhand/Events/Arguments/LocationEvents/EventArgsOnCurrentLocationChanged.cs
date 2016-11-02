using System;
using StardewValley;

namespace Farmhand.Events.Arguments.LocationEvents
{
    public class EventArgsOnCurrentLocationChanged : EventArgs
    {
        public EventArgsOnCurrentLocationChanged(GameLocation priorLocation, GameLocation newLocation)
        {
            PriorLocation = priorLocation;
            NewLocation = newLocation;
        }

        public GameLocation PriorLocation { get; set; }
        public GameLocation NewLocation { get; set; }
    }
}
