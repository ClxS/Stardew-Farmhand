namespace Farmhand.Events.Arguments.ControlEvents
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    ///     Arguments for ControllerButtonPressed.
    /// </summary>
    public class ControllerButtonPressedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ControllerButtonPressedEventArgs" /> class.
        /// </summary>
        /// <param name="playerIndex">
        ///     The player index.
        /// </param>
        /// <param name="buttonPressed">
        ///     The button pressed.
        /// </param>
        public ControllerButtonPressedEventArgs(PlayerIndex playerIndex, Buttons buttonPressed)
        {
            this.PlayerIndex = playerIndex;
            this.ButtonPressed = buttonPressed;
        }

        /// <summary>
        ///     Gets the player index.
        /// </summary>
        public PlayerIndex PlayerIndex { get; }

        /// <summary>
        ///     Gets the button pressed.
        /// </summary>
        public Buttons ButtonPressed { get; }
    }
}