namespace Farmhand.Events.Arguments.ControlEvents
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Arguments for ControllerTriggerReleased.
    /// </summary>
    public class ControllerTriggerReleasedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerTriggerReleasedEventArgs"/> class.
        /// </summary>
        /// <param name="playerIndex">
        /// The player index.
        /// </param>
        /// <param name="buttonReleased">
        /// The button released.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public ControllerTriggerReleasedEventArgs(PlayerIndex playerIndex, Buttons buttonReleased, float value)
        {
            this.PlayerIndex = playerIndex;
            this.ButtonReleased = buttonReleased;
            this.Value = value;
        }

        /// <summary>
        /// Gets the player index.
        /// </summary>
        public PlayerIndex PlayerIndex { get; }

        /// <summary>
        /// Gets the button released.
        /// </summary>
        public Buttons ButtonReleased { get; }

        /// <summary>
        /// Gets the strength of the trigger release.
        /// </summary>
        public float Value { get; }
    }
}