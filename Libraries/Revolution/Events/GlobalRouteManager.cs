using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Revolution.Logging;

namespace Revolution.Events
{
    public static class GlobalRouteManager
    {
        public static bool IsEnabled = false;

        //public static void GlobalRouteInvoke(string type, string method, out object output, params object[] @parans)
        public static bool GlobalRouteInvoke(string type, string method, out object output, params object[] @params)
        {
            output = null;
            return false;
        }
    }
}
