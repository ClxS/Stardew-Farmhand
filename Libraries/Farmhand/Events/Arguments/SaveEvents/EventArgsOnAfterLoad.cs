using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StardewValley;

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
