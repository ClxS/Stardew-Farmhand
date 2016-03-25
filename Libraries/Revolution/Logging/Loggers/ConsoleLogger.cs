using System;

namespace Revolution.Logging.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Write(LogEntry logItem)
        {
            SetConsoleColour(logItem.Color);
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {logItem.Message}");
            SetConsoleColour(logItem.Color);
        }

        private void SetConsoleColour(LogEntryColor colour)
        {
            Console.ForegroundColor = ConvertConsoleColour(colour);
        }

        private ConsoleColor ConvertConsoleColour(LogEntryColor colour)
        {
            switch (colour)
            {
                case LogEntryColor.DarkGrey:
                    return ConsoleColor.DarkGray;
                case LogEntryColor.Grey:
                    return ConsoleColor.Gray;
                case LogEntryColor.Green:
                    return ConsoleColor.Green;
                case LogEntryColor.Red:
                    return ConsoleColor.Red;
                case LogEntryColor.Yellow:
                    return ConsoleColor.Yellow;
                default: return ConsoleColor.Gray;
            }

        }
    }
}
