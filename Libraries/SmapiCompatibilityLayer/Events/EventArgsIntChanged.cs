using System;

namespace StardewModdingAPI.Events
{
    public class EventArgsIntChanged : EventArgs
    {
        public EventArgsIntChanged(int priorInt, int newInt)
        {
            NewInt = newInt;
            PriorInt = priorInt;
        }
        public int NewInt { get; }
        public int PriorInt { get; }
    }
}