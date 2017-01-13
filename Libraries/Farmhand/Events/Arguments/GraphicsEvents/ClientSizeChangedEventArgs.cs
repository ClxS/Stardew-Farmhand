namespace Farmhand.Events.Arguments.GraphicsEvents
{
    using System;

    /// <summary>
    ///     Arguments for ClientSizeChanged.
    /// </summary>
    public class ClientSizeChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientSizeChangedEventArgs" /> class.
        /// </summary>
        /// <param name="width">
        ///     The width.
        /// </param>
        /// <param name="height">
        ///     The height.
        /// </param>
        public ClientSizeChangedEventArgs(int width, int height)
        {
            this.NewWidth = width;
            this.NewHeight = height;
        }

        /// <summary>
        ///     Gets the new width of the client.
        /// </summary>
        public int NewWidth { get; }

        /// <summary>
        ///     Gets the new height of the client.
        /// </summary>
        public int NewHeight { get; }
    }
}