using System;
using Farmhand;
using Farmhand.API.Debug;
using Farmhand.Events;
using Farmhand.Events.Arguments.ControlEvents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley;

namespace EnableDebugMod
{
    public class EnableDebugMod : Mod
    {
        public static EnableDebugMod Instance;

        public override void Entry()
        {
            Instance = this;
            ControlEvents.KeyPressed += ControlEvents_OnKeyPressed;
            Farmhand.API.Debug.Debug.RegisterDebugCommand("testcommand", new DebugInformation(Instance, HandleCommands));
            Farmhand.API.Debug.Debug.RegisterDebugCommand("uh", new DebugInformation(Instance, HandleCommands));
        }

        private bool HandleCommands(string command, params string[] parameters)
        {
            switch (command) {
                case "testcommand":
                    Game1.player.money += 1500000;
                    Game1.showGlobalMessage("TestCommand has been executed!");
                    return true;
                case "uh":
                    Game1.player.HouseUpgradeLevel = Math.Min(3, Game1.player.HouseUpgradeLevel + 1);
                    Game1.removeFrontLayerForFarmBuildings();
                    Game1.addNewFarmBuildingMaps();
                    return true;
            }
            return false;
        }

        private void ControlEvents_OnKeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (Game1.paused)
                return;

            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.F8) && !Game1.oldKBState.IsKeyDown(Keys.F8))
                Game1.game1.requestDebugInput();

            if (e.KeyPressed == Keys.N && (Game1.oldKBState.IsKeyDown(Keys.RightShift) && Game1.oldKBState.IsKeyDown(Keys.LeftControl)))
            {
                Game1.loadForNewGame();
                Game1.saveOnNewDay = false;
                Game1.player.eventsSeen.Add(60367);
                Game1.player.currentLocation = Utility.getHomeOfFarmer(Game1.player);
                Game1.player.position = new Vector2(7f, 9f) * (float)Game1.tileSize;
                Game1.NewDay(0.0f);
                Game1.exitActiveMenu();
                Game1.setGameMode(3);
            }
        }
    }
}
