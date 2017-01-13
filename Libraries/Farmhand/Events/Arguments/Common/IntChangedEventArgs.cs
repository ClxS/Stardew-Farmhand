namespace Farmhand.Events.Arguments.Common
{
    using System;

    /// <summary>
    ///     Arguments for when an integer changes.
    /// </summary>
    public class IntChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IntChangedEventArgs" /> class.
        /// </summary>
        /// <param name="priorValue">
        ///     Original integer value.
        /// </param>
        /// <param name="newValue">
        ///     New integer value.
        /// </param>
        public IntChangedEventArgs(int priorValue, int newValue)
        {
            this.PriorValue = priorValue;
            this.NewValue = newValue;
        }

        /// <summary>
        ///     Gets the original integer value.
        /// </summary>
        public int PriorValue { get; }

        /// <summary>
        ///     Gets the new integer value.
        /// </summary>
        public int NewValue { get; }
    }
}