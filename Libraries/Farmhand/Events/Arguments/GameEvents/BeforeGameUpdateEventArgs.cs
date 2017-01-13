namespace Farmhand.Events.Arguments.GameEvents
{
    using System.ComponentModel;

    using Microsoft.Xna.Framework;

    /// <summary>
    ///     Arguments for BeforeGameUpdate.
    /// </summary>
    public class BeforeGameUpdateEventArgs : CancelEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BeforeGameUpdateEventArgs" /> class.
        /// </summary>
        /// <param name="gameTime">
        ///     The game time.
        /// </param>
        public BeforeGameUpdateEventArgs(GameTime gameTime)
        {
            this.GameTime = gameTime;
        }

        /// <summary>
        ///     Gets the elapsed time last frame.
        /// </summary>
        public GameTime GameTime { get; }
    }
}