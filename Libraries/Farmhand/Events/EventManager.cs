using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Farmhand.Attributes;
using StardewValley;

namespace Farmhand.Events
{
    public class EventManager
    {
        public static readonly PropertyWatcher Watcher = new PropertyWatcher();
        readonly Dictionary<Assembly, Dictionary<EventInfo, Delegate[]>> _detachedDelegates = new Dictionary<Assembly, Dictionary<EventInfo, Delegate[]>>();

        private static IEnumerable<Type> GetFarmhandEvents()
        {
            var FarmhandTypes = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => string.Equals(t.Namespace, "Farmhand.Events", StringComparison.Ordinal)
                                || string.Equals(t.Namespace, "StardewModdingAPI.Events", StringComparison.Ordinal))
                    .ToList();

            foreach (var layer in ModLoader.CompatibilityLayers)
            {
                FarmhandTypes.AddRange(layer.GetEventClasses());
            }

            return FarmhandTypes;
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "Initialize")]
        public static void ManualHookup()
        {
            SaveGame.farmerSerializer.UnknownElement += FarmerSerializer_UnknownElement;
            SaveGame.farmerSerializer.UnknownAttribute += FarmerSerializer_UnknownAttribute;
            SaveGame.farmerSerializer.UnknownNode += FarmerSerializer_UnknownNode;
            SaveGame.farmerSerializer.UnreferencedObject += FarmerSerializer_UnreferencedObject;
            SaveGame.serializer.UnknownElement += FarmerSerializer_UnknownElement;
            SaveGame.serializer.UnknownAttribute += FarmerSerializer_UnknownAttribute;
            SaveGame.serializer.UnknownNode += FarmerSerializer_UnknownNode;
            SaveGame.serializer.UnreferencedObject += FarmerSerializer_UnreferencedObject;
            SaveGame.locationSerializer.UnknownElement += FarmerSerializer_UnknownElement;
            SaveGame.locationSerializer.UnknownAttribute += FarmerSerializer_UnknownAttribute;
            SaveGame.locationSerializer.UnknownNode += FarmerSerializer_UnknownNode;
            SaveGame.locationSerializer.UnreferencedObject += FarmerSerializer_UnreferencedObject;

            Farmhand.Events.SaveEvents.OnAfterLoad += Farmhand.Events.PropertyWatcher.LoadFired;
            //Farmhand.Events.PlayerEvents.OnFarmerChanged += Farmhand.API.Items.Item.FixupItemIds;
        }

        private static void FarmerSerializer_UnreferencedObject(object sender, System.Xml.Serialization.UnreferencedObjectEventArgs e)
        {
            SerializerEvents.OnUnreferencedObject(sender, e);
        }

        private static void FarmerSerializer_UnknownNode(object sender, System.Xml.Serialization.XmlNodeEventArgs e)
        {
            SerializerEvents.OnUnknownNode(sender, e);
        }

        private static void FarmerSerializer_UnknownAttribute(object sender, System.Xml.Serialization.XmlAttributeEventArgs e)
        {
            SerializerEvents.OnUnknownAttribute(sender, e);
        }

        private static void FarmerSerializer_UnknownElement(object sender, System.Xml.Serialization.XmlElementEventArgs e)
        {
            SerializerEvents.OnUnknownElement(sender, e);
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "Update")]
        public static void ManualEventChecks()
        {
            Watcher.CheckForChanges();
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

            var testTypes = GetFarmhandEvents();
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
