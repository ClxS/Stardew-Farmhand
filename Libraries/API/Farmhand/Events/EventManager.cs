namespace Farmhand.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    using Farmhand.Attributes;
    using Farmhand.Extensibility;

    using Microsoft.Xna.Framework;

    using StardewValley;

    internal class EventManager
    {
        internal static readonly PropertyWatcher Watcher = new PropertyWatcher();

        private readonly Dictionary<Assembly, Dictionary<EventInfo, Delegate[]>> detachedDelegates =
            new Dictionary<Assembly, Dictionary<EventInfo, Delegate[]>>();

        private static IEnumerable<Type> GetFarmhandEvents()
        {
            var farmhandTypes =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => string.Equals(t.Namespace, "Farmhand.Events", StringComparison.Ordinal))
                    .ToList();

            foreach (var layer in ExtensibilityManager.Extensions)
            {
                farmhandTypes.AddRange(layer.GetEventClasses());
            }

            return farmhandTypes;
        }

#if XNA
        [Hook(HookType.Entry, "StardewValley.Game1", "Initialize")]
#endif
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

            // Farmhand.Events.PlayerEvents.OnFarmerChanged += Farmhand.API.Items.Item.FixupItemIds;
        }

        private static void FarmerSerializer_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            SerializerEvents.OnUnreferencedObject(sender, e);
        }

        private static void FarmerSerializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            SerializerEvents.OnUnknownNode(sender, e);
        }

        private static void FarmerSerializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            SerializerEvents.OnUnknownAttribute(sender, e);
        }

        private static void FarmerSerializer_UnknownElement(object sender, XmlElementEventArgs e)
        {
            SerializerEvents.OnUnknownElement(sender, e);
        }

        [Hook(HookType.Entry, "StardewValley.Game1", "Update")]
        public static void ManualEventChecks([InputBind(typeof(GameTime), "gameTime")] GameTime gameTime)
        {
            Watcher.CheckForChanges(gameTime);
        }

        public void DetachDelegates(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new Exception("Assembly cannot be null");
            }

            if (this.detachedDelegates.ContainsKey(assembly))
            {
                throw new Exception("Detached delegates already exist for this assembly");
            }

            var localDetachedDelegates = new Dictionary<EventInfo, Delegate[]>();

            var testTypes = GetFarmhandEvents();
            const BindingFlags Flags =
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public
                | BindingFlags.FlattenHierarchy;
            foreach (var type in testTypes)
            {
                foreach (var evt in type.GetEvents())
                {
                    var fi = type.GetField(evt.Name, Flags);
                    if (fi == null)
                    {
                        continue;
                    }

                    var del = (Delegate)fi.GetValue(null);
                    var delegates =
                        del.GetInvocationList()
                            .Where(n => n.Method.DeclaringType != null && n.Method.DeclaringType.Assembly == assembly)
                            .ToArray();
                    localDetachedDelegates[evt] = delegates;

                    foreach (var @delegate in delegates)
                    {
                        evt.RemoveEventHandler(@delegate.Target, @delegate);
                    }
                }
            }

            this.detachedDelegates[assembly] = localDetachedDelegates;
        }

        public void ReattachDelegates(Assembly assembly)
        {
            if (!this.detachedDelegates.ContainsKey(assembly))
            {
                return;
            }

            var delegates = this.detachedDelegates[assembly];

            foreach (var delegateEvent in delegates)
            {
                var evt = delegateEvent.Key;
                foreach (var @delegate in delegateEvent.Value)
                {
                    evt.AddEventHandler(@delegate.Target, @delegate);
                }
            }

            this.detachedDelegates.Remove(assembly);
        }
    }
}