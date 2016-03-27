using Microsoft.Xna.Framework;
using Revolution.Attributes;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Revolution.Events
{
    public class LocationEvents
    {
        public static event EventHandler OnLocationsChanged = delegate { };
        public static event EventHandler<NotifyCollectionChangedEventArgs> OnLocationObjectsChanged = delegate { };
        public static event EventHandler<NotifyCollectionChangedEventArgs> OnLocationTerrainFeaturesChanged = delegate { };
        public static event EventHandler OnCurrentLocationChanged = delegate { };
        
        [Hook(HookType.Exit, "StardewValley.Game1", "loadForNewGame")]
        internal static void InvokeLocationsChanged([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(OnLocationsChanged, @this);
        }

        [PendingHook]
        internal static void InvokeCurrentLocationChanged([ThisBind] object @this)
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
