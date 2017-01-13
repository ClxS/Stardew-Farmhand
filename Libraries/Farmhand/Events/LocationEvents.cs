namespace Farmhand.Events
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;

    using Farmhand.Attributes;
    using Farmhand.Events.Arguments.LocationEvents;

    using StardewValley;

    using xTile.Dimensions;

    /// <summary>
    ///     Contains events relating to locations
    /// </summary>
    public static class LocationEvents
    {
        /// <summary>
        ///     Fires just after the game's location list changes.
        /// </summary>
        /// <remarks>
        ///     This fires just after the game's loadForNewGame method.
        ///     TODO: API functionality should trigger this
        /// </remarks>
        public static event EventHandler<EventArgsLocationsChanged> LocationsChanged = delegate { };

        /// <summary>
        ///     Fires just after a location's object list changes.
        /// </summary>
        /// <remarks>
        ///     This fires just after the game's GameLocation::objectCollectionChanged method.
        ///     TODO: API functionality should trigger this
        /// </remarks>
        public static event EventHandler<NotifyCollectionChangedEventArgs> LocationObjectsChanged = delegate { };

        /// <summary>
        ///     Fires just after a location's terrain features change.
        /// </summary>
        /// <remarks>
        ///     TODO: API functionality should trigger this
        /// </remarks>
        public static event EventHandler<NotifyCollectionChangedEventArgs> LocationTerrainFeaturesChanged = delegate { };

        /// <summary>
        ///     Fires just before checking for action at a position.
        /// </summary>
        /// <remarks>
        ///     This event is returnable, allowing you to handle actions on your own and notify the game that you have done so.
        /// </remarks>
        public static event EventHandler<EventArgsOnBeforeCheckAction> BeforeCheckAction = delegate { };

        /// <summary>
        ///     Fires just before loading the objects in a location.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to prevent the default loading of objects in a location.
        /// </remarks>
        public static event EventHandler<CancelEventArgs> BeforeLocationLoadObjects = delegate { };

        /// <summary>
        ///     Fires just after loading the objects in a location.
        /// </summary>
        public static event EventHandler AfterLocationLoadObjects = delegate { };

        /// <summary>
        ///     Fires just before warping the player.
        /// </summary>
        /// <remarks>
        ///     This event is cancellable, allowing you to prevent the warping of a player.
        /// </remarks>
        public static event EventHandler<EventArgsOnBeforeWarp> BeforeWarp = delegate { };

        /// <summary>
        ///     Fired just before changing the current location.
        /// </summary>
        /// <remarks>
        ///     TODO: Not yet implemented
        /// </remarks>
        public static event EventHandler<EventArgsOnCurrentLocationChanged> CurrentLocationChanged = delegate { };

        [Hook(HookType.Exit, "StardewValley.Game1", "loadForNewGame")]
        internal static void OnLocationsChanged()
        {
            EventCommon.SafeInvoke(LocationsChanged, null, new EventArgsLocationsChanged(Game1.locations));
        }

        [Hook(HookType.Entry, "StardewValley.GameLocation", "loadObjects")]
        internal static bool OnBeforeLocationLoadObjects([ThisBind] object @this)
        {
            return EventCommon.SafeCancellableInvoke(BeforeLocationLoadObjects, @this, new CancelEventArgs());
        }

        [HookReturnable(HookType.Entry, "StardewValley.GameLocation", "checkAction")]
        internal static bool OnBeforeCheckAction(
            [UseOutputBind] ref bool useOutput,
            [ThisBind] object @this,
            [InputBind(typeof(Location), "tileLocation")] Location location,
            [InputBind(typeof(Rectangle), "viewport")] Rectangle viewport,
            [InputBind(typeof(Farmer), "who")] Farmer who)
        {
            var eventArgs = new EventArgsOnBeforeCheckAction((GameLocation)@this, location, viewport, who);
            EventCommon.SafeInvoke(BeforeCheckAction, @this, eventArgs);
            return eventArgs.Handled;
        }

        [Hook(HookType.Exit, "StardewValley.GameLocation", "loadObjects")]
        internal static void OnAfterLocationLoadObjects([ThisBind] object @this)
        {
            EventCommon.SafeInvoke(AfterLocationLoadObjects, @this);
        }

        [Hook(HookType.Entry, "StardewValley.Game1",
            "System.Void StardewValley.Game1::warpFarmer(StardewValley.GameLocation,System.Int32,System.Int32,System.Int32,System.Boolean)")]
        internal static bool OnBeforeWarp(
            [InputBind(typeof(GameLocation), "locationAfterWarp")] GameLocation locationAfterWarp,
            [InputBind(typeof(int), "tileX")] int tileX,
            [InputBind(typeof(int), "tileY")] int tileY,
            [InputBind(typeof(int), "facingDirectionAfterWarp")] int facingDirectionAfterWarp)
        {
            return EventCommon.SafeCancellableInvoke(
                BeforeWarp,
                null,
                new EventArgsOnBeforeWarp(locationAfterWarp, tileX, tileY, facingDirectionAfterWarp));
        }

        [PendingHook]
        internal static void OnCurrentLocationChanged(GameLocation priorLocation, GameLocation newLocation)
        {
            EventCommon.SafeInvoke(
                CurrentLocationChanged,
                null,
                new EventArgsOnCurrentLocationChanged(priorLocation, newLocation));
        }

        [Hook(HookType.Entry, "StardewValley.GameLocation", "objectCollectionChanged")]
        internal static void OnNewLocationObject(
            [ThisBind] object @this,
            [InputBind(typeof(NotifyCollectionChangedEventArgs), "e")] NotifyCollectionChangedEventArgs e)
        {
            EventCommon.SafeInvoke(LocationObjectsChanged, @this, e);
        }

        [Hook(HookType.Entry, "StardewValley.GameLocation", "terrainFeaturesCollectionChanged")]
        internal static void OnLocationTerrainFeaturesChanged(
            [ThisBind] object @this,
            [InputBind(typeof(NotifyCollectionChangedEventArgs), "e")] NotifyCollectionChangedEventArgs e)
        {
            EventCommon.SafeInvoke(LocationTerrainFeaturesChanged, @this, e);
        }
    }
}