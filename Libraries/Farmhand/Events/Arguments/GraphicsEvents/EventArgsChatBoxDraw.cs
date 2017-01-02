namespace Farmhand.Events.Arguments.GraphicsEvents
{
    using System.ComponentModel;

    using Microsoft.Xna.Framework.Graphics;

    using StardewValley.Menus;

    public class EventArgsChatBoxDraw : CancelEventArgs
    {
        public EventArgsChatBoxDraw(ChatBox chatBox, SpriteBatch spriteBatch)
        {
            this.ChatBox = chatBox;
            this.SpriteBatch = spriteBatch;
        }

        public ChatBox ChatBox { get; set; }

        public SpriteBatch SpriteBatch { get; set; }
    }
}