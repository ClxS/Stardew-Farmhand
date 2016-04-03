using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Revolution.Events.Arguments;
using Revolution.Logging;

namespace Revolution.Events
{
    //TODO This can have it's performance improved by quite a bit. Important considering the GlobalRouteInvoke will be called upwards of 100 times PER FRAME on heavily used
    //methods like draw. (Which modders should never, ever do. Modders if you're reading this - please ask me! Always happy to add an event. This class' sole purpose is to 
    //provide an alternative until I get around to implementing your requests.)
    public static class GlobalRouteManager
    {
        //IsEnabled could be a property which returns Listeners.Any or Listeners.Count > 0 but it's being accessed potentailly thousands of times per frame.
        public static bool IsEnabled = false;

        private static readonly Dictionary<string, List<Action<EventArgsGlobalRouteManager>>> Listeners = new Dictionary<string, List<Action<EventArgsGlobalRouteManager>>>();

        //public static void GlobalRouteInvoke(string type, string method, out object output, params object[] @parans) //TODO Add once implemented
        //public static void GlobalRouteInvoke(string type, string method, out object output, params object[] @params) //TODO Add once implemented
        public static void GlobalRouteInvoke(string type, string method)
        {
            if (!IsEnabled)
                return;
            
            var key = $"{type}.{method}";
            if (!Listeners.ContainsKey(key))
                return;

            var evtCallbacks = Listeners[key];
            if (evtCallbacks == null)
                return;

            var evtArgs = new EventArgsGlobalRouteManager(type, method, null, null);
            foreach (var evt in evtCallbacks)
            {
                evt.Invoke(evtArgs);
            }
        }

        /// <summary>
        /// Attach a listener and enable the global route table
        /// </summary>
        /// <param name="type">The type containing the method to listen for</param>
        /// <param name="method">The method to listen for</param>
        /// <param name="callback">The delegate to add</param>
        [Obsolete("This method will currently not work")]
        public static void Listen(string type, string method, Action<EventArgsGlobalRouteManager> callback)
        {
            return;
            /*var key = $"{type}.{method}";
            if (!Listeners.ContainsKey(key) || Listeners[key] == null)
                Listeners[key] = new List<Action<EventArgsGlobalRouteManager>>();

            Listeners[key].Add(callback);
            IsEnabled = true;*/
        }

        /// <summary>
        /// Remove an attached listener and disable the global route table if no listeners are attached
        /// </summary>
        /// <param name="type">The type containing the method to listen for</param>
        /// <param name="method">The method to listen for</param>
        /// <param name="callback">The delegate to remove. This must be the same instance used when first registering the listener</param>
        [Obsolete("This method will currently not work")]
        public static void Remove(string type, string method, Action<EventArgsGlobalRouteManager> callback)
        {
            return;
            /*var key = $"{type}.{method}";
            if (Listeners.ContainsKey(key))
            {
                if (Listeners[key] != null)
                {
                    Listeners[key].Remove(callback);
                    if (Listeners[key].Count <= 0)
                    {
                        Listeners[key] = null;
                    }
                }

                if (Listeners[key] == null)
                {
                    Listeners.Remove(key);
                }   
            }

            if (Listeners.Count <= 0)
            {
                IsEnabled = false;
            }*/
        }
    }
}
