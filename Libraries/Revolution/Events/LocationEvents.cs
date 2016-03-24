using Microsoft.Xna.Framework;
using Revolution.Attributes;
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
        
        [PendingHook]
        internal static void InvokeLocationsChanged(List<GameLocation> newLocations)
        {
            EventCommon.SafeInvoke(OnLocationsChanged, null);
        }

        [PendingHook]
        internal static void InvokeCurrentLocationChanged(GameLocation priorLocation, GameLocation newLocation)
        {
            EventCommon.SafeInvoke(OnCurrentLocationChanged, null);
        }

        [PendingHook]
        internal static void InvokeOnNewLocationObject(SerializableDictionary<Vector2, StardewValley.Object> newObjects)
        {
            EventCommon.SafeInvoke(OnLocationObjectsChanged, null);
        }
    }
}
