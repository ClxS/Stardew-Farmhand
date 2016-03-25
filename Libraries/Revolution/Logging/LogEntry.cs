namespace Revolution.Logging
{
    public class LogEntry
    {
        public string Message { get; set; }
        public LogEntryColor Color { get; set; } = LogEntryColor.Default;
    }
}
