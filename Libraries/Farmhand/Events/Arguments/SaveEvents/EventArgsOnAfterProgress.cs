using System;

namespace Farmhand.Events.Arguments.SaveEvents
{
    public class EventArgsOnAfterProgress : EventArgs
    {
        public EventArgsOnAfterProgress(int progress)
        {
            Progress = progress;
        }

        public int Progress { get; set; }
    }

    public class EventArgsOnAfterSaveProgress : EventArgsOnAfterProgress
    {
        public EventArgsOnAfterSaveProgress(int progress) : base(progress) { }
    }
    public class EventArgsOnAfterLoadProgress : EventArgsOnAfterProgress
    {
        public EventArgsOnAfterLoadProgress(string filename, int progress) : base(progress)
        {
            Filename = filename;
        }

        public string Filename { get; set; }
    }
}
