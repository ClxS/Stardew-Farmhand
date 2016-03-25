using Revolution.Events.Arguments;
using System;
using System.Reflection;

namespace Revolution.Events
{
    public static class ApiEvents
    {
        public static event EventHandler<EventArgsOnModError> OnModError = delegate { };

        internal static void InvokeOnModError(Assembly erroredAssembly, Exception ex)
        {
            EventCommon.SafeInvoke(OnModError, null, new EventArgsOnModError(erroredAssembly, ex));
        }
    }
}
