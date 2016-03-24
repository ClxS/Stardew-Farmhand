using Revolution.Logging.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Logging
{
    public class Log
    {
        public static bool IsVerbose { get; set; } = false;
        private static ILogger Logger { get; set; } = new ConsoleLogger();

        public static void SetLoggerType<T>() where T : ILogger, new()
        {
            Logger = new T();
        }
        
        /// <summary>
        /// Print provided parameters to the console/file as applicable
        /// </summary>
        /// <param name="message">Desired message</param>
        /// <param name="disableLogging">When true, writes to ONLY console and not the log file.</param>
        /// <param name="values">Additional params to be added to the message</param>
        private static void LogInternal(string message)
        {
            string logOutput = $"[{DateTime.Now.ToLongTimeString()}] {message}";
            Console.WriteLine(logOutput);
        }

        /// <summary>
        /// Successful message to display to console and logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Success(string message)
        {
            LogEntry logItem = new LogEntry() { Message = message, Color = LogEntryColor.Green };
            Logger.Write(logItem);
        }

        /// <summary>
        /// Generic comment to display to console and logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Verbose(string message)
        {
            if (IsVerbose)
            {
                LogEntry logItem = new LogEntry() { Message = message, Color = LogEntryColor.DarkGrey };
                Logger.Write(logItem);
            }
        }

        /// <summary>
        /// Message for only console. Does not appear in logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Info(string message)
        {
            LogEntry logItem = new LogEntry() { Message = message };
            Logger.Write(logItem);
        }

        internal static void Exception(string message, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (IsVerbose)
            {
                LogEntry logItem = new LogEntry() { Message = $"{message}\n\t{ex.Message}\n\t{ex.StackTrace}", Color = LogEntryColor.Red };
                Logger.Write(logItem);                
            }
            else
            {
                LogEntry logItem = new LogEntry() { Message = $"{message}\n\t{ex.Message}", Color = LogEntryColor.Red };
                Logger.Write(logItem);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Important message indicating an error.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Error(string message)
        {
            LogEntry logItem = new LogEntry() { Message = message, Color = LogEntryColor.Red };
            Logger.Write(logItem);
        }
    }
}
