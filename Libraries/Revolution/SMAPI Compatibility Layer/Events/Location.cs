using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Events
{
    public static class LocationEvents
    {
        public static event EventHandler<EventArgsGameLocationsChanged> LocationsChanged = delegate { };
        public static event EventHandler<EventArgsLocationObjectsChanged> LocationObjectsChanged = delegate { };
        public static event EventHandler<EventArgsCurrentLocationChanged> CurrentLocationChanged = delegate { };

        public static void InvokeLocationsChanged(List<GameLocation> newLocations)
        {
            LocationsChanged.Invoke(null, new EventArgsGameLocationsChanged(newLocations));
        }

        public static void InvokeCurrentLocationChanged(GameLocation priorLocation, GameLocation newLocation)
        {
            CurrentLocationChanged.Invoke(null, new EventArgsCurrentLocationChanged(priorLocation, newLocation));
        }

        internal static void InvokeOnNewLocationObject(SerializableDictionary<Vector2, StardewValley.Object> newObjects)
        {
            LocationObjectsChanged.Invoke(null, new EventArgsLocationObjectsChanged(newObjects));
        }
    }
}
