namespace Farmhand.Events.Arguments.SaveEvents
{
    using System.ComponentModel;

    /// <summary>
    ///     Arguments for BeforeLoad.
    /// </summary>
    public class BeforeLoadEventArgs : CancelEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BeforeLoadEventArgs" /> class.
        /// </summary>
        /// <param name="filename">
        ///     The filename.
        /// </param>
        public BeforeLoadEventArgs(string filename)
        {
            this.Filename = filename;
        }

        /// <summary>
        ///     Gets the filename.
        /// </summary>
        public string Filename { get; }
    }
}