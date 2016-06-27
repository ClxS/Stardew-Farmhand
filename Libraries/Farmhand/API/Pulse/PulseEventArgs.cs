using Farmhand.Registries.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Pulse
{
    [Obsolete("This utility is experimental and may be subject to change in a later version and will break mod compatibility.")]
    public class PulseEventArgs : EventArgs
    {
        public string PulseId;
        public IPulsableObject Data;
        public ModManifest Dispatcher;

        public PulseEventArgs(string pulseId, ModManifest dispatcher, IPulsableObject data)
        {
            PulseId = pulseId;
            Dispatcher = dispatcher;
            Data = data;
        }
    }
}
