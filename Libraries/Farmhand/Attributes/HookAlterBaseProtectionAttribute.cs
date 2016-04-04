using System;
#pragma warning disable 1591

namespace Farmhand.Attributes
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

#pragma warning restore 1591
