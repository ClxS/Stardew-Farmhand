using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    class LocationEvents
    {
        public static event EventHandler LocationsChanged = delegate { };
        public static event EventHandler LocationObjectsChanged = delegate { };
        public static event EventHandler CurrentLocationChanged = delegate { };

        public static void InvokeLocationsChanged(List<GameLocation> newLocations)
        {
            LocationsChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeCurrentLocationChanged(GameLocation priorLocation, GameLocation newLocation)
        {
            CurrentLocationChanged.Invoke(null, EventArgs.Empty);
        }

        internal static void InvokeOnNewLocationObject(SerializableDictionary<Vector2, StardewValley.Object> newObjects)
        {
            LocationObjectsChanged.Invoke(null, EventArgs.Empty);
        }
    }
}
