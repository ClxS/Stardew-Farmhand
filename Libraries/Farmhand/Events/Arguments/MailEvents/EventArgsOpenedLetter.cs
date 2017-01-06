namespace Farmhand.Events.Arguments.MailEvents
{
    using System;

    public class EventArgsOpenedLetter : EventArgs
    {
        public EventArgsOpenedLetter(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}