namespace Farmhand.Logging
{
    /// <summary>
    /// Defines a log entry message.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Gets or sets the message of the entry.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the type of entry. Defaults to LogEntryType.Info.
        /// </summary>
        public LogEntryType Type { get; set; } = LogEntryType.Info;
    }
}