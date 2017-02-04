namespace Farmhand.Events.Arguments.Menus.TitleMenuEvents
{
    using System;

    /// <summary>
    ///     The after receive left click.
    /// </summary>
    public class AfterHoverEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AfterHoverEventArgs" /> class.
        /// </summary>
        /// <param name="x">
        ///     The x.
        /// </param>
        /// <param name="y">
        ///     The y.
        /// </param>
        public AfterHoverEventArgs(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        ///     Gets the X position.
        /// </summary>
        public int X { get; }

        /// <summary>
        ///     Gets the Y position.
        /// </summary>
        public int Y { get; }
    }
}