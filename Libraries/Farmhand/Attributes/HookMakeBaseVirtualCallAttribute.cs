using System;

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookMakeBaseVirtualCallAttribute : Attribute
    {
        public HookMakeBaseVirtualCallAttribute(string type, string method)
        {
            Type = type;
            Method = method;
        }

        public string Type { get; set; }
        public string Method { get; set; }
    }
}
