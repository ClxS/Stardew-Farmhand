using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using StardewValley;

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
