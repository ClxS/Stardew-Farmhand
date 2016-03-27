using System;

namespace StardewModdingAPI.Events
{
    public class EventArgsStringChanged : EventArgs
    {
        public EventArgsStringChanged(string priorString, string newString)
        {
            NewString = newString;
            PriorString = priorString;
        }
        public string NewString { get; private set; }
        public string PriorString { get; private set; }
    }
}