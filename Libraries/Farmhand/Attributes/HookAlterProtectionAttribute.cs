#pragma warning disable 1591

namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Alters the protection of the provided class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookAlterProtectionAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HookAlterProtectionAttribute" /> class.
        /// </summary>
        /// <param name="protection">
        ///     The protection.
        /// </param>
        /// <param name="className">
        ///     The class name.
        /// </param>
        public HookAlterProtectionAttribute(LowestProtection protection, string className)
        {
            this.Protection = protection;
            this.ClassName = className;
        }

        /// <summary>
        ///     Gets or sets the protection.
        /// </summary>
        public LowestProtection Protection { get; set; }

        /// <summary>
        ///     Gets or sets the class name.
        /// </summary>
        public string ClassName { get; set; }
    }
}

#pragma warning restore 1591