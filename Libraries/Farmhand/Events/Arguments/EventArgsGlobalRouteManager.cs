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
            IsOutputSet = false;
        }

        public string Type;
        public string Method;
        public object[] Parameters;


        public object _output;
        public object Output
        {
            get
            {
                return _output;
            }
            set
            {
                IsOutputSet = true;
                _output = value;
            }
        }

        internal bool IsOutputSet = false;
    }
}
