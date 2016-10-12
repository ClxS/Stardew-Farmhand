using System;

namespace Farmhand.Events.Arguments.SaveEvents
{
    public class EventArgsOnAfterLoad : EventArgs
    {
        public EventArgsOnAfterLoad(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; set; }
    }
}
