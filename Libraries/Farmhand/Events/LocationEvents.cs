using Farmhand.Attributes;
using Farmhand.Events.Arguments.LocationEvents;
using StardewValley;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events relating to locations
    /// </summary>
    public static class LocationEvents
    {
        public static event EventHandler<EventArgsLocationsChanged> OnLocationsChanged = delegate { };
        public static event EventHandler<NotifyCollectionChangedEventArgs> OnLocationObjectsChanged = delegate { };
        public static event EventHandler<NotifyCollectionChangedEventArgs> OnLocationTerrainFeaturesChanged = delegate { };
        public static event EventHandler<CancelEventArgs> OnBeforeLocationLoadObjects = delegate { };
        public static event EventHandler OnAfterLocationLoadObjects = delegate { };
        public static event EventHandler<EventArgsOnBeforeWarp> OnBeforeWarp = delegate { };
        public static event EventHandler<EventArgsOnCurrentLocationChanged> OnCurrentLocationChanged = delegate { };
        
        [Hook(HookType.Exit, "StardewValley.Game1", "loadForNewGame")]
        internal static void InvokeLocationsChanged()
        {
            EventCommon.SafeInvoke(OnLocationsChanged, null, new EventArgsLocationsChanged(Game1.locations));
        }

        [Hook(HookType.Entry, "StardewValley.GameLocation", "loadObjects")]
        internal static bool InvokeOnBeforeLocationLoadObjects([ThisBind] object @this)
        {
            return EventCommon.SafeCancellableInvoke(OnBeforeLocationLoadObjects, @this, new CancelEventArgs());
        }

        [Hook(HookType.Exit, "StardewValley.GameLocation", "loadObjects")]
        internal static void InvokeOnAfterLocationLoadObjects([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnAfterLocationLoadObjects, @this);
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "System.Void StardewValley.Game1::warpFarmer(StardewValley.GameLocation,System.Int32,System.Int32,System.Int32,System.Boolean)")]
        internal static bool InvokeOnBeforeWarp([InputBind(typeof(GameLocation), "locationAfterWarp")] GameLocation locationAfterWarp,
            [InputBind(typeof(int), "tileX")] int tileX,
            [InputBind(typeof(int), "tileY")] int tileY,
            [InputBind(typeof(int), "facingDirectionAfterWarp")] int facingDirectionAfterWarp)
        {
            return EventCommon.SafeCancellableInvoke(OnBeforeWarp, null, new EventArgsOnBeforeWarp(locationAfterWarp, tileX, tileY, facingDirectionAfterWarp));
        }

        [PendingHook]
        internal static void InvokeCurrentLocationChanged(GameLocation priorLocation, GameLocation newLocation)
        {
            EventCommon.SafeInvoke(OnCurrentLocationChanged, null, new EventArgsOnCurrentLocationChanged(priorLocation, newLocation));
        }
        
        [Hook(HookType.Entry, "StardewValley.GameLocation", "objectCollectionChanged")]
        internal static void InvokeOnNewLocationObject(
            [ThisBind] object @this,
            [InputBind(typeof(NotifyCollectionChangedEventArgs), "e")] NotifyCollectionChangedEventArgs e
            )
        {
            EventCommon.SafeInvoke(OnLocationObjectsChanged, @this, e);
        }

        [Hook(HookType.Entry, "StardewValley.GameLocation", "terrainFeaturesCollectionChanged")]
        internal static void InvokeOnLocationTerrainFeaturesChanged(
            [ThisBind] object @this,
            [InputBind(typeof(NotifyCollectionChangedEventArgs), "e")] NotifyCollectionChangedEventArgs e
            )
        {
            EventCommon.SafeInvoke(OnLocationTerrainFeaturesChanged, @this, e);
        }
    }
}
