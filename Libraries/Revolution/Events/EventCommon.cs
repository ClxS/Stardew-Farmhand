using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Revolution.Events
{
    public static class EventCommon
    {
        internal static void SafeInvoke(EventHandler evt, object sender)
        {
            foreach (EventHandler @delegate in evt.GetInvocationList())
            {
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
            foreach (EventHandler<T> @delegate in evt.GetInvocationList())
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
            bool cancel = false;
            foreach (EventHandler<T> @delegate in evt.GetInvocationList())
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
