using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Farmhand.Events.Arguments.LocationEvents;
using Farmhand.Events.Arguments.PlayerEvents;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using StardewValley;

namespace SmapiCompatibilityLayer
{
    internal static class EventForwarder
    {
        

        public static void ForwardEvents()
        {
            // TODO:
            // MineEvents
            // TimeEvents
            // GameEvents
            //Farmhand.Events.ControlEvents.OnKeyboardChanged += ControlEvents_OnKeyboardChanged;
            //Farmhand.Events.ControlEvents.OnKeyPressed += ControlEvents_OnKeyPressed;
            //Farmhand.Events.ControlEvents.OnKeyReleased += ControlEvents_OnKeyReleased;
            //Farmhand.Events.ControlEvents.OnMouseChanged += ControlEvents_OnMouseChanged;
            //Farmhand.Events.ControlEvents.OnControllerButtonPressed += ControlEvents_OnControllerButtonPressed;
            //Farmhand.Events.ControlEvents.OnControllerButtonReleased += ControlEvents_OnControllerButtonReleased;
            //Farmhand.Events.ControlEvents.OnControllerTriggerPressed += ControlEvents_OnControllerTriggerPressed;
            //Farmhand.Events.ControlEvents.OnControllerButtonReleased += ControlEvents_OnControllerButtonReleased1;
            //Farmhand.Events.MenuEvents.OnMenuChanged += MenuEvents_OnMenuChanged;
            //Farmhand.Events.LocationEvents.OnCurrentLocationChanged += LocationEvents_OnCurrentLocationChanged;
            //Farmhand.Events.LocationEvents.OnLocationsChanged += LocationEvents_OnLocationsChanged;
            //Farmhand.Events.SaveEvents.OnAfterLoad += SaveEvents_OnAfterLoad;
            //Farmhand.Events.PlayerEvents.OnFarmerChanged += PlayerEvents_OnFarmerChanged;
            //// TODO: PlayerEvents.InventoryChanged
            //Farmhand.Events.PlayerEvents.OnLevelUp += PlayerEvents_OnLevelUp;
            //Farmhand.Events.TimeEvents.OnAfterTimeChanged += TimeEvents_OnAfterTimeChanged;
        }

        private static void TimeEvents_OnAfterTimeChanged(object sender, Farmhand.Events.Arguments.Common.EventArgsIntChanged e)
        {
            TimeEvents.InvokeTimeOfDayChanged(CompatibilityLayer.Monitor, e.PriorValue, e.NewValue);
        }

        private static void PlayerEvents_OnLevelUp(object sender, Farmhand.Events.Arguments.PlayerEvents.EventArgsOnLevelUp e)
        {
            EventArgsLevelUp.LevelType type;
            switch (e.Which)
            {
                case EventArgsOnLevelUp.LevelType.Farming:
                    type = EventArgsLevelUp.LevelType.Farming;
                    break;
                case EventArgsOnLevelUp.LevelType.Fishing:
                    type = EventArgsLevelUp.LevelType.Fishing;
                    break;
                case EventArgsOnLevelUp.LevelType.Foraging:
                    type = EventArgsLevelUp.LevelType.Foraging;
                    break;
                case EventArgsOnLevelUp.LevelType.Mining:
                    type = EventArgsLevelUp.LevelType.Mining;
                    break;
                case EventArgsOnLevelUp.LevelType.Combat:
                    type = EventArgsLevelUp.LevelType.Combat;
                    break;
                case EventArgsOnLevelUp.LevelType.Luck:
                    type = EventArgsLevelUp.LevelType.Luck;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PlayerEvents.InvokeLeveledUp(CompatibilityLayer.Monitor, type, e.NewLevel);
        }

        private static void PlayerEvents_OnFarmerChanged(object sender, Farmhand.Events.Arguments.PlayerEvents.EventArgsOnFarmerChanged e)
        {
            PlayerEvents.InvokeFarmerChanged(CompatibilityLayer.Monitor, e.PreviousFarmer, e.NewFarmer);
        }

        private static void SaveEvents_OnAfterLoad(object sender, Farmhand.Events.Arguments.SaveEvents.EventArgsOnAfterLoad e)
        {
            PlayerEvents.InvokeLoadedGame(CompatibilityLayer.Monitor, new EventArgsLoadedGameChanged(Game1.hasLoadedGame));
        }

        private static void LocationEvents_OnLocationsChanged(object sender, EventArgsLocationsChanged e)
        {
            LocationEvents.InvokeLocationsChanged(CompatibilityLayer.Monitor, e.NewLocations);
        }

        private static void LocationEvents_OnCurrentLocationChanged(object sender, Farmhand.Events.Arguments.LocationEvents.EventArgsOnCurrentLocationChanged e)
        {
            LocationEvents.InvokeCurrentLocationChanged(CompatibilityLayer.Monitor, e.PriorLocation, e.NewLocation);
        }

        private static void MenuEvents_OnMenuChanged(object sender, Farmhand.Events.Arguments.MenuEvents.EventArgsOnMenuChanged e)
        {
            if (e.NewMenu == null)
            {
                MenuEvents.InvokeMenuClosed(CompatibilityLayer.Monitor, e.PriorMenu);
            }
            else
            {
                MenuEvents.InvokeMenuChanged(CompatibilityLayer.Monitor, e.PriorMenu, e.NewMenu);
            }
        }

        private static void ControlEvents_OnControllerButtonReleased1(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerButtonReleased e)
        {
            ControlEvents.InvokeTriggerReleased(CompatibilityLayer.Monitor, e.PlayerIndex, e.ButtonReleased, 0.0f);
        }

        private static void ControlEvents_OnControllerTriggerPressed(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerTriggerPressed e)
        {
            ControlEvents.InvokeTriggerPressed(CompatibilityLayer.Monitor, e.PlayerIndex, e.ButtonPressed, e.Value);
        }

        private static void ControlEvents_OnControllerButtonReleased(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerButtonReleased e)
        {
            ControlEvents.InvokeButtonReleased(CompatibilityLayer.Monitor, e.PlayerIndex, e.ButtonReleased);
        }

        private static void ControlEvents_OnControllerButtonPressed(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerButtonPressed e)
        {
            ControlEvents.InvokeButtonReleased(CompatibilityLayer.Monitor, e.PlayerIndex, e.ButtonPressed);
        }

        private static void ControlEvents_OnMouseChanged(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsMouseStateChanged e)
        {
            var priorPoint = new Point(e.PriorState.X, e.PriorState.Y);
            var newPoint = new Point(e.PriorState.X, e.PriorState.Y);
            ControlEvents.InvokeMouseChanged(CompatibilityLayer.Monitor, e.PriorState, e.NewState, priorPoint, newPoint);
        }

        private static void ControlEvents_OnKeyReleased(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsKeyPressed e)
        {
            ControlEvents.InvokeKeyReleased(CompatibilityLayer.Monitor, e.KeyPressed);
        }

        private static void ControlEvents_OnKeyPressed(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsKeyPressed e)
        {
            ControlEvents.InvokeKeyPressed(CompatibilityLayer.Monitor, e.KeyPressed);
        }

        private static void ControlEvents_OnKeyboardChanged(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsKeyboardStateChanged e)
        {
            ControlEvents.InvokeKeyboardChanged(CompatibilityLayer.Monitor, e.PriorState, e.NewState);
        }
    }
}
