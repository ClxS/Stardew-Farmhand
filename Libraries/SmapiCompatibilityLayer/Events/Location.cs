using System;
using System.Collections.Specialized;
using StardewValley;
using Farmhand.Events;
using Farmhand.Events.Arguments.LocationEvents;

namespace StardewModdingAPI.Events
{
    public static class LocationEvents
    {
        public static event EventHandler<EventArgsGameLocationsChanged> LocationsChanged = delegate { };
        public static event EventHandler<EventArgsLocationObjectsChanged> LocationObjectsChanged = delegate { };
        public static event EventHandler<EventArgsCurrentLocationChanged> CurrentLocationChanged = delegate { };

        internal static void InvokeLocationsChanged(object sender, EventArgs eventArgs)
        {
            EventCommon.SafeInvoke(LocationsChanged, sender, new EventArgsGameLocationsChanged(Game1.locations));
        }

        internal static void InvokeCurrentLocationChanged(object sender, EventArgsOnCurrentLocationChanged eventArgs)
        {
            EventCommon.SafeInvoke(CurrentLocationChanged, sender, new EventArgsCurrentLocationChanged(eventArgs.PriorLocation, eventArgs.NewLocation));
        }

        internal static void InvokeOnNewLocationObject(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            var location = sender as GameLocation;
            EventCommon.SafeInvoke(LocationObjectsChanged, sender, new EventArgsLocationObjectsChanged(location?.objects));
        }
    }
}