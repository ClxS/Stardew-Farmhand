using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace StardewModdingAPI
{
    public class Log
    {
        private static StreamWriter _logStream;
        private static string _logPath;

        /// <summary>
        /// Set up the logging stream
        /// </summary>
        /// <param name="logPath"></param>
        public static void Initialize(string logPath)
        {
            _logPath = logPath;
            var logFile = string.Format($"{logPath}\\MODDED_ProgramLog.Log_LATEST.txt");
            try
            {
                _logStream = new StreamWriter(logFile, false);
            }
            catch (Exception)
            {
                // TODO: not use general exception
                Log.Error("Could not initialize LogStream - Logging is disabled");
            }
        }

        /// <summary>
        /// Print provided parameters to the console/file as applicable
        /// </summary>
        /// <param name="message">Desired message</param>
        /// <param name="suppressMessage">When true, writes to ONLY console and not the log file.</param>
        /// <param name="values">Additional params to be added to the message</param>
        private static void PrintLog(object message, bool disableLogging, params object[] values)
        {
            string logOutput = string.Format("[{0}] {1}", System.DateTime.Now.ToLongTimeString(), String.Format(message.ToString(), values));
            Console.WriteLine(logOutput);

            if (_logStream != null && !disableLogging)
            {
                _logStream.WriteLine(logOutput);
                _logStream.Flush();
            }
        }

        /// <summary>
        /// Successful message to display to console and logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Success(object message, params object[] values)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Log.PrintLog(message?.ToString(), false, values);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Generic comment to display to console and logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Verbose(object message, params object[] values)
        {
            Log.PrintLog(message?.ToString(), false, values);
        }

        /// <summary>
        /// Additional comment to display to console and logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Comment(object message, params object[] values)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log.PrintLog(message?.ToString(), false, values);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Message for only console. Does not appear in logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Info(object message, params object[] values)
        {
            Log.PrintLog(message.ToString(), true, values);
        }

        /// <summary>
        /// Important message indicating an error.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Error(object message, params object[] values)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log.PrintLog(message.ToString(), false, values);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// A message displayed only while in DEBUG mode
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Debug(object message, params object[] values)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log.PrintLog(message.ToString(), false, values);
            Console.ForegroundColor = ConsoleColor.Gray;
#endif
        }

        /// <summary>
        /// Catch unhandled exception from the application
        /// </summary>
        /// <remarks>Should be moved out of here if we do more than just log the exception.</remarks>
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("An exception has been caught");
            File.WriteAllText(_logPath + "\\MODDED_ErrorLog.Log_" + Extensions.Random.Next(100000000, 999999999) + ".txt", e.ExceptionObject.ToString());
        }

        /// <summary>
        /// Catch thread exception from the application
        /// </summary>
        /// <remarks>Should be moved out of here if we do more than just log the exception.</remarks>
        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Console.WriteLine("A thread exception has been caught");
            File.WriteAllText(_logPath + "\\MODDED_ErrorLog.Log_" + Extensions.Random.Next(100000000, 999999999) + ".txt", e.Exception.ToString());
        }

        // I'm including the following for now because they have a lot of references with different uses.
        // They should be removed since they do not provide any insight into actual problems, and other log methods should be used.

        public static void LogValueNotSpecified()
        {
            Error("<value> must be specified");
        }

        public static void LogObjectValueNotSpecified()
        {
            Error("<object> and <value> must be specified");
        }

        public static void LogValueInvalid()
        {
            Error("<value> is invalid");
        }

        public static void LogObjectInvalid()
        {
            Error("<object> is invalid");
        }

        public static void LogValueNotInt32()
        {
            Error("<value> must be a whole number (Int32)");
        }

    }
}
