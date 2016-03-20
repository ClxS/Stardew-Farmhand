using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookRedirectConstructorFromBaseAttribute : Attribute
    {
        public HookRedirectConstructorFromBaseAttribute(string type, string method)
        {
            Type = type;
            Method = method;
        }

        public HookType HookType;
        public string Type { get; set; }
        public string Method { get; set; }

    }
}
