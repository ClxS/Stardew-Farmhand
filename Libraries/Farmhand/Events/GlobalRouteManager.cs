namespace Farmhand.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.Events.Arguments.GlobalRoute;
    using Farmhand.Logging;

    /// <summary>
    ///     States which the listener is a pre listener, triggered prior to the function being executed and is cancellation, or
    ///     a post listener which passes the current returning value of the function prior to returning
    /// </summary>
    internal enum ListenerType
    {
        Pre,

        Post
    }

    internal static class GlobalRouteManager
    {
        private static List<Action<EventArgsGlobalRoute>>[] preListeners;

        private static List<Action<EventArgsGlobalRoute>>[] postListeners;

        private static readonly Dictionary<string, int> MapIndexes = new Dictionary<string, int>();

        public static int ListenedMethods { get; set; } = 0;

        public static bool IsEnabled { get; set; }

        private static List<Action<EventArgsGlobalRoute>>[] PreListeners
            => preListeners ?? (preListeners = new List<Action<EventArgsGlobalRoute>>[ListenedMethods]);

        private static List<Action<EventArgsGlobalRoute>>[] PostListeners
            => postListeners ?? (postListeners = new List<Action<EventArgsGlobalRoute>>[ListenedMethods]);

        public static void MapIndex(string type, string method, int index)
        {
            var key = $"{type}.{method}";
            if (MapIndexes.ContainsKey(key))
            {
                return;
            }

            MapIndexes[key] = index;
        }

        /// <summary>
        ///     This method is populated by the installer and contains the index mapping information for GRMable methods
        /// </summary>
        public static void InitialiseMappings()
        {
        }

        // public static void GlobalRoutePreInvoke(int index, string type, string method, params object[] @params)
        // {
        // if (!IsEnabled)
        // return;

        // if (PreListeners[index] != null)
        // {
        // var evtArgs = new EventArgsGlobalRouteManager(type, method, @params);
        // foreach (var evt in PreListeners[index])
        // {
        // evt.Invoke(evtArgs);
        // }
        // }
        // }
        public static bool GlobalRoutePreInvoke(
            int index,
            string type,
            string method,
            out object output,
            params object[] @params)
        {
            output = null;

            if (!IsEnabled)
            {
                return false;
            }

            if (PreListeners[index] == null)
            {
                return false;
            }

            var evtArgs = new EventArgsGlobalRouteReturnable(type, method, @params, null);
            foreach (var evt in PreListeners[index])
            {
                evt.Invoke(evtArgs);
            }

            output = evtArgs.Output;

            return evtArgs.Cancel;
        }

        public static void GlobalRoutePostInvoke(int index, string type, string method, params object[] @params)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (PostListeners[index] != null)
            {
                var evtArgs = new EventArgsGlobalRoute(type, method, @params);
                foreach (var evt in PostListeners[index])
                {
                    evt.Invoke(evtArgs);
                }
            }
        }

        public static void GlobalRoutePostInvoke(
            int index,
            string type,
            string method,
            ref object output,
            params object[] @params)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (PostListeners[index] != null)
            {
                var evtArgs = new EventArgsGlobalRouteReturnable(type, method, @params, output);
                foreach (var evt in PostListeners[index])
                {
                    evt.Invoke(evtArgs);
                }

                output = evtArgs.Output;
            }
        }

        /// <summary>
        ///     Returns whether any listeners are attached to this method
        /// </summary>
        /// <param name="method">Index of method</param>
        /// <returns>
        ///     Whether the method is being listened to on entry.
        /// </returns>
        public static bool IsBeingPreListenedTo(int method)
        {
            return PreListeners[method] != null;
        }

        /// <summary>
        ///     Returns whether any listeners are attached to this method
        /// </summary>
        /// <param name="method">Index of method</param>
        /// <returns>
        ///     Whether the method is being listened to on exit.
        /// </returns>
        public static bool IsBeingPostListenedTo(int method)
        {
            return PostListeners[method] != null;
        }

        /// <summary>
        ///     Attach a listener and enable the global route table
        /// </summary>
        /// <param name="listenerType">Whether this is an entry or exit event.</param>
        /// <param name="type">The type containing the method to listen for</param>
        /// <param name="method">The method to listen for</param>
        /// <param name="callback">The delegate to add</param>
        public static void Listen(
            ListenerType listenerType,
            string type,
            string method,
            Action<EventArgsGlobalRoute> callback)
        {
            var key = $"{type}.{method}";
            int index;
            if (MapIndexes.TryGetValue(key, out index))
            {
                if (listenerType == ListenerType.Pre)
                {
                    if (PreListeners[index] == null)
                    {
                        PreListeners[index] = new List<Action<EventArgsGlobalRoute>>();
                    }

                    PreListeners[index].Add(callback);
                }
                else
                {
                    if (PostListeners[index] == null)
                    {
                        PostListeners[index] = new List<Action<EventArgsGlobalRoute>>();
                    }

                    PostListeners[index].Add(callback);
                }

                IsEnabled = true;
            }
            else
            {
                if (MapIndexes.Any())
                {
                    throw new Exception("The method ({key}) is not available for listening");
                }

                Log.Warning("GRM disabled at install time. Event will not be hooked");
            }
        }

        /// <summary>
        ///     Remove an attached listener and disable the global route table if no listeners are attached
        /// </summary>
        /// <param name="type">The type containing the method to listen for</param>
        /// <param name="method">The method to listen for</param>
        /// <param name="callback">The delegate to remove. This must be the same instance used when first registering the listener</param>
        [Obsolete("Something wrong with this")]
        public static void Remove(string type, string method, Action<EventArgsGlobalRoute> callback)
        {
            // var key = $"{type}.{method}";
            // if (Listeners.ContainsKey(key))
            // {
            // if (Listeners[key] != null)
            // {
            // Listeners[key].Remove(callback);
            // if (Listeners[key].Count <= 0)
            // {
            // Listeners[key] = null;
            // }
            // }

            // if (Listeners[key] == null)
            // {
            // Listeners.Remove(key);
            // }   
            // }

            // if (Listeners.Count <= 0)
            // {
            // IsEnabled = false;
            // }
        }
    }
}