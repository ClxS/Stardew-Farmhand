using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand.Attributes;
using Farmhand.Registries;
using Microsoft.Xna.Framework;
using StardewValley;
using xTile.Dimensions;

namespace Farmhand.API.Locations
{
	/// <summary>
    /// Provides functions relating to all GameLocations
    /// </summary>
    public class Location
    {
		private static readonly List<MapActionInformation> GlobalActions = new List<MapActionInformation>();
		private static readonly Dictionary<string, List<MapActionInformation>> MapActions = new Dictionary<string, List<MapActionInformation>>();

        private static readonly List<MapTouchActionInformation> GlobalTouchActions = new List<MapTouchActionInformation>();
        private static readonly Dictionary<string, List<MapTouchActionInformation>> MapTouchActions = new Dictionary<string, List<MapTouchActionInformation>>();

        public static void RegisterTouchAction(string MapName, MapTouchActionInformation mapActionInformation)
        {
            if (!MapTouchActions.ContainsKey(MapName)) {
                MapTouchActions[MapName] = new List<MapTouchActionInformation>();
            }
            MapTouchActions[MapName].Add(mapActionInformation);
        }

        public static void RegisterTouchAction(MapTouchActionInformation mapActionInformation)
        {
            if (GlobalTouchActions.Contains(mapActionInformation)) {
                Logging.Log.Warning($"Potential conflict registering new action. Action {mapActionInformation.Action} has been registered by two separate mods. Only the last registered one will be used.");
            }
            GlobalTouchActions.Add(mapActionInformation);
        }

        public static void RegisterAction(string MapName, MapActionInformation mapActionInformation)
        {
            if (!MapActions.ContainsKey(MapName))
            {
                MapActions[MapName] = new List<MapActionInformation>();
            }
			MapActions[MapName].Add(mapActionInformation);
        }

        public static void RegisterAction(MapActionInformation mapActionInformation)
        {
            if (GlobalActions.Contains(mapActionInformation))
            {
                Logging.Log.Warning($"Potential conflict registering new action. Action {mapActionInformation.Action} has been registered by two separate mods. Only the last registered one will be used.");
            }
            GlobalActions.Add(mapActionInformation);
        }

        [HookReturnable(HookType.Entry, "StardewValley.GameLocation", "performAction")]
        internal static bool performAction([UseOutputBind] ref bool useOutput,
										   [ThisBind] object @this,
                                           [InputBind(typeof(string), "action")] string action,
                                           [InputBind(typeof(Farmer), "who")] Farmer who,
                                           [InputBind(typeof(xTile.Dimensions.Location), "tileLocation")] xTile.Dimensions.Location tileLocation)
        {
            useOutput = false;

            if (action == null || !who.IsMainPlayer) return useOutput;
            
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
                useOutput = mapActionInformation.Callback.Invoke(action);

            // TODO: Disabled Mod Handling 
            //if (ModRegistry.GetRegisteredItems().FirstOrDefault(_ => Equals(_.UniqueId, mapActionInformation?.Owner.ModSettings.UniqueId)) == null) 
            //    useOutput = false; 

            return useOutput;
        }

        [Hook(HookType.Entry, "StardewValley.GameLocation", "performTouchAction")]
        internal static bool performTouchAction([ThisBind] object @this,
                                                [InputBind(typeof(string), "fullActionString")] string fullActionString,
                                                [InputBind(typeof(Vector2), "playerStandingPosition")] Vector2 playerStandingPosition)
        {
            var useOutput = false;
            var action = fullActionString?.Split(' ')[0];
            var parameters = fullActionString?.Split(' ').Skip(1).ToArray();

            if (action == null)
                return useOutput;

            MapTouchActionInformation mapTouchActionInformation;
            if (MapTouchActions.ContainsKey(Game1.currentLocation.Name)) {
                var list = MapTouchActions[Game1.currentLocation.Name];
                mapTouchActionInformation = list?.FirstOrDefault(item => item.Action == action);
                if (mapTouchActionInformation != null) {
                    useOutput = mapTouchActionInformation.Callback.Invoke(action, parameters);
                }
            } else if ((mapTouchActionInformation = GlobalTouchActions.FirstOrDefault(item => item.Action == action)) != null)
                useOutput = mapTouchActionInformation.Callback.Invoke(action, parameters);

            // TODO: Disabled Mod Handling 
            //if (ModRegistry.GetRegisteredItems().FirstOrDefault(_ => Equals(_.UniqueId, mapTouchActionInformation?.Owner.ModSettings.UniqueId)) == null) 
            //    useOutput = false; 

            return useOutput;
        }
    }
}
