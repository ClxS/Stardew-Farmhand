using System;

namespace Revolution.Attributes
{
    public enum HookType
    {
        Entry,
        Exit
    }

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
