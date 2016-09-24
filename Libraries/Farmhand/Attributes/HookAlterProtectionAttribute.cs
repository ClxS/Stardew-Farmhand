using System;
#pragma warning disable 1591

namespace Farmhand.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookAlterProtectionAttribute : Attribute
    {
        public HookAlterProtectionAttribute(LowestProtection protection, string className)
        {
            Protection = protection;
            ClassName = className;
        }

        public LowestProtection Protection { get; set; }
        public string ClassName { get; set; }
    }
}

#pragma warning restore 1591
