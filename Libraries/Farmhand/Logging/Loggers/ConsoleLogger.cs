using System;

namespace Farmhand.Logging.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Write(LogEntry logItem)
        {
            SetConsoleColour(logItem.Type);
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {logItem.Message}");
            SetConsoleColour(logItem.Type);
        }

        private void SetConsoleColour(LogEntryType type)
        {
            Console.ForegroundColor = ConvertConsoleColour(type);
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
                    return ConsoleColor.Yellow;
                default: return ConsoleColor.Gray;
            }

        }
    }
}
