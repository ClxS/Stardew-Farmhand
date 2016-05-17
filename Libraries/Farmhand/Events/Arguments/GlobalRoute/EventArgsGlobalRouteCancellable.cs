using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Events.Arguments.GlobalRoute
{
    public class EventArgsGlobalRouteCancellable : EventArgsGlobalRoute
    {
        public EventArgsGlobalRouteCancellable(string type, string method, object[] parameters)
            : base(type, method, parameters)
        {
            Type = type;
            Method = method;
            Parameters = parameters;
            Cancel = false;
        }

        public virtual bool Cancel { get; set; }
    }
}
