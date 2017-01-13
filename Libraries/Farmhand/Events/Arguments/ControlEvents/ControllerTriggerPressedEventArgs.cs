namespace Farmhand.Events.Arguments.ControlEvents
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    ///     Arguments for ControllerTriggerPressed.
    /// </summary>
    public class ControllerTriggerPressedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ControllerTriggerPressedEventArgs" /> class.
        /// </summary>
        /// <param name="playerIndex">
        ///     The player index.
        /// </param>
        /// <param name="buttonPressed">
        ///     The button pressed.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        public ControllerTriggerPressedEventArgs(PlayerIndex playerIndex, Buttons buttonPressed, float value)
        {
            this.PlayerIndex = playerIndex;
            this.ButtonPressed = buttonPressed;
            this.Value = value;
        }

        /// <summary>
        ///     Gets the player index.
        /// </summary>
        public PlayerIndex PlayerIndex { get; }

        /// <summary>
        ///     Gets the button pressed.
        /// </summary>
        public Buttons ButtonPressed { get; }

        /// <summary>
        ///     Gets the strength of the trigger press.
        /// </summary>
        public float Value { get; }
    }
}