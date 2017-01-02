#pragma warning disable 1591

namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Defines a common, non-returnable hook.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HookAttribute" /> class.
        /// </summary>
        /// <param name="hookType">
        ///     The hook type.
        /// </param>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        public HookAttribute(HookType hookType, string type, string method)
        {
            this.HookType = hookType;
            this.Type = type;
            this.Method = method;
        }

        /// <summary>
        ///     Gets or sets the hook type.
        /// </summary>
        public HookType HookType { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Gets or sets the method.
        /// </summary>
        public string Method { get; set; }
    }
}

#pragma warning restore 1591