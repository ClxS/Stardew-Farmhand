using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Events.Arguments.GlobalRoute
{
    public class EventArgsGlobalRoute : EventArgs
    {
        public EventArgsGlobalRoute(string type, string method, object[] parameters)
        {
            Type = type;
            Method = method;
            Parameters = parameters;
        }

        public string Type;
        public string Method;
        public object[] Parameters;
    }
}
