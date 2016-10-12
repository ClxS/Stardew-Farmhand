using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookRedirectConstructorToMethodAttribute : Attribute
    {
        public HookRedirectConstructorToMethodAttribute(string type, string method, params Type[] genericArguments)
        {
            Type = type;
            Method = method;
            GenericArguments = genericArguments;
        }

        public HookRedirectConstructorToMethodAttribute(string type, string method)
        {
            Type = type;
            Method = method;
        }

        public string Type { get; set; }
        public string Method { get; set; }
        public Type[] GenericArguments { get; set; }
    }
}
