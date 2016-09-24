using System;
using System.Diagnostics;
using StardewValley;
using StardewValley.Menus;

namespace Farmhand.Logging.Loggers
{
    public class GameLogger : ILogger
    {
        public void Write(LogEntry logItem)
        {
            if (Game1.onScreenMenus == null || Game1.content == null) return;

            if (Game1.ChatBox == null)
            {
                Game1.onScreenMenus?.Add(new ChatBox());
            }
            if (Game1.ChatBox != null)
            {
                Game1.ChatBox?.receiveChatMessage($"[{DateTime.Now.ToLongTimeString()}] {logItem.Message}", 0L);
            }
        }
    }
}
