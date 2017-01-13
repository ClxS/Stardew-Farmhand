namespace Farmhand.Events
{
    using System;

    using Farmhand.Events.Arguments.ControlEvents;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    ///     Contains events relating to controls. (Keyboard/Mouse/Gamepad)
    /// </summary>
    public static class ControlEvents
    {
        /// <summary>
        ///     Fired on keyboard state change.
        /// </summary>
        public static event EventHandler<EventArgsKeyboardStateChanged> KeyboardChanged = delegate { };

        /// <summary>
        ///     Fired on key press.
        /// </summary>
        public static event EventHandler<EventArgsKeyPressed> KeyPressed = delegate { };

        /// <summary>
        ///     Fired on key release.
        /// </summary>
        public static event EventHandler<EventArgsKeyPressed> KeyReleased = delegate { };

        /// <summary>
        ///     Fired on mouse state change.
        /// </summary>
        public static event EventHandler<EventArgsMouseStateChanged> MouseChanged = delegate { };

        /// <summary>
        ///     Fired on controller button pressed.
        /// </summary>
        public static event EventHandler<EventArgsControllerButtonPressed> ControllerButtonPressed = delegate { };

        /// <summary>
        ///     Fired on controller button released.
        /// </summary>
        public static event EventHandler<EventArgsControllerButtonReleased> ControllerButtonReleased = delegate { };

        /// <summary>
        ///     Fired on controller trigger pressed.
        /// </summary>
        public static event EventHandler<EventArgsControllerTriggerPressed> ControllerTriggerPressed = delegate { };

        /// <summary>
        ///     Fired on controller trigger released.
        /// </summary>
        public static event EventHandler<EventArgsControllerTriggerReleased> ControllerTriggerReleased = delegate { };

        internal static void OnKeyboardChanged(KeyboardState priorState, KeyboardState newState)
        {
            EventCommon.SafeInvoke(KeyboardChanged, null, new EventArgsKeyboardStateChanged(priorState, newState));
        }

        internal static void OnMouseChanged(MouseState priorState, MouseState newState)
        {
            EventCommon.SafeInvoke(MouseChanged, null, new EventArgsMouseStateChanged(priorState, newState));
        }

        internal static void OnKeyPressed(Keys key)
        {
            EventCommon.SafeInvoke(KeyPressed, null, new EventArgsKeyPressed(key));
        }

        internal static void OnKeyReleased(Keys key)
        {
            EventCommon.SafeInvoke(KeyReleased, null, new EventArgsKeyPressed(key));
        }

        internal static void OnButtonPressed(PlayerIndex playerIndex, Buttons buttons)
        {
            EventCommon.SafeInvoke(
                ControllerButtonPressed,
                null,
                new EventArgsControllerButtonPressed(playerIndex, buttons));
        }

        internal static void OnButtonReleased(PlayerIndex playerIndex, Buttons buttons)
        {
            EventCommon.SafeInvoke(
                ControllerButtonReleased,
                null,
                new EventArgsControllerButtonReleased(playerIndex, buttons));
        }

        internal static void OnTriggerPressed(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            EventCommon.SafeInvoke(
                ControllerTriggerPressed,
                null,
                new EventArgsControllerTriggerPressed(playerIndex, buttons, value));
        }

        internal static void OnTriggerReleased(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            EventCommon.SafeInvoke(
                ControllerTriggerReleased,
                null,
                new EventArgsControllerTriggerReleased(playerIndex, buttons, value));
        }
    }
}