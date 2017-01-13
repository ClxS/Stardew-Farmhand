namespace Farmhand.Events.Arguments.SaveEvents
{
    using System;

    /// <summary>
    ///     Arguments for ProgressEvent.
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProgressEventArgs" /> class.
        /// </summary>
        /// <param name="progress">
        ///     The progress.
        /// </param>
        public ProgressEventArgs(int progress)
        {
            this.Progress = progress;
        }

        /// <summary>
        ///     Gets the progress.
        /// </summary>
        public int Progress { get; }
    }
}