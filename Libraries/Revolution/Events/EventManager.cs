using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Revolution.Attributes;
using StardewValley;

namespace Revolution.Events
{
    public class EventManager
    {
        private static readonly PropertyWatcher watcher = new PropertyWatcher();
        readonly Dictionary<Assembly, Dictionary<EventInfo, Delegate[]>> _detachedDelegates = new Dictionary<Assembly, Dictionary<EventInfo, Delegate[]>>();

        private static IEnumerable<Type> GetRevolutionEvents()
        {
            var revolutionTypes = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => string.Equals(t.Namespace, "Revolution.Events", StringComparison.Ordinal)
                                || string.Equals(t.Namespace, "StardewModdingAPI.Events", StringComparison.Ordinal))
                    .ToList();

            foreach (var layer in ModLoader.CompatibilityLayers)
            {
                revolutionTypes.AddRange(layer.GetEventClasses());
            }

            return revolutionTypes;
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "Initialize")]
        public static void ManualHookup()
        {
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "Update")]
        public static void ManualEventChecks()
        {
            watcher.CheckForChanges();
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

            var testTypes = GetRevolutionEvents();
            foreach (var type in testTypes)
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
            if (!_detachedDelegates.ContainsKey(assembly)) return;

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

        public void AttachSmapiEvents()
        {
            
        }
    }
}
