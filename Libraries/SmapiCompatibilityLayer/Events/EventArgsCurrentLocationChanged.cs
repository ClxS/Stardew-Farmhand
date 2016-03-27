using System;
using StardewValley;

namespace StardewModdingAPI.Events
{
    public class EventArgsCurrentLocationChanged : EventArgs
    {
        public EventArgsCurrentLocationChanged(GameLocation priorLocation, GameLocation newLocation)
        {
            NewLocation = newLocation;
            PriorLocation = priorLocation;
        }
        public GameLocation NewLocation { get; private set; }
        public GameLocation PriorLocation { get; private set; }
    }
}