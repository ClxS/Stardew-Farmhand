using System;
using System.Reflection;

namespace Farmhand.Events.Arguments
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
