namespace Farmhand.Events.Arguments.MailEvents
{
    using System;

    public class EventArgsOpenedMail : EventArgs
    {
        public EventArgsOpenedMail(string message, string mailTitle, string attachmentType, string attachmentValue)
        {
            this.Message = message;
            this.MailTitle = mailTitle;
            this.AttachmentType = attachmentType;
            this.AttachmentValue = attachmentValue;
        }

        public string Message { get; }

        public string MailTitle { get; }

        public string AttachmentType { get; }

        public string AttachmentValue { get; }
    }
}