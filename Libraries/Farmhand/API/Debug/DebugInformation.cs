using System;

namespace Farmhand.API.Debug
{
    public class DebugInformation
    {
        public Mod Owner { get; set; }
        public Func<string, string[], bool> Callback { get; set; }

        public DebugInformation(Mod owner, Func<string, string[], bool> callback)
        {
            Owner = owner;
            Callback = callback;
        }
    }
}
