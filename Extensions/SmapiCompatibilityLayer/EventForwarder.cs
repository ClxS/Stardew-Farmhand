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
        private static IMonitor _monitor;

        private static IMonitor Monitor
        {
            get
            {
                if (_monitor != null)
                {
                    return _monitor;
                }

                var projectType = typeof(StardewModdingAPI.Program);
                var fieldInfo = projectType.GetField("Monitor", BindingFlags.Static | BindingFlags.NonPublic);
                _monitor = (IMonitor)fieldInfo.GetValue(null);
                return _monitor;
            }
        }

        /// <summary>Raised after the in-game clock changes.</summary>
        public static event EventHandler<EventArgsIntChanged> TimeOfDayChanged;

        /// <summary>Raised after the day-of-month value changes, including when loading a save (unlike <see cref="OnNewDay"/>).</summary>
        public static event EventHandler<EventArgsIntChanged> DayOfMonthChanged;

        /// <summary>Raised after the year value changes.</summary>
        public static event EventHandler<EventArgsIntChanged> YearOfGameChanged;

        /// <summary>Raised after the season value changes.</summary>
        public static event EventHandler<EventArgsStringChanged> SeasonOfYearChanged;

        /// <summary>Raised when the player is transitioning to a new day and the game is performing its day update logic. This event is triggered twice: once after the game starts transitioning, and again after it finishes.</summary>
        public static event EventHandler<EventArgsNewDay> OnNewDay;

        public static void ForwardEvents()
        {
            // TODO:
            // MineEvents
            // TimeEvents
            // GameEvents
            Farmhand.Events.ControlEvents.OnKeyboardChanged += ControlEvents_OnKeyboardChanged;
            Farmhand.Events.ControlEvents.OnKeyPressed += ControlEvents_OnKeyPressed;
            Farmhand.Events.ControlEvents.OnKeyReleased += ControlEvents_OnKeyReleased;
            Farmhand.Events.ControlEvents.OnMouseChanged += ControlEvents_OnMouseChanged;
            Farmhand.Events.ControlEvents.OnControllerButtonPressed += ControlEvents_OnControllerButtonPressed;
            Farmhand.Events.ControlEvents.OnControllerButtonReleased += ControlEvents_OnControllerButtonReleased;
            Farmhand.Events.ControlEvents.OnControllerTriggerPressed += ControlEvents_OnControllerTriggerPressed;
            Farmhand.Events.ControlEvents.OnControllerButtonReleased += ControlEvents_OnControllerButtonReleased1;
            Farmhand.Events.MenuEvents.OnMenuChanged += MenuEvents_OnMenuChanged;
            Farmhand.Events.LocationEvents.OnCurrentLocationChanged += LocationEvents_OnCurrentLocationChanged;
            Farmhand.Events.LocationEvents.OnLocationsChanged += LocationEvents_OnLocationsChanged;
            Farmhand.Events.SaveEvents.OnAfterLoad += SaveEvents_OnAfterLoad;
            Farmhand.Events.PlayerEvents.OnFarmerChanged += PlayerEvents_OnFarmerChanged;
            // TODO: PlayerEvents.InventoryChanged
            Farmhand.Events.PlayerEvents.OnLevelUp += PlayerEvents_OnLevelUp;
            Farmhand.Events.TimeEvents.OnAfterTimeChanged += TimeEvents_OnAfterTimeChanged;
        }

        private static void TimeEvents_OnAfterTimeChanged(object sender, Farmhand.Events.Arguments.Common.EventArgsIntChanged e)
        {
            TimeEvents.InvokeTimeOfDayChanged(Monitor, e.PriorValue, e.NewValue);
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

            PlayerEvents.InvokeLeveledUp(Monitor, type, e.NewLevel);
        }

        private static void PlayerEvents_OnFarmerChanged(object sender, Farmhand.Events.Arguments.PlayerEvents.EventArgsOnFarmerChanged e)
        {
            PlayerEvents.InvokeFarmerChanged(Monitor, e.PreviousFarmer, e.NewFarmer);
        }

        private static void SaveEvents_OnAfterLoad(object sender, Farmhand.Events.Arguments.SaveEvents.EventArgsOnAfterLoad e)
        {
            PlayerEvents.InvokeLoadedGame(Monitor, new EventArgsLoadedGameChanged(Game1.hasLoadedGame));
        }

        private static void LocationEvents_OnLocationsChanged(object sender, EventArgsLocationsChanged e)
        {
            LocationEvents.InvokeLocationsChanged(Monitor, e.NewLocations);
        }

        private static void LocationEvents_OnCurrentLocationChanged(object sender, Farmhand.Events.Arguments.LocationEvents.EventArgsOnCurrentLocationChanged e)
        {
            LocationEvents.InvokeCurrentLocationChanged(Monitor, e.PriorLocation, e.NewLocation);
        }

        private static void MenuEvents_OnMenuChanged(object sender, Farmhand.Events.Arguments.MenuEvents.EventArgsOnMenuChanged e)
        {
            if (e.NewMenu == null)
            {
                MenuEvents.InvokeMenuClosed(Monitor, e.PriorMenu);
            }
            else
            {
                MenuEvents.InvokeMenuChanged(Monitor, e.PriorMenu, e.NewMenu);
            }
        }

        private static void ControlEvents_OnControllerButtonReleased1(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerButtonReleased e)
        {
            ControlEvents.InvokeTriggerReleased(Monitor, e.PlayerIndex, e.ButtonReleased, 0.0f);
        }

        private static void ControlEvents_OnControllerTriggerPressed(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerTriggerPressed e)
        {
            ControlEvents.InvokeTriggerPressed(Monitor, e.PlayerIndex, e.ButtonPressed, e.Value);
        }

        private static void ControlEvents_OnControllerButtonReleased(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerButtonReleased e)
        {
            ControlEvents.InvokeButtonReleased(Monitor, e.PlayerIndex, e.ButtonReleased);
        }

        private static void ControlEvents_OnControllerButtonPressed(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerButtonPressed e)
        {
            ControlEvents.InvokeButtonReleased(Monitor, e.PlayerIndex, e.ButtonPressed);
        }

        private static void ControlEvents_OnMouseChanged(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsMouseStateChanged e)
        {
            var priorPoint = new Point(e.PriorState.X, e.PriorState.Y);
            var newPoint = new Point(e.PriorState.X, e.PriorState.Y);
            ControlEvents.InvokeMouseChanged(Monitor, e.PriorState, e.NewState, priorPoint, newPoint);
        }

        private static void ControlEvents_OnKeyReleased(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsKeyPressed e)
        {
            ControlEvents.InvokeKeyReleased(Monitor, e.KeyPressed);
        }

        private static void ControlEvents_OnKeyPressed(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsKeyPressed e)
        {
            ControlEvents.InvokeKeyPressed(Monitor, e.KeyPressed);
        }

        private static void ControlEvents_OnKeyboardChanged(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsKeyboardStateChanged e)
        {
            ControlEvents.InvokeKeyboardChanged(Monitor, e.PriorState, e.NewState);
        }
    }
}
