namespace Farmhand.Logging.Loggers
{
    using System;
    using System.IO;

    /// <summary>
    ///     Writes to a log file in %AppData%/StardewValley/ErrorLogs/Farmhand-$Date$-log.txt
    /// </summary>
    public class FileLogger : ILogger
    {
        private StreamWriter file;

        private static string LogDir
            =>
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "StardewValley",
                    "ErrorLogs");

        private StreamWriter File
        {
            get
            {
                if (this.file != null)
                {
                    return this.file;
                }

                this.file = new StreamWriter(
                    Path.Combine(LogDir, $"Farmhand-{DateTime.Now.ToLongDateString()}-log.txt"));
                this.file.AutoFlush = true;

                return this.file;
            }
        }

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
            try
            {
                this.File.WriteLine($"[{DateTime.Now.ToLongTimeString()}][{module}] {logItem.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR ERROR ERROR");
                throw;
            }
        }

        #endregion

        ~FileLogger()
        {
            if (this.file != null)
            {
                this.file.Close();
                this.file.Dispose();
                this.file = null;
            }
        }
    }
}