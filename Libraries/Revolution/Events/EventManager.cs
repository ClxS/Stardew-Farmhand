using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Revolution.Events
{
    public class EventManager
    {
        readonly Dictionary<Assembly, Dictionary<EventInfo, Delegate[]>> _detachedDelegates = new Dictionary<Assembly, Dictionary<EventInfo, Delegate[]>>();

        private IEnumerable<Type> GetRevolutionEvents()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "Revolution.Events", StringComparison.Ordinal)).ToArray();
        }

        public void DetachDelegates(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new Exception("Assembly cannot be null");
            }

            if (_detachedDelegates.ContainsKey(assembly))
            {
                throw new Exception("Detached delegates already exist for this assembly");
            }


            Dictionary<EventInfo, Delegate[]> detachedDelegates = new Dictionary<EventInfo, Delegate[]>();
            foreach (var type in GetRevolutionEvents())
            {
                foreach (var evt in type.GetEvents())
                {
                    var fi = type.GetField(evt.Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                    if (fi == null) continue;

                    var del = (Delegate)fi.GetValue(null);
                    var delegates = del.GetInvocationList().Where(n => n.Method.DeclaringType != null && n.Method.DeclaringType.Assembly == assembly).ToArray();
                    detachedDelegates[evt] = delegates;

                    foreach (var @delegate in delegates)
                    {
                        evt.RemoveEventHandler(@delegate.Target, @delegate);
                    }
                }
            }

            _detachedDelegates[assembly] = detachedDelegates;
        }

        public void ReattachDelegates(Assembly assembly)
        {
            if (_detachedDelegates.ContainsKey(assembly))
            {
                var delegates = _detachedDelegates[assembly];

                foreach (var delegateEvent in delegates)
                {
                    var evt = delegateEvent.Key;
                    foreach (var @delegate in delegateEvent.Value)
                    {
                        evt.AddEventHandler(@delegate.Target, @delegate);
                    }
                }

                _detachedDelegates.Remove(assembly);
            }
        }
    }
}
