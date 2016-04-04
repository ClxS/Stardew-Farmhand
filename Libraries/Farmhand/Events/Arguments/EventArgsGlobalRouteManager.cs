using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Farmhand.Events.Arguments
{
    public class EventArgsGlobalRouteManager : CancelEventArgs
    {
        public EventArgsGlobalRouteManager(string type, string method, object[] parameters, object output)
        {
            Type = type;
            Method = method;
            Parameters = parameters;
            Output = output;
        }

        public string Type;
        public string Method;
        public object[] Parameters; //TODO - Implement
        public object Output; //TODO - Implement
    }
}
