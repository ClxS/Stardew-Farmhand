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
        public static event EventHandler<KeyboardStateChangedEventArgs> KeyboardChanged = delegate { };

        /// <summary>
        ///     Fired on key press.
        /// </summary>
        public static event EventHandler<KeyPressedEventArgs> KeyPressed = delegate { };

        /// <summary>
        ///     Fired on key release.
        /// </summary>
        public static event EventHandler<KeyPressedEventArgs> KeyReleased = delegate { };

        /// <summary>
        ///     Fired on mouse state change.
        /// </summary>
        public static event EventHandler<MouseStateChangedEventArgs> MouseChanged = delegate { };

        /// <summary>
        ///     Fired on controller button pressed.
        /// </summary>
        public static event EventHandler<ControllerButtonPressedEventArgs> ControllerButtonPressed = delegate { };

        /// <summary>
        ///     Fired on controller button released.
        /// </summary>
        public static event EventHandler<ControllerButtonReleasedEventArgs> ControllerButtonReleased = delegate { };

        /// <summary>
        ///     Fired on controller trigger pressed.
        /// </summary>
        public static event EventHandler<ControllerTriggerPressedEventArgs> ControllerTriggerPressed = delegate { };

        /// <summary>
        ///     Fired on controller trigger released.
        /// </summary>
        public static event EventHandler<ControllerTriggerReleasedEventArgs> ControllerTriggerReleased = delegate { };

        internal static void OnKeyboardChanged(KeyboardState priorState, KeyboardState newState)
        {
            EventCommon.SafeInvoke(KeyboardChanged, null, new KeyboardStateChangedEventArgs(priorState, newState));
        }

        internal static void OnMouseChanged(MouseState priorState, MouseState newState)
        {
            EventCommon.SafeInvoke(MouseChanged, null, new MouseStateChangedEventArgs(priorState, newState));
        }

        internal static void OnKeyPressed(Keys key)
        {
            EventCommon.SafeInvoke(KeyPressed, null, new KeyPressedEventArgs(key));
        }

        internal static void OnKeyReleased(Keys key)
        {
            EventCommon.SafeInvoke(KeyReleased, null, new KeyPressedEventArgs(key));
        }

        internal static void OnButtonPressed(PlayerIndex playerIndex, Buttons buttons)
        {
            EventCommon.SafeInvoke(
                ControllerButtonPressed,
                null,
                new ControllerButtonPressedEventArgs(playerIndex, buttons));
        }

        internal static void OnButtonReleased(PlayerIndex playerIndex, Buttons buttons)
        {
            EventCommon.SafeInvoke(
                ControllerButtonReleased,
                null,
                new ControllerButtonReleasedEventArgs(playerIndex, buttons));
        }

        internal static void OnTriggerPressed(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            EventCommon.SafeInvoke(
                ControllerTriggerPressed,
                null,
                new ControllerTriggerPressedEventArgs(playerIndex, buttons, value));
        }

        internal static void OnTriggerReleased(PlayerIndex playerIndex, Buttons buttons, float value)
        {
            EventCommon.SafeInvoke(
                ControllerTriggerReleased,
                null,
                new ControllerTriggerReleasedEventArgs(playerIndex, buttons, value));
        }
    }
}