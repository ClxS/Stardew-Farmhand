namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Redirects a constructor to a method (for factory classes to use).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HookRedirectConstructorToMethodAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HookRedirectConstructorToMethodAttribute" /> class.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        /// <param name="genericArguments">
        ///     The generic arguments.
        /// </param>
        public HookRedirectConstructorToMethodAttribute(string type, string method, params Type[] genericArguments)
        {
            this.Type = type;
            this.Method = method;
            this.GenericArguments = genericArguments;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HookRedirectConstructorToMethodAttribute" /> class.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        public HookRedirectConstructorToMethodAttribute(string type, string method)
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

        /// <summary>
        ///     Gets or sets the generic arguments.
        /// </summary>
        public Type[] GenericArguments { get; set; }
    }
}