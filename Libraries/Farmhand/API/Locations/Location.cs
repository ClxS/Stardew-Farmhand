using System;
using System.Collections.Generic;
using System.Linq;
using Farmhand.Attributes;
using Farmhand.Registries;
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
                    useOutput = mapActionInformation.Callback.Invoke();
                }
            }
            else if ((mapActionInformation = GlobalActions.FirstOrDefault(item => item.Action == action)) != null)
                useOutput = mapActionInformation.Callback.Invoke();

            if (ModRegistry.GetRegisteredItems().FirstOrDefault(_ => Equals(_.UniqueId, mapActionInformation?.Owner.ModSettings.UniqueId)) == null)
                useOutput = false;

            return useOutput;
        }
    }
}
