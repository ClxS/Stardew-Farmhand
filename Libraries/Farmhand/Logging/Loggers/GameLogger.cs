using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Farmhand.Attributes;
using Farmhand.Events;
using Farmhand.Events.Arguments;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace Farmhand.Logging.Loggers
{
    public class ChatBoxDrawEventArgs : CancelEventArgs
    {
        public ChatBoxDrawEventArgs(ChatBox chatBox, SpriteBatch spriteBatch)
        {
            ChatBox = chatBox;
            SpriteBatch = spriteBatch;
        }
        public ChatBox ChatBox;
        public SpriteBatch SpriteBatch;
    }

    public class GameLogger : ILogger
    {
        public static EventHandler<ChatBoxDrawEventArgs> ChatBoxDraw = delegate { };

        public void Write(LogEntry logItem)
        {
            if (Game1.onScreenMenus == null || Game1.content == null) return;

            if (Game1.ChatBox == null)
            {
                Game1.onScreenMenus?.Add(new ChatBox());
            }
            if (Game1.ChatBox != null)
            {
                Game1.ChatBox?.receiveChatMessage($"[FHLOG-{GetTypeSuffix(logItem.Type)}] {DateTime.Now.ToLongTimeString()} - {logItem.Message}", 0L);
            }
        }

        private string GetTypeSuffix(LogEntryType type)
        {
            switch(type)
            {
                case LogEntryType.Info:
                    return "Inf";
                case LogEntryType.Verbose:
                    return "Ver";
                case LogEntryType.Success:
                    return "Suc";
                case LogEntryType.Warning:
                    return "War";
                case LogEntryType.Error:
                    return "Err";
                case LogEntryType.Comment:
                    return "Com";
                default:
                    return "Ver";
            }
        }

        [Hook(HookType.Entry, "StardewValley.Menus.ChatBox", "draw")]
        public static bool draw([ThisBind] object @this, [InputBind(typeof(SpriteBatch), "b")] SpriteBatch b)
        {
            if (ChatBoxDraw.GetInvocationList().Length > 1)
            {
                return EventCommon.SafeCancellableInvoke(ChatBoxDraw, null, new ChatBoxDrawEventArgs((ChatBox)@this, b));
            }
            else
            {
                return true;
            }
        }
    }
}
