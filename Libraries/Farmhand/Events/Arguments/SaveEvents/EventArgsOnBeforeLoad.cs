using System.ComponentModel;

namespace Farmhand.Events.Arguments.SaveEvents
{
    public class EventArgsOnBeforeLoad : CancelEventArgs
    {
        public EventArgsOnBeforeLoad(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; set; }
    }
}
