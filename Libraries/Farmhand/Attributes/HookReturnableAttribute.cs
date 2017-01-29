
#pragma warning disable 1591

namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Creates a returnable hook.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookReturnableAttribute : FarmhandHook
    {
        /// <summary>
        ///     The hook type.
        /// </summary>
        public HookType HookType;

        /// <summary>
        /// Initializes a new instance of the <see cref="HookReturnableAttribute"/> class.
        /// </summary>
        /// <param name="hookType">
        /// The hook type.
        /// </param>
        /// <param name="type">
        /// The type name to hook into.
        /// </param>
        /// <param name="method">
        /// The method name to hook into.
        /// </param>
        public HookReturnableAttribute(HookType hookType, string type, string method)
        {
            this.HookType = hookType;
            this.Type = type;
            this.Method = method;
        }

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