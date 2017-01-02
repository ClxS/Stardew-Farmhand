
#pragma warning disable 1591

namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Creates a returnable hook.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookReturnableAttribute : Attribute
    {
        /// <summary>
        ///     The hook type.
        /// </summary>
        public HookType HookType;

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