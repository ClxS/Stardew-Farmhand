using Farmhand.Events.Arguments;
using System;
using System.Reflection;

namespace Farmhand.Events
{
    public static class ApiEvents
    {
        /// <summary>
        /// Triggered when a mod throws an unhandled exception
        /// </summary>
        public static event EventHandler<EventArgsOnModError> OnModError = delegate { };

        internal static void InvokeOnModError(Assembly erroredAssembly, Exception ex)
        {
            EventCommon.SafeInvoke(OnModError, null, new EventArgsOnModError(erroredAssembly, ex));
        }
    }
}
