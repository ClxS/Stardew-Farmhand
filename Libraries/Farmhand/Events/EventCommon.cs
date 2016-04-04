using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Farmhand.Events
{
    public static class EventCommon
    {
        /// <summary>
        /// Safely invokes an event and reports when mods throw exceptions. This overload handles ordinary EventHandler events
        /// </summary>
        /// <param name="evt">Event to throw</param>
        /// <param name="sender">Sender to pass to event</param>
        public static void SafeInvoke(EventHandler evt, object sender)
        {
            foreach (var @delegate in evt.GetInvocationList().Cast<EventHandler>())
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

        /// <summary>
        /// Safely invokes an event and reports when mods throw exceptions. This overload handles delegates with custom arguments
        /// </summary>
        /// <typeparam name="T">Type of event arguments. Must inherit from EventArgs</typeparam>
        /// <param name="evt">Event to throw</param>
        /// <param name="sender">Sender to pass to event</param>
        /// <param name="args">Arguments to pass to event</param>
        public static void SafeInvoke<T>(EventHandler<T> evt, object sender, T args) where T : EventArgs
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

        /// <summary>
        /// Safely invokes an event and reports when mods throw exceptions. This overload handles delegates with custom arguments
        /// </summary>
        /// <typeparam name="T">Type of event arguments. Must inherit from CancelEventArgs</typeparam>
        /// <param name="evt">Event to throw</param>
        /// <param name="sender">Sender to pass to event</param>
        /// <param name="args">Arguments to pass to event</param>
        /// <returns>True when event was canceled by a delegate</returns>
        public static bool SafeCancellableInvoke<T>(EventHandler<T> evt, object sender, T args) where T : CancelEventArgs
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
