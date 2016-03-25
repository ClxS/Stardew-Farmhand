using System;

namespace Revolution.Attributes
{
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
