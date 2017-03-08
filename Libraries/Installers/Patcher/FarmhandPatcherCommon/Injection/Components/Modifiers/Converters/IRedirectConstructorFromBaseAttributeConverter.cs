namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters
{
    using System;

    /// <summary>
    ///     The RedirectConstructorFromBaseAttributeConverter interface.
    /// </summary>
    public interface IRedirectConstructorFromBaseAttributeConverter : IAttributeConverter
    {
        /// <summary>
        ///     Gets the type.
        /// </summary>
        string Type { get; }

        /// <summary>
        ///     Gets the method.
        /// </summary>
        string Method { get; }

        /// <summary>
        ///     Gets the parameters.
        /// </summary>
        Type[] Parameters { get; }

        /// <summary>
        ///     Gets the generic arguments.
        /// </summary>
        Type[] GenericArguments { get; }
    }
}