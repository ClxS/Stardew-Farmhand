namespace Farmhand.Logging
{
    using System;

    using Farmhand.Logging.Loggers;

    /// <summary>
    ///     Contains general purpose logging functionality
    /// </summary>
    public static class Log
    {
        /// <summary>
        ///     Gets or sets a value indicating whether verbose logging is enabled.
        /// </summary>
        public static bool IsVerbose { get; set; } = true;

        private static ILogger Logger { get; set; } = new ConsoleLogger();

        /// <summary>
        ///     Sets the logger type
        /// </summary>
        /// <typeparam name="T">
        ///     A type which implements ILogger.
        /// </typeparam>
        public static void SetLoggerType<T>() where T : ILogger, new()
        {
            Logger = new T();
        }

        /// <summary>
        ///     Successful message to display to console and logging.
        /// </summary>
        /// <param name="message">
        ///     The message to display
        /// </param>
        public static void Success(string message)
        {
            var logItem = new LogEntry { Message = message, Type = LogEntryType.Success };
            Logger.Write(logItem);
        }

        /// <summary>
        ///     Warning message to display to console and logging.
        /// </summary>
        /// <param name="message">
        ///     The message to display
        /// </param>
        public static void Warning(string message)
        {
            var logItem = new LogEntry { Message = message, Type = LogEntryType.Warning };
            Logger.Write(logItem);
        }

        /// <summary>
        ///     Display mostly useless messages.
        /// </summary>
        /// <param name="message">
        ///     The message to display
        /// </param>
        public static void Verbose(string message)
        {
            if (IsVerbose)
            {
                var logItem = new LogEntry { Message = message, Type = LogEntryType.Verbose };
                Logger.Write(logItem);
            }
        }

        /// <summary>
        ///     Display useful, but not important messages.
        /// </summary>
        /// <param name="message">
        ///     The message to display
        /// </param>
        public static void Info(string message)
        {
            var logItem = new LogEntry { Message = message };
            Logger.Write(logItem);
        }

        /// <summary>
        ///     Display an exception.
        /// </summary>
        /// <param name="message">
        ///     The message to display
        /// </param>
        /// <param name="ex">
        ///     The exception to display.
        /// </param>
        public static void Exception(string message, Exception ex)
        {
            if (IsVerbose)
            {
                var innerEx = ex.InnerException ?? ex;
                var logItem = new LogEntry
                                  {
                                      Message = $"{message}\n\t{innerEx.Message}\n\t{innerEx.StackTrace}",
                                      Type = LogEntryType.Error
                                  };
                Logger.Write(logItem);
            }
            else
            {
                var innerEx = ex.InnerException ?? ex;
                var logItem = new LogEntry { Message = $"{message}\n\t{innerEx.Message}", Type = LogEntryType.Error };
                Logger.Write(logItem);
            }
        }

        /// <summary>
        ///     Important message indicating an error.
        /// </summary>
        /// <param name="message">The message to display</param>
        public static void Error(string message)
        {
            var logItem = new LogEntry { Message = message, Type = LogEntryType.Error };
            Logger.Write(logItem);
        }
    }
}