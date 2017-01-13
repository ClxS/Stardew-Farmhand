namespace Farmhand.Events.Arguments.TimeEvents
{
    using System;

    /// <summary>
    ///     Arguments for ShouldTimePass.
    /// </summary>
    public class ShouldTimePassEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShouldTimePassEventArgs" /> class.
        /// </summary>
        /// <param name="timeShouldPass">
        ///     The time should pass.
        /// </param>
        public ShouldTimePassEventArgs(bool timeShouldPass)
        {
            this.TimeShouldPass = timeShouldPass;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether time should pass.
        /// </summary>
        public bool TimeShouldPass { get; set; }
    }
}