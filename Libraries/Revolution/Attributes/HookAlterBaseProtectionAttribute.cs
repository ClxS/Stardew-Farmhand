using System;

namespace Revolution.Attributes
{
    public enum LowestProtection
    {
        Private,
        Protected,
        Public
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookAlterBaseProtectionAttribute : Attribute
    {
        public HookAlterBaseProtectionAttribute(LowestProtection protection)
        {
            Protection = protection;
        }

        public LowestProtection Protection { get; set; }
    }
}
