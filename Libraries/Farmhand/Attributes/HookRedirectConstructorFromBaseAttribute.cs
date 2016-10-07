using System;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookRedirectConstructorFromBaseAttribute : Attribute
    {
        public HookRedirectConstructorFromBaseAttribute(string type, string method, Type[] parameters, params Type[] genericArguments)
        {
            Type = type;
            Method = method;
            Parameters = parameters;
            GenericArguments = genericArguments;
        }

        public HookRedirectConstructorFromBaseAttribute(string type, string method)
        {
            Type = type;
            Method = method;
            Parameters = new Type[] { };
        }

        public string Type { get; set; }
        public string Method { get; set; }
        public Type[] Parameters { get; set; }
        public Type[] GenericArguments { get; set; }
    }
    
}
