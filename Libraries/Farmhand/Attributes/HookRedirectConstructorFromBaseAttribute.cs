namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Redirects a constructor from it's base class'.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HookRedirectConstructorFromBaseAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HookRedirectConstructorFromBaseAttribute" /> class.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        /// <param name="parameters">
        ///     The parameters.
        /// </param>
        /// <param name="genericArguments">
        ///     The generic arguments.
        /// </param>
        public HookRedirectConstructorFromBaseAttribute(
            string type,
            string method,
            Type[] parameters,
            params Type[] genericArguments)
        {
            this.Type = type;
            this.Method = method;
            this.Parameters = parameters;
            this.GenericArguments = genericArguments;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HookRedirectConstructorFromBaseAttribute" /> class.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        public HookRedirectConstructorFromBaseAttribute(string type, string method)
        {
            this.Type = type;
            this.Method = method;
            this.Parameters = new Type[] { };
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
        ///     Gets or sets the parameters.
        /// </summary>
        public Type[] Parameters { get; set; }

        /// <summary>
        ///     Gets or sets the generic arguments.
        /// </summary>
        public Type[] GenericArguments { get; set; }
    }
}