namespace Farmhand.Events.Arguments.Common
{
    using System;

    /// <summary>
    ///     Arguments for returnable events.
    /// </summary>
    public class ReturnableEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnableEventArgs" /> class.
        /// </summary>
        public ReturnableEventArgs()
        {
            this.IsHandled = false;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether a return value has been
        ///     set.
        /// </summary>
        public bool IsHandled { get; protected set; }
    }
}