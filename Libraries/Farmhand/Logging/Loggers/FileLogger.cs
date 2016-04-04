using System;
using System.IO;

namespace Farmhand.Logging.Loggers
{
    public class FileLogger : ILogger
    {
        private static string LogDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley", "ErrorLogs");

        private StreamWriter _file;
        private StreamWriter File
        {
            get
            {
                if (_file == null)
                {
                    _file = new StreamWriter(Path.Combine(LogDir, $"Farmhand-{DateTime.Now.ToLongDateString()}-log.txt"));
                    _file.AutoFlush = true;
                }
                return _file;
            }
        }

        ~FileLogger()
        {
            if (_file != null)
            {
                _file.Close();
                _file.Dispose();
                _file = null;
            }
        }

        public void Write(LogEntry logItem)
        {
            try
            {
                File.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {logItem.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR ERROR ERROR");
                throw;
            }
        }
    }
}
