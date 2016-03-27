using System;
using System.IO;
using System.Threading;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace StardewModdingAPI
{
    [Obsolete]
    public class Log
    {
        /// <summary>
        /// Set up the logging stream
        /// </summary>
        /// <param name="logPath"></param>
        public static void Initialize(string logPath)
        {
            
        }
        
        /// <summary>
        /// Successful message to display to console and logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Success(object message, params object[] values)
        {
            Revolution.Logging.Log.Success(string.Format(message.ToString(), values));
        }

        /// <summary>
        /// Generic comment to display to console and logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Verbose(object message, params object[] values)
        {
            Revolution.Logging.Log.Verbose(string.Format(message.ToString(), values));
        }

        /// <summary>
        /// Additional comment to display to console and logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Comment(object message, params object[] values)
        {
            Revolution.Logging.Log.Info(string.Format(message.ToString(), values));
        }

        /// <summary>
        /// Message for only console. Does not appear in logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Info(object message, params object[] values)
        {
            Revolution.Logging.Log.Info(string.Format(message.ToString(), values));
        }

        /// <summary>
        /// Important message indicating an error.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Error(object message, params object[] values)
        {
            Revolution.Logging.Log.Error(string.Format(message.ToString(), values));
        }

        /// <summary>
        /// A message displayed only while in DEBUG mode
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void Debug(object message, params object[] values)
        {
            Revolution.Logging.Log.Verbose(string.Format(message.ToString(), values));
        }

        /// <summary>
        /// Catch unhandled exception from the application
        /// </summary>
        /// <remarks>Should be moved out of here if we do more than just log the exception.</remarks>
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        /// <summary>
        /// Catch thread exception from the application
        /// </summary>
        /// <remarks>Should be moved out of here if we do more than just log the exception.</remarks>
        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
        }

        // I'm including the following for now because they have a lot of references with different uses.
        // They should be removed since they do not provide any insight into actual problems, and other log methods should be used.

        public static void LogValueNotSpecified()
        {
        }

        public static void LogObjectValueNotSpecified()
        {
        }

        public static void LogValueInvalid()
        {
        }

        public static void LogObjectInvalid()
        {
        }

        public static void LogValueNotInt32()
        {
        }

    }
}
