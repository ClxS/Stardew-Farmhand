namespace Farmhand.Logging.Loggers
{
    using System;

    /// <summary>
    ///     Writes to the console using Console.WriteLine
    /// </summary>
    public class ConsoleLogger : ILogger
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
            this.SetConsoleColour(logItem.Type);
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {logItem.Message}");
            this.SetConsoleColour(logItem.Type);
        }

        #endregion

        private void SetConsoleColour(LogEntryType type)
        {
            Console.ForegroundColor = this.ConvertConsoleColour(type);
        }

        private ConsoleColor ConvertConsoleColour(LogEntryType type)
        {
            switch (type)
            {
                case LogEntryType.Verbose:
                    return ConsoleColor.DarkGray;
                case LogEntryType.Info:
                    return ConsoleColor.Gray;
                case LogEntryType.Success:
                    return ConsoleColor.Green;
                case LogEntryType.Error:
                    return ConsoleColor.Red;
                case LogEntryType.Comment:
                    return ConsoleColor.DarkGreen;
                case LogEntryType.Warning:
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.Gray;
            }
        }
    }
}