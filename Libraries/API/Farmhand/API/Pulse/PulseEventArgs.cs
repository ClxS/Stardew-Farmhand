namespace Farmhand.API.Pulse
{
    using System;

    using Farmhand.Registries.Containers;

    [Obsolete(
        "This utility is experimental and may be subject to change in a later version and will break mod compatibility."
    )]
    public class PulseEventArgs : EventArgs
    {
        public IPulsableObject Data;

        public ModManifest Dispatcher;

        public string PulseId;

        public PulseEventArgs(string pulseId, ModManifest dispatcher, IPulsableObject data)
        {
            this.PulseId = pulseId;
            this.Dispatcher = dispatcher;
            this.Data = data;
        }
    }
}