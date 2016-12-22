using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Events.Arguments.Common
{
    public class EventArgsIntChanged : EventArgs
    {
        public EventArgsIntChanged(int priorValue, int newValue)
        {
            PriorValue = priorValue;
            NewValue = newValue;
        }

        public int PriorValue { get; set; }
        public int NewValue { get; set; }
    }
}
