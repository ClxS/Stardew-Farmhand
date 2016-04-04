using System;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookRedirectConstructorFromBaseAttribute : Attribute
    {
        public HookRedirectConstructorFromBaseAttribute(string type, string method)
        {
            Type = type;
            Method = method;
        }
        
        public string Type { get; set; }
        public string Method { get; set; }
    }
}
