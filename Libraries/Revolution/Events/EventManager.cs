using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Revolution.Events
{
    public class EventManager
    {
        Dictionary<Assembly, Dictionary<EventInfo, Delegate[]>> DetachedDelegates = new Dictionary<Assembly, Dictionary<EventInfo, Delegate[]>>();

        private Type[] GetRevolutionEvents()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "Revolution.Events", StringComparison.Ordinal)).ToArray();
        }

        public void DetachDelegates(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new Exception("Assembly cannot be null");
            }

            if (DetachedDelegates.ContainsKey(assembly))
            {
                throw new Exception("Detached delegates already exist for this assembly");
            }


            Dictionary<EventInfo, Delegate[]> detachedDelegates = new Dictionary<EventInfo, Delegate[]>();
            foreach (var type in GetRevolutionEvents())
            {
                foreach (var evt in type.GetEvents())
                {
                    FieldInfo fi = type.GetField(evt.Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                    Delegate del = (Delegate)fi.GetValue(null);
                    var delegates = del.GetInvocationList().Where(n => n.Method.DeclaringType != null && n.Method.DeclaringType.Assembly == assembly).ToArray();
                    detachedDelegates[evt] = delegates;

                    foreach (var @delegate in delegates)
                    {
                        evt.RemoveEventHandler(@delegate.Target, @delegate);
                    }
                }
            }

            DetachedDelegates[assembly] = detachedDelegates;
        }

        public void ReattachDelegates(Assembly assembly)
        {
            if (DetachedDelegates.ContainsKey(assembly))
            {
                var delegates = DetachedDelegates[assembly];

                foreach (var delegateEvent in delegates)
                {
                    var evt = delegateEvent.Key;
                    foreach (var @delegate in delegateEvent.Value)
                    {
                        evt.AddEventHandler(@delegate.Target, @delegate);
                    }
                }

                DetachedDelegates.Remove(assembly);
            }
        }
    }
}
