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
            Location.RegisterAction("Farm", new MapActionInformation(Instance, "WarpGreenhouse", OverrideGreenhouseWarp));

            Location.RegisterAction(new MapActionInformation(Instance, "FarmCaveVines", ActionFarmCaveVines));
            Location.RegisterAction(new MapActionInformation(Instance, "FarmCaveWall", ActionFarmCaveWall));
            Location.RegisterAction("BusStop", new MapActionInformation(Instance, "BusStopNewObject", ActionFarmCaveLantern));

            Location.RegisterAction("FarmHouse", new MapActionInformation(Instance, "asdf", ActionASDF));

            GameEvents.OnAfterLoadedContent += GameEvents_AfterContentLoaded;
        }

        private void GameEvents_AfterContentLoaded(object sender, EventArgs e)
        {
            var caveEdit = new MapInformation(Instance, Instance.ModSettings.GetMap("farmCave_Edit"));
            
            LocationUtilities.RegisterMap(Instance, "Maps\\FarmCave", caveEdit);
        }

        private bool OverrideGreenhouseWarp()
        {
            Console.WriteLine("Override has been called!");
            if (!Game1.player.hasGreenhouse)
            {
                Game1.drawObjectDialogue("YOU SHALL NOT PASS!");
                return true;
            }

            Game1.warpFarmer("Greenhouse", 10, 11, false);
            return true;
        }

        private bool ActionFarmCaveVines()
        {
            Game1.drawObjectDialogue("Just some nasty old vines.");
            return true;
        }

        private bool ActionFarmCaveWall()
        {
            Game1.drawObjectDialogue("What are you doing staring at a wall for? Completely pointless...");
            return true;
        }

        private bool ActionFarmCaveLantern()
        {
            Game1.drawObjectDialogue("WHY DOES THIS STAND HERE!?");
            return true;
        }

        private bool ActionASDF()
        {
            Game1.drawObjectDialogue("IT'S MY FUCKING FIREPLACE! HOLY BALLS!");
            return true;
        }
    }
}
