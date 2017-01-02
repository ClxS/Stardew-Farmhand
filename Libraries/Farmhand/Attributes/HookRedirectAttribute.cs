namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Redirects a method (unused).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookRedirectAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HookRedirectAttribute" /> class.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        public HookRedirectAttribute(string type, string method)
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