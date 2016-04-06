using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Farmhand.Attributes;
using System;
using Farmhand.Events.Arguments.ControlEvents;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events relating to controls. (Keyboard/Mouse/Gamepad)
    /// </summary>
    public static class ControlEvents
    {
        /// <summary>
        /// Triggered on keyboard state change
        /// </summary>
        public static event EventHandler<EventArgsKeyboardStateChanged> OnKeyboardChanged = delegate { };
        /// <summary>
        /// Triggered on key press
        /// </summary>
        public static event EventHandler<EventArgsKeyPressed> OnKeyPressed = delegate { };
        /// <summary>
        /// Triggered on key release
        /// </summary>
        public static event EventHandler<EventArgsKeyPressed> OnKeyReleased = delegate { };
        /// <summary>
        /// Triggered on mouse state change
        /// </summary>
        public static event EventHandler<EventArgsMouseStateChanged> OnMouseChanged = delegate { };
        /// <summary>
        /// Triggered on controller button pressed
        /// </summary>
        public static event EventHandler<EventArgsControllerButtonPressed> OnControllerButtonPressed = delegate { };
        /// <summary>
        /// Triggered on controller button released
        /// </summary>
        public static event EventHandler<EventArgsControllerButtonReleased> OnControllerButtonReleased = delegate { };
        /// <summary>
        /// Triggered on controller trigger pressed
        /// </summary>
        public static event EventHandler<EventArgsControllerTriggerPressed> OnControllerTriggerPressed = delegate { };
        /// <summary>
        /// Triggered on controller trigger released
        /// </summary>
        public static event EventHandler<EventArgsControllerTriggerReleased> OnControllerTriggerReleased = delegate { };
        
        internal static void InvokeKeyboardChanged(KeyboardState priorState, KeyboardState newState)
        {
            EventCommon.SafeInvoke(OnKeyboardChanged, null, new EventArgsKeyboardStateChanged(priorState, newState));
        }
        
        internal static void InvokeMouseChanged(MouseState priorState, MouseState newState)
        {
            EventCommon.SafeInvoke(OnMouseChanged, null, new EventArgsMouseStateChanged(priorState, newState));
        }
        
        internal static void InvokeKeyPressed(Keys key)
        {
            EventCommon.SafeInvoke(OnKeyPressed, null, new EventArgsKeyPressed(key));
        }
        
        internal static void InvokeKeyReleased(Keys key)
        {
            EventCommon.SafeInvoke(OnKeyReleased, null, new EventArgsKeyPressed(key));
        }
        
        internal static void InvokeButtonPressed(PlayerIndex playerIndex, Buttons buttons)
        {
            EventCommon.SafeInvoke(OnControllerButtonPressed, null, new EventArgsControllerButtonPressed(playerIndex, buttons));
        }
        
        internal static void InvokeButtonReleased(PlayerIndex playerIndex, Buttons buttons)
        {
            EventCommon.SafeInvoke(OnControllerButtonReleased, null, new EventArgsControllerButtonReleased(playerIndex, buttons));
        }
        
        internal static void InvokeTriggerPressed(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            EventCommon.SafeInvoke(OnControllerTriggerPressed, null, new EventArgsControllerTriggerPressed(playerIndex, buttons, value));
        }
        
        internal static void InvokeTriggerReleased(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            EventCommon.SafeInvoke(OnControllerTriggerReleased, null, new EventArgsControllerTriggerReleased(playerIndex, buttons, value));
        }
    }
}
