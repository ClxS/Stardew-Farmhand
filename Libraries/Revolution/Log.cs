using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution
{
    public class Log
    {
        public static bool IsVerbose { get; set; }

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
            Console.ForegroundColor = ConsoleColor.Green;
            LogInternal(message);
            Console.ForegroundColor = ConsoleColor.Gray;
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
                Console.ForegroundColor = ConsoleColor.DarkGray;
                LogInternal(message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        /// <summary>
        /// Message for only console. Does not appear in logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            LogInternal(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Important message indicating an error.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            LogInternal(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
