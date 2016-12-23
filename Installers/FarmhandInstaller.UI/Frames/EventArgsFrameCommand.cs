namespace FarmhandInstaller.UI.Frames
{
    using System;

    internal class EventArgsFrameCommand : EventArgs
    {
        public EventArgsFrameCommand(string command)
        {
            this.Command = command;
        }

        public string Command { get; set; }
    }
}
