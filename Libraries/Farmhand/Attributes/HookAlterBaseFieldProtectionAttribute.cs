
#pragma warning disable 1591

namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Alters the protection of base class fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookAlterBaseFieldProtectionAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HookAlterBaseFieldProtectionAttribute" /> class.
        /// </summary>
        /// <param name="protection">
        ///     The protection.
        /// </param>
        public HookAlterBaseFieldProtectionAttribute(LowestProtection protection)
        {
            this.Protection = protection;
        }

        /// <summary>
        ///     Gets or sets the protection.
        /// </summary>
        public LowestProtection Protection { get; set; }
    }
}

#pragma warning restore 1591