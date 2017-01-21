namespace Farmhand.Events.Arguments.GraphicsEvents
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///     Arguments for a range of drawing events.
    /// </summary>
    public class DrawEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DrawEventArgs" /> class.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The sprite batch.
        /// </param>
        /// <param name="gameTime">
        ///     The game time.
        /// </param>
        /// <param name="screen">
        ///     The game primary render target.
        /// </param>
        public DrawEventArgs(SpriteBatch spriteBatch, GameTime gameTime, RenderTarget2D screen)
        {
            this.SpriteBatch = spriteBatch;
            this.GameTime = gameTime;
            this.Screen = screen;
        }

        /// <summary>
        ///     Gets the sprite batch.
        /// </summary>
        public SpriteBatch SpriteBatch { get; }

        /// <summary>
        ///     Gets the game time.
        /// </summary>
        public GameTime GameTime { get; }

        /// <summary>
        ///     Gets the games primary render target.
        /// </summary>
        public RenderTarget2D Screen { get; }
    }
}