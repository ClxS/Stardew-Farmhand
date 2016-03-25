using Revolution.Logging.Loggers;
using System;

namespace Revolution.Logging
{
    public static class Log
    {
        public static bool IsVerbose { get; set; } = true;
        private static ILogger Logger { get; set; } = new ConsoleLogger();

        public static void SetLoggerType<T>() where T : ILogger, new()
        {
            Logger = new T();
        }
        
        /// <summary>
        /// Successful message to display to console and logging.
        /// </summary>
        /// <param name="message"></param>
        public static void Success(string message)
        {
            LogEntry logItem = new LogEntry { Message = message, Color = LogEntryColor.Green };
            Logger.Write(logItem);
        }

        /// <summary>
        /// Generic comment to display to console and logging.
        /// </summary>
        /// <param name="message"></param>
        public static void Verbose(string message)
        {
            if (IsVerbose)
            {
                LogEntry logItem = new LogEntry { Message = message, Color = LogEntryColor.DarkGrey };
                Logger.Write(logItem);
            }
        }

        /// <summary>
        /// Message for only console. Does not appear in logging.
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            LogEntry logItem = new LogEntry { Message = message };
            Logger.Write(logItem);
        }

        internal static void Exception(string message, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (IsVerbose)
            {
                LogEntry logItem = new LogEntry { Message = $"{message}\n\t{ex.Message}\n\t{ex.StackTrace}", Color = LogEntryColor.Red };
                Logger.Write(logItem);                
            }
            else
            {
                LogEntry logItem = new LogEntry { Message = $"{message}\n\t{ex.Message}", Color = LogEntryColor.Red };
                Logger.Write(logItem);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Important message indicating an error.
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            LogEntry logItem = new LogEntry { Message = message, Color = LogEntryColor.Red };
            Logger.Write(logItem);
        }
    }
}
