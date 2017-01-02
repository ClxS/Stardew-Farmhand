namespace Farmhand.UI
{
    using Farmhand.Attributes;
    using Farmhand.Events;
    using Farmhand.Events.Arguments.GraphicsEvents;
    using Farmhand.Logging;
    using Farmhand.Logging.Loggers;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using StardewValley;

    /// <summary>
    ///     A draw handler responsible for the chat box
    /// </summary>
    public static class ChatBoxDrawHandler
    {
        /// <summary>
        ///     Hooks up the GameLogger.ChatBoxDraw to our ChatBoxDraw method. Invoked via a hook into EventManager.ManualHookup.
        /// </summary>
        [Hook(HookType.Entry, "Farmhand.Events.EventManager", "ManualHookup")]
        public static void HookUpDrawEvent()
        {
            GraphicsEvents.ChatBoxDraw += ChatBoxDraw;
        }

        private static LogEntryType GetLogEntryType(string message)
        {
            if (message.StartsWith(":: [FHLOG-Err"))
            {
                return LogEntryType.Error;
            }

            if (message.StartsWith(":: [FHLOG-Com"))
            {
                return LogEntryType.Comment;
            }

            if (message.StartsWith(":: [FHLOG-Inf"))
            {
                return LogEntryType.Info;
            }

            if (message.StartsWith(":: [FHLOG-Suc"))
            {
                return LogEntryType.Success;
            }

            if (message.StartsWith(":: [FHLOG-Ver"))
            {
                return LogEntryType.Verbose;
            }

            if (message.StartsWith(":: [FHLOG-War"))
            {
                return LogEntryType.Warning;
            }

            return LogEntryType.Verbose;
        }
        
        private static Color ConvertChatColour(LogEntryType type)
        {
            switch (type)
            {
                case LogEntryType.Verbose:
                    return Color.DarkGray;
                case LogEntryType.Info:
                    return Color.Gray;
                case LogEntryType.Success:
                    return Color.Green;
                case LogEntryType.Error:
                    return Color.Red;
                case LogEntryType.Comment:
                    return Color.DarkGreen;
                case LogEntryType.Warning:
                    return Color.Yellow;
                default:
                    return Color.Gray;
            }
        }

        private static void ChatBoxDraw(object sender, EventArgsChatBoxDraw chatBoxDrawEventArgs)
        {
            var @this = chatBoxDrawEventArgs.ChatBox;
            var b = chatBoxDrawEventArgs.SpriteBatch;
            var num = 0;
            for (var index = @this.messages.Count - 1; index >= 0; --index)
            {
                if (@this.messages[index].message.StartsWith(":: [FHLOG"))
                {
                    var text = @this.messages[index].message;
                    var type = GetLogEntryType(text);
                    text = text.Remove(0, 13);
                    num += @this.messages[index].verticalSize;

                    b.DrawString(
                        @this.chatBox._font,
                        text,
                        new Vector2(4f, Game1.viewport.Height - num - 8),
                        ConvertChatColour(type) * @this.messages[index].alpha,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        0.99f);
                }
                else
                {
                    num += @this.messages[index].verticalSize;
                    b.DrawString(
                        @this.chatBox._font,
                        @this.messages[index].message,
                        new Vector2(4f, Game1.viewport.Height - num - 8),
                        @this.chatBox.textColor * @this.messages[index].alpha,
                        0.0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        0.99f);
                }
            }

            if (@this.chatBox.Selected)
            {
                @this.chatBox.Draw(b);
            }

            @this.update();

            chatBoxDrawEventArgs.Cancel = true;
        }
    }
}