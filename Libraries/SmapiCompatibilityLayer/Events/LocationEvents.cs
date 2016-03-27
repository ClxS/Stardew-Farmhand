using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

// ReSharper disable CheckNamespace
namespace StardewModdingAPI.Events
{
    public static class LocationEvents
    {
        public static event EventHandler<EventArgsGameLocationsChanged> LocationsChanged = delegate { };
        public static event EventHandler<EventArgsLocationObjectsChanged> LocationObjectsChanged = delegate { };
        public static event EventHandler<EventArgsCurrentLocationChanged> CurrentLocationChanged = delegate { };

        public static void InvokeLocationsChanged(object sender, EventArgs eventArgs)
        {
            LocationsChanged.Invoke(null, new EventArgsGameLocationsChanged(Game1.locations));
        }

        public static void InvokeCurrentLocationChanged(object sender, EventArgs eventArgs)
        {
            //CurrentLocationChanged.Invoke(null, new EventArgsCurrentLocationChanged(priorLocation, newLocation));
        }

        internal static void InvokeOnNewLocationObject(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            LocationObjectsChanged.Invoke(null, new EventArgsLocationObjectsChanged(Game1.currentLocation.objects));
        }
    }
}
