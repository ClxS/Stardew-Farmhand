using Farmhand.Logging.Loggers;
using System;

namespace Farmhand.Logging
{
    /// <summary>
    /// Contains general purpose logging functionality
    /// </summary>
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
            var logItem = new LogEntry { Message = message, Type = LogEntryType.Success };
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
                var logItem = new LogEntry { Message = message, Type = LogEntryType.Verbose };
                Logger.Write(logItem);
            }
        }

        /// <summary>
        /// Message for only console. Does not appear in logging.
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            var logItem = new LogEntry { Message = message };
            Logger.Write(logItem);
        }

        public static void Exception(string message, Exception ex)
        {
            if (IsVerbose)
            {
                var exInner = ex.InnerException ?? ex;
                var logItem = new LogEntry { Message = $"{message}\n\t{exInner.Message}\n\t{exInner.StackTrace}", Type = LogEntryType.Error };
                Logger.Write(logItem);                
            }
            else
            {
                var exInner = ex.InnerException ?? ex;
                var logItem = new LogEntry { Message = $"{message}\n\t{exInner.Message}", Type = LogEntryType.Error };
                Logger.Write(logItem);
            }
        }

        /// <summary>
        /// Important message indicating an error.
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            var logItem = new LogEntry { Message = message, Type = LogEntryType.Error };
            Logger.Write(logItem);
        }
    }
}
