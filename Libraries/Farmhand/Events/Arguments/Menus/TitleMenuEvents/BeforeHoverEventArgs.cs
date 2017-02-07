namespace Farmhand.Events.Arguments.Menus.TitleMenuEvents
{
    using System.ComponentModel;

    /// <summary>
    ///     The after receive left click.
    /// </summary>
    public class BeforeHoverEventArgs : CancelEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BeforeHoverEventArgs" /> class.
        /// </summary>
        /// <param name="x">
        ///     The x.
        /// </param>
        /// <param name="y">
        ///     The y.
        /// </param>
        public BeforeHoverEventArgs(int x, int y)
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