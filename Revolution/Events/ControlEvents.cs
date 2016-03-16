using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Events
{
    class ControlEvents
    {
        public static event EventHandler KeyboardChanged = delegate { };
        public static event EventHandler KeyPressed = delegate { };
        public static event EventHandler KeyReleased = delegate { };
        public static event EventHandler MouseChanged = delegate { };
        public static event EventHandler ControllerButtonPressed = delegate { };
        public static event EventHandler ControllerButtonReleased = delegate { };
        public static event EventHandler ControllerTriggerPressed = delegate { };
        public static event EventHandler ControllerTriggerReleased = delegate { };

        public static void InvokeKeyboardChanged(KeyboardState priorState, KeyboardState newState)
        {
            KeyboardChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeMouseChanged(MouseState priorState, MouseState newState)
        {
            MouseChanged.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeKeyPressed(Keys key)
        {
            KeyPressed.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeKeyReleased(Keys key)
        {
            KeyReleased.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeButtonPressed(PlayerIndex playerIndex, Buttons buttons)
        {
            ControllerButtonPressed.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeButtonReleased(PlayerIndex playerIndex, Buttons buttons)
        {
            ControllerButtonReleased.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeTriggerPressed(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            ControllerTriggerPressed.Invoke(null, EventArgs.Empty);
        }

        public static void InvokeTriggerReleased(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            ControllerTriggerReleased.Invoke(null, EventArgs.Empty);
        }
    }
}
