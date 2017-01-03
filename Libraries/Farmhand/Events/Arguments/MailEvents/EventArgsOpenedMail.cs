using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Events.Arguments.MailEvents
{
    public class EventArgsOpenedMail : EventArgs
    {
        public EventArgsOpenedMail(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}
