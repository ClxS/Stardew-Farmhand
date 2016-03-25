using System;
using System.Collections.Generic;
using StardewValley;

namespace StardewModdingAPI.Events
{
    public class EventArgsGameLocationsChanged : EventArgs
    {
        public EventArgsGameLocationsChanged(List<GameLocation> newLocations)
        {
            NewLocations = newLocations;
        }
        public List<GameLocation> NewLocations { get; private set; }
    }
}