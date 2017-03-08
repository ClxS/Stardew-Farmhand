namespace Farmhand.Logging.Loggers
{
    /// <summary>
    ///     The Logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///     Writes the message to the log.
        /// </summary>
        /// <param name="logItem">
        ///     The entry to log.
        /// </param>
        /// <param name="module">
        ///     The name of the module writing this log.
        /// </param>
        void Write(LogEntry logItem, string module);
    }
}