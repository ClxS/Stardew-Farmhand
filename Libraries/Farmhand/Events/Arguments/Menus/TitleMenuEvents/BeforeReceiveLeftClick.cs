namespace Farmhand.Events.Arguments.Menus.TitleMenuEvents
{
    using System.ComponentModel;

    /// <summary>
    ///     The after receive left click.
    /// </summary>
    public class BeforeReceiveLeftClick : CancelEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BeforeReceiveLeftClick" /> class.
        /// </summary>
        /// <param name="x">
        ///     The x.
        /// </param>
        /// <param name="y">
        ///     The y.
        /// </param>
        /// <param name="playSound">
        ///     The play sound.
        /// </param>
        public BeforeReceiveLeftClick(int x, int y, bool playSound)
        {
            this.X = x;
            this.Y = y;
            this.PlaySound = playSound;
        }

        /// <summary>
        ///     Gets the X position.
        /// </summary>
        public int X { get; }

        /// <summary>
        ///     Gets the Y position.
        /// </summary>
        public int Y { get; }

        /// <summary>
        ///     Gets a value indicating whether to play click sound.
        /// </summary>
        public bool PlaySound { get; }
    }
}