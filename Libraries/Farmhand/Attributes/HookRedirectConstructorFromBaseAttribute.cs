using System;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookRedirectConstructorFromBaseAttribute : Attribute
    {
        public HookRedirectConstructorFromBaseAttribute(string type, string method, params Type[] genericArguments)
        {
            Type = type;
            Method = method;
            GenericArguments = genericArguments;
        }
        
        public string Type { get; set; }
        public string Method { get; set; }
        public Type[] GenericArguments { get; set; }
    }
    
}
