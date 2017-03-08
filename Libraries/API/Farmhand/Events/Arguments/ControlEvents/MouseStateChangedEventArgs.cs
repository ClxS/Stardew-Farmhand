namespace Farmhand.Events.Arguments.ControlEvents
{
    using System;

    using Microsoft.Xna.Framework.Input;

    /// <summary>
    ///     Arguments for MouseStateChanged.
    /// </summary>
    public class MouseStateChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MouseStateChangedEventArgs" /> class.
        /// </summary>
        /// <param name="priorState">
        ///     The prior state.
        /// </param>
        /// <param name="newState">
        ///     The new state.
        /// </param>
        public MouseStateChangedEventArgs(MouseState priorState, MouseState newState)
        {
            this.NewState = newState;
            this.PriorState = priorState;
        }

        /// <summary>
        ///     Gets the new mouse state.
        /// </summary>
        public MouseState NewState { get; }

        /// <summary>
        ///     Gets the prior mouse state.
        /// </summary>
        public MouseState PriorState { get; }
    }
}