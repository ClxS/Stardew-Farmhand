using Farmhand.Registries.Containers;
using System;

namespace Farmhand.Events.Arguments.ApiEvents
{
    public class EventArgsOnModLoadEvent : EventArgs
    {
        public EventArgsOnModLoadEvent(ModManifest mod)
        {
            Mod = mod;
        }

        public ModManifest Mod { get; private set; }
    }
}
