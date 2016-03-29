using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Revolution.Events;

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

        internal static void InvokeKeyboardChanged(object sender, Revolution.Events.Arguments.ControlEvents.EventArgsKeyboardStateChanged e)
        {
            EventCommon.SafeInvoke(KeyboardChanged, null, new EventArgsKeyboardStateChanged(e.PriorState, e.NewState));
        }

        internal static void InvokeMouseChanged(object sender, Revolution.Events.Arguments.ControlEvents.EventArgsMouseStateChanged e)
        {
            EventCommon.SafeInvoke(MouseChanged, null, new EventArgsMouseStateChanged(e.PriorState, e.NewState));
        }

        internal static void InvokeKeyPressed(object sender, Revolution.Events.Arguments.ControlEvents.EventArgsKeyPressed e)
        {
            EventCommon.SafeInvoke(KeyPressed, null, new EventArgsKeyPressed(e.KeyPressed));
        }

        internal static void InvokeKeyReleased(object sender, Revolution.Events.Arguments.ControlEvents.EventArgsKeyPressed e)
        {
            EventCommon.SafeInvoke(KeyReleased, null, new EventArgsKeyPressed(e.KeyPressed));
        }

        internal static void InvokeButtonPressed(object sender, Revolution.Events.Arguments.ControlEvents.EventArgsControllerButtonPressed e)
        {
            EventCommon.SafeInvoke(ControllerButtonPressed, null, new EventArgsControllerButtonPressed(e.PlayerIndex, e.ButtonPressed));
        }

        internal static void InvokeButtonReleased(object sender, Revolution.Events.Arguments.ControlEvents.EventArgsControllerButtonReleased e)
        {
            EventCommon.SafeInvoke(ControllerButtonReleased, null, new EventArgsControllerButtonReleased(e.PlayerIndex, e.ButtonReleased));
        }

        internal static void InvokeTriggerPressed(object sender, Revolution.Events.Arguments.ControlEvents.EventArgsControllerTriggerPressed e)
        {
            EventCommon.SafeInvoke(ControllerTriggerPressed, null, new EventArgsControllerTriggerPressed(e.PlayerIndex, e.ButtonPressed, e.Value));
        }

        internal static void InvokeTriggerReleased(object sender, Revolution.Events.Arguments.ControlEvents.EventArgsControllerTriggerReleased e)
        {
            EventCommon.SafeInvoke(ControllerTriggerReleased, null, new EventArgsControllerTriggerReleased(e.PlayerIndex, e.ButtonReleased, e.Value));
        }
    }
}