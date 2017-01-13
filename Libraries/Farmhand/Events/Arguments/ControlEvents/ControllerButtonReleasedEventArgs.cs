namespace Farmhand.Events.Arguments.ControlEvents
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    ///     Arguments for ControllerButtonReleased.
    /// </summary>
    public class ControllerButtonReleasedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ControllerButtonReleasedEventArgs" /> class.
        /// </summary>
        /// <param name="playerIndex">
        ///     The player index.
        /// </param>
        /// <param name="buttonReleased">
        ///     The button released.
        /// </param>
        public ControllerButtonReleasedEventArgs(PlayerIndex playerIndex, Buttons buttonReleased)
        {
            this.PlayerIndex = playerIndex;
            this.ButtonReleased = buttonReleased;
        }

        /// <summary>
        ///     Gets the player index.
        /// </summary>
        public PlayerIndex PlayerIndex { get; }

        /// <summary>
        ///     Gets the button released.
        /// </summary>
        public Buttons ButtonReleased { get; }
    }
}