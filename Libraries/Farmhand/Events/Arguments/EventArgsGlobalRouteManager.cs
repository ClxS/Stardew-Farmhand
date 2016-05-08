using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Farmhand.Events.Arguments
{
    public class EventArgsGlobalRouteManager : CancelEventArgs
    {
        public EventArgsGlobalRouteManager(string type, string method, object[] parameters, object output = null)
        {
            Type = type;
            Method = method;
            Parameters = parameters;
            Output = output;
            DoesMethodReturnValue = output != null;
        }

        public string Type;
        public string Method;
        public object[] Parameters;
        public bool DoesMethodReturnValue;

        public object _output;
        public object Output
        {
            get
            {
                return _output;
            }
            set
            {
                Cancel = true;
                _output = value;
            }
        }
    }
}
