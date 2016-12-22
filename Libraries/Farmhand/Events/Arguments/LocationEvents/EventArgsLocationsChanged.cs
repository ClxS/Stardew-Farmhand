using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StardewValley;

namespace Farmhand.Events.Arguments.LocationEvents
{
    public class EventArgsLocationsChanged : EventArgs
    {
        public EventArgsLocationsChanged(List<GameLocation> locations)
        {
            NewLocations = locations;
        }

        public List<GameLocation> NewLocations { get; private set; }
    }
}
