using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    public class LocationEvents
    {
        public static event EventHandler OnLocationsChanged = delegate { };
        public static event EventHandler OnLocationObjectsChanged = delegate { };
        public static event EventHandler OnCurrentLocationChanged = delegate { };

        public static void InvokeLocationsChanged(List<GameLocation> newLocations)
        {
            OnLocationsChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeCurrentLocationChanged(GameLocation priorLocation, GameLocation newLocation)
        {
            OnCurrentLocationChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeOnNewLocationObject(SerializableDictionary<Vector2, StardewValley.Object> newObjects)
        {
            OnLocationObjectsChanged.Invoke(null, EventArgs.Empty);
        }
    }
}
