using System;

namespace StardewModdingAPI.Events
{
    public class EventArgsCommand : EventArgs
    {
        public EventArgsCommand(Command command)
        {
            Command = command;
        }
        public Command Command { get; private set; }
    }
}