using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Logging.Loggers
{
    public interface ILogger
    {
        void Write(LogEntry logItem);
    }
}
