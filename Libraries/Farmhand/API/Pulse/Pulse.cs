using Farmhand.Registries.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Pulse
{
    [Obsolete("This utility is experimental and may be subject to change in a later version - breaking mod compatibility.")]
    public class Pulse
    {
        private static Dictionary<string, Pulse> Pulses = new Dictionary<string, Pulse>();
        private static Dictionary<string, Resource> Resources = new Dictionary<string, Resource>();

        public event EventHandler<PulseEventArgs> Listeners;
        
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

        public void Invoke(Dictionary<string, object> Data)
        {
            if (Listeners.GetInvocationList().Length > 0)
                Listeners(this, new PulseEventArgs(Data));
        }

        private static string GetId(ModManifest mod, string pulse)
        {
            return $"\\{mod.UniqueId.ThisId}\\{pulse}";
        }
    }
}
