using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Events.Arguments.MailEvents
{
    public class EventArgsOpenedMailWithAttachment : EventArgs
    {
        public EventArgsOpenedMailWithAttachment(string message, string mailTitle)
        {
            this.Message = message;
            this.MailTitle = mailTitle;
        }

        public string Message { get; set; }

        public string MailTitle { get; set; }
    }
}
