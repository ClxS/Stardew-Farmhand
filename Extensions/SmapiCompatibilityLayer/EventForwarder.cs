using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;

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


        public static void ForwardEvents()
        {
            Farmhand.Events.ControlEvents.OnKeyboardChanged += ControlEvents_OnKeyboardChanged;
            Farmhand.Events.ControlEvents.OnKeyPressed += ControlEvents_OnKeyPressed;
            Farmhand.Events.ControlEvents.OnKeyReleased += ControlEvents_OnKeyReleased;
            Farmhand.Events.ControlEvents.OnMouseChanged += ControlEvents_OnMouseChanged;
            Farmhand.Events.ControlEvents.OnControllerButtonPressed += ControlEvents_OnControllerButtonPressed;
            Farmhand.Events.ControlEvents.OnControllerButtonReleased += ControlEvents_OnControllerButtonReleased;
            Farmhand.Events.ControlEvents.OnControllerTriggerPressed += ControlEvents_OnControllerTriggerPressed;
            Farmhand.Events.ControlEvents.OnControllerButtonReleased += ControlEvents_OnControllerButtonReleased1;
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
