namespace Farmhand.Installers
{
    using System.Collections.Generic;

    using Farmhand.Installers.Frames;

    internal class FlowInformation
    {
        public BaseFrame Element { get; set; }

        public Dictionary<string, BaseFrame> TransitionCommands { get; set; }
    }
}
