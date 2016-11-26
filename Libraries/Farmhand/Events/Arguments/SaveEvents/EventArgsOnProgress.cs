using System;

namespace Farmhand.Events.Arguments.SaveEvents
{
    public class EventArgsOnProgress : EventArgs
    {
        public EventArgsOnProgress(int progress)
        {
            Progress = progress;
        }

        public int Progress { get; set; }
    }
    
    public class EventArgsOnAfterLoad : EventArgs
    {
        public EventArgsOnAfterLoad() 
        {
        }
    }

    public class EventArgsOnAfterSave : EventArgs
    {
        public EventArgsOnAfterSave() 
        {
        }
    }
}
