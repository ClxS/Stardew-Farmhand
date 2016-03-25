using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

// ReSharper disable CheckNamespace
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

        public static void InvokeKeyboardChanged(KeyboardState priorState, KeyboardState newState)
        {
            KeyboardChanged.Invoke(null, new EventArgsKeyboardStateChanged(priorState, newState));
        }

        public static void InvokeMouseChanged(MouseState priorState, MouseState newState)
        {
            MouseChanged.Invoke(null, new EventArgsMouseStateChanged(priorState, newState));
        }

        public static void InvokeKeyPressed(Keys key)
        {
            KeyPressed.Invoke(null, new EventArgsKeyPressed(key));
        }

        public static void InvokeKeyReleased(Keys key)
        {
            KeyReleased.Invoke(null, new EventArgsKeyPressed(key));
        }

        public static void InvokeButtonPressed(PlayerIndex playerIndex, Buttons buttons)
        {
            ControllerButtonPressed.Invoke(null, new EventArgsControllerButtonPressed(playerIndex, buttons));
        }

        public static void InvokeButtonReleased(PlayerIndex playerIndex, Buttons buttons)
        {
            ControllerButtonReleased.Invoke(null, new EventArgsControllerButtonReleased(playerIndex, buttons));
        }

        public static void InvokeTriggerPressed(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            ControllerTriggerPressed.Invoke(null, new EventArgsControllerTriggerPressed(playerIndex, buttons, value));
        }

        public static void InvokeTriggerReleased(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            ControllerTriggerReleased.Invoke(null, new EventArgsControllerTriggerReleased(playerIndex, buttons, value));
        }
    }
}
