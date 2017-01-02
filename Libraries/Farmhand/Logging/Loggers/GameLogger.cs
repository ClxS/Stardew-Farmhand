namespace Farmhand.Logging.Loggers
{
    using System;

    using StardewValley;
    using StardewValley.Menus;

    /// <summary>
    ///     Writes to the in-game Chat Box
    /// </summary>
    public class GameLogger : ILogger
    {
        #region ILogger Members

        /// <summary>
        ///     Writes the message to the log.
        /// </summary>
        /// <param name="logItem">
        ///     The entry to log.
        /// </param>
        public void Write(LogEntry logItem)
        {
            if (Game1.onScreenMenus == null || Game1.content == null)
            {
                return;
            }

            if (Game1.ChatBox == null)
            {
                Game1.onScreenMenus?.Add(new ChatBox());
            }

            if (Game1.ChatBox != null)
            {
                Game1.ChatBox?.receiveChatMessage(
                    $"[FHLOG-{this.GetTypeSuffix(logItem.Type)}] {DateTime.Now.ToLongTimeString()} - {logItem.Message}",
                    0L);
            }
        }

        #endregion

        private string GetTypeSuffix(LogEntryType type)
        {
            switch (type)
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
    }
}