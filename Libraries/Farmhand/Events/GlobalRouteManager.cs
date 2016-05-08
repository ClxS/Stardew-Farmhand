using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmhand.Events.Arguments;
using Farmhand.Logging;

namespace Farmhand.Events
{
    /// <summary>
    /// States which the listener is a pre listener, triggered prior to the function being executed and is cancellation, or
    /// a post listener which passes the current returning value of the function prior to returning
    /// </summary>
    public enum ListenerType
    {
        Pre,
        Post
    }

    public static class GlobalRouteManager
    {
        public static int ListenedMethods = 0;
                
        //IsEnabled could be a property which returns Listeners.Any or Listeners.Count > 0 but it's being accessed potentailly thousands of times per frame.
        public static bool IsEnabled = false;

        private static List<Action<EventArgsGlobalRouteManager>>[] _preListeners;        
        private static List<Action<EventArgsGlobalRouteManager>>[] PreListeners
        {
            get
            {
                if (_preListeners == null)
                {
                    _preListeners = new List<Action<EventArgsGlobalRouteManager>>[ListenedMethods];
                }
                return _preListeners;
            }
        }

        private static List<Action<EventArgsGlobalRouteManager>>[] _postListeners;
        private static List<Action<EventArgsGlobalRouteManager>>[] PostListeners
        {
            get
            {
                if (_postListeners == null)
                {
                    _postListeners = new List<Action<EventArgsGlobalRouteManager>>[ListenedMethods];
                }
                return _postListeners;
            }
        }

        private static readonly Dictionary<string, int> MapIndexes = new Dictionary<string, int>();

        public static void MapIndex(string type, string method, int index)
        {
            var key = $"{type}.{method}";
            if (MapIndexes.ContainsKey(key))
                return;

            MapIndexes[key] = index;
        }

        /// <summary>
        /// This method is populated by the installer and contains the index mapping information for GRMable methods
        /// </summary>
        public static void InitialiseMappings()
        {
        }
        
        public static bool GlobalRoutePreInvoke(int index, string type, string method, out object output, params object[] @params) //TODO Add once implemented
        {
            output = null;

            if (!IsEnabled)
                return false;

            Logging.Log.Success($"{index} - {type} - {method}");            

            if (PreListeners[index] != null)
            {
                var evtArgs = new EventArgsGlobalRouteManager(type, method, @params, output);
                foreach (var evt in PreListeners[index])
                {
                    evt.Invoke(evtArgs);
                }

                output = evtArgs.Output;

                return evtArgs.IsOutputSet;
            }
            return false;
        }
        
        /// <summary>
        /// Returns whether any listeners are attached to this method
        /// </summary>
        /// <param name="method">Index of method</param>
        /// <returns></returns>
        public static bool IsBeingPreListenedTo(int method)
        {
            return PreListeners[method] != null;
        }

        /// <summary>
        /// Returns whether any listeners are attached to this method
        /// </summary>
        /// <param name="method">Index of method</param>
        /// <returns></returns>
        public static bool IsBeingPostListenedTo(int method)
        {
            return PostListeners[method] != null;
        }
        
        /// <summary>
        /// Attach a listener and enable the global route table
        /// </summary>
        /// <param name="listenerType"></param>
        /// <param name="type">The type containing the method to listen for</param>
        /// <param name="method">The method to listen for</param>
        /// <param name="callback">The delegate to add</param>
        public static void Listen(ListenerType listenerType, string type, string method, Action<EventArgsGlobalRouteManager> callback)
        {
            var key = $"{type}.{method}";
            int index;
            if (MapIndexes.TryGetValue(key, out index))
            {
                if (listenerType == ListenerType.Pre)
                {
                    if (PreListeners[index] == null)
                        PreListeners[index] = new List<Action<EventArgsGlobalRouteManager>>();
                    PreListeners[index].Add(callback);
                }
                else
                {
                    if (PostListeners[index] == null)
                        PostListeners[index] = new List<Action<EventArgsGlobalRouteManager>>();
                    PostListeners[index].Add(callback);
                }

                IsEnabled = true;
            }
            else
            {
                throw new Exception("The method ({key}) is not available for listening");
            }
            
        }

        /// <summary>
        /// Remove an attached listener and disable the global route table if no listeners are attached
        /// </summary>
        /// <param name="type">The type containing the method to listen for</param>
        /// <param name="method">The method to listen for</param>
        /// <param name="callback">The delegate to remove. This must be the same instance used when first registering the listener</param>
        [Obsolete("Something wrong with this")]
        public static void Remove(string type, string method, Action<EventArgsGlobalRouteManager> callback)
        {
            //var key = $"{type}.{method}";
            //if (Listeners.ContainsKey(key))
            //{
            //    if (Listeners[key] != null)
            //    {
            //        Listeners[key].Remove(callback);
            //        if (Listeners[key].Count <= 0)
            //        {
            //            Listeners[key] = null;
            //        }
            //    }

            //    if (Listeners[key] == null)
            //    {
            //        Listeners.Remove(key);
            //    }   
            //}

            //if (Listeners.Count <= 0)
            //{
            //    IsEnabled = false;
            //}
        }
    }
}
