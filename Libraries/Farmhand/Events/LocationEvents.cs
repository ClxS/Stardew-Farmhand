using Microsoft.Xna.Framework;
using Farmhand.Attributes;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events relating to locations
    /// </summary>
    public static class LocationEvents
    {
        public static event EventHandler OnLocationsChanged = delegate { };
        public static event EventHandler<NotifyCollectionChangedEventArgs> OnLocationObjectsChanged = delegate { };
        public static event EventHandler<NotifyCollectionChangedEventArgs> OnLocationTerrainFeaturesChanged = delegate { };
        public static event EventHandler<CancelEventArgs> OnBeforeLocationLoadObjects = delegate { };
        public static event EventHandler OnAfterLocationLoadObjects = delegate { };
        public static event EventHandler OnCurrentLocationChanged = delegate { };
        
        [Hook(HookType.Exit, "StardewValley.Game1", "loadForNewGame")]
        internal static void InvokeLocationsChanged()
        {
            EventCommon.SafeInvoke(OnLocationsChanged, null);
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

        [PendingHook]
        internal static void InvokeCurrentLocationChanged(GameLocation priorLocation, GameLocation newLocation)
        {
            EventCommon.SafeInvoke(OnCurrentLocationChanged, null);
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
