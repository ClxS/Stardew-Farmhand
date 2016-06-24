using System;
using System.Collections.Generic;

using Farmhand.Registries.Containers;

namespace FarmHand.API
{
    public static class InterModUtils
    {
        public class Pulse
        {
            public event EventHandler<PulseEventArgs> Listeners;
            public void Invoke(Dictionary<string, object> Data)
            {
                if (Listeners.GetInvocationList().Length > 0)
                    Listeners(this, new PulseEventArgs(Data));
            }
        }
        public class PulseEventArgs : EventArgs
        {
            public Dictionary<string, object> Data;
            public PulseEventArgs(Dictionary<string, object> data)
            {
                Data = data;
            }
        }
        private static Dictionary<string, Pulse> Pulses = new Dictionary<string, Pulse>();
        public class Resource
        {
            private dynamic _Value;
            private Type _Type;
            public bool Set<T>(T value)
            {
                if (_Value != null)
                    return false;
                _Value = value;
                _Type = typeof(T);
                return true;
            }
            public dynamic Get()
            {
                return _Value;
            }
            public Type Type()
            {
                return _Type;
            }
        }
        private static Dictionary<string, Resource> Resources = new Dictionary<string, Resource>();

        private static string GetId(ModManifest mod, string pulse)
        {
            return $"\\{mod.UniqueId.ThisId}\\{pulse}";
        }
        public static Pulse GetPulse(ModManifest mod, string pulse)
        {
            string id = GetId(mod, pulse);
            if (!Pulses.ContainsKey(id))
                Pulses.Add(id, new Pulse());
            return Pulses[id];
        }
        public static Resource GetResource(string id)
        {
            if (!Resources.ContainsKey(id))
                Resources.Add(id, new Resource());
            return Resources[id];
        }
    }
}
