using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Revolution.Events
{
    public static class EventCommon
    {
        internal static void SafeInvoke(EventHandler evt, object sender)
        {
            foreach (var delegate1 in evt.GetInvocationList())
            {
                var @delegate = (EventHandler) delegate1;
                try
                {
                    @delegate.Invoke(sender, EventArgs.Empty);
                }
                catch (Exception e)
                {
                    Assembly causingAssembly = @delegate.Method?.DeclaringType?.Assembly;
                    ApiEvents.InvokeOnModError(causingAssembly, e);
                }
            }
        }

        internal static void SafeInvoke<T>(EventHandler<T> evt, object sender, T args) where T : EventArgs
        {
            foreach (var @delegate in evt.GetInvocationList().Cast<EventHandler<T>>())
            {
                try
                {
                    @delegate.Invoke(sender, args);
                }
                catch (Exception e)
                {
                    Assembly causingAssembly = @delegate.Method?.DeclaringType?.Assembly;
                    ApiEvents.InvokeOnModError(causingAssembly, e);
                }
            }
        }

        internal static bool SafeCancellableInvoke<T>(EventHandler<T> evt, object sender, T args) where T : CancelEventArgs
        {
            var cancel = false;
            foreach (var @delegate in evt.GetInvocationList().Cast<EventHandler<T>>())
            {
                try
                {
                    @delegate.Invoke(sender, args);
                    cancel = cancel || args.Cancel;
                }
                catch (Exception e)
                {
                    Assembly causingAssembly = @delegate.Method?.DeclaringType?.Assembly;
                    ApiEvents.InvokeOnModError(causingAssembly, e);
                }
            }
            return cancel;
        }
    }
}
