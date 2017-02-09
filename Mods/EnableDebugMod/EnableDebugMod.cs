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
                    Farmhand.API.Game.Player.money += 1500000;
                    Game1.showGlobalMessage("TestCommand has been executed!");
                    return true;
                case "uh":
                    Farmhand.API.Game.Player.HouseUpgradeLevel = Math.Min(3, Farmhand.API.Game.Player.HouseUpgradeLevel + 1);
                    Game1.removeFrontLayerForFarmBuildings();
                    Game1.addNewFarmBuildingMaps();
                    return true;
            }
            return false;
        }

        private void ControlEvents_OnKeyPressed(object sender, KeyPressedEventArgs e)
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
                Farmhand.API.Game.Player.eventsSeen.Add(60367);
                Farmhand.API.Game.Player.currentLocation = Utility.getHomeOfFarmer(Farmhand.API.Game.Player);
                Farmhand.API.Game.Player.position = new Vector2(7f, 9f) * (float)Game1.tileSize;
                Game1.NewDay(0.0f);
                Game1.exitActiveMenu();
                Game1.setGameMode(3);
            }
        }
    }
}
