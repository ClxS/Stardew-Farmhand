using Farmhand.Registries.Containers;
using System;

namespace Farmhand.API.Pulse
{
    [Obsolete("This utility is experimental and may be subject to change in a later version and will break mod compatibility.")]
    public static class Pulse
    {
        public static event EventHandler<PulseEventArgs> Listeners;
        
        public static void Invoke(string pulseId, IPulsableObject data, ModManifest dispatchingMod)
        {
            if (Listeners.GetInvocationList().Length > 0)
                Listeners(null, new PulseEventArgs(pulseId, dispatchingMod, data));
        }
    }
}
