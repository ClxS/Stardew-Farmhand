using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Farmhand.Events;

namespace StardewModdingAPI.Events
{
    public static class ControlEvents
    {
        public static event EventHandler<EventArgsKeyboardStateChanged> KeyboardChanged = delegate { };
        public static event EventHandler<EventArgsKeyPressed> KeyPressed = delegate { };
        public static event EventHandler<EventArgsKeyPressed> KeyReleased = delegate { };
        public static event EventHandler<EventArgsMouseStateChanged> MouseChanged = delegate { };
        public static event EventHandler<EventArgsControllerButtonPressed> ControllerButtonPressed = delegate { };
        public static event EventHandler<EventArgsControllerButtonReleased> ControllerButtonReleased = delegate { };
        public static event EventHandler<EventArgsControllerTriggerPressed> ControllerTriggerPressed = delegate { };
        public static event EventHandler<EventArgsControllerTriggerReleased> ControllerTriggerReleased = delegate { };

        internal static void InvokeKeyboardChanged(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsKeyboardStateChanged e)
        {
            EventCommon.SafeInvoke(KeyboardChanged, sender, new EventArgsKeyboardStateChanged(e.PriorState, e.NewState));
        }

        internal static void InvokeMouseChanged(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsMouseStateChanged e)
        {
            EventCommon.SafeInvoke(MouseChanged, sender, new EventArgsMouseStateChanged(e.PriorState, e.NewState));
        }

        internal static void InvokeKeyPressed(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsKeyPressed e)
        {
            EventCommon.SafeInvoke(KeyPressed, sender, new EventArgsKeyPressed(e.KeyPressed));
        }

        internal static void InvokeKeyReleased(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsKeyPressed e)
        {
            EventCommon.SafeInvoke(KeyReleased, sender, new EventArgsKeyPressed(e.KeyPressed));
        }

        internal static void InvokeButtonPressed(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerButtonPressed e)
        {
            EventCommon.SafeInvoke(ControllerButtonPressed, sender, new EventArgsControllerButtonPressed(e.PlayerIndex, e.ButtonPressed));
        }

        internal static void InvokeButtonReleased(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerButtonReleased e)
        {
            EventCommon.SafeInvoke(ControllerButtonReleased, sender, new EventArgsControllerButtonReleased(e.PlayerIndex, e.ButtonReleased));
        }

        internal static void InvokeTriggerPressed(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerTriggerPressed e)
        {
            EventCommon.SafeInvoke(ControllerTriggerPressed, sender, new EventArgsControllerTriggerPressed(e.PlayerIndex, e.ButtonPressed, e.Value));
        }

        internal static void InvokeTriggerReleased(object sender, Farmhand.Events.Arguments.ControlEvents.EventArgsControllerTriggerReleased e)
        {
            EventCommon.SafeInvoke(ControllerTriggerReleased, sender, new EventArgsControllerTriggerReleased(e.PlayerIndex, e.ButtonReleased, e.Value));
        }
    }
}