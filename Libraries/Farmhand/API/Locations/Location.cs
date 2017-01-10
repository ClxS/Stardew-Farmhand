namespace Farmhand.API.Locations
{
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.Attributes;
    using Farmhand.Logging;

    using Microsoft.Xna.Framework;

    using StardewValley;

    /// <summary>
    ///     Provides functions relating to all GameLocations
    /// </summary>
    public static class Location
    {
        private static readonly List<MapActionInformation> GlobalActions = new List<MapActionInformation>();

        private static readonly List<MapTouchActionInformation> GlobalTouchActions =
            new List<MapTouchActionInformation>();

        private static readonly Dictionary<string, List<MapActionInformation>> MapActions =
            new Dictionary<string, List<MapActionInformation>>();

        private static readonly Dictionary<string, List<MapTouchActionInformation>> MapTouchActions =
            new Dictionary<string, List<MapTouchActionInformation>>();

        /// <summary>
        ///     Gets a list of all locations in the game.
        /// </summary>
        public static List<GameLocation> AllLocations => Game1.locations;

        /// <summary>
        ///     Registers an action to the API.
        /// </summary>
        /// <param name="mapName">
        ///     The map name.
        /// </param>
        /// <param name="mapActionInformation">
        ///     The map action information.
        /// </param>
        public static void RegisterAction(string mapName, MapActionInformation mapActionInformation)
        {
            if (!MapActions.ContainsKey(mapName))
            {
                MapActions[mapName] = new List<MapActionInformation>();
            }

            MapActions[mapName].Add(mapActionInformation);
        }

        /// <summary>
        ///     Registers an action to the API.
        /// </summary>
        /// <param name="mapActionInformation">
        ///     The map action information.
        /// </param>
        public static void RegisterAction(MapActionInformation mapActionInformation)
        {
            if (GlobalActions.Contains(mapActionInformation))
            {
                Log.Warning(
                    $"Potential conflict registering new action. Action {mapActionInformation.Action} has been registered by two separate mods. Only the last registered one will be used.");
            }

            GlobalActions.Add(mapActionInformation);
        }

        /// <summary>
        ///     Registers a touch action to the API.
        /// </summary>
        /// <param name="mapName">
        ///     The map name.
        /// </param>
        /// <param name="mapActionInformation">
        ///     The map Action Information.
        /// </param>
        public static void RegisterTouchAction(string mapName, MapTouchActionInformation mapActionInformation)
        {
            if (!MapTouchActions.ContainsKey(mapName))
            {
                MapTouchActions[mapName] = new List<MapTouchActionInformation>();
            }

            MapTouchActions[mapName].Add(mapActionInformation);
        }

        /// <summary>
        ///     Registers a touch action to the API.
        /// </summary>
        /// <param name="mapActionInformation">
        ///     The map action information.
        /// </param>
        public static void RegisterTouchAction(MapTouchActionInformation mapActionInformation)
        {
            if (GlobalTouchActions.Contains(mapActionInformation))
            {
                Log.Warning(
                    $"Potential conflict registering new action. Action {mapActionInformation.Action} has been registered by two separate mods. Only the last registered one will be used.");
            }

            GlobalTouchActions.Add(mapActionInformation);
        }

        [HookReturnable(HookType.Entry, "StardewValley.GameLocation", "performAction")]
        internal static bool PerformAction(
            [UseOutputBind] out bool useOutput,
            [ThisBind] object @this,
            [InputBind(typeof(string), "action")] string action,
            [InputBind(typeof(Farmer), "who")] Farmer who,
            [InputBind(typeof(xTile.Dimensions.Location), "tileLocation")] xTile.Dimensions.Location tileLocation)
        {
            useOutput = false;

            if (action == null || !who.IsMainPlayer)
            {
                return false;
            }

            MapActionInformation mapActionInformation;
            if (MapActions.ContainsKey(Game1.currentLocation.Name))
            {
                var list = MapActions[Game1.currentLocation.Name];
                mapActionInformation = list?.FirstOrDefault(item => item.Action == action);
                if (mapActionInformation != null)
                {
                    useOutput = mapActionInformation.Callback.Invoke(action);
                }
            }
            else if ((mapActionInformation = GlobalActions.FirstOrDefault(item => item.Action == action)) != null)
            {
                useOutput = mapActionInformation.Callback.Invoke(action);
            }

            // TODO: Disabled Mod Handling 
            // if (ModRegistry.GetRegisteredItems().FirstOrDefault(_ => Equals(_.UniqueId, mapActionInformation?.Owner.ModSettings.UniqueId)) == null) 
            // useOutput = false; 
            return useOutput;
        }

        [Hook(HookType.Entry, "StardewValley.GameLocation", "performTouchAction")]
        internal static bool PerformTouchAction(
            [ThisBind] object @this,
            [InputBind(typeof(string), "fullActionString")] string fullActionString,
            [InputBind(typeof(Vector2), "playerStandingPosition")] Vector2 playerStandingPosition)
        {
            var useOutput = false;
            var action = fullActionString?.Split(' ')[0];
            var parameters = fullActionString?.Split(' ').Skip(1).ToArray();

            if (action == null)
            {
                return false;
            }

            MapTouchActionInformation mapTouchActionInformation;
            if (MapTouchActions.ContainsKey(Game1.currentLocation.Name))
            {
                var list = MapTouchActions[Game1.currentLocation.Name];
                mapTouchActionInformation = list?.FirstOrDefault(item => item.Action == action);
                if (mapTouchActionInformation != null)
                {
                    useOutput = mapTouchActionInformation.Callback.Invoke(action, parameters);
                }
            }
            else if ((mapTouchActionInformation = GlobalTouchActions.FirstOrDefault(item => item.Action == action)) != null)
            {
                useOutput = mapTouchActionInformation.Callback.Invoke(action, parameters);
            }

            // TODO: Disabled Mod Handling 
            // if (ModRegistry.GetRegisteredItems().FirstOrDefault(_ => Equals(_.UniqueId, mapTouchActionInformation?.Owner.ModSettings.UniqueId)) == null) 
            // useOutput = false; 
            return useOutput;
        }
    }
}