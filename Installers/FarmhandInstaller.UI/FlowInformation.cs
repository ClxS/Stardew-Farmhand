namespace FarmhandInstaller.UI
{
    using System.Collections.Generic;

    using FarmhandInstaller.UI.Frames;

    internal class FlowInformation
    {
        public BaseFrame Element { get; set; }

        public Dictionary<string, BaseFrame> TransitionCommands { get; set; }
    }
}
