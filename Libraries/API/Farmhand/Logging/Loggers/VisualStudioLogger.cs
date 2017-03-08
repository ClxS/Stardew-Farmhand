namespace Farmhand.Logging.Loggers
{
    using System;
    using System.Diagnostics;

    /// <summary>
    ///     Writes to the VisualStudio output log via Debug.WriteLine
    /// </summary>
    public class VisualStudioLogger : ILogger
    {
        #region ILogger Members

        /// <summary>
        ///     Writes the message to the log.
        /// </summary>
        /// <param name="logItem">
        ///     The entry to log.
        /// </param>
        /// <param name="module">
        ///     The name of the module writing this log.
        /// </param>
        public void Write(LogEntry logItem, string module)
        {
            Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}][{module}] {logItem.Message}");
        }

        #endregion
    }
}