namespace Farmhand.Events.Arguments.ControlEvents
{
    using System;

    using Microsoft.Xna.Framework.Input;

    /// <summary>
    ///     Arguments for KeyboardStateChanged.
    /// </summary>
    public class KeyboardStateChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyboardStateChangedEventArgs" /> class.
        /// </summary>
        /// <param name="priorState">
        ///     The prior state.
        /// </param>
        /// <param name="newState">
        ///     The new state.
        /// </param>
        public KeyboardStateChangedEventArgs(KeyboardState priorState, KeyboardState newState)
        {
            this.NewState = newState;
            this.PriorState = priorState;
        }

        /// <summary>
        ///     Gets the new keyboard state.
        /// </summary>
        public KeyboardState NewState { get; }

        /// <summary>
        ///     Gets the prior keyboard state.
        /// </summary>
        public KeyboardState PriorState { get; }
    }
}