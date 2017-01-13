namespace Farmhand.Events.Arguments.GraphicsEvents
{
    using System.ComponentModel;

    using Microsoft.Xna.Framework.Graphics;

    using StardewValley.Menus;

    /// <summary>
    ///     Arguments for ChatBoxDraw.
    /// </summary>
    public class ChatBoxDrawEventArgs : CancelEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatBoxDrawEventArgs" /> class.
        /// </summary>
        /// <param name="chatBox">
        ///     The chat box to draw.
        /// </param>
        /// <param name="spriteBatch">
        ///     The sprite batch to use for drawing.
        /// </param>
        public ChatBoxDrawEventArgs(ChatBox chatBox, SpriteBatch spriteBatch)
        {
            this.ChatBox = chatBox;
            this.SpriteBatch = spriteBatch;
        }

        /// <summary>
        ///     Gets the chat box to draw.
        /// </summary>
        public ChatBox ChatBox { get; }

        /// <summary>
        ///     Gets the sprite batch to use for drawing.
        /// </summary>
        public SpriteBatch SpriteBatch { get; }
    }
}