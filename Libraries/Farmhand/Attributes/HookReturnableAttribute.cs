using System;

#pragma warning disable 1591
namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookReturnableAttribute : Attribute
    {
        public HookReturnableAttribute(HookType hookType, string type, string method)
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
