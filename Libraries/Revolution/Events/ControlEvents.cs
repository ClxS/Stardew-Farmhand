using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Revolution.Attributes;
using System;

namespace Revolution.Events
{
    public class ControlEvents
    {
        public static event EventHandler OnKeyboardChanged = delegate { };
        public static event EventHandler OnKeyPressed = delegate { };
        public static event EventHandler OnKeyReleased = delegate { };
        public static event EventHandler OnMouseChanged = delegate { };
        public static event EventHandler OnControllerButtonPressed = delegate { };
        public static event EventHandler OnControllerButtonReleased = delegate { };
        public static event EventHandler OnControllerTriggerPressed = delegate { };
        public static event EventHandler OnControllerTriggerReleased = delegate { };

        [PendingHook]
        internal static void InvokeKeyboardChanged(KeyboardState priorState, KeyboardState newState)
        {
            EventCommon.SafeInvoke(OnKeyboardChanged, null);
        }

        [PendingHook]
        internal static void InvokeMouseChanged(MouseState priorState, MouseState newState)
        {
            EventCommon.SafeInvoke(OnMouseChanged, null);
        }

        [PendingHook]
        internal static void InvokeKeyPressed(Keys key)
        {
            EventCommon.SafeInvoke(OnKeyPressed, null);
        }

        [PendingHook]
        internal static void InvokeKeyReleased(Keys key)
        {
            EventCommon.SafeInvoke(OnKeyReleased, null);
        }

        [PendingHook]
        internal static void InvokeButtonPressed(PlayerIndex playerIndex, Buttons buttons)
        {
            EventCommon.SafeInvoke(OnControllerButtonPressed, null);
        }

        [PendingHook]
        internal static void InvokeButtonReleased(PlayerIndex playerIndex, Buttons buttons)
        {
            EventCommon.SafeInvoke(OnControllerButtonReleased, null);
        }

        [PendingHook]
        internal static void InvokeTriggerPressed(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            EventCommon.SafeInvoke(OnControllerTriggerPressed, null);
        }

        [PendingHook]
        internal static void InvokeTriggerReleased(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            EventCommon.SafeInvoke(OnControllerTriggerReleased, null);
        }
    }
}
