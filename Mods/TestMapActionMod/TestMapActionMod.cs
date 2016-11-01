using System;
using System.Linq;
using Farmhand;
using Farmhand.API.Locations;
using Farmhand.Events;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using xTile.Dimensions;
using xTile.ObjectModel;
using Location = Farmhand.API.Locations.Location;

namespace TestMapActionMod
{
    public class TestMapActionMod : Mod
    {
        public static TestMapActionMod Instance;

        public override void Entry()
        {
            Instance = this;
            Location.RegisterAction("Farm", new MapActionInformation(Instance, "WarpGreenhouse", ActionHandler));

            Location.RegisterAction(new MapActionInformation(Instance, "FarmCaveVines", ActionHandler));
            Location.RegisterAction(new MapActionInformation(Instance, "FarmCaveWall", ActionHandler));
            Location.RegisterAction("BusStop", new MapActionInformation(Instance, "BusStopNewObject", ActionHandler));

            GameEvents.OnAfterLoadedContent += GameEvents_AfterContentLoaded;
        }

        private void GameEvents_AfterContentLoaded(object sender, EventArgs e)
        {
            var caveEdit = new MapInformation(Instance, Instance.ModSettings.GetMap("farmCave_Edit"));
            
            LocationUtilities.RegisterMap(Instance, "Maps\\FarmCave", caveEdit);
        }

        private bool ActionHandler(string action)
        {
            switch (action)
            {
                case "WarpGreenhouse":
                    if (!Game1.player.hasGreenhouse)
                    {
                        Game1.drawObjectDialogue("YOU SHALL NOT PASS!");
                        return true;
                    }

                    Game1.warpFarmer("Greenhouse", 10, 11, false);
                    return true;
                case "FarmCaveVines":
                    Game1.drawObjectDialogue("Just some nasty old vines.");
                    return true;
                case "FarmCaveWall":
                    Game1.drawObjectDialogue("What are you doing staring at a wall for? Completely pointless...");
                    return true;
                case "BusStopNewObject":
                    Game1.drawObjectDialogue("WHY DOES THIS STAND HERE!?");
                    return true;
                default:
                    return false;
            }
        }
    }
}
