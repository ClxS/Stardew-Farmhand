using System;
#pragma warning disable 1591

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookAlterBaseFieldProtectionAttribute : Attribute
    {
        public HookAlterBaseFieldProtectionAttribute(LowestProtection protection)
        {
            Protection = protection;
        }

        public LowestProtection Protection { get; set; }
    }
}

#pragma warning restore 1591
