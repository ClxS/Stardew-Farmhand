using System;

#pragma warning disable 1591
namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookAttribute : Attribute
    {
        public HookAttribute(HookType hookType, string type, string method)
        {
            HookType = hookType;
            Type = type;
            Method = method;
        }

        public HookType HookType;
        public string Type { get; set; }
        public string Method { get; set; }

    }
}
#pragma warning restore 1591
