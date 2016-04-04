namespace Farmhand.Logging
{
    public class LogEntry
    {
        public string Message { get; set; }
        public LogEntryType Type { get; set; } = LogEntryType.Info;
    }
}
