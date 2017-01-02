namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Alters call instructions to call virtual.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookMakeBaseVirtualCallAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HookMakeBaseVirtualCallAttribute" /> class.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        public HookMakeBaseVirtualCallAttribute(string type, string method)
        {
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