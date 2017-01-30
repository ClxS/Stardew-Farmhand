namespace Farmhand.Events.Arguments.Menus.TitleMenuEvents
{
    using System;

    /// <summary>
    ///     The after receive left click.
    /// </summary>
    public class AfterReceiveLeftClick : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AfterReceiveLeftClick" /> class.
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
        public AfterReceiveLeftClick(int x, int y, bool playSound)
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