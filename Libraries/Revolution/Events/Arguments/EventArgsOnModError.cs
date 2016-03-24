using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Revolution.Events.Arguments
{
    public class EventArgsOnModError : EventArgs
    {
        public EventArgsOnModError(Assembly assembly, Exception ex)
        {
            Assembly = assembly;
            Exception = ex;
        }

        public Assembly Assembly { get; private set; }
        public Exception Exception { get; private set; }
    }
}
